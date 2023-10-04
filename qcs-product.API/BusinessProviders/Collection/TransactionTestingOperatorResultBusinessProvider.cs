using qcs_product.API.DataProviders;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TransactionTestingOperatorResultBusinessProvider : ITransactionTestingOperatorResultBusinessProvider
    {
        private readonly ITransactionTestingOperatorResultDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public TransactionTestingOperatorResultBusinessProvider(ITransactionTestingOperatorResultDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

        }

        public async Task<ResponseViewModel<TransactionTestingOperatorResultView>> GetAll(string filter, string status, int testingId, int page, int limit)
        {
            ResponseViewModel<TransactionTestingOperatorResultView> result = new ResponseViewModel<TransactionTestingOperatorResultView>();

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
                if (status.Split(',').Length == 1)
                {
                    statusFilter.Add(Convert.ToInt16(status.Split(',')[0]));
                }
                else
                {
                    statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
                }

            }
            BasePagination pagination = new BasePagination(page, limit);
            var data = await _dataProvider.GetAll(filter, status, testingId, pagination.CalculateOffset(), limit);

            if (data.Any())
            {
                if (page == 0)
                    page = 1;
                if (limit == 0)
                    limit = int.MaxValue;

                int skip = (page - 1) * limit;
                int totalPages = (int)Math.Ceiling((double)data.Count / limit);

                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = data.Skip(skip).Take(limit).ToList();
                result.Meta = new MetaViewModel
                {
                    TotalItem = data.Count,
                    TotalPages = totalPages
                };
            }
            else
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }

            return result;
        }
    }
}
