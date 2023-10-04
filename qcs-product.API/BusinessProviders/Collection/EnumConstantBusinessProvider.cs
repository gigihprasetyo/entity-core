using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class EnumConstantBusinessProvider : IEnumConstantBusinessProvider
    {
        private readonly IEnumConstantDataProvider _dataProvider;

        private readonly ILogger<EnumConstantBusinessProvider> _logger;

        [ExcludeFromCodeCoverage]
        public EnumConstantBusinessProvider(IEnumConstantDataProvider dataProvider, ILogger<EnumConstantBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<EnumConstantViewModel>> List(String search, String keyGroup)
        {
            ResponseViewModel<EnumConstantViewModel> result = new ResponseViewModel<EnumConstantViewModel>();

            var getData = await _dataProvider.List(search, keyGroup);

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
