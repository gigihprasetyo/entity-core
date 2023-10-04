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
    public class MicrobaEventHandler : IIntegrationEventHandler<MicrobaIntegrationEvent>
    {
        private readonly ILogger<MicrobaEventHandler> _logger;
        private readonly IMicrofloraDataProvider _dataProvider;

        public MicrobaEventHandler(ILogger<MicrobaEventHandler> logger, IMicrofloraDataProvider dataProvider)
        {
            _logger = logger;
            _dataProvider = dataProvider;
        }

        public async Task Handle(MicrobaIntegrationEvent @event)
        {
            _logger.LogInformation("sync microflora from google pub/sub");
            try
            {
                _logger.LogInformation(JsonSerializer.Serialize(@event));

                _logger.LogInformation("insert or update microflora");
                var isNew = false;
                var microflora = await _dataProvider.GetByCode(@event.Code);

                if (microflora == null)
                {
                    isNew = true;
                    microflora = new Microflora();
                    microflora.Code = @event.Code;
                    microflora.CreatedAt = DateTime.Now;
                    microflora.CreatedBy = @event.CreatedBy;
                }

                microflora.Name = @event.MicrobaName;
                microflora.MicrobaId = @event.MicrobaId;
                microflora.ObjectStatus = @event.ObjectStatus;
                microflora.UpdatedAt = DateTime.Now;
                microflora.UpdatedBy = @event.UpdatedBy;
                microflora.RowStatus = @event.RowStatus;

                if (isNew)
                {
                    await _dataProvider.Insert(microflora);
                }
                else
                {
                    await _dataProvider.Update(microflora);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

        }
    }
}