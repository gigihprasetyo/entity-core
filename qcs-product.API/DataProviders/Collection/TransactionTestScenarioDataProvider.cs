using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.BindingModels;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionTestScenarioDataProvider : ITransactionTestScenarioDataProvider
    {
        private readonly QcsProductContext _context;

        public TransactionTestScenarioDataProvider(QcsProductContext context)
        {
            _context = context;
        }

        public async Task<TestScenario> GetByGradeRoomLabel(int GradeRoomId, string label)
        {
            var result = await (from ts in _context.TransactionTestScenario
                                join rgts in _context.TransactionRelGradeRoomScenario on ts.Id equals rgts.TestScenarioId
                                where ts.Label == label
                                && rgts.GradeRoomId == GradeRoomId
                                select ts).FirstOrDefaultAsync();

            var testScenario = new TestScenario()
            {
                Id = result.Id,
                Name = result.Name,
                Label = result.Label
            };

            return testScenario;
        }

        // public async Task<List<TestScenarioViewModel>> GetList(string search, List<int> GradeRoomId)
        // {

        //     string filter = "";
        //     if (search != null)
        //         filter = search.ToLower();

        //     var result = await (from ts in _context.TestScenarios
        //                         join rgrs in _context.RelGradeRoomScenarios on ts.Id equals rgrs.TestScenarioId
        //                         join gr in _context.GradeRooms on rgrs.GradeRoomId equals gr.Id
        //                         where ((EF.Functions.Like(ts.Name.ToLower(), "%" + filter + "%")) ||
        //                         (EF.Functions.Like(ts.Label.ToLower(), "%" + filter + "%")))
        //                         && GradeRoomId.Contains(rgrs.GradeRoomId)
        //                         && ts.RowStatus == null
        //                         group new { rgrs, gr, ts } by new { rgrs.GradeRoomId, ts.Id } into g
        //                         select new TestScenarioViewModel
        //                         {
        //                             Id = g.Max(x => x.ts.Id),
        //                             Label = g.Max(x => x.ts.Label),
        //                             Name = g.Max(x => x.ts.Name),
        //                             GradeRoomId = g.Max(x => x.rgrs.GradeRoomId),
        //                             GradeRoomCode = g.Max(x => x.gr.Code),
        //                             GradeRoomName = g.Max(x => x.gr.Name),
        //                         }).ToListAsync();


        //     return result;

        // }

        // public async Task<List<TestScenario>> GetList()
        // {
        //     return await (from sc in _context.TestScenarios
        //                   where sc.RowStatus == null
        //                   select sc).ToListAsync();
        // }

        // public async Task<TestScenario> GetById(int id)
        // {
        //     return await _context.TestScenarios.FirstOrDefaultAsync(x => x.Id == id);
        // }

        // public async Task<TestScenario> Insert(TestScenario testScenario)
        // {
        //     await _context.TestScenarios.AddAsync(testScenario);
        //     await _context.SaveChangesAsync();
        //     return testScenario;
        // }
        // public async Task<TestScenario> Update(TestScenario testScenario)
        // {
        //     _context.TestScenarios.Update(testScenario);
        //     await _context.SaveChangesAsync();
        //     return testScenario;
        // }

        // public async Task<TestScenario> GetByGradeRoomCodeLabel(string gradeRoomCode, string label)
        // {
        //     return await (from ts in _context.TestScenarios
        //                   join grs in _context.RelGradeRoomScenarios on ts.Id equals grs.TestScenarioId
        //                   join gr in _context.GradeRooms on grs.GradeRoomId equals gr.Id
        //                   where gr.Code == gradeRoomCode && ts.Label == label
        //                   select ts).FirstOrDefaultAsync();
        // }

        // public async Task<List<int>> GetIdsByGradeRoomId(int gradeRoomId)
        // {
        //     return await (from ts in _context.TestScenarios
        //                   join grs in _context.RelGradeRoomScenarios on ts.Id equals grs.TestScenarioId
        //                   where grs.GradeRoomId == gradeRoomId
        //                   select ts.Id).ToListAsync();

        // }

        // public async Task<RelTestScenarioParam> GetRelTestScenarioParam(int testScenarioId, int testParameterId)
        // {
        //     return await (from rtsp in _context.RelTestScenarioParams
        //                   where rtsp.TestScenarioId == testScenarioId && rtsp.TestParameterId == testParameterId
        //                   select rtsp).FirstOrDefaultAsync();

        // }

        // public async Task<RelTestScenarioParam> InsertRelTestScenarioParam(RelTestScenarioParam relTestScenarioParam)
        // {
        //     await _context.RelTestScenarioParams.AddAsync(relTestScenarioParam);
        //     await _context.SaveChangesAsync();
        //     return relTestScenarioParam;
        // }

        // public async Task<RelTestScenarioParam> UpdateRelTestScenarioParam(RelTestScenarioParam relTestScenarioParam)
        // {
        //     _context.RelTestScenarioParams.Update(relTestScenarioParam);
        //     await _context.SaveChangesAsync();
        //     return relTestScenarioParam;
        // }


        // public async Task<List<TestScenario>> GetByGradeRoomId(int GradeRoomId)
        // {
        //     var result = await (from ts in _context.TestScenarios
        //                         join rgts in _context.RelGradeRoomScenarios on ts.Id equals rgts.TestScenarioId
        //                         where rgts.GradeRoomId == GradeRoomId
        //                         select ts).ToListAsync();

        //     return result;
        // }

    }
}