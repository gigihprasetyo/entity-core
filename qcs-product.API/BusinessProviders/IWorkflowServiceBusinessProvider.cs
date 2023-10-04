using qcs_product.API.ViewModels;
using qcs_product.API.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.WorkflowModels;

namespace qcs_product.API.BusinessProviders
{
    public interface IWorkflowServiceBusinessProvider
    {
        public Task<ResponseViewModel<ResponseInsertReview>> SubmitAction(WorkflowSubmitBindingModel data);
        public Task<ResponseViewModel<ResponseInsertReview>> InitiateDoc(NewWorkflowDocument data);
        public Task<bool> RollbackDocument(RollbackWorkflowDocument data);
    }
}