using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.BusinessProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace qcs_product.API.Controllers
{
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class TamplateOperatorTestingController : ControllerBase
    {
        private readonly ITemplateOperatorTestingBusinessProvider _businessProvider;
        private readonly ILogger<QcSamplingController> _logger;

        [ExcludeFromCodeCoverage]
        public TamplateOperatorTestingController(ITemplateOperatorTestingBusinessProvider businessProvider, ILogger<QcSamplingController> logger)
        {
            _businessProvider = businessProvider;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<QcRequestTemplateOperatorViewModel>))]
        public async Task<IActionResult> GetAll([FromQuery] string filter, string status, DateTime? startDate, DateTime? endDate, string methodCode, int page, int limit)
        {
            ResponseViewModel<QcSamplingTemplateViewModel> response = new ResponseViewModel<QcSamplingTemplateViewModel>();
            try
            {
                response = await _businessProvider.GetAll(filter, status, startDate, endDate,methodCode, page, limit);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TemplateOperatorTesting>))]
        public async Task<IActionResult> Insert([FromBody] TemplateOperatorTesting data)
        {
            ResponseViewModel<TemplateOperatorTesting> response = new ResponseViewModel<TemplateOperatorTesting>();
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

        [HttpGet(template: "DetailTemplateTestingOperator/{templateTestingOperatorId}")]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<QcRequestTemplateOperatorViewModel>))]
        public async Task<IActionResult> DetailTemplateTestingOperator(int templateTestingOperatorId)
        {
            ResponseOneDataViewModel<QcRequestTemplateOperatorViewModel> response = new ResponseOneDataViewModel<QcRequestTemplateOperatorViewModel>();
            try
            {
                response = await _businessProvider.DetailTemplateTestingOperator(templateTestingOperatorId);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TemplateOperatorTesting>))]
        public async Task<IActionResult> Edit([FromBody] TemplateOperatorTesting data)
        {
            ResponseViewModel<TemplateOperatorTesting> response = new ResponseViewModel<TemplateOperatorTesting>();
            try
            {
                response = await _businessProvider.Edit(data);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "{Message}", ex.Message);
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }
    }
}
