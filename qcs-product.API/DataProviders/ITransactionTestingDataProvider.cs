using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface ITransactionTestingDataProvider
    {
        Task<List<TransactionTestingViewModel>> GetByFilter(string filter, int page, int limit, List<int> status, DateTime? startDate, DateTime? endDate);
        Task<TransactionTesting> Insert(InsertTransactionTestingBindingModel transactionTesting);
        Task<TransactionTestingDetailViewModel> Detail(int id);
        Task<TransactionTesting> Update(UpdateTransactionTestingBindingModel transactionTesting);
    }
}
