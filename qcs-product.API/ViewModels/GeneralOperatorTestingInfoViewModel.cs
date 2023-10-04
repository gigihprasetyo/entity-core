using qcs_product.API.Models;
using System.Collections.Generic;

namespace qcs_product.API.ViewModels
{
    public class GeneralOperatorTestingInfoViewModel
    {
        public bool IsPersonnelSaved { get; set; }
        public bool IsAttachmentSaved { get; set; }
        public List<TransactionTestingPersonnel> listPersonnel { get; set; }
        public List<TransactionHtrTestingPersonnel> listHtrPersonnel { get; set; }
        public List<TransactionTestingNote> listNote { get; set; }
        public List<TransactionHtrTestingNote> listHtrNote { get; set; }
        public List<TransactionTestingAttachment> listAttachment { get; set; }
        public List<TransactionHtrTestingAttachment> listHtrAttachment { get; set; }
    }
}
