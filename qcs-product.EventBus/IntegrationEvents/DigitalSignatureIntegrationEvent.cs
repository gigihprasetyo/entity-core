using System;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class DigitalSignatureIntegrationEvent : IntegrationEvent
    {
        public int DataId { get; set; }
        public string Operation { get; set; }
        public string SerialNumber { get; set; }
        public string Nik { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}