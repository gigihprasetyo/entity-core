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
    public class RoleToEndpointEventHandler : IIntegrationEventHandler<RoleToEndPointIntegrationEvent>
    {
        private readonly ILogger<RoleToEndpointEventHandler> _logger;
        private readonly EndpointDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public RoleToEndpointEventHandler(
            ILogger<RoleToEndpointEventHandler> logger,
            q100_authorizationContext context
        )
        {
            _logger = logger;
            _dataProvider = new EndpointDataProvider(context);
        }

        /// <summary>
        /// </summary>
        /// handle endpoint integration event
        public async Task Handle(RoleToEndPointIntegrationEvent @event)
        {
            try
            {
                RoleToEndpoint currentData = await _dataProvider.GetRoleToEndpointById(@event.DataId);
                switch (@event.Operation)
                {
                    case Q100AUAMAuthorizationConstant.INSERT_OPERATION:
                        if (currentData == null)
                        {
                            RoleToEndpoint insertedData = new RoleToEndpoint()
                            {
                                Id = @event.DataId,
                                ApplicationCode = @event.ApplicationCode,
                                RoleCode = @event.RoleCode,
                                EndpointCode = @event.EndPointCode,
                                CreatedAt = @event.CreatedAt,
                                CreatedBy = @event.CreatedBy,
                                UpdatedAt = @event.UpdatedAt,
                                UpdatedBy = @event.UpdatedBy,
                                RowStatus = @event.RowStatus
                            };
                            RoleToEndpoint insertedDataResult = await _dataProvider.InsertRoleToEndpoint(insertedData);
                        }
                        break;
                    default:
                        if (currentData != null)
                        {
                            currentData.ApplicationCode = @event.ApplicationCode;
                            currentData.RoleCode = @event.RoleCode;
                            currentData.EndpointCode = @event.EndPointCode;
                            currentData.CreatedAt = @event.CreatedAt;
                            currentData.CreatedBy = @event.CreatedBy;
                            currentData.UpdatedAt = @event.UpdatedAt;
                            currentData.UpdatedBy = @event.UpdatedBy;
                            currentData.RowStatus = @event.RowStatus;
                            RoleToEndpoint updatedDataResult = await _dataProvider.UpdateRoleToEndpoint(currentData);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Q100 Library Role to Endpoint Event Handler {Message}", e.Message);
            }
        }
    }
}