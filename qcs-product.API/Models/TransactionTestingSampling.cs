using System;
using System.ComponentModel.DataAnnotations;

namespace qcs_product.API.Models
{
    public class TransactionTestingSampling
    {
        [Key]
        public int Id { get; set; }
        public int SampleId { get; set; }
        public string SampleName { get; set; }
        public int TestingId { get; set; }
        public string Notes { get; set; }
        public string Attachment { get; set; }
        public string TestingCode { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
