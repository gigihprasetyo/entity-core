using Microsoft.AspNetCore.Mvc;
using qcs_product.API.BindingModels;
using qcs_product.API.BusinessProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace qcs_product.API.Controllers
{
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class TamplateTestingInfoController : ControllerBase
    {
        private readonly ITemplateTestingInfoBusinessProvider _businessProvider;

        [ExcludeFromCodeCoverage]
        public TamplateTestingInfoController(ITemplateTestingInfoBusinessProvider businessProvider)
        {
            _businessProvider = businessProvider;
        }

        [HttpGet(template: "InfoByTestingTemplate/{templateTestingId}")]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<TemplateTestingInfoViewModel>))]
        public async Task<IActionResult> InfoByTestingTemplate(int templateTestingId)
        {
            ResponseOneDataViewModel<TemplateTestingInfoViewModel> response = new ResponseOneDataViewModel<TemplateTestingInfoViewModel>();
            try
            {
                response = await _businessProvider.InfoByTestingTemplate(templateTestingId);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost(template: "InsertPersonnel")]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TemplateTestingPersonnel>))]
        public async Task<IActionResult> InsertPersonnel([FromBody] List<TemplateTestingPersonnel> data)
        {
            ResponseViewModel<TemplateTestingPersonnel> response = new ResponseViewModel<TemplateTestingPersonnel>();
            try
            {
                response = await _businessProvider.InsertPersonnel(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost(template: "InsertNote")]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<TemplateTestingPersonnel>))]
        public async Task<IActionResult> InsertNote([FromBody] TemplateTestingNote data)
        {
            ResponseOneDataViewModel<TemplateTestingNote> response = new ResponseOneDataViewModel<TemplateTestingNote>();
            try
            {
                response = await _businessProvider.InsertNote(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost(template: "InsertAttachment")]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TemplateTestingAttachment>))]
        public async Task<IActionResult> InsertAttachment([FromBody] List<TemplateTestingAttachment> data)
        {
            ResponseViewModel<TemplateTestingAttachment> response = new ResponseViewModel<TemplateTestingAttachment>();
            try
            {
                response = await _businessProvider.InsertAttachment(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost(template: "DeleteAttachment")]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TemplateTestingAttachment>))]
        public IActionResult DeleteAttachment([FromBody] List<int> listId)
        {
            ResponseViewModel<TemplateTestingAttachment> response = new ResponseViewModel<TemplateTestingAttachment>();
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
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<TemplateTestingAttachment>))]
        public async Task<IActionResult> CheckInCheckOutPersonnel([FromBody] InsertCheckInCheckOutPersonnel data)
        {
            ResponseOneDataViewModel<TemplateTestingPersonnel> response = new ResponseOneDataViewModel<TemplateTestingPersonnel>();
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

        [HttpGet(template: "DetailAttachment/{templateTestingAttachmentId}")]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<TemplateTestingInfoViewModel>))]
        public async Task<IActionResult> DetailAttachment(int templateTestingAttachmentId)
        {
            ResponseOneDataViewModel<TemplateTestingInfoViewModel> response = new ResponseOneDataViewModel<TemplateTestingInfoViewModel>();
            try
            {
                response = await _businessProvider.DetailAttachment(templateTestingAttachmentId);

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
