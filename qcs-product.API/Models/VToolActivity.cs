using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class VToolActivity
    {
        public Int32 ToolId { get; set; }
        public string ToolCode { get; set; }
        public string ToolName { get; set; }
        public Int32 ToolGroupId { get; set; }
        public string ToolGroupName { get; set; }
        public string ToolGroupLabel { get; set; }
        public Int32 RoomId { get; set; }
        public string RoomName { get; set; }
        public Int32 GradeRoomId { get; set; }
        public string GradeRoomName { get; set; }
        public DateTime? ActivityDateValidation { get; set; }
        public DateTime? ExpireDateValidation { get; set; }
        public DateTime? ActivityDateCalibration { get; set; }
        public DateTime? ExpireDateCalibration { get; set; }
    }
}
