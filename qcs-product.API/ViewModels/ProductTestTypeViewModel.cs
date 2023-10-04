using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class ProductTestTypeViewModel
    {
        public Int32 Id { get; set; }
        public Int32 ItemId { get; set; }
        public Int32 TestTypeId { get; set; }
        public string Name { get; set; }
        public int SampleAmountCount { get; set; }
        public double SampleAmountVolume { get; set; }
        public string SampleAmountUnit { get; set; }
        public string SampleAmountPresentation { get; set; }
        public string RowStatus { get; set; }
        public Int32 OrgId { get; set; }
        public string OrgName { get; set; }
    }
}
