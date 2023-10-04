using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.WorkflowModels
{
    [ExcludeFromCodeCoverage]
    public class NewWorkflowDocument
    {
        [Required]
        public string DocumentCode { get; set; }
        [Required]
        public string ApplicationCode { get; set; }
        [Required]
        public string WorkflowCode { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string CreatorOrgId { get; set; }
    }
}