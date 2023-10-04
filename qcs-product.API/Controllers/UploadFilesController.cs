using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.BusinessProviders;
using qcs_product.API.ViewModels;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System;

namespace qcs_product.API.Controllers
{
    [Route("qcs/v1/[controller]")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        private readonly IUploadFilesBusinessProvider _businessProvider;
        private readonly ILogger<UploadFilesController> _logger;

        [ExcludeFromCodeCoverage]
        public UploadFilesController(
            ILogger<UploadFilesController> logger,
            IUploadFilesBusinessProvider businessProvider)
        {
            _businessProvider = businessProvider ?? throw new ArgumentNullException(nameof(businessProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        // [Q100AUAMAuthorization]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ResponseViewModel<UploadFileViewModel>))]
        public async Task<IActionResult> Insert(IFormFile file)
        {
            var response = new ResponseViewModel<UploadFileViewModel>();

            try
            {
                var data = new UploadFilesBindingModel();
                data.attchmentFile = file;
                response = await _businessProvider.Upload(data);
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
