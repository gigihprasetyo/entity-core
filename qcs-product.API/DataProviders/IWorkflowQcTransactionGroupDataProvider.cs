using qcs_product.API.Models;
using qcs_product.API.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IWorkflowQcTransactionGroupDataProvider
    {
        public Task<WorkflowQcTransactionGroup> GetByWorkflowByQcTransactionGroupIdIsInWorkflow(int qcTransactionGroupId);
        public Task<List<WorkflowQcTransactionGroup>> GetByWorkflowByQcTransactionGroupIdIsInWorkflowAlt(int qcTransactionGroupId);
        public Task<WorkflowQcTransactionGroup> UpdateWorkflowQcTransactionGroupDataFromApproval(UpdateWorkflowQcTransactionGroupFromApproval data);
        public Task Insert(WorkflowQcTransactionGroup data);
        public Task<List<WorkflowQcTransactionGroup>> GetByWorkflowByQcTransactionGroupId(int qcTransactionGroupId);
        public Task<WorkflowQcTransactionGroup> GetByWorkflowByQcTransactionGroupIdLatest(int qcTransactionGroupId);
        public Task<List<WorkflowQcTransactionGroup>> GetInWorkflowByTransactionGroupIds(List<int> qcTransactionGroupIds);
    }
}