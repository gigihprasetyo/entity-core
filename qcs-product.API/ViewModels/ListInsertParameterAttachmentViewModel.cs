using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.BusinessProviders
{
    [ExcludeFromCodeCoverage]
    public class ListInsertParameterAttachmentViewModel
    {
        public string Filename { get; set; }
        public string MediaLink { get; set; }
        public string Ext { get; set; }
    }
}
