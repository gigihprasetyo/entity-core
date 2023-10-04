using System.Collections.Generic;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;

namespace qcs_product.API.BusinessProviders
{
    public interface IAUAMServiceBusinessProviders
    {
        Task<AUAMPersonalViewModel> GetPersonalDetailByNik(string nik);
        Task<AUAMPersonalExtViewModel> GetPersonalExtDetailByNik(string ExtNik);
        public Task<List<AUAMPersonalViewModel>> ListPersonalByRole(string appCode, string roleCode);
    }
}