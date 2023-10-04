using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class ResponseGetNikByOrganizationIdAndPositionType
    {
        public string UserId { get; set; }
        public string NewUserId { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string PositionType { get; set; }
    }
}
