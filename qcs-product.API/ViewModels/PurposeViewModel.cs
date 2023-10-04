using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class PurposeViewModel
    {
        public Int32 Id { get; set; }
        public Int32 RequestTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
