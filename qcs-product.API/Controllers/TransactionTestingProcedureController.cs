using Microsoft.AspNetCore.Mvc;
using production_execution_system.API.ViewModels;
using qcs_product.API.BindingModels;
using qcs_product.API.BusinessProviders;
using qcs_product.API.BusinessProviders.Collection;
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
    public class TransactionTestingProcedureController : ControllerBase
    {
        private readonly ITransactionTestTypeBusinessProvider _businessProvider;

        [ExcludeFromCodeCoverage]
        public TransactionTestingProcedureController(ITransactionTestTypeBusinessProvider businessProvider)
        {
            _businessProvider = businessProvider;        
        }


        [HttpGet("{testTypeId}")]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<TransactionTestingViewModel>))]
        public async Task<IActionResult> GetTestingWProcedure(int testTypeId)
        {
            ResponseOneDataViewModel<TransactionTestingViewModel> response = new ResponseOneDataViewModel<TransactionTestingViewModel>();
            try
            {
                response = await _businessProvider.GetTestingWProcedure(testTypeId);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<TransactionTestingProcedure>))]
        public async Task<IActionResult> UpdateParameter([FromBody] InsertTransactionTestingProcedureBindingModel data)
        {
            ResponseOneDataViewModel<TransactionTestingProcedure> response = new ResponseOneDataViewModel<TransactionTestingProcedure>();
            try
            {
                response = await _businessProvider.UpdateParameterValue(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<InsertParameterAttachmentViewModel>))]
        public async Task<IActionResult> InsertParameterAttachment([FromBody] InsertParameterAttachmentBindingModel data)
        {
            ResponseOneDataViewModel<InsertParameterAttachmentViewModel> response = new ResponseOneDataViewModel<InsertParameterAttachmentViewModel>();
            try
            {
                response = await _businessProvider.InsertParameterAttachment(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<ListParameterNoteViewModel>))]
        public async Task<IActionResult> InsertParameterNote([FromBody] InsertParameterNoteBindingModel data)
        {
            ResponseOneDataViewModel<ListParameterNoteViewModel> response = new ResponseOneDataViewModel<ListParameterNoteViewModel>();
            try
            {
                response = await _businessProvider.InsertParameterNote(data);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }


        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<TestingProcedureParameterViewModel>))]
        public async Task<IActionResult> InsertExecption([FromBody] InsertExceptionBindingModel data)
        {
            ResponseViewModel<TestingProcedureParameterViewModel> response = new ResponseViewModel<TestingProcedureParameterViewModel>();
            try
            {
                response = await _businessProvider.InsertMultipleDeviation(data);
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
