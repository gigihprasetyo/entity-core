﻿using System;
using System.Collections.Generic;

namespace qcs_product.API.MasterModels
{
    public class TestType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameId { get; set; }
        public string NameEn { get; set; }
        public int OrganizationId { get; set; }
        public int ObjectStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
        public virtual ICollection<TestTypeMethod> TestTypeMethod { get; set; }
    }
}
