using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TemplateOperatorTestingBusinessProvider : ITemplateOperatorTestingBusinessProvider
    {
        private readonly ITemplateOperatorTestingDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public TemplateOperatorTestingBusinessProvider(ITemplateOperatorTestingDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

        }

        public async Task<ResponseViewModel<QcSamplingTemplateViewModel>> GetAll(string filter, string status, DateTime? startDate, DateTime? endDate, string methodCode, int page, int limit)
        {
            ResponseViewModel<QcSamplingTemplateViewModel> result = new ResponseViewModel<QcSamplingTemplateViewModel>();

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
                if(status.Split(',').Length == 1)
                {
                    statusFilter.Add(Convert.ToInt16(status.Split(',')[0]));
                }
                else
                {
                    statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
                }
                
            }
            BasePagination pagination = new BasePagination(page, limit);
            var data = await _dataProvider.GetAll(filter, statusFilter, startDate, endDate, methodCode, pagination.CalculateOffset(), limit);

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

        public async Task<ResponseViewModel<TemplateOperatorTesting>> Insert(TemplateOperatorTesting templateOperatorTesting)
        {
            ResponseViewModel<TemplateOperatorTesting> result = new ResponseViewModel<TemplateOperatorTesting>();

            templateOperatorTesting.CreatedAt = DateTime.Now;
            templateOperatorTesting.CreatedBy = "admin";
            templateOperatorTesting.UpdatedAt = DateTime.Now;
            templateOperatorTesting.UpdatedBy = "admin";
            templateOperatorTesting.RowStatus = "0";
            var template = await _dataProvider.Insert(templateOperatorTesting);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = new List<TemplateOperatorTesting> { template };

            return result;
        }

        public async Task<ResponseOneDataViewModel<QcRequestTemplateOperatorViewModel>> DetailTemplateTestingOperator(int templateTestingOperatorId)
        {
            TemplateOperatorTesting data = new TemplateOperatorTesting();
            data = await _dataProvider.DetailTemplateTestingOperator(templateTestingOperatorId);

            ResponseOneDataViewModel<QcRequestTemplateOperatorViewModel> result = new ResponseOneDataViewModel<QcRequestTemplateOperatorViewModel>()
            {
                StatusCode = 200,
                Message = ApplicationConstant.OK_MESSAGE,
                Data = data
            };

            return result;
        }

        public async Task<ResponseViewModel<TemplateOperatorTesting>> Edit(TemplateOperatorTesting data)
        {
            ResponseViewModel<TemplateOperatorTesting> result = new ResponseViewModel<TemplateOperatorTesting>();

            var template = await _dataProvider.Edit(data);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = new List<TemplateOperatorTesting> { template };

            return result;
        }
    }
}
