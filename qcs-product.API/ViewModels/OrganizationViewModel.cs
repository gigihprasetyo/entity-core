using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public partial class OrganizationViewModel
    {
        public int Id { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int BIOHROrganizationId { get; set; }
        public string ObjectId { get; set; }
        public string Modul { get; set; }
        public string StatusObject { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}