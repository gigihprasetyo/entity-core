using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.IntegrationEvents;
using qcs_product.API.BusinessProviders;
using qcs_product.API.BusinessProviders.Collection;
using qcs_product.API.DataProviders;
using qcs_product.API.DataProviders.Collection;
using qcs_product.API.Models;
using qcs_product.API.SettingModels;
using qcs_product.API.ViewModels;
using qcs_product.Constants;

namespace qcs_product.API.EventHandlers
{
    public class ReminderReviewEventHandler : IIntegrationEventHandler<ReminderReviewIntegrationEvent>
    {
        private readonly ILogger<ReminderReviewEventHandler> _logger;
        private readonly EnvironmentSetting _environmentSetting;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEventBus _eventBus;
        private readonly IMonitoringBusinessProvider _monitoringBusinessProvider;
        private readonly IAUAMServiceBusinessProviders _auamServiceBusinessProviders;
        private readonly IQcSamplingBusinessProvider _qcSamplingBusinessProvider;

        public ReminderReviewEventHandler(ILogger<ReminderReviewEventHandler> logger,
            IOptions<EnvironmentSetting> environmentSetting,
            IServiceScopeFactory scopeFactory, IEventBus eventBus, IMonitoringBusinessProvider monitoringBusinessProvider, IAUAMServiceBusinessProviders auamServiceBusinessProviders, IQcSamplingBusinessProvider qcSamplingBusinessProvider)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _eventBus = eventBus;
            _environmentSetting = environmentSetting.Value;
            _monitoringBusinessProvider = monitoringBusinessProvider;
            _auamServiceBusinessProviders = auamServiceBusinessProviders;
            _qcSamplingBusinessProvider = qcSamplingBusinessProvider;
        }

        public async Task Handle(ReminderReviewIntegrationEvent @event)
        {
            _logger.LogInformation("trigger to send reminder review from google pub/sub");

            await SendReminderReviewSampling();

            _logger.LogInformation("end process reminder review");
        }

        private async Task SendReminderReviewQcTest(string workflowStatusName)
        {
            var workflowServiceDataProvider = _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IWorkflowServiceDataProvider>();
            var notificationServiceBusinessProvider = _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<INotificationServiceBusinessProvider>();
            var QcRequestDataProvider =
                _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IQcRequestDataProvider>();
            var qcTestDataProvider =
                _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IQcTestDataProvider>();

            var personal = await QcRequestDataProvider.GetPersonalById(1);

            var personalDictionary = new Dictionary<string, Personal>();

            var qcTestList = await qcTestDataProvider.GetPendingReview(workflowStatusName);

            foreach (var qcTest in qcTestList)
            {
                var requestData = await qcTestDataProvider.GetByTransactionGroupSampling(qcTest.Id);
                if (requestData == null)
                {
                    _logger.LogWarning("get QC request by qc test id: {Id} is empty", qcTest.Id);
                    continue;
                }

                var requestData2 = await qcTestDataProvider.GetQcRequestByTransactionGroupId(qcTest.Id);

                var purposeNames = await QcRequestDataProvider.getRequestPurposeNames(requestData.Id);
                var purposeNamesConcat = "";
                if (purposeNames.Any())
                {
                    purposeNamesConcat = string.Join(", ", purposeNames.ToArray());
                }

                _logger.LogInformation("Get PIC qc test document code = {WorkflowDocumentCode}", qcTest.WorkflowDocumentCode);

                //TODO untuk improvement bisa dengan cara:
                //1. menggunakan Task.WhenAll
                //2. penambahan end point di workflow service untuk get pic berdasarkan list of document codes
                var picResponseModel = await workflowServiceDataProvider.GetPIC(qcTest.WorkflowDocumentCode);
                if (picResponseModel.PICs == null)
                {
                    continue;
                }

                foreach (var documentPicViewModel in picResponseModel.PICs)
                {
                    #region get receiver
                    var nik = documentPicViewModel.orgId;

                    Personal receiver = null;

                    if (!personalDictionary.ContainsKey(nik))
                    {
                        receiver = await GetEmployeeByNik(nik);
                        if (receiver != null)
                        {
                            personalDictionary.Add(nik, receiver);
                        }
                    }
                    else
                    {
                        receiver = personalDictionary[nik];
                    }

                    if (_environmentSetting.EnvironmentName == ApplicationConstant.DEVELOPMENT_ENVIRONMENT_NAME
                        || _environmentSetting.EnvironmentName == ApplicationConstant.TESTING_ENVIRONMENT_NAME)
                    {
                        receiver ??= new Personal();
                        receiver.NoHandphone = personal?.NoHandphone;
                        receiver.Email = personal?.Email;
                        if (string.IsNullOrEmpty(receiver.Nik))
                        {
                            receiver.Nik = nik;
                        }
                    }

                    #endregion

                    #region send notification

                    if (workflowStatusName == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA)
                    {
                        var testType = requestData2?.TestType ?? "";

                        var msgQcTest = new MessageNotificationMonitoringViewModel(requestData.NoRequest,
                            requestData.NoBatch, requestData.ItemName, purposeNamesConcat, receiver?.Email,
                            ApplicationConstant.APPROVED_ACTION_SELF_NOTIF_TO_QA,
                            receiver?.Name, receiver?.NoHandphone, null,
                            testType, qcTest.Code, "");

                        if (!string.IsNullOrEmpty(receiver?.Email))
                        {
                            _logger.LogInformation("send notification email for review data test to {Nik} ({EmailAddress})",
                                receiver.Nik,
                                msgQcTest.EmailAddress);
                            await notificationServiceBusinessProvider.SendEmailNotifMonitoring(msgQcTest);
                        }

                        if (!string.IsNullOrEmpty(receiver?.NoHandphone))
                        {
                            _logger.LogInformation("send notification WA for review data test to {Nik} ({NoHandphone})",
                                receiver.Nik,
                                msgQcTest.NoHandphone);
                            await notificationServiceBusinessProvider.SendEmailNotifMonitoring(msgQcTest);
                        }

                    }
                    else
                    {
                        var msgQcTest = new MessageNotificationQcTestViewModel(
                            qcTest.Code,
                            qcTest.QcProcessName,
                            receiver?.Email,
                            ApplicationConstant.NEW_ACTION_NOTIF,
                            receiver?.Name,
                            receiver?.NoHandphone,
                            null,
                            ApplicationConstant.MSG_TYPE_BEFORE_PIC,
                            requestData.NoBatch,
                            requestData.NoRequest,
                            purposeNamesConcat
                        );

                        if (!string.IsNullOrEmpty(receiver?.Email))
                        {
                            _logger.LogInformation("send notification email for review data test to {Nik} ({EmailAddress})",
                                receiver.Nik,
                                msgQcTest.EmailAddress);
                            await notificationServiceBusinessProvider.SendEmailNotifQcTest(msgQcTest);
                        }

                        if (!string.IsNullOrEmpty(receiver?.NoHandphone))
                        {
                            _logger.LogInformation("send notification WA for review data test to {Nik} ({NoHandphone})",
                                receiver.Nik,
                                msgQcTest.NoHandphone);
                            await notificationServiceBusinessProvider.SendWhatsAppNotifQcTest(msgQcTest);
                        }
                    }

                    #endregion
                }
            }
        }

        public async Task SendReminderReviewSampling()
        {
            var QcRequestDataProvider =
                _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IQcRequestDataProvider>();

            List<string> roleCodes = new List<string>() { 
                ApplicationConstant.ROLE_CODE_KASIE_PEMFAS,
                ApplicationConstant.ROLE_CODE_KABAG_PEMFAS,
                ApplicationConstant.ROLE_CODE_REVIEWER_EM,
            };

            string appCodeQ100 = ApplicationConstant.APPLICATION_CODE_Q100;
            List<NikMonitoringListViewModel> lsUserPendingTrs = new List<NikMonitoringListViewModel>();

            foreach (var roleCode in roleCodes)
            {
                var personels = await _auamServiceBusinessProviders.ListPersonalByRole(appCodeQ100, roleCode);

                foreach (var personel in personels)
                {
                    var userPendingTrs = new NikMonitoringListViewModel()
                    {
                        Nik = personel.NewNik,
                        RoleCode = roleCode
                    };

                    lsUserPendingTrs.Add(userPendingTrs);
                }
            }

            var niks = lsUserPendingTrs.Select(x => x.Nik).Distinct().ToList();
            var lsEmployeeDetail = await GetEmployeeByListNik(niks);
            var personal = await QcRequestDataProvider.GetPersonalById(1); //get user tester untuk dikirim notif pada development mode

            //add info organizaiton Id
            foreach (var userPendingTrs in lsUserPendingTrs)
            {
                int orgId = lsEmployeeDetail.FirstOrDefault(x => x.Nik == userPendingTrs.Nik).OrgId;
                userPendingTrs.OrgId = orgId;
            }

            foreach (var userPendingTrs in lsUserPendingTrs)
            {
                switch (userPendingTrs.RoleCode)
                {
                    case ApplicationConstant.ROLE_CODE_KASIE_PEMFAS:
                        var resultSamplingQcs = await _qcSamplingBusinessProvider.List(null, 10_000, 1, DateTime.Now.AddYears(-2), DateTime.UtcNow, "1,6", userPendingTrs.OrgId, 0, 0);
                        userPendingTrs.RequestSamplingVm = resultSamplingQcs.Data;

                        break;
                    default:
                        var resultLsShort = await _monitoringBusinessProvider.ListShort(null, 10_000, 1, DateTime.Now.AddYears(-2), DateTime.UtcNow, null, null, 0, userPendingTrs.Nik, null);
                        userPendingTrs.MonitoringListVm = resultLsShort.Data;

                        break;
                }
            }

            //remove transaksi pending yg kosong sesuai roleCode
            lsUserPendingTrs.RemoveAll(x => x.RequestSamplingVm == null && x.RoleCode == ApplicationConstant.ROLE_CODE_KASIE_PEMFAS);
            lsUserPendingTrs.RemoveAll(x => x.MonitoringListVm == null && x.RoleCode != ApplicationConstant.ROLE_CODE_KASIE_PEMFAS);

            foreach (var userPendingTrs in lsUserPendingTrs)
            {
                #region get receiver

                Personal receiver = lsEmployeeDetail.FirstOrDefault(x => x.Nik == userPendingTrs.Nik);

                if (_environmentSetting.EnvironmentName == ApplicationConstant.DEVELOPMENT_ENVIRONMENT_NAME
                    || _environmentSetting.EnvironmentName == ApplicationConstant.TESTING_ENVIRONMENT_NAME)
                {
                    receiver ??= new Personal();
                    receiver.NoHandphone = personal?.NoHandphone;
                    receiver.Email = personal?.Email;
                    if (string.IsNullOrEmpty(receiver.Nik))
                    {
                        receiver.Nik = userPendingTrs.Nik;
                    }
                }

                #endregion

                var lsPicAndSampling = new List<PicSamplingViewModel>();

                switch (userPendingTrs.RoleCode)
                {
                    case ApplicationConstant.ROLE_CODE_KASIE_PEMFAS:
                        foreach (var itemRequestSampling in userPendingTrs.RequestSamplingVm)
                        {
                            var picAndSampling = new PicSamplingViewModel()
                            {
                                Nik = userPendingTrs.Nik,
                                SamplingTypeName = itemRequestSampling.SamplingTypeName,
                                NoRequest = itemRequestSampling.NoRequest,
                            };

                            lsPicAndSampling.Add(picAndSampling);
                        }

                        break;
                    default:
                        foreach (var itemMonitoring in userPendingTrs.MonitoringListVm)
                        {
                            foreach (var sampling in itemMonitoring.Sampling)
                            {
                                var picAndSampling = new PicSamplingViewModel()
                                {
                                    Nik = userPendingTrs.Nik,
                                    NoRequest = sampling.NoRequest,
                                };

                                lsPicAndSampling.Add(picAndSampling);
                            }
                        }
                        break;
                }
                
                //get unique PicAndSampling
                lsPicAndSampling = lsPicAndSampling.GroupBy(x => new { x.Nik, x.SamplingTypeName, x.NoRequest })
                    .Select(x => x.First())
                    .ToList();

                var groupBySamplingTypeName = lsPicAndSampling.GroupBy(x => x.SamplingTypeName).ToList();

                foreach (var group in groupBySamplingTypeName)
                {
                    #region send notification by nik, sampling type name, list of request numbers

                    string samplingTypeName = group.Key;
                    var requestNumbers = group.Select(x => x.NoRequest).ToList();

                    if (!string.IsNullOrEmpty(receiver?.Email))
                    {
                        _logger.LogInformation("send notification email for review data sampling to {Nik} ({EmailAddress})",
                            receiver.Nik,
                            receiver.Email);

                        var recipient = receiver.Email;
                        var subject = "[Q100+] Reminder Approval";
                        var message = BuildMessageEmail(samplingTypeName, requestNumbers);
                        var type = ApplicationConstant.NOTIFICATION_TYPE_EMAIL;
                        await SendNotification(recipient, subject, message, type);
                    }

                    if (!string.IsNullOrEmpty(receiver?.NoHandphone))
                    {
                        _logger.LogInformation("send notification WA for review data sampling to {Nik} ({NoHandphone})",
                            receiver.Nik,
                            receiver.NoHandphone);

                        var type = ApplicationConstant.NOTIFICATION_TYPE_WHATSAPP;
                        var recipient = receiver.NoHandphone;
                        var subject = "[Q100+] Reminder Approval";
                        var message = userPendingTrs.RoleCode == ApplicationConstant.ROLE_CODE_KASIE_PEMFAS
                            ? BuildMessageWhatsappRevKasie(receiver, samplingTypeName, requestNumbers)
                            : BuildMessageWhatsapp(receiver, requestNumbers);

                        await SendNotification(recipient, subject, message, type);
                    }

                    #endregion
                }
            }
        }

        private static string BuildMessageWhatsappRevKasie(Personal receiver, string samplingTypeName, List<string> requestNumbers)
        {
            var message = $"Reminder: Hallo {receiver.Name}, terdapat Data Sampling {samplingTypeName} baru dengan No Permohonan: ";

            string seperator = ", ";
            message += String.Join(seperator, requestNumbers);
            message += ". Silahkan cek aplikasi Q100+ untuk melakukan _review_ dan _approval_ data sampling.";

            return message;
        }

        private static string BuildMessageWhatsapp(Personal receiver, List<string> requestNumbers)
        {
            var message = $"Reminder: Hallo {receiver.Name}, terdapat Data Sampling baru dengan No Permohonan: ";

            string seperator = ", ";
            message += String.Join(seperator, requestNumbers);
            message += ". Silahkan cek aplikasi Q100+ untuk melakukan _review_ dan _approval_ data.";

            return message;
        }

        private static string BuildMessageEmail(string samplingTypeName, List<string> requestNumbers)
        {
            var message = $"Terdapat Data Sampling {samplingTypeName} baru dengan No Permohonan: ";
            if (requestNumbers.Count > 1)
            {
                foreach (var requestNumber in requestNumbers)
                {
                    message += $"<br /><b>{requestNumber}</b>";
                }

                message +=
                    "<p>Silahkan cek aplikasi Q100+ untuk melakukan <i>review</i> dan <i>approval</i> data sampling.</p>";
            }
            else
            {
                message +=
                    $"<b>{requestNumbers[0]}</b>. Silahkan cek aplikasi Q100+ untuk melakukan <i>review</i> dan <i>approval</i> data sampling.";
            }

            return message;
        }

        private async Task<List<Personal>> GetEmployeeByListNik(List<string> lsNik)
        {
            var bioHrIntegrationBusinessProviders = _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IBioHRIntegrationBussinesProviders>();
            var auamServiceBusinessProviders = _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IAUAMServiceBusinessProviders>();

            List<Personal> lsPersonal = new List<Personal>();
            var lsPersonalBioHR = await bioHrIntegrationBusinessProviders.GetListEmployeeByListNewNik(lsNik);

            foreach (var personalBioHR in lsPersonalBioHR)
            {
                var personal = new Personal();

                personal.Name = personalBioHR.Nama;
                personal.Email = personalBioHR.Email;
                personal.NoHandphone = personalBioHR.Telepon;
                personal.Nik = personalBioHR.NewNik;
                personal.OrgId = Convert.ToInt32(personalBioHR.SectionId);

                if (personal.NoHandphone == null)
                {
                    var personalUAM = await auamServiceBusinessProviders.GetPersonalExtDetailByNik(personal.Nik);
                    personal.NoHandphone = personalUAM?.NoTelp;
                }

                lsPersonal.Add(personal);
            }

            return lsPersonal;
        }

        private async Task<Personal> GetEmployeeByNik(string nik)
        {
            var bioHrIntegrationBusinessProviders = _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IBioHRIntegrationBussinesProviders>();
            var auamServiceBusinessProviders = _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<IAUAMServiceBusinessProviders>();

            var personal = new Personal();

            var personalBioHR = await bioHrIntegrationBusinessProviders.GetEmployeeByNewNik(nik);
            personal.Name = personalBioHR?.Name;
            personal.Email = personalBioHR?.Email;
            

            if (personalBioHR != null)
            {
                var personalDetailBioHR = await bioHrIntegrationBusinessProviders.GetEmployeeByNik(nik);
                personal.NoHandphone = personalDetailBioHR?.Telepon;
                personal.Nik = personalDetailBioHR?.NIK;
            }
            else
            {
                var personalUAM = await auamServiceBusinessProviders.GetPersonalExtDetailByNik(nik);
                personal.Name = personalUAM?.Name;
                personal.Email = personalUAM?.Email;
                personal.NoHandphone = personalUAM?.NoTelp;
                personal.Nik = personalUAM?.Nik;
            }

            return personal;
        }

        private async Task SendNotification(string recipient, string subject, string message, long type)
        {
            var eventMessage = new NotificationIntegrationEvent()
            {
                RecipientType = ApplicationConstant.NOTIFICATION_RECEIVED_TYPE_PERSONAL,
                Recipient = recipient,
                Subject = subject,
                Message = message,
                PriorityId = 1,
                NotificationType = type,
                RequestAppId = ApplicationConstant.APPLICATION_ID,
                ObjectMethod = "-",
                ObjectId = "-"
            };
            await _eventBus.PublishAsync(eventMessage);
        }

    }

}