using System;

namespace qcs_product.API.WorkflowModels.cs
{
    public partial class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Code { get; set; }
    }
}
