using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface ITemplateOperatorTestingDataProvider
    {
        Task<List<QcSamplingTemplateViewModel>> GetAll(string filter, List<int> status, DateTime? startDate, DateTime? endDate,string methodCode, int page, int limit);
        Task<TemplateOperatorTesting> Insert(TemplateOperatorTesting templateOperatorTesting);
        Task<TemplateOperatorTesting> DetailTemplateTestingOperator(int templateTestingOperatorId);
        Task<TemplateOperatorTesting> Edit(TemplateOperatorTesting data);
    }
}
