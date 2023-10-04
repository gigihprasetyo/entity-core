using Microsoft.Extensions.Logging;
using qcs_product.API.DataProviders;
using qcs_product.API.ViewModels;
using qcs_product.API.Models;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.SettingModels;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class BaseApiBioServiceBusinessProviders : IBaseApiBioServiceBusinessProviders
    {
        private readonly BioHRServiceSetting _BioHRServiceSetting;
        private readonly IHttpClientFactory _clientFactory;

        [ExcludeFromCodeCoverage]
        public BaseApiBioServiceBusinessProviders(IOptions<BioHRServiceSetting> bioHRServiceSetting, IHttpClientFactory clientFactory)
        {
            _BioHRServiceSetting = bioHRServiceSetting.Value;
            _clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage> Perfom(string endPoint, HttpMethod method, string content, string token = null)
        {
            string url = $"{_BioHRServiceSetting.BaseUrl}{endPoint}";

            HttpRequestMessage request = new HttpRequestMessage(method, url);

            request.Headers.Add("Accept", "application/json");
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            if (token != null)
                request.Headers.Add("Authorization", $"Bearer {token}");

            HttpClient client = _clientFactory.CreateClient();

            return await client.SendAsync(request);

        }
    }
}
