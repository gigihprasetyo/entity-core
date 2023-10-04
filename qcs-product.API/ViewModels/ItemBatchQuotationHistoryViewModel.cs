using System;

namespace qcs_product.API.ViewModels
{
    public class ItemBatchQuotationHistoryViewModel
    {
        public Int32 QcSampleId { get; set; }
        
        public Int32 QcSamplingMaterialsId { get; set; }
        
        public Int32? SamplingPointId { get; set; }
        
        public string NoBatch { get; set; }
    }
}