using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class TestTypeQcsViewModel
    {
        public Int32 Id { get; set; }
        public Int32 RequestQcsId { get; set; }
        public Int32 PurposeId { get; set; }
        public Int32 TestTypeId { get; set; }
        public string TestTypeName { get; set; }
        public Int32 TestTypeMethodId { get; set; }
        public string TestTypeMethodName { get; set; }
        public Int32? TestParameterId { get; set; }
        public string? TestParameterName { get; set; }
        public int? TestParameterSequence { get; set; }
        public int SampleAmountCount { get; set; }
        public double SampleAmountVolume { get; set; }
        public string SampleAmountUnit { get; set; }
        public string SampleAmountPresentation { get; set; }
        public Int32 OrgId { get; set; }
        public string OrgName { get; set; }
    }
}
