using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class TestParamRoom
    {
        public Int32 RoomId { get; set; }
        public Int32 TestScenarioId { get; set; }
        public string TestScenarioName { get; set; }
        public string TestScenarioLabel { get; set; }
        public string SampleCode { get; set; }
        public Int32 TestParameterId { get; set; }
        public string TestParameterName { get; set; }
        public int TestParameterSquence { get; set; }
        public int TotalTestParameter { get; set; }
    }
}
