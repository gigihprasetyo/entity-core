using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class ItemGroups
    {
        public Int32 Id { get; set; }
        public string ItemGroupCode { get; set; }
        public string ItemGroupName { get; set; }
        public int ObjectStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
