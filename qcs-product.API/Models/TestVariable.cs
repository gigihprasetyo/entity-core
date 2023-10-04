using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class TestVariable
    {
        public Int32 Id { get; set; }
        public Int32 TestParameterId { get; set; }
        public string VariableName { get; set; }
        public Int32 TresholdOperator { get; set; }
        public long? TresholdValue { get; set; }
        public long? TresholdMin { get; set; }
        public long? TresholdMax { get; set; }
        public Int32 Sequence { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
