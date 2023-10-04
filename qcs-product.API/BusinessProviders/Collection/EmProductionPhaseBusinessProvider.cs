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
    public class EmProductionPhaseBusinessProvider : IEmProductionPhaseBusinessProvider
    {
        private readonly IEmProductionPhaseDataProvider _dataProvider;
        private readonly ILogger<EmProductionPhaseBusinessProvider> _logger;
        public EmProductionPhaseBusinessProvider(IEmProductionPhaseDataProvider dataProvider, ILogger<EmProductionPhaseBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<EmProductionPhaseRelationViewModel>> List(string search)
        {
            ResponseViewModel<EmProductionPhaseRelationViewModel> result = new ResponseViewModel<EmProductionPhaseRelationViewModel>();
            List<EmProductionPhaseRelationViewModel> getData = await _dataProvider.List(search);

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

        public async Task<ResponseViewModel<EmProductionPhaseRelationViewModel>> GetByItemId(int itemId)
        {
            ResponseViewModel<EmProductionPhaseRelationViewModel> result = new ResponseViewModel<EmProductionPhaseRelationViewModel>();
            List<EmProductionPhaseRelationViewModel> getData = await _dataProvider.GetByItemId(itemId);

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
