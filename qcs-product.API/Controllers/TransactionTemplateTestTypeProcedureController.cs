using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.BusinessProviders;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.Controllers
{

    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class TransactionTemplateTestTypeProcedureController : ControllerBase
    {
        private readonly ITransactionTemplateTestTypeProcedureBusinessProvider _businessProvider;
        private readonly ILogger<QcSamplingController> _logger;
        public TransactionTemplateTestTypeProcedureController(ITransactionTemplateTestTypeProcedureBusinessProvider businessProvider)
        {
            _businessProvider = businessProvider;

        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionTemplateTestTypeProcessProcedure>>> GetAllTransactionTemplateTestTypeProcedures()
        {
            var procedures = await Task.Run(() => _businessProvider.GetAllTransactionTemplateTestTypeProcedures());
       
            
            
            return Ok(procedures);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionTemplateTestTypeProcessProcedure>> GetTransactionTemplateTestTypeProcedureById(int id)
        {
            var procedure = await Task.Run(() => _businessProvider.GetTransactionTemplateTestTypeProcedureById(id));
            if (procedure == null)
            {
                return NotFound();
            }
            return Ok(procedure);
        }

        [HttpGet("{id_trx_template_testing}")]
        public async Task<ActionResult<List<TransactionTemplateTestTypeProcessProcedure>>> GetTransactionTemplateTestTypeProcedureByIdTrxTesting(int id_trx_template_testing)
        {
            var procedure = await Task.Run(() => _businessProvider.GetTransactionTemplateTestTypeProcedureByIdTrxTesting(id_trx_template_testing));
            if (procedure == null)
            {
                return NotFound();
            }
            return Ok(procedure);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransactionTemplateTestTypeProcedure(TransactionTemplateTestTypeProcessProcedure procedure )
        {
            await Task.Run(() => _businessProvider.AddTransactionTemplateTestTypeProcedure(procedure));
            return CreatedAtAction(nameof(GetTransactionTemplateTestTypeProcedureById), new { id = procedure.Id }, procedure);
        }

        [HttpPut("{id_templatetest_type_procedure}")]
        public async Task<IActionResult> UpdateTransactionTemplateTestTypeProcedure(int id_templatetest_type_procedure, TransactionTemplateTestTypeProcessProcedure procedure)
        {
            if (id_templatetest_type_procedure != procedure.Id)
            {
                return BadRequest();
            }
            await Task.Run(() => _businessProvider.UpdateTransactionTemplateTestTypeProcedure(id_templatetest_type_procedure, procedure));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionTemplateTestTypeProcedure(int id)
        {
            await Task.Run(() => _businessProvider.DeleteTransactionTemplateTestTypeProcessProcedure(id));
            return NoContent();
        }
    }
}
