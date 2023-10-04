using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IQcSamplingTemplateBusinessProvider
    {
        Task<ResponseViewModel<QcSamplingTemplateViewModel>> GetAll(string filter, string status, int page, int limit);
        Task<ResponseViewModel<QcSamplingTemplate>> Insert(QcSamplingTemplate qcSamplingTemplate);
    }
}