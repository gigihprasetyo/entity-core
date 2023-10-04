using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using qcs_product.API.BusinessProviders;

namespace qcs_product.API.BindingModels
{
    /// <summary>
    /// for parsing user input data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class InsertParameterAttachmentBindingModel
    {
        [Required]
        public int ProcedureParameterId { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        public string Note { get; set; }
        public List<ListInsertParameterAttachmentViewModel> Attachments { get; set; }
    }
}
