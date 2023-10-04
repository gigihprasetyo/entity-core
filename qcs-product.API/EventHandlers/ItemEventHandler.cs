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
    public class ItemEventHandler : IIntegrationEventHandler<ItemIntegrationEvent>
    {
        private readonly ILogger<ItemEventHandler> _logger;
        private readonly IItemDataProvider _itemDataProvider;

        public ItemEventHandler(ILogger<ItemEventHandler> logger, IItemDataProvider itemDataProvider)
        {
            _logger = logger;
            _itemDataProvider = itemDataProvider;
        }

        public async Task Handle(ItemIntegrationEvent @event)
        {
            _logger.LogInformation("sync item from google pub/sub");
            try
            {
                _logger.LogInformation(JsonSerializer.Serialize(@event));
                _logger.LogInformation("insert or update item");

                var item = await _itemDataProvider.GetItemByCode(@event.ItemCode);
                var itemGroup = await _itemDataProvider.GetItemGroupsByCode(@event.ItemGroupCode);

                if (item == null)
                {
                    item = new Item()
                    {
                        ItemCode = @event.ItemCode,
                        Name = @event.ItemNameEn,
                        ItemGroupId = itemGroup.Id,
                        ItemGroupName = itemGroup.ItemGroupName,
                        ObjectStatus = @event.ObjectStatus,
                        ProductFormId = 1, // TODO: Tentukan id product form yang akan digunakan di MVP#1
                        OrgId = 0,
                        Temperature = "0",
                        CreatedAt = DateTime.Now,
                        CreatedBy = @event.CreatedBy,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = @event.CreatedBy
                    };

                    item = await _itemDataProvider.Insert(item);
                }
                else
                {
                    item.ItemCode = @event.ItemCode;
                    item.Name = @event.ItemNameEn;
                    item.ItemGroupId = itemGroup.Id;
                    item.ItemGroupName = itemGroup.ItemGroupName;
                    item.ObjectStatus = @event.ObjectStatus;
                    item.ProductFormId = 1; // TODO: Tentukan id product form yang akan digunakan di MVP#1
                    item.Temperature = "0";
                    item.UpdatedAt = DateTime.Now;
                    item.UpdatedBy = @event.CreatedBy;

                    item = await _itemDataProvider.Update(item);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

        }


    }
}