using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface ITransactionBatchDataProvider
    {
        public Task<TransactionBatchViewModel> GetById(int id);
        public Task<TransactionBatchViewModel> GetByRequestId(int requestId);
        public Task<List<TransactionBatchViewModel>> GetByRequestIds(List<int> requestIds);
    }
}
