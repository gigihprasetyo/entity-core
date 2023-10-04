using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ITemplateOperatorTestingBusinessProvider 
    {
        Task<ResponseViewModel<QcSamplingTemplateViewModel>> GetAll(string filter, string status, DateTime? startDate, DateTime? endDate,string methodCode, int page, int limit);
        Task<ResponseViewModel<TemplateOperatorTesting>> Insert(TemplateOperatorTesting templateOperatorTesting);
        Task<ResponseOneDataViewModel<QcRequestTemplateOperatorViewModel>> DetailTemplateTestingOperator(int templateTestingOperatorId);
        public Task<ResponseViewModel<TemplateOperatorTesting>> Edit(TemplateOperatorTesting data);
    }
}
