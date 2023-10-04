using System;
using System.Collections.Generic;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class GradeRoomIntegrationEvent : IntegrationEvent
    {

        public int DataId { get; set; }

        public string Operation { get; set; }

        public string Code { get; set; }

        public string Name { set; get; }
        public string GradeRoomDefault { set; get; }
        public int ObjectStatus { get; set; }

        public int TestGroupId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public string RowStatus { get; set; }

        public List<TestScenarioIntegrationEvent> TestScenarios { get; set; }
    }
}