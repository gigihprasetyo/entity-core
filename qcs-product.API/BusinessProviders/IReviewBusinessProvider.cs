using qcs_product.API.ViewModels;
using qcs_product.API.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.WorkflowModels;

namespace qcs_product.API.BusinessProviders
{
    public interface IReviewBusinessProvider
    {
        public Task<ResponseViewModel<WorkflowDocumentSubmitModel>> InsertApproval(InsertApprovalBindingModel data);
        public Task<List<string>> _GetNikNextPicOrgId(string workflowStatus, int samplingId);
        // public async Task<ResponseViewModel<WorkflowDocumentSubmitModel>> InsertReviewReject(InsertReviewModel data);
        // public async Task<ResponseViewModel<WorkflowDocumentSubmitModel>> InsertReviewCancel(InsertReviewCancel data);
        // public async Task<ResponseViewModel<WorkflowDocumentSubmitModel>> InsertReviewEdit(InsertReviewEditModel data);

        // public async Task<List<WorkflowHistoryModel>> ListHistoryWorkflow(string docCode);
        // public async Task<ResponseViewModel<FormulirReviewViewModel>> GetFromReviewByDocumentCode(string documentCode);
        // public async Task<ResponseViewModel<FormulirReviewViewModel>> GetFromReviewByDocumentCodeComplete(string documentCode);
    }
}