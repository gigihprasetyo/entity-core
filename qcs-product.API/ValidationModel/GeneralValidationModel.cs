using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.ValidationModels
{
    [ExcludeFromCodeCoverage]
    public partial class GeneralValidationModel
    {
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; }
    }
}