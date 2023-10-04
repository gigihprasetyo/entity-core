using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Helpers;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class SamplingShipmentBusinessProvider : ISamplingShipmentBusinessProvider
    {
        private readonly ISamplingShipmentDataProvider _dataProvider;
        private readonly IQcRequestDataProvider _dataProviderRequestQc;
        private readonly IQcSamplingDataProvider _dataProviderSamplingQc;
        private readonly INotificationServiceBusinessProvider _notification;
        private readonly ILogger<SamplingShipmentBusinessProvider> _logger;
        private readonly IBioHRIntegrationBussinesProviders _dataProviderBioHR;
        private readonly IAuditTrailBusinessProvider _auditTrailBusinessProvider;
        private readonly IDigitalSignatureDataProvider _digitalSignatureDataProvider;
        private readonly IAUAMServiceBusinessProviders _auamServiceBusinessProviders;

        [ExcludeFromCodeCoverage]
        public SamplingShipmentBusinessProvider(
            ISamplingShipmentDataProvider dataProvider,
            IQcRequestDataProvider dataProviderRequestQc,
            IQcSamplingDataProvider dataProviderSamplingQc,
            INotificationServiceBusinessProvider notification,
            ILogger<SamplingShipmentBusinessProvider> logger,
            IBioHRIntegrationBussinesProviders dataProviderBioHR,
            IAuditTrailBusinessProvider auditTrailBusinessProvider,
            IDigitalSignatureDataProvider digitalSignatureDataProvider,
            IAUAMServiceBusinessProviders auamServiceBusinessProviders)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _dataProviderRequestQc = dataProviderRequestQc ?? throw new ArgumentNullException(nameof(dataProviderRequestQc));
            _dataProviderSamplingQc = dataProviderSamplingQc ?? throw new ArgumentNullException(nameof(dataProviderSamplingQc));
            _logger = logger;
            _notification = notification;
            _dataProviderBioHR = dataProviderBioHR;
            _auditTrailBusinessProvider = auditTrailBusinessProvider;
            _digitalSignatureDataProvider = digitalSignatureDataProvider ?? throw new ArgumentNullException(nameof(digitalSignatureDataProvider));
            _auamServiceBusinessProviders = auamServiceBusinessProviders;
        }

        public async Task<ResponseViewModel<QcSamplingShipmentRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int fromOrgId, int toOrgId, int qcSamplingId)
        {
            var statusFilter = new List<int>();
            if (status == null)
            {
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_SENDING);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_INTRANSIT);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_RECEIVED);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_LATE_SAMPLE);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_LATE_REVIEWED);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_LATE_RECIVED);
            }
            else
            {
                // filter status from param status is string
                statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<QcSamplingShipmentRelationViewModel> result = new ResponseViewModel<QcSamplingShipmentRelationViewModel>();

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.END_DATE_LESS_THAN_BEGIN_DATE_ERROR_MESSAGE;

                    return result;
                }
            }

            List<QcSamplingShipmentRelationViewModel> getData = await _dataProvider.List(search, limit, pagination.CalculateOffset(), startDate, endDate, statusFilter, fromOrgId, toOrgId, qcSamplingId);

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
        public async Task<ResponseViewModel<QcShipmentLateViewModel>> ListLate(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int OrgId, string nik)
        {
            var statusFilter = new List<int>();
            if (status == null)
            {
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_LATE_SAMPLE);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_LATE_REVIEWED);
                /*statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_LATE_RECIVED);*/
            }
            else
            {
                // filter status from param status is string
                statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<QcShipmentLateViewModel> result = new ResponseViewModel<QcShipmentLateViewModel>();

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.END_DATE_LESS_THAN_BEGIN_DATE_ERROR_MESSAGE;

                    return result;
                }
            }

            List<QcShipmentLateViewModel> getData = await _dataProvider.ListLate(search, limit, pagination.CalculateOffset(), startDate, endDate, statusFilter, OrgId, nik);

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
        public async Task<ResponseViewModel<QcSamplingTransferViewModel>> ListTransfer(string search, int limit, int page, DateTime? startDate, DateTime? endDate, int status)
        {
            if (limit == 0 || limit < 0) limit = 10;

            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<QcSamplingTransferViewModel> result = new ResponseViewModel<QcSamplingTransferViewModel>();

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.END_DATE_LESS_THAN_BEGIN_DATE_ERROR_MESSAGE;

                    return result;
                }
            }

            List<QcSamplingTransferViewModel> getData = await _dataProvider.ListTransfer(search, limit, pagination.CalculateOffset(), startDate, endDate, status);

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

        public async Task<ResponseOneDataViewModel<QcShipmentLateViewModel>> GetTransferLateBySamplingId(int qcSamplingId, string nik)
        {
            ResponseOneDataViewModel<QcShipmentLateViewModel> result = new ResponseOneDataViewModel<QcShipmentLateViewModel>();
            var getData = await _dataProvider.GetTransferLateBySamplingId(qcSamplingId, nik);

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

        public async Task<ResponseOneDataViewModel<QcSamplingTransferViewModel>> GetTransferDetail(Int32 sampleId)
        {
            ResponseOneDataViewModel<QcSamplingTransferViewModel> result = new ResponseOneDataViewModel<QcSamplingTransferViewModel>();
            var getData = await _dataProvider.GetTransferDetail(sampleId);

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

        public async Task<ResponseOneDataViewModel<QcSamplingShipmentRelationViewModel>> GetById(int id)
        {
            ResponseOneDataViewModel<QcSamplingShipmentRelationViewModel> result = new ResponseOneDataViewModel<QcSamplingShipmentRelationViewModel>();
            var getData = await _dataProvider.GetById(id);

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

        public async Task<ResponseOneDataViewModel<QcSamplingShipment>> InsertSending(InsertSamplingShipmentBindingModel data)
        {
            ResponseOneDataViewModel<QcSamplingShipment> result = new ResponseOneDataViewModel<QcSamplingShipment>();
            List<ShipmentResponseMessageViewModel> outputMessage = new List<ShipmentResponseMessageViewModel>();

            var getOrganization = await _dataProviderBioHR.GetOrganizationById(Int32.Parse(data.OrgId));
            var getEmployee = await _dataProviderBioHR.GetEmployeeByNik(data.UpdatedBy);
            var getEmployeeAUAM = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(data.UpdatedBy);

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


            List<QcSamplingShipment> samplingShipmentCreatedList = new List<QcSamplingShipment>();
            List<QcSamplingShipmentTracker> samplingShipmentTrackerCreatedList = new List<QcSamplingShipmentTracker>();
            var countCreated = 0;
            var countFailedCreate = 0;
            if (!data.QRCodes.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            foreach (var QRScan in data.QRCodes)
            {
                var getDataShipment = await _dataProvider.GetByQRCode(QRScan.QRCode);

                //create insert header shipment if new shipment
                if (!getDataShipment.Any())
                {
                    //get data sampling for sending
                    var getDataSampling = await _dataProviderSamplingQc.GetSamplingBatchByQRCode(QRScan.QRCode);
                    
                    if (getDataSampling == null)
                    {
                        countFailedCreate++;

                        ShipmentResponseMessageViewModel insertMessageRes = new ShipmentResponseMessageViewModel()
                        {
                            QRCode = QRScan.QRCode,
                            Description = ApplicationConstant.FAILED_QRCODE_NOT_FOUND_MESSAGE,
                            Status = ApplicationConstant.STATUS_FAILED_STR
                        };
                        outputMessage.Add(insertMessageRes);
                    }
                    else
                    {

                        if (getDataSampling.Testparameters.Any())
                        {
                            //insert by test param sampling
                            foreach (var testParamSampling in getDataSampling.Testparameters)
                            {
                                QcSamplingShipment insertShipment = new QcSamplingShipment()
                                {
                                    QcSamplingId = getDataSampling.SamplingId,
                                    QrCode = getDataSampling.Code,
                                    NoRequest = getDataSampling.NoRequest,
                                    TestParamId = testParamSampling.Id,
                                    TestParamName = testParamSampling.Name,
                                    FromOrganizationId =
                                        Int32.Parse(data.OrgId), //harus di ganti dengan org id pengirim
                                    FromOrganizationName = (getOrganization != null
                                        ? getOrganization.OrganizationName
                                        : "Undefined"), //harus di ganti dengan org id pengirim
                                    ToOrganizationId = testParamSampling.OrgId,
                                    ToOrganizationName = testParamSampling.OrgName,
                                    StartDate = Convert.ToDateTime(data.ShipmentDate),
                                    Status = ApplicationConstant.STATUS_SHIPMENT_SENDING,
                                    CreatedBy = data.UpdatedBy,
                                    UpdatedBy = data.UpdatedBy,
                                    CreatedAt = DateHelper.Now(),
                                    UpdatedAt = DateHelper.Now()
                                };

                                QcSamplingShipmentTracker insertShipmentTracker = new QcSamplingShipmentTracker()
                                {
                                    QrCode = getDataSampling.Code,
                                    Type = ApplicationConstant.TRACKER_TYPE_SEND,
                                    processAt = Convert.ToDateTime(data.ShipmentDate),
                                    IdLogger = data.ShipmentIdLogger,
                                    Temperature = data.ShipmentTemperature,
                                    UserNik = data.UpdatedBy,
                                    UserName = (getEmployee != null ? getEmployee.Nama : "Pengirim"), //TODO harus ganti get by nik di bio hr
                                    OrganizationId = Int32.Parse(data.OrgId), //TODO harus di ganti dengan org id pengirim
                                    OrganizationName = (getOrganization != null ? getOrganization.OrganizationName : "Undefined"), 
                                    CreatedAt = DateHelper.Now(),
                                    UpdatedAt = DateHelper.Now()
                                };

                                //insert to data provider
                                var insertNewShipment = await _dataProvider.InsertNewShipment(insertShipment,
                                    insertShipmentTracker, ApplicationConstant.STATUS_SHIPMENT_SENDING);
                                countCreated++;

                                samplingShipmentCreatedList.Add(insertNewShipment);
                                samplingShipmentTrackerCreatedList.Add(insertShipmentTracker);

                                ShipmentResponseMessageViewModel insertMessageRes =
                                    new ShipmentResponseMessageViewModel()
                                    {
                                        QRCode = QRScan.QRCode,
                                        Description = ApplicationConstant.SUCCESS_PACKAGE_ALREADY_SEND_MESSAGE,
                                        Status = ApplicationConstant.STATUS_SUCCESS_STR
                                    };
                                outputMessage.Add(insertMessageRes);
                            } //end loop insert by test param
                        } //end check test param by get
                    } //end check get shipment if avail
                }
                else
                {
                    //if shipment already in sending
                    countFailedCreate++;

                    ShipmentResponseMessageViewModel insertMessageRes = new ShipmentResponseMessageViewModel()
                    {
                        QRCode = QRScan.QRCode,
                        Description = ApplicationConstant.FAILED_PACKAGE_NOT_YET_RECIVED_MESSAGE,
                        Status = ApplicationConstant.STATUS_FAILED_STR
                    };
                    outputMessage.Add(insertMessageRes);
                }
            }

            var resOutput = new ShipmentResponseVIewModel()
            {
                TotalSuccess = countCreated,
                TotalFailed = countFailedCreate,
                DetailMessages = outputMessage
            };

            if (countCreated > 0)
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = resOutput;

                var usernameModifier = await _auditTrailBusinessProvider.GetUsernameByNik(data.UpdatedBy);
                foreach (var qcSamplingShipment in samplingShipmentCreatedList)
                {
                    await _auditTrailBusinessProvider.Add(ApplicationConstant.QS_SAMPLING_STATUS_LABEL_SEND, qcSamplingShipment.QrCode, qcSamplingShipment, usernameModifier);

                    foreach (var qcSamplingShipmentTracker in samplingShipmentTrackerCreatedList)
                    {
                        if (qcSamplingShipmentTracker.QcSamplingShipmentId == qcSamplingShipment.Id)
                        {
                            await _auditTrailBusinessProvider.Add(ApplicationConstant.QS_SAMPLING_STATUS_LABEL_SEND, qcSamplingShipment.QrCode, qcSamplingShipmentTracker, qcSamplingShipment, usernameModifier);
                        }
                    }
                }
            }
            else
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.FAILED_INSERT_MESSAGE;
                result.Data = resOutput;
            }

            return result;
        }

        public async Task<ResponseOneDataViewModel<QcSamplingShipment>> InsertReceiving(InsertSamplingShipmentBindingModel data)
        {
            ResponseOneDataViewModel<QcSamplingShipment> result = new ResponseOneDataViewModel<QcSamplingShipment>();

            var getOrganization = await _dataProviderBioHR.GetOrganizationById(Int32.Parse(data.OrgId));
            var getEmployee = await _dataProviderBioHR.GetEmployeeByNik(data.UpdatedBy);

            List<QcSamplingShipment> output = new List<QcSamplingShipment>();
            var countCreated = 0;
            var countFailedCreate = 0;
            if (!data.QRCodes.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            var outputMessages = new List<ShipmentResponseMessageViewModel>();

            /* Check Interval Date sampling and sending */
            var statusShipment = ApplicationConstant.STATUS_SHIPMENT_RECEIVED;
            var statusShipmentLabel = ApplicationConstant.QS_SAMPLING_STATUS_LABEL_RECEIVE;

            /*
             * Untuk pesan error dibuat unique per QR Code
             */
            foreach (var QRScan in data.QRCodes)
            {
                var getDataShipment = await _dataProvider.GetShipmentHeaderByQRCode(QRScan.QRCode);

                //create insert header shipment if new shipment
                if (!getDataShipment.Any())
                {
                    countFailedCreate++;

                    ShipmentResponseMessageViewModel insertMessageRes = new ShipmentResponseMessageViewModel()
                    {
                        QRCode = QRScan.QRCode,
                        Description = ApplicationConstant.FAILED_PACKAGE_NOT_YET_SEND_MESSAGE,
                        Status = ApplicationConstant.STATUS_FAILED_STR
                    };
                    outputMessages.Add(insertMessageRes);

                }
                else
                {
                    //if shipment already in sending
                    foreach (var shipment in getDataShipment)
                    {
                        //shipment late transfer validation
                        if (shipment.IsLateTransfer == true)
                        {
                            statusShipment = ApplicationConstant.STATUS_SHIPMENT_LATE_RECIVED;

                            if (shipment.Status != ApplicationConstant.STATUS_SHIPMENT_LATE_REVIEWED)
                            {
                                result.StatusCode = 403;
                                result.Message = ApplicationConstant.MESSAGE_INVALID_SAMPLE;

                                return result;
                            }
                        }


                        //if shipment already received
                        if (shipment.Status == ApplicationConstant.STATUS_SHIPMENT_RECEIVED || shipment.Status == ApplicationConstant.STATUS_SHIPMENT_LATE_RECIVED)
                        {
                            countFailedCreate++;

                            ShipmentResponseMessageViewModel insertMessageRes = new ShipmentResponseMessageViewModel()
                            {
                                QRCode = QRScan.QRCode,
                                TestParamName = shipment.TestParamName,
                                TestParamId = shipment.TestParamId,
                                Description = ApplicationConstant.FAILED_PACKAGE_ALREADY_RECIVED_MESSAGE,
                                Status = ApplicationConstant.STATUS_FAILED_STR
                            };

                            if (outputMessages.Exists(x => x.QRCode == insertMessageRes.QRCode))
                            {
                                continue;
                            }

                            outputMessages.Add(insertMessageRes);
                            continue;
                        }


                        //TODO handling jika pengiriman dilakukan ke koordinator terlebih dahulu
                        //cek apakah tanggal kirim lebih besar dari tanggal penerimaan
                        if (shipment.StartDate.HasValue && shipment.StartDate.Value.CompareTo(data.ShipmentDate) > 0)
                        {
                            countFailedCreate++;
                            var errorMsg = string.Format(ApplicationConstant.FAILED_PACKAGE_SEND_DATE_GREATER_THAN_RECEIVE_DATE, shipment.QrCode);
                            ShipmentResponseMessageViewModel insertMessageRes = new ShipmentResponseMessageViewModel()
                            {
                                QRCode = QRScan.QRCode,
                                TestParamName = shipment.TestParamName,
                                TestParamId = shipment.TestParamId,
                                Description = errorMsg,
                                Status = ApplicationConstant.STATUS_FAILED_STR
                            };

                            if (outputMessages.Exists(x => x.QRCode == insertMessageRes.QRCode))
                            {
                                continue;
                            }

                            outputMessages.Add(insertMessageRes);
                            continue;
                        }

                        //if the destination of the shipment matches
                        try
                        {
                            QcSamplingShipmentTracker insertShipmentTracker = new QcSamplingShipmentTracker()
                            {
                                QrCode = shipment.QrCode,
                                Type = ApplicationConstant.TRACKER_TYPE_RECEIVE,
                                processAt = Convert.ToDateTime(data.ShipmentDate),
                                IdLogger = data.ShipmentIdLogger,
                                Temperature = data.ShipmentTemperature,
                                UserNik = data.UpdatedBy,
                                UserName = (getEmployee != null ? getEmployee.Nama : "Pengirim"), //TODO harus ganti get by nik di bio hr
                                OrganizationId = Int32.Parse(data.OrgId), //TODO harus di ganti dengan org id pengirim
                                OrganizationName = (getOrganization != null ? getOrganization.OrganizationName : "Undefined"), //harus di ganti dengan org id pengirim
                                CreatedAt = DateHelper.Now(),
                                UpdatedAt = DateHelper.Now()
                            };

                            shipment.Status = statusShipment;
                            shipment.EndDate = Convert.ToDateTime(data.ShipmentDate);
                            shipment.UpdatedBy = data.UpdatedBy;
                            shipment.UpdatedAt = DateHelper.Now();

                            //insert to data provider
                            await _dataProvider.InsertNewShipment(shipment, insertShipmentTracker, statusShipment);

                            ShipmentResponseMessageViewModel insertMessageRes = new ShipmentResponseMessageViewModel()
                            {
                                QRCode = QRScan.QRCode,
                                TestParamName = shipment.TestParamName,
                                TestParamId = shipment.TestParamId,
                                Description = ApplicationConstant.SUCCESS_PACKAGE_ALREADY_RECIVED_MESSAGE,
                                Status = ApplicationConstant.STATUS_SUCCESS_STR
                            };

                            outputMessages.Add(insertMessageRes);

                            countCreated++;

                            var usernameModifier = await _auditTrailBusinessProvider.GetUsernameByNik(data.UpdatedBy);
                            //audit trail shipment received
                            await _auditTrailBusinessProvider.Add(statusShipmentLabel, shipment.QrCode, shipment, usernameModifier);

                            //audit trail shipment tracker received
                            await _auditTrailBusinessProvider.Add(ApplicationConstant.QS_SAMPLING_STATUS_LABEL_RECEIVE, insertShipmentTracker.QrCode, insertShipmentTracker, shipment, usernameModifier);



                        }
                        catch (Exception e)
                        {
                            _logger?.LogError(e, "{Message}", e.Message);

                            var errorMsg = string.Format(ApplicationConstant.FAILED_PACKAGE_RECEIVE_MESSAGE, shipment.QrCode, shipment.TestParamName);
                            ShipmentResponseMessageViewModel insertMessageRes = new ShipmentResponseMessageViewModel()
                            {
                                QRCode = QRScan.QRCode,
                                TestParamName = shipment.TestParamName,
                                TestParamId = shipment.TestParamId,
                                Description = errorMsg,
                                Status = ApplicationConstant.STATUS_FAILED_STR
                            };
                            outputMessages.Add(insertMessageRes);
                        }
                    }
                }
            }

            var resOutput = new ShipmentResponseVIewModel()
            {
                TotalSuccess = countCreated,
                TotalFailed = countFailedCreate,
                DetailMessages = outputMessages
            };

            if (countCreated > 0)
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = resOutput;
                return result;
            }

            result.StatusCode = 403;
            result.Message = ApplicationConstant.FAILED_INSERT_MESSAGE;
            result.Data = resOutput;

            return result;
        }

        public async Task<ResponseOneDataViewModel<QcSampling>> InsertApproval(InsertApprovalShipmentBindingModel data)
        {
            ResponseOneDataViewModel<QcSampling> result = new ResponseOneDataViewModel<QcSampling>();

            /* Get Sampling*/
            var getDataSampling = await _dataProviderSamplingQc.GetById(data.DataId);
            if (getDataSampling == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            if (getDataSampling.ShipmentApprovalDate != null)
            {
                result.StatusCode = 403;
                result.Message = ApplicationConstant.MESSAGE_SHIPMENT_ALREADY_APPROVED;
                return result;
            }

            /* Get Digital Signature */
            var getDigitalSignature = await _digitalSignatureDataProvider.Authenticate(data.DigitalSignature, data.NIK);
            if (getDigitalSignature == false)
            {
                result.StatusCode = 403;
                result.Message = ApplicationConstant.WRONG_DIGITAL_SIGNATURE;
                return result;
            }

            if (data.Notes == null || data.Notes.Length <= 0)
            {
                result.StatusCode = 403;
                result.Message = ApplicationConstant.MESSAGE_NOTE_REQUIRED;
                return result;
            }

            if (data.Notes.Length > 200)
            {
                result.StatusCode = 403;
                result.Message = ApplicationConstant.NOTES_TO_LONG_MESSAGE;
                return result;
            }

            /* Get Shipment*/
            var getDataShipment = await _dataProvider.GetBySamplingId(data.DataId);
            if (!getDataShipment.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            /* Check late shipment */
            foreach (var sh in getDataShipment)
            {
                if (sh.IsLateTransfer == false)
                {
                    result.StatusCode = 403;
                    result.Message = ApplicationConstant.MESSAGE_SHIPMENT_NOT_LATE;
                    return result;
                }

                if (sh.EndDate != null)
                {
                    result.StatusCode = 403;
                    result.Message = ApplicationConstant.MESSAGE_SHIPMENT_ALREADY_RECIVED;
                    return result;
                }
            }

            /* Update Sampling for approve late TF*/
            await _dataProvider.UpdateApprovalSampling(data);
            /* Get Sampling*/
            var DataSampling = await _dataProviderSamplingQc.GetById(data.DataId);

            /* Get Shipment*/
            var getDataShipmentAfterEdit = await _dataProvider.GetBySamplingId(data.DataId);
            if (getDataShipmentAfterEdit.Any())
            {
                var usernameModifier = await _auditTrailBusinessProvider.GetUsernameByNik(data.NIK);
                foreach (var qcSamplingShipment in getDataShipmentAfterEdit)
                {
                    await _auditTrailBusinessProvider.Add(ApplicationConstant.QS_SAMPLING_STATUS_LABEL_REVIEW, qcSamplingShipment.QrCode, qcSamplingShipment, usernameModifier);
                }
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = DataSampling;

            return result;
        }

        public async Task<ResponseViewModel<QcSamplingShipmentRelationViewModelV2>> ListByBatch(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int fromOrgId, int toOrgId)
        {
            var statusFilter = new List<int>();
            if (status == null)
            {
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_SENDING);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_INTRANSIT);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_RECEIVED);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_LATE_SAMPLE);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_LATE_REVIEWED);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_LATE_RECIVED);
            }
            else
            {
                // filter status from param status is string
                statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<QcSamplingShipmentRelationViewModelV2> result = new ResponseViewModel<QcSamplingShipmentRelationViewModelV2>();

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.END_DATE_LESS_THAN_BEGIN_DATE_ERROR_MESSAGE;

                    return result;
                }
            }

            List<QcSamplingShipmentRelationViewModelV2> getData = await _dataProvider.ListByBatch(search, limit, pagination.CalculateOffset(), startDate, endDate, statusFilter, fromOrgId, toOrgId);

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

        public async Task<ResponseOneDataViewModel<QcSamplingShipmentDetailRelationViewModel>> GetByRequestQcsId(int requestQcsId)
        {
            ResponseOneDataViewModel<QcSamplingShipmentDetailRelationViewModel> result = new ResponseOneDataViewModel<QcSamplingShipmentDetailRelationViewModel>();
            var getData = await _dataProvider.GetByRequestQcsId(requestQcsId);

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

        public async Task<ResponseOneDataViewModel<QcShipmentLateDetailViewModel>> GetTransferLateByRequestQcsId(int requestQcsId)
        {
            ResponseOneDataViewModel<QcShipmentLateDetailViewModel> result = new ResponseOneDataViewModel<QcShipmentLateDetailViewModel>();
            var getData = await _dataProvider.GetTransferLateByRequestQcsId(requestQcsId);

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
    }
}
