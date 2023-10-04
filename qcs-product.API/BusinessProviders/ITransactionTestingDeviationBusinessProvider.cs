using qcs_product.API.ViewModels;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ITransactionTestingDeviationBusinessProvider
    {
        Task<ResponseViewModel<TransactionTestingDeviationViewModel>> GetAll(string filter, int sampleId, int productId, string batch, string testTypeName, int page, int limit);
    }
}
