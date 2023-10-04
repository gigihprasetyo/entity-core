using System;
using System.Collections.Generic;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class TestVariableIntegrationEvent : IntegrationEvent
    {
        public int DataId { get; set; }

        public string Operation { get; set; }

        public int TestScnrTestParamId { get; set; }

        public string TestScenarioName { get; set; }

        public string TestParameterName { get; set; }

        public string VariableName { get; set; }

        public int ThresholdOperator { get; set; }

        public decimal? ThresholdValue { get; set; }

        public decimal? ThresholdValueTo { get; set; }

        public decimal? ThresholdValueFrom { get; set; }

        public int? Sequence { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public string RowStatus { get; set; }
    }
}