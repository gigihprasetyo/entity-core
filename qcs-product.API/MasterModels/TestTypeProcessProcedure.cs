using System;

namespace qcs_product.API.MasterModels
{
    public class TestTypeProcessProcedure
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Instruction { get; set; }
        public int? TestTypeProcessId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
        public string AttachmentFile { get; set; }
        public string AttachmentStorageName { get; set; }
    }
}
