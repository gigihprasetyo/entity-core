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
    public class RoomBusinessProvider : IRoomBusinessProvider
    {
        private readonly IRoomDataProvider _dataProvider;
        private readonly ILogger<RoomBusinessProvider> _logger;
        public RoomBusinessProvider(IRoomDataProvider dataProvider, ILogger<RoomBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<RoomRelationViewModel>> GetDetailRoomById(int id)
        {
            ResponseViewModel<RoomRelationViewModel> result = new ResponseViewModel<RoomRelationViewModel>();
            List<RoomRelationViewModel> getData = await _dataProvider.GetDetailRelationById(id);

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

        public async Task<ResponseViewModel<RoomRelationViewModel>> List(string search, int limit, int page)
        {
            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<RoomRelationViewModel> result = new ResponseViewModel<RoomRelationViewModel>();
            List<RoomRelationViewModel> getData = await _dataProvider.List(search, limit, pagination.CalculateOffset());

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

        public async Task<ResponseViewModel<RoomSamplingPointLayout>> ListRoomSamplingPointLayout()
        {
            ResponseViewModel<RoomSamplingPointLayout> result = new ResponseViewModel<RoomSamplingPointLayout>();
            List<RoomSamplingPointLayout> getData = await _dataProvider.ListRoomSamplingPointLayout();
            //List<RoomSamplingPointLayoutViewModel> getData = new List<RoomSamplingPointLayoutViewModel>();

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

        public async Task<ResponseViewModel<RoomSamplingPointLayoutViewModel>> GetRoomSamplingPointLayoutBySamplingId(int samplingId)
        {
            ResponseViewModel<RoomSamplingPointLayoutViewModel> result = new ResponseViewModel<RoomSamplingPointLayoutViewModel>();
            List<RoomSamplingPointLayoutViewModel> getData = await _dataProvider.GetRoomSamplingPointLayoutBySamplingId(samplingId);
            //List<RoomSamplingPointLayoutViewModel> getData = new List<RoomSamplingPointLayoutViewModel>();
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
        public async Task<ResponseViewModel<RoomRelationViewModel>> ListFacilityAHU(string search, int FacilityId, string AhuId)
        {

            var AHUfilter = new List<int>();

            if (AhuId != null)
            {
                AHUfilter = AhuId.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            ResponseViewModel<RoomRelationViewModel> result = new ResponseViewModel<RoomRelationViewModel>();
            List<RoomRelationViewModel> getData = await _dataProvider.ListByFacilityAHU(search, FacilityId, AHUfilter);

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
