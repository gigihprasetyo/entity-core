using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IItemBusinessProvider
    {
        public Task<ResponseViewModel<ItemViewModel>> List();
        public Task<ResponseOneDataViewModel<ItemViewModel>> Get(int id);
        // public Task<ResponseViewModel<ItemBatchRelationViewModel>> List(string search, int GroupId, string GroupLabel, DateTime? startDate, DateTime? endDate);
        // public Task<ResponseViewModel<ItemRelationViewModel>> GetDetailRelationById(int id);
        // public Task<ResponseViewModel<ShortDataListViewModel>> ShortList(string search, int GroupId);
        // public Task<ResponseViewModel<ItemBatchRelationViewModel>> ListMediaWithBatch(string search, DateTime? startDate, DateTime? endDate);
    }
}
