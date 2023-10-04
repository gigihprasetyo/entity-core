using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionTestVariable
    {
        public int Id { get; set; }
        public int TestParameterId { get; set; }
        public int TresholdOperator { get; set; }
        public long? TresholdValue { get; set; }
        public long? ThresholdValueTo { get; set; }
        public long? ThresholdValueFrom { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Sequence { get; set; }
        public string VariableName { get; set; }

        public virtual TransactionRelTestScenarioParam TestParameter { get; set; }
    }
}
