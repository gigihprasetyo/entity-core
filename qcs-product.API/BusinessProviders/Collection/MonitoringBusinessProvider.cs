using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.API.WorkflowModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.SettingModels;
using Microsoft.Extensions.Options;
using qcs_product.API.Helpers;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class MonitoringBusinessProvider : IMonitoringBusinessProvider
    {
        private readonly IMonitoringDataProvider _dataProvider;
        private readonly IOrganizationDataProvider _dataProviderOrg;
        private readonly INotificationServiceBusinessProvider _notification;
        private readonly IBioHRIntegrationBussinesProviders _dataProviderBioHR;
        private readonly EnvironmentSetting _environmentSetting;

        private readonly ILogger<MonitoringBusinessProvider> _logger;

        private readonly IDigitalSignatureDataProvider _digitalSignatureDataProvider;
        private readonly IQcSamplingBusinessProvider _qcSamplingBusinessProvider;
        private readonly IQcTestBusinessProvider _qcTestBusinessProvider;
        private readonly IQcTestDataProvider _qcTestDataProvider;
        private readonly IQcSamplingDataProvider _qcSamplingDataProvider;
        private readonly IQcRequestDataProvider _qcRequestQcDataProvider;
        private readonly IBioHRIntegrationBussinesProviders _bioHRIntegrationBussinesProviders;
        private readonly IWorkflowQcSamplingDataProvider _workflowSamplingDataProviders;
        private readonly IWorkflowQcTransactionGroupDataProvider _workflowQcTransactionGroupDataProviders;
        private readonly IWorkflowServiceDataProvider _workflowServiceDataProviders;
        private readonly IAUAMServiceBusinessProviders _auamServiceBusinessProviders;
        private readonly IReviewDataProvider _reviewdataProvider;
        private readonly IAuditTrailBusinessProvider _auditTrailBusinessProvider;

        [ExcludeFromCodeCoverage]
        public MonitoringBusinessProvider(
            IMonitoringDataProvider dataProvider,
            IOptions<EnvironmentSetting> environmentSetting,
            IOrganizationDataProvider dataProviderOrg,
            ILogger<MonitoringBusinessProvider> logger,
            INotificationServiceBusinessProvider notification,
            IBioHRIntegrationBussinesProviders dataProviderBioHR,
            IDigitalSignatureDataProvider digitalSignatureDataProvider,
            IQcSamplingBusinessProvider qcSamplingBusinessProvider,
            IQcTestBusinessProvider qcTestBusinessProvider,
            IQcTestDataProvider qcTestDataProvider,
            IQcSamplingDataProvider qcSamplingDataProvider,
            IBioHRIntegrationBussinesProviders bioHRIntegrationBussinesProviders,
            IQcRequestDataProvider qcRequestQcDataProvider,
            IWorkflowQcSamplingDataProvider workflowSamplingDataProviders,
            IWorkflowQcTransactionGroupDataProvider workflowQcTransactionGroupDataProviders,
            IWorkflowServiceDataProvider workflowServiceDataProviders,
            IAUAMServiceBusinessProviders aUAMServiceBusinessProviders,
            IReviewDataProvider reviewdataProvider,
            IAuditTrailBusinessProvider auditTrailBusinessProvider
            )
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _environmentSetting = environmentSetting.Value;
            _dataProviderOrg = dataProviderOrg ?? throw new ArgumentNullException(nameof(dataProviderOrg));
            _logger = logger;
            _notification = notification;
            _dataProviderBioHR = dataProviderBioHR;
            _digitalSignatureDataProvider = digitalSignatureDataProvider ?? throw new ArgumentNullException(nameof(digitalSignatureDataProvider));
            _qcSamplingBusinessProvider = qcSamplingBusinessProvider ?? throw new ArgumentNullException(nameof(qcSamplingBusinessProvider));
            _qcTestBusinessProvider = qcTestBusinessProvider ?? throw new ArgumentNullException(nameof(qcTestBusinessProvider));
            _qcTestDataProvider = qcTestDataProvider ?? throw new ArgumentNullException(nameof(qcTestDataProvider));
            _qcSamplingDataProvider = qcSamplingDataProvider ?? throw new ArgumentNullException(nameof(qcSamplingDataProvider));
            _qcRequestQcDataProvider = qcRequestQcDataProvider ?? throw new ArgumentNullException(nameof(qcRequestQcDataProvider));
            _bioHRIntegrationBussinesProviders = bioHRIntegrationBussinesProviders ?? throw new ArgumentNullException(nameof(bioHRIntegrationBussinesProviders));
            _workflowSamplingDataProviders = workflowSamplingDataProviders ?? throw new ArgumentNullException(nameof(workflowSamplingDataProviders));
            _workflowQcTransactionGroupDataProviders = workflowQcTransactionGroupDataProviders ?? throw new ArgumentNullException(nameof(workflowQcTransactionGroupDataProviders));
            _workflowServiceDataProviders = workflowServiceDataProviders ?? throw new ArgumentNullException(nameof(workflowServiceDataProviders));
            _auamServiceBusinessProviders = aUAMServiceBusinessProviders ?? throw new ArgumentNullException(nameof(aUAMServiceBusinessProviders));
            _reviewdataProvider = reviewdataProvider ?? throw new ArgumentNullException(nameof(reviewdataProvider));
            _auditTrailBusinessProvider = auditTrailBusinessProvider ?? throw new ArgumentNullException(nameof(auditTrailBusinessProvider));
        }

        public async Task<ResponseViewModel<MonitoringRelationViewModel>> GetRequestQcById(Int32 requestQcId, string? nik)
        {
            ResponseViewModel<MonitoringRelationViewModel> result = new ResponseViewModel<MonitoringRelationViewModel>();
            _logger.LogInformation($"getData: {requestQcId}");
            var getData = await _dataProvider.GetRequestQcById(requestQcId, nik);
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

        public async Task<ResponseViewModel<MonitoringListViewModel>> ListShort(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, string typeRequestId, int orgId, string nik, int? facilityId)
        {
            var statusFilter = new List<int>();
            var typeRequestFilter = new List<int>();
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

            if (typeRequestId == null)
            {
                typeRequestFilter.Add(ApplicationConstant.REQUEST_TYPE_PRODUCT);
                typeRequestFilter.Add(ApplicationConstant.REQUEST_TYPE_EM);
                typeRequestFilter.Add(ApplicationConstant.REQUEST_TYPE_WP);
                typeRequestFilter.Add(ApplicationConstant.REQUEST_TYPE_RM);
            }
            else
            {
                // filter type request from param status is string
                typeRequestFilter = typeRequestId.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<MonitoringListViewModel> result = new ResponseViewModel<MonitoringListViewModel>();

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    result.StatusCode = 500;
                    result.Message = ApplicationConstant.END_DATE_LESS_THAN_BEGIN_DATE_ERROR_MESSAGE;

                    return result;
                }
            }

            List<MonitoringListViewModel> getData = await _dataProvider.ListShort(search, limit, pagination.CalculateOffset(), startDate, endDate, statusFilter, typeRequestFilter, orgId, nik, facilityId);

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

        public async Task<ResponseViewModel<WorkflowSubmitBindingModel>> InsertReject(InsertRejectToDoBindingModel data)
        {
            ResponseViewModel<WorkflowSubmitBindingModel> result = new ResponseViewModel<WorkflowSubmitBindingModel>();

            var getDigitalSignature = await _digitalSignatureDataProvider.Authenticate(data.DigitalSignature, data.NIK);
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
                InsertApprovalBindingModel insertApproval = new InsertApprovalBindingModel
                {
                    DataId = data.Id,
                    Notes = data.Notes,
                    DigitalSignature = data.DigitalSignature,
                    NIK = data.NIK,
                    IsApprove = false
                };
                List<string> listWorkflowDocumentCode = new List<string>();

                //di ubah jadi if 
                if (data.SamplingType == ApplicationConstant.APPROVAL_DATA_TYPE_SAMPLING)
                {
                    //dibuat untuk jika di reject oleh QA dan data tersebut sudah Complete
                    //cari worfklow data sampling berdasarkan ID nya,
                    var totWorkflowExist = await _workflowSamplingDataProviders.CountWorkflowNotCompleteBySamplingIdPhase2(data.Id);
                    if (totWorkflowExist == 0)
                    {
                        //jika data tersebut sudah complete maka lakukan hal berikut : inisial workflow baru -> submit -> lalu reject QA
                        InsertProcessQARejectWhenCompleteModel dataReject = new InsertProcessQARejectWhenCompleteModel()
                        {
                            SamplingId = data.Id,
                            NIKUserQA = data.NIK,
                            Notes = data.Notes
                        };
                        var insertReject = await _ProcessRejectCompleteByQA(dataReject);
                        result.StatusCode = insertReject.StatusCode;
                        result.Message = insertReject.Message;
                    }
                    else
                    {
                        //jika data tersebut belum complete: lakukan hal di bawah ini: 
                        result = await _qcSamplingBusinessProvider.InsertApproval(insertApproval);

                    }
                    var samplingData = await _qcSamplingDataProvider.GetById(data.Id);
                    var workflowSampling = await _workflowSamplingDataProviders.GetByWorkflowByQcSamplingId(data.Id);

                    // Get current workflow sampling data
                    var currentWorkflowSamplng = await _workflowSamplingDataProviders.GetByWorkflowByQcSamplingIdLatest(data.Id);

                    foreach (var itemWorkflowSampling in workflowSampling)
                    {
                        listWorkflowDocumentCode.Add(itemWorkflowSampling.WorkflowDocumentCode);
                    }

                    /** Send self notification reject to QA when QA reject sampling EM */
                    if ((currentWorkflowSamplng.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_QA_NAME || currentWorkflowSamplng.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT) && currentWorkflowSamplng.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2)
                    {
                        await SendNotifQA(listWorkflowDocumentCode, samplingData.RequestQcsId, data.NIK, ApplicationConstant.REJECTED_ACTION_SELF_NOTIF_TO_QA, samplingData.SamplingTypeName);
                    }

                    await SendNotifOperatorSampling(listWorkflowDocumentCode, samplingData.RequestQcsId, data.NIK, ApplicationConstant.REJECTED_ACTION_SAMPLING_NOTIF, samplingData.SamplingTypeName);
                }
                else if (data.SamplingType == ApplicationConstant.APPROVAL_DATA_TYPE_TESTING)
                {
                    result = await _qcTestBusinessProvider.InsertApproval(insertApproval);
                    var testingData = await _qcTestDataProvider.GetQcRequestByTransactionGroupId(data.Id);
                    var workflowQcTransactionGroup = await _workflowQcTransactionGroupDataProviders.GetByWorkflowByQcTransactionGroupIdIsInWorkflowAlt(data.Id);
                    foreach (var itemWorkflowQcTransactionGroup in workflowQcTransactionGroup)
                    {
                        listWorkflowDocumentCode.Add(itemWorkflowQcTransactionGroup.WorkflowDocumentCode);
                    }
                    await SendNotif(listWorkflowDocumentCode, testingData.RequestQcId, data.NIK, ApplicationConstant.REJECTED_ACTION_TESTING_NOTIF, testingData.TestType, testingData.CodeTest);
                }

            }

            return result;
        }

        public async Task<ResponseViewModel<WorkflowSubmitBindingModel>> InsertApprove(InsertApproveToDoBindingModel data)
        {
            ResponseViewModel<WorkflowSubmitBindingModel> result = new ResponseViewModel<WorkflowSubmitBindingModel>();
            ResponseViewModel<WorkflowSubmitBindingModel> resultFromSampling = new ResponseViewModel<WorkflowSubmitBindingModel>();
            ResponseViewModel<WorkflowSubmitBindingModel> resultFromTesting = new ResponseViewModel<WorkflowSubmitBindingModel>();

            var getDigitalSignature = await _digitalSignatureDataProvider.Authenticate(data.DigitalSignature, data.NIK);
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
                RequestQcs requestQcs = await _qcRequestQcDataProvider.GetById(data.QcRequestId);

                List<QcResult> qcResults = await _qcRequestQcDataProvider.findRequestNotAllPass(data.QcRequestId);

                //diubah terlebih dahulu
                //if (((requestQcs.NoDeviation == "" || requestQcs.NoDeviation == null) && requestQcs.Conclusion == "Tidak Memenuhi Syarat") || ((requestQcs.NoDeviation == "" || requestQcs.NoDeviation == null) && qcResults.Any()))

                if ((requestQcs.NoDeviation == "" || requestQcs.NoDeviation == null) && requestQcs.Conclusion == "Tidak Memenuhi Syarat")
                {
                    result.StatusCode = 403;
                    result.Message = ApplicationConstant.WORKFLOW_FAILED_DEV_NUMBER_MESSAGE;
                }
                else
                {
                    List<QcSampling> listQcSampling = await _qcSamplingDataProvider.GetSamplingByRequestIdOnPending(data.QcRequestId, data.NIK);
                    List<string> listWorkflowDocumentCode = new List<string>();
                    if (listQcSampling.Any())
                    {
                        // operatorSampling = listQcSampling.FirstOrDefault().CreatedBy;
                        foreach (var item in listQcSampling)
                        {
                            InsertApprovalBindingModel insertApprovalSampling = new InsertApprovalBindingModel
                            {
                                DataId = item.Id,
                                Notes = data.Notes,
                                DigitalSignature = data.DigitalSignature,
                                NIK = data.NIK,
                                IsApprove = true
                            };
                            resultFromSampling = await _qcSamplingBusinessProvider.InsertApproval(insertApprovalSampling);

                            if (resultFromSampling.StatusCode != 200)
                            {
                                return resultFromSampling;
                            }
                            else
                            {
                                //get workflow document code
                                var workflowSampling = await _workflowSamplingDataProviders.GetByWorkflowByQcSamplingId(item.Id);
                                // if (workflowSampling.FirstOrDefault(x => x.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_KABAG_NAME || x.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_QA_NAME) == null)
                                // {
                                foreach (var itemWorkflowSampling in workflowSampling)
                                {
                                    listWorkflowDocumentCode.Add(itemWorkflowSampling.WorkflowDocumentCode);
                                }

                                // }
                                result = resultFromSampling;
                            }
                        }
                    }

                    // Approve transaction group
                    List<int> listQcTransactionSample = await _qcTestDataProvider.GetByRequestIdOnPending(data.QcRequestId, data.NIK);

                    if (listQcTransactionSample.Count() != 0)
                    {
                        foreach (var item in listQcTransactionSample.GroupBy(x => x))
                        {
                            var sameWorkflowStatusSampling = await _qcTestDataProvider.GetSameSamplingById(item.Key);
                            // if (sameWorkflowStatusSampling.Count < 1)
                            // {
                            InsertApprovalBindingModel insertApprovalTransactionGroup = new InsertApprovalBindingModel
                            {
                                DataId = item.Key,
                                Notes = data.Notes,
                                DigitalSignature = data.DigitalSignature,
                                NIK = data.NIK,
                                IsApprove = true
                            };
                            resultFromTesting = await _qcTestBusinessProvider.InsertApproval(insertApprovalTransactionGroup);

                            if (resultFromTesting.StatusCode != 200)
                            {
                                return resultFromTesting;
                            }
                            else
                            {
                                var workflowQcTransactionGroup = await _workflowQcTransactionGroupDataProviders.GetByWorkflowByQcTransactionGroupIdIsInWorkflowAlt(item.Key);
                                // if (workflowQcTransactionGroup.FirstOrDefault(x => x.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_KABAG_NAME || x.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_QA_NAME) == null)
                                // {
                                foreach (var itemWorkflowQcTransactionGroup in workflowQcTransactionGroup)
                                {
                                    listWorkflowDocumentCode.Add(itemWorkflowQcTransactionGroup.WorkflowDocumentCode);
                                }
                                // }
                                result = resultFromTesting;
                            }
                            // }
                        }
                    }

                    // var testData = await _qcTestDataProvider.GetTestDataByRequestId(data.QcRequestId);

                    //await SendNotif(data.QcRequestId, data.NIK, true, receivers);

                    // List<string> listCurrentSampling = new List<string>();
                    var qcSamplingData = await _qcSamplingDataProvider.GetByRequestId(data.QcRequestId);
                    // foreach (var item in qcSamplingData)
                    // {
                    //     listCurrentSampling.Add(item.WorkflowStatus);
                    // }

                    // Check complete sampling amount
                    int countCompleteSampling = 0;
                    foreach (var item in qcSamplingData)
                    {
                        if ((item.SamplingTypeId == ApplicationConstant.SAMPLING_TYPE_ID_EMM || item.SamplingTypeId == ApplicationConstant.SAMPLING_TYPE_ID_EMP) && item.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME)
                        {
                            countCompleteSampling++;
                        }
                    }

                    /** Send notification after approve result By Kabag Pemfas/QA */
                    if (countCompleteSampling < 2)
                    {
                        await SendNotifQA(listWorkflowDocumentCode, data.QcRequestId, data.NIK, ApplicationConstant.APPROVED_ACTION_NOTIF_RESULT_TO_QA, "");
                        await SendNotifOperatorSampling(listWorkflowDocumentCode, data.QcRequestId, data.NIK, ApplicationConstant.APPROVED_ACTION_NOTIF_RESULT_TO_OPERATOR_SAMPLING, "");
                    }

                    /** Send self notification to QA after approve result By QA */
                    if (countCompleteSampling >= 2)
                    {
                        await SendNotifQA(listWorkflowDocumentCode, data.QcRequestId, data.NIK, ApplicationConstant.APPROVED_ACTION_SELF_NOTIF_TO_QA, "");
                    }

                    // if (operatorSampling != null)
                    // {
                    //     await SendNotifOperatorSampling(listWorkflowDocumentCode, data.QcRequestId, operatorSampling, ApplicationConstant.APPROVED_ACTION_NOTIF, "", "");
                    // }
                }

                //resultFromTetsing= await _qcSamplingBusinessProvider.InsertApprovalToDo(insertApproval);

            }

            return result;
        }

        public async Task<ResponseViewModel<MonitoringResultViewModel>> GetResult(Int32 requestQcId)
        {
            ResponseViewModel<MonitoringResultViewModel> result = new ResponseViewModel<MonitoringResultViewModel>();
            _logger.LogInformation($"getData: {requestQcId}");
            var getData = await _dataProvider.GetResult(requestQcId);
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

        public async Task<ResponseViewModel<InsertEditDev>> UpdateDeviation(InsertEditDev data)
        {
            ResponseViewModel<InsertEditDev> result = await _qcRequestQcDataProvider.UpdateDeviation(data);
            return result;
        }

        public async Task<ResponseViewModel<InsertEditDev>> UpdateConclusion(InsertEditConclusion data)
        {
            ResponseViewModel<InsertEditDev> result = new ResponseViewModel<InsertEditDev>();
            var res = await _qcRequestQcDataProvider.UpdateConclusion(data);

            if (!res)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.MESSAGE_EDIT_CONC_NOT_SUCCESS;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.MESSAGE_EDIT_CONC_SUCCESS;
            }
            return result;
        }

        public async Task<ResponseViewModel<MonitoringListViewModel>> ListReportQa(string search, int limit, int page, string nik, int? facilityId)
        {
            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<MonitoringListViewModel> result = new ResponseViewModel<MonitoringListViewModel>();

            List<MonitoringListViewModel> getData = await _dataProvider.ListReportQa(search, limit, pagination.CalculateOffset(), nik, facilityId);

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

        public async Task<ResponseViewModel<MonitoringListViewModel>> ListReportQa2(string search, int limit, int page, string nik, int? facilityId)
        {
            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<MonitoringListViewModel> result = new ResponseViewModel<MonitoringListViewModel>();

            List<MonitoringListViewModel> getData = await _dataProvider.ListReportQa2(search, limit, pagination.CalculateOffset(), nik, facilityId);

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

        private async Task<bool> SendNotif(List<string> listWorkflowDocumentCode, int requestQcId, string nik, string isApprove, string? testType, string? testCode)
        {
            //notification message init
            //notif personel
            var personal = await _qcRequestQcDataProvider.GetPersonalById(2);//TODO 1 pindah ke constant
            var requestQc = await _qcRequestQcDataProvider.GetById(requestQcId);

            //get request purposes
            string purposesMsg = "-";
            var nameAudit = "No Name";
            var NoHandphoneAudit = "";
            var EmailAudit = "";
            var sendPicRole = "";
            var PicEmailAudit = "";
            var PicNameAudit = "No Name";
            var PicPostionName = "";
            var purposesDatas = await _qcRequestQcDataProvider.getRequestPurposeNames(requestQcId);
            if (purposesDatas.Any())
            {
                purposesMsg = string.Join(", ", purposesDatas.ToArray());
            }

            //get info Pic Approved
            var getDetailPICBioHR = await _bioHRIntegrationBussinesProviders.GetEmployeeByNewNik(nik);
            if (getDetailPICBioHR != null)
            {
                PicNameAudit = getDetailPICBioHR.Name;
                PicEmailAudit = getDetailPICBioHR.Email;
                PicPostionName = getDetailPICBioHR.PositionName;
            }

            //get receiver 
            List<ReceiverNotifModel> receivers = await ListReceiverNotif(listWorkflowDocumentCode, nik);
            foreach (var item in receivers)
            {
                MessageNotificationMonitoringViewModel settingMessage = new MessageNotificationMonitoringViewModel(
                    requestQc.NoRequest,
                    requestQc.NoBatch,
                    requestQc.ItemName,
                    purposesMsg,
                    // personal.Email,
                    item.ReceiverEmail,
                    isApprove,
                    item.ReceiverName,
                    // personal.NoHandphone,
                    item.ReceiverNoHp,
                    // PicPostionName,
                    PicNameAudit,
                    testType,
                    testCode,
                    sendPicRole
                );
                // if (_environmentSetting.EnvironmentName == ApplicationConstant.SOFTLIVE_ENVIRONMENT_NAME)
                // {
                //     settingMessage.EmailAddress = item.ReceiverEmail;
                //     settingMessage.NoHandphone = item.ReceiverNoHp;
                // }

                if (_environmentSetting.EnvironmentName == ApplicationConstant.DEVELOPMENT_ENVIRONMENT_NAME || _environmentSetting.EnvironmentName == ApplicationConstant.TESTING_ENVIRONMENT_NAME)
                {
                    settingMessage.EmailAddress = personal.Email;
                    settingMessage.NoHandphone = personal.NoHandphone;
                }

                await _notification.SendEmailNotifMonitoring(settingMessage);
                await _notification.SendWhatsAppNotifMonitoring(settingMessage);
            }

            return true;
        }

        private async Task<List<ReceiverNotifModel>> ListReceiverNotif(List<string> workflowDocumetnCode, string nikReviewer)
        {
            List<ReceiverNotifModel> result = new List<ReceiverNotifModel>();
            List<string> listNik = new List<string>();
            //get workflow history for get nik
            foreach (var itemWorkflowDocumentCode in workflowDocumetnCode)
            {
                DocumentHistoryResponseViewModel workflowHistory = await _workflowServiceDataProviders.GetListHistoryWorkflow(itemWorkflowDocumentCode);
                foreach (var itemPIC in workflowHistory.History)
                {
                    foreach (var item in itemPIC.PICs)
                    {
                        listNik.Add(item.OrgId);
                    }
                }
            }

            //get info by nik 
            foreach (var nik in listNik.Distinct())
            {
                if (nik != nikReviewer)
                {
                    var getDetailPersonelBioHR = await _bioHRIntegrationBussinesProviders.GetEmployeeByNewNik(nik);


                    if (getDetailPersonelBioHR != null)
                    {
                        var getPICBioHR = await _bioHRIntegrationBussinesProviders.GetEmployeeByNik(getDetailPersonelBioHR.UserId);
                        ReceiverNotifModel obj = new ReceiverNotifModel()
                        {
                            ReceiverName = getDetailPersonelBioHR.Name,
                            ReceiverEmail = getDetailPersonelBioHR.Email,
                            ReceiverNoHp = getPICBioHR.Telepon
                        };
                        result.Add(obj);
                    }
                    else
                    {
                        var getDetailPICAUAM = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(nik);
                        if (getDetailPICAUAM != null)
                        {
                            ReceiverNotifModel rev = new ReceiverNotifModel()
                            {
                                ReceiverName = getDetailPICAUAM.Name,
                                ReceiverEmail = getDetailPICAUAM.Email,
                                ReceiverNoHp = getDetailPICAUAM.NoTelp,
                            };
                            result.Add(rev);
                        }

                    }
                }
            }
            return result;
        }

        private async Task<ResponseInsertReview> _ProcessRejectCompleteByQA(InsertProcessQARejectWhenCompleteModel data)
        {
            ResponseInsertReview result = new ResponseInsertReview();

            try
            {
                //1. insert data baru ke workflow qc sampling
                var workflowQcSampling = await _ProcessInsertDataToWorkflowQcSampling(data.SamplingId);
                //2. initial workflow baru
                var initialWorkflow = await _ProcessInitialNewWorkflow(workflowQcSampling.WorkflowDocumentCode, workflowQcSampling.CreatedBy);
                //3. insert approve by system (dari draft ke QA)
                var workflowActionId = await _reviewdataProvider.GetWorkflowActionId(workflowQcSampling.WorkflowDocumentCode, ApplicationConstant.WORKFLOW_ACTION_SUBMIT_REJECT_NAME);

                var nextPICOrgIdList = new List<string>();
                nextPICOrgIdList.Add(data.NIKUserQA);

                WorkflowDocumentSubmitModel insertedDocument = new WorkflowDocumentSubmitModel()
                {
                    ApplicationCode = ApplicationConstant.APP_CODE,
                    Notes = "-",
                    OrgId = workflowQcSampling.CreatedBy,
                    DocumentCode = workflowQcSampling.WorkflowDocumentCode,
                    //jika dari phase 1 ke phase 2 dan menuju kabag menggunakan 
                    NextPICOrgIdList = nextPICOrgIdList, //jika phase 2 berarti dari 
                    WorkflowActionId = workflowActionId
                };

                ResponseInsertReview submitDocument = await _reviewdataProvider.InsertReview(insertedDocument);

                //4. insert approve by system (dari QA ke draft)
                var operatorList = new List<string>();
                operatorList.Add(workflowQcSampling.CreatedBy);
                WorkflowDocumentSubmitModel submitApproval = new WorkflowDocumentSubmitModel()
                {
                    ApplicationCode = ApplicationConstant.APP_CODE,
                    DocumentCode = workflowQcSampling.WorkflowDocumentCode,
                    Notes = data.Notes == null ? "-" : data.Notes,
                    OrgId = data.NIKUserQA,
                    WorkflowActionId = await _reviewdataProvider.GetWorkflowActionId(workflowQcSampling.WorkflowDocumentCode, ApplicationConstant.WORKFLOW_ACTION_REJECT_COMPLETE_NAME),
                    NextPICOrgIdList = operatorList
                };

                var submitAction = await _workflowServiceDataProviders.SubmitAction(submitApproval);
                if (submitAction.StatusCode == 200)
                {
                    //5. update data qc sampling
                    UpdateQcSamplingFromApproval qcSamplingFromApproval = new UpdateQcSamplingFromApproval()
                    {
                        SamplingId = data.SamplingId,
                        WorkflowStatus = submitAction.WorkflowStatus,
                        Status = ApplicationConstant.STATUS_REJECT,
                        RowStatus = ApplicationConstant.WORKFLOW_ACTION_REJECT_COMPLETE_NAME,
                        UpdatedBy = data.NIKUserQA
                    };

                    await _qcSamplingDataProvider.UpdateQcSamplingDataFromApproval(qcSamplingFromApproval);

                    var qcSampling = await _qcSamplingDataProvider.GetById(data.SamplingId);
                    var auditOperation = ApplicationConstant.QS_SAMPLING_STATUS_LABEL_REJECT;
                    var usernameModifier = await _auditTrailBusinessProvider.GetUsernameByNik(qcSampling.UpdatedBy);
                    await _auditTrailBusinessProvider.Add(auditOperation, qcSampling.Code, qcSampling, usernameModifier);

                    result.StatusCode = submitAction.StatusCode;
                    result.WorkflowStatus = submitAction.WorkflowStatus;
                    result.Message = ApplicationConstant.WORKFLOW_SUCCESS_REJECT_MESSAGE;
                }
                else
                {
                    result.StatusCode = submitAction.StatusCode;
                    result.Message = submitAction.Message;
                }

            }
            catch (Exception e)
            {
                result.StatusCode = 400;
                result.Message = e.Message;
                throw;
            }

            return result;
        }

        private async Task<WorkflowQcSampling> _ProcessInsertDataToWorkflowQcSampling(int samplingId)
        {
            WorkflowQcSampling result = new WorkflowQcSampling();

            try
            {
                var getSamplingQc = await _qcSamplingDataProvider.GetById(samplingId);
                var getLastDataWorkflowQcSampling = await _workflowSamplingDataProviders.GetByWorkflowByQcSamplingIdLatest(samplingId);
                var sequenceWorklflowQcData = getLastDataWorkflowQcSampling.Id + 1;
                //insert workflow sampling

                WorkflowQcSampling insertQcSampling = new WorkflowQcSampling()
                {
                    QcSamplingId = getSamplingQc.Id,
                    WorkflowStatus = "Draft",
                    WorkflowDocumentCode = getSamplingQc.SamplingTypeName == ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE ? ApplicationConstant.PREFIX_EM_PC_WORKFLOW_CODE + "-" + getSamplingQc.Code + "-" + sequenceWorklflowQcData : ApplicationConstant.PREFIX_EM_M_WORKFLOW_CODE + "-" + getSamplingQc.Code + "-" + sequenceWorklflowQcData,
                    WorkflowCode = ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2,
                    IsInWorkflow = true,
                    CreatedBy = getLastDataWorkflowQcSampling.CreatedBy,
                    CreatedAt = DateHelper.Now(),
                    UpdatedBy = getLastDataWorkflowQcSampling.CreatedBy,
                    UpdatedAt = DateHelper.Now()
                };

                await _workflowSamplingDataProviders.Insert(insertQcSampling);
                result = insertQcSampling;
            }
            catch (System.Exception)
            {
                throw;
            }


            return result;
        }

        private async Task<ResponseInsertReview> _ProcessInitialNewWorkflow(string workflowCodeQcSampling, string nikOperatorSampling)
        {
            ResponseInsertReview result = new ResponseInsertReview();
            //buat document code baru
            //ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2
            //initial new workflow document
            NewWorkflowDocument newWorkflowDocument = new NewWorkflowDocument()
            {
                DocumentCode = workflowCodeQcSampling,
                ApplicationCode = ApplicationConstant.APP_CODE,
                WorkflowCode = ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2,
                Description = ApplicationConstant.WORKFLOW_INITIAL_DESC,
                CreatedBy = nikOperatorSampling,
                CreatorOrgId = nikOperatorSampling
            };

            ResponseInsertReview submitDocument = await _reviewdataProvider.InitialDoc(newWorkflowDocument);

            return submitDocument;
        }


        /** Method for send notification Result to QA */
        private async Task<bool> SendNotifQA(List<string> listWorkflowDocumentCode, int requestQcId, string nik, string isApprove, string? testType)
        {
            //notification message init
            //notif personel
            var personal = await _qcRequestQcDataProvider.GetPersonalById(2);//TODO 1 pindah ke constant
            var requestQc = await _qcRequestQcDataProvider.GetById(requestQcId);
            var testDataQc = await _qcTestDataProvider.GetTestDataByRequestId(requestQcId);

            //get request purposes
            string purposesMsg = "-";
            var nameAudit = "No Name";
            var NoHandphoneAudit = "";
            var EmailAudit = "";
            var sendPicRole = "";
            var PicEmailAudit = "";
            var PicNameAudit = "No Name";
            var PicPostionName = "";
            var purposesDatas = await _qcRequestQcDataProvider.getRequestPurposeNames(requestQcId);
            if (purposesDatas.Any())
            {
                purposesMsg = string.Join(", ", purposesDatas.ToArray());
            }

            //get info Pic Approved
            var getDetailPICBioHR = await _bioHRIntegrationBussinesProviders.GetEmployeeByNewNik(nik);
            if (getDetailPICBioHR != null)
            {
                PicNameAudit = getDetailPICBioHR.Name;
                PicEmailAudit = getDetailPICBioHR.Email;
                PicPostionName = getDetailPICBioHR.PositionName;
            }

            //get receiver 
            List<ReceiverNotifModel> receivers = await ListReceiverNotifQA(listWorkflowDocumentCode, nik);
            foreach (var item in receivers)
            {
                MessageNotificationMonitoringViewModel settingMessage = new MessageNotificationMonitoringViewModel(
                    requestQc.NoRequest,
                    requestQc.NoBatch,
                    requestQc.ItemName,
                    purposesMsg,
                    // personal.Email,
                    item.ReceiverEmail,
                    isApprove,
                    item.ReceiverName,
                    // personal.NoHandphone,
                    item.ReceiverNoHp,
                    // PicPostionName,
                    PicNameAudit,
                    testType,
                    testDataQc != null ? testDataQc.Code : "",
                    sendPicRole
                );
                // if (_environmentSetting.EnvironmentName == ApplicationConstant.SOFTLIVE_ENVIRONMENT_NAME)
                // {
                //     settingMessage.EmailAddress = item.ReceiverEmail;
                //     settingMessage.NoHandphone = item.ReceiverNoHp;
                // }

                if (_environmentSetting.EnvironmentName == ApplicationConstant.DEVELOPMENT_ENVIRONMENT_NAME || _environmentSetting.EnvironmentName == ApplicationConstant.TESTING_ENVIRONMENT_NAME)
                {
                    settingMessage.EmailAddress = personal.Email;
                    settingMessage.NoHandphone = personal.NoHandphone;
                }

                await _notification.SendEmailNotifMonitoring(settingMessage);
                await _notification.SendWhatsAppNotifMonitoring(settingMessage);
            }

            return true;
        }


        /** Method for get data QA by nik */
        private async Task<List<ReceiverNotifModel>> ListReceiverNotifQA(List<string> workflowDocumetnCode, string nikReviewer)
        {
            List<ReceiverNotifModel> result = new List<ReceiverNotifModel>();
            List<string> listNik = new List<string>();
            //get workflow history for get nik
            foreach (var itemWorkflowDocumentCode in workflowDocumetnCode)
            {
                DocumentHistoryResponseViewModel workflowHistory = await _workflowServiceDataProviders.GetListHistoryWorkflow(itemWorkflowDocumentCode);
                foreach (var itemPIC in workflowHistory.History)
                {
                    if (itemPIC.StatusName == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA)
                    {
                        foreach (var item in itemPIC.PICs)
                        {
                            listNik.Add(item.OrgId);
                        }
                    }
                }
            }

            //get info by nik 
            foreach (var nik in listNik.Distinct())
            {
                var getDetailPersonelBioHR = await _bioHRIntegrationBussinesProviders.GetEmployeeByNewNik(nik);


                if (getDetailPersonelBioHR != null)
                {
                    var getPICBioHR = await _bioHRIntegrationBussinesProviders.GetEmployeeByNik(getDetailPersonelBioHR.UserId);
                    ReceiverNotifModel obj = new ReceiverNotifModel()
                    {
                        ReceiverName = getDetailPersonelBioHR.Name,
                        ReceiverEmail = getDetailPersonelBioHR.Email,
                        ReceiverNoHp = getPICBioHR.Telepon
                    };
                    result.Add(obj);
                }
                else
                {
                    var getDetailPICAUAM = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(nik);
                    if (getDetailPICAUAM != null)
                    {
                        ReceiverNotifModel rev = new ReceiverNotifModel()
                        {
                            ReceiverName = getDetailPICAUAM.Name,
                            ReceiverEmail = getDetailPICAUAM.Email,
                            ReceiverNoHp = getDetailPICAUAM.NoTelp,
                        };
                        result.Add(rev);
                    }

                }
            }
            return result;
        }


        /** Method for Send Notification result to Operator Sampling */
        private async Task<bool> SendNotifOperatorSampling(List<string> listWorkflowDocumentCode, int requestQcId, string nik, string isApprove, string? testType)
        {
            //notification message init
            //notif personel
            var personal = await _qcRequestQcDataProvider.GetPersonalById(2);//TODO 1 pindah ke constant
            var requestQc = await _qcRequestQcDataProvider.GetById(requestQcId);
            var samplingQc = await _workflowSamplingDataProviders.GetWorkflowByRequestId(requestQcId);
            var testDataQc = await _qcTestDataProvider.GetTestDataByRequestId(requestQcId);

            //get request purposes
            string purposesMsg = "-";
            var nameAudit = "No Name";
            var NoHandphoneAudit = "";
            var EmailAudit = "";
            var sendPicRole = "";
            var PicEmailAudit = "";
            var PicNameAudit = "No Name";
            var PicPostionName = "";
            var purposesDatas = await _qcRequestQcDataProvider.getRequestPurposeNames(requestQcId);
            if (purposesDatas.Any())
            {
                purposesMsg = string.Join(", ", purposesDatas.ToArray());
            }

            //get info Pic Approved
            var getDetailPICBioHR = await _bioHRIntegrationBussinesProviders.GetEmployeeByNewNik(nik);
            if (getDetailPICBioHR != null)
            {
                PicNameAudit = getDetailPICBioHR.Name;
                PicEmailAudit = getDetailPICBioHR.Email;
                PicPostionName = getDetailPICBioHR.PositionName;
            }

            //get receiver 
            var nikOperator = samplingQc.CreatedBy;
            List<ReceiverNotifModel> receivers = await ListReceiverNotifOperatorSampling(listWorkflowDocumentCode, nik, nikOperator);
            foreach (var item in receivers)
            {
                MessageNotificationMonitoringViewModel settingMessage = new MessageNotificationMonitoringViewModel(
                    requestQc.NoRequest,
                    requestQc.NoBatch,
                    requestQc.ItemName,
                    purposesMsg,
                    // personal.Email,
                    item.ReceiverEmail,
                    isApprove,
                    item.ReceiverName,
                    // personal.NoHandphone,
                    item.ReceiverNoHp,
                    // PicPostionName,
                    PicNameAudit,
                    testType,
                    testDataQc != null ? testDataQc.Code : "",
                    sendPicRole
                );
                // if (_environmentSetting.EnvironmentName == ApplicationConstant.SOFTLIVE_ENVIRONMENT_NAME)
                // {
                //     settingMessage.EmailAddress = item.ReceiverEmail;
                //     settingMessage.NoHandphone = item.ReceiverNoHp;
                // }

                if (_environmentSetting.EnvironmentName == ApplicationConstant.DEVELOPMENT_ENVIRONMENT_NAME || _environmentSetting.EnvironmentName == ApplicationConstant.TESTING_ENVIRONMENT_NAME)
                {
                    settingMessage.EmailAddress = personal.Email;
                    settingMessage.NoHandphone = personal.NoHandphone;
                }

                await _notification.SendEmailNotifMonitoring(settingMessage);
                await _notification.SendWhatsAppNotifMonitoring(settingMessage);
            }

            return true;
        }


        /** Method for get data Operator Sampling by nik */
        private async Task<List<ReceiverNotifModel>> ListReceiverNotifOperatorSampling(List<string> workflowDocumetnCode, string nikReviewer, string nikOperator)
        {
            List<ReceiverNotifModel> result = new List<ReceiverNotifModel>();

            //get info by nik 
            if (nikOperator != nikReviewer)
            {
                var getDetailPersonelBioHR = await _bioHRIntegrationBussinesProviders.GetEmployeeByNewNik(nikOperator);


                if (getDetailPersonelBioHR != null)
                {
                    var getPICBioHR = await _bioHRIntegrationBussinesProviders.GetEmployeeByNik(getDetailPersonelBioHR.UserId);
                    ReceiverNotifModel obj = new ReceiverNotifModel()
                    {
                        ReceiverName = getDetailPersonelBioHR.Name,
                        ReceiverEmail = getDetailPersonelBioHR.Email,
                        ReceiverNoHp = getPICBioHR.Telepon
                    };
                    result.Add(obj);
                }
                else
                {
                    var getDetailPICAUAM = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(nikOperator);
                    if (getDetailPICAUAM != null)
                    {
                        ReceiverNotifModel rev = new ReceiverNotifModel()
                        {
                            ReceiverName = getDetailPICAUAM.Name,
                            ReceiverEmail = getDetailPICAUAM.Email,
                            ReceiverNoHp = getDetailPICAUAM.NoTelp,
                        };
                        result.Add(rev);
                    }

                }
            }
            return result;
        }

    }
}
