using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcSamplingAttachment : BaseEntity
    {
        public Int32 Id { get; set; }
        public Int32 QcSamplingId { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentFileLink { get; set; }
        public string AttachmentStorageName { get; set; }
        public string RowStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
