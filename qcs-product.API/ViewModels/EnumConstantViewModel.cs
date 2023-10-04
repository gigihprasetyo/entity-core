using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class EnumConstantViewModel
    {
        public Int32 Id { get; set; }
        public Int32 TypeId { get; set; }
        public string KeyGroup { get; set; }
        public string KeyValueLabel { get; set; }
        public string Name { get; set; }

    }
}
