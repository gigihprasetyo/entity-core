using System.Collections.Generic;

namespace qcs_product.API.ViewModels
{
    public class TransactionBatchViewModel
    {
        public int Id { get; set; }
        public string AttachmentNotes { get; set; }
        
        public int RequestId { get; set; }
        public List<TransactionBatchLineViewModel> Lines { get; set; }
        public List<TransactionBatchAttachmentViewModel> Attachments { get; set; }
    }
}
