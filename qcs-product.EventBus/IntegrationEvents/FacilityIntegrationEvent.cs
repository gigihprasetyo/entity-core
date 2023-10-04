using System;
using qcs_product.EventBus.EventBus.Base.Events;
using System.Collections.Generic;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class FacilityIntegrationEvent : IntegrationEvent
    {

        public int DataId { get; set; }
        public string Operation { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationName { get; set; }
        public int BIOHROrganizationId { get; set; }
        public int ObjectStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
        public List<FacilityRoomIntegrationEvent> FacilityRoom { get; set; }
        public List<RoomIntegrationEvent> Rooms { get; set; }

    }
}