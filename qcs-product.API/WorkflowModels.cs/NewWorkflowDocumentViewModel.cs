using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using qcs_product.API.WorkflowModels;

namespace qcs_product.API.WorkflowModels
{
    [ExcludeFromCodeCoverage]
    public class NewWorkflowDocumentViewModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StatusName { get; set; }
        public int TrackerId { get; set; }
        public List<ListAction> Actions { get; set; }
    }
}
