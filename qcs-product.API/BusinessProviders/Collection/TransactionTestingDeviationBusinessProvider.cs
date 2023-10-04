using qcs_product.API.DataProviders;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TransactionTestingDeviationBusinessProvider : ITransactionTestingDeviationBusinessProvider
    {
        private readonly ITransactionTestingDeviationDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public TransactionTestingDeviationBusinessProvider(ITransactionTestingDeviationDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

        }
        public async Task<ResponseViewModel<TransactionTestingDeviationViewModel>> GetAll(string filter, int sampleId, int productId, string batch, string testTypeName, int page, int limit)
        {
            ResponseViewModel<TransactionTestingDeviationViewModel> result = new ResponseViewModel<TransactionTestingDeviationViewModel>();

            BasePagination pagination = new BasePagination(page, limit);
            var data = await _dataProvider.GetAll(filter, sampleId, productId, batch, testTypeName, pagination.CalculateOffset(), limit);

            if (data.Any())
            {
                if (page == 0)
                    page = 1;
                if (limit == 0)
                    limit = int.MaxValue;

                int skip = (page - 1) * limit;
                int totalPages = (int)Math.Ceiling((double)data.Count / limit);

                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = data.Skip(skip).Take(limit).ToList();
                result.Meta = new MetaViewModel
                {
                    TotalItem = data.Count,
                    TotalPages = totalPages
                };
            }
            else
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }

            return result;
        }
    }
}
