using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IEmProductionPhaseDataProvider
    {
        public Task<List<EmProductionPhaseRelationViewModel>> List(string search);
        public Task<List<EmProductionPhaseRelationViewModel>> GetByItemId(int itemId);
        public Task<List<EmTestTypeViewModel>> TestTypeList(int RoomId);
        
    }
}
