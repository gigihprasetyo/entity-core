using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using qcs_product.API.BusinessProviders;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System.Threading.Tasks;
using System;

namespace qcs_product.API.Controllers
{
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class TransactionTestingOperatorResultController : ControllerBase
    {
        private readonly ITransactionTestingOperatorResultBusinessProvider _businessProvider;
        private readonly ILogger<QcSamplingController> _logger;

        public TransactionTestingOperatorResultController(ITransactionTestingOperatorResultBusinessProvider businessProvider)
        {
            _businessProvider = businessProvider;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TransactionTestingOperatorResultView>))]
        public async Task<IActionResult> GetAll([FromQuery] string filter, string status, int testingId, int page, int limit)
        {
            ResponseViewModel<TransactionTestingOperatorResultView> response = new ResponseViewModel<TransactionTestingOperatorResultView>();
            try
            {
                response = await _businessProvider.GetAll(filter, status, testingId, page, limit);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

    }
}
