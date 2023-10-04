using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IOperatorTestingBusinessProvider
    {
        public Task<ResponseOneDataViewModel<GeneralOperatorTestingInfoViewModel>> InfoGeneralByTestingId(int testingId);
        public Task<TransactionTesting> SetEndDate(int testingId);
        public Task<TransactionTesting> SetStartDate(int testingId);
        public Task<ResponseOneDataViewModel<InsertTestingPQViewModel>> InsertUserPQ(InsertTestingPQBindingModel data);
        public Task<ResponseViewModel<TransactionTestingNote>> InsertTestingNote(InsertTestingNoteBindingModel data);
        public Task<ResponseOneDataViewModel<InsertTestingAttachmentViewModel>> InsertTestingAttachment(InsertTestingAttachmentBindingModel data);
        public Task<ResponseOneDataViewModel<UserPresenceViewModel>> CheckInByPin(CheckInOutBindingModel data);
        public Task<ResponseOneDataViewModel<UserPresenceViewModel>> CheckOutByPin(CheckInOutBindingModel data);
    }
}
