using qcs_product.API.ViewModels;
using qcs_product.API.Models;
using qcs_product.API.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.WorkflowModels;

namespace qcs_product.API.DataProviders
{
    public interface IQcTestDataProvider
    {
        public Task<List<QcTransactionGroupViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, string orderDir);
        public Task<QcTransactionGroup> GetByIdRaw(Int32 id);
        public Task<QcTransactionGroupProcess> GetGroupProcessByIdRaw(Int32 id);
        public Task<QcTransactionGroupProcess> GetGroupProcessByParentIdRaw(Int32 parentId);
        public Task<QcTransactionGroupFormMaterial> GetGroupFormMaterialByIdRaw(Int32 id);
        public Task<QcTransactionGroupFormTool> GetGroupFormToolByIdRaw(Int32 id);
        public Task<QcTransactionGroupFormProcedure> GetGroupFormProcedureByIdRaw(Int32 id);
        public Task<QcTransactionGroupFormParameter> GetGroupFormParameterByIdRaw(Int32 id);
        public Task<List<QcTransactionGroupSample>> GetSampleTestByIdRaw(Int32 TestId);
        public Task<List<QcTransactionGroupValue>> GetGroupValueByParameterIdRaw(Int32 ParameterId);
        public Task<List<QcTransactionGroupSampleValue>> GetGroupSampleValueByParameterIdRaw(Int32 ParameterId);
        public Task<QcTransactionGroupDetailViewModel> GetById(Int32 id);
        public Task<List<SampleByQcProcessRelationViewModel>> ListSampleTest(Int32 QcProcessId, string search, Int32 TestParamId, Int32 RoomId, Int32 PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate);
        public Task<List<SampleBatchQcProcessViewModel>> ListSampleBatchTest(Int32 QcProcessId, string search, Int32 RoomId, Int32 PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate);
        public Task<QcTransactionGroup> Insert(QcTransactionGroup data, List<QcTransactionGroupSample> sampleTest, List<QcTransactionGroupSampling> samplingTest);
        public Task<QcTransactionGroup> Edit(QcTransactionGroup data, List<EditQcProcessSample> sampleTest, List<EditQcProcessSamplingBatch> samplingTest);
        public Task<QcTransactionGroup> UpdateTest(StartQcTestBindingModel data);
        public Task<QcTransactionGroupProcess> GenerateQcTestProcessAlt(QcTransactionGroup data, List<QcTransactionGroupSample> sampleTest);
        public void GenerateQcResult(QcTransactionGroup data, List<QcTransactionGroupSampleValue> dataSampleObservasi, List<QcTransactionGroupSampleValue> dataSampleUjiIdentifikasi);
        public Task<QcTransactionGroup> UpdateQcTransactionGroupDataFromApproval(UpdateQcTransactionGroupFromApproval data);
        public Task<int> GetRequestIdByWorkflowCode(string workflowDocumentCode, string workflowStatus);
        public Task<List<int>> GetAllRequestIdByWorkflowCode(List<ListReviewPending> listReviewPending, string workflowStatus);
        public Task<List<RequestQcs>> GetQcRequestByTestId(int transactionGroupId);
        public Task<List<int>> GetPurposeByRequestId(int requestQcsId);
        public Task<List<PurposesPersonel>> GetPurposePersonelByPurposeId(int purposeId);
        public Task<int> getOrganizationByRequestId(int samplingId);
        public Task<QcTransactionGroup> UpdateRaw(QcTransactionGroup data);
        public Task<List<int>> GetByRequestId(int qcsRequestId);
        public Task<List<int>> GetSameSamplingById(int transactionGroupId);
        public Task<int> GetRequestIdStatusComplete(int requestQcsId);

        public Task<QcTransactionGroup> GetByIdForAudit(int id);

        public Task<List<int>> GetRequestIdSamplingTestingInReviewQa();
        public Task<List<int>> GetByRequestIdOnPending(int qcsRequestId, string nik);
        public Task<List<int>> GetByWorkflowDocumentCodeOnPending(string workflowDocumentCode, string nik);
        public Task<BindingRequestQcTransactionGroupModel> GetQcRequestByTransactionGroupId(int transactionGroupId);

        public Task<QcTransactionGroup> EditV2(QcTransactionGroup data, List<EditQcProcessSample> sampleTest, List<EditQcProcessSamplingBatch> samplingTest);
        public Task<QcTransactionGroupDetailViewModel> GetByIdV2(int id);
        public Task<List<SampleBatchQcProcessViewModel>> ListSampleBatchTestV2(Int32 QcProcessId, string search, Int32 RoomId, Int32 PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate);
        public Task<List<ParameterThresholdRelationAltViewModel>> ListParameterThresholdAlt(List<int> GradeRoomId, string testScenarioLabel, int? testGroupId);
        public Task<RequestQcs> GetByTransactionGroupSampling(int transactionGroupId);
        public Task<List<int>> ListRequestIdTestingComplete(List<int> requestId);
        public Task<QcTransactionGroup> GetTestDataByRequestId(int qcsRequestId);
        public Task<QcTransactionGroupDetailViewModel> GetTransactionGroupProcessById(int id);
        public Task<List<int>> ListRequestIdTestingNotComplete(List<int> requestId, string workflowStatus);
        public Task<List<QcTransactionGroupViewModel>> GetPendingReview(string workflowStatus);
    }
}
