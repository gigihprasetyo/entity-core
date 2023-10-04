using System;
using System.Collections.Generic;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class WorkflowIntegrationEvent : IntegrationEvent
    {
        public int DataId { get; set; }
        public string Operation { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Version { get; set; }
        public int? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? ActiveOn { get; set; }
        public List<WorkflowStatusIntegrationEvent> WorkflowStatuses { get; set; }
    }

    public class WorkflowStatusIntegrationEvent
    {
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? Sequence { get; set; }
        public string WorkflowCode { get; set; }
        public string Code { get; set; }
        public string StatusCode { get; set; }
        public WorkflowStatusMasterIntegrationEvent Status { get; set; }
        public List<WorkflowActionIntegrationEvent> WorkflowActions { get; set; }
    }

    public class WorkflowActionIntegrationEvent
    {
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public double? CompletionValue { get; set; }
        public int? CompletionPolicy { get; set; }
        public string WorkflowStatusFromCode { get; set; }
        public string WorkflowStatusToCode { get; set; }
        public string ActionCode { get; set; }
        public string Code { get; set; }
        public WorkflowActionMasterIntegrationEvent Action { get; set; }
        public List<WorkflowActionOrgIntegrationEvent> WorkflowActionOrgs { get; set; }
    }

    public class WorkflowActionOrgIntegrationEvent
    {
        public string OrgId { get; set; }
        public string Parameter { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? OrgType { get; set; }
        public string OrgName { get; set; }
        public string WorkflowActionCode { get; set; }
        public string ActionCode { get; set; }
        public string Code { get; set; }
    }

    public class WorkflowStatusMasterIntegrationEvent
    {
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class WorkflowActionMasterIntegrationEvent
    {
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}