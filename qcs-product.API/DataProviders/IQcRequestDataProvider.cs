using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IQcRequestDataProvider
    {
        public Task<RequestQcs> Insert(RequestQcs data, List<TestTypeQcs> dataTestType);
        public Task<List<TestTypeQcs>> InsertProductTestTypeQcs(List<TestTypeQcs> data);
        public Task<List<RequestQcsRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int orgId, int TypeRequestId);
        public Task<List<RequestQcsListViewModel>> ListShort(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int orgId, int TypeRequestId);
        public Task<RequestQcs> EditNoBatch(RequestQcs data);
        public Task<RequestQcs> RejectRequestQc(RequestQcs data);
        public Task<RequestQcs> Edit(RequestQcs data, List<TestTypeQcs> dataTestType, bool isSubmit);
        public Task<RequestQcs> EditStatus(RequestQcs data);
        public Task<RequestQcs> GetById(Int32 id);
        public Task<List<TestScenarioViewModel>> GetTestScenarioById(Int32 id);
        public Task<Personal> GetPersonalById(Int32 Id);
        public Task<List<RequestQcsRelationViewModel>> GetRequestQcById(Int32 requestQcId);
        public Task<RequestQcsViewModel> GetRequestQcByBatchNumber(string BatchNumber);
        public Task<List<QcRequestSamplingGenerateViewModel>> getSamplingRequest(int RequestTypeId);
        public Task<QcSampling> generateSamplingQcAlt(Int32 TypeSamplingId, string RequestSamplingName, RequestQcs data, List<TestTypeQcs> dataTestType);
        public string generateCodeSample(int length);
        public Task<int> checkScenarioCode(int? TestScenarioId);
        public Task<ResponseViewModel<InsertEditDev>> UpdateDeviation(InsertEditDev data);
        public Task<RequestQcs> GetByBatchAndPhaseId(string batchNumber, int phaseId);
        public Task<bool> UpdateConclusion(InsertEditConclusion data);
        public Task<bool> GenerateUpdateConclusion(int requestId);
        public Task<List<string>> getRequestPurposeNames(int requestId);
        public Task<bool> UpdateReceiptDate(int id, bool isQA);
        public Task<bool> UpdateIsNoBatchEditable(int id, bool isNoBatchEditable);
        public Task<List<QcResult>> findRequestNotAllPass(int qcRequestId);
        public Task<List<VSamplePointTestParam>> _getSamplePointTestParameter(int roomId, int TestParameterId, string testScenarioLabel);
        public Task<List<VSamplePointTestParam>> getSamplePointTestParameter(int roomId, int TestParameterId, string testScenarioLabel, List<int> purposeId);
        public Task<List<VSamplePointTestParam>> GetSamplePointTestParamByToolsInRooms(List<int> roomIds, string testScenarioLabel, List<int> purposeIds);
        public Task<int> _getSamplePointTestParameterCount(int roomId, int TestParameterId, string testScenarioLabel);
        public Task<int> getSamplePointTestParameterCount(int roomId, int TestParameterId, string testScenarioLabel, List<int> purposeId);
        public Task<List<RequestQcs>> CheckNoBatchRequestEM(string noBatch, int? phaseId, string testScenarioLabel, int? requestId);
        public Task<List<VSamplePointTestParam>> GetSamplePointTestParamByToolsInRoomsExisting(List<int> roomIds, string testScenarioLabel, List<int> purposeIds);
    }
}
