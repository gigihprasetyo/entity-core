using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using qcs_product.API.ViewModels;
using qcs_product.Constants;

namespace qcs_product.API.BusinessProviders
{
    public class AUAMServiceBusinessProviders : IAUAMServiceBusinessProviders
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AUAMServiceBusinessProviders> _logger;

        [ExcludeFromCodeCoverage]
        public AUAMServiceBusinessProviders(IConfiguration configuration, IHttpClientFactory clientFactory, ILogger<AUAMServiceBusinessProviders> logger)
        {
            _clientFactory = clientFactory;
            var builder = new ConfigurationBuilder()
                .AddJsonFile(ApplicationConstant.APPSETTING_PATH, optional: true);
            _configuration = builder.Build();
            _logger = logger;
        }

        private async Task<HttpResponseMessage> _Perfom(string endPoint, HttpMethod method, string content, string token = null)
        {
            var baseUrl = _configuration["AUAMServiceURL"];
            string url = $"{baseUrl}{endPoint}";

            HttpRequestMessage request = new HttpRequestMessage(method, url);

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Origin", ApplicationConstant.APP_CODE);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            if (token != null)
                request.Headers.Add("Authorization", $"Bearer {token}");

            HttpClient client = _clientFactory.CreateClient();

            return await client.SendAsync(request);

        }

        public async Task<AUAMPersonalViewModel> GetPersonalDetailByNik(string nik)
        {
            try
            {
                var endPoint = $"v1/User/GetUserDetail?nik={nik}";
                var token = "";
                var content = "";

                HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Get, content, token);
                _logger.LogInformation($"url = {endPoint}");
                // var request = new HttpRequestMessage(HttpMethod.Get, url);

                // request.Headers.Add("Accept", "application/json");
                // request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                // var client = _clientFactory.CreateClient();
                // var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var result = await JsonSerializer.DeserializeAsync<AUAMResponse<List<AUAMPersonalViewModel>>>(responseStream);
                    foreach (var auamPersonal in result.Data)
                    {
                        return auamPersonal;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                // _logger.LogError($"url = {endPoint}, nik = {nik}");
            }

            return null;
        }

        public async Task<AUAMPersonalExtViewModel> GetPersonalExtDetailByNik(string ExtNik)
        {
            try
            {
                var endPoint = $"v1/ExtUser/GetDetailByNik?ExtUserNIK={ExtNik}";
                var token = "";
                var content = "";

                HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Get, content, token);
                _logger.LogInformation($"url = {endPoint}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var result = await JsonSerializer.DeserializeAsync<AUAMResponse<List<AUAMPersonalExtViewModel>>>(responseStream);
                    foreach (var auamPersonal in result.Data)
                    {
                        return auamPersonal;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                // _logger.LogError($"url = {url}, nik = {ExtNik}");
            }

            return null;
        }

        public async Task<List<AUAMPersonalViewModel>> ListPersonalByRole(string appCode, string roleCode)
        {
            try
            {
                var endPoint = $"v1/User/ListByRole?applicationCode={appCode}&roleCode={roleCode}";
                var token = "";
                var content = "";

                HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Get, content, token);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var result = await JsonSerializer.DeserializeAsync<AUAMResponse<List<AUAMPersonalViewModel>>>(responseStream);
                    return result.Data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                // _logger.LogError($"url = {url}");
            }

            return null;
        }
    }
}