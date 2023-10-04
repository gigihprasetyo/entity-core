using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionRoomSamplingPointLayout
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string AttachmentFile { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string FileType { get; set; }
        public int? RoomPurposeId { get; set; }
    }
}
