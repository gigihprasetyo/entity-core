using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class WorkflowSubmitBindingModel
    {
        [Required]
        public string ApplicationCode { get; set; }
        [Required]
        public string DocumentCode { get; set; }
        [Required]
        public string Notes { get; set; }
        [Required]
        public string OrgId { get; set; }
        [Required]
        public string ActionName { get; set; }
        public List<string> NextPICOrgIdList { get; set; }
    }
}
