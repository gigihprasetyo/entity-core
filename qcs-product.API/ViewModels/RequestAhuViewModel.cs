using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class RequestAhuViewModel
    {
        public Int32 Id { get; set; }
        public Int32 AhuId { get; set; }
        public string AhuCode { get; set; }
        public string AhuName { get; set; }
    }
}
