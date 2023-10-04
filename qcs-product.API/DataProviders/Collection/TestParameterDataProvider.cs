using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class TestParameterDataProvider : ITestParameterDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly IQcRequestDataProvider _dataProviderRequestQcs;
        private readonly ILogger<TestParameterDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public TestParameterDataProvider(QcsProductContext context, IQcRequestDataProvider dataProviderRequestQcs, ILogger<TestParameterDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dataProviderRequestQcs = dataProviderRequestQcs ?? throw new ArgumentNullException(nameof(dataProviderRequestQcs));
            _logger = logger;
        }

        public async Task<List<TestParameterByScenarioRelationViewModel>> List(int roomId, int testScenarioId)
        {

            var result = await (from ts in _context.TestScenarios
                                join grs in _context.RelGradeRoomScenarios on ts.Id equals grs.TestScenarioId
                                join rm in _context.Rooms on grs.GradeRoomId equals rm.GradeRoomId
                                where rm.Id == roomId && grs.TestScenarioId == testScenarioId
                                select new TestParameterByScenarioRelationViewModel
                                {
                                    Id = ts.Id,
                                    Name = ts.Name,
                                    Label = ts.Label
                                }).ToListAsync();


            foreach (var res in result)
            {
                // var getCountPurposePerRoomPurpose = await (from room_purp_2 in _context.RoomPurpose
                //                                            join room_purp_to_master in _context.RoomPurposeToMasterPurposes on room_purp_2.Id equals room_purp_to_master.RoomPurposeId
                //                                            where room_purp_2.RoomId == res.RoomId
                //                                            && purposeId.Contains(room_purp_to_master.PurposeId)
                //                                            group room_purp_2 by room_purp_2.Id into newGroup
                //                                            select new
                //                                            {
                //                                                roomPurposeId = newGroup.Key,
                //                                                countPurpose = newGroup.Count()
                //                                            }).FirstOrDefaultAsync(x => x.countPurpose == purposeId.Count());

                res.TestParameter = (from tp in _context.TestParameters
                                     select new EmTestTypeViewModel
                                     {
                                         TestParameterId = tp.Id,
                                         TestParameterName = tp.Name,
                                         RoomId = roomId,
                                         TestParameterSquence = tp.Sequence,
                                         CountParamater = (from tpr in _context.TestParamRoomModels
                                                           where tpr.TestParameterId == tp.Id && tpr.RoomId == roomId && tpr.TestScenarioId == testScenarioId
                                                           select tpr.TotalTestParameter).FirstOrDefault(),
                                         ThresholdRoomSamplingPoint = (tp.Id == ApplicationConstant.TEST_PARAMETER_GV ? 0 : (from rrsp in _context.RelRoomSamplings
                                                                                                                             join room_purp in _context.RoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                                                                                             where room_purp.RoomId == res.RoomId
                                                                                                                             select rrsp.SamplingPointId).Count()),
                                         ThresholdToolSamplingPoint = (from a in (from rst in _context.RelSamplingTools
                                                                                  join tps in _context.ToolPurposes on rst.ToolPurposeId equals tps.Id
                                                                                  join t in _context.Tools on tps.ToolId equals t.Id
                                                                                  group new { rst, t } by new { rst.SamplingPointId, t.RoomId } into g
                                                                                  where g.Key.RoomId == roomId
                                                                                  select new
                                                                                  { SpId = g.Key.SamplingPointId })
                                                                       select a.SpId).Count()
                                     }).OrderBy(x => x.TestParameterSquence).ToList();
            }

            return result;

        }

        public async Task<List<TestParameterByScenarioRelationViewModel>> ListAlt(List<int> roomIds, string testScenarioLabel)
        {

            var result = await (from ts in _context.TestScenarios
                                join grs in _context.RelGradeRoomScenarios on ts.Id equals grs.TestScenarioId
                                join rm in _context.Rooms on grs.GradeRoomId equals rm.GradeRoomId
                                join gr in _context.GradeRooms on rm.GradeRoomId equals gr.Id
                                where roomIds.Contains(rm.Id)
                                && ts.Label == testScenarioLabel
                                select new TestParameterByScenarioRelationViewModel
                                {
                                    Id = ts.Id,
                                    Name = ts.Name,
                                    Label = ts.Label,
                                    RoomId = rm.Id,
                                    RoomCode = rm.Code,
                                    RoomName = rm.Name,
                                    GradeRoomId = rm.GradeRoomId,
                                    GradeRoomName = gr.Name
                                }).ToListAsync();

            foreach (var res in result)
            {
                res.TestParameter = (from tp in _context.TestParameters
                                     select new EmTestTypeViewModel
                                     {
                                         TestParameterId = tp.Id,
                                         TestParameterName = tp.Name,
                                         RoomId = res.RoomId,
                                         TestParameterSquence = tp.Sequence,
                                         CountParamater = ((from rrs in _context.RelRoomSamplings //get count sample point room
                                                            join room_purp in _context.RoomPurpose on rrs.RoomPurposeId equals room_purp.Id
                                                            join sttp in _context.RelSamplingTestParams on rrs.SamplingPointId equals sttp.SamplingPointId
                                                            join tsp in _context.RelTestScenarioParams on sttp.TestScenarioParamId equals tsp.Id
                                                            join ts in _context.TestScenarios on tsp.TestScenarioId equals ts.Id
                                                            where ts.Label == testScenarioLabel
                                                            && room_purp.RoomId == res.RoomId
                                                            && tsp.TestParameterId == tp.Id
                                                            select rrs.SamplingPointId).Count()
                                                           +
                                                           (from rst in _context.RelSamplingTools //get count sample point tool
                                                            join tps in _context.ToolPurposes on rst.ToolPurposeId equals tps.Id
                                                            join tl in _context.Tools on tps.ToolId equals tl.Id
                                                            join sttp in _context.RelSamplingTestParams on rst.SamplingPointId equals sttp.SamplingPointId
                                                            join tsp in _context.RelTestScenarioParams on sttp.TestScenarioParamId equals tsp.Id
                                                            join ts in _context.TestScenarios on tsp.TestScenarioId equals ts.Id
                                                            where ts.Label == testScenarioLabel
                                                            && tl.RoomId == res.RoomId
                                                            && tsp.TestParameterId == tp.Id
                                                            select rst.SamplingPointId).Count()
                                                           ),

                                         ThresholdRoomSamplingPoint = (tp.Id == ApplicationConstant.TEST_PARAMETER_GV ? 0 : (from rrsp in _context.RelRoomSamplings
                                                                                                                             join room_purp in _context.RoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                                                                                             where room_purp.RoomId == res.RoomId
                                                                                                                             select rrsp.SamplingPointId).Count()),
                                         ThresholdToolSamplingPoint = (from a in (from rst in _context.RelSamplingTools
                                                                                  join tps in _context.ToolPurposes on rst.ToolPurposeId equals tps.Id
                                                                                  join t in _context.Tools on tps.ToolId equals t.Id
                                                                                  group new { rst, t } by new { rst.SamplingPointId, t.RoomId } into g
                                                                                  where g.Key.RoomId == res.RoomId
                                                                                  select new
                                                                                  { SpId = g.Key.SamplingPointId })
                                                                       select a.SpId).Count()
                                     }).OrderBy(x => x.TestParameterSquence).ToList();
            }

            return result;
        }

        public async Task<List<TestParameterViewModel>> ListShort(string search, int limit, int page, int TestGroupId)
        {

            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = (from tp in _context.TestParameters
                          where (EF.Functions.Like(tp.Name.ToLower(), "%" + filter + "%"))
                          && tp.RowStatus == null
                          select new TestParameterViewModel
                          {
                              Id = tp.Id,
                              Name = tp.Name,
                              Sequence = tp.Sequence,
                              TestGroupId = tp.TestGroupId,
                              CreatedBy = tp.CreatedBy,
                              CreatedAt = tp.CreatedAt
                          }).Where(x => x.TestGroupId == TestGroupId || TestGroupId == 0).AsQueryable();

            var resultData = new List<TestParameterViewModel>();

            if (limit > 0)
            {
                resultData = await result.Skip(page).Take(limit).ToListAsync();
            }
            else
            {
                resultData = await result.ToListAsync();
            }

            return resultData;
        }

        public async Task<List<int>> GetIds()
        {
            return await (from tp in _context.TestParameters
                          select tp.Id).ToListAsync();
        }

        public async Task<List<TestParameterByScenarioRelationViewModel>> ListV2(List<int> roomIds, string testScenarioLabel, List<int> purposeId)
        {
            List<int> getGroupToolPurposes = await (from rptg in _context.RelSamplingPurposeToolGroups
                                                    where rptg.RowStatus == null
                                                    && purposeId.Contains(rptg.PurposeId)
                                                    select rptg.ToolGroupId).ToListAsync();

            var result = await (from ts in _context.TestScenarios
                                join grs in _context.RelGradeRoomScenarios on ts.Id equals grs.TestScenarioId
                                join rm in _context.Rooms on grs.GradeRoomId equals rm.GradeRoomId
                                join gr in _context.GradeRooms on grs.GradeRoomId equals gr.Id
                                where roomIds.Contains(rm.Id) && ts.Label == testScenarioLabel
                                select new TestParameterByScenarioRelationViewModel
                                {
                                    Id = ts.Id,
                                    Name = ts.Name,
                                    Label = ts.Label,
                                    RoomId = rm.Id,
                                    RoomCode = rm.Code,
                                    RoomName = rm.Name,
                                    GradeRoomId = grs.GradeRoomId,
                                    GradeRoomCode = gr.Code,
                                    GradeRoomName = gr.Name,
                                }).ToListAsync();

            var samplingPointsByTools = await _dataProviderRequestQcs.GetSamplePointTestParamByToolsInRoomsExisting(roomIds, testScenarioLabel, purposeId);


            foreach (var res in result)
            {
                var getCountPurposePerRoomPurpose = await (from room_purp_2 in _context.RoomPurpose
                                                           join room_purp_to_master in _context.RoomPurposeToMasterPurposes on room_purp_2.Id equals room_purp_to_master.RoomPurposeId
                                                           where room_purp_2.RoomId == res.RoomId
                                                           && purposeId.Contains(room_purp_to_master.PurposeId)
                                                           select room_purp_2.Id).ToListAsync();

                List<int> roomPurposeId = getCountPurposePerRoomPurpose == null ? null : getCountPurposePerRoomPurpose;

                res.TestParameter = (from tp in _context.TestParameters
                                     select new EmTestTypeViewModel
                                     {
                                         TestParameterId = tp.Id,
                                         TestParameterName = tp.Name,
                                         RoomId = res.RoomId,
                                         TestParameterSquence = tp.Sequence,
                                         CountParamater = ((from rrs in _context.RelRoomSamplings //get count sample point room
                                                            join room_purp in _context.RoomPurpose on rrs.RoomPurposeId equals room_purp.Id
                                                            join sttp in _context.RelSamplingTestParams on rrs.SamplingPointId equals sttp.SamplingPointId
                                                            join tsp in _context.RelTestScenarioParams on sttp.TestScenarioParamId equals tsp.Id
                                                            join ts in _context.TestScenarios on tsp.TestScenarioId equals ts.Id
                                                            join rgrs in _context.RelGradeRoomScenarios on ts.Id equals rgrs.TestScenarioId
                                                            where ts.Label == testScenarioLabel
                                                            && room_purp.RoomId == res.RoomId
                                                            && tsp.TestParameterId == tp.Id
                                                            && rgrs.GradeRoomId == res.GradeRoomId
                                                            && roomPurposeId.Contains(room_purp.Id)
                                                            && rrs.ScenarioLabel == testScenarioLabel
                                                            select rrs.SamplingPointId).Distinct().Count()
                                                          ),
                                         ThresholdRoomSamplingPoint = (tp.Id == ApplicationConstant.TEST_PARAMETER_GV ? 0 : (from rrsp in _context.RelRoomSamplings
                                                                                                                             join room_purp in _context.RoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                                                                                             where room_purp.RoomId == res.RoomId
                                                                                                                             select rrsp.SamplingPointId).Distinct().Count())
                                         //  ThresholdToolSamplingPoint = (from a in (from rst in _context.RelSamplingTools
                                         //                                           join tps in _context.ToolPurposes on rst.ToolPurposeId equals tps.Id
                                         //                                           join t in _context.Tools on tps.ToolId equals t.Id
                                         //                                           where t.RoomId == res.RoomId
                                         //                                           && getGroupToolPurposes.Contains(t.ToolGroupId)
                                         //                                           group new { rst, t } by new { rst.SamplingPointId, t.RoomId } into g
                                         //                                           //where g.Key.RoomId == roomId
                                         //                                           select new
                                         //                                           { SpId = g.Key.SamplingPointId })
                                         //                                select a.SpId).Count()
                                     }).OrderBy(x => x.TestParameterSquence).ToList();

                foreach (var item in res.TestParameter)
                {
                    //add CountParamater sesuai samplingPointsByTools
                    item.CountParamater += samplingPointsByTools
                        .Where(x => x.RoomId == item.RoomId && x.TestParameterId == item.TestParameterId)
                        .Select(x => x.SamplePointId)
                        .Distinct()
                        .Count();
                }
            }

            return result;
        }
    }
}
