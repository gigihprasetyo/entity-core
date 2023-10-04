using qcs_product.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IToolPurposeToMasterPurposeDataProvider
    {
        public Task DeleteByToolCode(string toolCode);
        public Task<List<ToolPurposeToMasterPurpose>> GetByToolCode(string toolCode);
        public Task<List<ToolPurposeToMasterPurpose>> InsertList(List<ToolPurposeToMasterPurpose> lsToolPurposeToMasterPurpose);
    }
}