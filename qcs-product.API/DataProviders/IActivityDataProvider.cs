using System.Collections.Generic;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IActivityDataProvider
    {
        Task<Activity> GetByCode(string code);

        Task<Activity> Insert(Activity activity);
        
        Task<Activity> Update(Activity activity);

        Task<List<Activity>> GetListByCodes(List<string> codes);
    }
}