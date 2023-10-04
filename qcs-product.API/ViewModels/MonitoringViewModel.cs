using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class MonitoringViewModel
    {
        public Int32 Id { get; set; }
        public DateTime Date { get; set; }
        public string NoRequest { get; set; }
        public Int32 TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public string NoBatch { get; set; }
        public Int32 ItemId { get; set; }
        public string ItemName { get; set; }
        public Int32 StorageTemperatureId { get; set; }
        public string StorageTemperatureName { get; set; }
        public Int32 TestScenarioId { get; set; }
        public string TestScenarioName { get; set; }
        public string TestScenarioLabel { get; set; }
        public Int32 TypeFormId { get; set; }
        public string TypeFormName { get; set; }
        public Int32 ProductFormId { get; set; }
        public string ProductFormName { get; set; }
        public Int32 ProductPhaseId { get; set; }
        public string ProductPhaseName { get; set; }
        public string ProductTemperature { get; set; }
        public Int32 PurposeId { get; set; }
        public string PurposeName { get; set; }
        public Int32 EmRoomId { get; set; }
        public string EmRoomName { get; set; }
        public Int32 EmRoomGradeId { get; set; }
        public string EmRoomGradeName { get; set; }
        public Int32 EmPhaseId { get; set; }
        public string EmPhaseName { get; set; }
        public int Status { get; set; }
        public string WorkflowStatus { get; set; }
        public Int32 OrgId { get; set; }
        public string OrgName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<WorkflowHistoryViewModel> WorkFlowHistory { get; set; }
        public List<TestTypeQcsViewModel> TestTypeQcs { get; set; }
    }
}
