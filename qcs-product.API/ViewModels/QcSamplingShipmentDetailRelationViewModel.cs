using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcSamplingShipmentDetailRelationViewModel
    {
        public int RequestId { get; set; }
        public string NoRequest { get; set; }
        public string NoBatch { get; set; }
        public string QrCode { get; set; }
        public int? TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public int SamplingId { get; set; }
        public int? SamplingTypeId { get; set; }
        public string SamplingTypeName { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? EmPhaseId { get; set; }
        public string EmPhaseName { get; set; }
        public DateTime? LastSamplingDateTime { get; set; }
        public string ShipmentNote { get; set; }
        public List<RequestRoomRelationViewModel> RequestRooms { get; set; }
        public List<QcSamplingShipmentTranckerViewModel> ShipmentTrackers { get; set; }
    }
}