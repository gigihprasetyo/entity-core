using System;
using System.Collections.Generic;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class TestScenarioIntegrationEvent : IntegrationEvent
    {
        public int DataId { get; set; }

        public string Operation { get; set; }

        public string TestScenarioName { get; set; }

        public string TestScenarioLabel { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public string RowStatus { get; set; }
        public List<TestParameterIntegrationEvent> TestParameters { get; set; }
    }
}