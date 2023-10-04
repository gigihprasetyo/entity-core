using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface ITransactionTestTypeMethodValidationParameterDataProvider
    {

        Task<List<TransactionTestTypeMethodValidationParameter>> GetAll();
        Task<List<TransactionMethodValidationParameterViewModel>> GetByIdTestingId(int id);
        Task<TransactionTestTypeMethodValidationParameter> GetById(int id);
        Task Add(TransactionTestTypeMethodValidationParameter entity);
        Task Update(TransactionTestTypeMethodValidationParameter entity);
        Task Delete(TransactionTestTypeMethodValidationParameter entity);

    }
}
