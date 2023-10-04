using System;
using System.Collections.Generic;
namespace qcs_product.API.Models
{
    public class RequestQcsBulk
    {
        public string NoRequest { get; set; }
        public int? PurposeId { get; set; }
        public string PurposeName { get; set; }
        public Int32? FacilityId { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public virtual ICollection<RequestQcs> RequestQcs { get; set; }
    }
}