using System;
using System.Collections.Generic;

namespace qcs_product.API.ViewModels
{
    public class ItemBatchQuotationViewModel
    {
        public Int32 CurrentQuantity { get; set; }
        public List<ItemBatchQuotationHistoryViewModel> QcSampleHistory { get; set; }
    }
}