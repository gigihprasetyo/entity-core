using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    public class UploadFilesBindingModel
    {
        public IFormFile attchmentFile { get; set; }
    }
}
