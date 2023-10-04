using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IDeviationDataProvider
    {
        public Task<List<ListDeviationViewModel>> ListDeviation(string search, int page, int limit);
        public Task<DetailDeviationViewModel> DetailDeviation(int sampleId);
    }
}
