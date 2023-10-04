using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionRelSamplingPurposeToolGroup
    {
        public int Id { get; set; }
        public int PurposeId { get; set; }
        public int ToolGroupId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public virtual TransactionPurposes Purpose { get; set; }
        public virtual TransactionToolGroup ToolGroup { get; set; }
    }
}
