using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IToolBusinessProvider
    {
        public Task<ResponseViewModel<ToolRelationViewModel>> List(string search, string ToolGroupId, string RoomId, Int32 GradeRoomId, DateTime? startDate, DateTime? endDate, int? facilityId);
        public Task<ResponseViewModel<ShortDataListViewModel>> ShortList(string search, int GroupId, string groupName, int? facilityId);
        public Task<ResponseViewModel<ToolGroupViewModel>> ToolGroupList(string search);
    }
}
