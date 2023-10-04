using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcLabelRequestViewModel
    {
        public Int32 QcRequestId { get; set; }
        public string NoRequest { get; set; }
        public string NoBatch { get; set; }
        public string TypeRequest { get; set; }
        public string ItemName { get; set; }
        public string EmRoomName { get; set; }
        public string EmRoomGradeName { get; set; }
        public string EmPhaseName { get; set; }
        public string TestScenarioName { get; set; }
        public string TestScenarioLabel { get; set; }
        public string CreatedBy { get; set; }

    }
}
