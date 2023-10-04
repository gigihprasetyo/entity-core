using System.Collections.Generic;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IToolActivityDataProvider
    {

        Task<ToolActivity> GetByCode(string code);

        Task<List<ToolActivity>> GetListByToolId(int toolId);
        
        Task<ToolActivity> Insert(ToolActivity toolActivity);

        Task<ToolActivity> Update(ToolActivity toolActivity);
    }
}