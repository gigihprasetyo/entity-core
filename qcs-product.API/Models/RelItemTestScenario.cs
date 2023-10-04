using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class RelItemTestScenario
    {
        public Int32 Id { get; set; }
        public Int32 ItemId { get; set; }
        public Int32 TestScenarioId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
