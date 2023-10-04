using qcs_product.API.Infrastructure;
using qcs_product.API.WorkflowModels;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using qcs_product.API.SettingModels;
using System.Net.Http;
using qcs_product.Constants;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace qcs_product.API.DataProviders.Collection
{
    public class WorkflowServiceDataProvider : IWorkflowServiceDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IOptions<WorkflowServiceSetting> _WorkflowServiceSetting;

        [ExcludeFromCodeCoverage]
        public WorkflowServiceDataProvider(IConfiguration configuration, IHttpClientFactory clientFactory, QcsProductContext context,
            IOptions<WorkflowServiceSetting> workflowServiceSetting)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _WorkflowServiceSetting = workflowServiceSetting;
            _clientFactory = clientFactory;
            var builder = new ConfigurationBuilder()
                .AddJsonFile(ApplicationConstant.APPSETTING_PATH, optional: true);
            _configuration = builder.Build();
        }

        public async Task<HttpResponseMessage> _Perfom(string endPoint, HttpMethod method, string content, string token = null)
        {
            var baseUrl = _configuration["WorkflowServiceSetting:BaseUrl"];
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

        // public HttpClient HeaderAPIWorkflowService()
        // {
        //     HttpClient client = new HttpClient();
        //     client.BaseAddress = new Uri(_WorkflowServiceSetting.Value.BaseUrl);
        //     client.DefaultRequestHeaders.Clear();
        //     client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //     return client;
        // }

        public async Task<ResponseInsertReview> InitiateDoc(NewWorkflowDocument data)
        {
            ResponseInsertReview result = new ResponseInsertReview();
            var endPoint = $"v1/WorkflowDocument/InitiateWorkflowDocument";
            var token = "";
            var content = JsonSerializer.Serialize(data);

            HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Post, content, token);

            NewWorkflowDocumentViewModel responseSubmit = await response.Content.ReadAsAsync<NewWorkflowDocumentViewModel>();

            if (response.IsSuccessStatusCode)
            {
                result.StatusCode = responseSubmit.StatusCode;
                result.Message = responseSubmit.Message;
            }
            else
            {
                result.StatusCode = responseSubmit.StatusCode;
                result.Message = responseSubmit.Message;
            }
            return result;
        }

        public async Task<ResponseInsertReview> SubmitAction(WorkflowDocumentSubmitModel insert)
        {
            ResponseInsertReview result = new ResponseInsertReview();
            var endPoint = $"v1/WorkflowDocument/SubmitDocumentAction";
            var token = "";
            var content = JsonSerializer.Serialize(insert);

            HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Post, content, token);

            SubmitDocumentActionResponseViewModel responseSubmit = await response.Content.ReadAsAsync<SubmitDocumentActionResponseViewModel>();

            if (response.IsSuccessStatusCode)
            {
                result.StatusCode = responseSubmit.StatusCode;
                result.Message = responseSubmit.Message;
                result.WorkflowStatus = responseSubmit.CurrentStatus.CurrentStatusName;
            }
            else
            {
                result.StatusCode = responseSubmit.StatusCode;
                result.Message = responseSubmit.Message;
            }
            return result;
        }

        public async Task<DocumentPICResponseModel> GetPIC(string WorkflowDocumentCode)
        {
            // var client = HeaderAPIWorkflowService();
            var endPoint = $"v1/WorkflowDocument/GetDocumentPIC?document_code={WorkflowDocumentCode}&application_code={ApplicationConstant.APP_CODE}";
            var token = "";
            var content = "";

            HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Get, content, token);

            DocumentPICResponseModel workflowPIC = await response.Content.ReadAsAsync<DocumentPICResponseModel>();

            return workflowPIC;
        }

        public async Task<int> GetWorkflowActionId(string WorkflowDocumentCode, string action)
        {
            var endPoint = $"v1/WorkflowDocument/GetDocumentActions?document_code={WorkflowDocumentCode}&application_code={ApplicationConstant.APP_CODE}";
            var token = "";
            var content = "";

            HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Get, content, token);

            DocumentActionResponseViewModel workflowAction = await response.Content.ReadAsAsync<DocumentActionResponseViewModel>();

            int actionId = 0;

            foreach (var item in workflowAction.Actions)
            {
                if (item.ActionName == action)
                {
                    actionId = item.WorkflowActionId;
                }
            }

            if (response.IsSuccessStatusCode)
            {
                return actionId;
            }
            else
            {
                return 0;
            }
        }

        public async Task<DocumentHistoryResponseViewModel> GetListHistoryWorkflow(string workflowDocumentCode)
        {
            var endPoint = $"v1/WorkflowDocument/GetDocumentHistory?document_code={workflowDocumentCode}&application_code={ApplicationConstant.APP_CODE}";
            var token = "";
            var content = "";

            HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Get, content, token);

            DocumentHistoryResponseViewModel workflowInfo = await response.Content.ReadAsAsync<DocumentHistoryResponseViewModel>();

            return workflowInfo;
        }

        // orgId, padahal aslinya NIK 
        public async Task<bool> GetOrgIdInDocCodeStatus(string workflowDocumentCode, string orgId)
        {
            var endPoint = $"v1/WorkflowDocument/GetOrgIdInDocCodeStatus?document_code={workflowDocumentCode}&application_code={ApplicationConstant.APP_CODE}";
            var token = "";
            var content = "";

            HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Get, content, token);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> RollbackDocument(RollbackWorkflowDocument data)
        {
            var endPoint = $"v1/WorkflowDocument/RollbackDocument";
            var token = "";
            var content = JsonSerializer.Serialize(data);

            HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Post, content, token);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ListPendingWorkflow> GetListPendingByNik(string nik)
        {
            var endPoint = $"v1/WorkflowDocument/GetWorkflowPendingByAppCodeOrgId?ApplicationCode={ApplicationConstant.APP_CODE}&OrgId={nik}";
            var token = "";
            var content = "";

            HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Get, content, token);

            ListPendingWorkflow listPendingWorkflow = await response.Content.ReadAsAsync<ListPendingWorkflow>();

            return listPendingWorkflow;
        }

        public async Task<ListPendingWorkflow> GetListPendingByNik(string nik, string statusName)
        {
            var endPoint = $"v1/WorkflowDocument/GetWorkflowPendingByAppCodeStatusNameOrgId?ApplicationCode={ApplicationConstant.APP_CODE}&StatusName={ApplicationConstant.APP_CODE}&OrgId={nik}";
            var token = "";
            var content = "";

            HttpResponseMessage response = await _Perfom(endPoint, HttpMethod.Get, content, token);

            ListPendingWorkflow listPendingWorkflow = await response.Content.ReadAsAsync<ListPendingWorkflow>();

            return listPendingWorkflow;
        }
    }
}
