using qcs_product.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IToolPurposeDataProvider
    {
        public Task<ToolPurpose> Insert(ToolPurpose toolPurpose);
        public Task<List<ToolPurpose>> GetByToolCode(string toolCode);
        public Task DeleteByToolCode(string toolCode);
    }
}