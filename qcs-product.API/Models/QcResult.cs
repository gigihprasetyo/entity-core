using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcResult
    {
        public Int32 Id { get; set; }
        public Int32? ParentId { get; set; }
        public Int32 SampleId { get; set; }
        public string Value { get; set; }
        public string TestVariableConclusion { get; set; }
        public Int32? TestVariableId { get; set; }
        public string Note { get; set; }
        public string AttchmentFile { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
