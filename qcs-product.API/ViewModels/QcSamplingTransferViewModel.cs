using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcSamplingTransferViewModel
    {
        public int Id { get; set; }
        public int SamplingId { get; set; }
        public string QrCode { get; set; }
        public int RequestId { get; set; }
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
        public int TestParamId { get; set; }
        public string TestParamName { get; set; }
        public int? fromOrgId { get; set; }
        public string fromOrgName { get; set; }
        public int? toOrgId { get; set; }
        public string toOrgName { get; set; }
        public DateTime? SamplingDateFrom { get; set; }
        public DateTime? SamplingDateTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public List<QcSamplingShipmentTranckerViewModel>? ShipmentTrackers { get; set; }
    }
}
