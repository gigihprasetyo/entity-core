using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Q100Library.Authorization.Constants;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.IntegrationEvents;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;

namespace qcs_product.API.EventHandlers
{
    public class OrganizationEventHandler : IIntegrationEventHandler<OrganizationIntegrationEvent>
    {
        private readonly IOrganizationDataProvider _dataProvider;
        private readonly ILogger<OrganizationEventHandler> _logger;
        
        public OrganizationEventHandler(IOrganizationDataProvider dataProvider, ILogger<OrganizationEventHandler> logger)
        {
            _dataProvider = dataProvider;
            _logger = logger;
        }

        public async Task Handle(OrganizationIntegrationEvent @event)
        {
            _logger.LogInformation("Sync organization from google pub/sub");

            var organization = await _dataProvider.GetDetailByCode(@event.OrgCode, true);
            var isNew = false;
            if (organization == null)
            {
                isNew = true;
                organization = new Organization()
                {
                    OrgCode = @event.OrgCode,
                    CreatedAt = DateTime.Now
                };
            }
            
            organization.Name = @event.Name;
            organization.BIOHROrganizationId = @event.BIOHROrganizationId;
            organization.UpdatedAt = DateTime.Now;
            organization.CreatedBy = @event.CreatedBy;
            organization.UpdatedBy = @event.UpdatedBy;
            organization.RowStatus = @event.RowStatus;
            
            if (isNew)
            {
                await _dataProvider.Insert(organization);
            }
            else
            {
                await _dataProvider.Update(organization);
            }

        }
    }
}