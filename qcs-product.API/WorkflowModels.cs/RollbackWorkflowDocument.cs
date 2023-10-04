using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.WorkflowModels
{
    [ExcludeFromCodeCoverage]
    public class RollbackWorkflowDocument
    {
        [Required]
        public string DocumentCode { get; set; }
        [Required]
        public string ApplicationCode { get; set; }
    }
}