using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class MonitoringSamplingSidebarViewModel
    {
        public Int32 Id { get; set; }
        public string SamplingTypeName { get; set; }
        public string Name { get; set; }
        public bool IsAllowedProcessApprove { get; set; }
        public bool IsAllowedProcessReject { get; set; }
        public string Notes { get; set; }
        public QcTransactionGroupProcessRelViewModel QcTransactionGroupProcess { get; set; }
        public List<QcSampleViewModel> Samples { get; set; }
    }
}
