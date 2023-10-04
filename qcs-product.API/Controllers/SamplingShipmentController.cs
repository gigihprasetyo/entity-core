using Microsoft.AspNetCore.Mvc;
using qcs_product.API.BindingModels;
using qcs_product.API.BusinessProviders;
using qcs_product.API.ViewModels;
using qcs_product.API.Models;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace qcs_product.API.Controllers
{
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class SamplingShipmentController : ControllerBase
    {
        private readonly ISamplingShipmentBusinessProvider _businessProvider;
        private readonly ILogger<SamplingShipmentController> _logger;

        [ExcludeFromCodeCoverage]
        public SamplingShipmentController(ISamplingShipmentBusinessProvider businessProvider, ILogger<SamplingShipmentController> logger)
        {
            _businessProvider = businessProvider ?? throw new ArgumentNullException(nameof(businessProvider));
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<QcSamplingShipmentRelationViewModel>))]
        public async Task<IActionResult> List([FromQuery] string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int fromOrgId, int toOrgId, int qcSamplingId)
        {
            ResponseViewModel<QcSamplingShipmentRelationViewModel> response = new ResponseViewModel<QcSamplingShipmentRelationViewModel>();
            try
            {
                response = await _businessProvider.List(search, limit, page, startDate, endDate, status, fromOrgId, toOrgId, qcSamplingId);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);

        }

        [HttpGet(template: "{Id}")]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<QcSamplingShipmentRelationViewModel>))]
        public async Task<IActionResult> GetDetailById(Int32 Id)
        {
            ResponseOneDataViewModel<QcSamplingShipmentRelationViewModel> response = new ResponseOneDataViewModel<QcSamplingShipmentRelationViewModel>();
            try
            {
                response = await _businessProvider.GetById(Id);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.InnerException.ToString();
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<QcShipmentLateViewModel>))]
        public async Task<IActionResult> ListTransfer([FromQuery] string search, int limit, int page, DateTime? startDate, DateTime? endDate, int status)
        {
            ResponseViewModel<QcSamplingTransferViewModel> response = new ResponseViewModel<QcSamplingTransferViewModel>();
            try
            {
                response = await _businessProvider.ListTransfer(search, limit, page, startDate, endDate, status);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet(template: "{sampleId}")]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<QcSamplingTransferViewModel>))]
        public async Task<IActionResult> GetTransferDetail(Int32 sampleId)
        {
            ResponseOneDataViewModel<QcSamplingTransferViewModel> response = new ResponseOneDataViewModel<QcSamplingTransferViewModel>();
            try
            {
                response = await _businessProvider.GetTransferDetail(sampleId);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.InnerException.ToString();
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<QcSamplingShipmentRelationViewModelV2>))]
        public async Task<IActionResult> ListByBatch([FromQuery] string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int fromOrgId, int toOrgId)
        {
            ResponseViewModel<QcSamplingShipmentRelationViewModelV2> response = new ResponseViewModel<QcSamplingShipmentRelationViewModelV2>();
            try
            {
                response = await _businessProvider.ListByBatch(search, limit, page, startDate, endDate, status, fromOrgId, toOrgId);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);

        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<QcSamplingShipment>))]
        public async Task<IActionResult> Send([FromBody] InsertSamplingShipmentBindingModel data)
        {
            ResponseOneDataViewModel<QcSamplingShipment> response = new ResponseOneDataViewModel<QcSamplingShipment>();
            try
            {
                response = await _businessProvider.InsertSending(data);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "{Message}", ex.Message);
                response.StatusCode = 500;
                response.Message = ex.Message;
                if (ex.InnerException != null)
                {
                    response.Message = ex.InnerException.Message;
                }
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<QcSamplingShipment>))]
        public async Task<IActionResult> Receive([FromBody] InsertSamplingShipmentBindingModel data)
        {
            ResponseOneDataViewModel<QcSamplingShipment> response = new ResponseOneDataViewModel<QcSamplingShipment>();
            try
            {
                response = await _businessProvider.InsertReceiving(data);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "{Message}", ex.Message);
                response.StatusCode = 500;
                response.Message = ex.Message;
                if (ex.InnerException != null)
                {
                    response.Message = ex.InnerException.Message;
                }
            }
            return StatusCode(response.StatusCode, response);
        }

    }
}
