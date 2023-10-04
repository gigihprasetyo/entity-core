using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class ToolDataProvider : IToolDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<ToolDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public ToolDataProvider(QcsProductContext context, ILogger<ToolDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<ToolRelationViewModel>> List(string search, List<int> ToolGroupId, List<int> RoomId, Int32 GradeRoomId, DateTime? startDate, DateTime? endDate, int? facilityId)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var query = (from t in _context.Tools
                                join tg in _context.ToolGroups on t.ToolGroupId equals tg.Id
                                where ((EF.Functions.Like(t.ToolCode.ToLower(), "%" + filter + "%")) ||
                                    (EF.Functions.Like(t.Name.ToLower(), "%" + filter + "%")))
                                    && t.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                select new ToolRelationViewModel
                                {
                                    ToolId = t.Id,
                                    ToolCode = t.ToolCode,
                                    ToolName = t.Name,
                                    ToolGroupId = t.ToolGroupId,
                                    ToolGroupName = tg.Name,
                                    ToolGroupLabel = tg.Label,
                                    //RoomId = t.RoomId,
                                    //RoomName = r.Name,
                                    //GradeRoomId = t.GradeRoomId,
                                    //GradeRoomName = gr.Name,
                                    FacilityId = t.FacilityId,
                                    ActivityDateValidation = (from ta in _context.ToolActivities
                                                                join a in _context.Activities on ta.ActivityId equals a.Id
                                                                where ta.ToolId == t.Id && (EF.Functions.Like(a.Name.ToLower(), "%" + ApplicationConstant.TOOL_ACTIVITY_VALIDATION_LABEL.ToLower() + "%"))
                                                                orderby ta.ExpiredDate descending
                                                                select ta.ActivityDate).Take(1).FirstOrDefault(),
                                    ExpireDateValidation = (from ta in _context.ToolActivities
                                                            join a in _context.Activities on ta.ActivityId equals a.Id
                                                            where ta.ToolId == t.Id && (EF.Functions.Like(a.Name.ToLower(), "%" + ApplicationConstant.TOOL_ACTIVITY_VALIDATION_LABEL.ToLower() + "%"))
                                                            orderby ta.ExpiredDate descending
                                                            select ta.ExpiredDate).Take(1).FirstOrDefault(),
                                    ActivityDateCalibration = (from ta in _context.ToolActivities
                                                                join a in _context.Activities on ta.ActivityId equals a.Id
                                                                where ta.ToolId == t.Id && (EF.Functions.Like(a.Name.ToLower(), "%" + ApplicationConstant.TOOL_ACTIVITY_CALIBRATION_LABEL.ToLower() + "%"))
                                                                orderby ta.ExpiredDate descending
                                                                select ta.ActivityDate).Take(1).FirstOrDefault(),
                                    ExpireDateCalibration = (from ta in _context.ToolActivities
                                                                join a in _context.Activities on ta.ActivityId equals a.Id
                                                                where ta.ToolId == t.Id && (EF.Functions.Like(a.Name.ToLower(), "%" + ApplicationConstant.TOOL_ACTIVITY_CALIBRATION_LABEL.ToLower() + "%"))
                                                                orderby ta.ExpiredDate descending
                                                                select ta.ExpiredDate).Take(1).FirstOrDefault(),

                                }).AsQueryable();

            if (facilityId != null)
            {
                query = query.Where(x => x.FacilityId == facilityId);
            }

            var result = await query.ToListAsync();

            if (result.Any())
            {
                foreach (var r in result)
                {
                    r.ToolActivity = await (from ta in _context.ToolActivities
                                            join act in _context.Activities on ta.ActivityId equals act.Id
                                            where ta.ToolId == r.ToolId && ta.RowStatus == null
                                            group new { ta, act } by new { ta.ActivityId } into g
                                            select new ToolActivityData
                                            {
                                                ToolActivityId = g.Max(x => x.ta.Id),
                                                ToolActivityCode = g.Max(x => x.ta.ActivityCode),
                                                ActivityId = g.Max(x => x.ta.ActivityId),
                                                ActivityCode = g.Max(x => x.act.Code),
                                                ActivityName = g.Max(x => x.act.Name),
                                                ActivityDate = g.Max(x => x.ta.ActivityDate),
                                                ExpiredDate = g.Max(x => x.ta.ExpiredDate),
                                            }).ToListAsync();
                }
            }

            var resultData = new List<ToolRelationViewModel>();

            resultData = result.Where(x => (x.ExpireDateCalibration >= startDate || !startDate.HasValue)
                                        && (x.ExpireDateCalibration >= endDate || !endDate.HasValue)
                                        && (ToolGroupId.Any() ? ToolGroupId.Contains(x.ToolGroupId) : ToolGroupId == null)
                                        //&& (x.GradeRoomId == GradeRoomId || GradeRoomId == 0)
                                        )
                .OrderBy(x => x.ToolCode).ToList();

            return resultData;
        }

        public async Task<ToolRelationViewModel> GetToolById(int id)
        {
            var result = await (from t in _context.VToolActivities
                                where t.ToolId == id
                                select new ToolRelationViewModel
                                {
                                    ToolId = t.ToolId,
                                    ToolCode = t.ToolCode,
                                    ToolName = t.ToolName,
                                    ToolGroupId = t.ToolGroupId,
                                    ToolGroupName = t.ToolGroupName,
                                    ToolGroupLabel = t.ToolGroupLabel,
                                    RoomId = t.RoomId,
                                    RoomName = t.RoomName,
                                    GradeRoomId = t.GradeRoomId,
                                    GradeRoomName = t.GradeRoomName,
                                    ActivityDateValidation = t.ActivityDateValidation,
                                    ExpireDateValidation = t.ExpireDateValidation,
                                    ActivityDateCalibration = t.ActivityDateCalibration,
                                    ExpireDateCalibration = t.ExpireDateCalibration
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<ShortDataListViewModel>> ShortList(string search, int GroupId, string groupName, int? facilityId)
        {
            var result = new List<ShortDataListViewModel>();
            string filter = "";
            string filterGroupName = "";
            if (search != null)
                filter = search.ToLower();
            if (groupName != null)
                filterGroupName = groupName.ToLower();

            if (facilityId != null)
            {
                result = await (from t in _context.Tools
                                join tg in _context.ToolGroups on t.ToolGroupId equals tg.Id
                                where (EF.Functions.Like(t.Name.ToLower(), "%" + filter + "%"))
                                && (EF.Functions.Like(tg.Name.ToLower(), "%" + filterGroupName + "%"))
                                && t.RowStatus == null
                                && t.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                && t.FacilityId == facilityId.Value
                                select new ShortDataListViewModel
                                {
                                    Id = t.Id,
                                    Label = t.Name,
                                    Code = t.ToolCode,
                                    GroupId = t.ToolGroupId,
                                    GroupName = tg.Name
                                }).Where(x => x.GroupId == GroupId || GroupId == 0).OrderBy(x => x.Label).ToListAsync();
            }
            else
            {
                result = await (from t in _context.Tools
                                join tg in _context.ToolGroups on t.ToolGroupId equals tg.Id
                                where (EF.Functions.Like(t.Name.ToLower(), "%" + filter + "%"))
                                && (EF.Functions.Like(tg.Name.ToLower(), "%" + filterGroupName + "%"))
                                && t.RowStatus == null
                                && t.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                select new ShortDataListViewModel
                                {
                                    Id = t.Id,
                                    Label = t.Name,
                                    Code = t.ToolCode,
                                    GroupId = t.ToolGroupId,
                                    GroupName = tg.Name
                                }).Where(x => x.GroupId == GroupId || GroupId == 0).OrderBy(x => x.Label).ToListAsync();
            }


            return result;
        }

        public async Task<Tool> GetByCode(string code)
        {
            var tool = await _context.Tools.FirstOrDefaultAsync(x => x.ToolCode == code);
            return tool;
        }

        public async Task<Tool> Insert(Tool tool)
        {
            await _context.Tools.AddAsync(tool);
            await _context.SaveChangesAsync();
            return tool;
        }

        public async Task<Tool> Update(Tool tool)
        {
            _context.Tools.Update(tool);
            await _context.SaveChangesAsync();
            return tool;
        }

        public async Task<List<int>> ToolGroupIds()
        {
            var result = await (from a in _context.ToolGroups
                                where a.RowStatus == null
                                select a.Id).ToListAsync();

            return result;
        }

        public async Task<List<ToolGroupViewModel>> ToolGroupList(string search)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from tg in _context.ToolGroups
                                where ((EF.Functions.Like(tg.Label.ToLower(), "%" + filter + "%")) ||
                                    (EF.Functions.Like(tg.Code.ToLower(), "%" + filter + "%")) ||
                                    (EF.Functions.Like(tg.Name.ToLower(), "%" + filter + "%")))
                                && tg.RowStatus == null
                                select new ToolGroupViewModel
                                {
                                    Id = tg.Id,
                                    Code = tg.Code,
                                    Label = tg.Label,
                                    Name = tg.Name,
                                    CreatedAt = tg.CreatedAt,
                                    CreatedBy = tg.CreatedBy
                                }).ToListAsync();

            return result;
        }

        public async Task<ToolsAHUViewModel> getAhuById(int id)
        {
            var result = await (from t in _context.Tools
                                join tg in _context.ToolGroups on t.ToolGroupId equals tg.Id
                                where tg.Code == ApplicationConstant.TOOL_GROUP_CODE_AHU
                                && t.Id == id
                                && t.RowStatus == null
                                && t.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                select new ToolsAHUViewModel
                                {
                                    ToolId = t.Id,
                                    ToolCode = t.ToolCode,
                                    ToolName = t.Name
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ToolDetailViewModel> GetToolDetailById(int toolid)
        {
            var toolDetail = await (from t in _context.Tools
                where t.Id == toolid
                select new ToolDetailViewModel()
                {
                    Id = t.Id,
                    Code = t.ToolCode,
                    Name = t.Name,
                    RoomId = t.RoomId,
                    FacilityId = t.FacilityId,
                    GradeRoomId = t.GradeRoomId,
                    ToolGroupId = t.ToolGroupId,
                    MachineId = t.MachineId,
                    SerialNumberId = t.SerialNumberId,
                    Activities = (from ta in _context.ToolActivities
                                    join act in _context.Activities on ta.ActivityId equals act.Id
                                    where ta.ToolId == t.Id
                                    select new ToolActivityRelationViewModel
                                    {
                                        ActivityId = ta.ActivityId.ToString(),
                                        ActivityName = act.Name,
                                        ActivityCode = ta.ActivityCode,
                                        ActivityDate = ta.ActivityDate,
                                        ExpiredDate = ta.ExpiredDate,
                                        CreatedAt = ta.CreatedAt,
                                        CreatedBy = ta.CreatedBy
                                    }).ToList(),
                    //ListDataPurposes = null
                }).FirstOrDefaultAsync();

            var toolPurposes = await (from rp in _context.ToolPurposes
                where rp.ToolId == toolid
                select new ToolPurposesDatas()
                {
                    Id = rp.Id,
                    Purpose = (from mp in _context.ToolPurposeToMasterPurposes
                                join p in _context.Purposes on mp.PurposeId equals p.Id
                                where mp.ToolPurposeId == rp.Id
                                select p).ToList(),
                    //SamplingPoints = null,
                    SamplingPointLayout = (from rsp in _context.ToolSamplingPointLayout
                                            where rsp.ToolPurposeId == rp.Id
                                            select rsp).ToList()
                }).ToListAsync();
            var toolPurposeIds = toolPurposes.Select(x => x.Id).Distinct().ToList();

            var samplingPoints = await (from rts in _context.RelSamplingTools
                join sp in _context.SamplingPoints on rts.SamplingPointId equals sp.Id
                //where rps.RoomPurposeId == rp.Id
                where toolPurposeIds.Contains(rts.ToolPurposeId ?? 0)
                select new ToolSamplingPointViewModel()
                {
                    SamplingPointId = sp.Id,
                    Code = sp.Code,
                    RoomId = sp.RoomId,
                    ToolId = 0,
                    ScenarioLabel = rts.ScenarioLabel,
                    ToolPurposeId = rts.ToolPurposeId ?? 0,
                    //TestParameter = null
                }).ToListAsync();
            var samplingPointIds = samplingPoints.Select(x => x.SamplingPointId).Distinct().ToList();

            var testParameters = await (from tp in _context.TestParameters
                join rtsp in _context.RelTestScenarioParams on tp.Id equals rtsp.TestParameterId
                join rstp in _context.RelSamplingTestParams on rtsp.Id equals rstp.TestScenarioParamId
                //where rstp.SamplingPointId == sp.Id
                where samplingPointIds.Contains(rstp.SamplingPointId)
                select new TestParameterTool()
                {
                    Id = tp.Id,
                    TestParameterName = tp.Name,
                    SamplingPointId = rstp.SamplingPointId,
                    TestVariables = (from tv in _context.TestVariables
                        join rtsp2 in _context.RelTestScenarioParams on tv.TestParameterId equals rtsp2.Id
                        where rtsp2.TestParameterId == tp.Id
                        select tv).Distinct().ToList()
                })
                //.Distinct()
                .ToListAsync();

            toolDetail.ListDataPurposes = toolPurposes;
            foreach (var itemPurpose in toolDetail.ListDataPurposes)
            {
                itemPurpose.SamplingPoints = samplingPoints.Where(x => x.ToolPurposeId == itemPurpose.Id).ToList();

                foreach (var itemSp in itemPurpose.SamplingPoints)
                {
                    var filteredTestParams = testParameters.Where(x => x.SamplingPointId == itemSp.SamplingPointId).ToList();

                    //get unique test parameter
                    itemSp.TestParameter = filteredTestParams.GroupBy(x => new { x.Id })
                        .Select(x => x.First())
                        .ToList();
                }
            }

            return toolDetail;
        }

        public async Task<List<ToolDetailViewModel>> GetToolDetailByRoomId(int roomId)
        {
            var toolDetails = await (from t in _context.Tools
                          where t.RoomId == roomId
                          && t.GradeRoomId != 0
                          select new ToolDetailViewModel()
                          {
                              Id = t.Id,
                              Code = t.ToolCode,
                              Name = t.Name,
                              RoomId = t.RoomId,
                              FacilityId = t.FacilityId,
                              GradeRoomId = t.GradeRoomId,
                              ToolGroupId = t.ToolGroupId,
                              MachineId = t.MachineId,
                              SerialNumberId = t.SerialNumberId,
                              Activities = (from ta in _context.ToolActivities
                                            join act in _context.Activities on ta.ActivityId equals act.Id
                                            where ta.ToolId == t.Id
                                            select new ToolActivityRelationViewModel
                                            {
                                                ActivityId = ta.ActivityId.ToString(),
                                                ActivityName = act.Name,
                                                ActivityCode = ta.ActivityCode,
                                                ActivityDate = ta.ActivityDate,
                                                ExpiredDate = ta.ExpiredDate,
                                                CreatedAt = ta.CreatedAt,
                                                CreatedBy = ta.CreatedBy
                                            }).ToList(),
                              ListDataPurposes = (from rp in _context.ToolPurposes
                                                  where rp.ToolId == t.Id
                                                  select new ToolPurposesDatas()
                                                  {
                                                      Id = rp.Id,
                                                      Purpose = (from mp in _context.ToolPurposeToMasterPurposes
                                                                 join p in _context.Purposes on mp.PurposeId equals p.Id
                                                                 where mp.ToolPurposeId == rp.Id
                                                                 select p).ToList(),
                                                      SamplingPoints = (from rps in _context.RelSamplingTools
                                                                        join sp in _context.SamplingPoints on rps.SamplingPointId equals sp.Id
                                                                        where rps.ToolPurposeId == rp.Id
                                                                        select new ToolSamplingPointViewModel()
                                                                        {
                                                                            SamplingPointId = sp.Id,
                                                                            Code = sp.Code,
                                                                            RoomId = 0,
                                                                            ToolId = sp.ToolId,
                                                                            ScenarioLabel = rps.ScenarioLabel,
                                                                            TestParameter = (from tp in _context.TestParameters
                                                                                             join rtsp in _context.RelTestScenarioParams on tp.Id equals rtsp.TestParameterId
                                                                                             join rstp in _context.RelSamplingTestParams on rtsp.Id equals rstp.TestScenarioParamId
                                                                                             where rstp.SamplingPointId == sp.Id
                                                                                             select new TestParameterTool()
                                                                                             {
                                                                                                 Id = tp.Id,
                                                                                                 TestParameterName = tp.Name,
                                                                                                 TestVariables = (from tv in _context.TestVariables
                                                                                                                  join rtsp2 in _context.RelTestScenarioParams on tv.TestParameterId equals rtsp2.Id
                                                                                                                  where rtsp2.TestParameterId == tp.Id
                                                                                                                  select tv).Distinct().ToList()
                                                                                             }).Distinct().ToList()
                                                                        }).ToList(),
                                                      SamplingPointLayout = (from rsp in _context.ToolSamplingPointLayouts
                                                                             where rsp.ToolPurposeId == rp.Id
                                                                             select new ToolSamplingPointLayout()
                                                                             {
                                                                                 //RoomPurposeId = rsp.ToolPurposeId,
                                                                                 AttachmentFile = rsp.AttachmentFile,
                                                                                 FileName = rsp.FileName,
                                                                                 FileType = rsp.FileType,
                                                                                 CreatedAt = rsp.CreatedAt,
                                                                                 CreatedBy = rsp.CreatedBy
                                                                             }).ToList()
                                                  }).ToList()
                          }).ToListAsync();

            return toolDetails;
        }
    }
}
