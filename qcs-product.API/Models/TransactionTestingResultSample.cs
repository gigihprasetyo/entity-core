using System;

namespace qcs_product.API.Models
{
    public class TransactionTestingResultSample
    {
        public int Id { get; set; }
        //public string Note { get; set; }
        public bool NeedAttachment { get; set; }
        //public string AttachmentStorageName { get; set; }
        //public string AttachmentFile { get; set; }
        public string RowStatus { get; set; }
        //public string TestVariableConclusion { get; set; } -> masuk di properties
        public Object Properties { get; set; }
        public Object PropertiesValue { get; set; }
        public string TransactionTestTypeMethodResultParameterCode { get; set; }
        public int TransactionTestTypeMethodResultParameterId { get; set; }
        public int TransactionTestingId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
