using qcs_product.EventBus.EventBus.Base.Events;
using System.Threading.Tasks;

namespace qcs_product.EventBus.EventBus.Base.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
