using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class ItemBatchRelationViewModel
    {
        public Int32 Id { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public Int32 ProductFormId { get; set; }
        public Int32? ItemGroupId { get; set; }
        public string ItemGroupName { get; set; }
        public Int32? LabelId { get; set; }
        public Int32 OrgId { get; set; }
        public string OrgName { get; set; }
        public List<ItemBatchNumberViewModel> BatchNumbers { get; set; }
    }
}
