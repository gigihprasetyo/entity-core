using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    public class UpdateQcTransactionGroupFromApproval
    {
        public int TransactionGroupId { get; set; }
        public string WorkflowStatus { get; set; }
        public int Status { get; set; }
        public string UpdatedBy { get; set; }
    }
}
