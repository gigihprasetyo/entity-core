using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IMonitoringDataProvider
    {
        public Task<List<MonitoringListViewModel>> ListShort(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, List<int> typeRequestId, int orgId, string nik, int? facilityId);
        public Task<List<MonitoringRelationViewModel>> GetRequestQcById(Int32 requestQcId, string? nik);
        public Task<List<MonitoringResultViewModel>> GetResult(int requestQcId);
        public Task<List<MonitoringListViewModel>> ListReportQa(string search, int limit, int page, string nik, int? facilityId);
        public Task<List<MonitoringListViewModel>> ListReportQa2(string search, int limit, int page, string nik, int? facilityId);
    }
}
