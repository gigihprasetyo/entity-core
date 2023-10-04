using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IRoomBusinessProvider
    {
        public Task<ResponseViewModel<RoomRelationViewModel>> GetDetailRoomById(int id);
        public Task<ResponseViewModel<RoomRelationViewModel>> List(string search, int limit, int page);
        public Task<ResponseViewModel<RoomSamplingPointLayout>> ListRoomSamplingPointLayout();
        public Task<ResponseViewModel<RoomSamplingPointLayoutViewModel>> GetRoomSamplingPointLayoutBySamplingId(int samplingId);
        public Task<ResponseViewModel<RoomRelationViewModel>> ListFacilityAHU(string search, int FacilityId, string AhuId);
    }
}
