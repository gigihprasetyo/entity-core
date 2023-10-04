using System;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class FacilityRoomIntegrationEvent : IntegrationEvent
    {
        public int DataId { get; set; }
        public string Operation { get; set; }
        public int FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
    }
}