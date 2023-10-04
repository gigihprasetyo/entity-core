using System.Collections.Generic;
using System.Threading.Tasks;
using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders
{
    public interface ITransactionTestScenarioDataProvider
    {
        public Task<TestScenario> GetByGradeRoomLabel(int GradeRoomId, string label);
    }
}