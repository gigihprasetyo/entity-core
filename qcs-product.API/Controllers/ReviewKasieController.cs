using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace qcs_product.API.Controllers
{
    [ApiController]
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    public class ReviewKasieController : ControllerBase
    {
        private readonly IReviewKasieBusinessProvider _businessProvider;
        //private readonly IServiceProvider _serviceProvider;

        [ExcludeFromCodeCoverage]
        public ReviewKasieController(
            IReviewKasieBusinessProvider businessProvider/*, IServiceProvider serviceProvider*/)
        {
            _businessProvider = businessProvider ?? throw new ArgumentNullException(nameof(businessProvider));
            //_serviceProvider = serviceProvider;
        }

        /// <summary>
        /// example list
        /// </summary>
        /// <returns>list of example</returns>
        //[Q100AUAMAuthorization]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<ReviewKasieTemplateQCListViewModel>))]
        public async Task<IActionResult> ReviewListTemplate([FromQuery]string positionId,string filter, string status, int page, int limit, DateTime? startDate, DateTime? endDate)
        {
            ResponseViewModel<ReviewKasieTemplateQCListViewModel> response = new ResponseViewModel<ReviewKasieTemplateQCListViewModel>();
            try
            {
                //var authorization = Request.Headers[HeaderNames.Authorization];
                //string token = string.Empty;
                //if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                //{
                //    token = headerValue.Parameter;
                //}
                //var authorizationFilter = _serviceProvider.GetService<Q100AUAMAuthorizationFilter>();
                //var decode = authorizationFilter.DecodeJwtToken(token);

                response = await _businessProvider.ListReviewTemplate(positionId, filter, status, startDate, endDate, page, limit);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);

        }

        [HttpGet(template: "InfoByTestingId/{TestingId}")]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<TransactionTemplateTestingInfoViewModel>))]
        public async Task<IActionResult> InfoByTestingId(int TestingId, string filter)
        {
            ResponseOneDataViewModel<TransactionTemplateTestingInfoViewModel> response = new ResponseOneDataViewModel<TransactionTemplateTestingInfoViewModel>();
            try
            {
                response = await _businessProvider.InfoByTestingTemplate(TestingId, filter);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost(template: "UpdatePersonnel")]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TransactionTestingPersonnel>))]
        public async Task<IActionResult> UpdatePersonnel([FromBody] List<TransactionTestingPersonnel> data)
        {
            ResponseViewModel<TransactionTestingPersonnel> response = new ResponseViewModel<TransactionTestingPersonnel>();
            try
            {
                response = await _businessProvider.UpdatePersonnel(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost(template: "UpdateNote")]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<TransactionTestingNote>))]
        public async Task<IActionResult> InsertNote([FromBody] TransactionTestingNote data)
        {
            ResponseOneDataViewModel<TransactionTestingNote> response = new ResponseOneDataViewModel<TransactionTestingNote>();
            try
            {
                response = await _businessProvider.UpdateNote(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost(template: "UpdateAttachment")]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TransactionTestingAttachment>))]
        public async Task<IActionResult> InsertAttachment([FromBody] List<TransactionTestingAttachment> data)
        {
            ResponseViewModel<TransactionTestingAttachment> response = new ResponseViewModel<TransactionTestingAttachment>();
            try
            {
                response = await _businessProvider.UpdateAttachment(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost(template: "DeleteAttachment")]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TransactionTestingAttachment>))]
        public IActionResult DeleteAttachment([FromBody] List<int> listId)
        {
            ResponseViewModel<TransactionTestingAttachment> response = new ResponseViewModel<TransactionTestingAttachment>();
            try
            {
                response = _businessProvider.DeleteAttachment(listId);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost(template: "CheckInCheckOutPersonnel")]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<TransactionTestingPersonnel>))]
        public async Task<IActionResult> CheckInCheckOutPersonnel([FromBody] UpdateCheckInCheckOutPersonnel data)
        {
            ResponseOneDataViewModel<TransactionTestingPersonnel> response = new ResponseOneDataViewModel<TransactionTestingPersonnel>();
            try
            {
                response = await _businessProvider.CheckInCheckOut(data);
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
