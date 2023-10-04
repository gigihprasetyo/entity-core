using System;

namespace qcs_product.API.ViewModels
{
    public class ReviewKasieTemplateQCListViewModel
    {
        public int Id { set; get; }
        public string IdTemplate { set; get; }
        public string TemplateName { set; get; }
        public string TestTypeName { set; get; }
        public string MethodName { set; get; }
        public string ValidityPeriod { get; set; }
        public DateTime StartValidityPeriod { set; get; }
        public DateTime EndValidityPeriod { set; get; }
        public int Status { set; get; }
        public string StatusName { set; get; }
        public DateTime CreatedAt { set; get; }
        public DateTime UpdatedAt { set; get; }
        public int MethodId { set; get; }

    }
}
