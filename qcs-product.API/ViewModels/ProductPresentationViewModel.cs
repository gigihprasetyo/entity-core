using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class ProductPresentationViewModel
    {
        public Int32 Id { get; set; }
        public string PresentationCode { get; set; }
        public string PresentationName { get; set; }
        public string RowStatus { get; set; }
    }
}
