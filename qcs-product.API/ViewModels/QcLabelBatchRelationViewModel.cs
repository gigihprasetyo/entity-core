using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcLabelBatchRelationViewModel
    {
        public Int32 SamplingId { get; set; }
        
        public int RequestId { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string NoRequest { get; set; }
        public int? TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public int? SamplingTypeId { get; set; }
        public string SamplingTypeName { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? TypeFormId { get; set; }
        public string TypeFormName { get; set; }
        public string NoBatch { get; set; }
        public int? PurposeId { get; set; }
        public string PurposeName { get; set; }
        public int? EmRoomId { get; set; }
        public string EmRoomName { get; set; }
        public int? EmRoomGradeId { get; set; }
        public string EmRoomGradeName { get; set; }
        public int? EmPhaseId { get; set; }
        public string EmPhaseName { get; set; }
        public DateTime? SamplingDateFrom { get; set; }
        public DateTime? SamplingDateTo { get; set; }

        public int? OrgId { get; set; }
        public int Status { get; set; }
        public int ShipmentStatus { get; set; }
        public string OrgName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<RequestPurposesViewModel> RequestPurposes { get; set; }
        public List<RequestRoomViewModel> RequestRooms { get; set; }
        public List<QcLabelBatchTestParamViewModel> Testparameters { get; set; }
        
        public string WorkflowDocumentCode { get; set; }
        public string WorkflowStatus { get; set; }
    }
}
