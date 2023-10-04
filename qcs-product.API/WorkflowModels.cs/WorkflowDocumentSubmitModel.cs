using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace qcs_product.API.WorkflowModels
{
    [ExcludeFromCodeCoverage]
    public class WorkflowDocumentSubmitModel
    {
        [FromQuery(Name = "document_code")]
        public string DocumentCode { get; set; }
        [FromQuery(Name = "application_code")]
        public string ApplicationCode { get; set; }
        [FromQuery(Name = "workflow_action_id")]
        public int WorkflowActionId { get; set; }
        [FromQuery(Name = "org_id")]
        public string OrgId { get; set; }
        public string NextPICOrgId { get; set; }
        public List<string> NextPICOrgIdList { get; set; }
        [FromQuery(Name = "notes")]
        public string Notes { get; set; }
    }
}