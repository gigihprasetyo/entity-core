using System;

namespace qcs_product.API.MasterModels
{
    public class TestTypeProcessProcedureParameter
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int InputTypeId { get; set; }
        public int? TestTypeProcessProcedureId { get; set; }
        public Object? Properties { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
        public bool NeedAttachment { get; set; }
        public bool IsNullable { get; set; }
        public int Sequence { get; set; }
    }
}
