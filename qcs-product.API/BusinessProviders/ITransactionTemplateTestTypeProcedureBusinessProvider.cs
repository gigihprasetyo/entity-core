using Microsoft.AspNetCore.Mvc;
using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ITransactionTemplateTestTypeProcedureBusinessProvider
    {
        Task<ResponseViewModel<TransactionTemplateTestTypeProcessProcedure>> GetAllTransactionTemplateTestTypeProcedures();
        ActionResult<TransactionTemplateTestTypeProcessProcedure> GetTransactionTemplateTestTypeProcedureById(int id);
        ResponseViewModel<TransactionTemplateTestTypeProcessProcedure> GetTransactionTemplateTestTypeProcedureByIdTrxTesting(int id);
        Task<IActionResult> AddTransactionTemplateTestTypeProcedure(TransactionTemplateTestTypeProcessProcedure procedure);
        Task<ResponseViewModel<TransactionTemplateTestTypeProcessProcedure>>  UpdateTransactionTemplateTestTypeProcedure(int id, TransactionTemplateTestTypeProcessProcedure procedure );
        Task<IActionResult> DeleteTransactionTemplateTestTypeProcessProcedure(int id);

    }
}
