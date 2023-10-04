using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class ProductTestType
    {
        public Int32 Id { get; set; }
        public Int32 ItemId { get; set; }
        public Int32 TestTypeId { get; set; }
        public int SampleAmountCount { get; set; }
        public double SampleAmountVolume { get; set; }
        public string SampleAmountUnit { get; set; }
        public string SampleAmountPresentation { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
