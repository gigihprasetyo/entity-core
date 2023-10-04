using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.IntegrationEvents;
using qcs_product.API.DataProviders;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;

namespace qcs_product.API.EventHandlers
{
    public class RoomEventHandler : IIntegrationEventHandler<RoomIntegrationEvent>
    {
        private readonly QcsProductContext _context;
        private readonly IRoomDataProvider _dataProvider;
        private readonly IGradeRoomDataProvider _gradeRoomDataProvider;
        private readonly ILogger<RoomEventHandler> _logger;
        private readonly IOrganizationDataProvider _organizationDataProvider;
        private readonly ISamplingPointDataProvider _samplingPointDataProvider;
        private readonly ITestScenarioDataProvider _testScenarioDataProvider;
        private readonly GradeRoomEventHandler _gradeRoomEventHandler;
        private readonly IRoomSamplingPointLayoutDataProvider _roomSamplingPointLayoutDataProvider;
        private readonly IToolDataProvider _toolDataProvider;
        private readonly IRoomPurposeDataProvider _roomPurposeDataProvider;
        private readonly IRoomPurposeToMasterPurposeDataProvider _roomPurposeToMasterPurposeDataProvider;
        private readonly IPurposeDataProvider _purposeDataProvider;


        public RoomEventHandler(QcsProductContext context, IRoomDataProvider dataProvider, ILogger<RoomEventHandler> logger, IGradeRoomDataProvider gradeRoomDataProvider, IOrganizationDataProvider organizationDataProvider, ISamplingPointDataProvider samplingPointDataProvider, ITestScenarioDataProvider testScenarioDataProvider, GradeRoomEventHandler gradeRoomEventHandler, IRoomSamplingPointLayoutDataProvider roomSamplingPointLayoutDataProvider, IToolDataProvider toolDataProvider, IRoomPurposeDataProvider roomPurposeDataProvider, IRoomPurposeToMasterPurposeDataProvider roomPurposeToMasterPurposeDataProvider, IPurposeDataProvider purposeDataProvider)
        {
            _context = context;
            _dataProvider = dataProvider;
            _logger = logger;
            _gradeRoomDataProvider = gradeRoomDataProvider;
            _organizationDataProvider = organizationDataProvider;
            _samplingPointDataProvider = samplingPointDataProvider;
            _testScenarioDataProvider = testScenarioDataProvider;
            _gradeRoomEventHandler = gradeRoomEventHandler;
            _roomSamplingPointLayoutDataProvider = roomSamplingPointLayoutDataProvider;
            _toolDataProvider = toolDataProvider;
            _roomPurposeDataProvider = roomPurposeDataProvider;
            _roomPurposeToMasterPurposeDataProvider = roomPurposeToMasterPurposeDataProvider;
            _purposeDataProvider = purposeDataProvider;
        }

        public async Task Handle(RoomIntegrationEvent @event)
        {

            _logger.LogInformation("sync room from google pub/sub - begin");

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

                if (organization == null)
                {
                    throw new Exception("Can not insert room. Organization is empty");
                }

                #endregion

                #region insert grade room if not exists

                GradeRoom gradeRoom = null;

                if (!string.IsNullOrEmpty(@event.GradeRoomCode))
                {
                    gradeRoom = await _gradeRoomDataProvider.GetByCode(@event.GradeRoomCode);

                    //jika grade room belum terdaftar maka lakukan proses insert grade room
                    if (gradeRoom == null)
                    {
                        await _gradeRoomEventHandler.Handle(@event.GradeRoom);
                    }
                }

                #endregion

                #region insert or update room
                gradeRoom = await _gradeRoomDataProvider.GetByCode(@event.GradeRoomCode);
                var room = await _dataProvider.GetByCode(@event.Code);

                var isNew = false;
                if (room == null)
                {
                    isNew = true;
                    room = new Room()
                    {
                        CreatedAt = DateTime.Now,
                        Code = @event.Code
                    };
                }

                //get data tool by ToolCode untuk AHU
                Tool tool = new Tool();
                if (@event.ToolCode != null)
                {
                    tool = await _toolDataProvider.GetByCode(@event.ToolCode);
                    if (tool == null)
                    {
                        throw new Exception("Can not insert room. Ahu/Equipment is empty");
                    }
                }

                room.Name = @event.Name;
                room.PosId = @event.PosId;
                room.GradeRoomId = (gradeRoom != null ? gradeRoom.Id : 0);
                room.OrganizationId = organization.Id;
                room.Ahu = tool.Id;
                room.Floor = @event.Floor;
                room.Area = @event.Area;
                room.HumidityOperator = @event.HumidityOperator;
                room.HumidityValue = @event.HumidityValue;
                room.HumidityValueFrom = @event.HumidityValueFrom;
                room.HumidityValueTo = @event.HumidityValueTo;
                room.PressureOperator = @event.PressureOperator;
                room.PressureValue = @event.PressureValue;
                room.PressureValueFrom = @event.PressureValueFrom;
                room.PressureValueTo = @event.PressureValueTo;
                room.TemperatureOperator = @event.TemperatureOperator;
                room.TemperatureValue = @event.TemperatureValue;
                room.TemperatureValueFrom = @event.TemperatureValueFrom;
                room.TemperatureValueTo = @event.TemperatureValueTo;
                room.AirChangeOperator = @event.AirChangeOperator;
                room.AirChangeValue = @event.AirChangeValue;
                room.AirChangeValueFrom = @event.AirChangeValueFrom;
                room.AirChangeValueTo = @event.AirChangeValueTo;

                room.CreatedBy = @event.CreatedBy;
                room.UpdatedBy = @event.UpdatedBy;
                room.ObjectStatus = @event.ObjectStatus;
                room.UpdatedAt = DateTime.Now;

                if (isNew)
                {
                    await _dataProvider.Insert(room);
                }
                else
                {
                    await _dataProvider.Update(room);
                }

                #endregion

                #region insert room purpose
                //cek data room-purpose nya kosong atau engk
                if (@event.ListDataPurposes.Any())
                {
                    //delete data purpose berdasarkan room 
                    _roomPurposeDataProvider.RemoveRange(@event.Code);
                    foreach (var item in @event.ListDataPurposes)
                    {
                        RoomPurpose insertRoomPurpose = new RoomPurpose()
                        {
                            RoomId = room.Id,
                            CreatedBy = @event.CreatedBy,
                            UpdatedBy = @event.CreatedBy,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        var insertRoomPurposeData = await _roomPurposeDataProvider.InsertRoomPurpose(insertRoomPurpose);

                        #region insert room-purpose master data purpose
                        foreach (var itemPurp in item.Purpose)
                        {
                            //cek apakah data purpose ada by purpose code
                            var getExistingPurpose = await _purposeDataProvider.GetByCode(itemPurp.Code);
                            var purposeExistingId = 0;

                            if ((getExistingPurpose != null) || (getExistingPurpose.Name != ""))
                            {
                                purposeExistingId = getExistingPurpose.Id;
                            }
                            else
                            {
                                Purpose newInsertPurpose = new Purpose()
                                {
                                    Code = itemPurp.Code,
                                    Name = itemPurp.Name,
                                    //RequestTypeId = 0,
                                    CreatedBy = @event.CreatedBy,
                                    UpdatedBy = @event.CreatedBy,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now
                                };

                                var newDataPurpose = await _purposeDataProvider.Insert(newInsertPurpose);
                                purposeExistingId = newDataPurpose.Id;
                            }

                            RoomPurposeToMasterPurpose insertRoomPurposeToMasterPurpose = new RoomPurposeToMasterPurpose()
                            {
                                PurposeId = purposeExistingId,
                                RoomPurposeId = insertRoomPurposeData.Id,
                                CreatedBy = @event.CreatedBy,
                                UpdatedBy = @event.CreatedBy,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            };

                            //buat insert into room purp to master purpose 
                            await _roomPurposeToMasterPurposeDataProvider.Insert(insertRoomPurposeToMasterPurpose);

                        }
                        #endregion

                        #region insert sampling point
                        if (item.SamplingPoints.Any())
                        {
                            foreach (var samplingPointEvent in item.SamplingPoints)
                            {
                                var samplingPointExist = await _samplingPointDataProvider.GetByCode(samplingPointEvent.SamplingPointName);
                                var samplingPointIdExisting = 0;
                                if (samplingPointExist == null)
                                {
                                    SamplingPoint insertNewSamplingPoint = new SamplingPoint()
                                    {
                                        Code = samplingPointEvent.SamplingPointName,
                                        RoomId = room.Id,
                                        ToolId = null,
                                        CreatedAt = DateTime.Now,
                                        CreatedBy = samplingPointEvent.CreatedBy
                                    };

                                    var newDataSamplingPoint = await _samplingPointDataProvider.Insert(insertNewSamplingPoint);
                                    samplingPointIdExisting = newDataSamplingPoint.Id;
                                }
                                else
                                {
                                    samplingPointIdExisting = samplingPointExist.Id;
                                }


                                #region rel_room_sampling
                                RelRoomSampling relRoomSampling = new RelRoomSampling()
                                {
                                    //RoomId = room.Id,
                                    SamplingPointId = samplingPointIdExisting,
                                    ScenarioLabel = (samplingPointEvent.ScenarioLabel == "In Operation"
                                                ? "in_operation"
                                                : (samplingPointEvent.ScenarioLabel == "At Rest"
                                                ? "at_rest"
                                                : null)),
                                    RoomPurposeId = insertRoomPurposeData.Id,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = room.CreatedBy
                                };

                                await _dataProvider.InsertRelRoomSampling(relRoomSampling);
                                #endregion

                                #region insert rel_sampling_test_param

                                var testScenarios = await _testScenarioDataProvider.GetByGradeRoomId(gradeRoom.Id);
                                _logger.LogInformation("test scenario");
                                _logger.LogInformation(JsonSerializer.Serialize(testScenarios));

                                if (samplingPointEvent.TestParameters != null)
                                {
                                    foreach (var testParameterEvent in samplingPointEvent.TestParameters)
                                    {
                                        foreach (var testScenario in testScenarios)
                                        {
                                            var relTestScenarioParam = await _testScenarioDataProvider.GetRelTestScenarioParam(testScenario.Id, testParameterEvent.DataId);

                                            _logger.LogInformation("rel_test_scenario_param");
                                            _logger.LogInformation(JsonSerializer.Serialize(relTestScenarioParam));

                                            RelSamplingTestParam relSamplingTestParam = new RelSamplingTestParam()
                                            {
                                                SamplingPointId = samplingPointIdExisting,
                                                TestScenarioParamId = relTestScenarioParam.Id,
                                                CreatedAt = DateTime.Now,
                                                CreatedBy = samplingPointEvent.CreatedBy
                                            };

                                            await _samplingPointDataProvider.InsertRelSamplingParam(relSamplingTestParam);
                                            _logger.LogInformation("insert rel_sampling_test_param");
                                        }
                                    }
                                }

                                #endregion

                            }
                        }
                        #endregion

                        #region insert sampling point layout
                        if (item.SamplingPointLayout != null)
                        {

                            foreach (var samplingPointLayoutEvent in item.SamplingPointLayout)
                            {
                                var samplingPointLayout = new RoomSamplingPointLayout()
                                {
                                    //RoomId = room.Id,
                                    RoomPurposeId = insertRoomPurposeData.Id,
                                    AttachmentFile = samplingPointLayoutEvent.AttachmentFile,
                                    FileName = samplingPointLayoutEvent.FileName,
                                    FileType = samplingPointLayoutEvent.FileType,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = samplingPointLayoutEvent.CreatedBy,
                                    UpdatedAt = DateTime.Now,
                                    UpdatedBy = samplingPointLayoutEvent.UpdatedBy,
                                    RowStatus = samplingPointLayoutEvent.RowStatus
                                };

                                await _roomSamplingPointLayoutDataProvider.Insert(samplingPointLayout);
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                #region insert or update sampling point
                //di komen dulu karena akan ada perubahan
                //if (@event.SamplingPoints != null)
                // {
                //     foreach (var samplingPointEvent in @event.SamplingPoints)
                //     {
                //         var samplingPoint = await _samplingPointDataProvider.GetByCodeAndRoomCode(samplingPointEvent.SamplingPointName, @event.Code);
                //         if (samplingPoint == null)
                //         {
                //             samplingPoint = new SamplingPoint()
                //             {
                //                 Code = samplingPointEvent.SamplingPointName,
                //                 RoomId = room.Id,
                //                 ToolId = null,
                //                 CreatedAt = DateTime.Now,
                //                 CreatedBy = samplingPointEvent.CreatedBy
                //             };

                //             await _samplingPointDataProvider.Insert(samplingPoint);
                //         }
                //         else
                //         {
                //             samplingPoint.Code = samplingPointEvent.SamplingPointName;
                //             samplingPoint.RoomId = room.Id;
                //             samplingPoint.ToolId = null;
                //             samplingPoint.CreatedAt = DateTime.Now;
                //             samplingPoint.CreatedBy = samplingPointEvent.CreatedBy;

                //             await _samplingPointDataProvider.Update(samplingPoint);
                //         }


                //         #region rel_room_sampling

                //         var relRoomSampling = await _dataProvider.GetRelRoomSamplingByCodeAndSamplingPointCode(@event.Code, samplingPointEvent.SamplingPointName);
                //         if (relRoomSampling == null)
                //         {
                //             relRoomSampling = new RelRoomSampling()
                //             {
                //                 RoomId = room.Id,
                //                 SamplingPointId = samplingPoint.Id,
                //                 CreatedAt = DateTime.Now,
                //                 CreatedBy = room.CreatedBy
                //             };

                //             await _dataProvider.InsertRelRoomSampling(relRoomSampling);

                //         }
                //         else
                //         {
                //             relRoomSampling.RoomId = room.Id;
                //             relRoomSampling.SamplingPointId = samplingPoint.Id;
                //             relRoomSampling.CreatedAt = DateTime.Now;
                //             relRoomSampling.CreatedBy = room.CreatedBy;

                //             await _dataProvider.UpdateRelRoomSampling(relRoomSampling);
                //         }

                //         #endregion

                //         #region insert rel_sampling_test_param

                //         var testScenarios = await _testScenarioDataProvider.GetByGradeRoomId(gradeRoom.Id);
                //         _logger.LogInformation("test scenario");
                //         _logger.LogInformation(JsonSerializer.Serialize(testScenarios));

                //         if (samplingPointEvent.TestParameters != null)
                //         {
                //             foreach (var testParameterEvent in samplingPointEvent.TestParameters)
                //             {
                //                 foreach (var testScenario in testScenarios)
                //                 {
                //                     var relTestScenarioParam = await _testScenarioDataProvider.GetRelTestScenarioParam(testScenario.Id, testParameterEvent.DataId);
                //                     var relSamplingTestParam = await _samplingPointDataProvider.GetRelSamplingTestParam(samplingPoint.Id, relTestScenarioParam.Id);

                //                     _logger.LogInformation("rel_test_scenario_param");
                //                     _logger.LogInformation(JsonSerializer.Serialize(relTestScenarioParam));
                //                     _logger.LogInformation("rel_sampling_test_param");
                //                     _logger.LogInformation(JsonSerializer.Serialize(relSamplingTestParam));

                //                     if (relSamplingTestParam == null)
                //                     {
                //                         relSamplingTestParam = new RelSamplingTestParam()
                //                         {
                //                             SamplingPointId = samplingPoint.Id,
                //                             TestScenarioParamId = relTestScenarioParam.Id,
                //                             CreatedAt = DateTime.Now,
                //                             CreatedBy = samplingPoint.CreatedBy
                //                         };

                //                         await _samplingPointDataProvider.InsertRelSamplingParam(relSamplingTestParam);
                //                         _logger.LogInformation("insert rel_sampling_test_param");
                //                     }
                //                     else
                //                     {
                //                         relSamplingTestParam.SamplingPointId = samplingPoint.Id;
                //                         relSamplingTestParam.TestScenarioParamId = relTestScenarioParam.Id;
                //                         relSamplingTestParam.CreatedAt = DateTime.Now;
                //                         relSamplingTestParam.CreatedBy = samplingPoint.CreatedBy;

                //                         await _samplingPointDataProvider.UpdateRelSamplingParam(relSamplingTestParam);
                //                         _logger.LogInformation("update rel_sampling_test_param");
                //                     }

                //                 }
                //             }
                //         }

                //         #endregion

                //     }
                // }

                #endregion

                #region insert or update sampling point layout
                //di komen dulu karena akan ada perubahan
                // if (@event.SamplingPointLayout != null)
                // {
                //     var samplingPointLayouts = await _roomSamplingPointLayoutDataProvider.GetByRoomCode(room.Code);
                //     if (samplingPointLayouts.Any())
                //     {
                //         _context.RoomSamplingPointLayout.RemoveRange(samplingPointLayouts);
                //     }

                //     foreach (var samplingPointLayoutEvent in @event.SamplingPointLayout)
                //     {
                //         var samplingPointLayout = new RoomSamplingPointLayout()
                //         {
                //             RoomId = room.Id,
                //             AttachmentFile = samplingPointLayoutEvent.AttachmentFile,
                //             FileName = samplingPointLayoutEvent.FileName,
                //             FileType = samplingPointLayoutEvent.FileType,
                //             CreatedAt = DateTime.Now,
                //             CreatedBy = samplingPointLayoutEvent.CreatedBy,
                //             UpdatedAt = DateTime.Now,
                //             UpdatedBy = samplingPointLayoutEvent.UpdatedBy,
                //             RowStatus = samplingPointLayoutEvent.RowStatus
                //         };

                //         await _roomSamplingPointLayoutDataProvider.Insert(samplingPointLayout);
                //     }
                // }
                #endregion
                _logger.LogInformation("sync room succeed");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
                _logger.LogInformation(JsonSerializer.Serialize(@event));
            }

            _logger.LogInformation("sync room from google pub/sub - end");
        }
    }
}
