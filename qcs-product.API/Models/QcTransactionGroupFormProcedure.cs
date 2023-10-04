using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcTransactionGroupFormProcedure : BaseEntity
    {
        public Int32 QcTransactionGroupProcessId { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public Int32 FormProcedureId { get; set; }
        public Int32 QcTransactionGroupSectionId { get; set; }
        public string RowStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<QcTransactionGroupFormParameter> TransactionGroupFormParameters { get; set; }
    }
}


//e.Property(e => e.QcTransactionGroupSectionId).HasColumnName("qc_transaction_group_section_id");