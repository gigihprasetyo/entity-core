using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.IntegrationEvents;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using qcs_product.Constants;
using qcs_product.API.Infrastructure;
using System.Collections.Generic;

namespace qcs_product.API.EventHandlers
{
    public class ToolEventHandler : IIntegrationEventHandler<ToolIntegrationEvent>
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<ToolEventHandler> _logger;
        private readonly IToolDataProvider _toolDataProvider;
        private readonly IGradeRoomDataProvider _gradeRoomDataProvider;
        private readonly IRoomDataProvider _roomDataProvider;
        private readonly IToolGroupDataProvider _toolGroupDataProvider;
        private readonly IToolActivityDataProvider _toolActivityDataProvider;
        private readonly IActivityDataProvider _activityDataProvider;
        private readonly IOrganizationDataProvider _organizationDataProvider;
        private readonly ISamplingPointDataProvider _samplingPointDataProvider;
        private readonly ITestScenarioDataProvider _testScenarioDataProvider;
        private readonly IFacilityDataProvider _facilityDataProvider;
        private readonly GradeRoomEventHandler _gradeRoomEventHandler;
        private readonly IPurposeDataProvider _purposeDataProvider;
        private readonly IToolPurposeDataProvider _toolPurposeDataProvider;
        private readonly IToolPurposeToMasterPurposeDataProvider _toolPurposeMasterPurposeDataProvider;
        private readonly IToolSamplingPointLayoutDataProvider _toolSamplingPointLayoutDataProvider;
        private readonly IRelSamplingToolDataProvider _relSamplingToolDataProvider;
        private readonly IRelSamplingTestParamDataProvider _relSamplingTestParamDataProvider;

        public ToolEventHandler(QcsProductContext context, ILogger<ToolEventHandler> logger, IToolDataProvider toolDataProvider,
            IGradeRoomDataProvider gradeRoomDataProvider, IRoomDataProvider roomDataProvider, IToolGroupDataProvider toolGroupDataProvider, IToolActivityDataProvider toolActivityDataProvider, IActivityDataProvider activityDataProvider, IOrganizationDataProvider organizationDataProvider, ISamplingPointDataProvider samplingPointDataProvider, ITestScenarioDataProvider testScenarioDataProvider, GradeRoomEventHandler gradeRoomEventHandler, IFacilityDataProvider facilityDataProvider, IPurposeDataProvider purposeDataProvider, IToolPurposeDataProvider toolPurposeDataProvider, IToolPurposeToMasterPurposeDataProvider toolPurposeMasterPurposeDataProvider, IToolSamplingPointLayoutDataProvider toolSamplingPointLayoutDataProvider, IRelSamplingToolDataProvider relSamplingToolDataProvider, IRelSamplingTestParamDataProvider relSamplingTestParamDataProvider)
        {
            _context = context;
            _logger = logger;
            _toolDataProvider = toolDataProvider;
            _gradeRoomDataProvider = gradeRoomDataProvider;
            _roomDataProvider = roomDataProvider;
            _toolGroupDataProvider = toolGroupDataProvider;
            _toolActivityDataProvider = toolActivityDataProvider;
            _activityDataProvider = activityDataProvider;
            _organizationDataProvider = organizationDataProvider;
            _samplingPointDataProvider = samplingPointDataProvider;
            _testScenarioDataProvider = testScenarioDataProvider;
            _gradeRoomEventHandler = gradeRoomEventHandler;
            _facilityDataProvider = facilityDataProvider;
            _purposeDataProvider = purposeDataProvider;
            _toolPurposeDataProvider = toolPurposeDataProvider;
            _toolPurposeMasterPurposeDataProvider = toolPurposeMasterPurposeDataProvider;
            _toolSamplingPointLayoutDataProvider = toolSamplingPointLayoutDataProvider;
            _relSamplingToolDataProvider = relSamplingToolDataProvider;
            _relSamplingTestParamDataProvider = relSamplingTestParamDataProvider;
        }

        public async Task Handle(ToolIntegrationEvent @event)
        {
            _logger.LogInformation("sync tool from google pub/sub - begin");

            try
            {
                var data = JsonSerializer.Serialize(@event);
                _logger.LogInformation(data);

                #region insert organization if not exists 
                _logger.LogInformation("insert organization if not exists");

                Organization organization = null;

                if (!string.IsNullOrEmpty(@event.OrganizationCode))
                {
                    organization = await _organizationDataProvider.GetDetailByCode(@event.OrganizationCode, true);
                    if (organization == null)
                    {
                        organization = new Organization();
                        organization.OrgCode = @event.OrganizationCode;
                        organization.Name = @event.OrganizationName;
                        organization.BIOHROrganizationId = @event.BIOHROrganizationId;
                        organization.CreatedAt = DateTime.Now;
                        organization.UpdatedAt = DateTime.Now;
                        organization.CreatedBy = @event.CreatedBy;
                        organization.UpdatedBy = @event.UpdatedBy;
                        organization = await _organizationDataProvider.Insert(organization);
                    }
                }
                #endregion

                #region  insert grade room if not exists
                _logger.LogInformation("insert grade room if not exists");

                GradeRoom gradeRoom = null;

                if (!string.IsNullOrEmpty(@event.GradeRoomCode))
                {
                    gradeRoom = await _gradeRoomDataProvider.GetByCode(@event.GradeRoomCode);
                    if (gradeRoom == null)
                    {
                        await _gradeRoomEventHandler.Handle(@event.GradeRoom);
                    }
                }
                #endregion

                #region insert room if not exists
                _logger.LogInformation("insert room if not exists");




                // if (string.IsNullOrEmpty(@event.RoomCode))
                // {
                //     throw new Exception("Can not insert tool. Room code is empty");
                // }

                Room room = null;
                if (@event.GradeRoomCode != null)
                {
                    gradeRoom = await _gradeRoomDataProvider.GetByCode(@event.GradeRoomCode);
                }
                if (@event.RoomCode != null)
                {
                    room = await _roomDataProvider.GetByCode(@event.RoomCode);
                    if (room == null)
                    {
                        if (organization == null)
                        {
                            throw new Exception("Can not insert room. Organization is empty");
                        }

                        // if (string.IsNullOrEmpty(@event.GradeRoomCodeOfRoom))
                        // {
                        //     throw new Exception("Can not insert room. Grade room code is empty");
                        // }

                        if (@event.GradeRoomCodeOfRoom != null)
                        {
                            var gradeRoomOfRoom = await _gradeRoomDataProvider.GetByCode(@event.GradeRoomCodeOfRoom);

                            if (gradeRoomOfRoom == null)
                            {
                                gradeRoomOfRoom = new GradeRoom();
                                gradeRoomOfRoom.Code = @event.GradeRoomCodeOfRoom;
                                gradeRoomOfRoom.Name = @event.GradeRoomNameOfRoom;
                                gradeRoomOfRoom.ObjectStatus = ApplicationConstant.OBJECT_STATUS_ACTIVE;
                                gradeRoomOfRoom.CreatedAt = DateTime.Now;
                                gradeRoomOfRoom.UpdatedAt = DateTime.Now;
                                gradeRoomOfRoom.CreatedBy = @event.CreatedBy;
                                gradeRoomOfRoom.UpdatedBy = @event.UpdatedBy;

                                gradeRoomOfRoom = await _gradeRoomDataProvider.Insert(gradeRoomOfRoom);
                            }

                            room = new Room();
                            room.Code = @event.RoomCode;
                            room.Name = @event.RoomName;
                            room.CreatedAt = DateTime.Now;
                            room.UpdatedAt = DateTime.Now;
                            room.CreatedBy = @event.CreatedBy;
                            room.UpdatedBy = @event.UpdatedBy;
                            room.ObjectStatus = ApplicationConstant.OBJECT_STATUS_ACTIVE;
                            room.GradeRoomId = gradeRoomOfRoom.Id;
                            room.OrganizationId = organization.Id;

                            room = await _roomDataProvider.Insert(room);
                        }


                    }
                    else
                    {
                        var doUpdateRoom = false;
                        if (room.OrganizationId == 0 && organization != null)
                        {
                            doUpdateRoom = true;
                            room.OrganizationId = organization.Id;
                        }

                        if (@event.GradeRoomCode != null)
                        {
                            if (room.GradeRoomId == 0 && gradeRoom != null)
                            {
                                doUpdateRoom = true;
                                room.GradeRoomId = gradeRoom.Id;
                            }
                        }

                        if (doUpdateRoom)
                        {
                            await _roomDataProvider.Update(room);
                        }
                    }

                }
                #endregion

                #region insert tool group if not exists
                _logger.LogInformation("insert tool group if not exists");


                ToolGroup toolGroup = null;

                if (!string.IsNullOrEmpty(@event.ToolGroupCode))
                {
                    toolGroup = await _toolGroupDataProvider.GetByCode(@event.ToolGroupCode);
                    if (toolGroup == null)
                    {
                        toolGroup = new ToolGroup();
                        toolGroup.Code = @event.ToolGroupCode;
                        toolGroup.Name = @event.ToolGroupName;
                        toolGroup.Label = @event.ToolGroupName;
                        toolGroup.ObjectStatus = 0;//TODO seharusnya di-set apa ??
                        toolGroup.CreatedAt = DateTime.Now;
                        toolGroup.UpdatedAt = DateTime.Now;
                        toolGroup.CreatedBy = @event.CreatedBy;
                        toolGroup.UpdatedBy = @event.UpdatedBy;
                        toolGroup = await _toolGroupDataProvider.Insert(toolGroup);
                    }
                }
                #endregion

                #region facility
                var facailityId = 0;
                var facility = await _facilityDataProvider.GetByCode(@event.FacilityCode);
                if (facility != null)
                {
                    facailityId = facility.Id;
                }
                else
                {
                    facailityId = 0;
                    //throw new Exception("Can not insert facility. Organization is empty");
                }

                await Task.Delay(2000);

                #endregion

                #region insert or update tool
                _logger.LogInformation("insert or update tool if not exists");

                if (string.IsNullOrEmpty(@event.Code))
                {
                    throw new Exception("Tool code is empty");
                }

                var isNew = false;
                var tool = await _toolDataProvider.GetByCode(@event.Code);

                if (tool == null)
                {
                    if (toolGroup == null)
                    {
                        throw new Exception("Can not insert tool. Tool group is empty");
                    }

                    // if (gradeRoom == null)
                    // {
                    //     throw new Exception("Can not insert tool. Grade room is empty");
                    // }

                    isNew = true;
                    tool = new Tool();
                    tool.CreatedAt = DateTime.Now;
                    tool.CreatedBy = @event.CreatedBy;
                    tool.ToolCode = @event.Code;
                }

                if (toolGroup != null)
                {
                    tool.ToolGroupId = toolGroup.Id;
                }

                tool.Name = @event.Name;

                if (room != null)
                {
                    tool.RoomId = room.Id;
                }
                else
                {
                    tool.RoomId = 0;
                }

                if (gradeRoom != null)
                {
                    tool.GradeRoomId = gradeRoom.Id;
                }
                else
                {
                    tool.GradeRoomId = 0;
                }

                if (!string.IsNullOrEmpty(@event.MachineCode))
                {
                    var machine = await _toolDataProvider.GetByCode(@event.MachineCode);
                    if (machine != null)
                    {
                        tool.MachineId = machine.Id;
                    }
                }

                tool.UpdatedAt = DateTime.Now;
                tool.FacilityId = facailityId;
                tool.UpdatedBy = @event.UpdatedBy;
                tool.RowStatus = @event.RowStatus;
                tool.ObjectStatus = @event.ObjectStatus;
                tool.SerialNumberId = @event.SerialNumberId;

                if (isNew)
                {
                    tool = await _toolDataProvider.Insert(tool);
                }
                else
                {
                    await _toolDataProvider.Update(tool);
                }

                #endregion

                #region insert or update activity and tool 

                _logger.LogInformation("insert or update tool activity if not exists");

                if (@event.Activities != null && @event.Activities.Any())
                {
                    //1. jika ada existing activity yang tidak ada di list activity yang di-sync maka set row_status menjadi deleted
                    //2. jika ada existing activity yang row_status = deleted tapi ada di list activity yang akan di-sync maka set row_status menjadi null
                    var activityCodes = @event.Activities.Select(x => x.ActivityCode).Distinct().ToList();

                    var currActivities = await _toolActivityDataProvider.GetListByToolId(tool.Id);

                    var deleteActivities = currActivities.Where(x => !activityCodes.Contains(x.ActivityCode));

                    foreach (var deleteActivity in deleteActivities)
                    {
                        deleteActivity.RowStatus = ApplicationConstant.ROW_STATUS_DELETE;
                        await _toolActivityDataProvider.Update(deleteActivity);
                    }

                    var activities = await _activityDataProvider.GetListByCodes(activityCodes);

                    foreach (var eventActivity in @event.Activities)
                    {
                        var activity = activities.FirstOrDefault(x => x.Code == eventActivity.ActivityCode);
                        if (activity == null)
                        {
                            activity = new Activity();
                            activity.Code = eventActivity.ActivityCode;
                            activity.Name = eventActivity.ActivityName;
                            activity.UpdatedAt = DateTime.Now;
                            activity.CreatedAt = DateTime.Now;
                            activity.UpdatedBy = @event.UpdatedBy;
                            activity.CreatedBy = @event.CreatedBy;
                            activity = await _activityDataProvider.Insert(activity);

                            activities.Add(activity);
                        }

                        var isNewToolActivity = false;
                        var toolActivity = currActivities.FirstOrDefault(x => x.ToolId == tool.Id && x.ActivityId == activity.Id);
                        if (toolActivity == null)
                        {
                            isNewToolActivity = true;
                            toolActivity = new ToolActivity();
                            toolActivity.CreatedBy = @event.CreatedBy;
                            toolActivity.CreatedAt = DateTime.Now;
                        }

                        toolActivity.ActivityCode = eventActivity.ActivityCode;
                        toolActivity.ActivityDate = eventActivity.ActivityDate;
                        toolActivity.ExpiredDate = eventActivity.ExpiredDate;
                        toolActivity.UpdatedAt = DateTime.Now;
                        toolActivity.UpdatedBy = @event.UpdatedBy;
                        toolActivity.ToolId = tool.Id;
                        toolActivity.ActivityId = activity.Id;

                        if (isNewToolActivity)
                        {
                            await _toolActivityDataProvider.Insert(toolActivity);
                        }
                        else
                        {
                            await _toolActivityDataProvider.Update(toolActivity);
                        }
                    }
                }

                #endregion

                #region insert tool purposes

                _logger.LogInformation("insert tool purposes");

                _logger.LogInformation("delete tool purposes");
                await _relSamplingTestParamDataProvider.DeleteByToolCode(@event.Code);
                await _relSamplingToolDataProvider.DeleteByToolCode(@event.Code);
                await _toolPurposeMasterPurposeDataProvider.DeleteByToolCode(@event.Code);
                await _toolSamplingPointLayoutDataProvider.DeleteByToolCode(@event.Code);
                await _toolPurposeDataProvider.DeleteByToolCode(@event.Code);

                if (@event.ListDataPurposes != null)
                {

                    foreach (var evToolPurpose in @event.ListDataPurposes)
                    {

                        _logger.LogInformation("insert tool purposes - tool purposes");
                        ToolPurpose toolPurposeObj = new ToolPurpose
                        {
                            ToolId = tool.Id,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            CreatedBy = @event.CreatedBy,
                            UpdatedBy = @event.CreatedBy
                        };
                        toolPurposeObj = await _toolPurposeDataProvider.Insert(toolPurposeObj);

                        List<Purpose> lsPurposeObj = new List<Purpose>();
                        List<SamplingPoint> lsSamplingPointObj = new List<SamplingPoint>();

                        _logger.LogInformation("insert tool purposes - master purposes");
                        #region Master Purpose
                        if (evToolPurpose.Purpose != null)
                        {
                            foreach (var evPurpose in evToolPurpose.Purpose)
                            {
                                var purposeObj = await _purposeDataProvider.GetByCode(evPurpose.Code);

                                if (purposeObj == null)
                                {
                                    Purpose purpose = new Purpose
                                    {
                                        Code = evPurpose.Code,
                                        Name = evPurpose.Name,
                                        UpdatedAt = DateTime.Now,
                                        CreatedAt = DateTime.Now,
                                        UpdatedBy = @event.UpdatedBy,
                                        CreatedBy = @event.CreatedBy
                                    };
                                    purposeObj = await _purposeDataProvider.Insert(purpose);
                                }
                                lsPurposeObj.Add(purposeObj);
                            }
                        }
                        #endregion

                        _logger.LogInformation("insert tool purposes - master sampling point");
                        #region Master Sampling Point
                        if (evToolPurpose.SamplingPoints != null)
                        {
                            var testScenarios = await _testScenarioDataProvider.GetByGradeRoomId(gradeRoom.Id);
                            foreach (var evSamplingPoint in evToolPurpose.SamplingPoints)
                            {
                                var samplingPointObj = await _samplingPointDataProvider.GetByCode(evSamplingPoint.SamplingPointName);
                                if (samplingPointObj == null)
                                {
                                    samplingPointObj = new SamplingPoint()
                                    {
                                        Code = evSamplingPoint.SamplingPointName,
                                        RoomId = room.Id,
                                        ToolId = tool.Id,
                                        CreatedAt = DateTime.Now,
                                        CreatedBy = @event.CreatedBy
                                    };

                                    samplingPointObj = await _samplingPointDataProvider.Insert(samplingPointObj);
                                }
                                lsSamplingPointObj.Add(samplingPointObj);

                                #region rel_sampling_test_param
                                List<RelSamplingTestParam> lsRelSamplingTestParamObj = new List<RelSamplingTestParam>();

                                if (evSamplingPoint.TestParameters != null)
                                {
                                    foreach (var evTestParameter in evSamplingPoint.TestParameters)
                                    {
                                        foreach (var testScenario in testScenarios)
                                        {
                                            var relTestScenarioParam = await _testScenarioDataProvider.GetRelTestScenarioParam(testScenario.Id, evTestParameter.DataId);

                                            _logger.LogInformation("rel_test_scenario_param");
                                            _logger.LogInformation(JsonSerializer.Serialize(relTestScenarioParam));

                                            RelSamplingTestParam relSamplingTestParamObj = new RelSamplingTestParam()
                                            {
                                                SamplingPointId = samplingPointObj.Id,
                                                TestScenarioParamId = relTestScenarioParam.Id,
                                                CreatedAt = DateTime.Now,
                                                CreatedBy = @event.CreatedBy
                                            };

                                            lsRelSamplingTestParamObj.Add(relSamplingTestParamObj);
                                        }
                                    }
                                }

                                await _relSamplingTestParamDataProvider.InsertList(lsRelSamplingTestParamObj);
                                #endregion

                            }
                        }
                        #endregion

                        _logger.LogInformation("insert tool purposes - rel purpose to master purpose");
                        #region Rel Purpose To Master Purpose
                        List<ToolPurposeToMasterPurpose> lsToolPurposeToMasterPurposeObj = new List<ToolPurposeToMasterPurpose>();
                        if (lsPurposeObj != null)
                        {
                            foreach (var purposeObj in lsPurposeObj)
                            {
                                ToolPurposeToMasterPurpose toolPurposeToMasterPurposeObj = new ToolPurposeToMasterPurpose()
                                {
                                    ToolPurposeId = toolPurposeObj.Id,
                                    PurposeId = purposeObj.Id,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    CreatedBy = @event.CreatedBy,
                                    UpdatedBy = @event.CreatedBy
                                };

                                lsToolPurposeToMasterPurposeObj.Add(toolPurposeToMasterPurposeObj);
                            }
                        }
                        await _toolPurposeMasterPurposeDataProvider.InsertList(lsToolPurposeToMasterPurposeObj);
                        #endregion

                        _logger.LogInformation("insert tool purposes - rel purpose to sampling point");
                        #region Rel Purpose To Sampling Point
                        List<RelSamplingTool> lsRelSamplingToolObj = new List<RelSamplingTool>();
                        if (evToolPurpose.SamplingPoints != null)
                        {
                            foreach (var evSamplingPoint in evToolPurpose.SamplingPoints)
                            {
                                int samplingPointId = lsSamplingPointObj
                                    .Where(x => x.Code == evSamplingPoint.SamplingPointName)
                                    .Select(x => x.Id)
                                    .FirstOrDefault();

                                RelSamplingTool relSamplingToolObj = new RelSamplingTool()
                                {
                                    ToolPurposeId = toolPurposeObj.Id,
                                    ScenarioLabel = (evSamplingPoint.ScenarioLabel == "In Operation"
                                                ? "in_operation"
                                                : (evSamplingPoint.ScenarioLabel == "At Rest"
                                                ? "at_rest"
                                                : (evSamplingPoint.ScenarioLabel == "at_rest"
                                                ? "at_rest"
                                                : (evSamplingPoint.ScenarioLabel == "in_operation"
                                                ? "in_operation"
                                                : null)))),
                                    SamplingPointId = samplingPointId,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = @event.CreatedBy
                                };

                                lsRelSamplingToolObj.Add(relSamplingToolObj);
                            }
                        }
                        await _relSamplingToolDataProvider.InsertList(lsRelSamplingToolObj);
                        #endregion

                        _logger.LogInformation("insert tool purposes - rel purpose to sampling point layout");
                        #region Rel Purpose To Sampling Point Layout
                        List<ToolSamplingPointLayout> lsToolSamplingPointLayoutObj = new List<ToolSamplingPointLayout>();
                        if (evToolPurpose.SamplingPointLayout != null)
                        {
                            foreach (var evSamplingPointLayout in evToolPurpose.SamplingPointLayout)
                            {
                                ToolSamplingPointLayout toolSamplingPointLayoutObj = new ToolSamplingPointLayout()
                                {
                                    ToolPurposeId = toolPurposeObj.Id,
                                    AttachmentFile = evSamplingPointLayout.AttachmentFile,
                                    FileName = evSamplingPointLayout.FileName,
                                    FileType = evSamplingPointLayout.FileType,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    CreatedBy = @event.CreatedBy,
                                    UpdatedBy = @event.CreatedBy
                                };

                                lsToolSamplingPointLayoutObj.Add(toolSamplingPointLayoutObj);
                            }
                        }
                        await _toolSamplingPointLayoutDataProvider.InsertList(lsToolSamplingPointLayoutObj);
                        #endregion
                    }
                }
                #endregion

                _logger.LogInformation("sync tool from google pub/sub - end");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Pubsub Tool Error. {Message}", e.Message);
            }
        }
    }
}