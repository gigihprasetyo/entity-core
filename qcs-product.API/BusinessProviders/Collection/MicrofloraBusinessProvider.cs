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
    public class MicrofloraBusinessProvider : IMicrofloraBusinessProvider
    {
        private readonly IMicrofloraDataProvider _dataProvider;
        private readonly ILogger<MicrofloraBusinessProvider> _logger;
        public MicrofloraBusinessProvider(IMicrofloraDataProvider dataProvider, ILogger<MicrofloraBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<ShortDataListViewModel>> List(string search)
        {
            ResponseViewModel<ShortDataListViewModel> result = new ResponseViewModel<ShortDataListViewModel>();
            List<ShortDataListViewModel> getData = await _dataProvider.List(search);

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
