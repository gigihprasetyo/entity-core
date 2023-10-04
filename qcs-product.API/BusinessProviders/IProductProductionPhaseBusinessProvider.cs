using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IProductProductionPhaseBusinessProvider
    {
        public Task<ResponseViewModel<ProductProductionPhaseViewModel>> List(string search, int limit);
        public Task<ResponseViewModel<ProductProductionPhasesPersonelViewModel>> ListProductPhasePersonel(int phaseId);
    }
}
