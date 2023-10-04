using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcSample : BaseEntity
    {
        public Int32 QcSamplingId { get; set; }
        public string Code { get; set; }
        public Int32? ParentId { get; set; }
        public Int32? SampleSequence { get; set; }
        public Int32? SamplingPointId { get; set; }
        public string SamplingPointCode { get; set; }
        public Int32? GradeRoomId { get; set; }
        public string GradeRoomName { get; set; }
        public Int32? ToolId { get; set; }
        public string ToolCode { get; set; }
        public string ToolName { get; set; }
        public Int32? ToolGroupId { get; set; }
        public string ToolGroupName { get; set; }
        public string ToolGroupLabel { get; set; }
        public Int32 TestParamId { get; set; }
        public string TestParamName { get; set; }
        public Int32? PersonalId { get; set; }
        public string PersonalInitial { get; set; }
        public string PersonalName { get; set; }
        public DateTime? SamplingDateTimeFrom { get; set; }
        public DateTime? SamplingDateTimeTo { get; set; }
        public decimal? ParticleVolume { get; set; }
        public string AttchmentFile { get; set; }
        public string Note { get; set; }
        public string RowStatus { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }


        public int? TestScenarioId { get; set; }
        public string ReviewQaNote { get; set; }
        public bool IsDefault { get; set; }
        public Int32? QcSamplingToolsId { get; set; }
        public Int32? QcSamplingMaterialsId { get; set; }
        public Int32 TestTypeId { get; set; }
        public string TestTypeName { get; set; }
        public Int32 TestTypeMethodId { get; set; }
        public string TestTypeMethodName { get; set; }
    }
}
