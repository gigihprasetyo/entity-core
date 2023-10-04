using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class MonitoringRelationViewModel
    {
        public Int32 Id { get; set; }
        public string NoRequest { get; set; }
        public int? TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public string NoBatch { get; set; }
        //public bool IsAllowedProcessWorkflow { get; set; }
        public bool IsAllowedProcessApprove { get; set; }
        public bool IsAllowedDeviationButton { get; set; }
        public bool IsAllowedDeviationColoumn { get; set; }
        public List<MonitoringSamplingSidebarViewModel> Sampling { get; set; }
        public List<MonitoringSamplingSidebarViewModel> Transfer { get; set; }
        public List<MonitoringSamplingSidebarViewModel> Testing { get; set; }
        public List<RequestPurposesViewModel> RequestPurposes { get; set; }
    }
}
