using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class QcSamplingPersonelViewModel
    {
        public int SamplingPersonelId { get; set; }
        public int? ProductProdPhasesPersonelId { get; set; }
        public string PersonelNik { get; set; }
        public string PersonelName { get; set; }
    }
}
