using qcs_product.API.ViewModels;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IDeviationBusinessProvider
    {
        public Task<ResponseViewModel<ListDeviationViewModel>> ListDeviation(string search, int page, int limit);
        public Task<ResponseOneDataViewModel<DetailDeviationViewModel>> DetailDeviation(int sampleId);
    }
}
