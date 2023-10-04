using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class SamplingPointViewModel
    {
        public Int32 SamplingPointId { get; set; }
        public Int32? RoomId { get; set; }
        public Int32? ToolId { get; set; }
        public string Code { get; set; }
        public string ScenarioLabel { get; set; }
    }
}
