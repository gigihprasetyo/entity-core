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
using qcs_product.API.ValidationModels;
using qcs_product.API.Helpers;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class WorkflowServiceBusinessProvider : IWorkflowServiceBusinessProvider
    {
        private readonly IBaseApiBioServiceBusinessProviders _businessProviderBaseBioHR;
        private readonly IAuthenticatedUserBiohrDataProviders _dataProviderAuthBioHR;
        private readonly IWorkflowServiceDataProvider _dataProvider;
        private readonly IWorkflowQcSamplingDataProvider _workflowQcSamplingDataProvider;
        private readonly IQcSamplingDataProvider _qcSamplingDataProvider;
        private readonly IOptions<BioHRServiceSetting> _BioHRServiceSetting;
        private readonly IOptions<WorkflowServiceSetting> _WorkflowServiceSetting;
        private readonly IHttpClientFactory _clientFactory;
        private readonly QcsProductContext _context;
        private readonly IBioHRIntegrationBussinesProviders _bioHRIntegrationBussinesProviders;
        private readonly IDigitalSignatureDataProvider _digitalSignatureDataProvider;
        private readonly IWorkflowHistoryDataProvider _workflowHistoryDataProvider;
        public WorkflowServiceBusinessProvider(
            IBaseApiBioServiceBusinessProviders businessProviderBaseBioHR,
            IAuthenticatedUserBiohrDataProviders dataProviderAuthBioHR,
            IOptions<BioHRServiceSetting> bioHRServiceSetting,
            IHttpClientFactory clientFactory,
            QcsProductContext context,
            IOptions<WorkflowServiceSetting> workflowServiceSetting,
            IBioHRIntegrationBussinesProviders bioHRIntegrationBussinesProviders,
            IWorkflowServiceDataProvider dataProvider,
            IWorkflowQcSamplingDataProvider workflowQcSamplingDataProvider,
            IQcSamplingDataProvider qcSamplingDataProvider,
            IDigitalSignatureDataProvider digitalSignatureDataProvider,
            IWorkflowHistoryDataProvider workflowHistoryDataProvider)
        {
            _businessProviderBaseBioHR = businessProviderBaseBioHR ?? throw new ArgumentNullException(nameof(businessProviderBaseBioHR));
            _dataProviderAuthBioHR = dataProviderAuthBioHR ?? throw new ArgumentNullException(nameof(dataProviderAuthBioHR));
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _workflowQcSamplingDataProvider = workflowQcSamplingDataProvider ?? throw new ArgumentNullException(nameof(workflowQcSamplingDataProvider));
            _qcSamplingDataProvider = qcSamplingDataProvider ?? throw new ArgumentNullException(nameof(qcSamplingDataProvider));
            _digitalSignatureDataProvider = digitalSignatureDataProvider ?? throw new ArgumentNullException(nameof(digitalSignatureDataProvider));
            _BioHRServiceSetting = bioHRServiceSetting;
            _clientFactory = clientFactory;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _WorkflowServiceSetting = workflowServiceSetting;
            _bioHRIntegrationBussinesProviders = bioHRIntegrationBussinesProviders ?? throw new ArgumentNullException(nameof(bioHRIntegrationBussinesProviders));
            _workflowHistoryDataProvider = workflowHistoryDataProvider ?? throw new ArgumentNullException(nameof(workflowHistoryDataProvider));
        }

        public async Task<ResponseViewModel<ResponseInsertReview>> InitiateDoc(NewWorkflowDocument data)
        {
            ResponseViewModel<ResponseInsertReview> result = new ResponseViewModel<ResponseInsertReview>();
            List<ResponseInsertReview> initiateDocList = new List<ResponseInsertReview>();
            try
            {
                ResponseInsertReview initiateDocData = await _dataProvider.InitiateDoc(data);
                initiateDocList.Add(initiateDocData);
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = initiateDocList;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResponseViewModel<ResponseInsertReview>> SubmitAction(WorkflowSubmitBindingModel data)
        {
            ResponseViewModel<ResponseInsertReview> result = new ResponseViewModel<ResponseInsertReview>();

            string approvalType = data.ActionName;

            DocumentPICResponseModel workflowPIC = await _dataProvider.GetPIC(data.DocumentCode);

            DocumentActionViewModel documentAction = _GetWorkflowActionId(approvalType, workflowPIC);

            if (documentAction.WorkflowActionId != 0)
            {
                bool isNeedNextPICOrgIdList = false;

                foreach (var item in documentAction.ActionOrgs)
                {
                    if (item.OrgType == 14)
                    {
                        isNeedNextPICOrgIdList = true;
                        continue;
                    }
                }
                GeneralValidationModel nextPICOrgIdValidation = _ValidateNextPICOrgId(data, isNeedNextPICOrgIdList);
                if (nextPICOrgIdValidation.IsValid)
                {
                    WorkflowDocumentSubmitModel insertedDocument = new WorkflowDocumentSubmitModel()
                    {
                        ApplicationCode = ApplicationConstant.APP_CODE,
                        Notes = data.Notes,
                        OrgId = data.OrgId,
                        DocumentCode = data.DocumentCode,
                        WorkflowActionId = documentAction.WorkflowActionId
                    };

                    if (isNeedNextPICOrgIdList)
                    {
                        insertedDocument.NextPICOrgIdList = data.NextPICOrgIdList;
                    }

                    ResponseInsertReview submitDocument = await _dataProvider.SubmitAction(insertedDocument);

                    if (submitDocument.StatusCode == 200)
                    {
                        result.StatusCode = 200;
                        result.Data = new List<ResponseInsertReview>() { submitDocument };

                        #region insert into workflow history
                        WorkflowHistory insertWorkflowHistory = new WorkflowHistory()
                        {
                            WorkflowDocumentCode = insertedDocument.DocumentCode,
                            Note = insertedDocument.Notes,
                            WorkflowStatus = workflowPIC.CurrentStatusName,
                            PicNik = insertedDocument.OrgId,
                            Action = documentAction.ActionName,
                            CreatedBy = insertedDocument.OrgId,
                            CreatedAt = DateHelper.Now()
                        };

                        var insertWH = await _workflowHistoryDataProvider.Insert(insertWorkflowHistory);
                        #endregion
                    }
                    else
                    {
                        result.StatusCode = submitDocument.StatusCode;
                        result.Message = submitDocument.Message;
                    }
                }
                else
                {
                    result.StatusCode = 400;
                    result.Message = nextPICOrgIdValidation.ValidationMessage;
                }
            }
            else
            {
                result.StatusCode = 400;
                result.Message = "Conflict. Wrong Workflow Action Id.";
            }
            return result;
        }

        private DocumentActionViewModel _GetWorkflowActionId(string actionName, DocumentPICResponseModel data)
        {
            DocumentActionViewModel result = new DocumentActionViewModel();
            var documentAction =
            (
                from doc in data.Actions
                where doc.ActionName == actionName
                select doc
            ).FirstOrDefault();
            result = documentAction;
            return result;
        }

        private GeneralValidationModel _ValidateNextPICOrgId(WorkflowSubmitBindingModel data, bool isNeedNextPICOrgIdList)
        {
            GeneralValidationModel result = new GeneralValidationModel()
            {
                IsValid = true,
                ValidationMessage = ApplicationConstant.OK_MESSAGE
            };
            if (isNeedNextPICOrgIdList)
            {
                if (data.NextPICOrgIdList == null || !data.NextPICOrgIdList.Any())
                {
                    result.IsValid = false;
                    result.ValidationMessage = "Please provide NextPICOrgIdList.";
                }
            }
            return result;
        }

        public async Task<bool> RollbackDocument(RollbackWorkflowDocument data)
        {
            return await _dataProvider.RollbackDocument(data);
        }
    }
}
