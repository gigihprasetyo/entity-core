using qcs_product.API.DataProviders;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using qcs_product.Constants;
using qcs_product.API.Models;
using qcs_product.API.BindingModels;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TransactionTestingBusinessProvider : ITransactionTestingBusinessProvider
    {
        private readonly ITransactionTestingDataProvider _transactionTestingDataProvider;

        public TransactionTestingBusinessProvider(ITransactionTestingDataProvider transactionTestingDataProvider)
        {
            _transactionTestingDataProvider = transactionTestingDataProvider;
        }

        public async Task<ResponseViewModel<TransactionTestingViewModel>> List(string filter, int page, int limit, string status, DateTime? startDate, DateTime? endDate)
        {
            ResponseViewModel<TransactionTestingViewModel> result = new ResponseViewModel<TransactionTestingViewModel>();

            var statusFilter = new List<int>();
            if (status == null)
            {

                statusFilter.Add(ApplicationConstant.STATUS_REJECT);
                statusFilter.Add(ApplicationConstant.STATUS_CANCEL);
                statusFilter.Add(ApplicationConstant.STATUS_DRAFT);
                statusFilter.Add(ApplicationConstant.STATUS_SUBMIT);
                statusFilter.Add(ApplicationConstant.STATUS_APPROVED);
                statusFilter.Add(ApplicationConstant.STATUS_IN_REVIEW_KABAG);
                statusFilter.Add(ApplicationConstant.STATUS_IN_REVIEW_KASIE);
                statusFilter.Add(ApplicationConstant.STATUS_IN_REVIEW_QA);

            }
            else
            {
                // filter status from param status is string
                statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            BasePagination pagination = new BasePagination(page, limit);

            var getData = await _transactionTestingDataProvider.GetByFilter(filter, pagination.CalculateOffset(), limit, statusFilter, startDate, endDate);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = getData;
            return result;
        }

        public async Task<ResponseViewModel<TransactionTesting>> Insert(InsertTransactionTestingBindingModel insertTransactionTesting)
        {
            ResponseViewModel<TransactionTesting> result = new ResponseViewModel<TransactionTesting>();

            var transactionTesting = await _transactionTestingDataProvider.Insert(insertTransactionTesting);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = new List<TransactionTesting> { transactionTesting };

            return result;
        }

        public async Task<ResponseViewModel<TransactionTestingDetailViewModel>> Detail(int id)
        {
            ResponseViewModel<TransactionTestingDetailViewModel> result = new ResponseViewModel<TransactionTestingDetailViewModel>();

            var getData = await _transactionTestingDataProvider.Detail(id);

            if (getData == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = new List<TransactionTestingDetailViewModel>
            {
                getData
            };
            return result;
        }

        public async Task<ResponseViewModel<TransactionTesting>> Update(UpdateTransactionTestingBindingModel update)
        {
            ResponseViewModel<TransactionTesting> result = new ResponseViewModel<TransactionTesting>();

            var transactionTesting = await _transactionTestingDataProvider.Update(update);

            if(transactionTesting == null)
            {
                result.StatusCode = 404;
                result.Message = "Data Not Found";
                result.Data = null;

                return result;
            }
            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = new List<TransactionTesting> { transactionTesting };

            return result;
        }
    }
}
