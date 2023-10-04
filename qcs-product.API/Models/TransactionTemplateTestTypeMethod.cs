using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace qcs_product.API.Models
{
    public class TransactionTemplateTestTypeMethod
    {
        public TransactionTemplateTestTypeMethod()
        {
            TransactionTemplateTestTypeProcess = new HashSet<TransactionTemplateTestTypeProcess>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string RowStatus { get; set; }
        public int TestTypeId { get; set; }
        public string StandardProcedureNumber { get; set; }
        public string TestTypeCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public virtual ICollection<TransactionTemplateTestTypeProcess> TransactionTemplateTestTypeProcess { get; set; }

    }
}
