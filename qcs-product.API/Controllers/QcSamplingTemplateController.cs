using Microsoft.AspNetCore.Mvc;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System.Threading.Tasks;
using System;
using qcs_product.API.BusinessProviders;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using qcs_product.API.Models;

namespace qcs_product.API.Controllers
{
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class QcSamplingTemplateController : ControllerBase
    {
        private readonly IQcSamplingTemplateBusinessProvider _businessProvider;
        private readonly ILogger<QcSamplingController> _logger;

        [ExcludeFromCodeCoverage]
        public QcSamplingTemplateController(IQcSamplingTemplateBusinessProvider businessProvider, ILogger<QcSamplingController> logger)
        {
            _businessProvider = businessProvider ?? throw new ArgumentNullException(nameof(businessProvider));
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<QcRequestSamplingRelationViewModel>))]
        public async Task<IActionResult> GetAll([FromQuery] string filter, string status, int page, int limit)
        {
            ResponseViewModel<QcSamplingTemplateViewModel> response = new ResponseViewModel<QcSamplingTemplateViewModel>();
            try
            {
                response = await _businessProvider.GetAll(filter, status, page, limit);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<QcProcess>))]
        public async Task<IActionResult> Insert([FromBody] QcSamplingTemplate data)
        {
            ResponseViewModel<QcSamplingTemplate> response = new ResponseViewModel<QcSamplingTemplate>();
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
    }
}
