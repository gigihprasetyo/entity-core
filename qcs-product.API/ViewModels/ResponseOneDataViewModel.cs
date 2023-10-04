using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public partial class ResponseOneDataViewModel<T>
    {
        public int StatusCode { set; get; }
        public string Message { set; get; }
        public object Data { set; get; }
    }
}
