using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using qcs_product.API.WorkflowModels;

namespace qcs_product.API.WorkflowModels
{
    [ExcludeFromCodeCoverage]
    public class DocumentPICResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string CurrentStatusName { get; set; }
        public int TrackerId { get; set; }
        public int CurrentWorkflowStatusId { get; set; }
        public int TrackerShiftId { get; set; }
        public bool IsFinish { get; set; }
        public List<DocumentActionViewModel> Actions { get; set; }
        public List<DocumentPICViewModel> PICs { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DocumentPICViewModel
    {
        public int WorkflowActionId { get; set; }
        public string orgId { get; set; }
        public string orgName { get; set; }
        public string orgPositionId { get; set; }
        public string Notes { get; set; }
    }
}