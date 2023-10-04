using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class RoomViewModel
    {
        public Int32 Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Int32 GradeRoomId { get; set; }
        public string GradeRoomName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
