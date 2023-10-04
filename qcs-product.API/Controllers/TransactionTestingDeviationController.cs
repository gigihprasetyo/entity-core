using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using qcs_product.API.BusinessProviders;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace qcs_product.API.Controllers
{
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class TransactionTestingDeviationController : ControllerBase
    {
        private readonly ITransactionTestingDeviationBusinessProvider _businessProvider;
        private readonly ILogger<QcSamplingController> _logger;

        [ExcludeFromCodeCoverage]
        public TransactionTestingDeviationController(ITransactionTestingDeviationBusinessProvider businessProvider, ILogger<QcSamplingController> logger)
        {
            _businessProvider = businessProvider;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TransactionTestingDeviationViewModel>))]
        public async Task<IActionResult> GetAll([FromQuery] string filter, int sampleId, int productId, string batch, string testTypeName, int page, int limit)
        {
            ResponseViewModel<TransactionTestingDeviationViewModel> response = new ResponseViewModel<TransactionTestingDeviationViewModel>();
            try
            {
                response = await _businessProvider.GetAll(filter,sampleId , productId, batch, testTypeName, page, limit);

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
