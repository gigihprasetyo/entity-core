using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class UploadFilesBusinessProvider : IUploadFilesBusinessProvider
    {
        private readonly ILogger<UploadFilesBusinessProvider> _logger;

        [ExcludeFromCodeCoverage]
        public UploadFilesBusinessProvider(ILogger<UploadFilesBusinessProvider> logger)
        {
            _logger = logger;
        }

        //to do await process
        public async Task<ResponseViewModel<UploadFileViewModel>> Upload(UploadFilesBindingModel data)
        {
            var bucketName = ApplicationConstant.GCS_BUCKET_DEV_VENDOR;
            //var bucketName = ApplicationConstant.GCS_BUCKET_DEV;
            var objectExt = System.IO.Path.GetExtension(data.attchmentFile.FileName);
            var objectName = GenerateFileName() + objectExt;

            var storage = StorageClient.Create();
            var result = new ResponseViewModel<UploadFileViewModel>();
            var resultUpload = storage.UploadObject(bucketName, objectName, null, data.attchmentFile.OpenReadStream());
            var linkUpload = await GenerateV4SignedReadUrl(objectName);

            result.StatusCode = 200;
            result.Message = ApplicationConstant.OK_MESSAGE;
            result.Data = new List<UploadFileViewModel>();

            result.Data.Add(new UploadFileViewModel()
            {
                ObjectName = objectName,
                FileName = resultUpload.Name,
                Ext = objectExt,
                MediaLink = linkUpload,
                Size = resultUpload.Size
            });


            return result;
        }

        //to do helper
        public static string GenerateFileName()
        {
            long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return $"QCS-{milliseconds}";
        }

        public async Task<string> GenerateV4SignedReadUrl(string fileName)
        {
            string url = "";
            //string bucketName = ApplicationConstant.GCS_BUCKET_DEV;
            string bucketName = ApplicationConstant.GCS_BUCKET_TESTING;
            string objectName = fileName;
            var credentialFilePath = Environment.GetEnvironmentVariable(ApplicationConstant.GOOGLE_APPLICATION_CREDENTIALS);

            UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credentialFilePath);
            // V4 is the default signing version.
            url = await urlSigner.SignAsync(bucketName, objectName, TimeSpan.FromHours(1), HttpMethod.Get);

            return url;
        }


    }
}
