using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;

namespace qcs_product.API.BusinessProviders
{
    public interface IEmProductionPhaseBusinessProvider
    {
        public Task<ResponseViewModel<EmProductionPhaseRelationViewModel>> List(string search);
        public Task<ResponseViewModel<EmProductionPhaseRelationViewModel>> GetByItemId(int itemId);
    }
}
