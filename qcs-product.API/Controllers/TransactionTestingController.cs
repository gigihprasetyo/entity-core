using Microsoft.AspNetCore.Mvc;
using qcs_product.API.ViewModels;
using System.Threading.Tasks;
using System;
using qcs_product.API.BusinessProviders;
using qcs_product.API.DataProviders;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using qcs_product.Constants;
using qcs_product.API.BindingModels;
using qcs_product.API.Models;

namespace qcs_product.API.Controllers
{
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class TransactionTestingController : Controller
    {
        private readonly ITransactionTestingBusinessProvider _businessProvider;
        private readonly ILogger<TransactionTestingController> _logger;

        [ExcludeFromCodeCoverage]
        public TransactionTestingController(ITransactionTestingBusinessProvider businessProvider, ILogger<TransactionTestingController> logger)
        {
            _businessProvider = businessProvider ?? throw new ArgumentNullException(nameof(businessProvider));
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TransactionTestingViewModel>))]
        public async Task<IActionResult> List([FromQuery] string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status)
        {
            ResponseViewModel<TransactionTestingViewModel> response = new ResponseViewModel<TransactionTestingViewModel>();
            try
            {
                response = await _businessProvider.List(search, page, limit, status, startDate, endDate);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TransactionTestingDetailViewModel>))]
        public async Task<IActionResult> Detail([FromQuery] int transactionTestingId)
        {
            ResponseViewModel<TransactionTestingDetailViewModel> response = new ResponseViewModel<TransactionTestingDetailViewModel>();
            try
            {
                response = await _businessProvider.Detail(transactionTestingId);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);

        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TransactionTesting>))]
        public async Task<IActionResult> Insert([FromBody] InsertTransactionTestingBindingModel data)
        {
            ResponseViewModel<TransactionTesting> response = new ResponseViewModel<TransactionTesting>();
            try
            {
                response = await _businessProvider.Insert(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TransactionTesting>))]
        public async Task<IActionResult> Update([FromBody] UpdateTransactionTestingBindingModel data)
        {
            ResponseViewModel<TransactionTesting> response = new ResponseViewModel<TransactionTesting>();
            try
            {
                response = await _businessProvider.Update(data);
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
