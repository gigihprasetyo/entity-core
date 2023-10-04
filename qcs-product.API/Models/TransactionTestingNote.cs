using System;

namespace qcs_product.API.Models
{
    public class TransactionTestingNote
    {
        public string Name { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TestingId { get; set; }
        public string CreatedBy { get; set; }
        public string RowStatus { get; set; }
        public string Position { get; set; }
        public int Id { get; set; }
        public string TestingCode { get; set; }
        public string Note { get; set; }

    }
}
