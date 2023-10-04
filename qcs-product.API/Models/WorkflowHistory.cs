using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class WorkflowHistory
    {
        public Int32 Id { get; set; }
        public string WorkflowDocumentCode { get; set; }
        public string Action { get; set; }
        public string Note { get; set; }
        public string WorkflowStatus { get; set; }
        public string PicNik { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
