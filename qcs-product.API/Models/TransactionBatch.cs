using System;

namespace qcs_product.API.Models
{
    public partial class TransactionBatch
    {
        public int Id { get; set; }
        public int RequestQcsId { get; set; }
        public string AttachmentNotes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
