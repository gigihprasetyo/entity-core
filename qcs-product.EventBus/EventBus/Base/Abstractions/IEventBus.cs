using System.Threading.Tasks;
using qcs_product.EventBus.EventBus.Base.Events;

namespace qcs_product.EventBus.EventBus.Base.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync(IntegrationEvent @event);
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
        void StartSubscribing();
    }
}
