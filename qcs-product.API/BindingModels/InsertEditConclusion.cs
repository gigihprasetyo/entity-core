using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class InsertEditConclusion
    {
        public int QcRequestId { get; set; }
        public string Conclusion { get; set; }
        public string UpdatedBy { get; set; }
    }
}
