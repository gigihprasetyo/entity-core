using System;

namespace qcs_product.API.Models
{
    public class TemplateTestingAttachment : BaseEntity
    {
        public string Filename { get; set; }
        public string ExecutorNik { get; set; }
        public string ExecutorName { get; set; }
        public string ExecutorPosition { get; set; }
        public string MediaLink { get; set; }
        public int TemplateTestingId { get; set; }
        public string TemplateTestingCode { get; set; }
        public string RowStatus { get; set; }
        public string Ext { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
