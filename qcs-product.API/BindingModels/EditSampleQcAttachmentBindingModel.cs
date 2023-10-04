using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class EditSampleQcAttachmentBindingModel
    {
        public Int32? Id { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentFileLink { get; set; }
        public string AttachmentStorageName { get; set; }
    }
}
