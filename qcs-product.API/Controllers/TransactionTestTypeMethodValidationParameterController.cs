using Microsoft.AspNetCore.Mvc;
using qcs_product.API.BusinessProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.Controllers
{
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class TransactionTestTypeMethodValidationParameterController : Controller
    {
        private readonly ITransactionTestTypeMethodValidationParameterBusinessProvider _businessProvider;
       
        public TransactionTestTypeMethodValidationParameterController(ITransactionTestTypeMethodValidationParameterBusinessProvider businessProvider)
        {
            _businessProvider = businessProvider;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionTestTypeMethodValidationParameter>>> GetAll()
        {
            var entities = await _businessProvider.GetAll();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionTestTypeMethodValidationParameter>> GetById(int id)
        {
            var entity = await _businessProvider.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet("{TestingId}")]
        public async Task<ResponseViewModel<TransactionMethodValidationParameterViewModel>> GetByIdTestingId(int TestingId)
        {
            var entities = await _businessProvider.GetByIdTestingId(TestingId);
            return entities;
        }


        [HttpPost]
        public async Task<ActionResult> Add(TransactionTestTypeMethodValidationParameter entity)
        {
            await _businessProvider.Add(entity);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, TransactionTestTypeMethodValidationParameter entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }
            await _businessProvider.Update(entity);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _businessProvider.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            await _businessProvider.Delete(entity);
            return Ok();
        }
    }
}
