using System;

namespace qcs_product.API.Models
{
    public class TransactionTesting
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime TestingDate { get; set; }
        public int ObjectStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string RowStatus { get; set; }
        public string TestTypeNameIdn { get; set; }
        public string TestTypeNameEn { get; set; }
        public string TestTypeCode { get; set; }
        public int TestTypeId { get; set; }
        public string TestTypeMethodName { get; set; }
        public string TestTypeMethodCode { get; set; }
        public int TestTypeMethodId { get; set; }
        public int? TestTemplateId { get; set; }
        public DateTime? TestingStartDate { get; set; }
        public DateTime? TestingEndtDate { get; set; }
    }
}
