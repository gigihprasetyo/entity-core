using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.ViewModels
{
    public class TestScenarioViewModel
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public Int32? GradeRoomId { get; set; }
        public string GradeRoomCode { get; set; }
        public string GradeRoomName { get; set; }
        public List<TestParameterPubSubViewModel> TestParameters { get; set; }
        //public List<TestVariable> TestVariables { get; set; }

    }

    public class TestParameterPubSubViewModel
    {
        public int Id { get; set; }
        public string TestParameterName { get; set; }
        public List<TestVariable> TestVariables { get; set; }
    }
}
