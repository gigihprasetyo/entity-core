using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionBuilding
    {
        public int Id { get; set; }
        public string BuildingId { get; set; }
        public string BuildingName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
