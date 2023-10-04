using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class ItemProductGroup
    {
        public int Id { get; set; }
        public string ItemProductGroupCode { get; set; }
        public string ItemProductGroupName { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
