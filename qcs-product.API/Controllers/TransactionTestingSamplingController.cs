using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using qcs_product.API.BusinessProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.Controllers
{
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class TransactionTestingSamplingController : Controller
    {
        private readonly ITransactionTestingSamplingBusinessProvider _businessProvider;
        private readonly ILogger<TransactionTestingController> _logger;

        public TransactionTestingSamplingController(ITransactionTestingSamplingBusinessProvider businessProvider, ILogger<TransactionTestingController> logger)
        {
            _businessProvider = businessProvider ?? throw new ArgumentNullException(nameof(businessProvider)); ;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionTestingSampling>> GetById(int id)
        {
            var entity = await _businessProvider.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            return entity;
        }

        [HttpGet("{TestingId}")]
        public async Task<ResponseViewModel<TransactionTestingSampling>> GetByTestingId(int TestingId)
        {
            var entities = await _businessProvider.GetByTestingIdAsync(TestingId);
            return entities;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionTestingSampling>>> GetAll()
        {
            var entities = await _businessProvider.GetAll();
            return entities;
        }

        [HttpPost]
        public async Task<ActionResult> Create(TransactionTestingSampling entity)
        {
            // Perform any additional business logic or validation before calling the business provider
            await _businessProvider.Create(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, TransactionTestingSampling entity)
        {
            // Perform any additional business logic or validation before calling the business provider
            if (id != entity.Id)
            {
                return BadRequest();
            }
            await _businessProvider.Update(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Perform any additional business logic or validation before calling the business provider
            await _businessProvider.Delete(id);
            return NoContent();
        }
    }

}
