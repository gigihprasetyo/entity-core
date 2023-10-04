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
    public class ToolBusinessProvider : IToolBusinessProvider
    {
        private readonly IToolDataProvider _dataProvider;
        private readonly ILogger<ToolBusinessProvider> _logger;
        public ToolBusinessProvider(IToolDataProvider dataProvider, ILogger<ToolBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<ToolRelationViewModel>> List(string search, string ToolGroupId, string RoomId, int GradeRoomId, DateTime? startDate, DateTime? endDate, int? facilityId)
        {
            //get data activity master data
            var roomIdsFilter = new List<int>();
            List<int> ToolGroupFilter = await _dataProvider.ToolGroupIds();
            if (ToolGroupId != null)
            {
                ToolGroupFilter = ToolGroupId.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            // filter status from param status is string
            if (RoomId != null)
            {
                roomIdsFilter = RoomId.Split(',').Select(x => Int32.Parse(x)).Reverse().ToList();
            }
            else
            {
                roomIdsFilter.Add(0);
            }

            ResponseViewModel<ToolRelationViewModel> result = new ResponseViewModel<ToolRelationViewModel>();
            List<ToolRelationViewModel> getData = await _dataProvider.List(search, ToolGroupFilter, roomIdsFilter, GradeRoomId, startDate, endDate, facilityId);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;
        }

        public async Task<ResponseViewModel<ShortDataListViewModel>> ShortList(string search, int GroupId, string groupName, int? facilityId)
        {
            ResponseViewModel<ShortDataListViewModel> result = new ResponseViewModel<ShortDataListViewModel>();
            List<ShortDataListViewModel> getData = await _dataProvider.ShortList(search, GroupId, groupName, facilityId);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;
        }

        public async Task<ResponseViewModel<ToolGroupViewModel>> ToolGroupList(string search)
        {
            ResponseViewModel<ToolGroupViewModel> result = new ResponseViewModel<ToolGroupViewModel>();
            List<ToolGroupViewModel> getData = await _dataProvider.ToolGroupList(search);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;
        }
    }
}
