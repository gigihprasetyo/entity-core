using System;

namespace qcs_product.API.Models
{
    public class TransactionTestTypeMethodValidationParameter 
    {
        public int Id { get; set; }
        public bool NeedAttachment { get; set; }
        public string Properties { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsNullable { get; set; }
        public int InputTypeId { get; set; }
        public string Code { get; set; }
        public int Sequence { get; set; }
        public string TestTypeMethodCode { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
        public bool IsInstruction { get; set; }
        public string AttachmentFile { get; set; }
        public string Instruction { get; set; }
        public bool IsExisting { get; set; }
        public string TestTypeProcessProcedureParameterCode { get; set; }
        public string ValidationResult { get; set; }
        public int TransactionTestingProcedureParameterId { get; set; }
        public int TestingId { get; set; }
    }
}
