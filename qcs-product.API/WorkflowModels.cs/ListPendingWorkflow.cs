using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.WorkflowModels
{

    [ExcludeFromCodeCoverage]
    public class ListPendingWorkflow
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<ListReviewPending> ListPending { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ListReviewPending
    {
        public int WorkflowId { get; set; }
        public string RecordId { get; set; }
        public string ApplicationCode { get; set; }
        public string StatusName { get; set; }
        public int WorkflowTrackerShiftId { get; set; }
        public int WorkflowStatusId { get; set; }
        public string Notes { get; set; }
        public string OrgId { get; set; }
        public int WorkflowActionId { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}