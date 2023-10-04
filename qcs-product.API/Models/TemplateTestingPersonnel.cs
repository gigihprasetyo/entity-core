using System;

namespace qcs_product.API.Models
{
    public class TemplateTestingPersonnel : BaseEntity
    {
        public string NewNik { get; set; }
        public string Nik { get; set; }
        public string Name { get; set; }
        public string TemplateTestingCode { get; set; }
        public int TemplateTestingId { get; set; }
        public string PositionCode { get; set; }
        public int PositionId { get; set; }
        public string Position { get; set; }
        public string RowStatus { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
