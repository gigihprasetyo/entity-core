using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcTransactionSampleRelationViewModel
    {
        public Int32 Id { get; set; }
        public Int32 QcSampleId { get; set; }
        public string SampleCode { get; set; }
        public string NoBatch { get; set; }
        public string NoRequest { get; set; }
        public int? EmRoomId { get; set; }
        public string EmRoomName { get; set; }
        public int? GradeRoomId { get; set; }
        public string GradeRoomName { get; set; }
        public string GradeRoomCode { get; set; }
        public int? EmPhaseId { get; set; }
        public string EmPhaseName { get; set; }
        public int? TestParamId { get; set; }
        public string TestParamName { get; set; }
        public string TestParamShortName { get; set; }
        public int? TestParamSequence { get; set; }
        public int? PersonalId { get; set; }
        public string PersonalInitial { get; set; }
        public string PersonalName { get; set; }
        public DateTime? ShipmentStartDate { get; set; }
        public DateTime? ShipmentEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public int? SamplingPointId { get; set; }

        public string SamplingPointCode { get; set; }
        public string TestScenarioLabel { get; set; }
        public int? TestScenarioCode { get; set; }
        public int? FdLastSamplingPointCode { get; set; }
        public string FirstSamplingPointCode { get; set; }
        public int? LastSamplingPointCode { get; set; }
        public List<TestVariableViewModel> TestVariableThreshold { get; set; }
        public int? TestParamIndex { get; set; }
        public DateTime? SamplingDateTimeFrom { get; set; }
        public DateTime? SamplingDateTimeTo { get; set; }
        public List<RequestPurposesViewModel> Purpose { get; set; }
    }
}

