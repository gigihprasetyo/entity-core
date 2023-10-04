using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcSamplingMaterialViewModel
    {
        public Int32 Id { get; set; }
        public Int32 QcSamplingId { get; set; }
        public Int32 ItemId { get; set; }
        public string ItemName { get; set; }
        public Int32? ItemBatchId { get; set; }
        public string NoBatch { get; set; }
        public Int32 Quantity { get; set; }
        
        public ItemBatchQuotationViewModel ItemBatchQuotation { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
