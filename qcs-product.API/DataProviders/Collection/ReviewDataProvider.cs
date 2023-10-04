using qcs_product.API.Infrastructure;
using qcs_product.API.WorkflowModels;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using qcs_product.API.SettingModels;
using System.Net.Http;
using System.Net.Http.Headers;
using qcs_product.Constants;
using System.Text.Json;

namespace qcs_product.API.DataProviders.Collection
{
    public class ReviewDataProvider : IReviewDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly IOptions<WorkflowServiceSetting> _WorkflowServiceSetting;
        private readonly IWorkflowServiceDataProvider _workflowServiceDataProvider;

        [ExcludeFromCodeCoverage]
        public ReviewDataProvider(QcsProductContext context,
            IOptions<WorkflowServiceSetting> workflowServiceSetting,
            IWorkflowServiceDataProvider workflowServiceDataProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _WorkflowServiceSetting = workflowServiceSetting;
            _workflowServiceDataProvider = workflowServiceDataProvider;
        }

        public HttpClient HeaderAPIWorkflowService()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_WorkflowServiceSetting.Value.BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public async Task<ResponseInsertReview> InitialDoc(NewWorkflowDocument data)
        {
            ResponseInsertReview result = new ResponseInsertReview();
            
            var endPoint = $"v1/WorkflowDocument/InitiateWorkflowDocument";
            var token = "";
            var content = JsonSerializer.Serialize(data);

            HttpResponseMessage Res = await _workflowServiceDataProvider._Perfom(endPoint, HttpMethod.Post, content, token);

            NewWorkflowDocumentViewModel responseSubmit = await Res.Content.ReadAsAsync<NewWorkflowDocumentViewModel>();

            if (Res.IsSuccessStatusCode)
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

        public async Task<ResponseInsertReview> InsertReview(WorkflowDocumentSubmitModel insert)
        {
            ResponseInsertReview result = new ResponseInsertReview();
            
            var endPoint = $"v1/WorkflowDocument/SubmitDocumentAction";
            var token = "";
            var content = JsonSerializer.Serialize(insert);

            HttpResponseMessage Res = await _workflowServiceDataProvider._Perfom(endPoint, HttpMethod.Post, content, token);

            SubmitDocumentActionResponseViewModel responseSubmit = await Res.Content.ReadAsAsync<SubmitDocumentActionResponseViewModel>();

            if (Res.IsSuccessStatusCode)
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
            var endPoint = $"v1/WorkflowDocument/GetDocumentPIC?document_code=" + WorkflowDocumentCode + "&application_code=" + ApplicationConstant.APP_CODE;
            var token = "";
            var content = "";

            HttpResponseMessage Res = await _workflowServiceDataProvider._Perfom(endPoint, HttpMethod.Get, content, token);

            DocumentPICResponseModel workflowPIC = await Res.Content.ReadAsAsync<DocumentPICResponseModel>();

            return workflowPIC;
        }

        public async Task<int> GetWorkflowActionId(string WorkflowDocumentCode, string action)
        {
            var endPoint = $"v1/WorkflowDocument/GetDocumentActions?document_code=" + WorkflowDocumentCode + "&application_code=" + ApplicationConstant.APP_CODE;
            var token = "";
            var content = "";

            HttpResponseMessage Res = await _workflowServiceDataProvider._Perfom(endPoint, HttpMethod.Get, content, token);

            DocumentActionResponseViewModel workflowAction = await Res.Content.ReadAsAsync<DocumentActionResponseViewModel>();

            int actionId = 0;

            foreach (var item in workflowAction.Actions)
            {
                if (item.ActionName == action)
                {
                    actionId = item.WorkflowActionId;
                }
            }

            if (Res.IsSuccessStatusCode)
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
            var endPoint = $"v1/WorkflowDocument/GetDocumentHistory?document_code=" + workflowDocumentCode + "&application_code=" + ApplicationConstant.APP_CODE;
            var token = "";
            var content = "";

            HttpResponseMessage Res = await _workflowServiceDataProvider._Perfom(endPoint, HttpMethod.Get, content, token);

            DocumentHistoryResponseViewModel workflowInfo = await Res.Content.ReadAsAsync<DocumentHistoryResponseViewModel>();

            return workflowInfo;
        }

    }
}
