using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.ViewModels
{
    public class RoomSamplingPointViewModel
    {
        public Int32 SamplingPointId { get; set; }
        public Int32? RoomId { get; set; }
        public Int32? ToolId { get; set; }
        public string Code { get; set; }
        public string ScenarioLabel { get; set; }
        public int? RoomPurposeId { get; set; }
        public List<TestParameterRoom> TestParameter { get; set; }
    }

    public class TestParameterRoom
    {
        public int Id { get; set; }
        public string TestParameterName { get; set; }
        public int SamplingPointId { get; set; }
        public int RelTestScenarioParamId { get; set; }
        public string ScenarioLabel { get; set; }
        public List<TestVariable> TestVariables { get; set; }
        public TestVariable TestVariable { get; set; }
    }
}
