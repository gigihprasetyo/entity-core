using System;

namespace qcs_product.API.Models
{
    public class TransactionTestingProcedureParameterNote
    {
        public int Id { get; set; }
        public int TransactionTestingProcedureParameterId { get; set; }
        public string Note { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Type { get; set; } //parameter, deviation
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
        public int TransactionTestingSamplingId { get; set; }

        public virtual TransactionTestingProcedureParameter ProcedureParameter { get; set; }
    }
}
