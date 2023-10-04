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
    public class QcRequestController : ControllerBase
    {
        private readonly IQcRequestBusinessProvider _businessProvider;
        private readonly ILogger<QcRequestController> _logger;

        [ExcludeFromCodeCoverage]
        public QcRequestController(IQcRequestBusinessProvider businessProvider, ILogger<QcRequestController> logger)
        {
            _businessProvider = businessProvider ?? throw new ArgumentNullException(nameof(businessProvider));
            _logger = logger;

        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<RequestQcsRelationViewModel>))]
        public async Task<IActionResult> List([FromQuery] string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int orgId, int TypeRequestId)
        {
            ResponseViewModel<RequestQcsRelationViewModel> response = new ResponseViewModel<RequestQcsRelationViewModel>();
            try
            {
                response = await _businessProvider.List(search, limit, page, startDate, endDate, status, orgId, TypeRequestId);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<RequestQcsListViewModel>))]
        public async Task<IActionResult> ListShort([FromQuery] string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int orgId, int TypeRequestId)
        {
            ResponseViewModel<RequestQcsListViewModel> response = new ResponseViewModel<RequestQcsListViewModel>();
            try
            {
                response = await _businessProvider.ListShort(search, limit, page, startDate, endDate, status, orgId, TypeRequestId);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet(template: "{QcRequestId}")]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<RequestQcsRelationViewModel>))]
        public async Task<IActionResult> GetById(int QcRequestId)
        {
            ResponseViewModel<RequestQcsRelationViewModel> response = new ResponseViewModel<RequestQcsRelationViewModel>();
            try
            {
                response = await _businessProvider.GetRequestQcById(QcRequestId);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<RequestQcs>))]
        public async Task<IActionResult> Insert([FromBody] InsertRequestQcsBindingModel data)
        {
            ResponseViewModel<RequestQcs> response = new ResponseViewModel<RequestQcs>();
            try
            {
                response = await _businessProvider.Insert(data);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message, ex);
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
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<RequestQcs>))]
        public async Task<IActionResult> Edit([FromBody] EditRequestQcsBindingModel data)
        {
            ResponseViewModel<RequestQcs> response = new ResponseViewModel<RequestQcs>();
            try
            {
                response = await _businessProvider.Edit(data);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message, ex);
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

    }
}
