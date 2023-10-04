using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class ToolActivity
    {
        public Int32 Id { get; set; }
        public int ToolId { get; set; }
        public int ActivityId { get; set; }
        public string ActivityCode { get; set; }
        public DateTime ActivityDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
