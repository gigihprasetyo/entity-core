using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class FormMaterial
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public int ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string BatchNumber { get; set; }
        public decimal DefaultPackageQty { get; set; }
        public int UomPackage { get; set; }
        public decimal DefaultQty { get; set; }
        public int Uom { get; set; }
        public int ProcessId { get; set; }
        public int SectionId { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }

        public virtual QcProcess Process { get; set; }
    }
}
