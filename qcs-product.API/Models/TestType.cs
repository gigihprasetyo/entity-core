﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class TestType
    {
        public Int32 Id { get; set; }
        public Int32 OrgId { get; set; }
        public string OrgName { get; set; }
        public string TestTypeCode { get; set; }
        public string Name { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
