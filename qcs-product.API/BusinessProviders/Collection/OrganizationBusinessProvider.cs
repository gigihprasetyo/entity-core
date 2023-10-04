using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Q100Library.EventBus.Base.Abstractions;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Q100Library.Authorization.Constants;
using Q100Library.IntegrationEvents;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using qcs_product.API.DataProviders;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class OrganizationBusinessProvider : IOrganizationBusinessProvider
    {

        private readonly IOrganizationDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public OrganizationBusinessProvider(
            IOrganizationDataProvider dataProvider
        )
        {
            _dataProvider = dataProvider;
        }


        public async Task<ResponseViewModel<OrganizationViewModel>> List()
        {
            List<OrganizationViewModel> organizationList = await _dataProvider.List();

            ResponseViewModel<OrganizationViewModel> result = new ResponseViewModel<OrganizationViewModel>()
            {
                StatusCode = 200,
                Message = ApplicationConstant.OK_MESSAGE,
                Data = organizationList
            };

            return result;
        }
    }
}