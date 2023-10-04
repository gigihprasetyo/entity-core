using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcPersonel
    {
        public Int32 Id { get; set; }
        public string PersonelCode4 { get; set; }
        public string PersonelCode8 { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
