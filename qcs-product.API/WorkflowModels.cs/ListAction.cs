using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using qcs_product.API.WorkflowModels;

namespace qcs_product.API.WorkflowModels
{
    [ExcludeFromCodeCoverage]
    public class ListAction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        [NotMapped]
        public string CreatedAt { get; set; }
        [NotMapped]
        public string UpdatedAt { get; set; }
    }
}
