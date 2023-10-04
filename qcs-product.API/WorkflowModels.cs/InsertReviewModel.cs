using System;
using qcs_product.API.Infrastructure;
using System.Diagnostics.CodeAnalysis;


namespace qcs_product.API.WorkflowModels
{
    [ExcludeFromCodeCoverage]
    public class InsertReviewModel
    {
        public int ModulId { get; set; }
        public string NIK { get; set; }
        public string Notes { get; set; }
        public string DigitalSignature { get; set; }
        public string DocumentCode { get; set; }
    }
}