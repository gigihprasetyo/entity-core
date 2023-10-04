using Microsoft.Extensions.Logging;
using qcs_product.API.DataProviders;
using qcs_product.API.ViewModels;
using qcs_product.API.Models;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class ItemBusinessProvider : IItemBusinessProvider
    {
        private readonly IItemDataProvider _dataProvider;
        private readonly ILogger<ItemBusinessProvider> _logger;
        public ItemBusinessProvider(IItemDataProvider dataProvider, ILogger<ItemBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<ItemViewModel>> List()
        {
            ResponseViewModel<ItemViewModel> result = new ResponseViewModel<ItemViewModel>();
            List<ItemViewModel> getItem = await _dataProvider.List();
            if (!getItem.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getItem;
            }
            return result;
        }

        /// <summary>
        /// get item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>item</returns>
        public async Task<ResponseOneDataViewModel<ItemViewModel>> Get(int id)
        {
            ResponseOneDataViewModel<ItemViewModel> result = new ResponseOneDataViewModel<ItemViewModel>();
            ItemViewModel getItem = await _dataProvider.GetById(id);
            if (getItem == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getItem;
            }
            return result;
        }

        // public async Task<ResponseViewModel<ItemBatchRelationViewModel>> List(string search, int GroupId, string GroupLabel, DateTime? startDate, DateTime? endDate)
        // {
        //     ResponseViewModel<ItemBatchRelationViewModel> result = new ResponseViewModel<ItemBatchRelationViewModel>();
        //     var groupFilter = new List<string>();
        //     if (GroupLabel == null)
        //     {
        //         groupFilter.Add(ApplicationConstant.MEDIA);
        //         groupFilter.Add(ApplicationConstant.PRODUCT);
        //         groupFilter.Add(ApplicationConstant.RAW_MATERIAL);
        //         groupFilter.Add(ApplicationConstant.M_SAMP);
        //         groupFilter.Add(ApplicationConstant.M_CA);
        //         groupFilter.Add(ApplicationConstant.M_TSA);
        //     }
        //     else
        //     {
        //         // filter status from param status is string
        //         groupFilter = GroupLabel.Split(',').Select(x => x).Reverse().ToList();
        //     }

        //     List<ItemBatchRelationViewModel> getData = await _dataProvider.List(search, GroupId, groupFilter, startDate, endDate);

        //     if (!getData.Any())
        //     {
        //         result.StatusCode = 404;
        //         result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
        //     }
        //     else
        //     {
        //         result.StatusCode = 200;
        //         result.Message = ApplicationConstant.OK_MESSAGE;
        //         result.Data = getData;
        //     }

        //     return result;
        // }

        // public async Task<ResponseViewModel<ItemBatchRelationViewModel>> ListMediaWithBatch(string search, DateTime? startDate, DateTime? endDate)
        // {
        //     ResponseViewModel<ItemBatchRelationViewModel> result = new ResponseViewModel<ItemBatchRelationViewModel>();

        //     List<ItemBatchRelationViewModel> getData = await _dataProvider.ListMediaWithBatch(search, startDate, endDate);

        //     if (!getData.Any())
        //     {
        //         result.StatusCode = 404;
        //         result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
        //     }
        //     else
        //     {
        //         result.StatusCode = 200;
        //         result.Message = ApplicationConstant.OK_MESSAGE;
        //         result.Data = getData;
        //     }

        //     return result;
        // }

        // public async Task<ResponseViewModel<ItemRelationViewModel>> GetDetailRelationById(int id)
        // {
        //     ResponseViewModel<ItemRelationViewModel> result = new ResponseViewModel<ItemRelationViewModel>();
        //     _logger.LogInformation($"getData: {id}");
        //     var getData = await _dataProvider.GetDetailRelationById(id);
        //     _logger.LogInformation($"getData: {getData}");

        //     if (!getData.Any())
        //     {
        //         result.StatusCode = 404;
        //         result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
        //     }
        //     else
        //     {
        //         result.StatusCode = 200;
        //         result.Message = ApplicationConstant.OK_MESSAGE;
        //         result.Data = getData;
        //     }

        //     return result;

        // }

        // public async Task<ResponseViewModel<ShortDataListViewModel>> ShortList(string search, int GroupId)
        // {
        //     ResponseViewModel<ShortDataListViewModel> result = new ResponseViewModel<ShortDataListViewModel>();
        //     List<ShortDataListViewModel> getData = await _dataProvider.ShortList(search, GroupId);

        //     if (!getData.Any())
        //     {
        //         result.StatusCode = 404;
        //         result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
        //     }
        //     else
        //     {
        //         result.StatusCode = 200;
        //         result.Message = ApplicationConstant.OK_MESSAGE;
        //         result.Data = getData;
        //     }

        //     return result;
        // }
    }
}
