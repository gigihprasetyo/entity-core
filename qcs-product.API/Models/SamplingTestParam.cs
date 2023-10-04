﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class SamplingTestParam
    {
        public Int32 Id { get; set; }
        public Int32 SamplingPointId { get; set; }
        public Int32 TestParameterId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
