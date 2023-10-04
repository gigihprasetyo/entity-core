using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class EmTestTypeViewModel
    {
        public Int32? RoomId { get; set; }
        public Int32 TestParameterId { get; set; }
        public string TestParameterName { get; set; }
        public int TestParameterSquence { get; set; }
        public int TestScenarioId { get; set; }
        public int? CountParamater { get; set; }
        public int? ThresholdRoomSamplingPoint { get; set; }
        public int? ThresholdToolSamplingPoint { get; set; }
    }
}
