using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class TestParameterVariableRelationViewModel
    {
        public Int32 Id { get; set; }
        public Int32 TestGroupId { get; set; }
        public Int32 TestScenarioId { get; set; }
        public string TestScenarioName { get; set; }
        public string TestScenarioLabel { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public List<TestVariableViewModel> TestVariableThreshold { get; set; }
    }
}
