using Microsoft.Extensions.Logging;
using qcs_product.API.DataProviders;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace qcs_product.API.BusinessProviders.Collection
{

    public class TransactionBatchLineBusinessProvider : ITransactionBatchLineBusinessProvider
    {
        private readonly ITransactionBatchLineDataProvider _transactionBatchLineDataProvider;
        private readonly ILogger<TransactionBatchLineBusinessProvider> _logger;

        public TransactionBatchLineBusinessProvider(ITransactionBatchLineDataProvider transactionBatchLineDataProvider, ILogger<TransactionBatchLineBusinessProvider> logger)
        {
            _transactionBatchLineDataProvider = transactionBatchLineDataProvider;
            _logger = logger;
        }

        public async Task<ResponseViewModel<TransactionBatchLineViewModel>> List()
        {
            ResponseViewModel<TransactionBatchLineViewModel> result = new ResponseViewModel<TransactionBatchLineViewModel>();
            List<TransactionBatchLineViewModel> getData = await _transactionBatchLineDataProvider.List();

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = getData;
            return result;
        }
    }
}