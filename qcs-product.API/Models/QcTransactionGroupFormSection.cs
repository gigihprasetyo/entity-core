using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcTransactionGroupFormSection
    {
        public Int32 Id { get; set; }
        public Int32 SectionId { get; set; }
        public Int32 SectionTypeId { get; set; }
        public Int32 QcProcessId { get; set; }
        public int Sequence { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
