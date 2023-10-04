using qcs_product.API.Models;
using qcs_product.API.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IWorkflowQcSamplingDataProvider
    {
        public Task<WorkflowQcSampling> GetByWorkflowByQcSamplingIdIsInWorkflow(int qcSamplingId);
        public Task UpdateWorkflowQcSamplingDataFromApproval(UpdateWorkflowQcSamplingFromApproval data);
        public Task Insert(WorkflowQcSampling data);
        public Task<List<WorkflowQcSampling>> GetByWorkflowByQcSamplingId(int qcSamplingId);
        public Task<WorkflowQcSampling> GetByWorkflowByQcSamplingIdAlt(int qcSamplingId);
        public Task<List<WorkflowQcSampling>> GetByWorkflowPhase2ByQcSamplingId(int qcSamplingId);
        public Task UpdateWorkflowQcSamplingDataFromApprovalNextPhase(UpdateWorkflowQcSamplingFromApproval data);
        public Task<WorkflowQcSampling> GetByWorkflowByQcSamplingIdLatest(int qcSamplingId);
        public Task<int> CountWorkflowNotCompleteBySamplingIdPhase2(int qcSamplingId);
        public Task<WorkflowQcSampling> GetWorkflowByRequestId(int requestId);

        public Task<List<WorkflowQcSampling>> GetInWorkflowBySamplingIds(List<int> samplingIds);

    }
}