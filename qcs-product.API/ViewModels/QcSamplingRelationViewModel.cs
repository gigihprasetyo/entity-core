using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcSamplingRelationViewModel
    {
        public Int32 SamplingId { get; set; }
        public Int32 RequestId { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string NoRequest { get; set; }
        public int? TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public int? SamplingTypeId { get; set; }
        public string SamplingTypeName { get; set; }
        public DateTime? SamplingDateFrom { get; set; }
        public DateTime? SamplingDateTo { get; set; }
        public int? ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int? StorageTemperatureId { get; set; }
        public string StorageTemperatureName { get; set; }
        public int? TypeFormId { get; set; }
        public string TypeFormName { get; set; }
        public string TestScenarioLabel { get; set; }
        public string NoBatch { get; set; }
        public int? EmPhaseId { get; set; }
        public string EmPhaseName { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public string AttchmentFile { get; set; }
        public int? OrgId { get; set; }
        public string OrgName { get; set; }
        public Int32? FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string WorkflowCode { get; set; }
        public int? ProductFormId { get; set; }
        public string ProductFormName { get; set; }
        public int? ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public DateTime? ProductDate { get; set; }
        public int? ProductMethodId { get; set; }
        public string ProductMethodName { get; set; }
        public string ProductShipmentTemperature { get; set; }
        public DateTime? ProductShipmentDate { get; set; }
        public string ProductDataLogger { get; set; }
        public int? ProductPhaseId { get; set; }
        public string ProductPhaseName { get; set; }
        public string Location { get; set; }
        public DateTime? ProcessDate { get; set; }
        public int? ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ItemTemperature { get; set; }
        public bool IsNoBatchEditable { get; set; }
        public List<RequestRoomViewModel> RequestRooms { get; set; }
        public List<RequestAhuViewModel> RequestAhu { get; set; }
        public List<RequestPurposesViewModel> RequestPurposes { get; set; }
        public List<RoomsTestTypeViewModel> TestParameterSampling { get; set; }
        public List<QcSamplingToolsViewModel> SamplingTools { get; set; }
        public List<QcSamplingMaterialViewModel> SamplingMaterials { get; set; }
        public List<QcSamplingAttachmentViewModel> SamplingAttachments { get; set; }
        public List<QcSampleViewModel> SampleData { get; set; }
        public List<QcSamplingGradeRoomViewModel> SamplesGradeRooms { get; set; }
        public List<WorkflowHistoryQcSampling> WorkflowHistory { get; set; }
        public List<TestTypeQcsViewModel> ProductTestTypeQcs { get; set; }
        public List<QcSamplingPersonelViewModel> SamplingPersonels { get; set; }
        public TransactionBatchViewModel Batch { get; set; }

    }

    public partial class RoomsTestTypeViewModel
    {
        public Int32 RoomId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public Int32 TestScenarioId { get; set; }
        public string TestScenarioName { get; set; }
        public string TestScenarioLabel { get; set; }
        public List<EmTestTypeViewModel> TestParameterSamplingDetail { get; set; }
    }

    public partial class QcSamplingAttachmentViewModel
    {
        public Int32 Id { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentFileLink { get; set; }
        public string AttachmentStorageName {get;set;}
    }

    public partial class QcSamplingGradeRoomViewModel
    {
        public Int32? GradeRoomId { get; set; }
        public string GradeRoomCode { get; set; }
        public string GradeRoomName { get; set; }
    }
}
