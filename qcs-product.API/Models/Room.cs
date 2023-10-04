using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class Room
    {
        public Int32 Id { get; set; }
        public Int32 GradeRoomId { get; set; }
        public Int32 OrganizationId { get; set; }
        public Int32 BuildingId { get; set; }
        public string Code { get; set; }
        public string PosId { get; set; }
        public string Name { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string Floor { get; set; }
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
        public int? Area { get; set; }
        public int ObjectStatus { get; set; }
    }
}
