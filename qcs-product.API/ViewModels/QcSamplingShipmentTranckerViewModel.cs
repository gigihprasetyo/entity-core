using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcSamplingShipmentTranckerViewModel
    {
        public Int32 Id { get; set; }
        public string QrCode { get; set; }
        public string Type { get; set; }
        public DateTime processAt { get; set; }
        public string UserNik { get; set; }
        public string UserName { get; set; }
        public Int32? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
