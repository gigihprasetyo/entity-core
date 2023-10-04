using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public class EditNoBatchBindingModel
    {
        [Required]
        public Int32 RequestId { get; set; }
        //public Int32? SamplingId { get; set; }
        [Required]
        public string NoBatch { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
    }
}
