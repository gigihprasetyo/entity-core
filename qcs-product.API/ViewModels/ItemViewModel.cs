using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class ItemViewModel
    {
        public Int32 Id { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public Int32 ProductFormId { get; set; }
        public string ItemGroupId { get; set; }
        public string ItemGroupName { get; set; }
        public string LabelId { get; set; }
        public Int32 OrgId { get; set; }
        public string OrgName { get; set; }
    }
}
