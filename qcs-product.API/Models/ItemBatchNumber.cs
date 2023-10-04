using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class ItemBatchNumber
    {
        public Int32 Id { get; set; }
        public Int32 ItemId { get; set; }
        public Int32 ObjectStatus { get; set; }
        public Int32 Quantity { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ExpireDate { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
