using System;

namespace qcs_product.API.Models
{
    public class TemplateTestingNote : BaseEntity
    {
        public string Note { get; set; }
        public string Name { get; set; }
        public int TemplateTestingId { get; set; }
        public string TemplateTestingCode { get; set; }
        public string RowStatus { get; set; }
        public string Position { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
