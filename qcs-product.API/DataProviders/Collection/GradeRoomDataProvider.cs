using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders.Collection
{
    public class GradeRoomDataProvider : IGradeRoomDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<GradeRoomDataProvider> _logger;

        public GradeRoomDataProvider(QcsProductContext context, ILogger<GradeRoomDataProvider> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GradeRoom> GetByCode(string code)
        {
            return await _context.GradeRooms.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<GradeRoom> Insert(GradeRoom gradeRoom)
        {
            try
            {
                await _context.GradeRooms.AddAsync(gradeRoom);
                await _context.SaveChangesAsync();
                return gradeRoom;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

            return null;
        }

        public async Task<GradeRoom> Update(GradeRoom room)
        {
            try
            {
                _context.GradeRooms.Update(room);
                await _context.SaveChangesAsync();
                return room;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

            return null;
        }

        public async Task<RelGradeRoomScenario> GetRelGradeRoomScenario(string gradeRoomCode, string label)
        {
            return await (from grs in _context.RelGradeRoomScenarios
                          join gr in _context.GradeRooms on grs.GradeRoomId equals gr.Id
                          join ts in _context.TestScenarios on grs.TestScenarioId equals ts.Id
                          where gr.Code == gradeRoomCode && ts.Label == label
                          select grs).FirstOrDefaultAsync();
        }

        public async Task<RelGradeRoomScenario> InsertRelGradeRoomScenario(RelGradeRoomScenario relGradeRoomScenario)
        {
            await _context.RelGradeRoomScenarios.AddAsync(relGradeRoomScenario);
            await _context.SaveChangesAsync();
            return relGradeRoomScenario;
        }

        public async Task<RelGradeRoomScenario> UpdateRelGradeRoomScenario(RelGradeRoomScenario relGradeRoomScenario)
        {

            _context.RelGradeRoomScenarios.Update(relGradeRoomScenario);
            await _context.SaveChangesAsync();
            return relGradeRoomScenario;
        }

        public async Task<GradeRoomDetailView> DetailGradeRoomById(int gradeRoomId)
        {
            return await (from gr in _context.GradeRooms
                          where gr.Id == gradeRoomId
                          select new GradeRoomDetailView()
                          {
                              Id = gr.Id,
                              TestGroupId = gr.TestGroupId,
                              Code = gr.Code,
                              Name = gr.Name,
                              GradeRoomDefault = gr.GradeRoomDefault,
                              CreatedAt = gr.CreatedAt,
                              CreatedBy = gr.CreatedBy,
                              TestScenario = (
                                from rgsr in _context.RelGradeRoomScenarios
                                join ts in _context.TestScenarios on rgsr.TestScenarioId equals ts.Id
                                where rgsr.GradeRoomId == gr.Id
                                select new TestScenarioViewModel()
                                {
                                    Name = ts.Name,
                                    Label = ts.Label,
                                    TestParameters = (
                                        from test_parameter in _context.TestParameters
                                        join test_scenario_test_parameter in _context.RelTestScenarioParams
                                            on test_parameter.Id equals test_scenario_test_parameter.TestParameterId
                                        where test_scenario_test_parameter.TestScenarioId == ts.Id
                                        select new TestParameterPubSubViewModel()
                                        {
                                            Id = test_parameter.Id,
                                            TestParameterName = test_parameter.Name,
                                            TestVariables = (from test_variable in _context.TestVariables
                                                             join test_scenario_params in _context.RelTestScenarioParams
                                                                 on test_variable.TestParameterId equals test_scenario_params.Id
                                                             join test_scenario in _context.TestScenarios
                                                                 on test_scenario_params.TestScenarioId equals test_scenario.Id
                                                             join test_parameter in _context.TestParameters
                                                                 on test_scenario_params.TestParameterId equals test_parameter.Id
                                                             where test_variable.TestParameterId == test_scenario_test_parameter.Id
                                                             select test_variable).ToList()
                                        }
                                    ).ToList()
                                }
                            ).ToList()
                          }).FirstOrDefaultAsync();
        }
    }
}