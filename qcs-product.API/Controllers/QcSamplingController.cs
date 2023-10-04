using Microsoft.AspNetCore.Mvc;
using qcs_product.API.BindingModels;
using qcs_product.API.BusinessProviders;
using qcs_product.API.ViewModels;
using qcs_product.API.WorkflowModels;
using qcs_product.API.Models;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using qcs_product.Constants;

namespace qcs_product.API.Controllers
{

    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class QcSamplingController : ControllerBase
    {
        private readonly IQcSamplingBusinessProvider _businessProvider;
        private readonly ILogger<QcSamplingController> _logger;

        [ExcludeFromCodeCoverage]
        public QcSamplingController(IQcSamplingBusinessProvider businessProvider, ILogger<QcSamplingController> logger)
        {
            _businessProvider = businessProvider ?? throw new ArgumentNullException(nameof(businessProvider));
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<QcRequestSamplingRelationViewModel>))]
        public async Task<IActionResult> List([FromQuery] string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int? orgId, int TypeRequestId, int SamplingTypeId)
        {
            ResponseViewModel<QcRequestSamplingRelationViewModel> response = new ResponseViewModel<QcRequestSamplingRelationViewModel>();
            try
            {
                response = await _businessProvider.List(search, limit, page, startDate, endDate, status, orgId, TypeRequestId, SamplingTypeId);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);

        }

        [HttpGet(template: "{SamplingId}")]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<QcSamplingRelationViewModel>))]
        public async Task<IActionResult> GetDetailyId(int SamplingId, [FromQuery] string sort)
        {
            ResponseViewModel<QcSamplingRelationViewModel> response = new ResponseViewModel<QcSamplingRelationViewModel>();
            try
            {
                response = await _businessProvider.GetDetaiById(SamplingId, sort);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "{Message}", ex.Message);
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<QcSampling>))]
        public async Task<IActionResult> Edit([FromBody] EditSampleQcBindingModel data)
        {
            ResponseViewModel<QcSampling> response = new ResponseViewModel<QcSampling>();
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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<VRoomSamplePoint>))]
        public async Task<IActionResult> ListSampleAvailable([FromQuery] string search, string roomId, Int32 testParamId)
        {
            ResponseViewModel<SampleAvailableViewModel> response = new ResponseViewModel<SampleAvailableViewModel>();
            try
            {
                response = await _businessProvider.ListSampleAvailable(search, roomId, testParamId);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<VRoomSamplePoint>))]
        public async Task<IActionResult> ListSampleAvailableBySamplingId([FromQuery] string search, int samplingId, int? testParamId, string testScenarioLabel)
        {
            ResponseViewModel<SampleAvailableViewModel> response = new ResponseViewModel<SampleAvailableViewModel>();
            try
            {
                response = await _businessProvider.ListSampleAvailableBySamplingId(search, samplingId, testParamId, testScenarioLabel);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<QcLabelRelationViewModel>))]
        public async Task<IActionResult> ListLabelSampleQc([FromQuery] Int32 SamplingId, Int32 SampleId, string SampleCode, Int32 samplePointId, Int32 testParameterId)
        {
            ResponseViewModel<QcLabelRelationViewModel> response = new ResponseViewModel<QcLabelRelationViewModel>();
            try
            {
                response = await _businessProvider.ListLabelSampleQc(SamplingId, SampleId, SampleCode, samplePointId, testParameterId);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<QcLabelBatchRelationViewModel>))]
        public async Task<IActionResult> ListLabelBatchQc([FromQuery] string SamplingId, string SampleCode)
        {
            ResponseViewModel<QcLabelBatchRelationViewModel> response = new ResponseViewModel<QcLabelBatchRelationViewModel>();
            try
            {
                response = await _businessProvider.ListLabelBatchQc(SamplingId, SampleCode);

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
