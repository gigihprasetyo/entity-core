using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.API.WorkflowModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IQcSamplingDataProvider
    {
        public Task<List<QcRequestSamplingRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int? orgId, int TypeRequestId, int SamplingTypeId);
        public Task<List<QcSamplingRelationViewModel>> GetDetailRelationById(int id, string sort);
        public Task<QcSampling> Edit(QcSampling data, List<EditSampleQcSampleBindingModel> editSampleQcSamples, List<EditSampleQcPersonelBindingModel> editSampleQcPersonel, EditBatchRequestQcBindingModel editSampleBatch);
        public Task<QcSamplingRelationViewModel> GetShortById(int id);
        public Task<QcSampling> GetById(Int32 id);
        public Task<List<QcSample>> GetSampleBySamplingId(Int32 SamplingId);
        public Task<List<SampleAvailableViewModel>> ListSampleAvailable(string search, List<int> roomId, Int32 testParamId);
        public Task<List<TestParameterAvailableViewModel>> ListTestParamAvailable(string search, Int32 testGroupId, Int32 requestId);
        public Task<List<ParameterThresholdRelationViewModel>> ListParameterThreshold(List<int> roomId, int? testScenarioId = null);
        public Task<List<ParameterThresholdRelationAltViewModel>> ListParameterThresholdAlt(List<int> GradeRoomId, string testScenarioLabel, int? testGroupId);
        public Task<List<QcLabelRelationViewModel>> ListLabelSampleQc(Int32 SamplingId, Int32 SampleId, string SampleCode, Int32 samplePointId, Int32 testParameterId);
        public Task<List<QcLabelBatchRelationViewModel>> ListLabelBatchQc(List<int> SamplingId, string SampleCode);
        public Task<QcLabelBatchRelationViewModel> GetSamplingBatchByQRCode(string QRCode);
        public string generateCodeSample(int length);
        public string updateCodeSample(string codeSample, List<TransactionTestScenario> lsTestScenario, int? TestScenarioId);
        public Task<QcSampling> UpdateQcSamplingDataFromApproval(UpdateQcSamplingFromApproval data);
        public Task<int> getOrganizatiionBySamplingId(int samplingId);
        public Task<string> generateQcResult(QcSampling data);
        public Task<int> GetRequestIdByWorkflowCode(string workflowDocumentCode, string workflowStatus);
        public Task<List<int>> GetAllRequestIdByWorkflowCode(List<ListReviewPending> listReviewPending, string workflowStatus);
        public Task<List<QcSampling>> GetByRequestId(int id);
        public Task<bool> GetRequestIdStatusComplete(int requestQcsId);
        public Task<bool> ValidationEditSampling(int samplingId);
        public Task<bool> getRiviewKabagSampling(int samplingId);
        public Task<List<QcSampling>> GetSamplingByRequestIdOnPending(int id, string nik);
        public Task<List<QcSampling>> GetSamplingByWorkflowCodeIdOnPending(string workflowDocumentCode, string nik);
        public Task<RequestQcs> GetRequestBySamplingId(int samplingId);
        public Task<List<int>> GetPurposeByRequestId(int requestQcsId);
        public Task<List<PurposesPersonel>> GetPurposePersonelByPurposeId(int purposeId);
        public Task<List<int>> GetRequestIdSamplingInReviewQa();
        public Task<int> GetSampleAmountCountById(int id, int testParameter);
        public Task<string> GetBuildingCodeBySamplingId(int samplingId);
        public Task<string> GetPosIdBySamplingId(int samplingId);
        public Task<List<string>> GetPosIdBySamplingIdV2(int samplingId);
        public Task<List<QcSamplingNotReceivedViewModel>> ListSamplingNotReceived(string search);
        public Task<bool> GetRequestIdSamplingComplete(int requestId, string workflowStatus);
        public Task<List<int>> ListRequestIdSamplingComplete(List<int> requestId);
        public Task<QcSampling> GetRequestIdSamplingStillInReview(int requestId);
        public Task<List<QcSample>> UpdateSampleReviewQaNote(InsertReviewQaNoteQcSample insert);
        public Task<List<QcLabelBatchRelationViewModel>> GetPendingReview(string workflowStatus);
        public Task<List<QcLabelBatchRelationViewModel>> GetCompleteReview();
        public Task<List<SampleAvailableViewModel>> ListSampleAvailableBySamplingId(string search, int samplingId, int? testParamId, string testScenarioLabel);
        public Task<List<ParameterThresholdRelationAltViewModel>> ListParameterThresholdAltV2(List<int> GradeRoomId, string testScenarioLabel, int? testGroupId);
    }
}
