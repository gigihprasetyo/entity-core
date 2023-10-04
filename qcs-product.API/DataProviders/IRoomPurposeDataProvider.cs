using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IRoomPurposeDataProvider
    {
        public Task<RoomPurpose> InsertRoomPurpose(RoomPurpose insert);
        public Task<List<RoomPurpose>> ListRoomPurposeByRoomId(int roomId);
        public List<RoomPurpose> RemoveRange(string roomCode);
    }
}
