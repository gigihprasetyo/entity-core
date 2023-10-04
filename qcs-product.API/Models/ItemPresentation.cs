using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class ItemPresentation
    {
        public Int32 Id { get; set; }
        public string ItemPresentationCode { get; set; }
        public string ItemPresentationName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string RowStatus { get; set; }
    }
}
