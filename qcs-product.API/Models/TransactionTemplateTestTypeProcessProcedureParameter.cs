using System;
using System.Text.Json.Serialization;

namespace qcs_product.API.Models
{
    public class TransactionTemplateTestTypeProcessProcedureParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? DeviationAttachment { get; set; }
        public int? DeviationLevel { get; set; }
        public string DeviationNote { get; set; }
        public bool? HasAttachment { get; set; }
        public int? InputTypeId { get; set; }
        public bool? IsNullable { get; set; }
        public int? TestTypeProcessPrecedureId { get; set; }
      
        public string TestTypeProcessPrecedureCode { get; set; }
        public Object Properties { get; set; }
        public Object PropertiesValue { get; set; }
        public string RowStatus { get; set; }
        public string ComponentName { get; set; }
        public int? Sequence { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public virtual TransactionTemplateTestTypeProcessProcedure TransactionTemplateTestTypeProcessProcedure { get; set; }

    }
}
