namespace qcs_product.API.DataProviders
{
    using qcs_product.API.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITransactionTestingSamplingDataProvider
    {
        Task<TransactionTestingSampling> GetById(int id);
        Task<List<TransactionTestingSampling>> GetAll();
        Task<List<TransactionTestingSampling>> GetByTestingId(int TestingId);
        Task Create(TransactionTestingSampling entity);
        Task Update(TransactionTestingSampling entity);
        Task Delete(int id);
    }
}
