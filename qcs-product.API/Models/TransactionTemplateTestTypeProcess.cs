using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace qcs_product.API.Models
{
    public class TransactionTemplateTestTypeProcess
    {
        public TransactionTemplateTestTypeProcess()
        {
            TransactionTemplateTestTypeProcessProcedure = new HashSet<TransactionTemplateTestTypeProcessProcedure>();
        }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public string MethodCode { get; set; }
        public string UpdatedBy { get; set; }
        public int Methodid { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Id { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<TransactionTemplateTestTypeProcessProcedure> Procedures { get; set; }
        public virtual TransactionTemplateTestTypeMethod TransactionTemplateTestTypeMethod { get; set; }

        [JsonIgnore]
        public virtual ICollection<TransactionTemplateTestTypeProcessProcedure> TransactionTemplateTestTypeProcessProcedure { get; set; }

    }
}
