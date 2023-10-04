using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.Models
{
    [ExcludeFromCodeCoverage]
    [Keyless]
    public partial class NowTimestamp
    {
        public DateTime CurrentTimestamp { get; set; }
    }
}
