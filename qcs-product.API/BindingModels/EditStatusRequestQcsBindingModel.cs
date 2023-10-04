using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class EditStatusRequestQcsBindingModel
    {
        [Required]
        public Int32 Id { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public string UpdatedBy { get; set; }

    }
}
