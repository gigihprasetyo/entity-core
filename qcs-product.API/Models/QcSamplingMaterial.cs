using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcSamplingMaterial : BaseEntity
    {
        public Int32 QcSamplingId { get; set; }
        public Int32 ItemId { get; set; }
        public string ItemName { get; set; }
        public Int32? ItemBatchId { get; set; }
        public string NoBatch { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string RowStatus { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
