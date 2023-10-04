using System;
using System.Collections.Generic;
using qcs_product.API.BindingModels;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.ViewModels
{
    public partial class TransactionToolViewModel
    {

        public int Id { get; set; }
        public string ToolCode { get; set; }
        public string Name { get; set; }
        public int ToolGroupId { get; set; }
        public int? RoomId { get; set; }
        public int? GradeRoomId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SerialNumberId { get; set; }
        public int? MachineId { get; set; }
        public int? OrganizationId { get; set; }
        public int? FacilityId { get; set; }
        public int? ObjectStatus { get; set; }

        // public virtual TransactionToolGroup ToolGroup { get; set; }
        // public virtual ICollection<TransactionToolActivity> TransactionToolActivity { get; set; }
        public virtual List<SamplingPointLiteViewModel> SamplingPointInfo { get; set; }
    }
}
