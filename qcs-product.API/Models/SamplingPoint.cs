using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class SamplingPoint
    {
        public Int32 Id { get; set; }
        public Int32? RoomId { get; set; }
        public Int32? ToolId { get; set; }
        public string Code { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
