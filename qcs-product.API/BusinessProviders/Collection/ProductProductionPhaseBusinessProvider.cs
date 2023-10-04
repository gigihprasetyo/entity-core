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
    public class ProductProductionPhaseBusinessProvider : IProductProductionPhaseBusinessProvider
    {
        private readonly IProductProductionPhasesDataProvider _dataProvider;
        private readonly ILogger<ProductProductionPhaseBusinessProvider> _logger;
        public ProductProductionPhaseBusinessProvider(IProductProductionPhasesDataProvider dataProvider, ILogger<ProductProductionPhaseBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }
        public async Task<ResponseViewModel<ProductProductionPhaseViewModel>> List(string search, int limit)
        {
            ResponseViewModel<ProductProductionPhaseViewModel> result = new ResponseViewModel<ProductProductionPhaseViewModel>();
            List<ProductProductionPhaseViewModel> getData = await _dataProvider.List(search, limit);

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

        public async Task<ResponseViewModel<ProductProductionPhasesPersonelViewModel>> ListProductPhasePersonel(int phaseId)
        {
            ResponseViewModel<ProductProductionPhasesPersonelViewModel> result = new ResponseViewModel<ProductProductionPhasesPersonelViewModel>();
            List<ProductProductionPhasesPersonelViewModel> getData = await _dataProvider.GetPersonelByPhaseId(phaseId);

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
