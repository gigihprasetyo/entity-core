using Microsoft.Extensions.Logging;
using qcs_product.API.DataProviders;
using qcs_product.API.BindingModels;
using qcs_product.API.ViewModels;
using qcs_product.API.Models;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.SettingModels;
using qcs_product.API.BusinessProviders;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using qcs_product.API.Infrastructure;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using qcs_product.API.WorkflowModels;
using qcs_product.API.Helpers;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class ReviewBusinessProvider : IReviewBusinessProvider
    {
        private readonly IBaseApiBioServiceBusinessProviders _businessProviderBaseBioHR;
        private readonly IAuthenticatedUserBiohrDataProviders _dataProviderAuthBioHR;
        private readonly IReviewDataProvider _reviewDataProvider;
        private readonly IWorkflowQcSamplingDataProvider _workflowQcSamplingDataProvider;
        private readonly IQcSamplingDataProvider _qcSamplingDataProvider;
        private readonly IOptions<BioHRServiceSetting> _BioHRServiceSetting;
        private readonly IOptions<WorkflowServiceSetting> _WorkflowServiceSetting;
        private readonly IHttpClientFactory _clientFactory;
        private readonly QcsProductContext _context;
        private readonly IBioHRIntegrationBussinesProviders _bioHRIntegrationBussinesProviders;
        private readonly IDigitalSignatureDataProvider _digitalSignatureDataProvider;
        public ReviewBusinessProvider(
            IBaseApiBioServiceBusinessProviders businessProviderBaseBioHR,
            IAuthenticatedUserBiohrDataProviders dataProviderAuthBioHR,
            IOptions<BioHRServiceSetting> bioHRServiceSetting,
            IHttpClientFactory clientFactory,
            QcsProductContext context,
            IOptions<WorkflowServiceSetting> workflowServiceSetting,
            IBioHRIntegrationBussinesProviders bioHRIntegrationBussinesProviders,
            IReviewDataProvider reviewDataProvider,
            IWorkflowQcSamplingDataProvider workflowQcSamplingDataProvider,
            IQcSamplingDataProvider qcSamplingDataProvider,
            IDigitalSignatureDataProvider digitalSignatureDataProvider)
        {
            _businessProviderBaseBioHR = businessProviderBaseBioHR ?? throw new ArgumentNullException(nameof(businessProviderBaseBioHR));
            _dataProviderAuthBioHR = dataProviderAuthBioHR ?? throw new ArgumentNullException(nameof(dataProviderAuthBioHR));
            _reviewDataProvider = reviewDataProvider ?? throw new ArgumentNullException(nameof(reviewDataProvider));
            _workflowQcSamplingDataProvider = workflowQcSamplingDataProvider ?? throw new ArgumentNullException(nameof(workflowQcSamplingDataProvider));
            _qcSamplingDataProvider = qcSamplingDataProvider ?? throw new ArgumentNullException(nameof(qcSamplingDataProvider));
            _digitalSignatureDataProvider = digitalSignatureDataProvider ?? throw new ArgumentNullException(nameof(digitalSignatureDataProvider));
            _BioHRServiceSetting = bioHRServiceSetting;
            _clientFactory = clientFactory;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _WorkflowServiceSetting = workflowServiceSetting;
            _bioHRIntegrationBussinesProviders = bioHRIntegrationBussinesProviders ?? throw new ArgumentNullException(nameof(bioHRIntegrationBussinesProviders));
        }

        public async Task<ResponseViewModel<WorkflowDocumentSubmitModel>> InsertApproval(InsertApprovalBindingModel data)
        {
            ResponseViewModel<WorkflowDocumentSubmitModel> response = new ResponseViewModel<WorkflowDocumentSubmitModel>();

            var getDigitalSignature = await _digitalSignatureDataProvider.Authenticate(data.DigitalSignature, data.NIK);
            if (getDigitalSignature == false)
            {
                response.StatusCode = 403;
                response.Message = ApplicationConstant.WRONG_DIGITAL_SIGNATURE;
            }
            else if (data.Notes.Length > 200)
            {
                response.StatusCode = 403;
                response.Message = ApplicationConstant.NOTES_TO_LONG_MESSAGE;
            }
            else
            {
                var workflowQcSampling = await _workflowQcSamplingDataProvider.GetByWorkflowByQcSamplingIdIsInWorkflow(data.DataId);

                var approvalType = data.IsApprove == true ? ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME : ApplicationConstant.WORKFLOW_ACTION_REJECT_NAME;

                var workflowActionId = await _reviewDataProvider.GetWorkflowActionId(workflowQcSampling.WorkflowDocumentCode, approvalType);

                var workflowPIC = await _reviewDataProvider.GetPIC(workflowQcSampling.WorkflowDocumentCode);

                string orgId = "";
                var currentStatus = workflowPIC.CurrentStatusName;
                foreach (var item in workflowPIC.PICs)
                {
                    orgId = item.orgId;
                }

                if (workflowActionId != 0)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        List<string> nextPICOrgId = await _GetNikNextPicOrgId(currentStatus, data.DataId);

                        WorkflowDocumentSubmitModel insertedDocument = new WorkflowDocumentSubmitModel()
                        {
                            ApplicationCode = ApplicationConstant.APP_CODE,
                            Notes = data.Notes,
                            OrgId = data.NIK,
                            DocumentCode = workflowQcSampling.WorkflowDocumentCode,
                            NextPICOrgIdList = nextPICOrgId,
                            WorkflowActionId = workflowActionId
                        };

                        ResponseInsertReview submitDocument = await _reviewDataProvider.InsertReview(insertedDocument);

                        if (submitDocument.StatusCode == 200)
                        {
                            List<WorkflowDocumentSubmitModel> dataInsert = new List<WorkflowDocumentSubmitModel>() { insertedDocument };

                            //update row data by type data in table QCSamplingWorkflow and QCSampling

                            var getStatus = _getStatus(submitDocument.WorkflowStatus);
                            UpdateQcSamplingFromApproval qcSamplingFromApproval = new UpdateQcSamplingFromApproval()
                            {
                                SamplingId = data.DataId,
                                WorkflowStatus = submitDocument.WorkflowStatus,
                                Status =
                                data.IsApprove == true ? getStatus : ApplicationConstant.STATUS_REJECT,
                                UpdatedBy = data.NIK
                            };

                            await _qcSamplingDataProvider.UpdateQcSamplingDataFromApproval(qcSamplingFromApproval);

                            UpdateWorkflowQcSamplingFromApproval updateWorkflowQcSamplingFromApproval = new UpdateWorkflowQcSamplingFromApproval()
                            {
                                SamplingId = data.DataId,
                                WorkflowStatus = submitDocument.WorkflowStatus,
                                IsInWorkflow = submitDocument.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME ? false : true,
                                UpdatedBy = data.NIK,
                                RowStatus = data.IsApprove == true ? ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME : ApplicationConstant.WORKFLOW_ACTION_REJECT_NAME
                            };

                            await _workflowQcSamplingDataProvider.UpdateWorkflowQcSamplingDataFromApproval(updateWorkflowQcSamplingFromApproval);

                            //jika workflow phase satu dan approve lanjut ke phase dua 
                            if (workflowQcSampling.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_1 && data.IsApprove == true && workflowQcSampling.WorkflowDocumentCode.Substring(0, 3) == ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE)
                            {
                                InsertNextPhase insertNextPhase = new InsertNextPhase()
                                {
                                    SamplingId = workflowQcSampling.QcSamplingId,
                                    TypeSamplingId = ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE,
                                    WorkflowCode = ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2,
                                    NIK = workflowQcSampling.UpdatedBy,
                                };
                                await InsertToNextPhase(insertNextPhase);
                            }

                            transaction.Commit();

                            response.StatusCode = 200;
                            //response.Message = ApplicationConstant.SUCCESS_APPROVE_MESSAGE;
                            response.Data = dataInsert;
                        }
                        else
                        {
                            response.StatusCode = submitDocument.StatusCode;
                            response.Message = submitDocument.Message;
                        }
                    }

                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Conflict. Wrong Workflow Action Id.";
                }
            }
            return response;
        }

        public async Task<ResponseViewModel<WorkflowDocumentSubmitModel>> InsertToNextPhase(InsertNextPhase data)
        {
            ResponseViewModel<WorkflowDocumentSubmitModel> response = new ResponseViewModel<WorkflowDocumentSubmitModel>();
            var getSamplingQc = await _qcSamplingDataProvider.GetById(data.SamplingId);
            //insert workflow sampling
            WorkflowQcSampling insertQcSampling = new WorkflowQcSampling()
            {
                QcSamplingId = data.SamplingId,
                WorkflowStatus = "Draft",
                WorkflowDocumentCode = data.TypeSamplingId == ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE ? ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE + "-" + getSamplingQc.Code + "-2" : ApplicationConstant.PREFIX_EM_M_WORKFLOW_CODE + "-" + getSamplingQc.Code + "-2",
                WorkflowCode = data.WorkflowCode,
                CreatedBy = data.NIK,
                CreatedAt = DateHelper.Now(),
                UpdatedBy = data.NIK,
                UpdatedAt = DateHelper.Now()
            };

            await _workflowQcSamplingDataProvider.Insert(insertQcSampling);

            //initial new workflow document
            NewWorkflowDocument newWorkflowDocument = new NewWorkflowDocument()
            {
                DocumentCode = insertQcSampling.WorkflowDocumentCode,
                ApplicationCode = ApplicationConstant.APP_CODE,
                WorkflowCode = insertQcSampling.WorkflowCode,
                Description = ApplicationConstant.WORKFLOW_INITIAL_DESC,
                CreatedBy = data.NIK,
                CreatorOrgId = data.NIK
            };

            await _reviewDataProvider.InitialDoc(newWorkflowDocument);

            var workflowActionId = await _reviewDataProvider.GetWorkflowActionId(insertQcSampling.WorkflowDocumentCode, ApplicationConstant.WORKFLOW_ACTION_SUBMIT_NAME);
            var workflowPIC = await _reviewDataProvider.GetPIC(insertQcSampling.WorkflowDocumentCode);

            //insert document to workflow phase 2
            var nextPICOrgIdList = new List<string>();
            var organizationId = await _qcSamplingDataProvider.getOrganizatiionBySamplingId(data.SamplingId);
            var positionType = "";
            if (data.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2)
            {
                positionType = ApplicationConstant.POSITION_TYPE_KABAG;
            }
            var responseGetNik = await _bioHRIntegrationBussinesProviders.GetNikByOrganizationIdandPositionType(organizationId.ToString(), positionType);
            foreach (var item in responseGetNik.Data)
            {
                if (item.NewUserId != null && item.NewUserId != "" && item.NewUserId != "." && item.NewUserId != "-")
                {
                    nextPICOrgIdList.Add(item.NewUserId);
                }
            }

            WorkflowDocumentSubmitModel insertedDocument = new WorkflowDocumentSubmitModel()
            {
                ApplicationCode = ApplicationConstant.APP_CODE,
                Notes = "-",
                OrgId = data.NIK,
                DocumentCode = insertQcSampling.WorkflowDocumentCode,
                //jika dari phase 1 ke phase 2 dan menuju kabag menggunakan 
                NextPICOrgIdList = nextPICOrgIdList, //jika phase 2 berarti dari 
                WorkflowActionId = workflowActionId
            };

            ResponseInsertReview submitDocument = await _reviewDataProvider.InsertReview(insertedDocument);

            if (submitDocument.StatusCode == 200)
            {
                List<WorkflowDocumentSubmitModel> dataInsert = new List<WorkflowDocumentSubmitModel>() { insertedDocument };

                //update row data by type data in table QCSamplingWorkflow and QCSampling

                UpdateQcSamplingFromApproval qcSamplingFromApproval = new UpdateQcSamplingFromApproval()
                {
                    SamplingId = data.SamplingId,
                    WorkflowStatus = submitDocument.WorkflowStatus,
                    Status = ApplicationConstant.STATUS_SUBMIT, //status antara submit atau mengikuti workflow status
                    UpdatedBy = data.NIK
                };

                await _qcSamplingDataProvider.UpdateQcSamplingDataFromApproval(qcSamplingFromApproval);

                UpdateWorkflowQcSamplingFromApproval updateWorkflowQcSamplingFromApproval = new UpdateWorkflowQcSamplingFromApproval()
                {
                    SamplingId = data.SamplingId,
                    WorkflowStatus = submitDocument.WorkflowStatus,
                    IsInWorkflow = submitDocument.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME ? false : true,
                    UpdatedBy = data.NIK
                };

                await _workflowQcSamplingDataProvider.UpdateWorkflowQcSamplingDataFromApproval(updateWorkflowQcSamplingFromApproval);
            }

            return response;
        }

        public async Task<List<string>> _GetNikNextPicOrgId(string workflowStatus, int samplingId)
        {
            string nik = "";
            List<string> nikArray = new List<string>();

            if ((workflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT) || (workflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KASIE))
            {
                var positionType = "";
                var organizationId = await _qcSamplingDataProvider.getOrganizatiionBySamplingId(samplingId);
                //cari sedang berasa di status mana , jika di draft berarti ke orgId operator nextPicOrgId adalah kasie pemilik fasilitas
                //jika berada di kasie pemilik fasilitas maka nextPicOrgId adalah kabag pemilik fasilitas
                switch (workflowStatus)
                {
                    case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KASIE:
                        positionType = ApplicationConstant.POSITION_TYPE_KABAG;
                        break;
                    case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT:
                        positionType = ApplicationConstant.POSITION_TYPE_KASIE;
                        break;
                }
                var responseGetNik = await _bioHRIntegrationBussinesProviders.GetNikByOrganizationIdandPositionType(organizationId.ToString(), positionType);
                foreach (var item in responseGetNik.Data)
                {
                    nikArray.Add(item.NewUserId);
                }
            }
            // else
            // {
            //     nik = null;
            // }

            return nikArray;
        }

        private int _getStatus(string workflowStatus)
        {
            var status = 0;
            switch (workflowStatus)
            {
                case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT:
                    status = ApplicationConstant.STATUS_SUBMIT;
                    break;
                case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KASIE:
                    status = ApplicationConstant.STATUS_IN_REVIEW_KASIE;
                    break;
                case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG:
                    status = ApplicationConstant.STATUS_IN_REVIEW_KABAG;
                    break;
                case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA:
                    status = ApplicationConstant.STATUS_IN_REVIEW_QA;
                    break;
                case ApplicationConstant.WORKFLOW_STATUS_NAME_COMPLETE:
                    status = ApplicationConstant.STATUS_APPROVED;
                    break;
            }

            return status;
        }

    }
}
