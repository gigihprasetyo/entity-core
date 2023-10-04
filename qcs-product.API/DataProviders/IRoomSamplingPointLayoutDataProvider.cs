using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IRoomSamplingPointLayoutDataProvider
    {
        public Task<RoomSamplingPointLayout> Insert(RoomSamplingPointLayout roomSamplingPointLayout);
        public Task<List<RoomSamplingPointLayout>> GetByRoomCode(string roomCode);
    }
}
