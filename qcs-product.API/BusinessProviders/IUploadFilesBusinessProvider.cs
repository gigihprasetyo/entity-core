using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IUploadFilesBusinessProvider
    {
        public Task<ResponseViewModel<UploadFileViewModel>> Upload(UploadFilesBindingModel data);
        public Task<string> GenerateV4SignedReadUrl(string fileName);
    }
}
