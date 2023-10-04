using System;
using System.Collections.Generic;

namespace qcs_product.API.ViewModels
{
    public class TransactionTestingDetailViewModel : TransactionTestingViewModel
    {
        public List<TransactionTestingSamplingViewModel> samplings { get; set; }
        public DateTime? TestingStartDate { get; set; }
        public DateTime? TestingEndDate { get; set; }
    }
    public class TransactionTestingSamplingViewModel
    {
        public int Id { get; set; }
        public int SamplingId { get; set; }
        public string SamplingName { get; set; }
        public int TestingId { get; set; }
        public string Notes { get; set; }
        public string Attachment { get; set; }
        public string TestingCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
