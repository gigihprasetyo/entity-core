using System;

namespace qcs_product.API.Models
{
    public class Building
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}