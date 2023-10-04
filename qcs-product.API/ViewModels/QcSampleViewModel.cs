using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcSampleViewModel
    {
        public Int32 Id { get; set; }
        public Int32? ParentId { get; set; }
        public Int32? SampleSequence { get; set; }
        public Int32 QcSamplingId { get; set; }
        public string Code { get; set; }
        public Int32? SamplingPointId { get; set; }
        public string SamplingPointCode { get; set; }
        public Int32? GradeRoomId { get; set; }
        public string GradeRoomCode { get; set; }
        public string GradeRoomName { get; set; }
        public Int32? ToolId { get; set; }
        public string ToolCode { get; set; }
        public string ToolName { get; set; }
        public Int32? ToolGroupId { get; set; }
        public string ToolGroupName { get; set; }
        public string ToolGroupLabel { get; set; }
        public QcSamplingToolsViewModel SelectedTool { get; set; }
        public QcSamplingMaterialViewModel SelectedMaterial { get; set; }
        public Int32 TestParamId { get; set; }
        public string TestParamName { get; set; }
        public int TestParamSequence { get; set; }
        public Int32? PersonalId { get; set; }
        public string PersonalInitial { get; set; }
        public string PersonalName { get; set; }
        public DateTime? SamplingDateTimeFrom { get; set; }
        public DateTime? SamplingDateTimeTo { get; set; }
        public decimal? ParticleVolume { get; set; }
        public string AttchmentFile { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ResultValue { get; set; }
        public string ResultConclusion { get; set; }
        public int? TestScenarioId { get; set; }
        public string TestScenarioName { get; set; }
        public int? TestParamIndex { get; set; }
        public string ReviewQaNote { get; set; }
        public bool IsDefault { get; set; }
        public List<QcSampleChildViewModel> SampleChild { get; set; }
        public List<QcSampleTestScenarioViewModel> SampleTestScenario { get; set; }
        public List<RequestPurposesViewModel> Purpose { get; set; }

    }
    public partial class QcSampleTestScenarioViewModel
    {
        public Int32 TestScenarioId { get; set; }
        public string TestScenarioName { get; set; }
        public string TestScenarioLabel { get; set; }
    }
}
