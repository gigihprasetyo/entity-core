using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionRelSamplingTool
    {
        public int Id { get; set; }
        public int SamplingPoinId { get; set; }
        public int? ToolId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ToolPurposeId { get; set; }
        public string ScenarioLabel { get; set; }

        public virtual TransactionSamplingPoint SamplingPoin { get; set; }
    }
}
