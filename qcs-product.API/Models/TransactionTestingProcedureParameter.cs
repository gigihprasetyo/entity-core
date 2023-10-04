using System;
using System.Collections.Generic;

namespace qcs_product.API.Models
{
    public class TransactionTestingProcedureParameter : BaseEntity
    {
        public TransactionTestingProcedureParameter() 
        {
            Attachments = new HashSet<TransactionTestingProcedureParameterAttachment>();
            Notes = new HashSet<TransactionTestingProcedureParameterNote>();
            Histories = new HashSet<TransactionHtrTestingProcedureParameter>();
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public int? DeviationLevel { get; set; }
        public string DeviationNote { get; set; }
        public bool HasAttachment { get; set; }
        public int? InputTypeId { get; set; }
        public bool IsNullable { get; set; }
        public string Name { get; set; }
        public int TransactionTestingProcedureId { get; set; }
        public string TestTypeProcedureCode { get; set; }
        public object Properties { get; set; }
        public object PropertiesValue { get; set; }
        public string RowStatus { get; set; }
        public int Sequence { get; set; }
        public bool? IsDeviation { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public virtual TransactionTestingProcedure Procedure { get; set; }

        public ICollection<TransactionTestingProcedureParameterAttachment> Attachments { get; set; }

        public ICollection<TransactionTestingProcedureParameterNote> Notes { get; set; }
        public ICollection<TransactionHtrTestingProcedureParameter> Histories { get; set; }
    }
}
