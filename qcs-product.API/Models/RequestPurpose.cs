using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class RequestPurpose
    {
        public Int32 Id { get; set; }
        public Int32 QcRequestId { get; set; }
        public Int32 PurposeId { get; set; }
        public string PurposeCode { get; set; }
        public string PurposeName { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
