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
    public class QcSamplingTemplateBusinessProvider : IQcSamplingTemplateBusinessProvider
    {
        private readonly IQcSamplingTemplateDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public QcSamplingTemplateBusinessProvider(IQcSamplingTemplateDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public async Task<ResponseViewModel<QcSamplingTemplateViewModel>> GetAll(string filter, string status, int page, int limit)
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
                statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }
            BasePagination pagination = new BasePagination(page, limit);
            var data = await _dataProvider.GetAll(filter, statusFilter, pagination.CalculateOffset(), limit);

            if (data.Any())
            {
                result.StatusCode = 200;
                result.Data = data;
                result.Message = ApplicationConstant.OK_MESSAGE;
            }
            else
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }

            return result;
        }
        public async Task<ResponseViewModel<QcSamplingTemplate>> Insert(QcSamplingTemplate qcSamplingTemplate)
        {
            ResponseViewModel<QcSamplingTemplate> result = new ResponseViewModel<QcSamplingTemplate>();

            var template = await _dataProvider.Insert(qcSamplingTemplate);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = new List<QcSamplingTemplate>{ template };

            return result;
        }
    }
}
