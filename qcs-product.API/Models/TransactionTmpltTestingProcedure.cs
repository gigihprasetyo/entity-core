using System.Collections.Generic;

namespace qcs_product.API.Models
{
    public partial class TransactionTmpltTestingProcedure : BaseEntity
    {

        public TransactionTmpltTestingProcedure()
        {
            TransactionTmpltTestingProcedureParameter = new HashSet<TransactionTemplateTestTypeProcessProcedureParameter>();

        }
        public int Id { get; set; }
        public int TransactionTemplateTestingId { get; set; }
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

        public virtual ICollection<TransactionTemplateTestTypeProcessProcedureParameter> TransactionTmpltTestingProcedureParameter { get; set; }

    }
}
