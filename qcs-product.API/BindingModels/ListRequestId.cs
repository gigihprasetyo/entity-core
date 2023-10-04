using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class ListRequestId
    {
        public int QcRequestId { get; set; }
        public string StatusName { get; set; }
        public string Pic { get; set; }
    }
}
