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
using qcs_product.API.BusinessProviders;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using qcs_product.API.Infrastructure;
using System.Text.Json;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class BioHRIntegrationBussinesProviders : IBioHRIntegrationBussinesProviders
    {
        private readonly IBaseApiBioServiceBusinessProviders _businessProviderBaseBioHR;
        private readonly IAuthenticatedUserBiohrDataProviders _dataProviderAuthBioHR;
        private readonly IOptions<BioHRServiceSetting> _BioHRServiceSetting;
        private readonly IOptions<WorkflowServiceSetting> _WorkflowServiceSetting;
        private readonly IHttpClientFactory _clientFactory;
        private readonly QcsProductContext _context;

        [ExcludeFromCodeCoverage]
        public BioHRIntegrationBussinesProviders(
            IBaseApiBioServiceBusinessProviders businessProviderBaseBioHR,
            IAuthenticatedUserBiohrDataProviders dataProviderAuthBioHR,
            IOptions<BioHRServiceSetting> bioHRServiceSetting,
            IHttpClientFactory clientFactory,
            QcsProductContext context,
            IOptions<WorkflowServiceSetting> workflowServiceSetting)
        {
            _businessProviderBaseBioHR = businessProviderBaseBioHR ?? throw new ArgumentNullException(nameof(businessProviderBaseBioHR));
            _dataProviderAuthBioHR = dataProviderAuthBioHR ?? throw new ArgumentNullException(nameof(dataProviderAuthBioHR));
            _BioHRServiceSetting = bioHRServiceSetting;
            _clientFactory = clientFactory;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _WorkflowServiceSetting = workflowServiceSetting;
        }

        public async Task<string> Login()
        {
            string endPoint = "/api/User/devauth";
            BioHrLoginViewModel body = new BioHrLoginViewModel
            {
                Username = "2097",
                Password = "bi0farma",
                ApplicationCode = _BioHRServiceSetting.Value.ApplicationCode //TODO : Must corection for login
            };

            string loginBodyString = JsonSerializer.Serialize(body);

            HttpResponseMessage response = await _businessProviderBaseBioHR.Perfom(endPoint, HttpMethod.Post, loginBodyString, null);

            ResponseBioHRLoginViewModel responseView = new ResponseBioHRLoginViewModel();
            if (response.IsSuccessStatusCode)
            {
                responseView = JsonSerializer.Deserialize<ResponseBioHRLoginViewModel>(response.Content.ReadAsStringAsync().Result);

                var authNow = await _dataProviderAuthBioHR.GetAuthenticatedTokenActived();
                if (authNow != null)
                    await _dataProviderAuthBioHR.Delete(authNow.Id);
                await _dataProviderAuthBioHR.Insert(responseView.Token);
            }

            return responseView.Token;
        }

        public async Task<ResponseViewModel<ResponseGetOrganizationBioHRViewModel>> GetOrganization()
        {
            string endPoint = $"api/Organization?groupCode=*";
            string body = "";

            int loop = 1;
            List<ResponseGetOrganizationBioHRViewModel> listResponse = new List<ResponseGetOrganizationBioHRViewModel>();
            var authUser = await _dataProviderAuthBioHR.GetAuthenticatedTokenActived();
            string token = "";
            if (authUser == null)
            {
                token = await Login();
            }
            else
            {
                token = authUser.Token;
            }
            while (loop <= 2)
            {

                HttpResponseMessage response = await _businessProviderBaseBioHR.Perfom(endPoint, HttpMethod.Get, body, token);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        listResponse = await JsonSerializer.DeserializeAsync<List<ResponseGetOrganizationBioHRViewModel>>(responseStream);
                    }
                    loop = 3;
                }
                else
                {
                    token = await Login();
                    loop += 1;
                }
            }

            //return listResponse;

            ResponseViewModel<ResponseGetOrganizationBioHRViewModel> result = new ResponseViewModel<ResponseGetOrganizationBioHRViewModel>();

            if (listResponse.Count < 0)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = listResponse;
            }

            return result;
        }

        public HttpClient HeaderAPIWorkflowService()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_WorkflowServiceSetting.Value.BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
        public async Task<ResponseGetOrganizationBioHRViewModel> GetOrganizationById(int OrgId)
        {
            string endPoint = $"api/Organization?groupCode=*";
            string body = "";

            int loop = 1;
            List<ResponseGetOrganizationBioHRViewModel> listResponse = new List<ResponseGetOrganizationBioHRViewModel>();
            var authUser = await _dataProviderAuthBioHR.GetAuthenticatedTokenActived();
            string token = "";
            if (authUser == null)
            {
                token = await Login();
            }
            else
            {
                token = authUser.Token;
            }
            while (loop <= 2)
            {

                HttpResponseMessage response = await _businessProviderBaseBioHR.Perfom(endPoint, HttpMethod.Get, body, token);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        listResponse = await JsonSerializer.DeserializeAsync<List<ResponseGetOrganizationBioHRViewModel>>(responseStream);
                    }
                    loop = 3;
                }
                else
                {
                    token = await Login();
                    loop += 1;
                }
            }

            return listResponse.Where(x => x.OrganizationId == OrgId).FirstOrDefault();
        }

        public async Task<ResponseGetEmployeeBioHRViewModel> GetEmployeeByNik(string Nik)
        {
            string endPoint = $"api/Personal/EmployeeDetail?employeeNumber={Nik}";
            string body = "";

            int loop = 1;
            List<ResponseGetEmployeeBioHRViewModel> listResponse = new List<ResponseGetEmployeeBioHRViewModel>();
            var authUser = await _dataProviderAuthBioHR.GetAuthenticatedTokenActived();
            string token = "";
            if (authUser == null)
            {
                token = await Login();
            }
            else
            {
                token = authUser.Token;
            }
            while (loop <= 2)
            {

                HttpResponseMessage response = await _businessProviderBaseBioHR.Perfom(endPoint, HttpMethod.Get, body, token);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        listResponse = await JsonSerializer.DeserializeAsync<List<ResponseGetEmployeeBioHRViewModel>>(responseStream);
                    }
                    loop = 3;
                }
                else
                {
                    token = await Login();
                    loop += 1;
                }
            }
            return listResponse.Where(x => x.NIK.ToLower() == Nik.ToLower()).FirstOrDefault();
        }

        public async Task<IEnumerable<ResponseGetEmployeeBioHRViewModel>> GetListEmployeeByListNik(List<string> lsNik)
        {
            string nikStr = String.Join(",", lsNik);
            string endPoint = $"api/Personal/ListEmployeeDetail?listPernr={nikStr}";
            string body = "";

            int loop = 1;
            List<ResponseGetEmployeeBioHRViewModel> listResponse = new List<ResponseGetEmployeeBioHRViewModel>();
            var authUser = await _dataProviderAuthBioHR.GetAuthenticatedTokenActived();
            string token = "";
            if (authUser == null)
            {
                token = await Login();
            }
            else
            {
                token = authUser.Token;
            }
            while (loop <= 2)
            {

                HttpResponseMessage response = await _businessProviderBaseBioHR.Perfom(endPoint, HttpMethod.Get, body, token);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        listResponse = await JsonSerializer.DeserializeAsync<List<ResponseGetEmployeeBioHRViewModel>>(responseStream);
                    }
                    loop = 3;
                }
                else
                {
                    token = await Login();
                    loop += 1;
                }
            }

            return listResponse;
        }

        public async Task<ResponseViewModel<ResponseGetNikByOrganizationIdAndPositionType>> GetNikByOrganizationIdandPositionType(string organizationId, string positionType)
        {
            string endPoint = $"api/User/PerOrganization?organizationId={organizationId}";
            string body = "";

            int loop = 1;
            List<ResponseGetNikByOrganizationIdAndPositionType> listResponse = new List<ResponseGetNikByOrganizationIdAndPositionType>();
            var authUser = await _dataProviderAuthBioHR.GetAuthenticatedTokenActived();
            string token = "";
            if (authUser == null)
            {
                token = await Login();
            }
            else
            {
                token = authUser.Token;
            }
            while (loop <= 2)
            {

                HttpResponseMessage response = await _businessProviderBaseBioHR.Perfom(endPoint, HttpMethod.Get, body, token);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        listResponse = await JsonSerializer.DeserializeAsync<List<ResponseGetNikByOrganizationIdAndPositionType>>(responseStream);
                    }
                    loop = 3;
                }
                else
                {
                    token = await Login();
                    loop += 1;
                }
            }

            ResponseViewModel<ResponseGetNikByOrganizationIdAndPositionType> result = new ResponseViewModel<ResponseGetNikByOrganizationIdAndPositionType>();

            if (listResponse.Count < 0)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = listResponse.Where(x => x.PositionType == positionType).ToList();
            }

            return result;
        }

        public async Task<ResponseGetEmployeeNewBioHRViewModel> GetEmployeeByNewNik(string newNik)
        {
            string endPoint = $"api/User/organization?newUserId={newNik}";
            //string endPoint = $"api/Organization?groupCode=*";
            string body = "";

            int loop = 1;
            List<ResponseGetEmployeeNewBioHRViewModel> listResponse = new List<ResponseGetEmployeeNewBioHRViewModel>();
            var authUser = await _dataProviderAuthBioHR.GetAuthenticatedTokenActived();
            string token = "";
            if (authUser == null)
            {
                token = await Login();
            }
            else
            {
                token = authUser.Token;
            }
            while (loop <= 2)
            {

                HttpResponseMessage response = await _businessProviderBaseBioHR.Perfom(endPoint, HttpMethod.Get, body, token);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        listResponse = await JsonSerializer.DeserializeAsync<List<ResponseGetEmployeeNewBioHRViewModel>>(responseStream);
                    }
                    loop = 3;
                }
                else
                {
                    token = await Login();
                    loop += 1;
                }
            }

            return listResponse.Where(x => x.NewUserId == newNik).FirstOrDefault();
        }

        public async Task<IEnumerable<ResponseGetEmployeeBioHRViewModel>> GetListEmployeeByListNewNik(List<string> lsNewNik)
        {
            string newNikStr = String.Join(",", lsNewNik);
            string endPoint = $"api/Personal/ListEmployeeDetailByListNewNik?listNewNik={newNikStr}";
            string body = "";

            int loop = 1;
            List<ResponseGetEmployeeBioHRViewModel> listResponse = new List<ResponseGetEmployeeBioHRViewModel>();
            var authUser = await _dataProviderAuthBioHR.GetAuthenticatedTokenActived();
            string token = "";
            if (authUser == null)
            {
                token = await Login();
            }
            else
            {
                token = authUser.Token;
            }
            while (loop <= 2)
            {

                HttpResponseMessage response = await _businessProviderBaseBioHR.Perfom(endPoint, HttpMethod.Get, body, token);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        listResponse = await JsonSerializer.DeserializeAsync<List<ResponseGetEmployeeBioHRViewModel>>(responseStream);
                    }
                    loop = 3;
                }
                else
                {
                    token = await Login();
                    loop += 1;
                }
            }

            return listResponse;
        }

        public async Task<ResponseOneDataViewModel<ResponseGetEmployeeNewBioHRViewModel>> GetNewUserBioHR(string newNik)
        {
            ResponseOneDataViewModel<ResponseGetEmployeeNewBioHRViewModel> result = new ResponseOneDataViewModel<ResponseGetEmployeeNewBioHRViewModel>();
            var getPersonel = await GetEmployeeByNewNik(newNik);

            if (getPersonel == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getPersonel;
            }

            return result;
        }

        public async Task<ResponseGetEmployeeBioHRViewModel> GetEmployeeByPosId(string posId)
        {
            string endPoint = $"api/Personal/EmployeeDetailbyPosition?positionId={posId}";
            string body = "";

            int loop = 1;
            List<ResponseGetEmployeeBioHRViewModel> listResponse = new List<ResponseGetEmployeeBioHRViewModel>();
            var authUser = await _dataProviderAuthBioHR.GetAuthenticatedTokenActived();
            string token = "";
            if (authUser == null)
            {
                token = await Login();
            }
            else
            {
                token = authUser.Token;
            }
            while (loop <= 2)
            {

                HttpResponseMessage response = await _businessProviderBaseBioHR.Perfom(endPoint, HttpMethod.Get, body, token);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        listResponse = await JsonSerializer.DeserializeAsync<List<ResponseGetEmployeeBioHRViewModel>>(responseStream);
                    }
                    loop = 3;
                }
                else
                {
                    token = await Login();
                    loop += 1;
                }
            }

            return listResponse.FirstOrDefault();
        }

        public async Task<IEnumerable<ResponseGetEmployeeBioHRViewModel>> GetListEmployeeByListPosId(List<int> lsPosId)
        {
            string posidsStr = String.Join(",", lsPosId);
            string endPoint = $"api/Personal/ListEmployeeDetailByListPosition?listPosid={posidsStr}";
            string body = "";

            int loop = 1;
            List<ResponseGetEmployeeBioHRViewModel> listResponse = new List<ResponseGetEmployeeBioHRViewModel>();
            var authUser = await _dataProviderAuthBioHR.GetAuthenticatedTokenActived();
            string token = "";
            if (authUser == null)
            {
                token = await Login();
            }
            else
            {
                token = authUser.Token;
            }
            while (loop <= 2)
            {

                HttpResponseMessage response = await _businessProviderBaseBioHR.Perfom(endPoint, HttpMethod.Get, body, token);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        listResponse = await JsonSerializer.DeserializeAsync<List<ResponseGetEmployeeBioHRViewModel>>(responseStream);
                    }
                    loop = 3;
                }
                else
                {
                    token = await Login();
                    loop += 1;
                }
            }

            return listResponse;
        }

        public async Task<List<ResponseGetDelegationViewModel>> GetDelegationByPosId(string posId)
        {
            string endPoint = $"api/Organization/GetDetailDelegation?positionId={posId}";
            string body = "";

            int loop = 1;
            List<ResponseGetDelegationViewModel> listResponse = new List<ResponseGetDelegationViewModel>();
            var authUser = await _dataProviderAuthBioHR.GetAuthenticatedTokenActived();
            string token = "";
            if (authUser == null)
            {
                token = await Login();
            }
            else
            {
                token = authUser.Token;
            }
            while (loop <= 2)
            {

                HttpResponseMessage response = await _businessProviderBaseBioHR.Perfom(endPoint, HttpMethod.Get, body, token);

                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        listResponse = await JsonSerializer.DeserializeAsync<List<ResponseGetDelegationViewModel>>(responseStream);
                    }
                    loop = 3;
                }
                else
                {
                    token = await Login();
                    loop += 1;
                }
            }

            return listResponse;
        }

        public async Task<List<string>> GetListNewNikDelegation(string posId)
        {
            List<string> listNIK = new List<string>();

            var delegation = await GetDelegationByPosId(posId);

            if (delegation.Any())
            {
                foreach (var item in delegation)
                {
                    var detailEmployee = await GetEmployeeByNik(item.PersonalNumberTo);
                    if (detailEmployee != null)
                    {
                        listNIK.Add(detailEmployee.NewNik);
                    }
                }
            }
            return listNIK;
        }
    }
}
