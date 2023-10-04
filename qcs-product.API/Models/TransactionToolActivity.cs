using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionToolActivity
    {
        public int Id { get; set; }
        public int ToolId { get; set; }
        public int ActivityId { get; set; }
        public string ActivityCode { get; set; }
        public DateTime ActivityDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual TransactionActivity Activity { get; set; }
        public virtual TransactionTool Tool { get; set; }
    }
}
