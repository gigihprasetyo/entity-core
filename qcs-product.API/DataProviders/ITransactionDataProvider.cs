using System.Collections.Generic;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;
using qcs_product.API.BindingModels;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface ITransactionDataProvider
    {
        public Task<RoomDetailRelationViewModel> InsertRoomMasterDataForByRequestId(MasterDataUseByRequest data, List<TransactionTestParameter> getTestParam, List<SamplingPointLiteViewModel> samplingPointExisting);
        public Task<TransactionToolViewModel> _InsertToolTrxByToolId(int toolId, List<TransactionTestParameter> getTestParam, List<SamplingPointLiteViewModel> samplingPointExisting);
        public Task<ToolsAHUViewModel> getAhuById(int id);
        public Task<Facility> _InsertFacility(int facilityId);
        public Task<Organization> GetDetailById(int id);
        public Task<List<TransactionTestParameter>> GetListTransactionTestParams();
    }
}