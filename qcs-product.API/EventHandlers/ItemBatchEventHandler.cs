using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.IntegrationEvents;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;

namespace qcs_product.API.EventHandlers
{
    public class ItemBatchEventHandler : IIntegrationEventHandler<ItemBatchIntegrationEvent>
    {
        private readonly ILogger<ItemBatchEventHandler> _logger;
        private readonly IItemDataProvider _itemDataProvider;

        public ItemBatchEventHandler(ILogger<ItemBatchEventHandler> logger, IItemDataProvider itemDataProvider)
        {
            _logger = logger;
            _itemDataProvider = itemDataProvider;
        }

        public async Task Handle(ItemBatchIntegrationEvent @event)
        {
            _logger.LogInformation("sync item batch from google pub/sub");
            try
            {
                _logger.LogInformation(JsonSerializer.Serialize(@event));
                _logger.LogInformation("insert or update item batch");

                ItemBatchItemGroupViewModel itemBatch = await _itemDataProvider.ByItemBatch(@event.ItemCode, @event.BatchNumber);

                if (itemBatch == null)
                {
                    Item item = await _itemDataProvider.GetItemByCode(@event.ItemCode);
                    var itemGroup = await _itemDataProvider.GetItemGroupsByCode(@event.ItemGroupCode);

                    if (item == null)
                    {
                        item = new Item()
                        {
                            ItemCode = @event.ItemCode,
                            Name = @event.ItemName,
                            ItemGroupId = itemGroup.Id,
                            ItemGroupName = @event.ItemGroupName,
                            ObjectStatus = @event.ObjectStatus,
                            ProductFormId = 1, // TODO: Tentukan id product form yang akan digunakan di MVP#1
                            OrgId = 0,
                            Temperature = "0",
                            CreatedAt = DateTime.Now,
                            CreatedBy = @event.UpdatedBy == null ? @event.UpdatedBy : @event.CreatedBy,
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = @event.UpdatedBy == null ? @event.UpdatedBy : @event.CreatedBy
                        };

                        item = await _itemDataProvider.Insert(item);
                    }

                    ItemBatchNumber newItemBatch = new ItemBatchNumber()
                    {
                        ItemId = item.Id,
                        BatchNumber = @event.BatchNumber,
                        ExpireDate = @event.ExpDate,
                        CreatedAt = DateTime.Now,
                        CreatedBy = @event.UpdatedBy == null ? @event.UpdatedBy : @event.CreatedBy,
                        UpdatedAt = DateTime.Now,
                        ObjectStatus = @event.ObjectStatus,
                        UpdatedBy = @event.UpdatedBy == null ? @event.UpdatedBy : @event.CreatedBy
                    };

                    newItemBatch = await _itemDataProvider.InsertBatch(newItemBatch);
                }
                else
                {
                    var itemGroup = await _itemDataProvider.GetItemGroupsByCode(@event.ItemGroupCode);
                    Item item = await _itemDataProvider.GetItemByCode(@event.ItemCode);

                    item.ItemCode = @event.ItemCode;
                    item.Name = @event.ItemName;
                    item.ItemGroupId = itemGroup.Id;
                    item.ObjectStatus = @event.ObjectStatus;
                    item.ItemGroupName = @event.ItemGroupName;
                    item.ProductFormId = 1; // TODO: Tentukan id product form yang akan digunakan di MVP#1
                    item.Temperature = "0";
                    item.UpdatedAt = DateTime.Now;
                    item.UpdatedBy = @event.UpdatedBy == null ? @event.UpdatedBy : @event.CreatedBy;

                    item = await _itemDataProvider.Update(item);

                    ItemBatchNumber newItemBatch = new ItemBatchNumber()
                    {
                        Id = itemBatch.Id,
                        ItemId = itemBatch.ItemId,
                        BatchNumber = @event.BatchNumber,
                        ObjectStatus = @event.ObjectStatus,
                        ExpireDate = @event.ExpDate,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = @event.UpdatedBy == null ? @event.UpdatedBy : @event.CreatedBy,
                        UpdatedBy = @event.UpdatedBy == null ? @event.UpdatedBy : @event.CreatedBy
                    };

                    newItemBatch = await _itemDataProvider.UpdateBatch(newItemBatch);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

        }


    }
}