using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.BindingModels;

namespace qcs_product.API.ViewModels
{
    public class RoomDetailRelationViewModel
    {
        public Int32 RoomId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public Int32 GradeRoomId { get; set; }
        public string GradeRoomCode { get; set; }
        public string GradeRoomName { get; set; }
        public Int32? AhuId { get; set; }
        public string AhuCode { get; set; }
        public string AhuName { get; set; }
        public List<SamplingPointLiteViewModel> SamplingPointInfo { get; set; }
    }
}
