using System;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class PositionToRoleIntegrationEvent : IntegrationEvent
    {
        public int DataId { get; set; }
        public string ApplicationCode { get; set; }
        public string Operation { get; set; }
        public string RoleCode { get; set; }
        public string PosId { get; set; }
        public string Name { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}