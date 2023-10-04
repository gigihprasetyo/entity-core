using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IQcTestBusinessProvider
    {
        public Task<ResponseViewModel<QcTransactionGroupViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, string orderDir);
        public Task<ResponseOneDataViewModel<QcTransactionGroupDetailViewModel>> GetById(Int32 id);
        public Task<ResponseViewModel<SampleByQcProcessRelationViewModel>> ListSampleTest(Int32 QcProcessId, string search, Int32 TestParamId, Int32 RoomId, Int32 PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate);
        public Task<ResponseViewModel<SampleBatchQcProcessViewModel>> ListSampleBatchTest(Int32 QcProcessId, string search, Int32 RoomId, Int32 PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate);
        public Task<ResponseOneDataViewModel<QcTransactionGroup>> Insert(InsertQcTestBindingModel data);
        public Task<ResponseOneDataViewModel<QcTransactionGroup>> Edit(EditQcTestBindingModel data);
        public Task<ResponseOneDataViewModel<QcTransactionGroup>> StartTest(StartQcTestBindingModel data);
        public Task<ResponseViewModel<WorkflowSubmitBindingModel>> InsertApproval(InsertApprovalBindingModel data);
        public Task<ResponseOneDataViewModel<QcTransactionGroupDetailViewModel>> GetByIdV2(Int32 id);
        public Task<ResponseViewModel<SampleBatchQcProcessViewModel>> ListSampleBatchTestV2(Int32 QcProcessId, string search, Int32 RoomId, Int32 PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate);
        public Task<ResponseViewModel<ParameterThresholdRelationAltViewModel>> ListParameterThresholdAlt(string GradeRoomId, string testScenarioLabel, int? testGroupId);
        public Task<ResponseOneDataViewModel<QcTransactionGroupProcessRelViewModel>> GetTransactionGroupProcessById(int id);
    }
}
