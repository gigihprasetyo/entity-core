using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class GradeRoomDetailView
    {
        public Int32 Id { get; set; }
        public Int32 TestGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ObjectStatus { get; set; }
        public string GradeRoomDefault { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<TestScenarioViewModel> TestScenario { get; set; }
    }
}
