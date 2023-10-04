using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class ResponseGetOrganizationBioHRViewModel
    {
        public int OrganizationId { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationType { get; set; }

    }
}
