using System;

namespace qcs_product.API.Models
{
    public class TemplateOperatorTesting : BaseEntity
    {
        public DateTime? TestingDate { get; set; }
        public int ObjectStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ValidityPeriodStart { get; set; }
        public DateTime ValidityPeriodEnd { get; set; }
        public string IdTemplate { get; set; }
        public string RowStatus { get; set; }
        public string TestTypeNameIdn { get; set; }
        public string TestTypeNameEn { get; set; }
        public string TestTypeCode { get; set; }
        public Int32 TestTypeId { get; set; }
        public string TestTypeMethodName { get; set; }
        public string TestTypeMethodCode { get; set; }
        public Int32 TestTypeMethodId { get; set; }
    }
}
