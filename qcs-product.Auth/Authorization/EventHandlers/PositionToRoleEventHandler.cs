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
    public class PositionToRoleEventHandler : IIntegrationEventHandler<PositionToRoleIntegrationEvent>
    {
        private readonly ILogger<PositionToRoleEventHandler> _logger;
        private readonly RoleDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public PositionToRoleEventHandler(
            ILogger<PositionToRoleEventHandler> logger,
            q100_authorizationContext context
        )
        {
            _logger = logger;
            _dataProvider = new RoleDataProvider(context);
        }

        /// <summary>
        /// </summary>
        /// handle endpoint integration event
        public async Task Handle(PositionToRoleIntegrationEvent @event)
        {
            try
            {
                PositionToRole currentData = await _dataProvider.GetPositionToRoleById(@event.DataId);
                switch (@event.Operation)
                {
                    case Q100AUAMAuthorizationConstant.INSERT_OPERATION:
                        if (currentData == null)
                        {
                            PositionToRole insertedData = new PositionToRole()
                            {
                                Id = @event.DataId,
                                ApplicationCode = @event.ApplicationCode,
                                RoleCode = @event.RoleCode,
                                PosId = @event.PosId,
                                Name = @event.Name,
                                CreatedAt = @event.CreatedAt,
                                CreatedBy = @event.CreatedBy,
                                UpdatedAt = @event.UpdatedAt,
                                UpdatedBy = @event.UpdatedBy,
                                RowStatus = @event.RowStatus
                            };
                            PositionToRole insertedDataResult = await _dataProvider.InsertPositionToRole(insertedData);
                        }
                        break;
                    default:
                        if (currentData != null)
                        {
                            currentData.ApplicationCode = @event.ApplicationCode;
                            currentData.RoleCode = @event.RoleCode;
                            currentData.PosId = @event.PosId;
                            currentData.Name = @event.Name;
                            currentData.CreatedAt = @event.CreatedAt;
                            currentData.CreatedBy = @event.CreatedBy;
                            currentData.UpdatedAt = @event.UpdatedAt;
                            currentData.UpdatedBy = @event.UpdatedBy;
                            currentData.RowStatus = @event.RowStatus;
                            PositionToRole updatedDataResult = await _dataProvider.UpdatePositionToRole(currentData);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Q100 Library Position to Role Event Handler {Message}", e.Message);
            }
        }
    }
}