using System.Threading.Tasks;
using qcs_product.API.BindingModels;
using qcs_product.API.ViewModels;

namespace qcs_product.API.BusinessProviders
{
    public interface ITestScenarioBusinessProvider
    {
        Task<ResponseViewModel<TestScenarioViewModel>> GetList(string search, string GradeRoomId);
    }
}