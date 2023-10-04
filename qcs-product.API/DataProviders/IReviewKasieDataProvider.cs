using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IReviewKasieDataProvider
    {
        public Task<ResponseViewModel<ReviewKasieTemplateQCListViewModel>> ListReviewTemplate(int posId, string filter, List<int> statusFilter, DateTime? startDate, DateTime? endDate, int v, int limit);
        public Task<TransactionTestingPersonnel> CheckInCheckOut(UpdateCheckInCheckOutPersonnel data);
        public List<TransactionTestingAttachment> DeleteAttachment(List<int> listId);
        public Task<List<TransactionTestingAttachment>> GetAttachmentByTemplateTestingId(int templateId);
        public Task<List<TransactionTestingNote>> GetNoteByTemplateTestingId(int templateId);
        public Task<List<TransactionTestingPersonnel>> GetPersonnelByTemplateTestingId(int templateId, string filter);
        public Task<List<TransactionTemplateTestTypeProcess>> GetProcessByTemplateTestingId(int templateId);
        public Task<List<TransactionTestingAttachment>> UpdateAttachment(List<TransactionTestingAttachment> data);
        public Task<TransactionTestingNote> UpdateNote(TransactionTestingNote data);
        public Task<List<TransactionTestingPersonnel>> UpdatePersonnel(List<TransactionTestingPersonnel> data);

    }
}
