using System;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class OrganizationIntegrationEvent : IntegrationEvent
    {
        public int DataId { get; set; }

        public string Operation { get; set; }

        public string OrgCode { get; set; }

        public string Name { set; get; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public string RowStatus { get; set; }

        public int BIOHROrganizationId { get; set; }
    }
}