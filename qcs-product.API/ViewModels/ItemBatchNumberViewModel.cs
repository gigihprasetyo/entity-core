using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class ItemBatchNumberViewModel
    {
        public Int32 Id { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ExpireDate { get; set; }
        public Int32 Quantity { get; set; }
        
        public ItemBatchQuotationViewModel ItemBatchQuotation { get; set; }

    }
}
