using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.SettingModels
{
    [ExcludeFromCodeCoverage]
    public class JWTSetting
    {
        public string AccessTokenSecret { get; set; }
        public string RefreshTokenSecret { get; set; }
        public double AccessTokenExpiredTime { get; set; }
        public double RefreshTokenExpiredTime { get; set; }
    }
}
