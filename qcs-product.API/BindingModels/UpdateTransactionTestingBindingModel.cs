using System;

namespace qcs_product.API.BindingModels
{
    public class UpdateTransactionTestingBindingModel
    {
        public int Id { get; set; }
        public DateTime TestingDate { get; set; }
        public int ObjectStatus { get; set; }
        public string UpdatedBy { get; set; }
        public string TestTypeNameIdn { get; set; }
        public string TestTypeNameEn { get; set; }
        public string TestTypeCode { get; set; }
        public int TestTypeId { get; set; }
        public string TestTypeMethodName { get; set; }
        public string TestTypeMethodCode { get; set; }
        public int TestTypeMethodId { get; set; }
        public int? TestTemplateId { get; set; }
    }
}
