using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.ViewModels
{
    public partial class RequestGroupToolPurposeViewModel
    {
        public int Id { get; set; }
        public int RequestQcsId { get; set; }
        public int? ToolPurposeId { get; set; }
    }
}
