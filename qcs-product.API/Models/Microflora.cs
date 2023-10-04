using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class Microflora
    {
        public Int32 Id { get; set; }
        public string Code { get; set; }
        public string MicrobaCategory { get; set; }
        public string MicrobaId { get; set; }
        public int ObjectStatus { get; set; }
        public string Name { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
