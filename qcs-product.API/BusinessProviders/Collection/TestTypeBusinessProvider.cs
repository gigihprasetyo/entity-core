using Microsoft.Extensions.Logging;
using qcs_product.API.DataProviders;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TestTypeBusinessProvider : ITestTypeBusinessProvider
    {
        private readonly ITestTypeDataProvider _dataProvider;
        private readonly ILogger<TestTypeBusinessProvider> _logger;

        public TestTypeBusinessProvider(ITestTypeDataProvider dataProvider, ILogger<TestTypeBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<TestTypeViewModel>> List()
        {
            ResponseViewModel<TestTypeViewModel> result = new ResponseViewModel<TestTypeViewModel>();

            var getData = await _dataProvider.List();

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
