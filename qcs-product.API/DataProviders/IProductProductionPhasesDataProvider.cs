using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IProductProductionPhasesDataProvider
    {
        public Task<List<ProductProductionPhaseViewModel>> List(string search, int limit);
        public Task<List<ProductProductionPhasesPersonelViewModel>> GetPersonelByPhaseId(int phaseId);
    }
}
