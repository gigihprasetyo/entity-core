using System;
using System.Collections.Generic;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class RoomIntegrationEvent : IntegrationEvent
    {
        public int DataId { get; set; }

        public string Operation { get; set; }

        public string Code { get; set; }

        public string Name { set; get; }

        public int? GradeRoomId { get; set; }

        public string GradeRoomCode { get; set; }

        public string GradeRoomName { get; set; }

        public string OrganizationCode { get; set; }

        public string OrganizationName { get; set; }

        public int BIOHROrganizationId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public string RowStatus { get; set; }

        public int? Area { get; set; }
        public string ToolCode { get; set; }
        public string ToolName { get; set; }
        public string Floor { get; set; }
        public int? TemperatureOperator { get; set; }
        public decimal? TemperatureValue { get; set; }
        public decimal? TemperatureValueTo { get; set; }
        public decimal? TemperatureValueFrom { get; set; }
        public int ObjectStatus { get; set; }
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
        public GradeRoomIntegrationEvent GradeRoom { get; set; }
        public List<PurposesDataIntegrationEvent> ListDataPurposes { get; set; }
    }

    public partial class PurposesDataIntegrationEvent
    {
        public int DataId { get; set; }
        public List<ListMasterPurposeIntegrationEvent> Purpose { get; set; }
        public List<SamplingPointIntegrationEvent> SamplingPoints { get; set; }
        public List<SamplingPointLayoutIntegrationEvent> SamplingPointLayout { get; set; }
    }

    public partial class ListMasterPurposeIntegrationEvent
    {
        public int DataId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}