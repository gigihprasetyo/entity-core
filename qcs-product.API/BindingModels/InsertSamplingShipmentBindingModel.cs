using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class InsertSamplingShipmentBindingModel
    {
        [Required]
        public DateTime ShipmentDate { get; set; }
        [Required]
        public string ShipmentIdLogger { get; set; }
        [Required]
        public string ShipmentTemperature { get; set; }
        [Required]
        public string OrgId { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
        public List<SendingReceivedSampleBindingModel> QRCodes { get; set; }
    }
}
