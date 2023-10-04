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
    public class ProductionPhaseBusinessProvider : IProductionPhaseBusinessProvider
    {
        private readonly IProductionPhaseDataProvider _dataProvider;
        private readonly ILogger<ProductionPhaseBusinessProvider> _logger;
        public ProductionPhaseBusinessProvider(IProductionPhaseDataProvider dataProvider, ILogger<ProductionPhaseBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<ProductionPhaseViewModel>> GetDetailProductionPhaseById(int id)
        {
            ResponseViewModel<ProductionPhaseViewModel> result = new ResponseViewModel<ProductionPhaseViewModel>();
            _logger.LogInformation($"getData: {id}");
            var getData = await _dataProvider.GetDetailProductionPhaseById(id);
            _logger.LogInformation($"getData: {getData}");
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

        public async Task<ResponseViewModel<ProductionPhaseViewModel>> List(string search, int limit, int page)
        {
            ResponseViewModel<ProductionPhaseViewModel> result = new ResponseViewModel<ProductionPhaseViewModel>();
            List<ProductionPhaseViewModel> getData = await _dataProvider.List(search, limit, page);

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
