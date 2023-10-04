using System;
using System.Collections.Generic;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.IntegrationEvents
{
    public class ItemIntegrationEvent : IntegrationEvent
    {
        public string Operation { get; set; }
        public string ItemCode { get; set; }
        public string ItemNameIdn { get; set; }
        public string ItemNameEn { get; set; }
        public string ItemGroupCode { get; set; }
        public int ObjectStatus { get; set; }
        public string CreatedBy { get; set; }
        public List<ProductionProcessGroupForItemIntegrationEvent> ProductionProcessGroupCodes { get; set; }
    }

    public class ProductionProcessGroupForItemIntegrationEvent : IntegrationEvent
    {
        public string ProductionProcessGroupCode { get; set; }
    }
}