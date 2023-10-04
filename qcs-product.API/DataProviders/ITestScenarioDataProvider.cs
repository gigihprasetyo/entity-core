using System.Collections.Generic;
using System.Threading.Tasks;
using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders
{
    public interface ITestScenarioDataProvider
    {
        Task<List<TestScenarioViewModel>> GetList(string search, List<int> GradeRoomId);

        Task<List<TestScenario>> GetListExist();

        Task<TestScenario> GetById(int id);
        Task<TestScenario> Insert(TestScenario testScenario);
        Task<TestScenario> Update(TestScenario testScenario);
        Task<List<int>> GetIdsByGradeRoomId(int gradeRoomId);
        Task<RelTestScenarioParam> InsertRelTestScenarioParam(RelTestScenarioParam relTestScenarioParam);
        Task<RelTestScenarioParam> UpdateRelTestScenarioParam(RelTestScenarioParam relTestScenarioParam);
        Task<TestScenario> GetByGradeRoomCodeLabel(string gradeRoomCode, string label);
        Task<RelTestScenarioParam> GetRelTestScenarioParam(int testScenarioId, int testParameterId);

        Task<TestScenario> GetByGradeRoomLabel(int GradeRoomId, string label);
        Task<List<TestScenario>> GetByGradeRoomId(int GradeRoomId);
        public Task<List<TestScenarioViewModel>> GetListTransaction(string search, List<int> GradeRoomId);
        public Task<List<TransactionTestScenario>> GetList();
    }
}