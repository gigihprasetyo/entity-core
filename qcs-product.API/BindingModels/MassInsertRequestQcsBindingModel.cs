using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System;
namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class MassInsertRequestQcsBindingModel
    {
        [Required]
        public Int32 PurposeId { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public Int32 FacilityId { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public List<MassListRoomRequestQc> MassRequestRooms { get; set; }
        public class MassListRoomRequestQc
        {
            [Required]
            public List<MassRoomRequestQc> RequestRooms { get; set; }
        }
        public partial class MassRoomRequestQc
        {
            [Required]
            public Int32 RoomId { get; set; }
        }
    }
}