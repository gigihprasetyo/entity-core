using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace qcs_product.API.Models
{
    public class TransactionTmpltTestTypeProcessProcedureParameter : BaseEntity
    {

        public TransactionTmpltTestTypeProcessProcedureParameter()
        {
            TransactionTmpltTestingProcedureParameter = new HashSet<TransactionTemplateTestTypeProcessProcedureParameter>();
        }
        public string Attachment { get; set; }
        public string Code { get; set; }
        public string CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public int DeviationAttachment { get; set; }
        public int DeviationLevel { get; set; }
        public string DeviationNote { get; set; }
        public bool HasAttachment { get; set; }
        public int Id { get; set; }
        public int InputTypeId { get; set; }
        public bool IsNullable { get; set; }
        public string Name { get; set; }
        public int TestTypeProcessProcedureId { get; set; }
        public string TestTypeProcessProcedureCode { get; set; }
        public string Properties { get; set; }
        public string PropertiesValue { get; set; }
        public string RowStatus { get; set; }
        public int Sequence { get; set; }
        public string UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        [JsonIgnore]
        public virtual ICollection<TransactionTemplateTestTypeProcessProcedureParameter> TransactionTmpltTestingProcedureParameter { get; set; }

    }
}
