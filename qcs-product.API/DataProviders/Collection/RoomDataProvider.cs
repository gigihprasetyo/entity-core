using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.API.BusinessProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.Constants;
using System.Text.Json;
using System.IO;

namespace qcs_product.API.DataProviders.Collection
{
    public class RoomDataProvider : IRoomDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<RoomDataProvider> _logger;
        private readonly IUploadFilesBusinessProvider _uploadFilesBusinessProvider;

        [ExcludeFromCodeCoverage]
        public RoomDataProvider(QcsProductContext context, ILogger<RoomDataProvider> logger, IUploadFilesBusinessProvider uploadFilesBusinessProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
            _uploadFilesBusinessProvider = uploadFilesBusinessProvider;
        }

        public async Task<List<RoomRelationViewModel>> GetDetailRelationById(int id)
        {
            var result = await (from rm in _context.Rooms
                                join gr in _context.GradeRooms on rm.GradeRoomId equals gr.Id
                                where rm.Id == id
                                select new RoomRelationViewModel
                                {
                                    RoomId = rm.Id,
                                    RoomCode = rm.Code,
                                    RoomName = rm.Name,
                                    GradeRoomId = rm.GradeRoomId,
                                    GradeRoomCode = gr.Code,
                                    GradeRoomName = gr.Name
                                }).ToListAsync();


            foreach (var res in result)
            {
                res.EmTestTypeParameter = (from tp in _context.TestParameters
                                           select new EmTestTypeViewModel
                                           {
                                               TestParameterId = tp.Id,
                                               TestParameterName = tp.Name,
                                               RoomId = res.RoomId,
                                               TestParameterSquence = tp.Sequence,
                                               CountParamater = (from tpr in _context.TestParamRoomModels
                                                                 where tpr.TestParameterId == tp.Id && tpr.RoomId == res.RoomId
                                                                 select tpr.TotalTestParameter).FirstOrDefault(),
                                               ThresholdRoomSamplingPoint = (from rrsp in _context.RelRoomSamplings
                                                                             join room_purp in _context.RoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                                             where room_purp.RoomId == res.RoomId
                                                                             select rrsp.SamplingPointId).Count(),
                                               ThresholdToolSamplingPoint = (from a in (from rst in _context.RelSamplingTools
                                                                                        join tps in _context.ToolPurposes on rst.ToolPurposeId equals tps.Id
                                                                                        join t in _context.Tools on tps.ToolId equals t.Id
                                                                                        group new { rst, t } by new { rst.SamplingPointId, t.RoomId } into g
                                                                                        where g.Key.RoomId == res.RoomId
                                                                                        select new
                                                                                        { SpId = g.Key.SamplingPointId }
                                                                                        )
                                                                             select a.SpId).Count()
                                           }).OrderBy(x => x.TestParameterSquence).ToList();
            }

            _logger.LogInformation($"Data result : {result}");

            return result;
        }

        public async Task<List<RoomRelationViewModel>> List(string search, int limit, int page)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = (from r in _context.Rooms
                          join gr in _context.GradeRooms on r.GradeRoomId equals gr.Id
                          where ((EF.Functions.Like(r.Name.ToLower(), "%" + filter + "%")) ||
                            (EF.Functions.Like(r.Code.ToLower(), "%" + filter + "%")) ||
                            (EF.Functions.Like(gr.Name.ToLower(), "%" + filter + "%")))
                          && r.RowStatus == null
                          && r.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                          select new RoomRelationViewModel
                          {
                              RoomId = r.Id,
                              RoomCode = r.Code,
                              RoomName = r.Name,
                              GradeRoomId = r.GradeRoomId,
                              GradeRoomCode = gr.Code,
                              GradeRoomName = gr.Name
                          }).AsQueryable();

            var resultData = new List<RoomRelationViewModel>();

            if (limit > 0)
            {
                resultData = await result.Skip(page).Take(limit).ToListAsync();
            }
            else
            {
                resultData = await result.ToListAsync();
            }

            foreach (var res in resultData)
            {
                res.EmTestTypeParameter = (from tp in _context.TestParameters
                                           select new EmTestTypeViewModel
                                           {
                                               TestParameterId = tp.Id,
                                               TestParameterName = tp.Name,
                                               RoomId = res.RoomId,
                                               TestParameterSquence = tp.Sequence,
                                               CountParamater = (from tpr in _context.TestParamRoomModels
                                                                 where tpr.TestParameterId == tp.Id && tpr.RoomId == res.RoomId
                                                                 select tpr.TotalTestParameter).FirstOrDefault(),
                                               ThresholdRoomSamplingPoint = (from rrsp in _context.RelRoomSamplings
                                                                             join room_purp in _context.RoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                                             where room_purp.RoomId == res.RoomId
                                                                             select rrsp.SamplingPointId).Count(),
                                               ThresholdToolSamplingPoint = (from a in (from rst in _context.RelSamplingTools
                                                                                        join tps in _context.ToolPurposes on rst.ToolPurposeId equals tps.Id
                                                                                        join t in _context.Tools on tps.ToolId equals t.Id
                                                                                        group new { rst, t } by new { rst.SamplingPointId, t.RoomId } into g
                                                                                        where g.Key.RoomId == res.RoomId
                                                                                        select new
                                                                                        { SpId = g.Key.SamplingPointId }
                                                                                        )
                                                                             select a.SpId).Count()
                                           }).OrderBy(x => x.TestParameterSquence).ToList();
            }

            return resultData;

        }

        public async Task<List<RoomRelationViewModel>> ListByFacilityAHU(string search, int FacilityId, List<int> AhuId)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from r in _context.Rooms
                                join gr in _context.GradeRooms on r.GradeRoomId equals gr.Id
                                join RelFc in _context.RoomFacilities on r.Id equals RelFc.RoomId
                                join fc in _context.Facilities on RelFc.FacilityId equals fc.Id
                                join t in _context.Tools on r.Ahu equals t.Id into ahu
                                from t in ahu.DefaultIfEmpty()
                                where ((EF.Functions.Like(r.Name.ToLower(), "%" + filter + "%")) ||
                                  (EF.Functions.Like(r.Code.ToLower(), "%" + filter + "%")) ||
                                  (EF.Functions.Like(gr.Name.ToLower(), "%" + filter + "%")))
                                && RelFc.FacilityId == FacilityId
                                && r.RowStatus == null && r.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                select new RoomRelationViewModel
                                {
                                    RoomId = r.Id,
                                    RoomCode = r.Code,
                                    RoomName = r.Name,
                                    GradeRoomId = r.GradeRoomId,
                                    GradeRoomCode = gr.Code,
                                    GradeRoomName = gr.Name,
                                    FacilityId = fc.Id,
                                    FacilityCode = fc.Code,
                                    FacilityName = fc.Name,
                                    AhuId = r.Ahu,
                                    AhuCode = t.ToolCode,
                                    AhuName = t.Name
                                }).Where(x => AhuId.Contains((int)x.AhuId) || !AhuId.Any()).ToListAsync();
            //}).ToListAsync();


            foreach (var res in result)
            {
                res.EmTestTypeParameter = (from tp in _context.TestParameters
                                           select new EmTestTypeViewModel
                                           {
                                               TestParameterId = tp.Id,
                                               TestParameterName = tp.Name,
                                               RoomId = res.RoomId,
                                               TestParameterSquence = tp.Sequence,
                                               CountParamater = (from tpr in _context.TestParamRoomModels
                                                                 where tpr.TestParameterId == tp.Id && tpr.RoomId == res.RoomId
                                                                 select tpr.TotalTestParameter).FirstOrDefault(),
                                               ThresholdRoomSamplingPoint = (from rrsp in _context.RelRoomSamplings
                                                                             join room_purp in _context.RoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                                             where room_purp.RoomId == res.RoomId
                                                                             select rrsp.SamplingPointId).Count(),
                                               ThresholdToolSamplingPoint = (from a in (from rst in _context.RelSamplingTools
                                                                                        join tps in _context.ToolPurposes on rst.ToolPurposeId equals tps.Id
                                                                                        join t in _context.Tools on tps.ToolId equals t.Id
                                                                                        group new { rst, t } by new { rst.SamplingPointId, t.RoomId } into g
                                                                                        where g.Key.RoomId == res.RoomId
                                                                                        select new
                                                                                        { SpId = g.Key.SamplingPointId }
                                                                                        )
                                                                             select a.SpId).Count()
                                           }).OrderBy(x => x.TestParameterSquence).ToList();
            }

            return result;
        }

        public async Task<Room> GetByCode(string code)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(x => (x.Code == code) && (x.RowStatus == null));
            return room;
        }

        public async Task<Room> Insert(Room room)
        {
            try
            {
                await _context.Rooms.AddAsync(room);
                await _context.SaveChangesAsync();
                return room;

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return null;
        }

        public async Task<Room> Update(Room room)
        {
            try
            {
                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();
                return room;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return null;
        }

        public async Task<Room> GetById(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(x => (x.Id == id) && (x.RowStatus == null));
            return room;
        }

        public async Task<RelRoomSampling> GetRelRoomSamplingByCodeAndSamplingPointCode(string code, string samplingPointCode)
        {
            return await (from rrs in _context.RelRoomSamplings
                          join room_purp in _context.RoomPurpose on rrs.RoomPurposeId equals room_purp.Id
                          join r in _context.Rooms on room_purp.RoomId equals r.Id
                          join sp in _context.SamplingPoints on rrs.SamplingPointId equals sp.Id
                          where r.Code == code && sp.Code == samplingPointCode
                          select rrs).FirstOrDefaultAsync();
        }

        public async Task<RelRoomSampling> InsertRelRoomSampling(RelRoomSampling relRoomSampling)
        {
            try
            {
                await _context.RelRoomSamplings.AddAsync(relRoomSampling);
                await _context.SaveChangesAsync();
                return relRoomSampling;

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return null;
        }

        //todo -> move to transaction
        public async Task<List<RoomSamplingPointLayout>> ListRoomSamplingPointLayout()
        {
            var result = await (from rspl in _context.TransactionRoomSamplingPointLayout
                                select new RoomSamplingPointLayout
                                {
                                    Id = rspl.Id,
                                    RoomPurposeId = rspl.RoomPurposeId.Value,
                                    AttachmentFile = rspl.AttachmentFile,
                                    FileName = rspl.FileName,
                                    FileType = rspl.FileType
                                }).ToListAsync();

            foreach (var item in result)
            {
                item.AttachmentFile = await _uploadFilesBusinessProvider.GenerateV4SignedReadUrl($"{item.FileName}.{item.FileType}");
            }

            return result;
        }

        //todo -> move to transaction
        public async Task<List<RoomSamplingPointLayoutViewModel>> GetRoomSamplingPointLayoutBySamplingId(int samplingId)
        {
            var getRoomPurpose = await (from rq in _context.RequestQcs
                                        join rr in _context.RequestGroupRoomPurpose on rq.Id equals rr.RequestQcsId
                                        join qs in _context.QcSamplings on rq.Id equals qs.RequestQcsId
                                        where qs.Id == samplingId
                                        select rr.RoomPurposeId).ToListAsync();

            var result = await (from rq in _context.RequestQcs
                                join rr in _context.RequestRooms on rq.Id equals rr.QcRequestId
                                join qs in _context.QcSamplings on rq.Id equals qs.RequestQcsId
                                join r in _context.TransactionRoom on rr.RoomId equals r.Id
                                where qs.Id == samplingId
                                select new RoomSamplingPointLayoutViewModel
                                {
                                    SamplingId = qs.Id,
                                    RoomId = r.Id,
                                    RoomCode = r.Code,
                                    RoomName = r.Name,
                                    SamplingPointLayouts = (from rspl in _context.TransactionRoomSamplingPointLayout
                                                            join room_purp in _context.TransactionRoomPurpose on rspl.RoomPurposeId equals room_purp.Id
                                                            where room_purp.RoomId == r.Id
                                                            //&& rspl.RowStatus == null
                                                            && getRoomPurpose.Contains(room_purp.Id)
                                                            select new SamplingPointLayoutViewModel()
                                                            {
                                                                AttachmentFile = rspl.AttachmentFile,
                                                                FileName = rspl.FileName,
                                                                FileType = rspl.FileType
                                                            }).ToList()
                                }).ToListAsync();


            return result;
        }
        public async Task<RoomDetailRelationViewModel> GetRoomRelationDetailById(int id)
        {
            var result = await (from r in _context.Rooms
                                join gr in _context.GradeRooms on r.GradeRoomId equals gr.Id
                                join t in _context.Tools on r.Ahu equals t.Id into ahu
                                from t in ahu.DefaultIfEmpty()
                                where r.Id == id
                                && r.RowStatus == null
                                select new RoomDetailRelationViewModel
                                {
                                    RoomId = r.Id,
                                    RoomCode = r.Code,
                                    RoomName = r.Name,
                                    GradeRoomId = r.GradeRoomId,
                                    GradeRoomCode = gr.Code,
                                    GradeRoomName = gr.Name,
                                    AhuId = r.Ahu,
                                    AhuCode = t.ToolCode,
                                    AhuName = t.Name
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<RelRoomSampling> UpdateRelRoomSampling(RelRoomSampling relRoomSampling)
        {
            try
            {
                _context.RelRoomSamplings.Update(relRoomSampling);
                await _context.SaveChangesAsync();
                return relRoomSampling;

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return null;
        }

        public List<RelRoomSampling> RemoveRangeByRoomPurpose(int roomPurposeId)
        {
            var getListRelRoomSamplingByRoomPurposeId = _context.RelRoomSamplings.Where(x => x.RoomPurposeId == roomPurposeId).ToList();
            _context.RelRoomSamplings.RemoveRange(getListRelRoomSamplingByRoomPurposeId);
            return getListRelRoomSamplingByRoomPurposeId;
        }

        public async Task<RoomDetailViewModel> GetRoomDetailById(int roomId)
        {
            var roomDetail = await (from room in _context.Rooms
                                    where room.Id == roomId
                                    select new RoomDetailViewModel()
                                    {
                                        Id = room.Id,
                                        Code = room.Code,
                                        Name = room.Name,
                                        GradeRoomId = room.GradeRoomId,
                                        OrganizationId = room.OrganizationId,
                                        BuildingId = room.BuildingId,
                                        CreatedAt = room.CreatedAt,
                                        CreatedBy = room.CreatedBy,
                                        Area = room.Area,
                                        Ahu = room.Ahu,
                                        PosId = room.PosId,
                                        Floor = room.Floor,
                                        TemperatureOperator = room.TemperatureOperator,
                                        TemperatureValue = room.TemperatureValue,
                                        TemperatureValueFrom = room.TemperatureValueFrom,
                                        TemperatureValueTo = room.TemperatureValueTo,
                                        HumidityOperator = room.HumidityOperator,
                                        HumidityValue = room.HumidityValue,
                                        HumidityValueFrom = room.HumidityValueFrom,
                                        HumidityValueTo = room.HumidityValueTo,
                                        PressureOperator = room.PressureOperator,
                                        PressureValue = room.PressureValue,
                                        PressureValueTo = room.PressureValueTo,
                                        PressureValueFrom = room.PressureValueFrom,
                                        AirChangeOperator = room.AirChangeOperator,
                                        AirChangeValue = room.AirChangeValue,
                                        AirChangeValueFrom = room.AirChangeValueFrom,
                                        AirChangeValueTo = room.AirChangeValueTo,
                                        ListDataPurposes = (from rp in _context.RoomPurpose
                                                            where rp.RoomId == roomId
                                                            select new PurposesDatas()
                                                            {
                                                                Id = rp.Id,
                                                                Purpose = (from mp in _context.RoomPurposeToMasterPurposes
                                                                           join p in _context.Purposes on mp.PurposeId equals p.Id
                                                                           where mp.RoomPurposeId == rp.Id
                                                                           select p).ToList(),
                                                                //SamplingPoints = null,
                                                                SamplingPointLayout = (from rsp in _context.RoomSamplingPointLayout
                                                                                       where rsp.RoomPurposeId == rp.Id
                                                                                       select rsp).ToList()
                                                            }).ToList()

                                    }).FirstOrDefaultAsync();

            var roomPurposeIds = roomDetail.ListDataPurposes.Select(x => x.Id).Distinct().ToList();
            var samplingPoints = await (from rps in _context.RelRoomSamplings
                                        join sp in _context.SamplingPoints on rps.SamplingPointId equals sp.Id
                                        where roomPurposeIds.Contains(rps.RoomPurposeId ?? 0)
                                        select new RoomSamplingPointViewModel()
                                        {
                                            SamplingPointId = sp.Id,
                                            Code = sp.Code,
                                            RoomId = sp.RoomId,
                                            ToolId = 0,
                                            ScenarioLabel = rps.ScenarioLabel,
                                            RoomPurposeId = rps.RoomPurposeId ?? 0,
                                        }).ToListAsync();

            var samplingPointIds = samplingPoints.Select(x => x.SamplingPointId).Distinct().ToList();
            var testParameters = await (from tp in _context.TestParameters
                                        join rtsp in _context.RelTestScenarioParams on tp.Id equals rtsp.TestParameterId
                                        join rstp in _context.RelSamplingTestParams on rtsp.Id equals rstp.TestScenarioParamId
                                        join tv in _context.TestVariables on rtsp.Id equals tv.TestParameterId
                                        join ts in _context.TestScenarios on rtsp.TestScenarioId equals ts.Id
                                        where samplingPointIds.Contains(rstp.SamplingPointId)
                                        select new TestParameterRoom()
                                        {
                                            Id = tp.Id,
                                            TestParameterName = tp.Name,
                                            SamplingPointId = rstp.SamplingPointId,
                                            ScenarioLabel = ts.Label,
                                            TestVariable = tv
                                        }).ToListAsync();

            //get unique testParameters
            var groupedTestParameters = testParameters.GroupBy(x => new { x.Id, x.TestParameterName, x.SamplingPointId, x.ScenarioLabel })
                .Select(x => x.First())
                .ToList();

            foreach (var item in groupedTestParameters)
            {
                item.TestVariable = null;
                var filteredTestVariables = testParameters
                    .Where(x => x.Id == item.Id && x.TestParameterName == item.TestParameterName
                        && x.SamplingPointId == item.SamplingPointId && x.ScenarioLabel == item.ScenarioLabel
                        && x.TestVariable != null)
                    .Select(x => x.TestVariable)
                    .ToList();

                //get unique testVariables
                item.TestVariables = filteredTestVariables
                    .GroupBy(x => x.Id)
                    .Select(x => x.First())
                    .ToList();
            }

            foreach (var itemPurpose in roomDetail.ListDataPurposes)
            {
                itemPurpose.SamplingPoints = samplingPoints
                    .Where(x => x.RoomPurposeId == itemPurpose.Id)
                    .ToList();

                foreach (var itemSp in itemPurpose.SamplingPoints)
                {
                    itemSp.TestParameter = groupedTestParameters
                        .Where(x => x.SamplingPointId == itemSp.SamplingPointId && x.ScenarioLabel == itemSp.ScenarioLabel)
                        .ToList();
                }
            }

            return roomDetail;
        }
    }
}
