﻿namespace qcs_product.API.Models
{
    public class TransactionTmpltTestTypeProcessProcedure : BaseEntity
    {
        public int Id { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int TestTypeProcessId { get; set; }
        public string TestTypeProcessCode { get; set; }
        public string UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string Title { get; set; }
        public string Instruction { get; set; }
        public string AttachmentFile { get; set; }
        public string RowStatus { get; set; }
        public string AttachmentStorageName { get; set; }
    }
}

