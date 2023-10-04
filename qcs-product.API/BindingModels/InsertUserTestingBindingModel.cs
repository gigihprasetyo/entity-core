using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class InsertUserTestingBindingModel
    {
        [Required]
        public string Nama { get; set; }
        public string JenisKelamin { get; set; }
    }
}
