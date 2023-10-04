using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IBuildingDataProvider
    {
        Task<Building> GetByCode(string code);
        
        Task<Building> Insert(Building building);
        
        Task<Building> Update(Building building);

    }
}