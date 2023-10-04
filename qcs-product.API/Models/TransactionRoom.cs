using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace qcs_product.API.Models
{
    public partial class TransactionRoom
    {
        public TransactionRoom()
        {
            TransactionFacilityRoom = new HashSet<TransactionFacilityRoom>();
        }

        public int Id { get; set; }
        public int GradeRoomId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? OrganizationId { get; set; }
        public string Floor { get; set; }
        public int? Area { get; set; }
        public int? TemperatureOperator { get; set; }
        public decimal? TemperatureValue { get; set; }
        public decimal? TemperatureValueTo { get; set; }
        public decimal? TemperatureValueFrom { get; set; }
        public int? HumidityOperator { get; set; }
        public decimal? HumidityValue { get; set; }
        public decimal? HumidityValueTo { get; set; }
        public decimal? HumidityValueFrom { get; set; }
        public int? PressureOperator { get; set; }
        public decimal? PressureValue { get; set; }
        public decimal? PressureValueTo { get; set; }
        public decimal? PressureValueFrom { get; set; }
        public int? AirChangeOperator { get; set; }
        public decimal? AirChangeValue { get; set; }
        public decimal? AirChangeValueTo { get; set; }
        public decimal? AirChangeValueFrom { get; set; }
        public int? Ahu { get; set; }
        public int? BuildingId { get; set; }
        public int? ObjectStatus { get; set; }
        public string PosId { get; set; }

        public virtual TransactionOrganization Organization { get; set; }
        public virtual ICollection<TransactionFacilityRoom> TransactionFacilityRoom { get; set; }
    }
}
