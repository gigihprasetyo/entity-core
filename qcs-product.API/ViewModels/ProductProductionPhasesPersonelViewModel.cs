using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class ProductProductionPhasesPersonelViewModel
    {
        public int ProductProdPhasesPersonelId { get; set; }
        public int ProductProductionPhasesId { get; set; }
        public string ProductProductionPhasesName { get; set; }
        public string PersonelNik { get; set; }
        public string PersonelName { get; set; }
    }
}
