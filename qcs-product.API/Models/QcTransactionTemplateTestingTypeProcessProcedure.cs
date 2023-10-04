using System;

namespace qcs_product.API.Models
{
    public class QcTransactionTemplateTestingTypeProcessProcedure : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public Int32 TestTypeProcessId { get; set; }
        public string TestTypeProcessCode { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string Title { get; set; }
        public string Instruction { get; set; }
        public string AttachmentFile { get; set; }
        public string RowStatus { get; set; }
        public string AttachmentStorageName { get; set; }
        public Int32 TransactionTemplateTestingId { get; set; }
        public bool IsEachSample { get; set; }

    }
}
