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
    public class PersonelBusinessProvider : IPersonelBusinessProvider
    {
        private readonly IPersonelDataProvider _dataProvider;
        private readonly ILogger<PersonelBusinessProvider> _logger;
        public PersonelBusinessProvider(IPersonelDataProvider dataProvider, ILogger<PersonelBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<QcPersonelViewModel>> List(string search)
        {
            ResponseViewModel<QcPersonelViewModel> result = new ResponseViewModel<QcPersonelViewModel>();
            List<QcPersonelViewModel> getData = await _dataProvider.List(search);

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
