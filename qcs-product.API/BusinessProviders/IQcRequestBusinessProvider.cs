using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IQcRequestBusinessProvider
    {
        public Task<ResponseViewModel<RequestQcs>> Insert(InsertRequestQcsBindingModel data);
        public Task<ResponseViewModel<RequestQcsRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int orgId, int TypeRequestId);
        public Task<ResponseViewModel<RequestQcsListViewModel>> ListShort(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int orgId, int TypeRequestId);
        public Task<ResponseOneDataViewModel<RequestQcs>> RejectRequestQc(RejectRequestQcBindingModel data);
        public Task<ResponseViewModel<RequestQcs>> Edit(EditRequestQcsBindingModel data);
        public Task<ResponseOneDataViewModel<RequestQcs>> EditNoBatch(EditNoBatchBindingModel data);
        public Task<ResponseViewModel<RequestQcs>> EditStatus(EditStatusRequestQcsBindingModel data);
        public Task<ResponseViewModel<RequestQcsRelationViewModel>> GetRequestQcById(Int32 requestQcId);
        public Task<ResponseViewModel<TestScenarioViewModel>> GetTestScenarioById(Int32 requestQcId);
        public Task<ResponseOneDataViewModel<RequestQcsViewModel>> GetRequestQcByBatch(string BatchNumber);
        // public Task<ResponseViewModel<RequestQcsBulk>> MassInsert(MassInsertRequestQcsBindingModel data);
    }
}
