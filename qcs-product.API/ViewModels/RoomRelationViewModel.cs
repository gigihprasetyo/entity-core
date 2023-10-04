using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class RoomRelationViewModel
    {
        public Int32 RoomId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public Int32 GradeRoomId { get; set; }
        public string GradeRoomCode { get; set; }
        public string GradeRoomName { get; set; }
        public Int32? FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public int? AhuId { get; set; }
        public string AhuCode { get; set; }
        public string AhuName { get; set; }
        public List<EmTestTypeViewModel> EmTestTypeParameter { get; set; }
    }
}
