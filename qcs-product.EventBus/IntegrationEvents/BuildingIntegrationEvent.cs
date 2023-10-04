using System;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class BuildingIntegrationEvent : IntegrationEvent
    {
        public int DataId { get; set; }
        public string Operation { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ObjectStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
    }
}