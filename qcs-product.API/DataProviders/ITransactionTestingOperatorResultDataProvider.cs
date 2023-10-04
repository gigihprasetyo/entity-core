using qcs_product.API.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace qcs_product.API.DataProviders
{
    public interface ITransactionTestingOperatorResultDataProvider
    {
        Task<List<TransactionTestingOperatorResultView>> GetAll(string filter, string status, int testingId, int page, int limit);

    }
}
