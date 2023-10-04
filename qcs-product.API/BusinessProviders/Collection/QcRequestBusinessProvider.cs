using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.Helpers;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class QcRequestBusinessProvider : IQcRequestBusinessProvider
    {
        private readonly IQcRequestDataProvider _dataProvider;
        private readonly IQcSamplingDataProvider _dataProviderSampling;
        private readonly IRoomDataProvider _dataProviderRoom;
        private readonly IItemDataProvider _dataProviderItem;
        private readonly IToolDataProvider _dataProviderTool;
        private readonly IFacilityDataProvider _dataProviderFacility;
        private readonly ITestScenarioDataProvider _dataProviderTestScenario;
        private readonly IPurposeDataProvider _dataProviderPurpose;
        private readonly IOrganizationDataProvider _dataProviderOrganization;
        private readonly INotificationServiceBusinessProvider _notification;
        private readonly IAuditTrailBusinessProvider _auditTrailBusinessProvider;
        private readonly ILogger<QcRequestBusinessProvider> _logger;
        private readonly IBioHRIntegrationBussinesProviders _bioHRIntegrationBussinesProviders;
        private readonly IAUAMServiceBusinessProviders _auamServiceBusinessProviders;
        private readonly ITransactionDataProvider _transactioDataProvider;
        private readonly ITransactionTestScenarioDataProvider _transactionTestScenarioDataProvider;
        private readonly ITestParameterDataProvider _dataTestParameterProvider;

        [ExcludeFromCodeCoverage]
        public QcRequestBusinessProvider(
            IQcRequestDataProvider dataProvider,
            IQcSamplingDataProvider dataProviderSampling,
            IRoomDataProvider dataProviderRoom,
            IItemDataProvider dataProviderItem,
            IToolDataProvider dataProviderTool,
            IFacilityDataProvider dataProviderFacility,
            ITestScenarioDataProvider dataProviderTestScenario,
            IPurposeDataProvider dataProviderPurpose,
            IOrganizationDataProvider dataProviderOrganization,
            ILogger<QcRequestBusinessProvider> logger,
            INotificationServiceBusinessProvider notification,
            IAuditTrailBusinessProvider auditTrailBusinessProvider,
            IBioHRIntegrationBussinesProviders bioHRIntegrationBussinesProviders,
            IAUAMServiceBusinessProviders auamServiceBusinessProviders,
            ITransactionDataProvider transactioDataProvider,
            ITransactionTestScenarioDataProvider transactionTestScenarioDataProvider,
            ITestParameterDataProvider dataTestParameterProvider
            )
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _dataProviderSampling = dataProviderSampling ?? throw new ArgumentNullException(nameof(dataProviderSampling));
            _dataProviderRoom = dataProviderRoom ?? throw new ArgumentNullException(nameof(dataProviderRoom));
            _dataProviderItem = dataProviderItem ?? throw new ArgumentNullException(nameof(dataProviderItem));
            _dataProviderTool = dataProviderTool ?? throw new ArgumentNullException(nameof(dataProviderTool));
            _dataProviderFacility = dataProviderFacility ?? throw new ArgumentNullException(nameof(dataProviderFacility));
            _dataProviderTestScenario = dataProviderTestScenario ?? throw new ArgumentNullException(nameof(dataProviderTestScenario));
            _dataProviderPurpose = dataProviderPurpose ?? throw new ArgumentNullException(nameof(dataProviderPurpose));
            _dataProviderOrganization = dataProviderOrganization ?? throw new ArgumentNullException(nameof(dataProviderOrganization));
            _logger = logger;
            _notification = notification;
            _auditTrailBusinessProvider = auditTrailBusinessProvider;
            _bioHRIntegrationBussinesProviders = bioHRIntegrationBussinesProviders ?? throw new ArgumentNullException(nameof(bioHRIntegrationBussinesProviders));
            _auamServiceBusinessProviders = auamServiceBusinessProviders;
            _transactioDataProvider = transactioDataProvider;
            _transactionTestScenarioDataProvider = transactionTestScenarioDataProvider;
            _dataTestParameterProvider = dataTestParameterProvider ?? throw new ArgumentNullException(nameof(dataTestParameterProvider));

        }

        public async Task<ResponseViewModel<RequestQcs>> Edit(EditRequestQcsBindingModel data)
        {
            ResponseViewModel<RequestQcs> result = new ResponseViewModel<RequestQcs>();
            var getDataRequestQcs = await _dataProvider.GetById(data.Id);
            if (getDataRequestQcs == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }
            if (data.ProductPhaseName == null || data.ItemName == null || data.NoBatch == null)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.MESSAGE_MUST_REQUIRED;
                return result;
            }

            if (getDataRequestQcs.Status != ApplicationConstant.STATUS_DRAFT && getDataRequestQcs.Status != ApplicationConstant.STATUS_REJECT)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.DATA_CANT_EDIT;
                return result;
            }

            // if (data.RequestPurposes.FirstOrDefault(x => x.PurposeId == ApplicationConstant.PURPOSE_BR_ID) != null)
            // {

            //     var CheckNoBatchRequestEM = await _dataProvider.CheckNoBatchRequestEM(data.NoBatch, getDataRequestQcs.EmPhaseId, getDataRequestQcs.TestScenarioLabel, null);
            //     // Nomor batch boleh sama jika kombinasi antara fase produksi & no batch & kondisi & status request/sampling tersebut berbeda & location != "QC"
            //     if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT
            //         && data.Location.ToLower() != ApplicationConstant.REQUEST_LOCATION_QC
            //         && CheckNoBatchRequestEM.Any())
            //     {
            //         var CheckNoBatchRequestEMSelfID = CheckNoBatchRequestEM.Where(x => x.Id != getDataRequestQcs.Id && x.NoBatch == data.NoBatch).ToList();
            //         if (CheckNoBatchRequestEMSelfID.Any())
            //         {
            //             result.StatusCode = ApplicationConstant.ERROR_CODE_400;
            //             result.Message = ApplicationConstant.MESSAGE_NO_BATCH_ALREADY_EXIST;
            //             return result;
            //         }
            //     }
            // }

            var auditTrailOperation = ApplicationConstant.QC_REQUEST_STATUS_LABEL_EDIT;

            //status dinamis
            if (data.IsSubmit)
            {
                /*if (data.NoBatch != "" && data.NoBatch != null) //jika no batch tidak mandatory)
                {
                    var checkRequestQcs = await _dataProvider.GetByBatchAndPhaseId(data.NoBatch, data.EmPhaseId);
                    if (checkRequestQcs != null && checkRequestQcs.Id != getDataRequestQcs.Id)
                    {
                        result.StatusCode = ApplicationConstant.ERROR_CODE_400;
                        result.Message = $"Batch number is already exist in {checkRequestQcs.EmPhaseName}";
                        return result;
                    }
                }*/


                getDataRequestQcs.Status = ApplicationConstant.STATUS_APPROVED;
                if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT)
                {
                    auditTrailOperation = ApplicationConstant.QC_REQUEST_STATUS_LABEL_COMPLETED;
                }
                else if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT)
                {
                    auditTrailOperation = ApplicationConstant.QC_REQUEST_STATUS_LABEL_APPROVE;
                }

                //auditTrailOperation = ApplicationConstant.QC_REQUEST_STATUS_LABEL_APPROVE;
            }

            // if (!data.RequestRooms.Any())
            // {
            //     result.StatusCode = ApplicationConstant.ERROR_CODE_400;
            //     result.Message = $"Request Room data is empty";
            //     return result;
            // }

            // check facility
            Facility getFacility = null;
            var getOrg = new Organization();
            // if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT)
            // {
            //     if (data.IsSubmit)
            //     {
            //         #region - insert into facility ext
            //         getFacility = await _transactioDataProvider._InsertFacility(data.FacilityId);
            //         #endregion
            //         getOrg = await _transactioDataProvider.GetDetailById(getFacility.OrganizationId);
            //     }
            //     else
            //     {
            //         getFacility = await _dataProviderFacility.GetById(data.FacilityId);
            //         getOrg = await _dataProviderOrganization.GetDetailById(getFacility.OrganizationId);
            //     }

            //     if (getFacility == null)
            //     {
            //         result.StatusCode = ApplicationConstant.ERROR_CODE_400;
            //         result.Message = $"Facility Id data is empty"; // TODO : pindah ke const
            //         return result;
            //     }


            //     if (getFacility == null)
            //     {
            //         result.StatusCode = ApplicationConstant.ERROR_CODE_400;
            //         result.Message = $"Facility Id data is empty"; // TODO : pindah ke const
            //         return result;
            //     }
            // }



            // #region check request AHU

            // if (data.RequestAhu.Any())
            // {
            //     foreach (var rAhu in data.RequestAhu)
            //     {
            //         var getAhu = new ToolsAHUViewModel();
            //         var getAhuExt = await _dataProviderTool.getAhuById(rAhu.AhuId);
            //         if (data.IsSubmit)
            //         {
            //             #region get data ahu id(tool id) setelah dicopykan ke table transaksi 
            //             var getAhuTRx = await _transactioDataProvider._InsertToolTrxByToolId(rAhu.AhuId, getTestParam, samplingPointIdTrxExisting);
            //             samplingPointIdTrxExisting.AddRange(getAhuTRx.SamplingPointInfo);
            //             #endregion
            //             getAhu.ToolCode = getAhuTRx.ToolCode;
            //             getAhu.ToolId = getAhuTRx.Id;
            //             getAhu.ToolName = getAhuTRx.Name;
            //         }
            //         else
            //         {
            //             getAhu.ToolCode = getAhuExt.ToolCode;
            //             getAhu.ToolId = getAhuExt.ToolId;
            //             getAhu.ToolName = getAhuExt.ToolName;
            //         }

            //         #region get data ahu id(tool id) setelah dicopykan ke table transaksi 
            //         //var getAhu = await _transactioDataProvider._InsertToolTrxByToolId(rAhu.AhuId, getTestParam, samplingPointIdTrxExisting);
            //         //samplingPointIdTrxExisting.AddRange(getAhu.SamplingPointInfo);
            //         #endregion

            //         if (getAhu == null)
            //         {
            //             result.StatusCode = ApplicationConstant.ERROR_CODE_400;
            //             result.Message = $"Ahu Id {rAhu.AhuId} data is empty"; // TODO : pindah ke const
            //             return result;
            //         }
            //     }
            // }
            // #endregion

            // #region check request purpose
            // if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT)
            // {
            //     foreach (var reqPurpose in data.RequestPurposes)
            //     {
            //         var getPurpose = await _dataProviderPurpose.GetById(reqPurpose.PurposeId);
            //         if (getPurpose == null)
            //         {
            //             result.StatusCode = ApplicationConstant.ERROR_CODE_400;
            //             result.Message = $"Purpose Id {reqPurpose.PurposeId} data is empty"; // TODO : pindah ke const
            //             return result;
            //         }
            //     }
            // }
            // #endregion

            // #region check request rooms
            // foreach (var reqRoom in data.RequestRooms)
            // {
            //     var getRoom = await _dataProviderRoom.GetRoomRelationDetailById(reqRoom.RoomId);
            //     //if (data.IsSubmit)
            //     //{
            //     //    #region memindahkan data room yang digunakan ke tabel transaksi
            //     //    var roomUsedByRequest = new MasterDataUseByRequest()
            //     //    {
            //     //        RoomId = reqRoom.RoomId,
            //     //        CreatedBy = data.UpdatedBy
            //     //    };
            //     //    getRoom = await _transactioDataProvider.InsertRoomMasterDataForByRequestId(roomUsedByRequest, getTestParam, samplingPointIdTrxExisting);

            //     //    samplingPointIdTrxExisting.AddRange(getRoom.SamplingPointInfo);
            //     //    #endregion
            //     //}
            //     //else
            //     //{
            //     //    getRoom = await _dataProviderRoom.GetRoomRelationDetailById(reqRoom.RoomId);
            //     //}

            //     TestScenario getTestScenario = null;
            //     if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT)
            //     {
            //         getTestScenario = await _dataProviderTestScenario.GetByGradeRoomLabel(getRoom.GradeRoomId, data.TestScenarioLabel);
            //         //if (data.IsSubmit)
            //         //{
            //         //    getTestScenario = await _transactionTestScenarioDataProvider.GetByGradeRoomLabel(getRoom.GradeRoomId, data.TestScenarioLabel);
            //         //}
            //         //else
            //         //{
            //         //    getTestScenario = await _dataProviderTestScenario.GetByGradeRoomLabel(getRoom.GradeRoomId, data.TestScenarioLabel);
            //         //}

            //         if (getTestScenario == null)
            //         {
            //             result.StatusCode = ApplicationConstant.ERROR_CODE_400;
            //             result.Message = $"Grade Room {getRoom.GradeRoomCode}, Test Scenario data is empty"; // TODO : pindah ke const
            //             return result;
            //         }
            //     }
            // }
            // #endregion

            int? orgRequestId = null;
            int? orgRequestBioHrId = null;
            string orgRequestName = null;

            // // EM
            // if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT)
            // {
            //     if (getOrg != null)
            //     {
            //         orgRequestBioHrId = getOrg.BIOHROrganizationId;
            //         orgRequestName = getOrg.Name;
            //         orgRequestId = getOrg.Id;
            //     }
            //     else
            //     {
            //         result.StatusCode = ApplicationConstant.ERROR_CODE_400;
            //         result.Message = $"Organisazion Facility data is empty"; // TODO : pindah ke const
            //         return result;
            //     }
            // }

            // getDataRequestQcs.Date = data.Date;
            // getDataRequestQcs.OrgId = orgRequestId;
            // getDataRequestQcs.OrgName = orgRequestName;
            // getDataRequestQcs.NoBatch = data.NoBatch.Trim();
            // getDataRequestQcs.ItemId = data.ItemId;
            // getDataRequestQcs.ItemName = data.ItemName;
            // getDataRequestQcs.ProductFormId = data.ProductFormId;
            // getDataRequestQcs.ProductFormName = data.ProductFormName;
            // if (data.TypeFormId > 0)
            //     getDataRequestQcs.TypeFormId = data.TypeFormId;
            // getDataRequestQcs.ProductGroupId = data.ProductGroupId;
            // getDataRequestQcs.ProductGroupName = data.ProductGroupName;
            // getDataRequestQcs.ProductPresentationId = data.ProductPresentationId;
            // getDataRequestQcs.ProductPresentationName = data.ProductPresentationName;
            // getDataRequestQcs.TypeFormName = data.TypeFormName;
            // getDataRequestQcs.TypeRequestId = data.TypeRequestId;
            // getDataRequestQcs.TypeRequest = data.TypeRequest;
            // getDataRequestQcs.ProductTemperature = data.ProductTemperature;
            // getDataRequestQcs.StorageTemperatureId = data.StorageTemperatureId;
            // getDataRequestQcs.StorageTemperatureName = data.StorageTemperatureName;
            // getDataRequestQcs.PurposeId = data.PurposeId;
            // getDataRequestQcs.PurposeName = data.PurposeName;

            // // Input Request Product ------------------------------------
            // getDataRequestQcs.ProductPhaseId = (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT ? data.ProductPhaseId : 0);
            // getDataRequestQcs.ProductPhaseName = (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT ? data.ProductPhaseName : null);
            // // Input Request EM -----------------------------------------
            // getDataRequestQcs.TestScenarioLabel = (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT ? data.TestScenarioLabel : null);
            // getDataRequestQcs.EmPhaseId = (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT ? data.EmPhaseId : 0);
            // getDataRequestQcs.EmPhaseName = (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT ? data.EmPhaseName : null);
            // // ----------------------------------------------------------
            // getDataRequestQcs.FacilityId = (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT ? data.FacilityId : 0);
            // getDataRequestQcs.FacilityCode = (getFacility != null ? getFacility.Code : null);
            // getDataRequestQcs.FacilityName = (getFacility != null ? getFacility.Name : null);

            // getDataRequestQcs.Location = data.Location;
            // getDataRequestQcs.ProcessDate = data.ProcessDate;
            // getDataRequestQcs.ProcessId = data.ProcessId;
            // getDataRequestQcs.ProcessName = data.ProcessName;
            // getDataRequestQcs.ItemTemperature = data.ItemTemperature;


            getDataRequestQcs.Date = data.Date;
            getDataRequestQcs.OrgId = 0; //(orgRequestId != null ? orgRequestId : null),
            getDataRequestQcs.OrgName = "";//(orgRequestName != null ? orgRequestName : null),
            getDataRequestQcs.NoBatch = data.NoBatch.Trim();
            getDataRequestQcs.RequestQcsId = data.RequestQcsId;
            getDataRequestQcs.RequestQcsNo = data.RequestQcsNo;
            getDataRequestQcs.ItemId = data.ItemId;
            getDataRequestQcs.ItemName = data.ItemName;
            getDataRequestQcs.ProductFormId = data.ProductFormId;
            getDataRequestQcs.ProductFormName = data.ProductFormName;
            getDataRequestQcs.ProductGroupId = data.ProductGroupId;
            getDataRequestQcs.ProductGroupName = data.ProductGroupName;
            getDataRequestQcs.ProductPresentationId = data.ProductPresentationId;
            getDataRequestQcs.ProductPresentationName = data.ProductPresentationName;
            getDataRequestQcs.TypeRequestId = data.TypeRequestId;
            getDataRequestQcs.TypeRequest = data.TypeRequest;
            getDataRequestQcs.StorageTemperatureId = data.StorageTemperatureId;
            getDataRequestQcs.StorageTemperatureName = data.StorageTemperatureName;
            getDataRequestQcs.PurposeId = data.PurposeId;
            getDataRequestQcs.PurposeName = data.PurposeName;
            
            getDataRequestQcs.ProductPhaseId = data.ProductPhaseId;
            getDataRequestQcs.ProductPhaseName = data.ProductPhaseName;

            getDataRequestQcs.ProcessId = data.ProcessId;
            getDataRequestQcs.ProcessName = data.ProcessName;

            getDataRequestQcs.UpdatedBy = data.UpdatedBy;
            getDataRequestQcs.UpdatedAt = DateHelper.Now();

            
            List<TestTypeQcs> dataTestTypeQcs = new List<TestTypeQcs>();
            foreach (var testTypeQcs in data.TestTypesQcs)
            {
                TestTypeQcs dataTestTypeQc = new TestTypeQcs()
                {
                    // Input Request Product ------------------------------------
                    PurposeId = testTypeQcs.PurposeId,
                    TestTypeId = testTypeQcs.TestTypeId,
                    TestTypeName = testTypeQcs.TestTypeName,
                    TestTypeMethodId = testTypeQcs.TestTypeMethodId,
                    TestTypeMethodName = testTypeQcs.TestTypeMethodName,
                    SampleAmountVolume = testTypeQcs.SampleAmountVolume,
                    SampleAmountUnit = testTypeQcs.SampleAmountUnit ,
                    SampleAmountPresentation = testTypeQcs.SampleAmountPresentation ,
                    SampleAmountCount = testTypeQcs.SampleAmountCount,
                    CreatedBy = data.UpdatedBy,
                    UpdatedBy = data.UpdatedBy,
                    OrgId = testTypeQcs.OrgId,
                    OrgName = testTypeQcs.OrgName,

                    CreatedAt = DateHelper.Now(),
                    UpdatedAt = DateHelper.Now()
                    //Status = 1
                };
                dataTestTypeQcs.Add(dataTestTypeQc);

            }

            var dataEdit = await _dataProvider.Edit(getDataRequestQcs, dataTestTypeQcs, data.IsSubmit);
            
            List<RequestQcs> listResult = new List<RequestQcs>() { dataEdit };

            // if (getDataRequestQcs.Status >= ApplicationConstant.STATUS_SUBMIT)
            // {
            //     //notif
            //     if (data.TypeRequestId != ApplicationConstant.REQUEST_TYPE_PRODUCT && data.TypeRequestId != ApplicationConstant.REQUEST_TYPE_PRODUCT) //request em not send notif massage
            //     {
            //         var personal = await _dataProvider.GetPersonalById(1);

            //         if (personal != null)
            //         {
            //             _logger.LogInformation($"{personal.Id}");
            //             MessageNotificationRequestQcsViewModel settingMessage = new MessageNotificationRequestQcsViewModel(getDataRequestQcs.NoRequest, data.NoBatch, data.ItemName, personal.Email, ApplicationConstant.NEW_ACTION_NOTIF, personal.Name, personal.NoHandphone);
            //             _notification.SendEmailNotif(settingMessage);
            //             _notification.SendWhatsAppNotif(settingMessage);
            //         }
            //     }
            // }

            // var usernameModifier = await _auditTrailBusinessProvider.GetUsernameByNik(getDataRequestQcs.UpdatedBy);
            // await _auditTrailBusinessProvider.Add(auditTrailOperation, getDataRequestQcs.NoRequest, getDataRequestQcs, usernameModifier);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = listResult;

            return result;
        }

        public async Task<ResponseOneDataViewModel<RequestQcs>> EditNoBatch(EditNoBatchBindingModel data)
        {
            ResponseOneDataViewModel<RequestQcs> result = new ResponseOneDataViewModel<RequestQcs>();

            // allowed status in request
            var AllowedStatus = new List<int>();
            AllowedStatus.Add(ApplicationConstant.STATUS_REJECT);
            AllowedStatus.Add(ApplicationConstant.STATUS_CANCEL);
            AllowedStatus.Add(ApplicationConstant.STATUS_DRAFT);

            var getDataRequestQcs = await _dataProvider.GetById(data.RequestId);
            if (getDataRequestQcs == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            /*if (data.NoBatch != getDataRequestQcs.NoBatch)
            {
                var checkNoBatch = await _dataProvider.GetRequestQcByBatchNumber(data.NoBatch);
                if (checkNoBatch != null)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.MESSAGE_NO_BATCH_ALREADY_EXIST;
                    return result;
                }
            }*/

            var CheckNoBatchRequestEM = await _dataProvider.CheckNoBatchRequestEM(data.NoBatch, getDataRequestQcs.EmPhaseId, getDataRequestQcs.TestScenarioLabel, null);
            // Nomor batch boleh sama jika kombinasi antara fase produksi & no batch & kondisi & status request/sampling tersebut berbeda & location != "QC
            if (CheckNoBatchRequestEM.Any()
                && getDataRequestQcs.Location.ToLower() != ApplicationConstant.REQUEST_LOCATION_QC)
            {
                var CheckNoBatchRequestEMSelfID = CheckNoBatchRequestEM.Where(x => x.Id != getDataRequestQcs.Id && x.NoBatch == data.NoBatch).ToList();
                if (CheckNoBatchRequestEMSelfID.Any())
                {
                    result.StatusCode = ApplicationConstant.ERROR_CODE_400;
                    result.Message = ApplicationConstant.MESSAGE_NO_BATCH_ALREADY_EXIST;
                    return result;
                }
            }

            getDataRequestQcs.NoBatch = data.NoBatch;
            getDataRequestQcs.UpdatedBy = data.UpdatedBy;
            getDataRequestQcs.UpdatedAt = DateHelper.Now();

            var updateNoBatch = await _dataProvider.EditNoBatch(getDataRequestQcs);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = updateNoBatch;

            return result;
        }

        public async Task<ResponseOneDataViewModel<RequestQcs>> RejectRequestQc(RejectRequestQcBindingModel data)
        {
            ResponseOneDataViewModel<RequestQcs> result = new ResponseOneDataViewModel<RequestQcs>();

            var getDataRequestQcs = await _dataProvider.GetById(data.RequestId);
            if (getDataRequestQcs == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            if (getDataRequestQcs.Status == ApplicationConstant.STATUS_CANCEL)
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.QC_REQUEST_ALREADY_CANCELED;
                return result;
            }

            // change status
            getDataRequestQcs.Status = ApplicationConstant.STATUS_CANCEL;
            getDataRequestQcs.UpdatedBy = data.UpdatedBy;
            getDataRequestQcs.UpdatedAt = DateHelper.Now();

            // audit trail cancel request
            var rejectRequestProcess = await _dataProvider.RejectRequestQc(getDataRequestQcs);
            var usernameModifierRequest = await _auditTrailBusinessProvider.GetUsernameByNik(rejectRequestProcess.UpdatedBy);
            await _auditTrailBusinessProvider.Add(ApplicationConstant.QC_REQUEST_STATUS_LABEL_CANCEL, rejectRequestProcess.NoRequest, rejectRequestProcess, usernameModifierRequest);

            // audit trail cancel sampling
            var dataQcSampling = await _dataProviderSampling.GetByRequestId(data.RequestId);
            foreach (var item in dataQcSampling)
            {
                var usernameModifierSampling = await _auditTrailBusinessProvider.GetUsernameByNik(item.UpdatedBy);
                await _auditTrailBusinessProvider.Add(ApplicationConstant.QS_SAMPLING_STATUS_LABEL_CANCEL, item.Code, item, usernameModifierSampling);
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = rejectRequestProcess;

            return result;
        }

        public async Task<ResponseViewModel<RequestQcs>> EditStatus(EditStatusRequestQcsBindingModel data)
        {
            ResponseViewModel<RequestQcs> result = new ResponseViewModel<RequestQcs>();

            var statusFilter = new List<int>();

            statusFilter.Add(ApplicationConstant.STATUS_REJECT);
            statusFilter.Add(ApplicationConstant.STATUS_CANCEL);
            statusFilter.Add(ApplicationConstant.STATUS_DRAFT);
            statusFilter.Add(ApplicationConstant.STATUS_SUBMIT);
            statusFilter.Add(ApplicationConstant.STATUS_APPROVED);


            if (!statusFilter.Contains(data.Status))
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.WRONG_STATUS_CODE_REQUEST;

                return result;
            }

            var getDataRequestQcs = await _dataProvider.GetById(data.Id);

            if (getDataRequestQcs == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                getDataRequestQcs.Status = data.Status;
                getDataRequestQcs.UpdatedBy = data.UpdatedBy;
                getDataRequestQcs.UpdatedAt = DateHelper.Now();

                var statusUpdate = ApplicationConstant.UPDATED_ACTION_NOTIF;

                switch (getDataRequestQcs.Status)
                {
                    case -2:
                        statusUpdate = ApplicationConstant.REJECTED_ACTION_NOTIF;
                        break;
                    case -1:
                        statusUpdate = ApplicationConstant.CANCELLED_ACTION_NOTIF;
                        break;
                    case 0:
                        statusUpdate = ApplicationConstant.NEW_ACTION_NOTIF;
                        break;
                    case 1:
                        statusUpdate = ApplicationConstant.UPDATED_ACTION_NOTIF;
                        break;
                    case 2:
                        statusUpdate = ApplicationConstant.APPROVED_ACTION_NOTIF;
                        break;
                }

                //notif
                var personal = await _dataProvider.GetPersonalById(1);

                if (personal != null)
                {
                    _logger.LogInformation($"{personal.Id}");
                    MessageNotificationRequestQcsViewModel settingMessage = new MessageNotificationRequestQcsViewModel(getDataRequestQcs.NoRequest, getDataRequestQcs.NoBatch, getDataRequestQcs.ItemName, personal.Email, statusUpdate, personal.Name, personal.NoHandphone);
                    await _notification.SendEmailNotif(settingMessage);
                    await _notification.SendWhatsAppNotif(settingMessage);
                }

                var editStatus = await _dataProvider.EditStatus(getDataRequestQcs);
                List<RequestQcs> listResult = new List<RequestQcs>() { editStatus };
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = listResult;

            }

            return result;
        }

        public async Task<ResponseViewModel<RequestQcsRelationViewModel>> GetRequestQcById(Int32 requestQcId)
        {
            ResponseViewModel<RequestQcsRelationViewModel> result = new ResponseViewModel<RequestQcsRelationViewModel>();
            _logger.LogInformation($"getData: {requestQcId}");
            var getData = await _dataProvider.GetRequestQcById(requestQcId);
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

        public async Task<ResponseOneDataViewModel<RequestQcsViewModel>> GetRequestQcByBatch(string BatchNumber)
        {
            ResponseOneDataViewModel<RequestQcsViewModel> result = new ResponseOneDataViewModel<RequestQcsViewModel>();

            var getData = await _dataProvider.GetRequestQcByBatchNumber(BatchNumber);

            if (getData == null)
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

        private async Task<string> GenerateRequestNumber(DateTime date, int? orgId, string orgName)
        {
            var month = GetMonthRomawi(date);
            var year = date.Date.Year.ToString();
            var getOrganization = await _dataProviderOrganization.GetDetailByBIOHROrganizationIdAndOrgName(orgId, orgName);
            var codeOrg = getOrganization.OrgCode;

            ///TODO PMV pindahkan ke constant
            var noRequest = "/" + codeOrg + "/" + month + "/" + year;

            return noRequest;
        }

        public async Task<ResponseViewModel<RequestQcs>> Insert(InsertRequestQcsBindingModel data)
        {
            ResponseViewModel<RequestQcs> result = new ResponseViewModel<RequestQcs>();

            var statusRequest = data.IsSubmit ? ApplicationConstant.STATUS_SUBMIT : ApplicationConstant.STATUS_DRAFT;
            var getTestParam = await _transactioDataProvider.GetListTransactionTestParams();
            var samplingPointIdTrxExisting = new List<SamplingPointLiteViewModel>();

            if (statusRequest == ApplicationConstant.STATUS_SUBMIT)
            {
                if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT || data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT)
                {
                    statusRequest = ApplicationConstant.STATUS_APPROVED;
                }

                // if (data.RequestPurposes.FirstOrDefault(x => x.PurposeId == ApplicationConstant.PURPOSE_BR_ID) != null)
                // {
                //     var CheckNoBatchRequestEM = await _dataProvider.CheckNoBatchRequestEM(data.NoBatch, data.EmPhaseId, data.TestScenarioLabel, null);
                //     // Nomor batch boleh sama jika kombinasi antara fase produksi & no batch & kondisi & status request/sampling tersebut berbeda & location != "QC
                //     if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT
                //         && data.Location.ToLower() != ApplicationConstant.REQUEST_LOCATION_QC
                //         && CheckNoBatchRequestEM.Any())
                //     {
                //         result.StatusCode = ApplicationConstant.ERROR_CODE_400;
                //         result.Message = ApplicationConstant.MESSAGE_NO_BATCH_ALREADY_EXIST;
                //         return result;
                //     }
                // }


                //if (data.NoBatch != "" && data.NoBatch != null) //jika no batch tidak mandatory
                //{
                //    var checkRequestQcs = await _dataProvider.GetByBatchAndPhaseId(data.NoBatch, data.EmPhaseId);
                //    if (checkRequestQcs != null)
                //    {
                //        result.StatusCode = ApplicationConstant.ERROR_CODE_400;
                //        result.Message = $"Batch number is already exist in {checkRequestQcs.EmPhaseName}";
                //        return result;
                //    }
                //}

                /*foreach (var reqPurpose in data.RequestPurposes)
                {
                    if (data.NoBatch != "" && data.NoBatch != null)
                    {
                        if (reqPurpose.PurposeId == 3) //jika purpose request = batch related
                        {
                            var checkRequestQcs = await _dataProvider.GetByBatchAndPhaseId(data.NoBatch, data.EmPhaseId);
                            if (checkRequestQcs != null)
                            {
                                result.StatusCode = ApplicationConstant.ERROR_CODE_400;
                                result.Message = $"Batch number is already exist in {checkRequestQcs.EmPhaseName}";
                                return result;
                            }
                        }
                    }
                }*/


            }

            // if (!data.RequestRooms.Any())
            // {
            //     result.StatusCode = ApplicationConstant.ERROR_CODE_400;
            //     result.Message = $"Request Room data is empty"; // TODO : pindah ke const
            //     return result;
            // }

            // check facility
            //TransactionFacility getFacility = null;
            // int? orgRequestId = null;
            int? orgRequestBioHrId = null;
            string orgRequestName = null;
            // var getFacilityExt = new Facility();
            var getOrg = new Organization();
            if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT)
            {
                // if (data.IsSubmit)
                // {
                //     #region - insert into facility ext
                //     getFacilityExt = await _transactioDataProvider._InsertFacility(data.FacilityId);
                //     #endregion
                //     getOrg = await _transactioDataProvider.GetDetailById(getFacilityExt.OrganizationId);
                // }
                // else
                // {
                //     getFacilityExt = await _dataProviderFacility.GetById(data.FacilityId);
                //     getOrg = await _dataProviderOrganization.GetDetailById(getFacilityExt.OrganizationId);
                // }

                // if (getFacilityExt == null)
                // {
                //     result.StatusCode = ApplicationConstant.ERROR_CODE_400;
                //     result.Message = $"Facility Id data is empty"; // TODO : pindah ke const
                //     return result;
                // }

                // EM
                // if (getOrg != null)
                // {
                //     orgRequestBioHrId = getOrg.BIOHROrganizationId; //todo : cek kembali 
                //     orgRequestName = getOrg.Name;
                //     orgRequestId = getOrg.Id;
                // }
                // else
                // {
                //     result.StatusCode = ApplicationConstant.ERROR_CODE_400;
                //     result.Message = $"Organisazion Facility data is empty"; // TODO : pindah ke const
                //     return result;
                // }
            }

            // Generate no request
            var orgId = orgRequestBioHrId != null ? orgRequestBioHrId : null;
            var orgName = orgRequestName != null ? orgRequestName : null;
            string noRequest = "RANDOM NUMBER"; //await GenerateRequestNumber(data.Date, orgId, orgName);

            RequestQcs insertData = new RequestQcs()
            {
                Date = data.Date,
                OrgId = 0, //(orgRequestId != null ? orgRequestId : null),
                OrgName = "",//(orgRequestName != null ? orgRequestName : null),
                NoBatch = data.NoBatch.Trim(),
                NoRequest = noRequest,
                RequestQcsId = data.RequestQcsId,
                RequestQcsNo = data.RequestQcsNo,
                ItemId = data.ItemId,
                ItemName = data.ItemName,
                ProductFormId = data.ProductFormId,
                ProductFormName = data.ProductFormName,
                ProductGroupId = data.ProductGroupId,
                ProductGroupName = data.ProductGroupName,
                ProductPresentationId = data.ProductPresentationId,
                ProductPresentationName = data.ProductPresentationName,
                TypeRequestId = data.TypeRequestId,
                TypeRequest = data.TypeRequest,
                Status = statusRequest,
                StorageTemperatureId = data.StorageTemperatureId,
                StorageTemperatureName = data.StorageTemperatureName,
                PurposeId = data.PurposeId,
                PurposeName = data.PurposeName,
                
                ProductPhaseId = data.ProductPhaseId,
                ProductPhaseName = data.ProductPhaseName,

                ProcessId = data.ProcessId,
                ProcessName = data.ProcessName,

                CreatedBy = data.CreatedBy,
                UpdatedBy = data.CreatedBy,
                CreatedAt = DateHelper.Now(),
                UpdatedAt = DateHelper.Now(),

                //Conclussion Message init
                //Conclusion = ApplicationConstant.MSG_CONCLUSION_PASS
            };

            #region insert test types

            List<TestTypeQcs> insertToProductTestTypeQcs = new List<TestTypeQcs>();
            foreach (var testTypeQcs in data.TestTypesQcs)
            {
                TestTypeQcs dataTestTypeQcs = new TestTypeQcs()
                {
                    // Input Request Product ------------------------------------
                    PurposeId = testTypeQcs.PurposeId,
                    TestTypeId = testTypeQcs.TestTypeId,
                    TestTypeName = testTypeQcs.TestTypeName,
                    TestTypeMethodId = testTypeQcs.TestTypeMethodId,
                    TestTypeMethodName = testTypeQcs.TestTypeMethodName,
                    SampleAmountVolume = testTypeQcs.SampleAmountVolume,
                    SampleAmountUnit = testTypeQcs.SampleAmountUnit ,
                    SampleAmountPresentation = testTypeQcs.SampleAmountPresentation ,
                    SampleAmountCount = testTypeQcs.SampleAmountCount,
                    CreatedBy = data.CreatedBy,
                    UpdatedBy = data.CreatedBy,
                    OrgId = testTypeQcs.OrgId,
                    OrgName = testTypeQcs.OrgName,

                    CreatedAt = DateHelper.Now(),
                    UpdatedAt = DateHelper.Now()
                    //Status = 1
                };
                insertToProductTestTypeQcs.Add(dataTestTypeQcs);

            }

            #endregion

            _logger.LogInformation($"insertData : {insertData}");
            _logger.LogInformation($"insertToProductTestTypeQcs : {insertToProductTestTypeQcs}");

            if (insertData.TypeRequest.ToUpper().Contains("PRODUCT") || insertData.TypeRequest.ToUpper().Contains("PRODUK"))
            {
                if (insertData.ProductPhaseName == null || insertData.ItemName == null || insertData.NoBatch == null)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.MESSAGE_MUST_REQUIRED;
                    return result;
                }
            }

            var insertedRequestQcs = await _dataProvider.Insert(insertData, insertToProductTestTypeQcs);
            // get data QCSampling Id and WorkflowDocumentCode And 

            _logger.LogInformation($"simpan nih : {insertedRequestQcs}");

            var statusUpdate = ApplicationConstant.NEW_ACTION_NOTIF;
            var usernameModifier = await _auditTrailBusinessProvider.GetUsernameByNik(insertedRequestQcs.UpdatedBy);

            switch (statusRequest)
            {
                case ApplicationConstant.STATUS_REJECT:
                    statusUpdate = ApplicationConstant.REJECTED_ACTION_NOTIF;
                    await _auditTrailBusinessProvider.Add(ApplicationConstant.QC_REQUEST_STATUS_LABEL_REJECT, noRequest, insertedRequestQcs, usernameModifier);
                    break;
                case ApplicationConstant.STATUS_CANCEL:
                    statusUpdate = ApplicationConstant.CANCELLED_ACTION_NOTIF;
                    await _auditTrailBusinessProvider.Add(ApplicationConstant.QC_REQUEST_STATUS_LABEL_CANCEL, noRequest, insertedRequestQcs, usernameModifier);
                    break;
                case ApplicationConstant.STATUS_DRAFT:
                    statusUpdate = ApplicationConstant.NEW_ACTION_NOTIF;
                    await _auditTrailBusinessProvider.Add(ApplicationConstant.QC_REQUEST_STATUS_LABEL_DRAFT, noRequest, insertedRequestQcs, usernameModifier);
                    break;
                case ApplicationConstant.STATUS_SUBMIT:
                    statusUpdate = ApplicationConstant.UPDATED_ACTION_NOTIF;
                    await _auditTrailBusinessProvider.Add(ApplicationConstant.QC_REQUEST_STATUS_LABEL_DRAFT, noRequest, insertedRequestQcs, usernameModifier);
                    break;
                case ApplicationConstant.STATUS_APPROVED:
                    statusUpdate = ApplicationConstant.APPROVED_ACTION_NOTIF;
                    if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT)
                    {
                        await _auditTrailBusinessProvider.Add(ApplicationConstant.QC_REQUEST_STATUS_LABEL_APPROVE, noRequest, insertedRequestQcs, usernameModifier);
                    }
                    else if (data.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT)
                    {
                        await _auditTrailBusinessProvider.Add(ApplicationConstant.QC_REQUEST_STATUS_LABEL_COMPLETED, noRequest, insertedRequestQcs, usernameModifier);
                    }
                    break;
            }

            if (statusRequest >= ApplicationConstant.STATUS_SUBMIT)
            {
                //notif
                //TODO kenapa di-hardcode 1 ??
                if (data.TypeRequestId != ApplicationConstant.REQUEST_TYPE_PRODUCT && data.TypeRequestId != ApplicationConstant.REQUEST_TYPE_PRODUCT) //EM not sending notif PRODUCT SEMENTARA
                {
                    var personal = await _dataProvider.GetPersonalById(1);
                    if (personal != null)
                    {
                        _logger.LogInformation($"{personal.Id}");
                        MessageNotificationRequestQcsViewModel settingMessage = new MessageNotificationRequestQcsViewModel(insertedRequestQcs.NoRequest, insertedRequestQcs.NoBatch, insertedRequestQcs.ItemName, personal.Email, statusUpdate, personal.Name, personal.NoHandphone);
                        await _notification.SendEmailNotif(settingMessage);
                        await _notification.SendWhatsAppNotif(settingMessage);
                    }
                }
            }


            List<RequestQcs> listDataResult = new List<RequestQcs>() { insertedRequestQcs };

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = listDataResult;

            return result;

        }

        public async Task<ResponseViewModel<RequestQcsRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int orgId, int TypeRequestId)
        {
            var statusFilter = new List<int>();
            if (status == null)
            {

                statusFilter.Add(ApplicationConstant.STATUS_REJECT);
                statusFilter.Add(ApplicationConstant.STATUS_CANCEL);
                statusFilter.Add(ApplicationConstant.STATUS_DRAFT);
                statusFilter.Add(ApplicationConstant.STATUS_SUBMIT);
                statusFilter.Add(ApplicationConstant.STATUS_APPROVED);

            }
            else
            {
                // filter status from param status is string
                statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }


            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<RequestQcsRelationViewModel> result = new ResponseViewModel<RequestQcsRelationViewModel>();

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.END_DATE_LESS_THAN_BEGIN_DATE_ERROR_MESSAGE;

                    return result;
                }
            }

            List<RequestQcsRelationViewModel> getData = await _dataProvider.List(search, limit, pagination.CalculateOffset(), startDate, endDate, statusFilter, orgId, TypeRequestId);

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

        public async Task<ResponseViewModel<RequestQcsListViewModel>> ListShort(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int orgId, int TypeRequestId)
        {
            var statusFilter = new List<int>();
            if (status == null)
            {

                statusFilter.Add(ApplicationConstant.STATUS_REJECT);
                statusFilter.Add(ApplicationConstant.STATUS_CANCEL);
                statusFilter.Add(ApplicationConstant.STATUS_DRAFT);
                statusFilter.Add(ApplicationConstant.STATUS_SUBMIT);
                statusFilter.Add(ApplicationConstant.STATUS_APPROVED);

            }
            else
            {
                // filter status from param status is string
                statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<RequestQcsListViewModel> result = new ResponseViewModel<RequestQcsListViewModel>();

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.END_DATE_LESS_THAN_BEGIN_DATE_ERROR_MESSAGE;

                    return result;
                }
            }

            List<RequestQcsListViewModel> getData = await _dataProvider.ListShort(search, limit, pagination.CalculateOffset(), startDate, endDate, statusFilter, orgId, TypeRequestId);

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

        public async Task<ResponseViewModel<TestScenarioViewModel>> GetTestScenarioById(int requestQcId)
        {
            ResponseViewModel<TestScenarioViewModel> result = new ResponseViewModel<TestScenarioViewModel>();

            var getData = await _dataProvider.GetTestScenarioById(requestQcId);

            if (getData == null)
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

        private string GetMonthRomawi(DateTime date)
        {
            var onDate = date.Month;
            string romawi = "I";

            if (onDate == 2)
            {
                romawi = "II";
            }
            else if (onDate == 3)
            {
                romawi = "III";
            }
            else if (onDate == 4)
            {
                romawi = "IV";
            }
            else if (onDate == 5)
            {
                romawi = "V";
            }
            else if (onDate == 6)
            {
                romawi = "VI";
            }
            else if (onDate == 7)
            {

                romawi = "VII";
            }
            else if (onDate == 8)
            {
                romawi = "VIII";
            }
            else if (onDate == 9)
            {
                romawi = "IX";
            }
            else if (onDate == 10)
            {
                romawi = "X";
            }
            else if (onDate == 11)
            {
                romawi = "XI";
            }
            else if (onDate == 12)
            {
                romawi = "XII";
            }

            return romawi;
        }

        // public async Task<ResponseViewModel<RequestQcsBulk>> MassInsert(MassInsertRequestQcsBindingModel data)
        // {
        //     ResponseViewModel<RequestQcsBulk> result = new ResponseViewModel<RequestQcsBulk>();

        //     if (!data.MassRequestRooms.Any())
        //     {
        //         result.StatusCode = ApplicationConstant.ERROR_CODE_400;
        //         result.Message = $"Request Room data is empty";
        //         return result;
        //     }
            
        //     var getTestParam = await _transactioDataProvider.GetListTransactionTestParams();
        //     var samplingPointIdTrxExisting = new List<SamplingPointLiteViewModel>();

        //     int? orgRequestId = null;
        //     int? orgRequestBioHrId = null;
        //     string orgRequestName = null;
            
        //     var getFacilityExt = await _dataProviderFacility.GetById(data.FacilityId);
        //     var getOrg = await _dataProviderOrganization.GetDetailById(getFacilityExt.OrganizationId);

        //     if (getOrg != null)
        //     {
        //         orgRequestBioHrId = getOrg.BIOHROrganizationId; //todo : cek kembali 
        //         orgRequestName = getOrg.Name;
        //         orgRequestId = getOrg.Id;
        //     }
        //     else
        //     {
        //         result.StatusCode = ApplicationConstant.ERROR_CODE_400;
        //         result.Message = $"Organisazion Facility data is empty"; // TODO : pindah ke const
        //         return result;
        //     }

        //     var orgId = orgRequestBioHrId != null ? orgRequestBioHrId : null;
        //     var orgName = orgRequestName != null ? orgRequestName : null;

        //     string noRequest = await GenerateRequestNumber(DateHelper.Now(), orgId, orgName);
        //     var getPurpose = await _dataProviderPurpose.GetById(data.PurposeId);
            
            
        //     #region insert request rooms

        //     List<RequestQcs> listDataRequestQcs = new List<RequestQcs>();
        //     for (int bulkId = 0; bulkId < data.MassRequestRooms.Count(); ++bulkId)
        //     {
        //         var massReqRoom = data.MassRequestRooms.ElementAt(bulkId);
        //         // string mockNoRequest = String.Concat("bulk|", bulkId, "|", noRequest);
                
        //         if (!massReqRoom.RequestRooms.Any())
        //         {
        //             result.StatusCode = ApplicationConstant.ERROR_CODE_400;
        //             result.Message = $"Request Room data is empty"; 
        //             return result;
        //         }
                
        //         var noBatchGenerate = "";
        //         if (data.PurposeId == ApplicationConstant.PURPOSE_BR_ID)
        //         {
        //             Random random = new Random();

        //             noBatchGenerate = string.Concat("BULK/", random.Next(1000000).ToString());
        //             var checkNoBatchRequestEm = await _dataProvider.CheckNoBatchRequestEM(noBatchGenerate, 
        //                 null, ApplicationConstant.TEST_SCENARIO_IN_OPERATION, null);
                    
        //             // Nomor batch boleh sama jika kombinasi antara fase produksi & no batch & kondisi & status request/sampling tersebut berbeda & location != "QC
        //             if (data.Location.ToLower() != ApplicationConstant.REQUEST_LOCATION_QC && checkNoBatchRequestEm.Any())
        //             {
        //                 result.StatusCode = ApplicationConstant.ERROR_CODE_400;
        //                 result.Message = ApplicationConstant.MESSAGE_NO_BATCH_ALREADY_EXIST;
        //                 return result;
        //             }
        //         }
                
        //         RequestQcs insertData = new RequestQcs()
        //         {
        //             Date = DateHelper.Now(),
        //             OrgId = (orgRequestId != null ? orgRequestId : null),
        //             OrgName = (orgRequestName != null ? orgRequestName : null),
        //             NoBatch = null,
        //             NoRequest = noRequest,
        //             ItemId = 0,
        //             ItemName = null,
        //             ProductFormId = 0,
        //             ProductFormName = null,
        //             ProductTemperature = null,
        //             ProductGroupId = 0,
        //             ProductGroupName = null,
        //             ProductPresentationId = 0,
        //             ProductPresentationName = null,
        //             TypeFormId = 0,
        //             TypeFormName = null,
        //             TypeRequestId = ApplicationConstant.REQUEST_TYPE_PRODUCT,
        //             TypeRequest = "Environment Monitoring",
        //             Status = ApplicationConstant.STATUS_DRAFT,
        //             StorageTemperatureId = null,
        //             StorageTemperatureName = null,
        //             ProductPhaseId = 0,
        //             ProductPhaseName = null,
        //             TestScenarioLabel = null,
        //             EmPhaseId = 0,
        //             EmPhaseName = null,
        //             FacilityId = (getFacilityExt != null ? getFacilityExt.Id : 0),
        //             FacilityCode = (getFacilityExt != null ? getFacilityExt.Code : null),
        //             FacilityName = (getFacilityExt != null ? getFacilityExt.Name : null),
        //             Location = data.Location,
        //             ProcessDate = null,
        //             ProcessId = null,
        //             ProcessName = null,
        //             ItemTemperature = null,
        //             CreatedBy = data.CreatedBy,
        //             UpdatedBy = data.CreatedBy,
        //             CreatedAt = DateHelper.Now(),
        //             UpdatedAt = DateHelper.Now(),
        //             IsFromBulkRequest = true
        //         };

                
        //         #region insert request AHU
        //         List<RequestAhu> insertRequestAhu = new List<RequestAhu>();
        //         List<RoomRelationViewModel> listByFacilityAhu = await _dataProviderRoom
        //             .ListByFacilityAHU("", getFacilityExt.Id, new List<int>());
        //         if (listByFacilityAhu.Any())
        //         {
        //             var listAhu = listByFacilityAhu.Select(room => room.AhuId).Distinct();
        //             foreach (var ahuId in listAhu)
        //             {
        //                 var ahu = new ToolsAHUViewModel();
        //                 var getAhuExt = await _dataProviderTool.getAhuById(ahuId ?? 0);
                        
        //                 if (getAhuExt == null)
        //                 {
        //                     result.StatusCode = ApplicationConstant.ERROR_CODE_400;
        //                     result.Message = $"Ahu Id {ahuId} data is empty"; // TODO : pindah ke const
        //                     return result;
        //                 }

        //                 ahu.ToolCode = getAhuExt.ToolCode;
        //                 ahu.ToolId = getAhuExt.ToolId;
        //                 ahu.ToolName = getAhuExt.ToolName;

        //                 RequestAhu dataRequestAhu = new RequestAhu()
        //                 {
        //                     AhuId = ahu.ToolId,
        //                     AhuCode = ahu.ToolCode,
        //                     AhuName = ahu.ToolName,
        //                     CreatedBy = data.CreatedBy,
        //                     UpdatedBy = data.CreatedBy,
        //                     CreatedAt = DateHelper.Now(),
        //                     UpdatedAt = DateHelper.Now()
        //                 };
        //                 insertRequestAhu.Add(dataRequestAhu);
        //             }
        //         }
        //         #endregion
                
        //         #region insert request purpose

        //         List<RequestPurpose> insertRequestPurpose = new List<RequestPurpose>();

        //         if (getPurpose == null)
        //         {
        //             result.StatusCode = ApplicationConstant.ERROR_CODE_400;
        //             result.Message = $"Purpose Id {data.PurposeId} data is empty"; // TODO : pindah ke const
        //             return result;
        //         }

        //         RequestPurpose dataRequestPurpose = new RequestPurpose()
        //         {
        //             PurposeId = data.PurposeId,
        //             PurposeCode = getPurpose.Code,
        //             PurposeName = getPurpose.Name,
        //             // ----------------------------------------------------------
        //             CreatedBy = data.CreatedBy,
        //             UpdatedBy = data.CreatedBy,
        //             CreatedAt = DateHelper.Now(),
        //             UpdatedAt = DateHelper.Now()
        //         };

        //         insertRequestPurpose.Add(dataRequestPurpose);

        //         #endregion

        //         List<RequestRoom> insertRequestRooms = new List<RequestRoom>();
        //         foreach (var reqRoom in massReqRoom.RequestRooms)
        //         {
        //             var getRoom = await _dataProviderRoom.GetRoomRelationDetailById(reqRoom.RoomId);
        //             if (getRoom == null)
        //             {
        //                 result.StatusCode = ApplicationConstant.ERROR_CODE_400;
        //                 result.Message = $"Room Id {reqRoom.RoomId} data is empty"; // TODO : pindah ke const
        //                 return result;
        //             }

        //             var getTestScenarioTrx = await _dataProviderTestScenario.GetByGradeRoomLabel(getRoom.GradeRoomId, ApplicationConstant.TEST_SCENARIO_IN_OPERATION);
        //             if (getTestScenarioTrx == null)
        //             {
        //                 result.StatusCode = ApplicationConstant.ERROR_CODE_400;
        //                 result.Message = $"Grade Room {getRoom.GradeRoomCode}, Test Scenario data is empty"; // TODO : pindah ke const
        //                 return result;
        //             }

        //             RequestRoom dataReqRoom = new RequestRoom()
        //             {
        //                 RoomId = getRoom.RoomId,
        //                 RoomCode = getRoom.RoomCode,
        //                 RoomName = getRoom.RoomName,
        //                 GradeRoomId = getRoom.GradeRoomId,
        //                 GradeRoomCode = getRoom.GradeRoomCode,
        //                 GradeRoomName = getRoom.GradeRoomName,
        //                 AhuId = getRoom.AhuId,
        //                 AhuCode = getRoom.AhuCode,
        //                 AhuName = getRoom.AhuName,

        //                 TestScenarioId = 0,
        //                 TestScenarioName = "",
        //                 TestScenarioLabel = ApplicationConstant.TEST_SCENARIO_IN_OPERATION,

        //                 CreatedBy = data.CreatedBy,
        //                 UpdatedBy = data.CreatedBy,
        //                 CreatedAt = DateHelper.Now(),
        //                 UpdatedAt = DateHelper.Now()
        //             };
        //             insertRequestRooms.Add(dataReqRoom);
        //         }
                
        //         #region insert test types

        //         List<TestTypeQcs> insertToProductTestTypeQcs = new List<TestTypeQcs>();
        //         var listRoomIds = insertRequestRooms.Select(room => room.RoomId).Distinct().ToList();
                
        //         List<TestParameterByScenarioRelationViewModel> listTestTypesQcs = await _dataTestParameterProvider
        //             .ListV2(listRoomIds, "in_operation", new List<int> { data.PurposeId });

        //         foreach (var testTypeQcs in listTestTypesQcs)
        //         {
        //             foreach (var testParameters in testTypeQcs.TestParameter)
        //             {
        //                 var index = (insertToProductTestTypeQcs.FindIndex(X =>
        //                     X.TestParameterId == testParameters.TestParameterId));

        //                 if (index >= 0)
        //                 {
        //                     insertToProductTestTypeQcs[index].SampleAmountCount += testParameters.CountParamater ?? 0;
        //                 }
        //                 else
        //                 {
        //                     TestTypeQcs dataTestTypeQcs = new TestTypeQcs()
        //                     {
        //                         TestTypeId = 0,
        //                         TestTypeName = null,
        //                         SampleAmountVolume = 0,
        //                         SampleAmountUnit = null,
        //                         SampleAmountPresentation = null,
        //                         TestParameterId = testParameters.TestParameterId,
        //                         TestParameterName = testParameters.TestParameterName,
        //                         SampleAmountCount = testParameters.CountParamater ?? 0,
        //                         CreatedBy = data.CreatedBy,
        //                         UpdatedBy = data.CreatedBy,
        //                         OrgId = orgId ?? 0,
        //                         OrgName = orgName,

        //                         CreatedAt = DateHelper.Now(),
        //                         UpdatedAt = DateHelper.Now()
        //                     };
                            
        //                     insertToProductTestTypeQcs.Add(dataTestTypeQcs);

        //                 }
        //             }
        //         }
                
        //         #endregion


        //         if (insertData.TypeRequestId == 2 ||
        //             insertData.TypeRequest.ToUpper().Contains("ENVIRONMENT MONITORING"))
        //         {
        //             if (insertData.FacilityId == null || data.PurposeId == 0)
        //             {
        //                 result.StatusCode = 400;
        //                 result.Message = ApplicationConstant.MESSAGE_MUST_REQUIRED;
        //                 return result;
        //             }
        //         }

        //         var batchObjectInitial = new InsertBatchRequestQcBindingModel()
        //         {
        //             AttachmentNotes = "",
        //             Lines = new List<InsertBatchLineRequestQcBindingModel>(),
        //             Attachments = new List<InsertBatchAttachmentRequestQcBindingModel>()
        //         };

        //         var insertedRequestQcs = await _dataProvider.Insert(insertData, insertToProductTestTypeQcs, 
        //             insertRequestPurpose, insertRequestRooms, new List<RequestAhu>(), batchObjectInitial);

        //         listDataRequestQcs.Add(insertedRequestQcs);

        //     }

        //     #endregion


        //     List<RequestQcsBulk> listDataResult = new List<RequestQcsBulk>
        //     {
        //         new RequestQcsBulk()
        //         {
        //             RequestQcs = listDataRequestQcs,
        //             NoRequest = noRequest,
        //             PurposeId = data.PurposeId,
        //             PurposeName = getPurpose.Code,
        //             FacilityId = getFacilityExt.Id,
        //             FacilityCode = getFacilityExt.Code,
        //             FacilityName = getFacilityExt.Name
        //         }
        //     };

        //     result.StatusCode = 200;
        //     result.Message = ApplicationConstant.OK_MESSAGE;
        //     result.Data = listDataResult;

        //     return result;
        // }
    }
}
