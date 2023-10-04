using System.Collections.Generic;

namespace qcs_product.API.BindingModels
{
    public class InsertBatchRequestQcBindingModel
    {
        public string AttachmentNotes { get; set; }
        public List<InsertBatchLineRequestQcBindingModel> Lines { get; set; }
        public List<InsertBatchAttachmentRequestQcBindingModel> Attachments { get; set; }
    }
}
