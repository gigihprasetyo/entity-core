using System.Collections.Generic;
using System.Threading.Tasks;
using qcs_product.API.Models;
using qcs_product.API.Infrastructure;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders
{
    public interface IRoomPurposeToMasterPurposeDataProvider
    {
        public Task<RoomPurposeToMasterPurpose> Insert(RoomPurposeToMasterPurpose insert);
        public Task<List<RoomPurposeToMasterPurpose>> ListRoomPurposeToMasterPurposeByRoomId(int roomPurposeId);
        public List<RoomPurposeToMasterPurpose> RemoveRange(List<RoomPurposeToMasterPurpose> data);
    }
}