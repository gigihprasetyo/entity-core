using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.IntegrationEvents;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;

namespace qcs_product.API.EventHandlers
{
    public class BuildingEventHandler : IIntegrationEventHandler<BuildingIntegrationEvent>
    {
        private readonly ILogger<BuildingEventHandler> _logger;
        private readonly IBuildingDataProvider _buildingDataProvider;

        public BuildingEventHandler(ILogger<BuildingEventHandler> logger, IBuildingDataProvider buildingDataProvider)
        {
            _logger = logger;
            _buildingDataProvider = buildingDataProvider;
        }

        public async Task Handle(BuildingIntegrationEvent @event)
        {
            _logger.LogInformation("sync building from google pub/sub");
            try
            {
                _logger.LogInformation(JsonSerializer.Serialize(@event));
                
                _logger.LogInformation("insert or update building");
                var isNew = false;
                var building = await _buildingDataProvider.GetByCode(@event.Code);
                
                if (building == null)
                {
                    isNew = true;
                    building = new Building();
                    building.Code = @event.Code;
                    building.CreatedAt = DateTime.Now;
                    building.CreatedBy = @event.CreatedBy;
                }


                building.Name = @event.Name;
                building.UpdatedAt = DateTime.Now;
                building.UpdatedBy = @event.UpdatedBy;
                building.RowStatus = @event.RowStatus;

                if (isNew)
                {
                    await _buildingDataProvider.Insert(building);
                }
                else
                {
                    await _buildingDataProvider.Update(building);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
            
        }
    }
}