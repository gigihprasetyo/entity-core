using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class ShipmentResponseVIewModel
    {
        public int TotalSuccess { get; set; }
        public int TotalFailed { get; set; }
        public List<ShipmentResponseMessageViewModel> DetailMessages { get; set; }
    }
}
