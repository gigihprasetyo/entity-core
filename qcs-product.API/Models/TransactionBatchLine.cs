using System;

namespace qcs_product.API.Models
{
    public partial class TransactionBatchLine
    {
        public int Id { get; set; }
        public int TrsBatchId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string NoBatch { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
