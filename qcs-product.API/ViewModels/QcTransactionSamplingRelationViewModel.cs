using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.ViewModels
{
    public class QcTransactionSamplingRelationViewModel
    {
        public Int32 Id { get; set; }
        public Int32 SamplingId { get; set; }
        public string BacthQrCode { get; set; }
        public string NoBatch { get; set; }
        public int? PhaseId { get; set; }
        public string PhaseName { get; set; }
        public int? RoomId { get; set; }
        public string RoomName { get; set; }
        public List<RequestRoom> ListRoom { get; set; }
        public DateTime? ShipmentStartDate { get; set; }
        public DateTime? ShipmentEndDate { get; set; }
        public int ShipmentStatus { get; set; }
        public int SamplingTestTransaction { get; set; }
    }
}
