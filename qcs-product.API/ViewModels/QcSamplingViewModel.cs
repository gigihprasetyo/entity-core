using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcSamplingViewModel
    {
        public Int32 Id { get; set; }
        public Int32 RequestQcsId { get; set; }
        public string Code { get; set; }
        public DateTime? SamplingDateFrom { get; set; }
        public DateTime? SamplingDateTo { get; set; }
        public Int32 SamplingTypeId { get; set; }
        public string SamplingTypeName { get; set; }
        public Int32 Status { get; set; }
        public string WorkflowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
