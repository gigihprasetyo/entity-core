using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IToolDataProvider
    {
        public Task<List<ToolRelationViewModel>> List(string search, List<int> ToolGroupId, List<int> RoomId, Int32 GradeRoomId, DateTime? startDate, DateTime? endDate, int? facilityId);

        public Task<ToolRelationViewModel> GetToolById(int id);

        public Task<ToolsAHUViewModel> getAhuById(int id);

        public Task<List<ShortDataListViewModel>> ShortList(string search, int GroupId, string groupName, int? facilityId);

        public Task<List<ToolGroupViewModel>> ToolGroupList(string search);

        public Task<List<Int32>> ToolGroupIds();

        public Task<Tool> GetByCode(string code);

        public Task<Tool> Insert(Tool tool);

        public Task<Tool> Update(Tool tool);
        public Task<ToolDetailViewModel> GetToolDetailById(int toolid);
        public Task<List<ToolDetailViewModel>> GetToolDetailByRoomId(int roomId);
    }
}
