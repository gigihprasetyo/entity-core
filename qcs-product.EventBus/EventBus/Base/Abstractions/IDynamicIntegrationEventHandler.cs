using System.Threading.Tasks;

namespace qcs_product.EventBus.EventBus.Base.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
