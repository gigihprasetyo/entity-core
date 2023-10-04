using Google.Type;
using qcs_product.API.DataProviders;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class DeviationBusinessProvider : IDeviationBusinessProvider
    {
        private readonly IDeviationDataProvider _dataProvider;
        public DeviationBusinessProvider(IDeviationDataProvider dataProvider) 
        {
            _dataProvider = dataProvider;
        }

        public async Task<ResponseViewModel<ListDeviationViewModel>> ListDeviation(string search, int page, int limit)
        {
            var result = new ResponseViewModel<ListDeviationViewModel>();

            BasePagination pagination = new BasePagination(page, limit);

            var data = await _dataProvider.ListDeviation(search, pagination.CalculateOffset(), limit);

            if (!data.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = data;
            return result;
        }

        public async Task<ResponseOneDataViewModel<DetailDeviationViewModel>> DetailDeviation(int sampleId)
        {
            var result = new ResponseOneDataViewModel<DetailDeviationViewModel>();

            var data = await _dataProvider.DetailDeviation(sampleId);

            if (data == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = data;
            return result;
        }
    }
}
