using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using qcs_product.API.BindingModels;
using qcs_product.API.BusinessProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Auth.Authorization;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace qcs_product.API.Controllers
{
    [ApiController]
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    public class OperatorTestingController : ControllerBase
    {
        private readonly IOperatorTestingBusinessProvider _businessProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OperatorTestingController> _logger;

        [ExcludeFromCodeCoverage]
        public OperatorTestingController(
            IOperatorTestingBusinessProvider businessProvider, IServiceProvider serviceProvider, ILogger<OperatorTestingController> logger)
        {
            _businessProvider = businessProvider ?? throw new ArgumentNullException(nameof(businessProvider));
            _serviceProvider = serviceProvider;
            _logger = logger;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<TransactionTesting>))]
        public async Task<IActionResult> SetStartDate(int testingId)
        {
            ResponseOneDataViewModel<TransactionTesting> response = new ResponseOneDataViewModel<TransactionTesting>();
            try
            {
                response.StatusCode = 200;
                response.Message = ApplicationConstant.OK_MESSAGE;
                response.Data = await _businessProvider.SetStartDate(testingId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message, ex);
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<TransactionTesting>))]
        public async Task<IActionResult> SetEndDate(int testingId)
        {
            ResponseOneDataViewModel<TransactionTesting> response = new ResponseOneDataViewModel<TransactionTesting>();
            try
            {
                response.StatusCode = 200;
                response.Message = ApplicationConstant.OK_MESSAGE;
                response.Data = await _businessProvider.SetEndDate(testingId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message, ex);
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet(template: "InfoGeneralByTestingId/{TestingId}")]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<GeneralOperatorTestingInfoViewModel>))]
        public async Task<IActionResult> InfoByTestingId(int TestingId)
        {
            ResponseOneDataViewModel<GeneralOperatorTestingInfoViewModel> response = new ResponseOneDataViewModel<GeneralOperatorTestingInfoViewModel>();
            try
            {
                response = await _businessProvider.InfoGeneralByTestingId(TestingId);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// list of process group by item id
        /// </summary>
        /// <returns>list of process group</returns>
        // [Q100AUAMAuthorization]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<InsertTestingPQViewModel>))]
        public async Task<IActionResult> InsertUserPQ([FromBody] InsertTestingPQBindingModel data)
        {
            ResponseOneDataViewModel<InsertTestingPQViewModel> response = new ResponseOneDataViewModel<InsertTestingPQViewModel>();
            try
            {
                response = await _businessProvider.InsertUserPQ(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        // [Q100AUAMAuthorization]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TransactionTestingNote>))]
        public async Task<IActionResult> InsertNote([FromBody] InsertTestingNoteBindingModel data)
        {
            ResponseViewModel<TransactionTestingNote> response = new ResponseViewModel<TransactionTestingNote>();
            try
            {
                response = await _businessProvider.InsertTestingNote(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        // [Q100AUAMAuthorization]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<InsertTestingAttachmentViewModel>))]
        public async Task<IActionResult> InsertTestingAttachment([FromBody] InsertTestingAttachmentBindingModel data)
        {
            ResponseOneDataViewModel<InsertTestingAttachmentViewModel> response = new ResponseOneDataViewModel<InsertTestingAttachmentViewModel>();
            try
            {
                response = await _businessProvider.InsertTestingAttachment(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Check In by Pin
        /// </summary>
        /// <returns>UserPresenceViewModel</returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<UserPresenceViewModel>))]
        public async Task<IActionResult> CheckInByPin([FromBody] CheckInOutBindingModel data)
        {
            ResponseOneDataViewModel<UserPresenceViewModel> response = new ResponseOneDataViewModel<UserPresenceViewModel>();
            try
            {
                response = await _businessProvider.CheckInByPin(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Check In by Pin
        /// </summary>
        /// <returns>UserPresenceViewModel</returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<UserPresenceViewModel>))]
        public async Task<IActionResult> CheckOutByPin([FromBody] CheckInOutBindingModel data)
        {
            ResponseOneDataViewModel<UserPresenceViewModel> response = new ResponseOneDataViewModel<UserPresenceViewModel>();
            try
            {
                response = await _businessProvider.CheckOutByPin(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }
    }
}
