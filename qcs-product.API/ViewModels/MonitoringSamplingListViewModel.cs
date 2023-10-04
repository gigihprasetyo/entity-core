using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class MonitoringSamplingListViewModel
    {
        public Int32 SamplingId { get; set; }
        public string Code { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string NoRequest { get; set; }
        public int? TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public int? SamplingTypeId { get; set; }
        public string SamplingTypeName { get; set; }
        public int Process { get; set; }
        public int? Status { get; set; }
        public int? StatusRequest { get; set; }
        public int? StatusSampling { get; set; }
        public int? StatusTransfer { get; set; }
        public int? StatusTesting { get; set; }
    }
}
