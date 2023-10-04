using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcLabelBatchTestParamViewModel
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public int? Sequence { get; set; }
        public int? CountParamater { get; set; }
        public int? OrgId { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
    }
}
