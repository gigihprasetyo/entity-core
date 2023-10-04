using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ITransactionTestingBusinessProvider
    {
        Task<ResponseViewModel<TransactionTestingViewModel>> List(string filter, int page, int limit, string status, DateTime? startDate, DateTime? endDate);
         Task<ResponseViewModel<TransactionTesting>> Insert(InsertTransactionTestingBindingModel qcSamplingTemplate);
        Task<ResponseViewModel<TransactionTestingDetailViewModel>> Detail(int id);
        Task<ResponseViewModel<TransactionTesting>> Update(UpdateTransactionTestingBindingModel update);
    }
}