using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class ResponseBioHRLoginViewModel
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string PositionId { get; set; }
        public string OrganizationId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string PositionName { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string Password { get; set; }
        public string Grade { get; set; }
        public string Email { get; set; }
        public string ApplicationCode { get; set; }
        public string Token { get; set; }
    }
}
