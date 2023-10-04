using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;

namespace qcs_product.API.Models
{
    public class VSamplePointTestParam
    {
        public Int32 RoomId { get; set; }
        public Int32? ToolId { get; set; }
        public string ToolName { get; set; }
        public Int32? ToolGroupId { get; set; }
        public Int32 GradeRoomId { get; set; }
        public string GradeRoomName { get; set; }
        public Int32 SamplePointId { get; set; }
        public string SamplePointName { get; set; }
        public string TestScenarioName { get; set; }
        public Int32 TestScenarioId { get; set; }
        public string TestScenarioLabel { get; set; }
        public Int32 TestParameterId { get; set; }
        public string TestParameterName { get; set; }
        public Int32 TestGroupId { get; set; }
        public int Seq { get; set; }
        public int toolPurposeId { get; set; }

        public List<RequestPurposesViewModel> Purposes { get; set; }
    }
}
