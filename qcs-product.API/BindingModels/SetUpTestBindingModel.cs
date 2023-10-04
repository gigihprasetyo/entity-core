using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public class SetUpTestBindingModel
    {
        [Required]
        public Int32 QcTestId { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
    }
}
