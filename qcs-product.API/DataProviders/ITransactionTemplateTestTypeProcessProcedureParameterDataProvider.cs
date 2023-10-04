using qcs_product.API.Models;
using System.Collections.Generic;

namespace qcs_product.API.DataProviders
{
    public interface ITransactionTemplateTestTypeProcessProcedureParameterDataProvider
    {
        List<TransactionTemplateTestTypeProcessProcedureParameter> GetAllTransactionTmpltTestTypeProcessProcedureParameters();
        TransactionTemplateTestTypeProcessProcedureParameter GetTransactionTemplateTestTypeProcessProcedureParameterById(int id);
        void AddTransactionTemplateTestTypeProcessProcedureParameter(TransactionTemplateTestTypeProcessProcedureParameter parameter);
        void UpdateTransactionTemplateTestTypeProcessProcedureParameter(TransactionTemplateTestTypeProcessProcedureParameter parameter);
        void DeleteTransactionTemplateTestTypeProcessProcedureParameter(int id);
    }
}
