using qcs_product.API.ViewModels;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Threading.Tasks;
using qcs_product.API.DataProviders;
using qcs_product.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using qcs_product.API.Models;
using qcs_product.API.BindingModels;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class ReviewKasieBusinessProvider : IReviewKasieBusinessProvider
    {
        private readonly IReviewKasieDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public ReviewKasieBusinessProvider(IReviewKasieDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public async Task<ResponseOneDataViewModel<TransactionTestingPersonnel>> CheckInCheckOut(UpdateCheckInCheckOutPersonnel data)
        {
            ResponseOneDataViewModel<TransactionTestingPersonnel> result = new ResponseOneDataViewModel<TransactionTestingPersonnel>();
            TransactionTestingPersonnel updatedData = new TransactionTestingPersonnel();

            updatedData = await _dataProvider.CheckInCheckOut(data);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = updatedData;

            return result;
        }

        public ResponseViewModel<TransactionTestingAttachment> DeleteAttachment(List<int> listId)
        {
            ResponseViewModel<TransactionTestingAttachment> result = new ResponseViewModel<TransactionTestingAttachment>();
            List<TransactionTestingAttachment> deletedData = new List<TransactionTestingAttachment>();

            deletedData = _dataProvider.DeleteAttachment(listId);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = deletedData;

            return result;
        }

        public async Task<ResponseOneDataViewModel<TransactionTemplateTestingInfoViewModel>> InfoByTestingTemplate(int templateId, string filter)
        {
            TransactionTemplateTestingInfoViewModel dataInfo = new TransactionTemplateTestingInfoViewModel();
            dataInfo.listPersonnel = await _dataProvider.GetPersonnelByTemplateTestingId(templateId, filter);
            dataInfo.listNote = await _dataProvider.GetNoteByTemplateTestingId(templateId);
            dataInfo.listAttachment = await _dataProvider.GetAttachmentByTemplateTestingId(templateId);
            dataInfo.listProcess = await _dataProvider.GetProcessByTemplateTestingId(templateId);

            ResponseOneDataViewModel<TransactionTemplateTestingInfoViewModel> result = new ResponseOneDataViewModel<TransactionTemplateTestingInfoViewModel>()
            {
                StatusCode = 200,
                Message = ApplicationConstant.OK_MESSAGE,
                Data = dataInfo
            };

            return result;
        }

        public async Task<ResponseViewModel<ReviewKasieTemplateQCListViewModel>> ListReviewTemplate(string positionId, string filter, string status, DateTime? startDate, DateTime? endDate, int page = 1, int limit = 10)
        {
            ResponseViewModel<ReviewKasieTemplateQCListViewModel> result = new ResponseViewModel<ReviewKasieTemplateQCListViewModel>();

            var posId = 0;
            if (!string.IsNullOrEmpty(positionId)) posId = Convert.ToInt32(positionId);

            var statusFilter = new List<int>();
            if (status == null)
            {

                statusFilter.Add(ApplicationConstant.STATUS_REJECT);
                statusFilter.Add(ApplicationConstant.STATUS_CANCEL);
                statusFilter.Add(ApplicationConstant.STATUS_DRAFT);
                statusFilter.Add(ApplicationConstant.STATUS_APPROVED);
                statusFilter.Add(ApplicationConstant.STATUS_IN_REVIEW_KASIE);

            }
            else
            {
                // filter status from param status is string
                statusFilter = status.Split(',').Select(x => int.Parse(x)).Reverse().ToList();
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate > endDate)
                {
                    result.StatusCode = 400;
                    result.Message = ApplicationConstant.END_DATE_LESS_THAN_BEGIN_DATE_ERROR_MESSAGE;

                    return result;
                }
            }

            BasePagination pagination = new BasePagination(page, limit);
            var data = await _dataProvider.ListReviewTemplate(posId, filter, statusFilter, startDate, endDate, pagination.CalculateOffset(), limit);

            return data;

        }

        public async Task<ResponseViewModel<TransactionTestingAttachment>> UpdateAttachment(List<TransactionTestingAttachment> data)
        {
            ResponseViewModel<TransactionTestingAttachment> result = new ResponseViewModel<TransactionTestingAttachment>();
            List<TransactionTestingAttachment> updatedData = new List<TransactionTestingAttachment>();

            updatedData = await _dataProvider.UpdateAttachment(data);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = updatedData;

            return result;
        }

        public async Task<ResponseOneDataViewModel<TransactionTestingNote>> UpdateNote(TransactionTestingNote data)
        {
            ResponseOneDataViewModel<TransactionTestingNote> result = new ResponseOneDataViewModel<TransactionTestingNote>();
            TransactionTestingNote updatedData = new TransactionTestingNote();

            updatedData = await _dataProvider.UpdateNote(data);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = updatedData;

            return result;
        }

        public async Task<ResponseViewModel<TransactionTestingPersonnel>> UpdatePersonnel(List<TransactionTestingPersonnel> data)
        {
            ResponseViewModel<TransactionTestingPersonnel> result = new ResponseViewModel<TransactionTestingPersonnel>();
            List<TransactionTestingPersonnel> updatedData = new List<TransactionTestingPersonnel>();

            updatedData = await _dataProvider.UpdatePersonnel(data);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = updatedData;

            return result;
        }
    }
}
