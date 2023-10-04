using qcs_product.API.Models;
using System.Collections.Generic;

namespace qcs_product.API.ViewModels
{
    public class InsertTestingAttachmentViewModel
    {
        public List<TransactionTestingAttachment> Attachment { get; set; }
        public List<TransactionHtrTestingAttachment> AttachmentHistory { get; set; }
    }
}
