using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class EditSampleQcMaterialBindingModel
    {
        public Int32? Id { get; set; }
        public Int32 ItemId { get; set; }
        public string ItemName { get; set; }
        public Int32? ItemBatchId { get; set; }
        public string NoBatch { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
