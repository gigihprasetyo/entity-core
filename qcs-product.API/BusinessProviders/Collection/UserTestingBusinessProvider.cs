using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class UserTestingBusinessProvider : IUserTestingBusinessProvider
    {
        private readonly IUserTestingDataProvider _dataProvider;
        private readonly IBioHRIntegrationBussinesProviders _bioHRIntegrationBussinesProviders;
        private readonly IAUAMServiceBusinessProviders _auamServiceBusinessProviders;
        public UserTestingBusinessProvider(IUserTestingDataProvider dataProvider,
                                            IBioHRIntegrationBussinesProviders bioHRIntegrationBussinesProviders,
                                            IAUAMServiceBusinessProviders auamServiceBusinessProviders)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _bioHRIntegrationBussinesProviders = bioHRIntegrationBussinesProviders ?? throw new ArgumentNullException(nameof(bioHRIntegrationBussinesProviders));
            _auamServiceBusinessProviders = auamServiceBusinessProviders;
        }

        public async Task<ResponseViewModel<UserTestingViewModel>> Insert(InsertUserTestingBindingModel data)
        {
            UserTesting userNew = new UserTesting();
            userNew.Name = data.Nama;
            userNew.JenisKelamin = data.JenisKelamin;
           

            UserTesting insertedUser = await _dataProvider.Insert(userNew);
            UserTestingViewModel insertedUserNew = new UserTestingViewModel
            {
                Name = insertedUser.Name,
                JenisKelamin = insertedUser.JenisKelamin
            };
            List<UserTestingViewModel> insertedDataUser = new List<UserTestingViewModel>() { insertedUserNew };

            ResponseViewModel<UserTestingViewModel> result = new ResponseViewModel<UserTestingViewModel>
            {
                StatusCode = 200,
                Message = ApplicationConstant.OK_MESSAGE,
                Data = insertedDataUser
            };
            return result;
        }

        public async Task<ResponseOneDataViewModel<AUAMPersonalExtViewModel>> getUserExtAuam(string nik)
        {
            ResponseOneDataViewModel<AUAMPersonalExtViewModel> result = new ResponseOneDataViewModel<AUAMPersonalExtViewModel>();
            var personal = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(nik);
            if (personal == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = personal;
            }

            return result;
        }

        public async Task<ResponseOneDataViewModel<ResponseGetEmployeeBioHRViewModel>> getUserOldBioHR(string nik)
        {

            ResponseOneDataViewModel<ResponseGetEmployeeBioHRViewModel> result = new ResponseOneDataViewModel<ResponseGetEmployeeBioHRViewModel>();
            var personal = await _bioHRIntegrationBussinesProviders.GetEmployeeByNik(nik);
            if (personal == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = personal;
            }

            return result;
        }
    }
}
