using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class ItemRelationViewModel
    {
        public ItemViewModel Item { get; set; }
        public ProductFormViewModel ProductForm { get; set; }
        public ProductGroupViewModel ProductGroup { get; set; }
        public ProductPresentationViewModel ProductPresentation { get; set; }
        public StorageTemperatureViewModel StorageTemperature { get; set; }
        public List<ProductTestTypeViewModel> ProductTestTypes { get; set; }
        public List<ProductProductionPhaseViewModel> ProductProductionPhase { get; set; }
        public List<ItemBatchNumberViewModel> BatchNumbers { get; set; }


    }
}
