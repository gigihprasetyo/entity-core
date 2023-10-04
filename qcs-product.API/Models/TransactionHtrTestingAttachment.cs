using System;

namespace qcs_product.API.Models
{
    public class TransactionHtrTestingAttachment
    {
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RowStatus { get; set; }
        public string Filename { get; set; }
        public string ExecutorNik { get; set; }
        public string MediaLink { get; set; }
        public int Id { get; set; }
        public string ExecutorName { get; set; }
        public int TestingId { get; set; }
        public string TestingCode { get; set; }
        public string ExecutorPosition { get; set; }
        public string Ext { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Note { get; set; }
        public string Action { get; set; }
    }
}
