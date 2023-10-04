using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.API.WorkflowModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Helpers;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class FacilityBusinessProvider : IFacilityBusinessProvider
    {
        private readonly IFacilityDataProvider _dataProvider;
        private readonly ILogger<FacilityBusinessProvider> _logger;

        [ExcludeFromCodeCoverage]
        public FacilityBusinessProvider(
            IFacilityDataProvider dataProvider,
            ILogger<FacilityBusinessProvider> logger
            )
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<FacilityAHUViewModel>> facilityAHUList(string search)
        {
            ResponseViewModel<FacilityAHUViewModel> result = new ResponseViewModel<FacilityAHUViewModel>();
            List<FacilityAHUViewModel> getData = await _dataProvider.getFacilityAHUList(search);

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
