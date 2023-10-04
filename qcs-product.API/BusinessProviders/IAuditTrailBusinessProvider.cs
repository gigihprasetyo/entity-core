using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.BusinessProviders
{
    public interface IAuditTrailBusinessProvider
    {
        Task Add(string operation, string remarks, RequestQcs entity, string usernameModifier);
        
        Task Add(string operation, string remarks, QcSampling entity, string usernameModifier);
        
        Task Add(string operation, string remarks, QcSamplingShipment entity, string usernameModifier);
        
        Task Add(string operation, string remarks, QcSamplingShipmentTracker entity, string usernameModifier);
        
        Task Add(string operation, string remarks, QcSamplingShipmentTracker entity, QcSamplingShipment parent, string usernameModifier);
        
        Task Add(string operation, string remarks, QcTransactionGroup entity);
        Task<string> GetUsernameByNik(string nik);
    }
}