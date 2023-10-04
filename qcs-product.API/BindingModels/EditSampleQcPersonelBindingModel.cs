using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class EditSampleQcPersonelBindingModel
    {
        public int? Id { get; set; }
        public int ProductProdPhasePersonelId { get; set; }
        public string PersonelNik { get; set; }
        public string PersonelName { get; set; }
    }
}
