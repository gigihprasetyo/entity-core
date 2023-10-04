using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class InsertProcessQARejectWhenCompleteModel
    {
        public int SamplingId { get; set; }
        public string NIKUserQA { get; set; }
        public string Notes { get; set; }
    }
}