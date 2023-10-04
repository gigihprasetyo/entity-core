using System;

namespace qcs_product.API.ViewModels
{
    public class TransactionTestingDeviationViewModel
    {
        public int Id { get; set; }
        public int SampleId { get; set; }
        public int ProductId { get; set; }
        public string Batch {  get; set; }
        public DateTime ReportDate { get; set; }
        public int TestTypeId { get; set; }
        public string TestTypeName { get; set; }
    }
}
