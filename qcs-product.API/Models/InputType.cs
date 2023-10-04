using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class InputType
    {
        public Int32 Id { get; set; }
        public Int32 TypeId { get; set; }
        public string Label { get; set; }
        public string Reference { get; set; }
        public string ReferenceType { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
