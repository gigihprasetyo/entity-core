using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcTransactionGroupSample : BaseEntity
    {
        public Int32 QcTransactionGroupId { get; set; }
        public Int32? QcTransactionSamplingId { get; set; }
        public Int32? QcSamplingId { get; set; }
        public Int32 QcSampleId { get; set; }
        public string RowStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
