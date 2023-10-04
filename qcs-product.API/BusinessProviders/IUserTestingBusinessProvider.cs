using qcs_product.API.BindingModels;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IUserTestingBusinessProvider
    {
        public Task<ResponseViewModel<UserTestingViewModel>> Insert(InsertUserTestingBindingModel data);
        public Task<ResponseOneDataViewModel<AUAMPersonalExtViewModel>> getUserExtAuam(string nik);
        public Task<ResponseOneDataViewModel<ResponseGetEmployeeBioHRViewModel>> getUserOldBioHR(string nik);
    }
}
