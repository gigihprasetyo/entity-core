using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class FacilityAHUViewModel
    {
        public int Id { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationName { get; set; }
        public Int32 BIOHROrganizationId { get; set; }
        public List<ToolsAHUViewModel> ToolsAHU { get; set; }
    }


    public partial class ToolsAHUViewModel
    {
        public int ToolId { get; set; }
        public string ToolCode { get; set; }
        public string ToolName { get; set; }
    }
}
