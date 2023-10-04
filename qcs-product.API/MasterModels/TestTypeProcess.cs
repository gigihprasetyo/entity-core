using System;

namespace qcs_product.API.MasterModels
{
    public class TestTypeProcess
    {
        public int? Id { get; set; }
        public string MethodCode { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
        public int? ParentTestTypeProcessId { get; set; }
        public bool IsEachSample { get; set; }
        public int? TestTypeMethodId { get; set; }
    }
}
