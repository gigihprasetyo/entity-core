using System.Diagnostics.CodeAnalysis;

namespace qcs_product.Auth.Authorization.SettingModels
{
    [ExcludeFromCodeCoverage]
    public class Q100AuthorizationSetting
    {
        public string AccessTokenSecret { get; set; }
        public string ApplicationCode { get; set; }
        public string AUAMServiceURL { get; set; }
    }
}