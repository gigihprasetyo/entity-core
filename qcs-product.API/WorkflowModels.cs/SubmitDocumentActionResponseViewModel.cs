using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.WorkflowModels
{
    [ExcludeFromCodeCoverage]
    public class SubmitDocumentActionResponseViewModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsFinish { get; set; }
        public DocumentStatusViewModel CurrentStatus { get; set; }
        public DocumentStatusViewModel PreviousStatus { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DocumentStatusViewModel
    {
        public string CurrentStatusName { get; set; }
        public int TrackerId { get; set; }
    }
}