using qcs_product.API.ViewModels;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ITransactionTestingOperatorResultBusinessProvider
    {
        Task<ResponseViewModel<TransactionTestingOperatorResultView>> GetAll(string filter, string status,int testingId, int page, int limit);
    }
}
