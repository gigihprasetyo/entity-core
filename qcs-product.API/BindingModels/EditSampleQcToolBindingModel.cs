using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class EditSampleQcToolBindingModel
    {
        public Int32? Id { get; set; }
        public Int32 ToolId { get; set; }
        public string ToolCode { get; set; }
        public string ToolName { get; set; }
        public Int32 ToolGroupId { get; set; }
        public string ToolGroupName { get; set; }
        public string ToolGroupLabel { get; set; }
        public DateTime? EdValidation { get; set; }
        public DateTime? EdCalibration { get; set; }
    }
}
