using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public partial class InsertNotificationResponseViewModel
    {
        [JsonPropertyName("message")]
        public string message { get; set; }
        public IEnumerable Data { set; get; }
    }
}
