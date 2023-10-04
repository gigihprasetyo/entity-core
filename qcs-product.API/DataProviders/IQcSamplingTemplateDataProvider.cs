using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IQcSamplingTemplateDataProvider
    {
        Task<List<QcSamplingTemplateViewModel>> GetAll(string filter, List<int> status, int page, int limit);
        Task<QcSamplingTemplate> Insert(QcSamplingTemplate qcSamplingTemplate);
    }
}