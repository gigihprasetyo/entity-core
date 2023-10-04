﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class FormProcedure
    {
        public FormProcedure()
        {
            FormParameter = new HashSet<FormParameter>();
        }

        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public int ProcessId { get; set; }
        public int SectionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }

        public virtual QcProcess Process { get; set; }
        public virtual ICollection<FormParameter> FormParameter { get; set; }
    }
}
