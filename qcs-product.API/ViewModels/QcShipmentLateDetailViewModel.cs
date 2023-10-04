using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcShipmentLateDetailViewModel
    {
        public Int32 QcRequestId { get; set; }
        public Int32 QcSamplingId { get; set; }
        public Int32 QcShipmentId { get; set; }
        public string NoRequest { get; set; }
        public string NoBatch { get; set; }
        public int? OrgId { get; set; }
        public string QrSamplingCode { get; set; }
        public string QrShipmentCode { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public int? SamplingTypeId { get; set; }
        public string SamplingTypeName { get; set; }
        public string TestParameterSamplings { get; set; }
        public int? EmPhaseId { get; set; }
        public string EmPhaseName { get; set; }
        public Int32? FromOrganizationId { get; set; }
        public string FromOrganizationName { get; set; }
        public Int32? ToOrganizationId { get; set; }
        public string ToOrganizationName { get; set; }
        public int Status { get; set; }
        public DateTime? ShipmentStartDate { get; set; }
        public DateTime? ShipmentEndDate { get; set; }
        public DateTime? LastSamplingDateTime { get; set; }
        public string ShipmentNote { get; set; }
        public DateTime? ShipmentApprovalDate { get; set; }
        public string ShipmentApprovalBy { get; set; }
        public List<RequestRoomRelationViewModel> RequestRooms { get; set; }
        public List<QcSamplingShipmentTranckerViewModel> ShipmentTrackers { get; set; }
    }
}