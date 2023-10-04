using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionRelSamplingTestParam
    {
        public int Id { get; set; }
        public int SamplingPointId { get; set; }
        public int TestScenarioParamId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual TransactionSamplingPoint SamplingPoint { get; set; }
        public virtual TransactionRelTestScenarioParam TestScenarioParam { get; set; }
    }
}
