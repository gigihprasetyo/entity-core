using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class ShipmentResponseMessageViewModel
    {
        public string QRCode { get; set; }
        
        public string TestParamName { get; set; }
        
        public int TestParamId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
