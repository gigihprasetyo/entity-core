using qcs_product.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IToolSamplingPointLayoutDataProvider
    {
        public Task DeleteByToolCode(string toolCode);
        public Task<List<ToolSamplingPointLayout>> GetByToolCode(string toolCode);
        public Task<List<ToolSamplingPointLayout>> InsertList(List<ToolSamplingPointLayout> lsToolSamplingPointLayout);
    }
}