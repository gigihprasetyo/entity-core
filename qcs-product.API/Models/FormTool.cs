using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class FormTool
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public int Type { get; set; }
        public int ToolId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ItemId { get; set; }
        public decimal Qty { get; set; }
        public int ProcessId { get; set; }
        public int SectionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }

        public virtual QcProcess Process { get; set; }
    }
}
