using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class ProductGroupViewModel
    {
        public Int32 Id { get; set; }
        public string GroupCode { get; set; }
        public string Name { get; set; }
        public string RowStatus { get; set; }
    }
}
