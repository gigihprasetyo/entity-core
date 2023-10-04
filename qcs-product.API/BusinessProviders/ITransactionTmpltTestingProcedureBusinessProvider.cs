using Microsoft.AspNetCore.Mvc;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ITransactionTmpltTestingProcedureBusinessProvider
    {
        Task<ResponseViewModel<TransactionTmpltTestingProcedure>> GetAllTransactionTmpltTestingProcedures();
        Task<ActionResult<TransactionTmpltTestingProcedure>> GetTransactionTmpltTestingProcedureById(int id);
        Task<ActionResult<TransactionTmpltTestingProcedure>> GetTransactionTmpltTestingProcedureByIdTrxTesting(int id);
        Task<IActionResult> AddTransactionTmpltTestingProcedure(TransactionTmpltTestingProcedure procedure);
        Task<IActionResult> UpdateTransactionTmpltTestingProcedure(int id, TransactionTmpltTestingProcedure procedure);
        Task<IActionResult> DeleteTransactionTmpltTestingProcedure(int id);

    }
}
