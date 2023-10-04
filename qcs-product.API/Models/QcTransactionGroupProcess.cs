using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcTransactionGroupProcess : BaseEntity
    {
        public Int32 QcTransactionGroupId { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public Int32? ParentId { get; set; }
        public Int32? RoomId { get; set; }
        public int? IsInputForm { get; set; }
        public Int32 QcProcessId { get; set; }
        public string RowStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<QcTransactionGroupFormProcedure> TransactionGroupFormProcedures { get; set; }

        [JsonIgnore]
        public virtual ICollection<QcTransactionGroupFormTool> TransactionGroupFormTools { get; set; }

    }
}
