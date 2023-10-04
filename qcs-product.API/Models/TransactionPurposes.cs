using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionPurposes
    {
        public TransactionPurposes()
        {
            TransactionRelSamplingPurposeToolGroup = new HashSet<TransactionRelSamplingPurposeToolGroup>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<TransactionRelSamplingPurposeToolGroup> TransactionRelSamplingPurposeToolGroup { get; set; }
    }
}
