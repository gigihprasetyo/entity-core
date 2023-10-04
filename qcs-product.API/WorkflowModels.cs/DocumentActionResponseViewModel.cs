using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using qcs_product.API.Models;

namespace qcs_product.API.WorkflowModels
{
    [ExcludeFromCodeCoverage]
    public class DocumentActionResponseViewModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string CurrentStatusName { get; set; }
        public int TrackerId { get; set; }
        public int TrackerShiftId { get; set; }
        public bool IsFinish { get; set; }
        public List<DocumentActionViewModel> Actions { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DocumentActionViewModel
    {
        public int WorkflowActionId { get; set; }
        public int WorkflowStatusFrom { get; set; }
        public int WorkflowStatusTo { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public List<ActionOrg> ActionOrgs { get; set; }
        public double CompletionPolicyValue { get; set; }
        public int CompletionPolicyType { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ActionOrg
    {
        public string OrgId { get; set; }
        public int Action { get; set; }
        public int OrgType { get; set; }
        public string OrgName { get; set; }
    }
}