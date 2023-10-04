using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class SampleAvailableViewModel
    {
        public int RoomId { get; set; }
        public int? ToolId { get; set; }
        public string ToolName { get; set; }
        public int? GradeRoomId { get; set; }
        public string GradeRoomCode { get; set; }
        public string GradeRoomName { get; set; }
        public Int32 SamplePointId { get; set; }
        public string SamplePointName { get; set; }
        public string FirstSamplePointName { get; set; }
        public int? LastSamplePointName { get; set; }
        public List<RequestPurposesViewModel> Purposes { get; set; }
    }
}
