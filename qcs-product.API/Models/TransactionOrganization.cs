using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionOrganization
    {
        public TransactionOrganization()
        {
            TransactionFacility = new HashSet<TransactionFacility>();
            TransactionRoom = new HashSet<TransactionRoom>();
            TransactionTestParameter = new HashSet<TransactionTestParameter>();
        }

        public int Id { get; set; }
        public string OrgCode { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? BiohrOrganizationId { get; set; }

        public virtual ICollection<TransactionFacility> TransactionFacility { get; set; }
        public virtual ICollection<TransactionRoom> TransactionRoom { get; set; }
        public virtual ICollection<TransactionTestParameter> TransactionTestParameter { get; set; }
    }
}
