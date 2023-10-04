using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class EmProductionPhaseRelationViewModel
    {
        public Int32 Id { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string QcProduct { get; set; }
        public string QcEm { get; set; }
        public Int32 FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public List<ToolsAHUViewModel> ToolsAHU { get; set; }
        public List<RelEmPhaseToRoomViewModel> ProductionRooms { get; set; }

    }

    public partial class RelEmPhaseToRoomViewModel
    {
        public Int32 Id { get; set; }
        public Int32 RoomId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public Int32 GradeRoomId { get; set; }
        public string GradeRoomCode { get; set; }
        public string GradeRoomName { get; set; }
        public int EmPhaseId { get; set; }
        public List<TestScenarioGradeRoomViewModel> TestScenario { get; set; }
    }

    public partial class TestScenarioGradeRoomViewModel
    {
        public Int32 Id { get; set; }
        public string TestScenarioName { get; set; }
        public string TestScenarioLabel { get; set; }
    }
}
