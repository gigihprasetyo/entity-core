using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class ProductionPhaseViewModel
    {
        public Int32 Id { get; set; }
        public string ProdPhaseCode { get; set; }
        public string Name { get; set; }
        public string RowStatus { get; set; }
    }
}
