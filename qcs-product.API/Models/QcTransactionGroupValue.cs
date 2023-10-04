using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcTransactionGroupValue : BaseEntity
    { 
        public Int32 QcTransactionGroupFormParameterId { get; set; }
        public int Sequence { get; set; }
        public string Value { get; set; }
        public string AttchmentFile { get; set; }
        public Int32? QcTransactionGroupFormMaterialId { get; set; }
        public Int32? QcTransactionGroupFormToolId { get; set; }
   
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
    }
}
