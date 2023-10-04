using Microsoft.AspNetCore.Mvc;
using qcs_product.API.BusinessProviders;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace qcs_product.API.Controllers
{
    [Route(ApplicationConstant.ENDPOINT_FORMAT)]
    [ApiController]
    public class DeviationController : ControllerBase
    {
        private readonly IDeviationBusinessProvider _deviationBusinessProvider;
        [ExcludeFromCodeCoverage]
        public DeviationController(IDeviationBusinessProvider deviationBusinessProvider) 
        {
            _deviationBusinessProvider = deviationBusinessProvider;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<ListDeviationViewModel>))]
        public async Task<IActionResult> GetListDeviation([FromQuery] string search, int page, int limit)
        {
            ResponseViewModel<ListDeviationViewModel> response = new ResponseViewModel<ListDeviationViewModel>();
            try
            {
                response = await _deviationBusinessProvider.ListDeviation(search, page, limit);

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ResponseOneDataViewModel<DetailDeviationViewModel>))]
        public async Task<IActionResult> GetDetailDeviation([FromQuery] int sampleId)
        {
            ResponseOneDataViewModel<DetailDeviationViewModel> response = new ResponseOneDataViewModel<DetailDeviationViewModel>();
            try
            {
                response.Data = await _deviationBusinessProvider.DetailDeviation(sampleId);

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
