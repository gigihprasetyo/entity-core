using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace qcs_product.Auth.Authorization.ViewModels
{
    /// <summary>
    /// for parsing user input data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class IsAccessTokenActiveViewModel
    {
        [Required]
        public string AccessToken { get; set; }
    }
}
