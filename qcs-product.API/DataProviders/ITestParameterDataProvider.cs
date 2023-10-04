using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders
{
    public interface ITestParameterDataProvider
    {
        public Task<List<TestParameterByScenarioRelationViewModel>> List(Int32 roomId, Int32 testScenarioId);
        public Task<List<TestParameterViewModel>> ListShort(string search, int limit, int page, int TestGroupId);
        public Task<List<int>> GetIds();
        public Task<List<TestParameterByScenarioRelationViewModel>> ListAlt(List<int> roomIds, string testScenarioLabel);
        public Task<List<TestParameterByScenarioRelationViewModel>> ListV2(List<int> roomIds, string testScenarioLabel, List<int> purposeId);
    }
}
