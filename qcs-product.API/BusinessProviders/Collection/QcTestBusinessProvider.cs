using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.API.WorkflowModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using qcs_product.API.Helpers;
using qcs_product.API.SettingModels;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class QcTestBusinessProvider : IQcTestBusinessProvider
    {
        private readonly IQcTestDataProvider _dataProvider;
        private readonly IReviewBusinessProvider _reviewBusinessProvider;
        private readonly IReviewDataProvider _reviewDataProvider;
        private readonly IQcRequestDataProvider _dataProviderRequestQc;
        private readonly IWorkflowQcTransactionGroupDataProvider _workflowQcTransactionGroupDataProvider;
        private readonly IWorkflowServiceBusinessProvider _workflowServiceBusinessProvider;
        private readonly IWorkflowServiceDataProvider _workflowServiceDataProvider;
        private readonly ILogger<QcTestBusinessProvider> _logger;
        private readonly IDigitalSignatureDataProvider _digitalSignatureDataProvider;
        private readonly IBioHRIntegrationBussinesProviders _bioHRIntegrationBussinesProviders;
        private readonly QcsProductContext _context;
        private readonly IAuditTrailBusinessProvider _auditTrailBusinessProvider;
        private readonly IQcProcessDataProvider _dataProviderQcProcess;
        private readonly IQcSamplingDataProvider _dataProviderSampling;
        private readonly IAUAMServiceBusinessProviders _auamServiceBusinessProviders;
        private readonly INotificationServiceBusinessProvider _notification;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly EnvironmentSetting _environmentSetting;

        [ExcludeFromCodeCoverage]
        public QcTestBusinessProvider(
            IQcTestDataProvider dataProvider,
            IQcRequestDataProvider dataProviderRequestQc,
            IReviewBusinessProvider reviewBusinessProvider,
            IWorkflowQcTransactionGroupDataProvider workflowQcTransactionGroupDataProvider,
            IReviewDataProvider reviewDataProvider,
            IWorkflowServiceBusinessProvider workflowServiceBusinessProvider,
            IWorkflowServiceDataProvider workflowServiceDataProvider,
            IDigitalSignatureDataProvider digitalSignatureDataProvider,
            IBioHRIntegrationBussinesProviders bioHRIntegrationBussinesProviders,
            QcsProductContext context,
            ILogger<QcTestBusinessProvider> logger,
            INotificationServiceBusinessProvider notification,
            IAuditTrailBusinessProvider auditTrailBusinessProvider,
            IQcSamplingDataProvider dataProviderSampling,
            IAUAMServiceBusinessProviders auamServiceBusinessProviders,
            IQcProcessDataProvider dataProviderQcProcess,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<EnvironmentSetting> environmentSetting)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _dataProviderRequestQc = dataProviderRequestQc ?? throw new ArgumentNullException(nameof(dataProviderRequestQc));
            _reviewBusinessProvider = reviewBusinessProvider ?? throw new ArgumentNullException(nameof(reviewBusinessProvider));
            _workflowQcTransactionGroupDataProvider = workflowQcTransactionGroupDataProvider ?? throw new ArgumentNullException(nameof(workflowQcTransactionGroupDataProvider));
            _reviewDataProvider = reviewDataProvider ?? throw new ArgumentNullException(nameof(reviewDataProvider));
            _workflowServiceBusinessProvider = workflowServiceBusinessProvider ?? throw new ArgumentNullException(nameof(workflowServiceBusinessProvider));
            _workflowServiceDataProvider = workflowServiceDataProvider ?? throw new ArgumentNullException(nameof(workflowServiceDataProvider));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _digitalSignatureDataProvider = digitalSignatureDataProvider ?? throw new ArgumentNullException(nameof(digitalSignatureDataProvider));
            _bioHRIntegrationBussinesProviders = bioHRIntegrationBussinesProviders ?? throw new ArgumentNullException(nameof(bioHRIntegrationBussinesProviders));
            _logger = logger;
            _auditTrailBusinessProvider = auditTrailBusinessProvider;
            _notification = notification;
            _dataProviderSampling = dataProviderSampling;
            _auamServiceBusinessProviders = auamServiceBusinessProviders;
            _dataProviderQcProcess = dataProviderQcProcess;
            _serviceScopeFactory = serviceScopeFactory;
            _environmentSetting = environmentSetting.Value;
        }

        public async Task<ResponseViewModel<QcTransactionGroupViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, string orderDir)
        {
            var statusFilter = new List<int>();
            if (status == null)
            {
                statusFilter.Add(ApplicationConstant.STATUS_TEST_DRAFT);
                statusFilter.Add(ApplicationConstant.STATUS_TEST_READYTOTEST);
                statusFilter.Add(ApplicationConstant.STATUS_TEST_INPROGRESS);
                statusFilter.Add(ApplicationConstant.STATUS_TEST_INREVIEW_PAIRING);
                statusFilter.Add(ApplicationConstant.STATUS_TEST_INREVIEW_AHLI_MUDA_QC);
                statusFilter.Add(ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_QC);
                statusFilter.Add(ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_PRODUKSI);
                statusFilter.Add(ApplicationConstant.STATUS_TEST_INREVIEW_QA);
                statusFilter.Add(ApplicationConstant.STATUS_TEST_APPROVED);
                statusFilter.Add(ApplicationConstant.STATUS_TEST_REJECTED);
            }
            else
            {
                // filter status from param status is string
                statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<QcTransactionGroupViewModel> result = new ResponseViewModel<QcTransactionGroupViewModel>();

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.END_DATE_LESS_THAN_BEGIN_DATE_ERROR_MESSAGE;

                    return result;
                }
            }

            List<QcTransactionGroupViewModel> getData = await _dataProvider.List(search, limit, pagination.CalculateOffset(), startDate, endDate, statusFilter, orderDir);

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

        public async Task<ResponseOneDataViewModel<QcTransactionGroupDetailViewModel>> GetById(int id)
        {
            ResponseOneDataViewModel<QcTransactionGroupDetailViewModel> result = new ResponseOneDataViewModel<QcTransactionGroupDetailViewModel>();
            var getData = await _dataProvider.GetById(id);

            if (getData == null)
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

        public async Task<ResponseViewModel<SampleByQcProcessRelationViewModel>> ListSampleTest(Int32 QcProcessId, string search, Int32 TestParamId, Int32 RoomId, Int32 PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate)
        {
            ResponseViewModel<SampleByQcProcessRelationViewModel> result = new ResponseViewModel<SampleByQcProcessRelationViewModel>();

            if (QcProcessId == 0)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.ERROR_REQUIRED_PAYLOAD_FIELD + " QcProcessId " + ApplicationConstant.ERROR_REQUIRED_PAYLOAD_MSG;

                return result;
            }

            List<SampleByQcProcessRelationViewModel> getData = await _dataProvider.ListSampleTest(QcProcessId, search, TestParamId, RoomId, PhaseId, ReceiptStartDate, ReceiptEndDate);

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

        public async Task<ResponseViewModel<SampleBatchQcProcessViewModel>> ListSampleBatchTest(int QcProcessId, string search, int RoomId, int PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate)
        {
            ResponseViewModel<SampleBatchQcProcessViewModel> result = new ResponseViewModel<SampleBatchQcProcessViewModel>();

            if (QcProcessId == 0)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.ERROR_REQUIRED_PAYLOAD_FIELD + " QcProcessId " + ApplicationConstant.ERROR_REQUIRED_PAYLOAD_MSG;

                return result;
            }

            List<SampleBatchQcProcessViewModel> getData = await _dataProvider.ListSampleBatchTest(QcProcessId, search, RoomId, PhaseId, ReceiptStartDate, ReceiptEndDate);

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

        public async Task<ResponseOneDataViewModel<QcTransactionGroup>> Insert(InsertQcTestBindingModel data)
        {
            ResponseOneDataViewModel<QcTransactionGroup> result = new ResponseOneDataViewModel<QcTransactionGroup>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var getQcProcess = await _dataProviderQcProcess.GetQcProcessById(data.QcProcessId);
            if (getQcProcess == null)
            {
                result.StatusCode = 400;
                result.Message = "QcProcessId " + ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            //TODO untuk generate kode atau nomor  pindah ke service terpisah
            string month = StringHelper.GetMonthRomawi(data.TestDate);
            string year = data.TestDate.Year.ToString();
            string noTest = "/" + month + "/" + year;
            var statusTest = data.IsSubmit ? ApplicationConstant.STATUS_TEST_READYTOTEST : ApplicationConstant.STATUS_TEST_DRAFT;

            QcTransactionGroup insertData = new QcTransactionGroup()
            {
                Code = noTest,
                QcProcessId = data.QcProcessId,
                QcProcessName = data.QcProcessName,
                TestDate = Convert.ToDateTime(data.TestDate).AddHours(7),
                PersonelNik = data.PersonelNik,
                PersonelName = data.PersonelName,
                PersonelPairingNik = data.PersonelPairingNik,
                PersonelPairingName = data.PersonelPairingName,
                Status = statusTest,
                CreatedBy = data.CreatedBy,
                UpdatedBy = data.CreatedBy,
                CreatedAt = DateHelper.Now(),
                UpdatedAt = DateHelper.Now()
            };

            List<QcTransactionGroupSample> insertTransactionSample = new List<QcTransactionGroupSample>();
            List<QcTransactionGroupSampling> insertTransactionSampling = new List<QcTransactionGroupSampling>();


            if (getQcProcess.AddSampleLayoutType == ApplicationConstant.ADD_SAMPLE_TEST)
            {
                if (!data.QcProcessSample.Any())
                {
                    result.StatusCode = 400;
                    result.Message = "QcProcessSample " + ApplicationConstant.NO_CONTENT_MESSAGE;
                    return result;
                }

                insertTransactionSample.AddRange(data.QcProcessSample.Select(sampleTest => new QcTransactionGroupSample()
                {
                    QcSampleId = sampleTest.QcSampleId,
                    CreatedBy = data.CreatedBy,
                    UpdatedBy = data.CreatedBy,
                    CreatedAt = DateHelper.Now(),
                    UpdatedAt = DateHelper.Now()
                }));
            }
            else if (getQcProcess.AddSampleLayoutType == ApplicationConstant.ADD_SAMPLE_BY_BATCH_TEST)
            {
                #region add qc sampling list dan qc sample list

                if (!data.QcProcessSamplingBatch.Any())
                {
                    result.StatusCode = 400;
                    result.Message = "QcProcessSamplingBatch " + ApplicationConstant.NO_CONTENT_MESSAGE;
                    return result;
                }

                var samplingIds = data.QcProcessSamplingBatch.Select(x => x.QcSamplingId).Distinct().ToList();

                var samplingList = await (from tsm in _context.QcTransactionSamplings
                                          where tsm.RowStatus == null
                                                && samplingIds.Contains(tsm.QcSamplingId)
                                          select tsm).ToListAsync();

                foreach (var samplingTest in samplingList)
                {
                    result.StatusCode = 400;
                    result.Message = "QcSamplingId : " + samplingTest.QcSamplingId + " Already Testing ";
                    return result;
                }

                insertTransactionSampling.AddRange(data.QcProcessSamplingBatch.Select(x =>
                    new QcTransactionGroupSampling()
                    {
                        QcSamplingId = x.QcSamplingId,
                        CreatedBy = data.CreatedBy,
                        UpdatedBy = data.CreatedBy,
                        CreatedAt = DateHelper.Now(),
                        UpdatedAt = DateHelper.Now()

                    }).ToList());

                var sampleList = await (from s in _context.QcSamples
                                        join tp in _context.TestParameters on s.TestParamId equals tp.Id
                                        where s.RowStatus == null
                                              && tp.QcProcessId == data.QcProcessId
                                              && samplingIds.Contains(s.QcSamplingId)
                                        select s).ToListAsync();

                insertTransactionSample.AddRange(sampleList.Select(sd => new QcTransactionGroupSample()
                {
                    QcSamplingId = sd.QcSamplingId,
                    QcSampleId = sd.Id,
                    CreatedBy = data.CreatedBy,
                    UpdatedBy = data.CreatedBy,
                    CreatedAt = DateHelper.Now(),
                    UpdatedAt = DateHelper.Now()
                }));

                #endregion
            }

            var insertDataTest = await _dataProvider.Insert(insertData, insertTransactionSample, insertTransactionSampling);

            stopwatch.Stop();
            _logger.LogInformation("Populate and insert sampling & sample. Elapsed Time is {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

            _ = Task.Run(async () =>
            {
                await AddAuditTrail(ApplicationConstant.QC_TEST_STATUS_LABEL_CREATE, insertDataTest.Id);
            });


            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = insertDataTest;

            return result;
        }

        public async Task<ResponseOneDataViewModel<QcTransactionGroup>> Edit(EditQcTestBindingModel data)
        {
            ResponseOneDataViewModel<QcTransactionGroup> result = new ResponseOneDataViewModel<QcTransactionGroup>();

            var statusTest = data.IsSubmit ? ApplicationConstant.STATUS_TEST_READYTOTEST : ApplicationConstant.STATUS_TEST_DRAFT;

            var getDataTest = await _dataProvider.GetByIdRaw(data.Id);
            if (getDataTest == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            if (getDataTest.Status != ApplicationConstant.STATUS_TEST_DRAFT && getDataTest.Status != ApplicationConstant.STATUS_TEST_REJECTED)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.DATA_CANT_EDIT;
                return result;
            }

            //personel pairing update draft & rejected only
            if (getDataTest.Status == ApplicationConstant.STATUS_TEST_DRAFT || getDataTest.Status == ApplicationConstant.STATUS_TEST_REJECTED)
            {
                getDataTest.PersonelPairingNik = data.PersonelPairingNik;
                getDataTest.PersonelPairingName = data.PersonelPairingName;
            }

            getDataTest.QcProcessId = data.QcProcessId;
            getDataTest.QcProcessName = data.QcProcessName;
            getDataTest.TestDate = data.TestDate;
            getDataTest.Status = statusTest;
            getDataTest.UpdatedBy = data.UpdatedBy;
            getDataTest.UpdatedAt = DateHelper.Now();

            // var dataEdit = await _dataProvider.Edit(getDataTest, data.QcProcessSample, data.QcProcessSamplingBatch);
            var dataEdit = await _dataProvider.EditV2(getDataTest, data.QcProcessSample, data.QcProcessSamplingBatch);

            #region add audit trail

            var auditTrailOperation = ApplicationConstant.QC_TEST_STATUS_LABEL_DRAFT;
            if (statusTest == ApplicationConstant.STATUS_TEST_READYTOTEST)
            {
                auditTrailOperation = ApplicationConstant.QC_TEST_STATUS_LABEL_EDIT;
            }
            _ = Task.Run(async () => await AddAuditTrail(auditTrailOperation, getDataTest.Id));

            #endregion

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = dataEdit;

            return result;
        }

        public async Task<ResponseOneDataViewModel<QcTransactionGroup>> StartTest(StartQcTestBindingModel data)
        {
            ResponseOneDataViewModel<QcTransactionGroup> result = new ResponseOneDataViewModel<QcTransactionGroup>();

            var getDataTest = await _dataProvider.GetByIdRaw(data.QcTestId);
            var getDataTestSample = await _dataProvider.GetSampleTestByIdRaw(data.QcTestId);
            var requestData = await _dataProvider.GetByTransactionGroupSampling(data.QcTestId);
            if (getDataTest == null || getDataTestSample == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            if (getDataTest.Status != ApplicationConstant.STATUS_TEST_READYTOTEST &&
                getDataTest.Status != ApplicationConstant.STATUS_TEST_REJECTED &&
                getDataTest.Status != ApplicationConstant.STATUS_TEST_INPROGRESS &&
                getDataTest.Status != ApplicationConstant.STATUS_TEST_INREVIEW_PAIRING)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.DATA_CANT_EDIT;
                return result;
            }

            /*Validation check input required*/

            #region validation check input required

            if (data.IsSubmit)
            {
                bool isAttch = true;
                if (data.qcTransactionGroupProcessChild.Any()) //check data process child
                {
                    foreach (var pc in data.qcTransactionGroupProcessChild)
                    {
                        var getTransactionQcProcess = await _dataProvider.GetGroupProcessByIdRaw(pc.Id);

                        if (getTransactionQcProcess != null)
                        {
                            if (getTransactionQcProcess.QcProcessId == ApplicationConstant.PROCESS_UJI_IDENTIFIKASI)
                            {
                                if (pc.qcTransactionGroupFormProcedure.Any()) //check data form procedure
                                {
                                    foreach (var fpr in pc.qcTransactionGroupFormProcedure)
                                    {
                                        if (fpr.qcTransactionGroupFormParameter.Any()) //check form param
                                        {
                                            foreach (var gfp in fpr.qcTransactionGroupFormParameter)
                                            {
                                                if (gfp.GroupSampleValues.Any()) //check data sample value
                                                {
                                                    foreach (var gsp in gfp.GroupSampleValues)
                                                    {
                                                        if (gsp.AttchmentFile == null) //check attachment is available?
                                                        {
                                                            isAttch = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //return data if attchment false
                if (isAttch == false)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.NO_CONTENT_ATTACHMENT;
                    return result;
                }
            }

            #endregion

            if (data.IsSubmit)
            {
                if (data.PersonelPairingNik == null)
                {
                    result.StatusCode = 403;
                    result.Message = ApplicationConstant.PAIRING_OPERATOR_NIK_CANNOT_NULL;
                    return result;
                }
                if (data.UpdatedBy == null || data.UpdatedBy == "" || data.UpdatedBy.ToLower() == "string")
                {
                    result.StatusCode = 500;
                    result.Message = ApplicationConstant.GENERAL_ERROR;
                    return result;
                }
            }

            // update test qc
            var StartTest = await _dataProvider.UpdateTest(data);


            #region edit with operator
            // edit with operator

            if (data.IsSubmit)
            {

                #region save into workflow transaction group code

                WorkflowQcTransactionGroup insertQcTransactionGroup = new WorkflowQcTransactionGroup()
                {
                    QcTransactionGroupId = StartTest.Id,
                    WorkflowStatus = "Draft",
                    WorkflowDocumentCode = ApplicationConstant.PREFIX_EM_M_TESTING_WORKFLOW_CODE + "-" + StartTest.Code,
                    WorkflowCode = ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_1,
                    IsInWorkflow = true,
                    CreatedBy = data.UpdatedBy,
                    CreatedAt = DateHelper.Now(),
                    UpdatedBy = data.UpdatedBy,
                    UpdatedAt = DateHelper.Now(),
                };

                // get workflow transaction group
                var workflowQcTransactionGroupCurrent = await _workflowQcTransactionGroupDataProvider.GetByWorkflowByQcTransactionGroupIdIsInWorkflow(data.QcTestId);

                if (workflowQcTransactionGroupCurrent == null)
                {
                    await _workflowQcTransactionGroupDataProvider.Insert(insertQcTransactionGroup);

                    //insert into review business to initial workflow

                    NewWorkflowDocument newWorkflowDocument = new NewWorkflowDocument()
                    {
                        DocumentCode = insertQcTransactionGroup.WorkflowDocumentCode,
                        ApplicationCode = ApplicationConstant.APP_CODE,
                        WorkflowCode = insertQcTransactionGroup.WorkflowCode,
                        Description = ApplicationConstant.WORKFLOW_INITIAL_DESC,
                        CreatedBy = data.UpdatedBy,
                        CreatorOrgId = data.UpdatedBy
                    };

                    await _workflowServiceDataProvider.InitiateDoc(newWorkflowDocument);
                }

                var workflowQcTransactionGroup = await _workflowQcTransactionGroupDataProvider.GetByWorkflowByQcTransactionGroupIdIsInWorkflow(data.QcTestId);

                //nextPICOrgId and submit workflow
                var actionType = "";
                if ((workflowQcTransactionGroup.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_KABAG_NAME) || (workflowQcTransactionGroup.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_QA_NAME) || workflowQcTransactionGroup.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_NAME)
                {
                    actionType = ApplicationConstant.WORKFLOW_ACTION_EDIT_NAME;
                }
                else
                {
                    actionType = ApplicationConstant.WORKFLOW_ACTION_SUBMIT_NAME;
                }
                var nextPICOrgId = StartTest.PersonelPairingNik;

                WorkflowSubmitBindingModel submitDoc = new WorkflowSubmitBindingModel()
                {
                    ApplicationCode = ApplicationConstant.APP_CODE,
                    DocumentCode = workflowQcTransactionGroup.WorkflowDocumentCode,
                    Notes = "-",
                    OrgId = data.UpdatedBy,
                    ActionName = actionType,
                    NextPICOrgIdList = new List<string>() { nextPICOrgId }
                };

                ResponseViewModel<ResponseInsertReview> submitWorkflowDocument = new ResponseViewModel<ResponseInsertReview>();
                submitWorkflowDocument.StatusCode = 200;

                if (workflowQcTransactionGroup.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT)
                {
                    submitWorkflowDocument = await _workflowServiceBusinessProvider.SubmitAction(submitDoc);
                }

                #endregion

                #region send notif ke operator uji pairing

                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _serviceScopeFactory.CreateScope();

                        var bioHrService = scope.ServiceProvider.GetRequiredService<IBioHRIntegrationBussinesProviders>();
                        var auamService = scope.ServiceProvider.GetRequiredService<IAUAMServiceBusinessProviders>();
                        var notificationService =
                            scope.ServiceProvider.GetRequiredService<INotificationServiceBusinessProvider>();

                        var nameOperatorUjiPairing = getDataTest.PersonelPairingName;
                        var emailOperatorUjiPairing = "";
                        var PicNoHandphoneAudit = "";

                        var getDetailOperatorUjiPairing = await bioHrService.GetEmployeeByNewNik(getDataTest.PersonelPairingNik);
                        if (getDetailOperatorUjiPairing != null)
                        {
                            nameOperatorUjiPairing = getDetailOperatorUjiPairing.Name;
                            emailOperatorUjiPairing = getDetailOperatorUjiPairing.Email;
                            var getPICBioHR = await bioHrService.GetEmployeeByNik(getDetailOperatorUjiPairing.UserId);
                            if (getPICBioHR != null)
                            {
                                PicNoHandphoneAudit = getPICBioHR.Telepon;
                            }
                        }
                        else
                        {
                            var getDetailPICAUAM = await auamService.GetPersonalExtDetailByNik(getDataTest.PersonelPairingNik);
                            if (getDetailPICAUAM != null)
                            {
                                nameOperatorUjiPairing = getDetailPICAUAM.Name;
                                PicNoHandphoneAudit = getDetailPICAUAM.NoTelp;
                                emailOperatorUjiPairing = getDetailPICAUAM.Email;
                            }
                        }

                        var settingMessage = new MessageNotificationQcTestViewModel(
                            getDataTest.Code,
                            getDataTest.QcProcessName,
                            emailOperatorUjiPairing,
                            ApplicationConstant.NEW_ACTION_NOTIF,
                            nameOperatorUjiPairing,
                            PicNoHandphoneAudit,
                            "",
                            ApplicationConstant.MSG_TYPE_NEXT_PIC,
                            requestData.NoBatch,
                            requestData.NoRequest,
                            requestData.PurposeName
                        );

                        // await notificationService.SendEmailNotifQcTest(settingMessage);
                        // await notificationService.SendWhatsAppNotifQcTest(settingMessage);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "{Message}", e.Message);
                    }
                });

                //test wendi

                #endregion

                //update workflow transaction group
                if (submitWorkflowDocument.StatusCode == 200)
                {
                    if (workflowQcTransactionGroup.WorkflowStatus != ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_PAIRING)
                    {
                        ResponseInsertReview submitDocument = submitWorkflowDocument.Data.FirstOrDefault();
                        UpdateWorkflowQcTransactionGroupFromApproval updateWorkflowQcTransactionGroupFromApproval = new UpdateWorkflowQcTransactionGroupFromApproval()
                        {
                            TransactionGroupId = data.QcTestId,
                            WorkflowStatus = submitDocument.WorkflowStatus,
                            IsInWorkflow = submitDocument.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME ? false : true,
                            RowStatus = actionType,
                            UpdatedBy = data.UpdatedBy
                        };

                        await _workflowQcTransactionGroupDataProvider.UpdateWorkflowQcTransactionGroupDataFromApproval(updateWorkflowQcTransactionGroupFromApproval);
                    }

                    //notification edit operator  data.QcTestId = qctransactionid
                    if (actionType == ApplicationConstant.WORKFLOW_ACTION_EDIT_NAME)
                    {
                        var testingData = await _dataProvider.GetQcRequestByTransactionGroupId(data.QcTestId);
                        _sendNotif(testingData.RequestQcId, new List<string>() { nextPICOrgId }, data.UpdatedBy, ApplicationConstant.EDIT_ACTION_TESTING_NOTIF, testingData.TestType, testingData.CodeTest, "");
                    }

                    _ = Task.Run(async () =>
                        await AddAuditTrail(ApplicationConstant.QC_TEST_STATUS_LABEL_SUBMIT, getDataTest.Id));

                    result.StatusCode = 200;
                    result.Message = ApplicationConstant.OK_MESSAGE;
                    result.Data = StartTest;
                }
                else
                {
                    result.StatusCode = 403;
                    result.Message = submitWorkflowDocument.Message;
                }
            }
            else
            {
                _ = Task.Run(async () =>
                    await AddAuditTrail(ApplicationConstant.QC_TEST_STATUS_LABEL_CREATE, getDataTest.Id));


                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = StartTest;
            }
            #endregion


            return result;
        }

        public async Task<ResponseViewModel<WorkflowSubmitBindingModel>> InsertApproval(InsertApprovalBindingModel data)
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
                WorkflowQcTransactionGroup workflowQcTransactionGroup = await _workflowQcTransactionGroupDataProvider.GetByWorkflowByQcTransactionGroupIdIsInWorkflow(data.DataId);
                var currentRowStatus = workflowQcTransactionGroup.RowStatus;
                var currentStatus = workflowQcTransactionGroup.WorkflowStatus;

                // Get List Receiver Before PIC Notif
                List<string> receiverBefore = await _ListReceiverNotif(workflowQcTransactionGroup.WorkflowDocumentCode, data.NIK);

                // Get data receiver, position as Operator Uji
                List<string> receiverOperatorUji = await _ListReceiverOperatorUji(workflowQcTransactionGroup.WorkflowDocumentCode, data.NIK);

                var action = "";
                if (data.IsApprove)
                {
                    action = ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME;
                }
                else if ((!data.IsApprove) && (currentStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG))
                {
                    action = ApplicationConstant.WORKFLOW_ACTION_REJECT_KABAG_NAME;
                }
                else if ((!data.IsApprove) && (currentStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA))
                {
                    action = ApplicationConstant.WORKFLOW_ACTION_REJECT_QA_NAME;
                }
                else
                {
                    action = ApplicationConstant.WORKFLOW_ACTION_REJECT_NAME;
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        string approvalType = data.IsApprove == true ? ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME : ApplicationConstant.WORKFLOW_ACTION_REJECT_NAME;

                        QcTransactionGroup transactionGroupData = await _dataProvider.GetByIdRaw(data.DataId);
                        var requestData = await _dataProvider.GetByTransactionGroupSampling(data.DataId);
                        var purposesDatas = await _dataProviderRequestQc.getRequestPurposeNames(requestData.Id);
                        var purposesMsg = "";
                        if (purposesDatas.Any())
                        {
                            purposesMsg = string.Join(", ", purposesDatas.ToArray());
                        }

                        List<string> nextPICList = new List<string>();
                        switch (transactionGroupData.WorkflowStatus)
                        {
                            case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG_QC:
                                nextPICList = await _getNextPhasePIC(transactionGroupData.Id);
                                break;
                            case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG:
                                nextPICList = await _getQAPIC(transactionGroupData.Id);
                                break;
                            case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA:
                                nextPICList = null;
                                break;
                            default:
                                nextPICList.Add(transactionGroupData.PersonelPairingNik);
                                break;
                        }

                        if (data.Notes == "" || data.Notes == null)
                        {
                            data.Notes = "-";
                        }

                        WorkflowSubmitBindingModel submitDoc = new WorkflowSubmitBindingModel()
                        {
                            ApplicationCode = ApplicationConstant.APP_CODE,
                            DocumentCode = workflowQcTransactionGroup.WorkflowDocumentCode,
                            Notes = data.Notes,
                            OrgId = data.NIK,
                            ActionName = approvalType,
                            NextPICOrgIdList = nextPICList
                        };
                        ResponseViewModel<ResponseInsertReview> approvalDoc = await _workflowServiceBusinessProvider.SubmitAction(submitDoc);

                        if (approvalDoc.StatusCode == 200)
                        {
                            ResponseInsertReview workflowSubmitData = approvalDoc.Data.FirstOrDefault();
                            int getStatus = _getStatus(workflowSubmitData.WorkflowStatus == null ? ApplicationConstant.WORKFLOW_STATUS_NAME_COMPLETE : workflowSubmitData.WorkflowStatus);

                            UpdateQcTransactionGroupFromApproval qcTransactionGroupFromApproval = new UpdateQcTransactionGroupFromApproval()
                            {
                                TransactionGroupId = data.DataId,
                                WorkflowStatus = workflowSubmitData.WorkflowStatus == null ? ApplicationConstant.WORKFLOW_STATUS_NAME_COMPLETE : workflowSubmitData.WorkflowStatus,
                                Status =
                                data.IsApprove ? getStatus : ApplicationConstant.STATUS_TEST_REJECTED,
                                UpdatedBy = data.NIK
                            };

                            QcTransactionGroup updatedTransactionGroup = await _dataProvider.UpdateQcTransactionGroupDataFromApproval(qcTransactionGroupFromApproval);

                            UpdateWorkflowQcTransactionGroupFromApproval updateWorkflowQcTransactionGroupFromApproval = new UpdateWorkflowQcTransactionGroupFromApproval()
                            {
                                TransactionGroupId = data.DataId,
                                WorkflowStatus = updatedTransactionGroup.WorkflowStatus,
                                IsInWorkflow = true,
                                UpdatedBy = data.NIK,
                                RowStatus = action
                            };

                            WorkflowQcTransactionGroup updatedWorkflowTransactionGroup = await _workflowQcTransactionGroupDataProvider.UpdateWorkflowQcTransactionGroupDataFromApproval(updateWorkflowQcTransactionGroupFromApproval);

                            if (workflowSubmitData.WorkflowStatus == null)
                            {

                                UpdateWorkflowQcTransactionGroupFromApproval updateWorkflowQcTransactionGroupComplete = new UpdateWorkflowQcTransactionGroupFromApproval()
                                {
                                    TransactionGroupId = data.DataId,
                                    WorkflowStatus = "Complete",
                                    IsInWorkflow = false,
                                    UpdatedBy = data.NIK,
                                    RowStatus = ApplicationConstant.WORKFLOW_ACTION_APPROVE_NAME
                                };
                                WorkflowQcTransactionGroup updatedWorkflowTransactionGroupComplete = await _workflowQcTransactionGroupDataProvider.UpdateWorkflowQcTransactionGroupDataFromApproval(updateWorkflowQcTransactionGroupComplete);


                                if (updatedWorkflowTransactionGroupComplete.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_1)
                                {
                                    transactionGroupData.WorkflowStatus = ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG;
                                    transactionGroupData.Status = ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_PRODUKSI;

                                    await _InsertToNextPhase(transactionGroupData, updatedWorkflowTransactionGroupComplete);
                                }
                                await _dataProvider.UpdateRaw(transactionGroupData);
                            }

                            //notifikasi 
                            WorkflowQcTransactionGroup currentWorkflowQcTransactionGroup = await _workflowQcTransactionGroupDataProvider.GetByWorkflowByQcTransactionGroupIdLatest(data.DataId);
                            if (data.IsApprove)
                            {

                                if (currentWorkflowQcTransactionGroup.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_QA_NAME || currentWorkflowQcTransactionGroup.RowStatus == ApplicationConstant.WORKFLOW_ACTION_REJECT_KABAG_NAME)
                                {
                                    var editor = await _GetEditor(workflowQcTransactionGroup.WorkflowDocumentCode);
                                    var listNextPIC = await _ListNextPIC(workflowQcTransactionGroup.WorkflowDocumentCode);
                                    if (!string.IsNullOrEmpty(editor))
                                    {
                                        var testingData = await _dataProvider.GetQcRequestByTransactionGroupId(data.DataId);
                                        _sendNotif(testingData.RequestQcId, listNextPIC, data.NIK, ApplicationConstant.EDIT_APPROVE_ACTION_TESTING_NOTIF, testingData.TestType, testingData.CodeTest, editor);
                                    }
                                }
                                // else
                                // {
                                //     List<string> receiver = await _ListReceiverNotif(workflowQcTransactionGroup.WorkflowDocumentCode, data.NIK);
                                //     if (((workflowSubmitData.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA) || (workflowSubmitData.WorkflowStatus == null)) && workflowQcTransactionGroup.WorkflowCode != ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_1)
                                //     {
                                //         _sendNotif(testingData.RequestQcId, receiver, data.NIK, ApplicationConstant.APPROVED_ACTION_NOTIF, testingData.TestType, testingData.CodeTest, "");
                                //     }
                                // }
                            }

                            transaction.Commit();

                            #region add audit trail
                            var auditOperation = ApplicationConstant.QC_TEST_STATUS_LABEL_REJECT;
                            if (data.IsApprove)
                            {
                                auditOperation = ApplicationConstant.QC_TEST_STATUS_LABEL_APPROVE;
                            }

                            await _auditTrailBusinessProvider.Add(auditOperation, transactionGroupData.Code, transactionGroupData);
                            #endregion


                            /* Notification Email & WA */

                            // Get List Receiver Notif

                            //notif personel
                            var personal = await _dataProviderRequestQc.GetPersonalById(1);//TODO 1 pindah ke constant

                            var nameAudit = "No Name";
                            // var noHandphoneAudit = "";
                            var emailAudit = "";

                            // Get info Audit
                            var getDetailAudit = await _bioHRIntegrationBussinesProviders.GetEmployeeByNewNik(data.NIK);
                            if (getDetailAudit != null)
                            {
                                nameAudit = getDetailAudit.Name;
                                emailAudit = getDetailAudit.Email;
                            }
                            else
                            {
                                var getDetailPICAUAM = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(data.NIK);
                                if (getDetailPICAUAM != null)
                                {
                                    nameAudit = getDetailPICAUAM.Name;
                                    emailAudit = getDetailPICAUAM.Email;
                                }
                            }

                            /* Send Before PIC */

                            /** Send Notification To Operator uji if data testing Reject By Ahli Muda or Kabag Uji */
                            if (currentStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_AHLI_MUDA && data.IsApprove == false || currentStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG_QC && data.IsApprove == false)
                            {
                                // if (receiverBefore.Any())
                                if (receiverOperatorUji.Any())
                                {
                                    foreach (var rcv in receiverOperatorUji)
                                    {
                                        var namePIC = "No Name";
                                        var noHandphonePIC = "";
                                        var emailPIC = "";

                                        //get info PIC
                                        var getDetailPIC = await _bioHRIntegrationBussinesProviders.GetEmployeeByNewNik(rcv);
                                        if (getDetailPIC != null)
                                        {
                                            var getPICBioHR = await _bioHRIntegrationBussinesProviders.GetEmployeeByNik(getDetailPIC.UserId);
                                            if (getPICBioHR != null)
                                            {
                                                namePIC = getPICBioHR.Nama;
                                                noHandphonePIC = getPICBioHR.Telepon;
                                                emailPIC = getPICBioHR.Email;
                                            }
                                        }
                                        else
                                        {
                                            var getDetailPICAUAM = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(rcv);
                                            if (getDetailPICAUAM != null)
                                            {
                                                namePIC = getDetailPICAUAM.Name;
                                                noHandphonePIC = getDetailPICAUAM.NoTelp;
                                                emailPIC = getDetailPICAUAM.Email;
                                            }
                                        }

                                        MessageNotificationQcTestViewModel settingMessage = new MessageNotificationQcTestViewModel(
                                            transactionGroupData.Code,
                                            transactionGroupData.QcProcessName,
                                            emailPIC, // real pic email
                                                      // personal.Email,
                                                      // (data.IsApprove == true ? ApplicationConstant.APPROVED_ACTION_NOTIF : ApplicationConstant.REJECTED_ACTION_NOTIF),
                                            ApplicationConstant.REJECTED_ACTION_NOTIF,
                                            namePIC,
                                            noHandphonePIC, // real pic phone
                                                            // personal.NoHandphone,
                                            nameAudit,
                                            ApplicationConstant.MSG_TYPE_BEFORE_PIC,
                                            requestData.NoBatch,
                                            requestData.NoRequest,
                                            purposesMsg
                                        );

                                        if (_environmentSetting.EnvironmentName == ApplicationConstant.DEVELOPMENT_ENVIRONMENT_NAME || _environmentSetting.EnvironmentName == ApplicationConstant.TESTING_ENVIRONMENT_NAME)
                                        {
                                            settingMessage.EmailAddress = personal.Email;
                                            settingMessage.NoHandphone = personal.NoHandphone;
                                        }

                                        await _notification.SendEmailNotifQcTest(settingMessage);
                                        await _notification.SendWhatsAppNotifQcTest(settingMessage);
                                    }
                                }
                            }


                            /* Send Next Reciver */

                            /** Send Notification To Kabag Pemfas if Data Testing Approved By Kabag Uji */
                            if (currentStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG_QC && data.IsApprove)
                            {
                                // List<string> receiverNext = await _ListReceiverNotif(workflowQcTransactionGroup.WorkflowDocumentCode, data.NIK);

                                // Get Last Reciver
                                // var LastNextReciver = receiverNext.LastOrDefault();
                                // if (LastNextReciver != null)
                                if (nextPICList.Any())
                                {
                                    foreach (var receiverKabag in nextPICList)
                                    {
                                        var namePIC = "No Name";
                                        var noHandphonePIC = "";
                                        var emailPIC = "";

                                        //get info PIC
                                        var getDetailPIC = await _bioHRIntegrationBussinesProviders.GetEmployeeByNewNik(receiverKabag);
                                        if (getDetailPIC != null)
                                        {
                                            var getPICBioHR = await _bioHRIntegrationBussinesProviders.GetEmployeeByNik(getDetailPIC.UserId);
                                            if (getPICBioHR != null)
                                            {
                                                namePIC = getPICBioHR.Nama;
                                                noHandphonePIC = getPICBioHR.Telepon;
                                                emailPIC = getPICBioHR.Email;
                                            }
                                        }
                                        else
                                        {
                                            var getDetailPICAUAM = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(receiverKabag);
                                            if (getDetailPICAUAM != null)
                                            {
                                                namePIC = getDetailPICAUAM.Name;
                                                noHandphonePIC = getDetailPICAUAM.NoTelp;
                                                emailPIC = getDetailPICAUAM.Email;
                                            }
                                        }

                                        MessageNotificationQcTestViewModel settingMessage = new MessageNotificationQcTestViewModel(
                                            transactionGroupData.Code,
                                            transactionGroupData.QcProcessName,
                                            emailPIC, // real pic email
                                                      // personal.Email,
                                                      // (data.IsApprove == true ? ApplicationConstant.APPROVED_ACTION_NOTIF : ApplicationConstant.REJECTED_ACTION_NOTIF),
                                            ApplicationConstant.APPROVED_ACTION_NOTIF,
                                            namePIC,
                                            noHandphonePIC, // real pic phone
                                                            // personal.NoHandphone,
                                            nameAudit,
                                            ApplicationConstant.MSG_TYPE_BEFORE_PIC,
                                            requestData.NoBatch,
                                            requestData.NoRequest,
                                            purposesMsg
                                        );

                                        if (_environmentSetting.EnvironmentName == ApplicationConstant.DEVELOPMENT_ENVIRONMENT_NAME || _environmentSetting.EnvironmentName == ApplicationConstant.TESTING_ENVIRONMENT_NAME)
                                        {
                                            settingMessage.EmailAddress = personal.Email;
                                            settingMessage.NoHandphone = personal.NoHandphone;
                                        }

                                        await _notification.SendEmailNotifQcTest(settingMessage);
                                        await _notification.SendWhatsAppNotifQcTest(settingMessage);
                                    }
                                }
                            }


                            /* END Notification Email & WA */

                            result.StatusCode = 200;
                            result.Message = data.IsApprove == true ? ApplicationConstant.WORKFLOW_SUCCESS_APPROVE_MESSAGE : ApplicationConstant.WORKFLOW_SUCCESS_REJECT_MESSAGE;
                            result.Data = new List<WorkflowSubmitBindingModel>() { submitDoc };
                        }
                        else
                        {
                            throw new Exception(approvalDoc.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "{Message}", ex.Message);
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return result;
        }

        private async Task<ResponseViewModel<QcTransactionGroup>> _InsertToNextPhase(QcTransactionGroup StartTest, WorkflowQcTransactionGroup data)
        {
            ResponseViewModel<QcTransactionGroup> result = new ResponseViewModel<QcTransactionGroup>();

            // save into workflow transaction group code
            WorkflowQcTransactionGroup insertQcTransactionGroup = new WorkflowQcTransactionGroup()
            {
                QcTransactionGroupId = StartTest.Id,
                WorkflowStatus = "Draft",
                WorkflowDocumentCode = ApplicationConstant.PREFIX_EM_M_TESTING_WORKFLOW_CODE + "-" + StartTest.Code + "-2",
                WorkflowCode = ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_2,
                IsInWorkflow = true,
                CreatedBy = data.CreatedBy,
                CreatedAt = DateHelper.Now(),
                UpdatedBy = data.CreatedBy,
                UpdatedAt = DateHelper.Now(),
            };

            await _workflowQcTransactionGroupDataProvider.Insert(insertQcTransactionGroup);

            //insert into review business to initial workflow

            NewWorkflowDocument newWorkflowDocument = new NewWorkflowDocument()
            {
                DocumentCode = insertQcTransactionGroup.WorkflowDocumentCode,
                ApplicationCode = ApplicationConstant.APP_CODE,
                WorkflowCode = insertQcTransactionGroup.WorkflowCode,
                Description = ApplicationConstant.WORKFLOW_INITIAL_DESC,
                CreatedBy = data.CreatedBy,
                CreatorOrgId = data.CreatedBy
            };

            await _workflowServiceDataProvider.InitiateDoc(newWorkflowDocument);

            //nextPICOrgId and submit workflow
            string actionType = ApplicationConstant.WORKFLOW_ACTION_SUBMIT_NAME;
            List<string> nextPICOrgId = await _getNextPhasePIC(StartTest.Id);

            WorkflowSubmitBindingModel submitDoc = new WorkflowSubmitBindingModel()
            {
                ApplicationCode = ApplicationConstant.APP_CODE,
                DocumentCode = insertQcTransactionGroup.WorkflowDocumentCode,
                Notes = "-",
                OrgId = data.CreatedBy,
                ActionName = actionType,
                NextPICOrgIdList = nextPICOrgId
            };
            ResponseViewModel<ResponseInsertReview> submitWorkflowDocument = await _workflowServiceBusinessProvider.SubmitAction(submitDoc);

            //update workflow transaction group
            if (submitWorkflowDocument.StatusCode == 200)
            {
                ResponseInsertReview submitDocument = submitWorkflowDocument.Data.FirstOrDefault();
                UpdateWorkflowQcTransactionGroupFromApproval updateWorkflowQcTransactionGroupFromApproval = new UpdateWorkflowQcTransactionGroupFromApproval()
                {
                    TransactionGroupId = data.QcTransactionGroupId,
                    WorkflowStatus = submitDocument.WorkflowStatus,
                    IsInWorkflow = submitDocument.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME ? false : true,
                    UpdatedBy = data.CreatedBy
                };

                var updatedWorkflowQcTG = await _workflowQcTransactionGroupDataProvider.UpdateWorkflowQcTransactionGroupDataFromApproval(updateWorkflowQcTransactionGroupFromApproval);

                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = new List<QcTransactionGroup>() { StartTest };
            }
            else
            {
                result.StatusCode = 403;
                result.Message = ApplicationConstant.ERROR_MESSAGE;
            }

            return result;
        }

        private int _getStatus(string workflowStatus)
        {
            var status = 0;
            switch (workflowStatus)
            {
                case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT:
                    status = ApplicationConstant.STATUS_SUBMIT;
                    break;
                case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_PAIRING:
                    status = ApplicationConstant.STATUS_TEST_INREVIEW_PAIRING;
                    break;
                case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_AHLI_MUDA:
                    status = ApplicationConstant.STATUS_TEST_INREVIEW_AHLI_MUDA_QC;
                    break;
                case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG_QC:
                    status = ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_QC;
                    break;
                case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG:
                    status = ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_PRODUKSI;
                    break;
                case ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA:
                    status = ApplicationConstant.STATUS_TEST_INREVIEW_QA;
                    break;
                case ApplicationConstant.WORKFLOW_STATUS_NAME_COMPLETE:
                    status = ApplicationConstant.STATUS_TEST_APPROVED;
                    break;
            }
            return status;
        }

        private async Task<List<string>> _getNextPhasePIC(int transactionGroupId)
        {
            List<string> result = new List<string>();
            List<RequestQcs> requestQcsList = await _dataProvider.GetQcRequestByTestId(transactionGroupId);
            List<RequestQcs> requestQcsListDistinct = requestQcsList.GroupBy(x => x.Id).Select(y => y.First()).ToList();
            foreach (var requestQcs in requestQcsListDistinct)
            {
                int tempOrganization = await _dataProvider.getOrganizationByRequestId(requestQcs.Id);

                ResponseViewModel<ResponseGetNikByOrganizationIdAndPositionType> responseGetNik = await _bioHRIntegrationBussinesProviders.GetNikByOrganizationIdandPositionType(tempOrganization.ToString(), ApplicationConstant.POSITION_TYPE_KABAG);
                foreach (var PIC in responseGetNik.Data)
                {
                    if (PIC.NewUserId != null && PIC.NewUserId != "" && PIC.NewUserId != "." && PIC.NewUserId != "-")
                    {
                        result.Add(PIC.NewUserId);
                    }
                }

                if (!result.Any() && responseGetNik.Data.Any())
                {
                    var delegatation = await _bioHRIntegrationBussinesProviders.GetListNewNikDelegation(responseGetNik.Data[0].PositionId.ToString());
                    result.AddRange(delegatation);
                }
            }
            return result;
        }

        private async Task<List<string>> _getQAPIC(int transactionGroupId)
        {
            List<string> result = new List<string>();
            List<RequestQcs> requestQcsList = await _dataProvider.GetQcRequestByTestId(transactionGroupId);
            List<RequestQcs> requestQcsListDistinct = requestQcsList.GroupBy(x => x.Id).Select(y => y.First()).ToList();
            foreach (var requestQcs in requestQcsListDistinct)
            {
                List<string> personelNik = new List<string>();
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
            }
            return result.Distinct().ToList();
        }

        private async Task AddAuditTrail(string operation, int transactionGroupId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var qcTestDataProvider = scope.ServiceProvider.GetRequiredService<IQcTestDataProvider>();
                var auditTrailBusinessProvider = scope.ServiceProvider.GetRequiredService<IAuditTrailBusinessProvider>();

                var transactionGroup = await qcTestDataProvider.GetByIdForAudit(transactionGroupId);

                if (transactionGroup != null)
                {
                    await auditTrailBusinessProvider.Add(operation, transactionGroup.Code, transactionGroup);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
            }
        }

        private void _sendNotif(int requestQcId, List<string> nextPICOrgId, string nik, string status, string testType, string testCode, string editor)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();

                    var dataProviderRequestQc = scope.ServiceProvider.GetRequiredService<IQcRequestDataProvider>();
                    var bioHrService = scope.ServiceProvider.GetRequiredService<IBioHRIntegrationBussinesProviders>();
                    var auamService = scope.ServiceProvider.GetRequiredService<IAUAMServiceBusinessProviders>();
                    var notificationService =
                        scope.ServiceProvider.GetRequiredService<INotificationServiceBusinessProvider>();

                    //notification message init
                    //notif personel
                    // var personal = await _dataProviderRequestQc.GetPersonalById(2);//TODO 1 pindah ke constant
                    var requestQc = await dataProviderRequestQc.GetById(requestQcId);

                    //get request purposes
                    string purposesMsg = "-";
                    // var nameAudit = "No Name";
                    // var NoHandphoneAudit = "";
                    // var EmailAudit = "";
                    var PicEmailAudit = "";
                    var PicNameAudit = "No Name";
                    var PicPostionName = "";
                    var sendPicRole = "";
                    var sendPicName = "";
                    var purposesDatas = await dataProviderRequestQc.getRequestPurposeNames(requestQcId);
                    if (purposesDatas.Any())
                    {
                        purposesMsg = string.Join(", ", purposesDatas.ToArray());
                    }

                    //get info Pic Approved
                    List<ReceiverNotifModel> receivers = new List<ReceiverNotifModel>();
                    foreach (var item in nextPICOrgId.GroupBy(x => x))
                    {
                        var getDetailPICBioHR = await bioHrService.GetEmployeeByNewNik(item.Key);
                        if (getDetailPICBioHR != null)
                        {
                            var nameTemp = getDetailPICBioHR.Name;
                            var EmailTemp = getDetailPICBioHR.Email;
                            var NoHandphoneTemp = "";

                            var getPICBioHR = await bioHrService.GetEmployeeByNik(getDetailPICBioHR.UserId);
                            if (getPICBioHR != null)
                            {
                                NoHandphoneTemp = getPICBioHR.Telepon;
                            }

                            ReceiverNotifModel rev = new ReceiverNotifModel()
                            {
                                ReceiverName = nameTemp,
                                ReceiverEmail = EmailTemp,
                                ReceiverNoHp = NoHandphoneTemp,
                            };
                            receivers.Add(rev);
                        }
                        else
                        {
                            var getDetailPICAUAM = await auamService.GetPersonalExtDetailByNik(item.Key);
                            if (getDetailPICAUAM != null)
                            {
                                ReceiverNotifModel rev = new ReceiverNotifModel()
                                {
                                    ReceiverName = getDetailPICAUAM.Name,
                                    ReceiverEmail = getDetailPICAUAM.Email,
                                    ReceiverNoHp = getDetailPICAUAM.NoTelp,
                                };
                                receivers.Add(rev);
                            }
                        }
                    }

                    var getDetailPICBioHRCurrent = await bioHrService.GetEmployeeByNewNik(nik);
                    if (getDetailPICBioHRCurrent != null)
                    {
                        PicNameAudit = getDetailPICBioHRCurrent.Name;
                        PicEmailAudit = getDetailPICBioHRCurrent.Email;
                        PicPostionName = getDetailPICBioHRCurrent.PositionName;
                    }

                    var getDetailPICBioHREditor = await bioHrService.GetEmployeeByNewNik(editor);
                    if (getDetailPICBioHREditor != null)
                    {
                        sendPicRole = getDetailPICBioHREditor.PositionName;
                        sendPicName = getDetailPICBioHREditor.Name;
                    }

                    //get receiver 
                    foreach (var settingMessage in receivers.Select(itemRe => new MessageNotificationMonitoringViewModel(
                                 requestQc.NoRequest,
                                 requestQc.NoBatch,
                                 requestQc.ItemName,
                                 purposesMsg,
                                 itemRe.ReceiverEmail, //item.ReceiverEmail,
                                 status,
                                 itemRe.ReceiverName,
                                 itemRe.ReceiverNoHp, //item.Handphone,
                                                      //  PicPostionName,
                                 PicNameAudit,
                                 testType,
                                 testCode,
                                //  sendPicRole
                                sendPicName)))
                    {
                        // await notificationService.SendEmailNotifMonitoring(settingMessage);
                        // await notificationService.SendWhatsAppNotifMonitoring(settingMessage);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "{Message}", e.Message);
                }

            });
        }

        private async Task<string> _GetEditor(string workflowDocumentCode)
        {
            var editor = "";
            DocumentHistoryResponseViewModel workflowHistory = await _reviewDataProvider.GetListHistoryWorkflow(workflowDocumentCode);

            foreach (var itemHistory in workflowHistory.History)
            {
                foreach (var itemPIC in itemHistory.PICs)
                {
                    if (itemPIC.ActionName == ApplicationConstant.WORKFLOW_ACTION_EDIT_NAME)
                    {
                        editor = itemPIC.OrgId;
                    }
                }
            }

            return editor;
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
            foreach (var item in workflowPIC.PICs)
            {
                listPIC.Add(item.orgId);
            }

            return listPIC.Distinct().ToList();
        }

        public async Task<ResponseOneDataViewModel<QcTransactionGroupDetailViewModel>> GetByIdV2(int id)
        {
            ResponseOneDataViewModel<QcTransactionGroupDetailViewModel> result = new ResponseOneDataViewModel<QcTransactionGroupDetailViewModel>();
            var getData = await _dataProvider.GetByIdV2(id);

            if (getData == null)
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

        public async Task<ResponseViewModel<SampleBatchQcProcessViewModel>> ListSampleBatchTestV2(int QcProcessId, string search, int RoomId, int PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate)
        {
            ResponseViewModel<SampleBatchQcProcessViewModel> result = new ResponseViewModel<SampleBatchQcProcessViewModel>();

            if (QcProcessId == 0)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.ERROR_REQUIRED_PAYLOAD_FIELD + " QcProcessId " + ApplicationConstant.ERROR_REQUIRED_PAYLOAD_MSG;

                return result;
            }

            List<SampleBatchQcProcessViewModel> getData = await _dataProvider.ListSampleBatchTestV2(QcProcessId, search, RoomId, PhaseId, ReceiptStartDate, ReceiptEndDate);

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

        public async Task<ResponseViewModel<ParameterThresholdRelationAltViewModel>> ListParameterThresholdAlt(string GradeRoomId, string testScenarioLabel, int? testGroupId)
        {
            //get data activity master data
            var gradeRoomIds = new List<int>();

            // filter status from param status is string
            gradeRoomIds = GradeRoomId.Split(',').Select(x => Int32.Parse(x)).Reverse().ToList();


            ResponseViewModel<ParameterThresholdRelationAltViewModel> result = new ResponseViewModel<ParameterThresholdRelationAltViewModel>();
            List<ParameterThresholdRelationAltViewModel> getData = await _dataProvider.ListParameterThresholdAlt(gradeRoomIds, testScenarioLabel, testGroupId);

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

        private async Task<List<string>> _ListReceiverOperatorUji(string workflowDocumetnCode, string nikReviewer)
        {
            List<string> listNik = new List<string>();
            //get workflow history for get nik
            DocumentHistoryResponseViewModel workflowHistory = await _workflowServiceDataProvider.GetListHistoryWorkflow(workflowDocumetnCode);
            foreach (var itemPIC in workflowHistory.History)
            {
                if (itemPIC.StatusName == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT || itemPIC.StatusName == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_PAIRING)
                {
                    foreach (var item in itemPIC.PICs)
                    {
                        if (item.OrgId != nikReviewer)
                        {
                            listNik.Add(item.OrgId);
                        }
                    }
                }
            }
            return listNik.Distinct().ToList();
        }

        public async Task<ResponseOneDataViewModel<QcTransactionGroupProcessRelViewModel>> GetTransactionGroupProcessById(int id)
        {
            var result = new ResponseOneDataViewModel<QcTransactionGroupProcessRelViewModel>();
            var transactionGroupProcess = await _dataProvider.GetTransactionGroupProcessById(id);

            if (transactionGroupProcess == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = transactionGroupProcess;

            return result;
        }
    }
}