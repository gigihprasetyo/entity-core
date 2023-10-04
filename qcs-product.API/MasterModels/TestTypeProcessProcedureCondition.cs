using System;

namespace qcs_product.API.MasterModels
{
    public class TestTypeProcessProcedureCondition
    {
        public int Id { get; set; }
        public int TestTypeProcessProcedureParameterId { get; set; }
        public int? TestTypeProcessProcedureId { get; set; }
        public Object? Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
    }
}
