using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    public class SamplingPointLiteViewModel
    {
        public int SamplingPointId { get; set; }
        public string SamplingPointCode { get; set; }
    }
}
