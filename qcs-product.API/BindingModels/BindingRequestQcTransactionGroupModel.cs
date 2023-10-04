using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public class BindingRequestQcTransactionGroupModel
    {
        public Int32 RequestQcId { get; set; }
        public string CodeTest { get; set; }
        public string TestType { get; set; }
    }
}
