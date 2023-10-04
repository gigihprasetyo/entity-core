using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.ViewModels
{
    public class RoomDetailViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { set; get; }

        public int? GradeRoomId { get; set; }


        public int OrganizationId { get; set; }
        public int BuildingId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public int? Area { get; set; }
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
        public string PosId { get; set; }
        public List<PurposesDatas> ListDataPurposes { get; set; }
    }

    public partial class PurposesDatas
    {
        public int Id { get; set; }
        public List<Purpose> Purpose { get; set; }
        public List<RoomSamplingPointViewModel> SamplingPoints { get; set; }
        public List<RoomSamplingPointLayout> SamplingPointLayout { get; set; }
    }

}
