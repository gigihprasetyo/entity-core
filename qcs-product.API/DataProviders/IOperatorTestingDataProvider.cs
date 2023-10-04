using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IOperatorTestingDataProvider
    {
        public Task<List<TransactionTestingPersonnel>> DeleteNotInRangePersonel(int testingId, List<TransactionTestingPersonnel> latestPersonnelData);
        public Task<List<TransactionTestingAttachment>> GetAttachmentByTestingId(int testingId);
        public Task<List<TransactionHtrTestingPersonnel>> GetHtrPersonelByTestingId(int testingId);
        public Task<List<TransactionTestingNote>> GetNoteByTestingId(int testingId);
        public Task<TransactionTestingPersonnel> GetPersonelByNewNIK(int testingId, string newNik);
        public Task<List<TransactionTestingPersonnel>> GetPersonnelByTestingId(int testingId, string filter);
        public Task<TransactionTesting> GetTrxTestingById(int testingId);
        public Task<GeneralOperatorTestingInfoViewModel> InfoGeneralByTestingId(int testingId);
        public Task<TransactionHtrTestingPersonnel> InsertHtrPersonel(TransactionHtrTestingPersonnel personnelHistoryData);
        public Task<TransactionTestingNote> InsertNote(TransactionTestingNote transactionTestingNote);
        public Task<TransactionHtrTestingNote> InsertHtrNote(TransactionHtrTestingNote data);
        public Task<TransactionTestingPersonnel> InsertPersonel(TransactionTestingPersonnel productionProcessData);
        public Task<TransactionTesting> SetEndDate(int testingId);
        public Task<TransactionTesting> SetStartDate(int testingId);
        public Task<TransactionTestingPersonnel> UpdatePersonel(TransactionTestingPersonnel currentProcessPersonel);
        public Task<List<TransactionHtrTestingAttachment>> GetHtrAttachmentByTestingId(int testingId);
        public Task<TransactionTestingAttachment> InsertAttachment(TransactionTestingAttachment transactionTestingAttachment);
        public Task<TransactionTestingAttachment> GetAttachmentByMediaLink(string attachmentStorageName);
        public Task<List<TransactionTestingAttachment>> DeleteNotInRangeAttachment(int testingId, List<TransactionTestingAttachment> latestAttachmentData);
        public Task<TransactionHtrTestingAttachment> InsertHtrAttachment(TransactionHtrTestingAttachment attachmentHistoryData);
        public Task<bool> Authenticate(string pin, string newNIK);
        public Task<TransactionTestingPersonnel> GetCheckedInUserPresenceByUsername(string nik);
        public Task<TransactionHtrTestingPersonnel> InsertHtrPresence(TransactionHtrTestingPersonnel user);
        public Task<TransactionTestingPersonnel> CheckedInUserPresence(CheckInOutBindingModel data);
        public Task<TransactionTestingPersonnel> CheckedOutUserPresence(TransactionTestingPersonnel existingData);
    }
}
