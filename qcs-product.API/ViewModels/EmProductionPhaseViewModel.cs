using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class EmProductionPhaseViewModel
    {
        public Int32 Id { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public Int32 ParentId { get; set; }
        public string QcProduct { get; set; }
        public string QcEm { get; set; }
        public Int32 RoomId { get; set; }
        public Int32 FacilityId { get; set; }
    }
}
