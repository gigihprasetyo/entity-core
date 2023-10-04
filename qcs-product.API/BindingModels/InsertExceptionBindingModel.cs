using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.BindingModels
{
    /// <summary>
    /// for parsing user input data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class InsertExceptionBindingModel
    {
        [Required]
        public List<ExceptionBindingModel> Parameters { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public partial class ExceptionBindingModel
    {
        [Required]
        public int ProcedureParameterId { get; set; }
        public string ExceptionNote { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        public int DeviationLevel { get; set; }
        public List<AttachmentException> Attachments { get; set; }

    }
    public partial class AttachmentException
    {
        [Required]
        public string Filename { get; set; }
        [Required]
        public string MediaLink { get; set; }
        [Required]
        public string Ext { get; set; }

    }
}
