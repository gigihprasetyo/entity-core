using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IRoomDataProvider
    {
        public Task<List<RoomRelationViewModel>> GetDetailRelationById(int id);
        public Task<List<RoomRelationViewModel>> List(string search, int limit, int page);
        public Task<List<RoomRelationViewModel>> ListByFacilityAHU(string search, int FacilityId, List<int> AhuId);

        public Task<RoomDetailRelationViewModel> GetRoomRelationDetailById(int id);

        public Task<Room> GetById(int id);

        public Task<Room> GetByCode(string code);

        public Task<Room> Insert(Room room);

        public Task<Room> Update(Room room);
        public Task<RelRoomSampling> GetRelRoomSamplingByCodeAndSamplingPointCode(string code, string samplingPointCode);
        public Task<RelRoomSampling> InsertRelRoomSampling(RelRoomSampling relRoomSampling);
        public Task<List<RoomSamplingPointLayout>> ListRoomSamplingPointLayout();
        public Task<List<RoomSamplingPointLayoutViewModel>> GetRoomSamplingPointLayoutBySamplingId(int samplingId);
        public Task<RelRoomSampling> UpdateRelRoomSampling(RelRoomSampling relRoomSampling);
        public List<RelRoomSampling> RemoveRangeByRoomPurpose(int roomPurposeId);
        public Task<RoomDetailViewModel> GetRoomDetailById(int roomId);
    }
}
