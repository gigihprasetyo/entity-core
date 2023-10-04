using System;

namespace qcs_product.API.Models
{
    public class QcSamplingTemplate
    {
        public int Id { get; set; }
        public string name { get; set; }
        public DateTime ValidityPeriodStart { get; set; }
        public DateTime ValidityPeriodEnd { get; set; }
        public int TestTypeId { get; set; }
        public int MethodId { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
