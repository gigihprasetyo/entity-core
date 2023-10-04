using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcTransactionGroupFormParameter : BaseEntity
    {
        public Int32 QcTransactionGroupFormProcedureId { get; set; }
        public int Sequence { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public int InputType { get; set; }
        public int? Uom { get; set; }
        public int? ThresholdOperator { get; set; }
        public decimal? ThresholdValue { get; set; }
        public decimal? ThresholdValueTo { get; set; }
        public decimal? ThresholdValueFrom { get; set; }
        public bool NeedAttachment { get; set; }
        public string Note { get; set; }
        public Int32 FormProcedureId { get; set; }
        public bool IsForAllSample { get; set; }
        public bool IsResult { get; set; }
        public string DefaultValue { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<QcTransactionGroupValue> TransactionGroupValues { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<QcTransactionGroupSampleValue> TransactionGroupSampleValues { get; set; }

    }
}
