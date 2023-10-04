using qcs_product.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IRelSamplingToolDataProvider
    {
        public Task DeleteByToolCode(string toolCode);
        public Task<RelSamplingTool> GetByCodeAndSamplingPointCode(string samplingPointCode, string toolCode);
        public Task<List<RelSamplingTool>> GetByToolCode(string toolCode);
        public Task<List<RelSamplingTool>> InsertList(List<RelSamplingTool> lsRelSamplingTool);
        public Task<RelSamplingTool> Update(RelSamplingTool relSamplingTool);
    }
}