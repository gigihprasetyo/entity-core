using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.IntegrationEvents;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.Constants;

namespace qcs_product.API.EventHandlers
{
    public class DigitalSignatureEventHandler : IIntegrationEventHandler<DigitalSignatureIntegrationEvent>
    {
        private readonly ILogger<DigitalSignatureEventHandler> _logger;
        private readonly IDigitalSignatureDataProvider _dataProvider;

        public DigitalSignatureEventHandler(ILogger<DigitalSignatureEventHandler> logger, IDigitalSignatureDataProvider dataProvider)
        {
            _logger = logger;
            _dataProvider = dataProvider;
        }

        public async Task Handle(DigitalSignatureIntegrationEvent @event)
        {
            _logger.LogInformation("sync digital signature from google pub/sub");

            DateTime now = DateTime.UtcNow;
            DateTime nowTimestamp = now.AddHours(ApplicationConstant.TIMEZONE);

            try
            {
                DigitalSignature newDigitalSignatureData = new DigitalSignature
                {
                    Nik = @event.Nik,
                    SerialNumber = @event.SerialNumber,
                    CreatedAt = @event.CreatedAt,
                    CreatedBy = @event.CreatedBy,
                    UpdatedAt = @event.UpdatedAt,
                    UpdatedBy = @event.UpdatedBy,
                    BeginDate = @event.BeginDate,
                    EndDate = @event.EndDate
                };
                DigitalSignature newDigitalSignature = await _dataProvider.Insert(newDigitalSignatureData);

                //update old data
                DigitalSignature oldDigitalSignatureData = await _dataProvider.GetLastByNIKStep1(@event.UpdatedBy);
                if (oldDigitalSignatureData != null)
                {
                    oldDigitalSignatureData.EndDate = nowTimestamp.AddDays(-1);
                    oldDigitalSignatureData.UpdatedAt = nowTimestamp;
                    oldDigitalSignatureData.UpdatedBy = @event.UpdatedBy;
                    await _dataProvider.Update(oldDigitalSignatureData);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
        }
    }
}