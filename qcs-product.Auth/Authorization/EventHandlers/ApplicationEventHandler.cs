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
    public class ApplicationEventHandler : IIntegrationEventHandler<ApplicationIntegrationEvent>
    {
        // private readonly q100_authorizationContext _context;
        private readonly ILogger<ApplicationEventHandler> _logger;
        private readonly ApplicationDataProvider _dataProvider;

        [ExcludeFromCodeCoverage]
        public ApplicationEventHandler(
            ILogger<ApplicationEventHandler> logger,
            q100_authorizationContext context
        )
        {
            // _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
            _dataProvider = new ApplicationDataProvider(context);
        }

        /// <summary>
        /// </summary>
        /// handle application integration event
        public async Task Handle(ApplicationIntegrationEvent @event)
        {
            try
            {
                switch (@event.Operation)
                {
                    case Q100AUAMAuthorizationConstant.INSERT_OPERATION:
                        Application insertedData = new Application()
                        {
                            ApplicationCode = @event.ApplicationCode,
                            ApplicationName = @event.ApplicationName,
                            BeginDate = @event.BeginDate,
                            EndDate = @event.EndDate,
                            CreatedAt = @event.CreatedAt,
                            CreatedBy = @event.CreatedBy,
                            UpdatedAt = @event.UpdatedAt,
                            UpdatedBy = @event.UpdatedBy
                        };
                        Application insertedDataResult = await _dataProvider.Insert(insertedData);
                        break;
                    case Q100AUAMAuthorizationConstant.UPDATE_OPERATION:
                        Application updatedData = new Application()
                        {
                            ApplicationCode = @event.ApplicationCode,
                            ApplicationName = @event.ApplicationName,
                            BeginDate = @event.BeginDate,
                            EndDate = @event.EndDate,
                            CreatedAt = @event.CreatedAt,
                            CreatedBy = @event.CreatedBy,
                            UpdatedAt = @event.UpdatedAt,
                            UpdatedBy = @event.UpdatedBy
                        };
                        Application updatedDataResult = await _dataProvider.Update(updatedData);
                        break;
                    default:
                        Application deletedData = new Application()
                        {
                            ApplicationCode = @event.ApplicationCode,
                            ApplicationName = @event.ApplicationName,
                            BeginDate = @event.BeginDate,
                            EndDate = @event.EndDate,
                            CreatedAt = @event.CreatedAt,
                            CreatedBy = @event.CreatedBy,
                            UpdatedAt = @event.UpdatedAt,
                            UpdatedBy = @event.UpdatedBy
                        };
                        Application deletedDataResult = await _dataProvider.Update(deletedData);
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Q100 Library Application Event Handler {Message}", e.Message);
            }
        }
    }
}