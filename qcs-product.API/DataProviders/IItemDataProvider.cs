using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders
{
    public interface IItemDataProvider
    {
        public Task<List<ItemViewModel>> List();
        public Task<ItemViewModel> GetById(int id);
        public Task<List<ItemBatchRelationViewModel>> ListItemBatch(string search, int GroupId, List<string> groupCode, DateTime? startDate, DateTime? endDate);
        public Task<List<ItemRelationViewModel>> GetDetailRelationById(int id);
        public Task<ItemSingleBatchRelViewModel> GetItemBatchById(int id);
        public Task<List<ShortDataListViewModel>> ShortList(string search, int GroupId);
        public Task<ItemBatchItemGroupViewModel> ByItemBatch(string itemId, string batchNumber);
        public Task<Item> GetItemByCode(string itemCode);
        public Task<Item> Insert(Item data);
        public Task<Item> Update(Item data);
        public Task<ItemBatchNumber> InsertBatch(ItemBatchNumber data);
        public Task<ItemBatchNumber> UpdateBatch(ItemBatchNumber data);
        public Task<List<ItemBatchRelationViewModel>> ListMediaWithBatch(string search, DateTime? startDate, DateTime? endDate);
        public Task<ItemGroups> GetItemGroupsByCode(string code);
        public Task<ItemBatchQuotationViewModel> ItemMediaQuotation(string batchNumber);
    }
}
