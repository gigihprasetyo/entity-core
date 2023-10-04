using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IReviewKasieBusinessProvider
    {
        public Task<ResponseViewModel<ReviewKasieTemplateQCListViewModel>> ListReviewTemplate(string positionId, string filter, string status, DateTime? startDate, DateTime? endDate, int page = 1, int limit = 10);
        public Task<ResponseOneDataViewModel<TransactionTestingPersonnel>> CheckInCheckOut(UpdateCheckInCheckOutPersonnel data);
        public ResponseViewModel<TransactionTestingAttachment> DeleteAttachment(List<int> listId);
        public Task<ResponseOneDataViewModel<TransactionTemplateTestingInfoViewModel>> InfoByTestingTemplate(int templateId, string filter);
        public Task<ResponseViewModel<TransactionTestingAttachment>> UpdateAttachment(List<TransactionTestingAttachment> data);
        public Task<ResponseOneDataViewModel<TransactionTestingNote>> UpdateNote(TransactionTestingNote data);
        public Task<ResponseViewModel<TransactionTestingPersonnel>> UpdatePersonnel(List<TransactionTestingPersonnel> data);
    }
}
