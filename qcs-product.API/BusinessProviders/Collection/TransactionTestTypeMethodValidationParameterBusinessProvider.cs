using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TransactionTestTypeMethodValidationParameterBusinessProvider : ITransactionTestTypeMethodValidationParameterBusinessProvider
    {
        private readonly ITransactionTestTypeMethodValidationParameterDataProvider _dataProvider;

        public TransactionTestTypeMethodValidationParameterBusinessProvider(ITransactionTestTypeMethodValidationParameterDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<List<TransactionTestTypeMethodValidationParameter>> GetAll()
        {
            return await _dataProvider.GetAll();
        }

        public async Task<TransactionTestTypeMethodValidationParameter> GetById(int id)
        {
            return await _dataProvider.GetById(id);
        }

        public async Task Add(TransactionTestTypeMethodValidationParameter entity)
        {
            await _dataProvider.Add(entity);
        }

        public async Task Update(TransactionTestTypeMethodValidationParameter entity)
        {
            await _dataProvider.Update(entity);
        }

        public async Task Delete(TransactionTestTypeMethodValidationParameter entity)
        {
            await _dataProvider.Delete(entity);
        }

        public async Task<ResponseViewModel<TransactionMethodValidationParameterViewModel>> GetByIdTestingId(int id)
        {
            ResponseViewModel<TransactionMethodValidationParameterViewModel> result = new ResponseViewModel<TransactionMethodValidationParameterViewModel>();
            var getData = await _dataProvider.GetByIdTestingId(id);
            if (getData == null || !getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = getData;
            return result;

           
        }
    }
}
