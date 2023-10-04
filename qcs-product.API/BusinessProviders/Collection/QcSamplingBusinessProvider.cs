using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.Helpers;
using qcs_product.API.Models;
using qcs_product.API.SettingModels;
using qcs_product.API.ViewModels;
using qcs_product.API.WorkflowModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class QcSamplingBusinessProvider : IQcSamplingBusinessProvider
    {
        private readonly IQcSamplingDataProvider _dataProvider;
        private readonly IQcRequestDataProvider _dataProviderRequestQc;
        private readonly IItemDataProvider _dataProviderItem;
        private readonly INotificationServiceBusinessProvider _notification;
        private readonly ILogger<QcSamplingBusinessProvider> _logger;
        private readonly IBioHRIntegrationBussinesProviders _dataProviderBioHR;
        private readonly EnvironmentSetting _environmentSetting;

        private readonly ISamplingShipmentDataProvider _dataProviderShipment;
        private readonly IReviewDataProvider _reviewDataProvider;
        private readonly IReviewBusinessProvider _reviewBusinessProvider;
        private readonly IWorkflowQcSamplingDataProvider _workflowQcSamplingDataProvider;
        private readonly IDigitalSignatureDataProvider _digitalSignatureDataProvider;
        private readonly IWorkflowServiceBusinessProvider _workflowServiceBusinessProvider;
        private readonly IBioHRIntegrationBussinesProviders _bioHRIntegrationBussinesProviders;
        private readonly IAuditTrailBusinessProvider _auditTrailBusinessProvider;
        private readonly IAUAMServiceBusinessProviders _auamServiceBusinessProviders;
        private readonly IWorkflowServiceDataProvider _workflowServiceDataProvider;
        private readonly IQcTestDataProvider _qcTestDataProvider;


        [ExcludeFromCodeCoverage]
        public QcSamplingBusinessProvider(IQcSamplingDataProvider dataProvider, IQcRequestDataProvider dataProviderRequestQc, IItemDataProvider dataProviderItem, ILogger<QcSamplingBusinessProvider> logger, INotificationServiceBusinessProvider notification,
            IOptions<EnvironmentSetting> environmentSetting,
            IBioHRIntegrationBussinesProviders dataProviderBioHR,
            ISamplingShipmentDataProvider dataProviderShipment,
            IReviewDataProvider reviewDataProvider,
            IWorkflowQcSamplingDataProvider workflowQcSamplingDataProvider,
            IReviewBusinessProvider reviewBusinessProvider,
            IDigitalSignatureDataProvider digitalSignatureDataProvider,
            IWorkflowServiceBusinessProvider workflowServiceBusinessProvider,
            IWorkflowServiceDataProvider workflowServiceDataProvider,
            IBioHRIntegrationBussinesProviders bioHRIntegrationBussinesProviders,
            IAUAMServiceBusinessProviders auamServiceBusinessProviders,
            IAuditTrailBusinessProvider auditTrailBusinessProvider,
            IQcTestDataProvider qcTestDataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _environmentSetting = environmentSetting.Value;
            _dataProviderRequestQc = dataProviderRequestQc ?? throw new ArgumentNullException(nameof(dataProviderRequestQc));
            _dataProviderItem = dataProviderItem ?? throw new ArgumentNullException(nameof(dataProviderItem));
            _logger = logger;
            _notification = notification;
            _dataProviderShipment = dataProviderShipment ?? throw new ArgumentNullException(nameof(dataProviderShipment));
            _reviewDataProvider = reviewDataProvider ?? throw new ArgumentNullException(nameof(reviewDataProvider));
            _workflowQcSamplingDataProvider = workflowQcSamplingDataProvider ?? throw new ArgumentNullException(nameof(workflowQcSamplingDataProvider));
            _reviewBusinessProvider = reviewBusinessProvider ?? throw new ArgumentNullException(nameof(reviewBusinessProvider));
            _digitalSignatureDataProvider = digitalSignatureDataProvider ?? throw new ArgumentNullException(nameof(digitalSignatureDataProvider));
            _workflowServiceBusinessProvider = workflowServiceBusinessProvider ?? throw new ArgumentNullException(nameof(workflowServiceBusinessProvider)); _bioHRIntegrationBussinesProviders = bioHRIntegrationBussinesProviders ?? throw new ArgumentNullException(nameof(bioHRIntegrationBussinesProviders));
            _auditTrailBusinessProvider = auditTrailBusinessProvider;
            _dataProviderBioHR = dataProviderBioHR;
            _auamServiceBusinessProviders = auamServiceBusinessProviders;
            _workflowServiceDataProvider = workflowServiceDataProvider ?? throw new ArgumentNullException(nameof(workflowServiceDataProvider));
            _qcTestDataProvider = qcTestDataProvider ?? throw new ArgumentNullException(nameof(qcTestDataProvider));
        }

        public async Task<ResponseViewModel<QcRequestSamplingRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int? orgId, int TypeRequestId, int SamplingTypeId)
        {
            var statusFilter = new List<int>();
            if (status == null)
            {

                statusFilter.Add(ApplicationConstant.STATUS_REJECT);
                statusFilter.Add(ApplicationConstant.STATUS_CANCEL);
                statusFilter.Add(ApplicationConstant.STATUS_DRAFT);
                statusFilter.Add(ApplicationConstant.STATUS_SUBMIT);
                statusFilter.Add(ApplicationConstant.STATUS_APPROVED);
                statusFilter.Add(ApplicationConstant.STATUS_IN_REVIEW_KABAG);
                statusFilter.Add(ApplicationConstant.STATUS_IN_REVIEW_KASIE);
                statusFilter.Add(ApplicationConstant.STATUS_IN_REVIEW_QA);

            }
            else
            {
                // filter status from param status is string
                statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<QcRequestSamplingRelationViewModel> result = new ResponseViewModel<QcRequestSamplingRelationViewModel>();

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.END_DATE_LESS_THAN_BEGIN_DATE_ERROR_MESSAGE;

                    return result;
                }
            }

            List<QcRequestSamplingRelationViewModel> getData = await _dataProvider.List(search, limit, pagination.CalculateOffset(), startDate, endDate, statusFilter, orgId, TypeRequestId, SamplingTypeId);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;


        }

        public async Task<ResponseViewModel<QcSamplingRelationViewModel>> GetDetaiById(int id, string sort)
        {
            ResponseViewModel<QcSamplingRelationViewModel> result = new ResponseViewModel<QcSamplingRelationViewModel>();
            _logger.LogInformation($"getData: {id}");
            var getData = await _dataProvider.GetDetailRelationById(id, sort);
            _logger.LogInformation($"getData: {getData}");

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;

        }

        public async Task<ResponseViewModel<QcSampling>> Edit(EditSampleQcBindingModel data)
        {
            ResponseViewModel<QcSampling> result = new ResponseViewModel<QcSampling>();

            var getSamplingQc = await _dataProvider.GetById(data.SamplingId);

            if (getSamplingQc == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.MESSAGE_MUST_REQUIRED;
                return result;
            }
            
            // checking quantity first

            if (data.IsSubmit)
            {
                foreach(var items in data.SampleData.GroupBy(x => x.NoBatch)
                            .Select(group => new { 
                                NoBatch = group.Key, 
                                Count = group.Count() 
                            })
                            .OrderBy(x => x.NoBatch))
                {
                    _logger.LogInformation("{0} {1}", items.NoBatch, items.Count);
                
                    var quantity = await _dataProviderItem.ItemMediaQuotation(items.NoBatch);
                    if (items.Count > quantity.CurrentQuantity)
                    {
                        result.StatusCode = 400;
                        result.Message = String.Format("Item dengan Batch {0} quantity kurang {1}", items.NoBatch,
                            Math.Abs(quantity.CurrentQuantity - items.Count));
                    
                        return result;
                    }
                }
            }
           
            // eof checking quantity first

            var currentStatus = getSamplingQc.Status;

            if (getSamplingQc.Status != ApplicationConstant.STATUS_DRAFT && getSamplingQc.Status != ApplicationConstant.STATUS_REJECT)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.DATA_CANT_EDIT;
                return result;
            }

            var usernameModifier = await _auditTrailBusinessProvider.GetUsernameByNik(data.UpdatedBy);

            if (getSamplingQc.SamplingTypeId == 15 || getSamplingQc.SamplingTypeId == 16)
            {
                // var sampleAmountCountAS = await _dataProvider.GetSampleAmountCountById(data.SamplingId, ApplicationConstant.TEST_PARAMETER_AS);
                // if (getSamplingQc.Status >= 1)
                // {

                //     if (!data.SamplingTools.Any() && sampleAmountCountAS != 0)
                //     {
                //         result.StatusCode = 400;
                //         result.Message = ApplicationConstant.TOOLS_SAMPING_MUST_REQUIRED;
                //         return result;
                //     }

                //     /*if (!data.SamplingMaterials.Any())
                //     {
                //         result.StatusCode = 400;
                //         result.Message = ApplicationConstant.MATERIALS_SAMPING_MUST_REQUIRED;
                //         return result;
                //     }*/
                // }

                if (data.IsSubmit)
                {
                    // if (!data.SamplingTools.Any() && sampleAmountCountAS != 0)
                    // {
                    //     result.StatusCode = 400;
                    //     result.Message = ApplicationConstant.TOOLS_SAMPLING_MUST_REQUIRED;
                    //     return result;
                    // }


                    /*if (getSamplingQc.SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_EMM)
                    {
                        if (!data.SamplingMaterials.Any())
                        {
                            result.StatusCode = 400;
                            result.Message = ApplicationConstant.MATERIAL_SAMPLING_MUST_REQUIRED;
                            return result;
                        }
                    }*/

                    var ListSampleEmptyAttch = new List<string>();

                    /* Check mandatory data */
                    if (getSamplingQc.SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_PC) // Check Sampling EM-PC
                    {
                        if (data.SampleData.Any())
                        {
                            foreach (var sp in data.SampleData) // Loop For Sample Data
                            {

                                // check sample portable - child sample
                                if (sp.SampleChild.Any())
                                {
                                    var noSampleChild = 0;
                                    foreach (var spChild in sp.SampleChild) // Loop for Child Sample
                                    {
                                        if (spChild.AttchmentFile == null || spChild.AttchmentFile == "")
                                        {
                                            noSampleChild++;
                                            var msgSpChild = $"Portable {sp.SamplingPointCode}";
                                            if (!ListSampleEmptyAttch.Contains(msgSpChild))
                                            {
                                                ListSampleEmptyAttch.Add(msgSpChild);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // check sample Continues
                                    if (sp.AttchmentFile == null || sp.AttchmentFile == "") // Check is empty??
                                    {
                                        if (!ListSampleEmptyAttch.Contains(sp.SamplingPointCode))
                                        {
                                            ListSampleEmptyAttch.Add(sp.SamplingPointCode);
                                        }
                                    }
                                }
                            }
                        }

                        /* check list */
                        if (ListSampleEmptyAttch.Any())
                        {
                            var msgErrorList = String.Join(", ", ListSampleEmptyAttch.ToArray());

                            result.StatusCode = 400;
                            result.Message = $"Attachment sample for {msgErrorList} is empty";
                            return result;
                        }
                    }

                    getSamplingQc.Status = ApplicationConstant.STATUS_SUBMIT;
                }

                //check sample
                if (data.SampleData.Any())
                {
                    foreach (var s in data.SampleData)
                    {
                        if (data.IsSubmit)
                        {
                            //if (data.SamplingDateFrom == null || data.SamplingDateTo == null)
                            //{
                            //    result.StatusCode = 400;
                            //    result.Message = "Sampling Date is Required";
                            //    return result;
                            //}
                            //check finger dab
                            if (s.TestParamId == ApplicationConstant.TEST_PARAMETER_FD)
                            {
                                //check data personal
                                if (s.PersonalId == null || s.PersonalInitial == null || s.PersonalName == null)
                                {
                                    result.StatusCode = 400;
                                    result.Message = ApplicationConstant.PERSONAL_MUST_REQUIRED;
                                    return result;
                                }
                            }

                            if (s.SamplingPointCode == null ||
                                s.GradeRoomId == null ||
                                s.GradeRoomName == null ||
                                s.TestParamId == null ||
                                s.TestParamName == null ||
                                s.SamplingDateTimeFrom == null ||
                                s.SamplingDateTimeTo == null ||
                                s.TestScenarioId == null)
                            {
                                result.StatusCode = 400;
                                result.Message = ApplicationConstant.SAMPLE_MUST_REQUIRED;
                                return result;
                            }


                        }
                    }
                }

                getSamplingQc.SamplingDateFrom = data.SamplingDateFrom;
                getSamplingQc.SamplingDateTo = data.SamplingDateTo;
                getSamplingQc.UpdatedBy = data.UpdatedBy;
                getSamplingQc.Note = data.Note;
                getSamplingQc.AttchmentFile = data.AttchmentFile;
                getSamplingQc.UpdatedAt = DateHelper.Now();

                var editData = await _dataProvider.Edit(getSamplingQc, data.SampleData, data.SamplingPersonels, data.Batch);
                List<QcSampling> ResultData = new List<QcSampling>() { editData };

                var getData = await _dataProvider.GetShortById(getSamplingQc.Id);
                string workflowDocumentCode = "";
                if (getSamplingQc.Status == ApplicationConstant.STATUS_SUBMIT)
                {
                    //jika status nya bukan reject
                    if (currentStatus != ApplicationConstant.STATUS_REJECT)
                    {
                        // save into workflow sampling code
                        WorkflowQcSampling insertQcSampling = new WorkflowQcSampling()
                        {
                            QcSamplingId = data.SamplingId,
                            WorkflowStatus = "Review Kasie", //TODO pindah ke constant
                            WorkflowDocumentCode = getSamplingQc.SamplingTypeId == ApplicationConstant.SAMPLING_TYPE_ID_EMP ? ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE + "-" + getSamplingQc.Code : ApplicationConstant.PREFIX_EM_M_WORKFLOW_CODE + "-" + getSamplingQc.Code,
                            WorkflowCode = ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_1,
                            IsInWorkflow = true,
                            CreatedBy = data.UpdatedBy,
                            CreatedAt = DateHelper.Now(),
                            UpdatedBy = data.UpdatedBy,
                            UpdatedAt = DateHelper.Now(),
                        };

                        await _workflowQcSamplingDataProvider.Insert(insertQcSampling);

                        //insert into review business to initial workflow
                        NewWorkflowDocument newWorkflowDocument = new NewWorkflowDocument()
                        {
                            DocumentCode = insertQcSampling.WorkflowDocumentCode,
                            ApplicationCode = ApplicationConstant.APP_CODE,
                            WorkflowCode = insertQcSampling.WorkflowCode,
                            Description = ApplicationConstant.WORKFLOW_INITIAL_DESC,
                            CreatedBy = data.UpdatedBy,
                            CreatorOrgId = data.UpdatedBy
                        };

                        await _reviewDataProvider.InitialDoc(newWorkflowDocument);
                        workflowDocumentCode = insertQcSampling.WorkflowDocumentCode;
                    }
                    else
                    {
                        var getQcSamplingHistory = await _workflowQcSamplingDataProvider.GetByWorkflowByQcSamplingIdIsInWorkflow(getSamplingQc.Id);

                        workflowDocumentCode = getQcSamplingHistory.WorkflowDocumentCode;
                    }

                    //pada bagian ini user insert/submit data untuk mulai workflow step pertama

                    //get nextPIC if submit doc
                    //nextPICOrgId
                    //var posId = await _dataProvider.GetPosIdBySamplingId(data.SamplingId);
                    var posIdData = await _dataProvider.GetPosIdBySamplingIdV2(data.SamplingId);
                    var posIdInt = posIdData.Select(x => Convert.ToInt32(x)).Distinct().ToList();
                    var actionType = currentStatus == ApplicationConstant.STATUS_DRAFT ? ApplicationConstant.WORKFLOW_ACTION_SUBMIT_NAME : ApplicationConstant.WORKFLOW_ACTION_EDIT_NAME;
                    var workflowPIC = await _reviewDataProvider.GetPIC(workflowDocumentCode);
                    List<ResponseGetEmployeeBioHRViewModel> lsNextPIC = new List<ResponseGetEmployeeBioHRViewModel>();
                    List<string> lsNextPICNewNik = new List<string>();

                    if (posIdData.Any())
                    {
                        //get user PIC by list positionId
                        var nextPics = await _bioHRIntegrationBussinesProviders.GetListEmployeeByListPosId(posIdInt);
                        lsNextPIC.AddRange(nextPics);
                        lsNextPICNewNik.AddRange(nextPics.Select(x => x.NewNik));

                        //jika user PIC not found -> get dari delegasi
                        var foundPosIds = nextPics.Select(x => x.PosisiId).ToList();
                        var notFoundPosIds = posIdInt
                            .Where(x => !foundPosIds.Contains(x.ToString()))
                            .Select(x => x.ToString())
                            .Distinct().ToList();
                        List<string> lsDelegasiNik = new List<string>();

                        foreach (var posId in notFoundPosIds)
                        {
                            var delegation = await _bioHRIntegrationBussinesProviders.GetDelegationByPosId(posId);
                            var lsNik = delegation.Select(x => x.PersonalNumberTo);
                            lsDelegasiNik.AddRange(lsNik);
                        }

                        if (lsDelegasiNik.Any())
                        {
                            var delegasiPics = await _bioHRIntegrationBussinesProviders.GetListEmployeeByListNik(lsDelegasiNik);
                            lsNextPIC.AddRange(delegasiPics);
                            lsNextPICNewNik.AddRange(delegasiPics.Select(x => x.NewNik));
                        }
                    }

                    lsNextPICNewNik = lsNextPICNewNik.Distinct().ToList();

                    WorkflowDocumentSubmitModel insertedDocument = new WorkflowDocumentSubmitModel()
                    {
                        ApplicationCode = ApplicationConstant.APP_CODE,
                        Notes = "-",
                        OrgId = data.UpdatedBy,
                        DocumentCode = workflowDocumentCode,
                        NextPICOrgIdList = lsNextPICNewNik,
                        WorkflowActionId = await _reviewDataProvider.GetWorkflowActionId(workflowDocumentCode, actionType)
                    };

                    ResponseInsertReview submitDocument = await _reviewDataProvider.InsertReview(insertedDocument);

                    UpdateQcSamplingFromApproval qcSamplingFromApproval = new UpdateQcSamplingFromApproval()
                    {
                        SamplingId = data.SamplingId,
                        WorkflowStatus = submitDocument.WorkflowStatus,
                        Status = ApplicationConstant.STATUS_SUBMIT,
                        UpdatedBy = data.UpdatedBy
                    };

                    await _dataProvider.UpdateQcSamplingDataFromApproval(qcSamplingFromApproval);

                    //jika di dari data rijek
                    if (currentStatus == ApplicationConstant.STATUS_REJECT)
                    {
                        UpdateWorkflowQcSamplingFromApproval updateWorkflowQcSamplingFromApproval = new UpdateWorkflowQcSamplingFromApproval()
                        {
                            SamplingId = getSamplingQc.Id,
                            WorkflowStatus = submitDocument.WorkflowStatus,
                            IsInWorkflow = true,
                            UpdatedBy = data.UpdatedBy,
                            RowStatus = ApplicationConstant.WORKFLOW_ACTION_EDIT_NAME
                        };
                        await _workflowQcSamplingDataProvider.UpdateWorkflowQcSamplingDataFromApproval(updateWorkflowQcSamplingFromApproval);

                    }

                    // hardcode khusus gedung 16
                    string buildingCode = await _dataProvider.GetBuildingCodeBySamplingId(getSamplingQc.Id);
                    if ((buildingCode != null) && (buildingCode == ApplicationConstant.GEDUNG_16_CODE))
                    {
                        lsNextPICNewNik = new List<string> { ApplicationConstant.GILANG_NADIA_NEWNIK };
                    }

                    if (actionType == ApplicationConstant.WORKFLOW_ACTION_EDIT_NAME)
                    {
                        var niks = lsNextPICNewNik.Select(x => x).ToList();
                        niks.Add(data.UpdatedBy);
                        var employees = await _bioHRIntegrationBussinesProviders.GetListEmployeeByListNewNik(niks);

                        await _sendNotif(getSamplingQc.RequestQcsId, lsNextPICNewNik, data.UpdatedBy, ApplicationConstant.EDIT_ACTION_SAMPLING_NOTIF, getSamplingQc.SamplingTypeName, "", employees);
                    }
                    else
                    {
                        //notif
                        var personal = await _dataProviderRequestQc.GetPersonalById(1);//TODO 1 pindah ke constant
                                                                                       //get request purposes
                        string purposesMsg = "-";
                        var nameAudit = "No Name";
                        var NoHandphoneAudit = "";
                        var EmailAudit = "";
                        var PicNameAudit = "No Name";
                        var purposesDatas = await _dataProviderRequestQc.getRequestPurposeNames(getSamplingQc.RequestQcsId);
                        if (purposesDatas.Any())
                        {
                            purposesMsg = string.Join(", ", purposesDatas.ToArray());
                        }

                        //get info personel untuk send notif
                        if (lsNextPICNewNik.Any())
                        {
                            foreach (var picNIK in lsNextPICNewNik)
                            {
                                var getDetailPersonelBioHR = lsNextPIC.FirstOrDefault(x => x.NewNik == picNIK);
                                if (getDetailPersonelBioHR != null)
                                {
                                    nameAudit = getDetailPersonelBioHR.Nama;
                                    NoHandphoneAudit = getDetailPersonelBioHR.Telepon;
                                    EmailAudit = getDetailPersonelBioHR.Email;
                                    PicNameAudit = "";
                                }
                                else
                                {
                                    var getDetailPICAUAM = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(picNIK);
                                    if (getDetailPICAUAM != null)
                                    {
                                        nameAudit = getDetailPICAUAM.Name;
                                        NoHandphoneAudit = getDetailPICAUAM.NoTelp;
                                        EmailAudit = getDetailPICAUAM.Email;
                                    }
                                }
                                if (personal != null)
                                {
                                    MessageNotificationSamplingAltViewModel settingMessage = new MessageNotificationSamplingAltViewModel(
                                        getData.NoRequest,
                                        getData.TypeRequestId,
                                        getData.NoBatch,
                                        getData.ItemName,
                                        purposesMsg,
                                        // personal.Email,
                                        EmailAudit,
                                        ApplicationConstant.NEW_ACTION_NOTIF,
                                        (nameAudit == "No Name" ? personal.Name : nameAudit),
                                        // personal.NoHandphone,
                                        NoHandphoneAudit,
                                        PicNameAudit
                                    );
                                    // if (_environmentSetting.EnvironmentName == ApplicationConstant.SOFTLIVE_ENVIRONMENT_NAME)
                                    // {
                                    //     settingMessage.EmailAddress = EmailAudit;
                                    //     settingMessage.NoHandphone = NoHandphoneAudit;
                                    // }

                                    if (_environmentSetting.EnvironmentName == ApplicationConstant.DEVELOPMENT_ENVIRONMENT_NAME || _environmentSetting.EnvironmentName == ApplicationConstant.TESTING_ENVIRONMENT_NAME)
                                    {
                                        settingMessage.EmailAddress = personal.Email;
                                        settingMessage.NoHandphone = personal.NoHandphone;
                                    }

                                    _notification.SendEmailNotifSampling(settingMessage);
                                    _notification.SendWhatsAppNotifSampling(settingMessage);
                                }
                            }
                        }
                    }

                    if (getSamplingQc.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_COMPLETE_NAME)
                    {
                        await _auditTrailBusinessProvider.Add(ApplicationConstant.QS_SAMPLING_STATUS_LABEL_EDIT, getSamplingQc.Code, getSamplingQc, usernameModifier);
                    }
                    else
                    {
                        await _auditTrailBusinessProvider.Add(ApplicationConstant.QS_SAMPLING_STATUS_LABEL_SUBMIT, getSamplingQc.Code, getSamplingQc, usernameModifier);

                    }
                }
                else
                {
                    await _auditTrailBusinessProvider.Add(ApplicationConstant.QS_SAMPLING_STATUS_LABEL_DRAFT, getSamplingQc.Code, getSamplingQc, usernameModifier);
                }
            }
            else if (getSamplingQc.SamplingTypeId == 34)
            {
                if (data.IsSubmit)
                {
                    if (!data.SamplingPersonels.Any())
                    {
                        result.StatusCode = 400;
                        result.Message = ApplicationConstant.PERSONAL_MUST_REQUIRED;
                        return result;
                    }

                    if (data.ProductDate == null ||
                        data.ProductMethodId == null ||
                        data.ProductShipmentDate == null ||
                        data.ProductShipmentTemperature == null)
                    {
                        result.StatusCode = 400;
                        result.Message = ApplicationConstant.MESSAGE_MUST_REQUIRED;
                        return result;
                    }

                    getSamplingQc.Status = ApplicationConstant.STATUS_IN_REVIEW_KASIE_KABAG;
                }

                getSamplingQc.SamplingDateFrom = data.SamplingDateFrom;
                getSamplingQc.SamplingDateTo = data.SamplingDateTo;
                getSamplingQc.UpdatedBy = data.UpdatedBy;
                getSamplingQc.UpdatedAt = DateHelper.Now();
                // Input Sampling Product ------------------------------------
                getSamplingQc.ProductDate = data.ProductDate;
                getSamplingQc.ProductMethodId = data.ProductMethodId;
                getSamplingQc.ProductShipmentDate = data.ProductShipmentDate;
                getSamplingQc.ProductShipmentTemperature = data.ProductShipmentTemperature;
                getSamplingQc.ProductDataLogger = data.ProductDataLogger;

                var editData = await _dataProvider.Edit(getSamplingQc, data.SampleData, data.SamplingPersonels, data.Batch);

                if (getSamplingQc.Status == ApplicationConstant.STATUS_IN_REVIEW_KASIE_KABAG)
                {
                    await _auditTrailBusinessProvider.Add(ApplicationConstant.QS_SAMPLING_STATUS_LABEL_SUBMIT, getSamplingQc.Code, getSamplingQc, usernameModifier);
                }
                else
                {
                    await _auditTrailBusinessProvider.Add(ApplicationConstant.QS_SAMPLING_STATUS_LABEL_DRAFT, getSamplingQc.Code, getSamplingQc, usernameModifier);
                }
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            // result.Data = ResultData;
            result.Data = null;

            return result;
        }

        public async Task<ResponseViewModel<SampleAvailableViewModel>> ListSampleAvailable(string search, string roomId, int testParamId)
        {
            ResponseViewModel<SampleAvailableViewModel> result = new ResponseViewModel<SampleAvailableViewModel>();

            if (roomId == null)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.MESSAGE_MUST_REQUIRED;

                return result;
            }

            //get data activity master data
            var roomIdsFilter = new List<int>();

            // filter status from param status is string
            roomIdsFilter = roomId.Split(',').Select(x => Int32.Parse(x)).Reverse().ToList();

            if (testParamId == 0)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.MESSAGE_MUST_REQUIRED;

                return result;
            }

            List<SampleAvailableViewModel> getData = await _dataProvider.ListSampleAvailable(search, roomIdsFilter, testParamId);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;

        }

        public async Task<ResponseViewModel<SampleAvailableViewModel>> ListSampleAvailableBySamplingId(string search, int samplingId, int? testParamId, string testScenarioLabel)
        {
            ResponseViewModel<SampleAvailableViewModel> result = new ResponseViewModel<SampleAvailableViewModel>();
            var testParams = new List<int>();

            if ((samplingId == null) || (samplingId == 0))
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.MESSAGE_MUST_REQUIRED;

                return result;
            }

            if (testParamId == 0)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.MESSAGE_MUST_REQUIRED;

                return result;
            }

            List<SampleAvailableViewModel> getData = await _dataProvider.ListSampleAvailableBySamplingId(search, samplingId, testParamId, testScenarioLabel);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;

        }

        public async Task<ResponseViewModel<TestParameterAvailableViewModel>> ListTestParamAvailable(string search, Int32 testGroupId, Int32 requestId)
        {

            ResponseViewModel<TestParameterAvailableViewModel> result = new ResponseViewModel<TestParameterAvailableViewModel>();
            List<TestParameterAvailableViewModel> getData = await _dataProvider.ListTestParamAvailable(search, testGroupId, requestId);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;

        }

        public async Task<ResponseViewModel<ParameterThresholdRelationViewModel>> ListParameterThreshold(string roomId, int? testScenarioId = null)
        {

            //get data activity master data
            var roomIdsFilter = new List<int>();

            // filter status from param status is string
            roomIdsFilter = roomId.Split(',').Select(x => Int32.Parse(x)).Reverse().ToList();

            ResponseViewModel<ParameterThresholdRelationViewModel> result = new ResponseViewModel<ParameterThresholdRelationViewModel>();
            List<ParameterThresholdRelationViewModel> getData = await _dataProvider.ListParameterThreshold(roomIdsFilter, testScenarioId);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = getData;

            return result;
        }

        public async Task<ResponseViewModel<ParameterThresholdRelationAltViewModel>> ListParameterThresholdAlt(string GradeRoomId, string testScenarioLabel, int? testGroupId)
        {
            //get data activity master data
            var gradeRoomIds = new List<int>();

            // filter status from param status is string
            gradeRoomIds = GradeRoomId.Split(',').Select(x => Int32.Parse(x)).Reverse().ToList();


            ResponseViewModel<ParameterThresholdRelationAltViewModel> result = new ResponseViewModel<ParameterThresholdRelationAltViewModel>();
            //List<ParameterThresholdRelationAltViewModel> getData = await _dataProvider.ListParameterThresholdAlt(gradeRoomIds, testScenarioLabel, testGroupId);
            List<ParameterThresholdRelationAltViewModel> getData = await _dataProvider.ListParameterThresholdAltV2(gradeRoomIds, testScenarioLabel, testGroupId);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = getData;

            return result;

        }

        public async Task<ResponseViewModel<QcLabelRelationViewModel>> ListLabelSampleQc(Int32 SamplingId, Int32 SampleId, string SampleCode, Int32 samplePointId, Int32 testParameterId)
        {
            ResponseViewModel<QcLabelRelationViewModel> result = new ResponseViewModel<QcLabelRelationViewModel>();
            List<QcLabelRelationViewModel> getData = await _dataProvider.ListLabelSampleQc(SamplingId, SampleId, SampleCode, samplePointId, testParameterId);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;
        }

        public async Task<ResponseViewModel<QcLabelBatchRelationViewModel>> ListLabelBatchQc(string SamplingId, string SampleCode)
        {
            var samplingIds = new List<int>();
            if (SamplingId != null)
            {
                samplingIds = SamplingId.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            ResponseViewModel<QcLabelBatchRelationViewModel> result = new ResponseViewModel<QcLabelBatchRelationViewModel>();
            List<QcLabelBatchRelationViewModel> getData = await _dataProvider.ListLabelBatchQc(samplingIds, SampleCode);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {

                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;
        }

        public async Task<ResponseViewModel<WorkflowSubmitBindingModel>> InsertApproval(InsertApprovalBindingModel data)
        {
            ResponseViewModel<WorkflowSubmitBindingModel> result = new ResponseViewModel<WorkflowSubmitBindingModel>();

            var getDigitalSignature = await _digitalSignatureDataProvider.Authenticate(data.DigitalSignature, data.NIK);
            var workflowQcSampling = await _workflowQcSamplingDataProvider.GetByWorkflowByQcSamplingIdIsInWorkflow(data.DataId);
            if (getDigitalSignature == false)
            {
                result.StatusCode = 403;
                result.Message = ApplicationConstant.WRONG_DIGITAL_SIGNATURE;
            }
            else if (data.Notes.Length > 200)
            {
                result.StatusCode = 403;
                result.Message = ApplicationConstant.NOTES_TO_LONG_MESSAGE;
            }
            else
            {
                var validasiNextOrgId = await _GetNikNextPicOrgId(workflowQcSampling.WorkflowStatus, data.DataId);
                var currentStatus = workflowQcSampling.WorkflowStatus;

                if ((validasiNextOrgId.Count < 1) && ((currentStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KASIE) || (currentStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG)))
                {
                    result.StatusCode = 403;
                    result.Message = ApplicationConstant.EMPTY_PIC;
                }
                else
                {
                    var currentRowStatus = workflowQcSampling.RowStatus;
                    var editor = workflowQcSampling.UpdatedBy;
                    var workflowPIC = await _reviewDataProvider.GetPIC(workflowQcSampling.WorkflowDocumentCode);
                    var action = "";

                    //get data sampling before
                    var qcSamplingBefore = await _dataProvider.GetById(data.DataId);

                    if (data.IsApprove)
                    {
                        action = ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME;
                    }
                    else if ((!data.IsApprove) && (currentStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG))
                    {
                        action = ApplicationConstant.WORKFLOW_ACTION_REJECT_KABAG_NAME;
                        await _dataProviderRequestQc.UpdateIsNoBatchEditable(qcSamplingBefore.RequestQcsId, false);
                    }
                    else if ((!data.IsApprove) && (currentStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA))
                    {
                        action = ApplicationConstant.WORKFLOW_ACTION_REJECT_QA_NAME;
                        await _dataProviderRequestQc.UpdateIsNoBatchEditable(qcSamplingBefore.RequestQcsId, false);
                    }
                    else
                    {
                        action = ApplicationConstant.WORKFLOW_ACTION_REJECT_NAME;
                    }

                    List<string> nextPICOrgId = await _reviewBusinessProvider._GetNikNextPicOrgId(workflowPIC.CurrentStatusName, data.DataId);

                    #region

                    //Get History Documents
                    List<WorkflowQcSampling> worfklowQcSampling = await _workflowQcSamplingDataProvider.GetByWorkflowByQcSamplingId(data.DataId);
                    List<WorkflowHistoryQcSampling> workflowQcSamplingHistory = new List<WorkflowHistoryQcSampling>();
                    string eDate = null;
                    // string eDate2 = null;
                    foreach (var item in worfklowQcSampling)
                    {
                        DocumentHistoryResponseViewModel workflowHistory = await _reviewDataProvider.GetListHistoryWorkflow(item.WorkflowDocumentCode);
                        foreach (var itemHistory in workflowHistory.History)
                        {
                            if (((itemHistory.StatusName != "Complete") && (item.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_1)))
                            {
                                foreach (var itemPIC in itemHistory.PICs)
                                {
                                    string actionUser = "";
                                    if (itemPIC.ActionName == "Submit")
                                    {
                                        actionUser = itemPIC.ActionName;
                                        WorkflowHistoryQcSampling addWorkflowHistory = new WorkflowHistoryQcSampling()
                                        {
                                            Action = actionUser,
                                            Note = itemPIC.Notes,
                                            DateTime = itemPIC.ActionDate == null ? eDate : itemPIC.ActionDate,
                                            PersonalName = itemPIC.OrgName,
                                            PersonalNik = itemPIC.OrgId,
                                            Position = itemPIC.OrgPositionName,
                                            ChangeStatusTime = itemPIC.ActionDate == null ? (actionUser == ApplicationConstant.WORKFLOW_STATUS_PENDING ? ApplicationConstant.MAX_DATETIME : eDate) : itemPIC.ActionDate
                                        };

                                        eDate = addWorkflowHistory.DateTime;

                                        workflowQcSamplingHistory.Add(addWorkflowHistory);
                                    }
                                }
                            }
                        }
                    }
                    List<WorkflowHistoryQcSampling> workflowHistoryQcs =
                    workflowQcSamplingHistory.OrderByDescending(x => x.ChangeStatusTime).ToList();
                    WorkflowHistoryQcSampling getLastSubmit = workflowQcSamplingHistory.Where(x => x.Action == "Submit").OrderByDescending(x => x.ChangeStatusTime).FirstOrDefault();
                    #endregion

                    var timeSubmit = qcSamplingBefore.UpdatedAt;

                    if (getLastSubmit != null)
                    {
                        if (getLastSubmit.ChangeStatusTime != null)
                        {
                            //Convert Date | TODO : bikin utility convert date
                            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("id-ID");
                            DateTime getParseFormated = DateTime.ParseExact(getLastSubmit.ChangeStatusTime, "MM/dd/yyyy HH:mm:ss", null);
                            //timeSubmit = getParseFormated.ToString("yyyy-MM-dd HH:mm:ss");
                            timeSubmit = getParseFormated;
                            //Console.WriteLine(strDefaultFormatedDate);

                        }
                    }

                    //get data samping
                    var samplingById = await _dataProvider.GetById(data.DataId);
                    var actionNameSubmitWorkflow = ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME; //default actionya approve


                    if ((!data.IsApprove) && (samplingById.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_COMPLETE_NAME))
                    {
                        actionNameSubmitWorkflow = ApplicationConstant.WORKFLOW_ACTION_REJECT_COMPLETE_NAME;
                    }
                    else if (!data.IsApprove)
                    {
                        actionNameSubmitWorkflow = ApplicationConstant.WORKFLOW_ACTION_REJECT_NAME;
                    }


                    WorkflowSubmitBindingModel submitApproval = new WorkflowSubmitBindingModel()
                    {
                        ApplicationCode = ApplicationConstant.APP_CODE,
                        DocumentCode = workflowQcSampling.WorkflowDocumentCode,
                        Notes = data.Notes == null ? "-" : data.Notes,
                        OrgId = data.NIK,
                        ActionName = actionNameSubmitWorkflow,
                        NextPICOrgIdList = await _GetNikNextPicOrgId(workflowQcSampling.WorkflowStatus, data.DataId)
                    };

                    ResponseViewModel<ResponseInsertReview> submitAction = await _workflowServiceBusinessProvider.SubmitAction(submitApproval);

                    if (submitAction.StatusCode != 200)
                    {
                        result.StatusCode = submitAction.StatusCode;
                        result.Message = submitAction.Message;
                    }
                    else
                    {

                        var status = "";
                        foreach (var item in submitAction.Data)
                        {
                            status = item.WorkflowStatus == null ? ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME : item.WorkflowStatus;
                        }
                        var getStatus = _getStatus(status);
                        UpdateQcSamplingFromApproval qcSamplingFromApproval = new UpdateQcSamplingFromApproval()
                        {
                            SamplingId = data.DataId,
                            WorkflowStatus = status,
                            Status = data.IsApprove == true ? getStatus : ApplicationConstant.STATUS_REJECT,
                            UpdatedBy = data.NIK
                        };

                        await _dataProvider.UpdateQcSamplingDataFromApproval(qcSamplingFromApproval);



                        UpdateWorkflowQcSamplingFromApproval updateWorkflowQcSamplingFromApproval = new UpdateWorkflowQcSamplingFromApproval()
                        {
                            SamplingId = data.DataId,
                            WorkflowStatus = status,
                            IsInWorkflow = status == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME ? false : true,
                            UpdatedBy = data.NIK,
                            RowStatus = action
                        };

                        await _workflowQcSamplingDataProvider.UpdateWorkflowQcSamplingDataFromApproval(updateWorkflowQcSamplingFromApproval);

                        //get data sampling
                        var qcSampling = await _dataProvider.GetById(data.DataId);
                        //var getSamplingRelation = await _dataProvider.GetDetailRelationById(qcSampling.Id);

                        if (workflowQcSampling.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_1 && data.IsApprove == true)
                        //if (workflowQcSampling.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_1 && data.IsApprove == true && workflowQcSampling.WorkflowDocumentCode.Substring(0, 3) == ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE)
                        {
                            // If workflow sampling QCS phase 1/QCS-1 status is Complete, return to sampling QCS phase 2/QCS-5
                            // if (status == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME)
                            // {
                            InsertNextPhase insertNextPhase = new InsertNextPhase()
                            {
                                SamplingId = workflowQcSampling.QcSamplingId,
                                TypeSamplingId = workflowQcSampling.WorkflowDocumentCode.Substring(0, 3) == ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE ? ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE : ApplicationConstant.PREFIX_EM_M_WORKFLOW_CODE,
                                WorkflowCode = ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2,
                                NIK = workflowQcSampling.CreatedBy,
                            };
                            await InsertToNextPhase(insertNextPhase);
                            // }
                        }

                        //notifikasi 

                        var currentWorkflowQcSampling = await _workflowQcSamplingDataProvider.GetByWorkflowByQcSamplingIdLatest(data.DataId);
                        var listOperatorSampling = new List<string>() { qcSamplingBefore.CreatedBy };

                        //get employees
                        List<string> listNextPIC = new List<string>();
                        if ((data.IsApprove == true) && (currentWorkflowQcSampling.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2) && (currentWorkflowQcSampling.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG))
                        {
                            listNextPIC = await _ListNextPIC(currentWorkflowQcSampling.WorkflowDocumentCode);
                        }
                        else
                        {
                            listNextPIC = await _ListNextPIC(workflowQcSampling.WorkflowDocumentCode);
                        }

                        var niks = listNextPIC.Select(x => x).ToList();
                        niks.Add(data.NIK);
                        niks.Add(editor);
                        var employees = await _bioHRIntegrationBussinesProviders.GetListEmployeeByListNewNik(niks);

                        if (currentWorkflowQcSampling.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_KABAG_NAME && data.IsApprove == true)
                        {
                            await _sendNotif(qcSampling.RequestQcsId, listNextPIC, data.NIK, ApplicationConstant.APPROVED_ACTION_NOTIF, qcSampling.SamplingTypeName, editor, employees);
                            await _sendNotifOperatorSampling(qcSampling.RequestQcsId, listOperatorSampling, data.NIK, ApplicationConstant.APPROVED_ACTION_NOTIF_TO_OPERATOR_SAMPLING, qcSampling.SamplingTypeName, editor, employees);
                        }
                        else if (currentWorkflowQcSampling.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_NAME && data.IsApprove != true && currentWorkflowQcSampling.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_1)
                        {
                            await _sendNotif(qcSampling.RequestQcsId, listNextPIC, data.NIK, ApplicationConstant.REJECTED_ACTION_SAMPLING_NOTIF, qcSampling.SamplingTypeName, editor, employees);
                            // await _sendNotifOperatorSampling(qcSampling.RequestQcsId, listOperatorSampling, data.NIK, ApplicationConstant.REJECTED_ACTION_SAMPLING_NOTIF, qcSampling.SamplingTypeName, editor);
                        }
                        else if ((data.IsApprove == true) && (currentWorkflowQcSampling.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2) && (currentWorkflowQcSampling.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG))
                        {
                            await _sendNotif(qcSampling.RequestQcsId, listNextPIC, data.NIK, ApplicationConstant.APPROVED_ACTION_NOTIF, qcSampling.SamplingTypeName, editor, employees);
                            await _sendNotifOperatorSampling(qcSampling.RequestQcsId, listOperatorSampling, data.NIK, ApplicationConstant.APPROVED_ACTION_NOTIF_TO_OPERATOR_SAMPLING, qcSampling.SamplingTypeName, editor, employees);
                        }


                        var auditOperation = ApplicationConstant.QS_SAMPLING_STATUS_LABEL_REJECT;
                        if (data.IsApprove)
                        {
                            auditOperation = ApplicationConstant.QS_SAMPLING_STATUS_LABEL_APPROVE;

                            // If workflow status completed transfer sampling data
                            // if (status == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME)
                            // {
                            await InsertSendingSample(qcSampling.Code, timeSubmit);
                            // }
                        }

                        var usernameModifier = await _auditTrailBusinessProvider.GetUsernameByNik(qcSampling.UpdatedBy);
                        await _auditTrailBusinessProvider.Add(auditOperation, qcSampling.Code, qcSampling, usernameModifier);

                        result.Message = !data.IsApprove ? ApplicationConstant.WORKFLOW_SUCCESS_REJECT_MESSAGE : ApplicationConstant.WORKFLOW_SUCCESS_APPROVE_MESSAGE;
                        result.StatusCode = submitAction.StatusCode;
                        result.Data = new List<WorkflowSubmitBindingModel>() { submitApproval };
                    }
                }
            }
            return result;
        }

        public async Task<List<string>> _GetNikNextPicOrgId(string workflowStatus, int samplingId)
        {
            List<string> nikArray = new List<string>();

            if ((workflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT) || (workflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KASIE) || (workflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG))
            {
                if (workflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG)
                {
                    List<string> qaPICs = await _getQAPIC(samplingId);
                    foreach (var qaPIC in qaPICs)
                    {
                        nikArray.Add(qaPIC);
                    }
                }
                else
                {
                    var positionType = "";
                    var organizationId = await _dataProvider.getOrganizatiionBySamplingId(samplingId);
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
                        // Filter next pic if userId is null or empty or - dont add to nikArray
                        if (item.NewUserId != null && item.NewUserId != "" && item.NewUserId != "." && item.NewUserId != "-")
                        {
                            nikArray.Add(item.NewUserId);
                        }
                        else
                        {
                            var delegatation = await _bioHRIntegrationBussinesProviders.GetListNewNikDelegation(responseGetNik.Data[0].PositionId.ToString());
                            nikArray.AddRange(delegatation);
                        }
                    }
                }
            }
            // else
            // {
            //     nik = null;
            // }

            return nikArray;
        }

        public async Task<ResponseViewModel<WorkflowDocumentSubmitModel>> InsertToNextPhase(InsertNextPhase data)
        {
            ResponseViewModel<WorkflowDocumentSubmitModel> response = new ResponseViewModel<WorkflowDocumentSubmitModel>();

            var getSamplingQc = await _dataProvider.GetById(data.SamplingId);

            //insert workflow sampling
            WorkflowQcSampling insertQcSampling = new WorkflowQcSampling()
            {
                QcSamplingId = data.SamplingId,
                WorkflowStatus = "Draft",
                WorkflowDocumentCode = data.TypeSamplingId == ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE ? ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE + "-" + getSamplingQc.Code + "-2" : ApplicationConstant.PREFIX_EM_M_WORKFLOW_CODE + "-" + getSamplingQc.Code + "-2",
                WorkflowCode = data.WorkflowCode,
                IsInWorkflow = true,
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
            var organizationId = await _dataProvider.getOrganizatiionBySamplingId(data.SamplingId);
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

            if (!nextPICOrgIdList.Any() && responseGetNik.Data.Any())
            {
                var delegatation = await _bioHRIntegrationBussinesProviders.GetListNewNikDelegation(responseGetNik.Data[0].PositionId.ToString());
                nextPICOrgIdList.AddRange(delegatation);
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
                    Status = ApplicationConstant.STATUS_IN_REVIEW_KABAG, //status antara submit atau mengikuti workflow status
                    UpdatedBy = data.NIK
                };

                await _dataProvider.UpdateQcSamplingDataFromApproval(qcSamplingFromApproval);

                UpdateWorkflowQcSamplingFromApproval updateWorkflowQcSamplingFromApproval = new UpdateWorkflowQcSamplingFromApproval()
                {
                    SamplingId = data.SamplingId,
                    WorkflowStatus = submitDocument.WorkflowStatus,
                    IsInWorkflow = true,
                    UpdatedBy = data.NIK
                };

                await _workflowQcSamplingDataProvider.UpdateWorkflowQcSamplingDataFromApprovalNextPhase(updateWorkflowQcSamplingFromApproval);
            }

            return response;
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
                    // status = ApplicationConstant.STATUS_SUBMIT;
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

        private async Task<List<string>> _getQAPIC(int samplingId)
        {
            List<string> result = new List<string>();
            RequestQcs requestQcs = await _dataProvider.GetRequestBySamplingId(samplingId);
            List<int> purposeList = await _dataProvider.GetPurposeByRequestId(requestQcs.Id);

            List<int> specialCondition = new List<int>
            {
                ApplicationConstant.PURPOSE_ID_RE_KLASIFIKASI,
                ApplicationConstant.PURPOSE_ID_UJI_BULANAN
            };

            if (purposeList.All(specialCondition.Contains) && purposeList.Count == specialCondition.Count)
            {
                var personel = await _dataProvider.GetPurposePersonelByPurposeId(ApplicationConstant.PURPOSE_ID_RE_KLASIFIKASI);
                foreach (var nikPersonel in personel)
                {
                    result.Add(nikPersonel.Nik);
                }
            }
            else
            {
                foreach (var purpose in purposeList)
                {
                    var personel = await _dataProvider.GetPurposePersonelByPurposeId(purpose);
                    foreach (var nikPersonel in personel)
                    {
                        result.Add(nikPersonel.Nik);
                    }
                }
            }
            return result.Distinct().ToList();
        }

        private async Task _sendNotif(int requestQcId, List<string> nextPICOrgId, string nik, string status, string testType, string editor, IEnumerable<ResponseGetEmployeeBioHRViewModel> employees)
        {
            //notification message init
            //notif personel
            var personal = await _dataProviderRequestQc.GetPersonalById(2);//TODO 1 pindah ke constant
            var requestQc = await _dataProviderRequestQc.GetById(requestQcId);
            var testDataQc = await _qcTestDataProvider.GetTestDataByRequestId(requestQcId);

            //get request purposes
            string purposesMsg = "-";
            var PicEmailAudit = "";
            var PicNameAudit = "No Name";
            var PicPostionName = "";
            var EditorPostionName = "";
            var EditorName = "";
            var purposesDatas = await _dataProviderRequestQc.getRequestPurposeNames(requestQcId);
            if (purposesDatas.Any())
            {
                purposesMsg = string.Join(", ", purposesDatas.ToArray());
            }

            //get info Pic Approved
            List<ReceiverNotifModel> receivers = new List<ReceiverNotifModel>();
            foreach (var item in nextPICOrgId.GroupBy(x => x))
            {
                var getDetailPICBioHR = employees.FirstOrDefault(x => x.NewNik == item.Key);
                
                if (getDetailPICBioHR == null)
                {
                    var getDetailPICAUAM = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(item.Key);
                    if (getDetailPICAUAM != null)
                    {
                        ReceiverNotifModel revAuam = new ReceiverNotifModel()
                        {
                            ReceiverName = getDetailPICAUAM.Name,
                            ReceiverEmail = getDetailPICAUAM.Email,
                            ReceiverNoHp = getDetailPICAUAM.NoTelp,
                        };
                        receivers.Add(revAuam);
                    }
                    continue;
                }

                ReceiverNotifModel rev = new ReceiverNotifModel()
                {
                    ReceiverName = getDetailPICBioHR.Nama,
                    ReceiverEmail = getDetailPICBioHR.Email,
                    ReceiverNoHp = getDetailPICBioHR.Telepon,
                };
                receivers.Add(rev);
            }

            var getDetailPICBioHRCurrent = employees.FirstOrDefault(x => x.NewNik == nik);
            if (getDetailPICBioHRCurrent != null)
            {
                PicNameAudit = getDetailPICBioHRCurrent.Nama;
                PicEmailAudit = getDetailPICBioHRCurrent.Email;
                PicPostionName = getDetailPICBioHRCurrent.Posisi;
            }

            var getDetailPICBioHREditor = employees.FirstOrDefault(x => x.NewNik == editor);
            if (getDetailPICBioHREditor != null)
            {
                EditorPostionName = getDetailPICBioHREditor.Posisi;
                EditorName = getDetailPICBioHREditor.Nama;
            }

            //get receiver 
            foreach (var itemRe in receivers)
            {
                MessageNotificationMonitoringViewModel settingMessage = new MessageNotificationMonitoringViewModel(
                    requestQc.NoRequest,
                    requestQc.NoBatch,
                    requestQc.ItemName,
                    purposesMsg,
                    // personal.Email, //item.ReceiverEmail,
                    itemRe.ReceiverEmail,
                    status,
                    itemRe.ReceiverName,
                    // personal.NoHandphone, //item.Handphone,
                    // PicPostionName,
                    itemRe.ReceiverNoHp,
                    PicNameAudit,
                    testType,
                    testDataQc == null ? "" : testDataQc.Code,
                    // EditorPostionName
                    EditorName
                );
                // if (_environmentSetting.EnvironmentName == ApplicationConstant.SOFTLIVE_ENVIRONMENT_NAME)
                // {
                //     settingMessage.EmailAddress = EmailAudit;
                //     settingMessage.NoHandphone = NoHandphoneAudit;
                // }

                if (_environmentSetting.EnvironmentName == ApplicationConstant.DEVELOPMENT_ENVIRONMENT_NAME || _environmentSetting.EnvironmentName == ApplicationConstant.TESTING_ENVIRONMENT_NAME)
                {
                    settingMessage.EmailAddress = personal.Email;
                    settingMessage.NoHandphone = personal.NoHandphone;
                }

                await _notification.SendEmailNotifMonitoring(settingMessage);
                await _notification.SendWhatsAppNotifMonitoring(settingMessage);
            }

        }

        private async Task _sendNotifOperatorSampling(int requestQcId, List<string> nextPICOrgId, string nik, string status, string testType, string editor, IEnumerable<ResponseGetEmployeeBioHRViewModel> employees)
        {
            //notification message init
            //notif personel
            var personal = await _dataProviderRequestQc.GetPersonalById(2);//TODO 1 pindah ke constant
            var requestQc = await _dataProviderRequestQc.GetById(requestQcId);
            var testDataQc = await _qcTestDataProvider.GetTestDataByRequestId(requestQcId);

            //get request purposes
            string purposesMsg = "-";
            var PicEmailAudit = "";
            var PicNameAudit = "No Name";
            var PicPostionName = "";
            var EditorPostionName = "";
            var EditorName = "";
            var purposesDatas = await _dataProviderRequestQc.getRequestPurposeNames(requestQcId);
            if (purposesDatas.Any())
            {
                purposesMsg = string.Join(", ", purposesDatas.ToArray());
            }

            //get info Pic Approved
            List<ReceiverNotifModel> receivers = new List<ReceiverNotifModel>();

            foreach (var item in nextPICOrgId.GroupBy(x => x))
            {
                var getDetailPICBioHR = employees.FirstOrDefault(x => x.NewNik == item.Key);
                
                if (getDetailPICBioHR == null)
                {
                    var getDetailPICAUAM = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(item.Key);
                    if (getDetailPICAUAM != null)
                    {
                        ReceiverNotifModel revAuam = new ReceiverNotifModel()
                        {
                            ReceiverName = getDetailPICAUAM.Name,
                            ReceiverEmail = getDetailPICAUAM.Email,
                            ReceiverNoHp = getDetailPICAUAM.NoTelp,
                        };
                        receivers.Add(revAuam);
                    }
                    continue;
                }

                ReceiverNotifModel rev = new ReceiverNotifModel()
                {
                    ReceiverName = getDetailPICBioHR.Nama,
                    ReceiverEmail = getDetailPICBioHR.Email,
                    ReceiverNoHp = getDetailPICBioHR.Telepon,
                };
                receivers.Add(rev);
            }

            var getDetailPICBioHRCurrent = employees.FirstOrDefault(x => x.NewNik == nik);
            if (getDetailPICBioHRCurrent != null)
            {
                PicNameAudit = getDetailPICBioHRCurrent.Nama;
                PicEmailAudit = getDetailPICBioHRCurrent.Email;
                PicPostionName = getDetailPICBioHRCurrent.Posisi;
            }

            var getDetailPICBioHREditor = employees.FirstOrDefault(x => x.NewNik == editor);
            if (getDetailPICBioHREditor != null)
            {
                EditorPostionName = getDetailPICBioHREditor.Posisi;
                EditorName = getDetailPICBioHREditor.Nama;
            }

            //get receiver 
            foreach (var itemRe in receivers)
            {
                MessageNotificationMonitoringViewModel settingMessage = new MessageNotificationMonitoringViewModel(
                    requestQc.NoRequest,
                    requestQc.NoBatch,
                    requestQc.ItemName,
                    purposesMsg,
                    personal.Email, //itemRe.ReceiverEmail,
                    status,
                    itemRe.ReceiverName,
                    personal.NoHandphone, //itemRe.ReceiverNoHp,
                    PicNameAudit,
                    testType,
                    testDataQc == null ? "" : testDataQc.Code,
                    // EditorPostionName
                    EditorName
                );
                await _notification.SendEmailNotifMonitoring(settingMessage);
                await _notification.SendWhatsAppNotifMonitoring(settingMessage);
            }

        }

        private async Task<List<string>> _ListReceiverNotif(string workflowDocumetnCode, string nikReviewer)
        {
            List<string> listNik = new List<string>();
            //get workflow history for get nik
            DocumentHistoryResponseViewModel workflowHistory = await _workflowServiceDataProvider.GetListHistoryWorkflow(workflowDocumetnCode);
            foreach (var itemPIC in workflowHistory.History)
            {
                foreach (var item in itemPIC.PICs)
                {
                    if (item.OrgId != nikReviewer)
                    {
                        listNik.Add(item.OrgId);
                    }
                }
            }
            return listNik.Distinct().ToList();
        }

        private async Task<List<string>> _ListNextPIC(string workflowDocumentCode)
        {
            List<string> listPIC = new List<string>();
            DocumentPICResponseModel workflowPIC = await _workflowServiceDataProvider.GetPIC(workflowDocumentCode);

            if (workflowPIC.PICs != null && workflowPIC.PICs.Any())
            {
                foreach (var item in workflowPIC.PICs)
                {
                    listPIC.Add(item.orgId);
                }
            }

            return listPIC.Distinct().ToList();
        }

        public async Task<int> InsertSendingSample(string QrCode, DateTime TimeSending)
        {
            /* Init Variable List */
            List<QcSamplingShipment> samplingShipmentCreatedList = new List<QcSamplingShipment>();
            List<QcSamplingShipmentTracker> samplingShipmentTrackerCreatedList = new List<QcSamplingShipmentTracker>();
            var countCreated = 0;
            var statusShipment = ApplicationConstant.STATUS_SHIPMENT_SENDING;
            var statusShipmentLabel = ApplicationConstant.QS_SAMPLING_STATUS_LABEL_SEND;

            /* Get Data Shippment By QrCode */
            var getDataShipment = await _dataProviderShipment.GetByQRCode(QrCode);

            if (getDataShipment.Any()) return countCreated;
            
            /* Get Data Sampling By QrCode */
            var getDataSampling = await _dataProvider.GetSamplingBatchByQRCode(QrCode);

            if (getDataSampling == null) return countCreated;
                
            var getDataRequest = await _dataProvider.GetRequestBySamplingId(getDataSampling.SamplingId);
            var getOrganization = await _dataProviderBioHR.GetOrganizationById(getDataRequest.OrgId.GetValueOrDefault());
            var getEmployee = await _dataProviderBioHR.GetEmployeeByNewNik(getDataSampling.CreatedBy);
            var getEmployeeAUAM = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(getDataSampling.CreatedBy);

            var getOrganizationUser = new ResponseGetOrganizationBioHRViewModel();
            if (getEmployee != null)
            {
                getOrganizationUser = await _dataProviderBioHR.GetOrganizationById(Int32.Parse(getEmployee.DepartmentId));
            }
            else if (getEmployeeAUAM != null)
            {
                getOrganizationUser = await _dataProviderBioHR.GetOrganizationById(getEmployeeAUAM.BioHROrganizationId);
            }
            else
            {
                getOrganizationUser.OrganizationId = 0;
                getOrganizationUser.OrganizationName = "Undefined";
            }

            /* Get Last sampling time */
            var getSample = await _dataProvider.GetSampleBySamplingId(getDataSampling.SamplingId);
            DateTime? LastTimeSample = getSample.Select(x => x.SamplingDateTimeTo).DefaultIfEmpty().Max(x => x == null ? null : x);

            /* Check Interval Date sampling and sending */
            var isLate = false;
            var intervalSend = LastTimeSample == null ? 0 : (DateHelper.Now() - LastTimeSample).Value.TotalHours;
            if (intervalSend > ApplicationConstant.THRESHOLD_SHIPMENT_2X24)
            {
                statusShipment = ApplicationConstant.STATUS_SHIPMENT_LATE_SAMPLE;
                //statusShipmentLabel = ApplicationConstant.QS_SAMPLING_STATUS_LABEL_LATE_SAMPLE;
                isLate = true;
            }


            /* Check data EM-M Sampling Has Approved */
            if (getDataSampling.SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_EMM &&
                getDataSampling.Status <= ApplicationConstant.STATUS_IN_REVIEW_KABAG)
            {
                /* Check Test Parameters */
                if (getDataSampling.Testparameters.Any())
                {
                    /* Insert by test param sampling */
                    foreach (var itm in getDataSampling.Testparameters)
                    {
                        QcSamplingShipment insertShipment = new QcSamplingShipment()
                        {
                            QcSamplingId = getDataSampling.SamplingId,
                            QrCode = getDataSampling.Code,
                            NoRequest = getDataSampling.NoRequest,
                            TestParamId = itm.Id,
                            TestParamName = itm.Name,
                            FromOrganizationId = getDataRequest.OrgId.GetValueOrDefault(), //harus di ganti dengan org id pengirim
                            FromOrganizationName = (getOrganization != null
                                ? getOrganization.OrganizationName
                                : "Undefined"), //harus di ganti dengan org id pengirim
                            ToOrganizationId = itm.OrgId,
                            ToOrganizationName = itm.OrgName,
                            IsLateTransfer = isLate,
                            StartDate = Convert.ToDateTime(TimeSending),
                            Status = statusShipment,
                            CreatedBy = getDataSampling.CreatedBy,
                            UpdatedBy = getDataSampling.CreatedBy,
                            CreatedAt = DateHelper.Now(),
                            UpdatedAt = DateHelper.Now()
                        };

                        QcSamplingShipmentTracker insertShipmentTracker = new QcSamplingShipmentTracker()
                        {
                            QrCode = getDataSampling.Code,
                            Type = ApplicationConstant.TRACKER_TYPE_SEND,
                            processAt = Convert.ToDateTime(TimeSending),
                            UserNik = getDataSampling.CreatedBy,
                            UserName = (getEmployee != null
                                ? getEmployee.Name
                                : (getEmployeeAUAM != null
                                ? getEmployeeAUAM.Name
                                : "Pengirim")), //harus ganti get by nik di bio hr
                            OrganizationId = getOrganizationUser.OrganizationId, //harus di ganti dengan org id pengirim
                            OrganizationName = getOrganizationUser.OrganizationName, //harus di ganti dengan org id pengirim
                            CreatedAt = DateHelper.Now(),
                            UpdatedAt = DateHelper.Now()
                        };

                        //insert to data provider
                        var insertNewShipment = await _dataProviderShipment.InsertNewShipment(insertShipment,
                            insertShipmentTracker, statusShipment);
                        countCreated++;

                        samplingShipmentCreatedList.Add(insertNewShipment);
                        samplingShipmentTrackerCreatedList.Add(insertShipmentTracker);

                    }
                }

            }

            if (countCreated > 0)
            {
                var usernameModifier = await _auditTrailBusinessProvider.GetUsernameByNik(getDataSampling.CreatedBy);
                foreach (var qcSamplingShipment in samplingShipmentCreatedList)
                {
                    await _auditTrailBusinessProvider.Add(statusShipmentLabel, qcSamplingShipment.QrCode, qcSamplingShipment, usernameModifier);

                    foreach (var qcSamplingShipmentTracker in samplingShipmentTrackerCreatedList)
                    {
                        if (qcSamplingShipmentTracker.QcSamplingShipmentId == qcSamplingShipment.Id)
                        {
                            await _auditTrailBusinessProvider.Add(ApplicationConstant.QS_SAMPLING_STATUS_LABEL_SEND, qcSamplingShipment.QrCode, qcSamplingShipmentTracker, qcSamplingShipment, usernameModifier);
                        }
                    }
                }
            }

            /* Return Count Sending Sampling */
            return countCreated;
        }

        public async Task<ResponseViewModel<QcSamplingNotReceivedViewModel>> ListSamplingNotReceived(string search)
        {
            ResponseViewModel<QcSamplingNotReceivedViewModel> result = new ResponseViewModel<QcSamplingNotReceivedViewModel>();
            List<QcSamplingNotReceivedViewModel> getData = await _dataProvider.ListSamplingNotReceived(search);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {

                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;
        }

        public async Task<ResponseViewModel<QcSample>> InsertReviewQaNoteDataSample(InsertReviewQaNoteQcSample insert)
        {
            ResponseViewModel<QcSample> result = new ResponseViewModel<QcSample>();
            try
            {
                var cekValidasiLengthNote = insert.Samples.FirstOrDefault(x => x.Notes.Length > 200);

                if (cekValidasiLengthNote != null)
                {
                    result.StatusCode = 403;
                    result.Message = ApplicationConstant.NOTES_TO_LONG_MESSAGE;
                }
                else
                {
                    var insertData = await _dataProvider.UpdateSampleReviewQaNote(insert);
                    result.StatusCode = 200;
                    result.Message = ApplicationConstant.MESSAGE_SAVE_NOTE_SUCCESS;
                    result.Data = insertData;
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

            return result;

        }
    }
}
