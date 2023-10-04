using System.Threading.Tasks;
using qcs_product.API.ViewModels;

namespace qcs_product.API.BusinessProviders
{
    public interface ITransactionBatchLineBusinessProvider
    {
        public Task<ResponseViewModel<TransactionBatchLineViewModel>> List();
    }
}
