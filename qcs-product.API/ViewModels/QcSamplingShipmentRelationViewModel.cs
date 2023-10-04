using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcSamplingShipmentRelationViewModel
    {
        public Int32 Id { get; set; }
        public string QrCode { get; set; }
        public string NoRequest { get; set; }
        public string NoBatch { get; set; }
        public int? TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public int? SamplingTypeId { get; set; }
        public string SamplingTypeName { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? EmPhaseId { get; set; }
        public string EmPhaseName { get; set; }
        public int? TestParamId { get; set; }
        public string TestParamName { get; set; }
        public Int32? FromOrganizationId { get; set; }
        public string FromOrganizationName { get; set; }
        public Int32? ToOrganizationId { get; set; }
        public string ToOrganizationName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsLateTransfer { get; set; }
        public int Status { get; set; }
        public Int32 QcSamplingId { get; set; }
        public DateTime? LastSamplingDateTime { get; set; }
        public string ShipmentNote { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<QcSamplingShipmentTranckerViewModel>? ShipmentTrackers { get; set; }
    }
}
