using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcLabelSampleViewModel
    {
        public Int32 SampleId { get; set; }
        public string Code { get; set; }
        public Int32? SamplingPointId { get; set; }
        public string SamplingPointCode { get; set; }
        public string GradeRoomName { get; set; }
        public string ToolCode { get; set; }
        public string ToolName { get; set; }
        public string ToolGroupName { get; set; }
        public string ToolGroupLabel { get; set; }
        public Int32 TestParamId { get; set; }
        public string TestParamName { get; set; }
        public int? TestParamIndex { get; set; }
        public string PersonalInitial { get; set; }
        public string PersonalName { get; set; }
        public DateTime? SampleDateTimeFrom { get; set; }
        public DateTime? SampleDateTimeTo { get; set; }
        public decimal? ParticleVolume { get; set; }
        public string AttchmentFile { get; set; }
        public string Note { get; set; }
    }
}
