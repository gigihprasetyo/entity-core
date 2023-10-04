using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class WorkflowQcSampling
    {
        public int Id { get; set; }
        public int QcSamplingId { get; set; }
        public bool IsInWorkflow { get; set; }
        public string WorkflowStatus { get; set; }
        public string WorkflowDocumentCode { get; set; }
        public string WorkflowCode { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
