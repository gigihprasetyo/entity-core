using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class TestParameterByScenarioRelationViewModel
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public Int32 RoomId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public Int32 GradeRoomId { get; set; }
        public string GradeRoomCode { get; set; }
        public string GradeRoomName { get; set; }
        public List<EmTestTypeViewModel> TestParameter { get; set; }
    }
}
