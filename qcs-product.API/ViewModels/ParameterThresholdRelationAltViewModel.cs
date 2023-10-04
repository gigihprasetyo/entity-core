using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class ParameterThresholdRelationAltViewModel
    {
        public Int32? GradeRoomId { get; set; }
        public string GradeRoomCode { get; set; }
        public string GradeRoomName { get; set; }
        public List<TestScenarioThresholdViewModel> TestScenario { get; set; }
    }

    public partial class TestScenarioThresholdViewModel {
        public Int32 TestScenarioId { get; set; }
        public string TestScenarioName { get; set; }
        public string TestScenarioLabel { get; set; }
        public int GradeRoomId { get; set; }
        public List<TestParameterThresholdViewModel> TestParameterThreshold { get; set; }
    }

    public partial class TestParameterThresholdViewModel
    {
        public Int32 TestParameterId { get; set; }
        public string TestParameterName { get; set; }
        public string TestParameterShort { get; set; }
        public Int32 TestGroupId { get; set; }
        public int Sequence { get; set; }
        public List<TestVariableThresholdViewModel> TestVariableThreshold { get; set; }
    }

    public partial class TestVariableThresholdViewModel
    {
        public Int32 TestVariableId { get; set; }
        public int TestParameterId { get; set; }
        public int Sequence { get; set; }
        public string VariableName { get; set; }
        public Int32 TresholdOperator { get; set; }
        public string TresholdOperatorName { get; set; }
        public long? TresholdValue { get; set; }
        public long? TresholdMin { get; set; }
        public long? TresholdMax { get; set; }
    }
}
