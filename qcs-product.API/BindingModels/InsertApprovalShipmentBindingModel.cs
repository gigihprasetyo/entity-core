using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public class InsertApprovalShipmentBindingModel
    {
        [Required]
        public int DataId { get; set; }
        public string Notes { get; set; }
        [Required]
        public string DigitalSignature { get; set; }
        [Required]
        public string NIK { get; set; }
    }
}
