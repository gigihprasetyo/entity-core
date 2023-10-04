using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public interface ITransactionTestTypeDataProvider
    {
        public Task<TransactionTestingProcedure> GetProcedureById(int procedureid);
        public Task<TransactionTestingViewModel> GetTestingById(int testingId);
        public Task<TransactionTestingProcedureParameter> GetParameterById(int parameterId);
        public Task<TransactionTestingProcedureParameter> UpdateParameter(TransactionTestingProcedureParameter data);
        public Task<TransactionTestingProcedure> UpdateProcedure(TransactionTestingProcedure data);
        public Task<TransactionTestingProcedureParameterAttachment> GetByMediaLink(string mediaLink);
        public Task<List<TransactionTestingProcedureParameterAttachment>> GetAttachmentByParameterId(int parameterId);
        public Task<List<TransactionHtrProcessProcedureParameterAttachment>> GetAttachmentHistoryByParameterId(int parameterId);
        public Task<TransactionTestingProcedureParameterAttachment> InsertProcedureParameterAttachment(TransactionTestingProcedureParameterAttachment data);
        public Task<TransactionHtrTestingProcedureParameter> InsertHistoryParameter(TransactionHtrTestingProcedureParameter data);
        public Task<TransactionHtrProcessProcedureParameterAttachment> InsertHistoryParameterAttachment(TransactionHtrProcessProcedureParameterAttachment data);
        public Task<TransactionTestingProcedureParameterNote> InsertProcedureParameterNote(TransactionTestingProcedureParameterNote data);
        public Task<List<TransactionTestingProcedureParameterAttachment>> DeleteNotInRange(int procedureParameterId, List<TransactionTestingProcedureParameterAttachment> data);
        public Task<TestingProcedureParameterViewModel> PatchExceptionParameter(int id, string exception);
        public Task<TransactionTestingProcedureParameterAttachment> InsertAttachmentException(string createdBy, string mediaLink, string filename, int procedureParameterId, string ext);
    }
}
