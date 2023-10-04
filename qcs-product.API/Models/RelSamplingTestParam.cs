using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class RelSamplingTestParam
    {
        public Int32 Id { get; set; }
        public Int32 SamplingPointId { get; set; }
        public Int32 TestScenarioParamId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
