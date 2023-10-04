using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface ITransactionBatchLineDataProvider
    {
        public Task<List<TransactionBatchLineViewModel>> List();
    }
}
