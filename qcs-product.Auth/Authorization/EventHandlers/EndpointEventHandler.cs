using Microsoft.Extensions.Logging;
using qcs_product.Auth.Authorization.Constants;
using qcs_product.Auth.Authorization.DataProviders;
using qcs_product.Auth.Authorization.Infrastructure;
using qcs_product.Auth.Authorization.Models;
using qcs_product.EventBus.EventBus.Base.Abstractions;
using qcs_product.EventBus.IntegrationEvents;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace qcs_product.Auth.Authorization.EventHandlers
{
    public class EndpointEventHandler : IIntegrationEventHandler<EndPointIntegrationEvent>
    {
        private readonly ILogger<EndpointEventHandler> _logger;
        private readonly EndpointDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public EndpointEventHandler(
            ILogger<EndpointEventHandler> logger,
            q100_authorizationContext context
        )
        {
            _logger = logger;
            _dataProvider = new EndpointDataProvider(context);
        }

        /// <summary>
        /// </summary>
        /// handle endpoint integration event
        public async Task Handle(EndPointIntegrationEvent @event)
        {
            try
            {
                switch (@event.Operation)
                {
                    case Q100AUAMAuthorizationConstant.INSERT_OPERATION:
                        Endpoint insertedData = new Endpoint()
                        {
                            ApplicationCode = @event.ApplicationCode,
                            EndpointCode = @event.EndPointCode,
                            EndpointName = @event.EndPointName,
                            EndpointPath = @event.EndPointPath,
                            BeginDate = @event.BeginDate,
                            EndDate = @event.EndDate,
                            CreatedAt = @event.CreatedAt,
                            CreatedBy = @event.CreatedBy,
                            UpdatedAt = @event.UpdatedAt,
                            UpdatedBy = @event.UpdatedBy
                        };
                        Endpoint insertedDataResult = await _dataProvider.Insert(insertedData);
                        break;
                    case Q100AUAMAuthorizationConstant.UPDATE_OPERATION:
                        Endpoint updatedData = new Endpoint()
                        {
                            ApplicationCode = @event.ApplicationCode,
                            EndpointCode = @event.EndPointCode,
                            EndpointName = @event.EndPointName,
                            EndpointPath = @event.EndPointPath,
                            BeginDate = @event.BeginDate,
                            EndDate = @event.EndDate,
                            CreatedAt = @event.CreatedAt,
                            CreatedBy = @event.CreatedBy,
                            UpdatedAt = @event.UpdatedAt,
                            UpdatedBy = @event.UpdatedBy
                        };
                        Endpoint updatedDataResult = await _dataProvider.Update(updatedData);
                        break;
                    default:
                        Endpoint deletedData = new Endpoint()
                        {
                            ApplicationCode = @event.ApplicationCode,
                            EndpointCode = @event.EndPointCode,
                            EndpointName = @event.EndPointName,
                            EndpointPath = @event.EndPointPath,
                            BeginDate = @event.BeginDate,
                            EndDate = @event.EndDate,
                            CreatedAt = @event.CreatedAt,
                            CreatedBy = @event.CreatedBy,
                            UpdatedAt = @event.UpdatedAt,
                            UpdatedBy = @event.UpdatedBy
                        };
                        Endpoint deletedDataResult = await _dataProvider.Update(deletedData);
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Q100 Library Endpoint Event Handler {Message}", e.Message);
            }
        }
    }
}