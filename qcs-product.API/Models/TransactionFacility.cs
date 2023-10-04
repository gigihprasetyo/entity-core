using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionFacility
    {
        public TransactionFacility()
        {
            TransactionFacilityRoom = new HashSet<TransactionFacilityRoom>();
        }

        public int Id { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public int? OrganizationId { get; set; }

        public virtual TransactionOrganization Organization { get; set; }
        public virtual ICollection<TransactionFacilityRoom> TransactionFacilityRoom { get; set; }
    }
}
