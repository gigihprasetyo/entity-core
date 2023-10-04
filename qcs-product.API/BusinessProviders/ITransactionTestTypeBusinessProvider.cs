using production_execution_system.API.ViewModels;
using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ITransactionTestTypeBusinessProvider
    {
        public Task<ResponseOneDataViewModel<TransactionTestingViewModel>> GetTestingWProcedure(int testingId);
        public Task<ResponseOneDataViewModel<TransactionTestingProcedure>> UpdateParameterValue(InsertTransactionTestingProcedureBindingModel data);
        public Task<ResponseOneDataViewModel<InsertParameterAttachmentViewModel>> InsertParameterAttachment(InsertParameterAttachmentBindingModel data);
        public Task<ResponseOneDataViewModel<ListParameterNoteViewModel>> InsertParameterNote(InsertParameterNoteBindingModel data);
        public Task<ResponseViewModel<TestingProcedureParameterViewModel>> InsertMultipleDeviation(InsertExceptionBindingModel data);
    }
}
