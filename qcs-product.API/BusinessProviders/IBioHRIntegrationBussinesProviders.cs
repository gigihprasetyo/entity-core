using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;

namespace qcs_product.API.BusinessProviders
{
    public interface IBioHRIntegrationBussinesProviders
    {
        public Task<string> Login();
        public Task<ResponseViewModel<ResponseGetOrganizationBioHRViewModel>> GetOrganization();
        public Task<ResponseGetEmployeeBioHRViewModel> GetEmployeeByPosId(string posId);
        public Task<IEnumerable<ResponseGetEmployeeBioHRViewModel>> GetListEmployeeByListPosId(List<int> lsPosId);
        public Task<ResponseOneDataViewModel<ResponseGetEmployeeNewBioHRViewModel>> GetNewUserBioHR(string newNik);
        public HttpClient HeaderAPIWorkflowService();
        public Task<ResponseGetOrganizationBioHRViewModel> GetOrganizationById(int OrgId);
        public Task<ResponseGetEmployeeBioHRViewModel> GetEmployeeByNik(string Nik);
        public Task<IEnumerable<ResponseGetEmployeeBioHRViewModel>> GetListEmployeeByListNik(List<string> lsNik);
        public Task<ResponseGetEmployeeNewBioHRViewModel> GetEmployeeByNewNik(string newNik);
        public Task<IEnumerable<ResponseGetEmployeeBioHRViewModel>> GetListEmployeeByListNewNik(List<string> lsNewNik);
        public Task<ResponseViewModel<ResponseGetNikByOrganizationIdAndPositionType>> GetNikByOrganizationIdandPositionType(string organizationId, string positionType);
        public Task<List<ResponseGetDelegationViewModel>> GetDelegationByPosId(string posId);
        public Task<List<string>> GetListNewNikDelegation(string posId);
    }
}
