using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionRelRoomSamplingPoint
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int SamplingPoinId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? RoomPurposeId { get; set; }
        public string ScenarioLabel { get; set; }

        public virtual TransactionSamplingPoint SamplingPoin { get; set; }
    }
}
