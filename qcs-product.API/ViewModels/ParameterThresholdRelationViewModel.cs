using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class ParameterThresholdRelationViewModel
    {
        public Int32 RoomId { get; set; }
        public Int32? ToolId { get; set; }
        public string ToolName { get; set; }
        public Int32? GradeRoomId { get; set; }
        public string GradeRoomName { get; set; }
        public Int32 SamplePointId { get; set; }
        public string SamplePointName { get; set; }
        public List<TestParameterVariableRelationViewModel> TestParameter { get; set; }

    }
}
