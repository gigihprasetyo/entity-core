using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcSamplingNotReceivedViewModel
    {
        public Int32 SamplingId { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string NoRequest { get; set; }
        public int? TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public int? SamplingTypeId { get; set; }
        public string SamplingTypeName { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public string NoBatch { get; set; }
        public int? EmRoomId { get; set; }
        public string EmRoomName { get; set; }
        public int? EmPhaseId { get; set; }
        public string EmPhaseName { get; set; }
        public int Status { get; set; }
        public int ShipmentStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<RequestRoomViewModel> RequestRooms { get; set; }
    }
}
