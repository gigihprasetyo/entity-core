using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class RequestQcs : BaseEntity
    {
        public Int32 Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public DateTime? ReceiptDateQA { get; set; }
        public DateTime? ReceiptDateKabag { get; set; }
        public string NoRequest { get; set; }
        public int? RequestQcsId { get; set; }
        public string RequestQcsNo { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? TypeFormId { get; set; }
        public string TypeFormName { get; set; }
        public string NoBatch { get; set; }
        public string TypeRequest { get; set; }
        public int? ProductFormId { get; set; }
        public string ProductFormName { get; set; }
        public int? ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public int? ProductPresentationId { get; set; }
        public string ProductPresentationName { get; set; }
        public int? ProductPhaseId { get; set; }
        public string ProductPhaseName { get; set; }
        public string ProductTemperature { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? OrgId { get; set; }
        public string OrgName { get; set; }
        public string RowStatus { get; set; }
        public int? StorageTemperatureId { get; set; }
        public string StorageTemperatureName { get; set; }
        public int? PurposeId { get; set; }
        public string PurposeName { get; set; }
        public int? EmRoomId { get; set; }
        public string EmRoomName { get; set; }
        public int? EmRoomGradeId { get; set; }
        public string EmRoomGradeName { get; set; }
        public string WorkflowStatus { get; set; }
        public int? EmPhaseId { get; set; }
        public string EmPhaseName { get; set; }
        public int TypeRequestId { get; set; }
        public int? TestScenarioId { get; set; }
        public string TestScenarioName { get; set; }
        public string TestScenarioLabel { get; set; }
        public Int32? FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public string NoDeviation { get; set; }
        public string Conclusion { get; set; }
        public string Location { get; set; }
        public DateTime? ProcessDate { get; set; }
        public int? ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ItemTemperature { get; set; }
        public bool IsNoBatchEditable { get; set; } = true;
        public bool IsFromBulkRequest { get; set; } = false;
        public virtual ICollection<TestTypeQcs> TestTypeQcs { get; set; }
    }
}
