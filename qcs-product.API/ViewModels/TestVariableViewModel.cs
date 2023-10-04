using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class TestVariableViewModel
    {
        public int Id { get; set; }
        public string VariableName { get; set; }
        public int TresholdOperator { get; set; }
        public string TresholdOperatorName { get; set; }
        public long? TresholdValue { get; set; }
        public long? TresholdMin { get; set; }
        public long? TresholdMax { get; set; }
        public int Sequence { get; set; }
        public int? TestScenarioId { get; set; }
        
        public int? TestParameterId { get; set; }
    }
}
