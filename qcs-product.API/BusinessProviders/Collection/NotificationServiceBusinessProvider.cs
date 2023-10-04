using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.IntegrationEvents;
using qcs_product.API.SettingModels;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class NotificationServiceBusinessProvider : INotificationServiceBusinessProvider
    {
        private readonly NotificationServiceSetting _notification;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<NotificationServiceBusinessProvider> _logger;
        private readonly IEventBus _eventBus;

        public NotificationServiceBusinessProvider(IOptions<NotificationServiceSetting> notificationSetting, IHttpClientFactory clientFactory, ILogger<NotificationServiceBusinessProvider> logger,
            IEventBus eventBus)
        {
            _notification = notificationSetting.Value;
            _clientFactory = clientFactory;
            _logger = logger;
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task SendEmailNotif(MessageNotificationRequestQcsViewModel message)
        {
            var eventMessage = new NotificationIntegrationEvent()
            {
                RecipientType = ApplicationConstant.NOTIFICATION_RECEIVED_TYPE_PERSONAL,
                Recipient = message.EmailAddress,
                Subject = message.Subject,
                Message =message.MessageEmail,
                PriorityId = 1,
                NotificationType = ApplicationConstant.NOTIFICATION_TYPE_EMAIL,
                RequestAppId = ApplicationConstant.APPLICATION_ID,
                ObjectMethod = "-",
                ObjectId = "-"
            };
            await _eventBus.PublishAsync(eventMessage);

        }

        public async Task SendWhatsAppNotif(MessageNotificationRequestQcsViewModel message)
        {
            var eventMessage = new NotificationIntegrationEvent()
            {
                RecipientType = ApplicationConstant.NOTIFICATION_RECEIVED_TYPE_PERSONAL,
                Recipient = message.NoHandphone,
                Subject = message.Subject,
                Message = message.MessageWhatsApp,
                PriorityId = 1,
                NotificationType = ApplicationConstant.NOTIFICATION_TYPE_WHATSAPP,
                RequestAppId = ApplicationConstant.APPLICATION_ID,
                ObjectMethod = "-",
                ObjectId = "-"
            };
            await _eventBus.PublishAsync(eventMessage);

        }

        public async Task SendEmailNotifSampling(MessageNotificationSamplingAltViewModel message)
        {
            var eventMessage = new NotificationIntegrationEvent()
            {
                RecipientType = ApplicationConstant.NOTIFICATION_RECEIVED_TYPE_PERSONAL,
                Recipient = message.EmailAddress,
                Subject = message.Subject,
                Message = message.MessageEmail,
                PriorityId = 1,
                NotificationType = ApplicationConstant.NOTIFICATION_TYPE_EMAIL,
                RequestAppId = ApplicationConstant.APPLICATION_ID,
                ObjectMethod = "-",
                ObjectId = "-"
            };
            await _eventBus.PublishAsync(eventMessage);

        }

        public async Task SendWhatsAppNotifSampling(MessageNotificationSamplingAltViewModel message)
        {
            var eventMessage = new NotificationIntegrationEvent()
            {
                RecipientType = ApplicationConstant.NOTIFICATION_RECEIVED_TYPE_PERSONAL,
                Recipient = message.NoHandphone,
                Subject = message.Subject,
                Message = message.MessageWhatsApp,
                PriorityId = 1,
                NotificationType = ApplicationConstant.NOTIFICATION_TYPE_WHATSAPP,
                RequestAppId = ApplicationConstant.APPLICATION_ID,
                ObjectMethod = "-",
                ObjectId = "-"
            };
            await _eventBus.PublishAsync(eventMessage);

        }

        public async Task SendEmailNotifMonitoring(MessageNotificationMonitoringViewModel message)
        {
            var eventMessage = new NotificationIntegrationEvent()
            {
                RecipientType = ApplicationConstant.NOTIFICATION_RECEIVED_TYPE_PERSONAL,
                Recipient = message.EmailAddress,
                Subject = message.Subject,
                Message = message.MessageEmail,
                PriorityId = 1,
                NotificationType = ApplicationConstant.NOTIFICATION_TYPE_EMAIL,
                RequestAppId = ApplicationConstant.APPLICATION_ID,
                ObjectMethod = "-",
                ObjectId = "-"
            };
            await _eventBus.PublishAsync(eventMessage);

        }

        public async Task SendWhatsAppNotifMonitoring(MessageNotificationMonitoringViewModel message)
        {
            var eventMessage = new NotificationIntegrationEvent()
            {
                RecipientType = ApplicationConstant.NOTIFICATION_RECEIVED_TYPE_PERSONAL,
                Recipient = message.NoHandphone,
                Subject = message.Subject,
                Message = message.MessageWhatsApp,
                PriorityId = 1,
                NotificationType = ApplicationConstant.NOTIFICATION_TYPE_WHATSAPP,
                RequestAppId = ApplicationConstant.APPLICATION_ID,
                ObjectMethod = "-",
                ObjectId = "-"
            };
            await _eventBus.PublishAsync(eventMessage);

        }

        public async Task SendEmailNotifQcTest(MessageNotificationQcTestViewModel message)
        {
            var eventMessage = new NotificationIntegrationEvent()
            {
                RecipientType = ApplicationConstant.NOTIFICATION_RECEIVED_TYPE_PERSONAL,
                Recipient = message.EmailAddress,
                Subject = message.Subject,
                Message = message.MessageEmail,
                PriorityId = 1,
                NotificationType = ApplicationConstant.NOTIFICATION_TYPE_EMAIL,
                RequestAppId = ApplicationConstant.APPLICATION_ID,
                ObjectMethod = "-",
                ObjectId = "-"
            };
            await _eventBus.PublishAsync(eventMessage);
        }

        public async Task SendWhatsAppNotifQcTest(MessageNotificationQcTestViewModel message)
        {
            var eventMessage = new NotificationIntegrationEvent()
            {
                RecipientType = ApplicationConstant.NOTIFICATION_RECEIVED_TYPE_PERSONAL,
                Recipient = message.NoHandphone,
                Subject = message.Subject,
                Message = message.MessageWhatsApp,
                PriorityId = 1,
                NotificationType = ApplicationConstant.NOTIFICATION_TYPE_WHATSAPP,
                RequestAppId = ApplicationConstant.APPLICATION_ID,
                ObjectMethod = "-",
                ObjectId = "-"
            };
            await _eventBus.PublishAsync(eventMessage);
        }
    }
}
