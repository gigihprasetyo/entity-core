using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class ResponseViewModel<T>
    {
        public int StatusCode { set; get; }
        public string Message { set; get; }
        public List<T> Data { set; get; }
        public MetaViewModel Meta { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public partial class MetaViewModel
    {
        public int TotalPages { get; set; }
        public int TotalItem { get; set; }
    }
}
