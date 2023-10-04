using System;
using System.Collections.Generic;

namespace qcs_product.API.BindingModels
{
    public class InsertTransactionTestingBindingModel
    {
        public DateTime TestingDate { get; set; }
        public int ObjectStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TestTypeNameIdn { get; set; }
        public string TestTypeNameEn { get; set; }
        public string TestTypeCode { get; set; }
        public int TestTypeId { get; set; }
        public string TestTypeMethodName { get; set; }
        public string TestTypeMethodCode { get; set; }
        public int TestTypeMethodId { get; set; }
        public int? TestTemplateId { get; set; }
        public List<SamplingBindingModel> Samplings { get; set; }
    }
    public class SamplingBindingModel
    {
        public int SamplingId { get; set; }
        public string SamplingName { get; set; }
        public string Notes { get; set; }
        public string Attachment { get; set; }
        public string TestingCode { get; set; }
    }

}
