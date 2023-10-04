using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class WorkflowQcTransactionGroup
    {
        public int Id { get; set; }
        public int QcTransactionGroupId { get; set; }
        public string WorkflowStatus { get; set; }
        public string WorkflowDocumentCode { get; set; }
        public string WorkflowCode { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsInWorkflow { get; set; }

        public virtual QcTransactionGroup QcTransactionGroup { get; set; }
    }
}
