using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace qcs_product.API.Models
{
    public class TransactionTemplateTestTypeProcessProcedure : BaseEntity
    {
        public TransactionTemplateTestTypeProcessProcedure()
        {
            TransactionTemplateTestTypeProcessProcedureParameter = new HashSet<TransactionTemplateTestTypeProcessProcedureParameter>();
        }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int TestTypeProcessId { get; set; }
        public int TransactionTemplateTestingId { get; set; }
        public string TestTypeProcessCode { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string Title { get; set; }
        public string Instruction { get; set; }
        public string AttachmentFile { get; set; }
        public string RowStatus { get; set; }
        public string AttachmentStorageName { get; set; }

        public virtual ICollection<TransactionTemplateTestTypeProcessProcedureParameter> TransactionTemplateTestTypeProcessProcedureParameter { get; set; }

    }
}
