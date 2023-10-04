using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class SampleByQcProcessRelationViewModel
    {
        public Int32 Id { get; set; }
        public string BacthQrCode { get; set; }
        public string QrCode { get; set; }
        public string NoBatch { get; set; }
        public int? PhaseId { get; set; }
        public string PhaseName { get; set; }
        public int? TestParamId { get; set; }
        public string TestParamName { get; set; }
        public string TestParamShortName { get; set; }
        public int? RoomId { get; set; }
        public string RoomName { get; set; }
        public DateTime? SamplingDateTimeFrom { get; set; }
        public DateTime? SamplingDateTimeTo { get; set; }
        public int ShipmentStatus { get; set; }
        public DateTime? ShipmentStartDate { get; set; }
        public DateTime? ShipmentEndDate { get; set; }
        public int SampleTestTransaction { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }


        public string SamplingPointCode { get; set; }
        
        public int? SamplingPointId { get; set; }
    }
}
