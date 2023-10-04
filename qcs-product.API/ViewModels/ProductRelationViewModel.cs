using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class ProductRelationViewModel
    {
        public ProductViewModel Product { get; set; }
        public ProductFormViewModel ProductForm { get; set; }
        public List<ProductTestTypeViewModel> ProductTestTypes { get; set; }
        public List<ProductProductionPhaseViewModel> ProductProductionPhase { get; set; }
    }
}
