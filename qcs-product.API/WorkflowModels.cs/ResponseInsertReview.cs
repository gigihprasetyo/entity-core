using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.WorkflowModels
{
    [ExcludeFromCodeCoverage]
    public class ResponseInsertReview
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string WorkflowStatus { get; set; }
    }
}