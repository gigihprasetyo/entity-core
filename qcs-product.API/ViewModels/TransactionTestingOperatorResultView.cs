using System;

namespace qcs_product.API.ViewModels
{
    public class TransactionTestingOperatorResultView
    {
        public int Id { get; set; }
        public int InputTypeId { get; set; }
        public Object Property { get; set; }
        public int ParameterIdExisting { get; set; }
        public int TestingId { get; set; }
        public int Existing {  get; set; }
        public bool IsExisting { get; set; }
        public string MethodCode { get; set; }
        public string Name { get; set; }
        public bool NeedAttachment { get; set; }
        public Object Properties { get; set; }
        public Object PropertiesValue { get; set; }
        public int Sequence {  get; set; }
        public int TransactionTestingProcedureParameterId { get; set; }
    }
}
