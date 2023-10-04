using System;

namespace qcs_product.API.Models
{
    public class QcTransactionTemplateTestingType : BaseEntity
    {
        public string Code { get; set; }
        public string NameId { get; set; }
        public Int32 ObjectStatus { get; set; }
        public string CreatedBy { get; set; }
        public Int32 OrganizationId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string NameEn { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
