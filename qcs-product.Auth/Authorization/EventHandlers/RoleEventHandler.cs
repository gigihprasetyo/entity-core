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
    public class RoleEventHandler : IIntegrationEventHandler<RoleIntegrationEvent>
    {
        private readonly ILogger<RoleEventHandler> _logger;
        private readonly RoleDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public RoleEventHandler(
            ILogger<RoleEventHandler> logger,
            q100_authorizationContext context
        )
        {
            _logger = logger;
            _dataProvider = new RoleDataProvider(context);
        }

        /// <summary>
        /// </summary>
        /// handle endpoint integration event
        public async Task Handle(RoleIntegrationEvent @event)
        {
            try
            {
                switch (@event.Operation)
                {
                    case Q100AUAMAuthorizationConstant.INSERT_OPERATION:
                        Role insertedData = new Role()
                        {
                            ApplicationCode = @event.ApplicationCode,
                            RoleCode = @event.RoleCode,
                            RoleName = @event.RoleName,
                            BeginDate = @event.BeginDate,
                            EndDate = @event.EndDate,
                            CreatedAt = @event.CreatedAt,
                            CreatedBy = @event.CreatedBy,
                            UpdatedAt = @event.UpdatedAt,
                            UpdatedBy = @event.UpdatedBy
                        };
                        Role insertedDataResult = await _dataProvider.Insert(insertedData);
                        break;
                    case Q100AUAMAuthorizationConstant.UPDATE_OPERATION:
                        Role updatedData = new Role()
                        {
                            ApplicationCode = @event.ApplicationCode,
                            RoleCode = @event.RoleCode,
                            RoleName = @event.RoleName,
                            BeginDate = @event.BeginDate,
                            EndDate = @event.EndDate,
                            CreatedAt = @event.CreatedAt,
                            CreatedBy = @event.CreatedBy,
                            UpdatedAt = @event.UpdatedAt,
                            UpdatedBy = @event.UpdatedBy
                        };
                        Role updatedDataResult = await _dataProvider.Update(updatedData);
                        break;
                    default:
                        Role deletedData = new Role()
                        {
                            ApplicationCode = @event.ApplicationCode,
                            RoleCode = @event.RoleCode,
                            RoleName = @event.RoleName,
                            BeginDate = @event.BeginDate,
                            EndDate = @event.EndDate,
                            CreatedAt = @event.CreatedAt,
                            CreatedBy = @event.CreatedBy,
                            UpdatedAt = @event.UpdatedAt,
                            UpdatedBy = @event.UpdatedBy
                        };
                        Role deletedDataResult = await _dataProvider.Update(deletedData);
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Q100 Library Role Event Handler {Message}", e.Message);
            }
        }
    }
}