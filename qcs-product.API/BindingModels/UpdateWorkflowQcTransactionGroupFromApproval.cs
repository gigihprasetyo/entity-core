using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    public class UpdateWorkflowQcTransactionGroupFromApproval
    {
        public int TransactionGroupId { get; set; }
        public string WorkflowStatus { get; set; }
        public bool IsInWorkflow { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
    }
}
