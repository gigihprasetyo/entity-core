using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class TestTypeViewModel
    {
        public Int32 Id { get; set; }
        public Int32 OrgId { get; set; }
        public string OrgName { get; set; }
        public string TestTypeCode { get; set; }
        public string Name { get; set; }
    }
}
