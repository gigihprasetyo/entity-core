using System;

namespace qcs_product.API.MasterModels
{
    public class TestTypeMethodValidationParameter
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public int OrderStatic { get; set; }
        public int TestTypeMethodId { get; set; }
        public string TestTypeMethodCode { get; set; }
        public string TestTypeMethodName { get; set; }
        public int TestTypeMethodValidationInstructionId { get; set; }
        public string RowStatus { get; set; }
        public int TestTypeProcessPrecedureParameterId { get; set; }
        public string TestTypeProcessPrecedureParameterCode { get; set; }
        public string TestTypeProcessPrecedureParameterName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
