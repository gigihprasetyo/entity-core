using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;

namespace qcs_product.API.BusinessProviders
{
    public interface IBaseApiBioServiceBusinessProviders
    {
        public Task<HttpResponseMessage> Perfom(string endPoint, HttpMethod method, string content, string token);
    }
}
