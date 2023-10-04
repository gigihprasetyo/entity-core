using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class GradeRoomViewModel
    {
        public Int32 Id { get; set; }
        public Int32 TestGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<TestScenarioViewModel> TestScenario { get; set; }
    }
}
