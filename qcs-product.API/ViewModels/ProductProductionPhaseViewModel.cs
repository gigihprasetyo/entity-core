using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class ProductProductionPhaseViewModel
    {
        public Int32 Id { get; set; }
        public string ProductProdPhaseCode { get; set; }
        public Int32 ItemId { get; set; }
        public string ItemName { get; set; }
        public string Name { get; set; }
        public List<RoomViewModel> Rooms { get; set; }
    }
}
