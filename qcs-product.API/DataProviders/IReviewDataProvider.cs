using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using qcs_product.API.WorkflowModels;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IReviewDataProvider
    {
        public HttpClient HeaderAPIWorkflowService();
        public Task<ResponseInsertReview> InitialDoc(NewWorkflowDocument data);
        public Task<ResponseInsertReview> InsertReview(WorkflowDocumentSubmitModel insert);
        public Task<DocumentPICResponseModel> GetPIC(string WorkflowDocumentCode);
        public Task<int> GetWorkflowActionId(string WorkflowDocumentCode, string action);
        public Task<DocumentHistoryResponseViewModel> GetListHistoryWorkflow(string workflowDocumentCode);

    }
}
