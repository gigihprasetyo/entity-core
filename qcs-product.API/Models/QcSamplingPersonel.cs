using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcSamplingPersonel
    {
        public int Id { get; set; }
        public int QcSamplingId { get; set; }
        public int? ProductProductionPhasesPersonelId { get; set; }
        public string PersonelNik { get; set; }
        public string PersonelName { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
