using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public class MasterDataUseByRequest
    {
        public int RoomId { get; set; }
        public int FacilityId { get; set; }
        public string CreatedBy { get; set; }
    }
}
