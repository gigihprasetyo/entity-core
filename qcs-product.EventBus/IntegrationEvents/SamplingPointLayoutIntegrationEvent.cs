using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class SamplingPointLayoutIntegrationEvent
    {
        public int DataId { get; set; }
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public string AttachmentFile { get; set; }
        public string FileName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string RowStatus { get; set; }
        public string FileType { get; set; }
    }
}
