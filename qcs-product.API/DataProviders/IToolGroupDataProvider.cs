using System.Collections.Generic;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IToolGroupDataProvider
    {
        Task<ToolGroup> GetByCode(string code);

        Task<List<ToolGroup>> GetList();
        
        Task<ToolGroup> Insert(ToolGroup toolGroup);
        
        Task<ToolGroup> Update(ToolGroup toolGroup);
    }
}