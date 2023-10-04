using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface ITransactionTestingDeviationDataProvider
    {
        Task<List<TransactionTestingDeviationViewModel>> GetAll(string filter, int sampleId, int productId, string batch, string testTypeName, int page, int limit);
    }
}
