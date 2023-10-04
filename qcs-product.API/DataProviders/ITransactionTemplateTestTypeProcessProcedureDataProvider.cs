using qcs_product.API.Models;
using System.Collections.Generic;

namespace qcs_product.API.DataProviders
{
    public interface ITransactionTemplateTestTypeProcessProcedureDataProvider
    {
        List<TransactionTemplateTestTypeProcessProcedure> GetAllTransactionTemplateTestTypeProcessProcedures();
        TransactionTemplateTestTypeProcessProcedure GetTransactionTmpltTestTypeProcessProcedureById(int id);
        List<TransactionTemplateTestTypeProcessProcedure> GetTransactionTemplateTestTypeProcedureByIdTrxTesting(int id);
        void AddTransactionTemplateTestTypeProcessProcedure(TransactionTemplateTestTypeProcessProcedure procedure);
        void UpdateTransactionTemplateTestTypeProcessProcedure(int id, TransactionTemplateTestTypeProcessProcedure procedure);
        void DeleteTransactionTemplateTestTypeProcessProcedure(int id);

    }
}
