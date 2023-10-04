using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class TypeRequestSapmleRelationViewModel
    {
        public Int32 TypeRequestId { get; set; }
        public Int32 TypeSamplingId { get; set; }
        public string RequestSamplingName { get; set; }
        public string RequestSamplingLabel { get; set; }
    }
}
