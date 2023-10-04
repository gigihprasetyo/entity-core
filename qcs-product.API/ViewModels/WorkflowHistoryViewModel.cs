using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class WorkflowHistoryViewModel
    {
        public Int32 Id { get; set; }
        public string Action { get; set; }
        public string Note { get; set; }
        public Int32 GroupMenuId { get; set; }
        public string GroupMenuName { get; set; }
        public string PersonalName { get; set; }
        public string PersonalNik { get; set; }
        public string Position { get; set; }
        public DateTime CreatedAt { get; set; }
        
    }
}
