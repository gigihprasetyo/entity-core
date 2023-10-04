using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcs_product.API.Models
{
    public class QcTransactionTemplateTestingTypeMethodValidationParameter : BaseEntity
    {
        public bool NeedAttachment { get; set; }
        [Column(TypeName = "jsonb")]
        public string Properties { get; set; }

        public string UpdatedBy { get; set; }
        public int Value { get; set; }
        public string TestTypeMethodCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsNullable { get; set; }
        public Int32 InputTypeId { get; set; }
        public string Code { get; set; }
        public Int32 Sequence { get; set; }
        public Int32 TestTypeMethodId { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
        public bool IsInstruction { get; set; }
        public string AttachmentFile { get; set; }
        public string Instruction { get; set; }
        public bool IsExisting { get; set; }
        public Int32 TestTypeProcessProcedureParameterId { get; set; }
        public string ValidationResult { get; set; }
        public string TestTypeProcessProcedureParameterCode { get; set; }
    }
}
