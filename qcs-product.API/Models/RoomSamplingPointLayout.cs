using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class RoomSamplingPointLayout
    {
        public Int32 Id { get; set; }
        //public Int32 RoomId { get; set; }
        public Int32 RoomPurposeId { get; set; }
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
