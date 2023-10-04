using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionSamplingPoint
    {
        public TransactionSamplingPoint()
        {
            TransactionRelRoomSamplingPoint = new HashSet<TransactionRelRoomSamplingPoint>();
            TransactionRelSamplingTestParam = new HashSet<TransactionRelSamplingTestParam>();
            TransactionRelSamplingTool = new HashSet<TransactionRelSamplingTool>();
        }

        public int Id { get; set; }
        public int? RoomId { get; set; }
        public int? ToolId { get; set; }
        public string Code { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<TransactionRelRoomSamplingPoint> TransactionRelRoomSamplingPoint { get; set; }
        public virtual ICollection<TransactionRelSamplingTestParam> TransactionRelSamplingTestParam { get; set; }
        public virtual ICollection<TransactionRelSamplingTool> TransactionRelSamplingTool { get; set; }
    }
}
