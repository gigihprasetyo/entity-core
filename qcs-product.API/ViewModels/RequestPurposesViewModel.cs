using System;

namespace qcs_product.API.ViewModels
{
    public class RequestPurposesViewModel
    {
        public Int32 Id { get; set; }
        public Int32 PurposeId { get; set; }
        public string PurposeCode { get; set; }
        public string PurposeName { get; set; }
        
        public int RequestId { get; set; }
        public int SamplingPointId { get; set; }
        public int QcSamplingId { get; set; }
    }
}
