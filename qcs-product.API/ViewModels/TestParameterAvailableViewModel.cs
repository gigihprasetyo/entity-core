using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class TestParameterAvailableViewModel
    {
        public Int32 Id { get; set; }
        public Int32 TestGroupId { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public int? CountParamater { get; set; }
        public int? ThresholdRoomSamplingPoint { get; set; }
        public int? ThresholdToolSamplingPoint { get; set; }
    }
}
