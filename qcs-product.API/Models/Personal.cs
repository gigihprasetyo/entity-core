using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class Personal
    {
        public Int32 Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Nik { get; set; }
        public Int32 OrgId { get; set; }
        public Int32 PosId { get; set; }
        public string Pin { get; set; }
        public string Email { get; set; }
        public string NoHandphone { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
