using System.Threading.Tasks;
using System.Net.Http;
using qcs_product.API.WorkflowModels;

namespace qcs_product.API.DataProviders
{
    public interface IWorkflowServiceDataProvider
    {
        public Task<HttpResponseMessage> _Perfom(string endPoint, HttpMethod method, string content, string token = null);
        public Task<ResponseInsertReview> InitiateDoc(NewWorkflowDocument data);
        public Task<ResponseInsertReview> SubmitAction(WorkflowDocumentSubmitModel insert);
        public Task<DocumentPICResponseModel> GetPIC(string WorkflowDocumentCode);
        public Task<int> GetWorkflowActionId(string WorkflowDocumentCode, string action);
        public Task<DocumentHistoryResponseViewModel> GetListHistoryWorkflow(string workflowDocumentCode);
        public Task<bool> GetOrgIdInDocCodeStatus(string workflowDocumentCode, string orgId);
        public Task<bool> RollbackDocument(RollbackWorkflowDocument data);
        public Task<ListPendingWorkflow> GetListPendingByNik(string nik);

    }
}
