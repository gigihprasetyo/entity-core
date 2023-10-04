using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IMonitoringBusinessProvider
    {
        public Task<ResponseViewModel<MonitoringListViewModel>> ListShort(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, string typeRequestId, int orgId, string nik, int? facilityId);
        public Task<ResponseViewModel<MonitoringRelationViewModel>> GetRequestQcById(Int32 requestQcId, string? nik);
        //public Task<ResponseViewModel<WorkflowSubmitBindingModel>> InsertReject(InsertRejectToDoBindingModel data);
        public Task<ResponseViewModel<MonitoringResultViewModel>> GetResult(Int32 requestQcId);
        public Task<ResponseViewModel<WorkflowSubmitBindingModel>> InsertReject(InsertRejectToDoBindingModel data);
        public Task<ResponseViewModel<WorkflowSubmitBindingModel>> InsertApprove(InsertApproveToDoBindingModel data);
        public Task<ResponseViewModel<InsertEditDev>> UpdateDeviation(InsertEditDev data);
        public Task<ResponseViewModel<InsertEditDev>> UpdateConclusion(InsertEditConclusion data);
        public Task<ResponseViewModel<MonitoringListViewModel>> ListReportQa(string search, int limit, int page, string nik, int? facilityId);
        public Task<ResponseViewModel<MonitoringListViewModel>> ListReportQa2(string search, int limit, int page, string nik, int? facilityId);
    }
}
