using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ITestParameterBusinessProvider
    {
        public Task<ResponseViewModel<TestParameterByScenarioRelationViewModel>> List(Int32 roomId, Int32 testScenarioId);
        public Task<ResponseViewModel<TestParameterViewModel>> ListShort(string search, int limit, int page, int TestGroupId);
        public Task<ResponseViewModel<TestParameterByScenarioRelationViewModel>> ListAlt(string roomIds, string testScenarioLabel);
        public Task<ResponseViewModel<TestParameterByScenarioRelationViewModel>> ListV2(string roomIds, string testScenarioLabel, string purposeId);
    }
}
