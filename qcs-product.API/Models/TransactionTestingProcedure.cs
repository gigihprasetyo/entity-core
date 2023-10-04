using System;
using System.Collections.Generic;

namespace qcs_product.API.Models
{
    public class TransactionTestingProcedure : BaseEntity
    {
        public TransactionTestingProcedure() 
        {
            ProcedureParameter = new HashSet<TransactionTestingProcedureParameter>();
        }
        public int Id { get; set; }
        public int? TransactionTestTypeMethodId { get; set; }
        public string TestTypeMethodCode { get; set; }
        public string Title { get; set; }
        public string Instruction { get; set; }
        public string RowStatus { get; set; }
        public int Sequence { get; set; }
        public string AttachmentStorageName { get; set; }
        public string AttachmentFile { get; set; }
        public bool IsEachSample { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Status { get; set; }

        public virtual ICollection<TransactionTestingProcedureParameter> ProcedureParameter { get; set; }

    }
}
