using System;
using System.Collections.Generic;

namespace qcs_product.API.MasterModels
{
    public class TestTypeMethodValidationInstruction
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public int OrderStatic { get; set; }
        public int TestTypeMethodId { get; set; }
        public string TestTypeMethodCode { get; set; }
        public string TestTypeMethodName { get; set; }
        public string RowStatus { get; set; }
        public string Instructions { get; set; }
        public string AttachmentUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public virtual List<TestTypeMethodValidationParameter> Parameters { get; set; }
    }
}
