using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface INotificationServiceBusinessProvider
    {
        public Task SendEmailNotif(MessageNotificationRequestQcsViewModel message);
        public Task SendWhatsAppNotif(MessageNotificationRequestQcsViewModel message);
        public Task SendEmailNotifSampling(MessageNotificationSamplingAltViewModel message);
        public Task SendWhatsAppNotifSampling(MessageNotificationSamplingAltViewModel message);
        public Task SendEmailNotifMonitoring(MessageNotificationMonitoringViewModel message);
        public Task SendWhatsAppNotifMonitoring(MessageNotificationMonitoringViewModel message);
        public Task SendEmailNotifQcTest(MessageNotificationQcTestViewModel message);
        public Task SendWhatsAppNotifQcTest(MessageNotificationQcTestViewModel message);
    }
}
