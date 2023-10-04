using System.Collections.Generic;
using System.Threading.Tasks;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders
{
    public interface IGradeRoomDataProvider
    {
        public Task<GradeRoom> GetByCode(string code);

        public Task<GradeRoom> Insert(GradeRoom room);

        public Task<GradeRoom> Update(GradeRoom room);
        public Task<RelGradeRoomScenario> GetRelGradeRoomScenario(string gradeRoomCode, string label);
        public Task<RelGradeRoomScenario> InsertRelGradeRoomScenario(RelGradeRoomScenario relGradeRoomScenario);
        public Task<RelGradeRoomScenario> UpdateRelGradeRoomScenario(RelGradeRoomScenario relGradeRoomScenario);
        public Task<GradeRoomDetailView> DetailGradeRoomById(int gradeRoomId);
    }
}