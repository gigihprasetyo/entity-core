using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcSamplingShipmentViewModel
    {
        public Int32 Id { get; set; }
        public string QrCode { get; set; }
        public string NoRequest { get; set; }
        public Int32 TestParamId { get; set; }
        public string TestParamName { get; set; }
        public Int32? FromOrganizationId { get; set; }
        public string FromOrganizationName { get; set; }
        public Int32? ToOrganizationId { get; set; }
        public string ToOrganizationName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsLateTransfer { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<QcSamplingShipmentTranckerViewModel> ShipmentTrackers { get; set; }
    }
}
