using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class BioHrLoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApplicationCode { get; set; }
    }
}
