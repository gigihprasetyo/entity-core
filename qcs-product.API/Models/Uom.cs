﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class Uom
    {
        public Int32 Id { get; set; }
        public Int32 UomId { get; set; }
        public string Label { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
