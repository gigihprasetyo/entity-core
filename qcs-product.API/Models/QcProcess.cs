using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class QcProcess
    {
        public QcProcess()
        {
            FormMaterial = new HashSet<FormMaterial>();
            FormProcedure = new HashSet<FormProcedure>();
            FormTool = new HashSet<FormTool>();
        }

        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? RoomId { get; set; }
        public int? IsInputForm { get; set; }
        public int? AddSampleLayoutType { get; set; }
        public int? PurposeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }

        [JsonIgnore]
        public virtual ICollection<FormMaterial> FormMaterial { get; set; }
        [JsonIgnore]
        public virtual ICollection<FormProcedure> FormProcedure { get; set; }
        [JsonIgnore]
        public virtual ICollection<FormTool> FormTool { get; set; }
    }
}
