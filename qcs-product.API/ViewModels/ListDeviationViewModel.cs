using Google.Type;

namespace qcs_product.API.ViewModels
{
    public class ListDeviationViewModel
    {
        public string SampleId { get; set; }
        public string ProductId { get; set; }
        public string Batch { get; set; }
        public DateTime ReportDate { get; set; }
        public string TestTypeNameId { get; set; }
        public string TestTypeNameEn { get; set; }
        public string TestTypeCode { get; set; }
        public int TestTypeId { get; set; }
        public int TransactionTestTypeProcedureParameterId { get; set; }
    }
}
