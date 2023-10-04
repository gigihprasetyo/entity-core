using System;

namespace qcs_product.API.Models
{
    public partial class TransactionHtrTestingProcedureParameter
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int DeviationLevel { get; set; }
        public string DeviationNote { get; set; }
        public bool HasAttachment { get; set; }
        public int? InputTypeId { get; set; }
        public bool IsNullable { get; set; }
        public string Name { get; set; }
        public object Properties { get; set; }
        public object PropertiesValue { get; set; }
        public string RowStatus { get; set; }
        public int Sequence { get; set; }
        public bool IsDeviation { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int ParameterId { get; set; }
        public string ExecutorName { get; set; }
        public string ExecutorPosition { get; set; }
        public string ExecutorNik { get; set; }
        public virtual TransactionTestingProcedureParameter Paramater { get; set; }
    }
}
