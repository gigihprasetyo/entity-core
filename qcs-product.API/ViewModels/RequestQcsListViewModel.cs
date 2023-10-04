using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class RequestQcsListViewModel
    {
        public Int32 Id { get; set; }
        public DateTime Date { get; set; }
        public string NoRequest { get; set; }
        public int? TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public string NoBatch { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? TypeFormId { get; set; }
        public string TypeFormName { get; set; }
        public int Status { get; set; }
        public string WorkflowStatus { get; set; }
        public int? OrgId { get; set; }
        public string OrgName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
