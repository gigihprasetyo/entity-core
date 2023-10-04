using qcs_product.API.Models;
using System.Collections.Generic;

namespace qcs_product.API.ViewModels
{
    public class TransactionTemplateTestingInfoViewModel
    {
        public List<TransactionTestingPersonnel> listPersonnel { get; set; }
        public List<TransactionTestingNote> listNote { get; set; }
        public List<TransactionTestingAttachment> listAttachment { get; set; }
        public List<TransactionTemplateTestTypeProcess> listProcess { get; set; }

    }
}
