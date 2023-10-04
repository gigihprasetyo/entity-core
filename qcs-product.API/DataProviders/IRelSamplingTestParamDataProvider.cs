using qcs_product.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IRelSamplingTestParamDataProvider
    {
        public Task<RelSamplingTestParam> Get(int samplingPointId, int testScnParamId);
        public Task<List<RelSamplingTestParam>> GetByToolCode(string toolCode);
        public Task<List<RelSamplingTestParam>> InsertList(List<RelSamplingTestParam> lsRelSamplingTestParam);
        public Task<RelSamplingTestParam> Update(RelSamplingTestParam relSamplingTestParam);
        public Task DeleteByToolCode(string toolCode);
    }
}