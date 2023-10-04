using qcs_product.API.Models;
using System.Collections.Generic;

namespace qcs_product.API.ViewModels
{
    public class TemplateTestingInfoViewModel
    {
        public List<TemplateTestingPersonnel> listPersonnel { get; set; }

        public List<TemplateTestingNote> listNote { get; set; }

        public List<TemplateTestingAttachment> listAttachment { get; set; }
    }
}
