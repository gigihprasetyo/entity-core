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
    public class TypeFormBusinessProvider : ITypeFormBusinessProvider
    {
        private readonly ITypeFormDataProvider _dataProvider;
        private readonly ILogger<TypeFormBusinessProvider> _logger;
        public TypeFormBusinessProvider(ITypeFormDataProvider dataProvider, ILogger<TypeFormBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<TypeFormViewModel>> List()
        {
            ResponseViewModel<TypeFormViewModel> result = new ResponseViewModel<TypeFormViewModel>();

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
