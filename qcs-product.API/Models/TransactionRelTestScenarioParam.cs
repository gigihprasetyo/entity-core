using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionRelTestScenarioParam
    {
        public TransactionRelTestScenarioParam()
        {
            TransactionRelSamplingTestParam = new HashSet<TransactionRelSamplingTestParam>();
            TransactionTestVariable = new HashSet<TransactionTestVariable>();
        }

        public int Id { get; set; }
        public int TestParameterId { get; set; }
        public int TestScenarioId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual TransactionTestParameter TestParameter { get; set; }
        public virtual TransactionTestScenario TestScenario { get; set; }
        public virtual ICollection<TransactionRelSamplingTestParam> TransactionRelSamplingTestParam { get; set; }
        public virtual ICollection<TransactionTestVariable> TransactionTestVariable { get; set; }
    }
}
