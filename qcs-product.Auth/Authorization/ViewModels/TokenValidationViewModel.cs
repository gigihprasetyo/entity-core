using System.Diagnostics.CodeAnalysis;

namespace qcs_product.Auth.Authorization.ViewModels
{
    /// <summary>
    /// for API response
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class TokenValidationViewModel
    {
        public bool IsValid { set; get; }
        public string Message { set; get; }
        public string PositionId { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
    }
}
