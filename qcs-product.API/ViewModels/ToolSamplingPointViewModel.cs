using System;
using System.Collections.Generic;
using qcs_product.API.Models;

namespace qcs_product.API.ViewModels
{
    public class ToolSamplingPointViewModel
    {
        public Int32 SamplingPointId { get; set; }
        public Int32? RoomId { get; set; }
        public Int32? ToolId { get; set; }
        public string Code { get; set; }
        public string ScenarioLabel { get; set; }
        public int ToolPurposeId { get; set; }
        public List<TestParameterTool> TestParameter { get; set; }
    }

    public class TestParameterTool
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
