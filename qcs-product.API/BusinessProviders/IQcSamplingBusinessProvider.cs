using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.API.WorkflowModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IQcSamplingBusinessProvider
    {
        public Task<ResponseViewModel<QcRequestSamplingRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int? orgId, int TypeRequestId, int SamplingTypeId);
        public Task<ResponseViewModel<QcSamplingRelationViewModel>> GetDetaiById(int id, string sort);
        public Task<ResponseViewModel<QcSampling>> Edit(EditSampleQcBindingModel data);
        public Task<ResponseViewModel<SampleAvailableViewModel>> ListSampleAvailable(string search, string roomId, Int32 testParamId);
        public Task<ResponseViewModel<TestParameterAvailableViewModel>> ListTestParamAvailable(string search, Int32 testGroupId, Int32 requestId);
        public Task<ResponseViewModel<ParameterThresholdRelationViewModel>> ListParameterThreshold(string roomId, int? testScenarioId = null);
        public Task<ResponseViewModel<ParameterThresholdRelationAltViewModel>> ListParameterThresholdAlt(string GradeRoomId, string testScenarioLabel, int? testGroupId);
        public Task<ResponseViewModel<QcLabelRelationViewModel>> ListLabelSampleQc(Int32 SamplingId, Int32 SampleId, string SampleCode, Int32 samplePointId, Int32 testParameterId);
        public Task<ResponseViewModel<QcLabelBatchRelationViewModel>> ListLabelBatchQc(string SamplingId, string SampleCode);
        public Task<ResponseViewModel<WorkflowSubmitBindingModel>> InsertApproval(InsertApprovalBindingModel data);
        public Task<List<string>> _GetNikNextPicOrgId(string workflowStatus, int samplingId);
        public Task<ResponseViewModel<WorkflowDocumentSubmitModel>> InsertToNextPhase(InsertNextPhase data);
        public Task<int> InsertSendingSample(string QrCode, DateTime TimeSending);
        public Task<ResponseViewModel<QcSamplingNotReceivedViewModel>> ListSamplingNotReceived(string search);
        public Task<ResponseViewModel<QcSample>> InsertReviewQaNoteDataSample(InsertReviewQaNoteQcSample insert);
        public Task<ResponseViewModel<SampleAvailableViewModel>> ListSampleAvailableBySamplingId(string search, int roomId, int? testParamId, string testScenarioLabel);

    }
}
