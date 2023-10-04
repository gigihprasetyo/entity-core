using System;
using System.Text.Json.Serialization;

namespace qcs_product.API.Models
{
    public class TransactionTestingProcedureParameterAttachment
    {
        public int Id { get; set; }
        public int TransactionTestingProcedureParameterId { get; set; }
        public string Filename { get; set; }
        public string MediaLink { get; set; }
        public string Ext { get; set; }
        public string ExecutorName { get; set; }
        public string ExecutorPosition { get; set; }
        public string ExecutorNik { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
        public string Type { get; set; }
        public int TransactionTestingSamplingId { get; set; }

        [JsonIgnore]
        public virtual TransactionTestingProcedureParameter ProcedureParameter { get; set;}
    }
}
