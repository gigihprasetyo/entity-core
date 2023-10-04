using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public partial class WorkflowHistoryQcSampling
    {
        public string Action { get; set; }
        public string Note { get; set; }
        public string PersonalName { get; set; }
        public string PersonalNik { get; set; }
        public string Position { get; set; }
        public string DateTime { get; set; }
        public string ChangeStatusTime { get; set; }
    }
}