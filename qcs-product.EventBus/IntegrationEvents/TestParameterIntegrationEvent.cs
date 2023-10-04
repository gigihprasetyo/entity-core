using qcs_product.EventBus.EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class TestParameterIntegrationEvent : IntegrationEvent
    {
        public int DataId { get; set; }
        public string TestParameterName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
        public List<TestVariableIntegrationEvent> TestVariables { get; set; }
    }
}
