using System;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public partial class UserPresenceViewModel
    {
        public int PosId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string TestingCode { get; set; }
        public int TestingId { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
    }
}
