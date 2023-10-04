using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IProductionPhaseBusinessProvider
    {
        public Task<ResponseViewModel<ProductionPhaseViewModel>> List(string search, int limit, int page);
        Task<ResponseViewModel<ProductionPhaseViewModel>> GetDetailProductionPhaseById(int productionPhaseId);
    }
}
