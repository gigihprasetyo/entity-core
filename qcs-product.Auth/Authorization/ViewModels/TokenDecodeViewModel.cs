namespace qcs_product.AuthAndEventBus.Authorization.ViewModels
{
    public class TokenDecodeViewModel
    {
        public bool IsValid { set; get; }
        public string Message { set; get; }
        public string PositionId { set; get; }
        public string NIK { set; get; }
        public string OrganizationId { set; get; }
    }
}
