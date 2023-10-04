using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class EmProductionPhaseDataProvider : IEmProductionPhaseDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<EmProductionPhaseDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public EmProductionPhaseDataProvider(QcsProductContext context, ILogger<EmProductionPhaseDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<EmProductionPhaseRelationViewModel>> List(string search)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from em in _context.EmProductionPhases
                                join fc in _context.Facilities on em.FacilityId equals fc.Id
                                where (EF.Functions.Like(em.Name.ToLower(), "%" + filter + "%"))
                                && em.RowStatus == null
                                select new EmProductionPhaseRelationViewModel
                                {
                                    Id = em.Id,
                                    Sequence = em.Sequence,
                                    Name = em.Name,
                                    ParentId = em.ParentId.ToString(),
                                    QcProduct = em.QcProduct,
                                    QcEm = em.QcEm,
                                    FacilityId = em.FacilityId,
                                    FacilityCode = fc.Code,
                                    FacilityName = fc.Name,
                                }).OrderBy(x => x.Sequence).ToListAsync();

            
            foreach (var res in result)
            {
                res.ProductionRooms = await (from epr in _context.RelEmProdPhaseToRooms
                                             join r in _context.Rooms on epr.RoomId equals r.Id
                                             join gr in _context.GradeRooms on r.GradeRoomId equals gr.Id
                                             where r.RowStatus == null
                                             && gr.RowStatus == null
                                             && epr.EmPhaseId == res.Id
                                             select new RelEmPhaseToRoomViewModel
                                             {
                                                 Id = epr.Id,
                                                 RoomId = epr.RoomId,
                                                 RoomCode = r.Code,
                                                 RoomName = r.Name,
                                                 GradeRoomId = gr.Id,
                                                 GradeRoomCode = gr.Code,
                                                 GradeRoomName = gr.Name,
                                                 TestScenario = (from tsc in _context.TestScenarios
                                                                 join gdts in _context.RelGradeRoomScenarios on tsc.Id equals gdts.TestScenarioId
                                                                 where gdts.GradeRoomId == gr.Id
                                                                 select new TestScenarioGradeRoomViewModel
                                                                 {
                                                                     Id = tsc.Id,
                                                                     TestScenarioName = tsc.Name,
                                                                     TestScenarioLabel = tsc.Label
                                                                 }).ToList()
                                             }).ToListAsync();

                /*res.ProductionRooms = await (from rf in _context.RoomFacilities
                                             join r in _context.Rooms on rf.RoomId equals r.Id
                                             join gr in _context.GradeRooms on r.GradeRoomId equals gr.Id
                                             where r.RowStatus == null
                                             && gr.RowStatus == null
                                             && rf.FacilityId == res.FacilityId
                                             select new RelEmPhaseToRoomViewModel
                                             {
                                                 Id = rf.Id,
                                                 RoomId = rf.RoomId,
                                                 RoomCode = r.Code,
                                                 RoomName = r.Name,
                                                 GradeRoomId = gr.Id,
                                                 GradeRoomCode = gr.Code,
                                                 GradeRoomName = gr.Name,
                                                 TestScenario = (from tsc in _context.TestScenarios
                                                                 join gdts in _context.RelGradeRoomScenarios on tsc.Id equals gdts.TestScenarioId
                                                                 where gdts.GradeRoomId == gr.Id
                                                                 select new TestScenarioGradeRoomViewModel
                                                                 {
                                                                     Id = tsc.Id,
                                                                     TestScenarioName = tsc.Name,
                                                                     TestScenarioLabel = tsc.Label
                                                                 }).ToList()
                                             }).ToListAsync();*/
            }

            _logger.LogInformation($"Data result : {result}");

            return result;
        }

        public async Task<List<EmProductionPhaseRelationViewModel>> GetByItemId(int itemId)
        {
            var result = await (from em in _context.EmProductionPhases
                join fc in _context.Facilities on em.FacilityId equals fc.Id
                join i in _context.Items on em.QcProduct equals i.ItemCode
                where em.RowStatus == null
                && i.Id == itemId
                select new EmProductionPhaseRelationViewModel
                {
                    Id = em.Id,
                    Sequence = em.Sequence,
                    Name = em.Name,
                    ParentId = em.ParentId.ToString(),
                    QcProduct = em.QcProduct,
                    QcEm = em.QcEm,
                    FacilityId = em.FacilityId,
                    FacilityCode = fc.Code,
                    FacilityName = fc.Name,
                }).OrderBy(x => x.Sequence).ToListAsync();
            var ids = result.Select(x => x.Id).Distinct().ToList();

            var productionRooms = await (from epr in _context.RelEmProdPhaseToRooms
                join r in _context.Rooms on epr.RoomId equals r.Id
                join gr in _context.GradeRooms on r.GradeRoomId equals gr.Id
                where r.RowStatus == null
                && gr.RowStatus == null
                && ids.Contains(epr.EmPhaseId)
                select new RelEmPhaseToRoomViewModel
                {
                    Id = epr.Id,
                    RoomId = epr.RoomId,
                    RoomCode = r.Code,
                    RoomName = r.Name,
                    GradeRoomId = gr.Id,
                    GradeRoomCode = gr.Code,
                    GradeRoomName = gr.Name,
                    EmPhaseId = epr.EmPhaseId,
                    TestScenario = (from tsc in _context.TestScenarios
                        join gdts in _context.RelGradeRoomScenarios on tsc.Id equals gdts.TestScenarioId
                        where gdts.GradeRoomId == gr.Id
                        select new TestScenarioGradeRoomViewModel
                        {
                            Id = tsc.Id,
                            TestScenarioName = tsc.Name,
                            TestScenarioLabel = tsc.Label
                        }).ToList()
                }).ToListAsync();

            foreach (var res in result)
            {
                res.ProductionRooms = productionRooms.Where(x => x.EmPhaseId == res.Id).ToList();
            }

            _logger.LogInformation($"Data result : {result}");

            return result;
        }

        public async Task<List<EmTestTypeViewModel>> TestTypeList(int RoomId)
        {
            var dataTestUjis = await (from tpr in _context.TestParamRoomModels
                                where tpr.RoomId == RoomId
                                select new EmTestTypeViewModel
                                {
                                    RoomId = tpr.RoomId,
                                    TestParameterId = tpr.TestParameterId,
                                    TestParameterName = tpr.TestParameterName,
                                    CountParamater = tpr.TotalTestParameter
                                }).ToListAsync();

            return dataTestUjis;
        }
    }
}
