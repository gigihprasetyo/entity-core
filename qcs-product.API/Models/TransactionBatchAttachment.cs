using System;

namespace qcs_product.API.Models
{
    public partial class TransactionBatchAttachment
    {
        public int Id { get; set; }
        public int TrsBatchId { get; set; }
        public string Title { get; set; }
        public string AttachmentFile { get; set; }
        public string AttachmentStorageName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}