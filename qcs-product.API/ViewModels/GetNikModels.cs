using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class GetNikModels
    {
        public string Nik { get; set; }
        public List<string> NikArray { get; set; }
    }
}