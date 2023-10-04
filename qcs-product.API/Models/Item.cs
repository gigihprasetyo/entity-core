using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class Item
    {
        public Int32 Id { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public Int32 ProductFormId { get; set; }
        public int ObjectStatus { get; set; }
        public string Temperature { get; set; }
        public Int32? ItemGroupId { get; set; }
        public string ItemGroupName { get; set; }
        public Int32 LabelId { get; set; }
        public Int32 OrgId { get; set; }
        public string OrgName { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Int32 ProductGroupId { get; set; }
    }
}
