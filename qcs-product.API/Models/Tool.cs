using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class Tool
    {
        public Int32 Id { get; set; }
        public string ToolCode { get; set; }
        public string Name { get; set; }
        public Int32 ToolGroupId { get; set; }
        public Int32 RoomId { get; set; }
        public Int32 GradeRoomId { get; set; }
        public Int32? FacilityId { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string SerialNumberId { get; set; }

        public int? MachineId { get; set; }
        public int ObjectStatus { get; set; }

    }
}
