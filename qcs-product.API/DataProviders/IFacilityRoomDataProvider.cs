using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IFacilityRoomDataProvider
    {

        Task<RoomFacility> Insert(RoomFacility facility);

        Task<RoomFacility> Update(RoomFacility facility);
        Task<RoomFacility> GetByCode(int facilityId);
        Task<List<RoomFacility>> GetByFacilityIdAndRoomId(int facilityId, int roomId);
        public Task<List<RoomFacility>> GetByFacilityId(int facilityId);
    }
}