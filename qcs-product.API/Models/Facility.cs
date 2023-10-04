using System;

namespace qcs_product.API.Models
{
    public class Facility
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public int ObjectStatus { get; set; }
        public int OrganizationId { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}