using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class MonitoringResultViewModel
    {
        public Int32 Id { get; set; }
        public string NoRequest { get; set; }
        public DateTime Date { get; set; }
        public DateTime? SamplingDateFrom { get; set; }
        public DateTime? SamplingDateTo { get; set; }
        public int? EmPhaseId { get; set; }
        public string EmPhaseName { get; set; }
        public int? EmRoomId { get; set; }
        public string EmRoomName { get; set; }
        public string NoDeviation { get; set; }
        public string Conclusion { get; set; }
        public int? EmRoomGradeId { get; set; }
        public string EmRoomGradeName { get; set; }

        public List<MonitoringSamplingSidebarViewModel> Sampling { get; set; }
    }
}
