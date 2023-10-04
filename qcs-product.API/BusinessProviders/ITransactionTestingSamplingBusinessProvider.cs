using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ITransactionTestingSamplingBusinessProvider
    {
        Task<TransactionTestingSampling> GetById(int id);
        Task<List<TransactionTestingSampling>> GetAll();
        Task<ResponseViewModel<TransactionTestingSampling>> GetByTestingIdAsync(int TestingId);
        Task Create(TransactionTestingSampling entity);
        Task Update(TransactionTestingSampling entity);
        Task Delete(int id);

    }
}
