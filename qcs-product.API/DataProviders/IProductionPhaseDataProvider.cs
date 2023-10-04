using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IProductionPhaseDataProvider
    {
        public Task<List<ProductionPhaseViewModel>> List(string search, int limit, int page);

        public Task<List<ProductionPhaseViewModel>> GetDetailProductionPhaseById(int productionPhaseId);
    }
}
