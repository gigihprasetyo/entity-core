using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.Helpers;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.API.WorkflowModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class QcSamplingTypeBusinessProvider : IQcSamplingTypeBusinessProvider
    {
        private readonly IQcSamplingTypeDataProvider _dataProvider;
        private readonly ILogger<QcSamplingTypeBusinessProvider> _logger;

        [ExcludeFromCodeCoverage]
        public QcSamplingTypeBusinessProvider(IQcSamplingTypeDataProvider dataProvider, ILogger<QcSamplingTypeBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<QcSamplingTypeViewModel>> List()
        {
            ResponseViewModel<QcSamplingTypeViewModel> result = new ResponseViewModel<QcSamplingTypeViewModel>();
            List<QcSamplingTypeViewModel> getData = await _dataProvider.List();

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