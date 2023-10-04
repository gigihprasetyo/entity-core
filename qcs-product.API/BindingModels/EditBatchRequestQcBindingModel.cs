using System.Collections.Generic;

namespace qcs_product.API.BindingModels
{
    public class EditBatchRequestQcBindingModel
    {
        public int? Id { get; set; }
        public string AttachmentNotes { get; set; }
        public List<EditBatchLineRequestQcBindingModel> Lines { get; set; }
        public List<EditBatchAttachmentRequestQcBindingModel> Attachments { get; set; }
    }
}
