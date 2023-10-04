using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.ViewModels
{
    public class ToolDetailViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int? RoomId { get; set; }

        public int? FacilityId { get; set; }

        public int? GradeRoomId { get; set; }

        public int? ToolGroupId { get; set; }

        public List<ToolActivityRelationViewModel> Activities { get; set; }

        public int? MachineId { get; set; }

        public string SerialNumberId { get; set; }

        public List<ToolPurposesDatas> ListDataPurposes { get; set; }

    }

    public partial class ToolPurposesDatas
    {
        public int Id { get; set; }
        public int ToolId { get; set; }
        public List<Purpose> Purpose { get; set; }
        public List<ToolSamplingPointViewModel> SamplingPoints { get; set; }
        public List<ToolSamplingPointLayout> SamplingPointLayout { get; set; }
    }

    // public class ToolActivityIntegrationEvent
    // {
    //     public string ActivityCode { get; set; }

    //     public string ActivityName { get; set; }

    //     public DateTime ActivityDate { get; set; }

    //     public DateTime ExpiredDate { get; set; }
    // }
}
