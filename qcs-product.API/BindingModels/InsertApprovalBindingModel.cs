using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class InsertApprovalBindingModel
    {
        [Required]
        public int DataId { get; set; }
        public string Notes { get; set; }
        [Required]
        public string DigitalSignature { get; set; }
        [Required]
        public string NIK { get; set; }
        [Required]
        public bool IsApprove { get; set; }
        [Required]
        public int DataType { get; set; }
    }
}
