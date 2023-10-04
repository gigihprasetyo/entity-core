using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcLabelSamplingViewModel
    {
        public Int32 QcSamplingId { get; set; }
        public string SamplingTypeName { get; set; }
        public DateTime? SamplingDateFrom { get; set; }
        public DateTime? SamplingDateTo { get; set; }
        public Int32 SamplingStatus { get; set; }
    }
}
