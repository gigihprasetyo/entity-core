using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcResultTestDetailViewModel
    {
        public Int32 RequestId { get; set; }
        public string NoBatch { get; set; }
        public int? EmPhaseId { get; set; }
        public string EmPhaseName { get; set; }
        public int? EmRoomId { get; set; }
        public string EmRoomName { get; set; }
        public DateTime? SamplingDateFrom { get; set; }
        public DateTime? SamplingDateTo { get; set; }
        public DateTime? ShipmentStartDate { get; set; }
        public DateTime? ShipmentEndDate { get; set; }
        public List<SamplingResult> SamplingResult { get; set; }
    }

    public partial class SamplingResult
    {
        public int? SampleId { get; set; }
        public int? SamplingPointId { get; set; }
        public string SamplingPointCode { get; set; }
        public string SampleCode { get; set; }
        public int? TestScenarioId { get; set; }
        public string TestScenarioName { get; set; }
        public Int32 TestParamId { get; set; }
        public string TestParamName { get; set; }
        public Int32? GradeRoomId { get; set; }
        public string GradeRoomName { get; set; }
        public string QcResultValue { get; set; }
        public string TestVariableConclusion { get; set; }
        public Int32? TestVariableId { get; set; }
        public string Note { get; set; }
        public string AttchmentFile { get; set; }
    }

}
