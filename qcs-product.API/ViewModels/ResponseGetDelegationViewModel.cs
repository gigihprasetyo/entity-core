using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class ResponseGetDelegationViewModel
    {
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string PersonalNumber { get; set; }
        public string FromPositionId { get; set; }
        public string FromPositionCode { get; set; }
        public string FromPositionName { get; set; }
        public string FromOrganizationName { get; set; }
        public string PersonalNumberTo { get; set; }
        public string ToPositioId { get; set; }
        public string ToPositionCode { get; set; }
        public string ToPositionName { get; set; }
        public string ToPersonalName { get; set; }
        public string ToOrganizationName { get; set; }
    }
}
