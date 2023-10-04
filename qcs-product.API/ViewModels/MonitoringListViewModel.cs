using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class MonitoringListViewModel
    {
        public Int32 Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public DateTime? ReceiptDateQA { get; set; }
        public DateTime? ReceiptDateKabag { get; set; }
        public string NoRequest { get; set; }
        public int TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public string NoBatch { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? TypeFormId { get; set; }
        public string TypeFormName { get; set; }
        public int Status { get; set; }
        public string WorkflowStatus { get; set; }
        public int? FacilityId { get; set; }
        public int? OrgId { get; set; }
        public string OrgName { get; set; }
        public string CreatedBy { get; set; }
        public string PhaseName { get; set; }
        public int? PhaseId { get; set; }
        public string RoomName { get; set; }
        public int? RoomId { get; set; }
        public string NoDeviation { get; set; }
        public string Conclusion { get; set; }
        public string ConclusionTemp { get; set; }

        public DateTime CreatedAt { get; set; }
        public List<MonitoringSamplingListViewModel> Sampling { get; set; }
        public List<RequestPurposesViewModel> RequestPurposes { get; set; }
    }

    public partial class NikMonitoringListViewModel
    {
        public string Nik { get; set; }
        public int OrgId { get; set; }
        public string RoleCode { get; set; }
        public List<MonitoringListViewModel> MonitoringListVm { get; set; }
        public List<QcRequestSamplingRelationViewModel> RequestSamplingVm { get; set; }
    }
}
