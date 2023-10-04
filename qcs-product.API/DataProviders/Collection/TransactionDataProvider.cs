using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.BindingModels;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using qcs_product.API.Helpers;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionDataProvider : ITransactionDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly IGradeRoomDataProvider _gradeRoomDataProvider;
        private readonly IRoomDataProvider _roomDataProvider;
        private readonly IToolDataProvider _toolDataProvider;

        public TransactionDataProvider(QcsProductContext context, IGradeRoomDataProvider gradeRoomDataProvider, IRoomDataProvider roomDataProvider, IToolDataProvider toolDataProvider)
        {
            _context = context;
            _gradeRoomDataProvider = gradeRoomDataProvider;
            _roomDataProvider = roomDataProvider;
            _toolDataProvider = toolDataProvider;
        }

        public async Task<RoomDetailRelationViewModel> InsertRoomMasterDataForByRequestId(MasterDataUseByRequest data, List<TransactionTestParameter> getTestParam, List<SamplingPointLiteViewModel> samplingPointExisting)
        {
            #region Get Room Existing
            var roomExt = await _roomDataProvider.GetRoomDetailById(data.RoomId);
            #endregion

            #region insert room
            var roomTrx = await _insertRoom(roomExt, getTestParam, samplingPointExisting);
            #endregion

            return roomTrx;
        }

        private async Task<TransactionOrganization> _InsertOrganization(int organizationId)
        {
            var orgExt = await _context.Organizations.FirstOrDefaultAsync(x => x.Id == organizationId);
            TransactionOrganization orgTransaction = new TransactionOrganization()
            {
                OrgCode = orgExt.OrgCode,
                BiohrOrganizationId = orgExt.BIOHROrganizationId,
                Name = orgExt.Name,
                CreatedAt = DateHelper.Now(),
                CreatedBy = orgExt.CreatedBy
            };

            await _context.TransactionOrganization.AddAsync(orgTransaction);
            await _context.SaveChangesAsync();

            return orgTransaction;
        }

        public async Task<Facility> _InsertFacility(int facilityId)
        {
            var facilityExt = await _context.Facilities.FirstOrDefaultAsync(x => x.Id == facilityId);
            #region organization
            var orgTrx = await _InsertOrganization(facilityExt.OrganizationId);
            #endregion
            TransactionFacility insertTrxFacility = new TransactionFacility()
            {
                FacilityCode = facilityExt.Code,
                FacilityName = facilityExt.Name,
                OrganizationId = orgTrx.Id,
                CreatedAt = DateHelper.Now(),
                CreatedBy = facilityExt.CreatedBy
            };
            var facilityTrx = await _context.TransactionFacility.AddAsync(insertTrxFacility);
            await _context.SaveChangesAsync();

            var facility = new Facility()
            {
                Code = insertTrxFacility.FacilityCode,
                Name = insertTrxFacility.FacilityName,
                OrganizationId = insertTrxFacility.OrganizationId.Value
            };

            return facility;
        }

        private async Task<TransactionGradeRoom> _InsertGradeRoom(int gradeRoomMasterId, List<TransactionTestParameter> getTestParam)
        {
            var gradeRoomExt = await _gradeRoomDataProvider.DetailGradeRoomById(gradeRoomMasterId);
            var insertTransactionRelGradeRoomScenario = new List<TransactionRelGradeRoomScenario>();
            TransactionGradeRoom insertTrxGradeRoom = new TransactionGradeRoom()
            {
                TestGroupId = gradeRoomExt.TestGroupId,
                Code = gradeRoomExt.Code,
                Name = gradeRoomExt.Name,
                CreatedAt = DateHelper.Now(),
                CreatedBy = gradeRoomExt.CreatedBy,
                GradeRoomDefault = gradeRoomExt.GradeRoomDefault
            };

            await _context.TransactionGradeRoom.AddAsync(insertTrxGradeRoom);
            await _context.SaveChangesAsync();

            //test scenario -- rel_grade_room_scenario
            var relGradeRoomScenarioExt = await _context.RelGradeRoomScenarios.Where(x => x.GradeRoomId == gradeRoomMasterId).ToListAsync();
            var insertTransactionTestVariable = new List<TransactionTestVariable>();

            foreach (var item in gradeRoomExt.TestScenario)
            {
                //insert test scenario
                TransactionTestScenario newTestScenario = new TransactionTestScenario()
                {
                    Name = item.Name,
                    Label = item.Label,
                    CreatedAt = DateHelper.Now(),
                    CreatedBy = gradeRoomExt.CreatedBy,
                };

                var newTestScenarioTrx = await _InsertTransactionTestScenario(newTestScenario);

                TransactionRelGradeRoomScenario newRelGradeRoomTestScenario = new TransactionRelGradeRoomScenario()
                {
                    GradeRoomId = insertTrxGradeRoom.Id,
                    TestScenarioId = newTestScenario.Id,
                    CreatedAt = DateHelper.Now(),
                    CreatedBy = gradeRoomExt.CreatedBy,
                };

                insertTransactionRelGradeRoomScenario.Add(newRelGradeRoomTestScenario);

                //insert - transaction test param and rel test scenario test params


                foreach (var itemParams in item.TestParameters)
                {
                    TransactionRelTestScenarioParam newTestParams = new TransactionRelTestScenarioParam()
                    {
                        TestParameterId = itemParams.Id,
                        TestScenarioId = newTestScenario.Id,
                        CreatedAt = DateHelper.Now(),
                        CreatedBy = gradeRoomExt.CreatedBy,
                    };

                    var newTestParamsTrx = await _InsertTransactionRelTestScenarioParam(newTestParams);

                    foreach (var itemVar in itemParams.TestVariables)
                    {
                        TransactionTestVariable newTestVar = new TransactionTestVariable()
                        {
                            TestParameterId = newTestParams.Id,
                            TresholdOperator = itemVar.TresholdOperator,
                            TresholdValue = itemVar.TresholdValue,
                            ThresholdValueTo = itemVar.TresholdMax,
                            ThresholdValueFrom = itemVar.TresholdMin,
                            Sequence = itemVar.Sequence,
                            VariableName = itemVar.VariableName,
                            CreatedAt = DateHelper.Now(),
                            CreatedBy = gradeRoomExt.CreatedBy
                        };
                        insertTransactionTestVariable.Add(newTestVar);
                    }
                }
            }

            var newTransactionTestVariableTrx = await _InsertTransactionTestVariable(insertTransactionTestVariable);
            var TransactionRelGradeRoomScenario = await _InsertTransactionRelGradeRoomScenario(insertTransactionRelGradeRoomScenario);

            return insertTrxGradeRoom;
        }

        private async Task<RoomDetailRelationViewModel> _insertRoom(RoomDetailViewModel roomExt, List<TransactionTestParameter> getTestParam, List<SamplingPointLiteViewModel> samplingPointExisting)
        {
            #region organization
            var orgTrx = await _InsertOrganization(roomExt.OrganizationId);
            #endregion

            #region insert tool by ahu id in room 
            var getToolIdForAhu = await _InsertToolTrxByToolId(roomExt.Ahu.Value, getTestParam, samplingPointExisting);
            samplingPointExisting.AddRange(getToolIdForAhu.SamplingPointInfo);
            #endregion

            #region GradeRoom
            var gradeTrx = await _InsertGradeRoom(roomExt.GradeRoomId.Value, getTestParam);
            #endregion

            #region 
            var buildingExt = 0;
            if (roomExt.BuildingId != 0)
            {
                var buildingTrx = await _InsertBuilding(roomExt.BuildingId);
                buildingExt = buildingTrx.Id;
            }
            #endregion

            TransactionRoom insertRoomTrx = new TransactionRoom()
            {
                Code = roomExt.Code,
                Name = roomExt.Name,
                CreatedAt = DateHelper.Now(),
                CreatedBy = roomExt.CreatedBy,
                PosId = roomExt.PosId,
                GradeRoomId = gradeTrx.Id,
                BuildingId = buildingExt,
                OrganizationId = orgTrx.Id,
                Ahu = getToolIdForAhu.Id
            };

            await _context.TransactionRoom.AddAsync(insertRoomTrx);
            await _context.SaveChangesAsync();

            var testScenario = await (from ts in _context.TransactionTestScenario
                                      join rgts in _context.TransactionRelGradeRoomScenario on ts.Id equals rgts.TestScenarioId
                                      where rgts.GradeRoomId == insertRoomTrx.GradeRoomId
                                      select ts).ToListAsync();

            var testScenarioIds = testScenario.Select(x => x.Id).ToList();
            var lsRelTestScenarioParamTrx = await (from rtsp in _context.TransactionRelTestScenarioParam
                                                   where testScenarioIds.Contains(rtsp.TestScenarioId)
                                                   select rtsp).ToListAsync();

            var insertTransactionRoomPurposeToMasterPurpose = new List<TransactionRoomPurposeToMasterPurpose>();
            var insertTransactionRelRoomSamplingPoint = new List<TransactionRelRoomSamplingPoint>();
            var insertTransactionRelSamplingTestParam = new List<TransactionRelSamplingTestParam>();
            var insertTransactionRoomSamplingPointLayout = new List<TransactionRoomSamplingPointLayout>();

            //insert room purpose
            foreach (var itemRoomPurpose in roomExt.ListDataPurposes)
            {
                TransactionRoomPurpose insertRoomPurposeTrx = new TransactionRoomPurpose()
                {
                    RoomId = insertRoomTrx.Id
                };

                var newTransactionRoomPurposeTrx = await _InsertTransactionRoomPurpose(insertRoomPurposeTrx);

                var purpId = 0;
                foreach (var itemPurp in itemRoomPurpose.Purpose)
                {
                    purpId = itemPurp.Id;
                }

                TransactionRoomPurposeToMasterPurpose insertRoomPurposeToMasterPurpose = new TransactionRoomPurposeToMasterPurpose()
                {
                    PurposeId = purpId,
                    RoomPurposeId = insertRoomPurposeTrx.Id,
                };

                insertTransactionRoomPurposeToMasterPurpose.Add(insertRoomPurposeToMasterPurpose);

                //insert room sampling point

                foreach (var itemSamplingPoint in itemRoomPurpose.SamplingPoints)
                {
                    #region sampling point
                    var samplingPointLite = samplingPointExisting.Find(x => x.SamplingPointCode == itemSamplingPoint.Code);
                    var samplingPointIdExisting = 0;
                    if (samplingPointLite == null)
                    {
                        TransactionSamplingPoint insertSamplintPoint = new TransactionSamplingPoint()
                        {
                            Code = itemSamplingPoint.Code,
                            CreatedAt = DateHelper.Now(),
                            CreatedBy = insertRoomTrx.CreatedBy
                        };

                        var newTransactionSamplingPointTrx = await _InsertTransactionSamplingPoint(insertSamplintPoint);
                        samplingPointIdExisting = newTransactionSamplingPointTrx.Id;

                        var samplingCode = new SamplingPointLiteViewModel()
                        {
                            SamplingPointCode = newTransactionSamplingPointTrx.Code,
                            SamplingPointId = newTransactionSamplingPointTrx.Id
                        };
                        samplingPointExisting.Add(samplingCode);
                    }
                    else
                    {
                        samplingPointIdExisting = samplingPointLite.SamplingPointId;
                    }

                    #region rel_room_sampling
                    TransactionRelRoomSamplingPoint insertRelRoomSampling = new TransactionRelRoomSamplingPoint()
                    {
                        RoomId = insertRoomTrx.Id,
                        SamplingPoinId = samplingPointIdExisting,
                        ScenarioLabel = itemSamplingPoint.ScenarioLabel,
                        RoomPurposeId = insertRoomPurposeTrx.Id,
                        CreatedAt = DateHelper.Now(),
                        CreatedBy = insertRoomTrx.CreatedBy
                    };

                    insertTransactionRelRoomSamplingPoint.Add(insertRelRoomSampling);
                    #endregion
                    #endregion

                    foreach (var itemTestParams in itemSamplingPoint.TestParameter)
                    {
                        foreach (var itemTestScenario in testScenario)
                        {
                            var relTestScenarioParamTrx = lsRelTestScenarioParamTrx
                                .FirstOrDefault(x => x.TestScenarioId == itemTestScenario.Id
                                    && x.TestParameterId == itemTestParams.Id);

                            var insertRelSamplingTestParamsTrx = new TransactionRelSamplingTestParam()
                            {
                                SamplingPointId = samplingPointIdExisting,
                                TestScenarioParamId = relTestScenarioParamTrx.Id,
                                CreatedAt = DateHelper.Now(),
                                CreatedBy = insertRoomTrx.CreatedBy
                            };
                            insertTransactionRelSamplingTestParam.Add(insertRelSamplingTestParamsTrx);
                        }
                    }

                }

                //insert room sampling point layout
                foreach (var itemSamplingPointLayout in itemRoomPurpose.SamplingPointLayout)
                {
                    var samplingPointLayout = new TransactionRoomSamplingPointLayout()
                    {
                        AttachmentFile = itemSamplingPointLayout.AttachmentFile,
                        FileName = itemSamplingPointLayout.FileName,
                        FileType = itemSamplingPointLayout.FileType,
                        RoomPurposeId = insertRoomPurposeTrx.Id,
                        CreatedAt = DateHelper.Now(),
                        CreatedBy = itemSamplingPointLayout.CreatedBy
                    };
                    insertTransactionRoomSamplingPointLayout.Add(samplingPointLayout);
                }

            }

            var newTransactionRoomPurposeToMasterPurposeTrx = await _InsertTransactionRoomPurposeToMasterPurpose(insertTransactionRoomPurposeToMasterPurpose);
            var newTransactionRelRoomSamplingPointTrx = await _InsertTransactionRelRoomSamplingPoint(insertTransactionRelRoomSamplingPoint);
            var newTransactionRelSamplingTestParamTrx = await _InsertTransactionRelSamplingTestParam(insertTransactionRelSamplingTestParam);
            var newTransactionRoomSamplingPointLayoutTrx = await _InsertTransactionRoomSamplingPointLayout(insertTransactionRoomSamplingPointLayout);

            //insert tool by room id
            var toolTrx = await _InsertToolTrx(roomExt.Id, insertRoomTrx.Id, getTestParam, samplingPointExisting);
            var sampling = toolTrx;
            samplingPointExisting.AddRange(sampling);

            var newRoomTrx = new RoomDetailRelationViewModel()
            {
                RoomId = insertRoomTrx.Id,
                RoomCode = insertRoomTrx.Code,
                RoomName = insertRoomTrx.Name,
                GradeRoomId = gradeTrx.Id,
                GradeRoomCode = gradeTrx.Code,
                GradeRoomName = gradeTrx.Name,
                AhuId = getToolIdForAhu.Id,
                AhuCode = getToolIdForAhu.ToolCode,
                AhuName = getToolIdForAhu.Name,
                SamplingPointInfo = samplingPointExisting
            };

            return newRoomTrx;
        }

        private async Task<List<SamplingPointLiteViewModel>> _InsertToolTrx(int roomMasterId, int roomTrx, List<TransactionTestParameter> getTestParam, List<SamplingPointLiteViewModel> samplingPointExisting)
        {
            List<TransactionTool> newDataTrx = new List<TransactionTool>();

            var toolExt = await _toolDataProvider.GetToolDetailByRoomId(roomMasterId);

            foreach (var item in toolExt)
            {
                var getGRTrx = await _InsertGradeRoom(item.GradeRoomId.Value, getTestParam);
                var gradeRoomId = getGRTrx.Id;

                var toolGroupExt = await _context.ToolGroups.FirstOrDefaultAsync(x => x.Id == item.ToolGroupId);

                TransactionToolGroup newToolGroupTrx = new TransactionToolGroup()
                {
                    Code = toolGroupExt.Code,
                    Name = toolGroupExt.Name,
                    Label = toolGroupExt.Label,
                    CreatedAt = DateHelper.Now(),
                    CreatedBy = toolGroupExt.CreatedBy
                };

                var newTransactionToolGroup = await _InsertTransactionToolGroup(newToolGroupTrx);

                TransactionTool insertToolTrx = new TransactionTool()
                {
                    ToolCode = item.Code,
                    Name = item.Name,
                    ToolGroupId = newToolGroupTrx.Id,
                    RoomId = roomTrx,
                    GradeRoomId = gradeRoomId,
                    SerialNumberId = item.SerialNumberId,
                    CreatedAt = DateHelper.Now(),
                    CreatedBy = newToolGroupTrx.CreatedBy,
                    MachineId = item.MachineId
                };

                await _context.TransactionTool.AddAsync(insertToolTrx);
                await _context.SaveChangesAsync();

                var testScenario = await (from ts in _context.TransactionTestScenario
                                          join rgts in _context.TransactionRelGradeRoomScenario on ts.Id equals rgts.TestScenarioId
                                          where rgts.GradeRoomId == insertToolTrx.GradeRoomId
                                          select ts).ToListAsync();

                var testScenarioIds = testScenario.Select(x => x.Id).ToList();
                var lsRelTestScenarioParamTrx = await (from rtsp in _context.TransactionRelTestScenarioParam
                                                       where testScenarioIds.Contains(rtsp.TestScenarioId)
                                                       select rtsp).ToListAsync();

                var insertTransactionToolActivity = new List<TransactionToolActivity>();
                var insertTransactionToolPurposeToMasterPurpose = new List<TransactionToolPurposeToMasterPurpose>();
                var insertTransactionRelSamplingTool = new List<TransactionRelSamplingTool>();
                var insertTransactionRelSamplingTestParam = new List<TransactionRelSamplingTestParam>();
                var insertTransactionToolSamplingPointLayout = new List<TransactionToolSamplingPointLayout>();


                foreach (var itemToolActivity in item.Activities)
                {
                    var newActivities = new TransactionActivity()
                    {
                        Code = itemToolActivity.ActivityCode,
                        Name = itemToolActivity.ActivityName,
                        CreatedAt = DateHelper.Now(),
                        CreatedBy = itemToolActivity.CreatedBy
                    };

                    var newTransactionActivityTrx = await _InsertTransactionActivity(newActivities);

                    var newToolActivity = new TransactionToolActivity()
                    {
                        ToolId = insertToolTrx.Id,
                        ActivityId = newActivities.Id,
                        ActivityCode = itemToolActivity.ActivityCode,
                        ActivityDate = itemToolActivity.ActivityDate,
                        ExpiredDate = itemToolActivity.ExpiredDate,
                        CreatedAt = DateHelper.Now(),
                        CreatedBy = itemToolActivity.CreatedBy
                    };

                    insertTransactionToolActivity.Add(newToolActivity);
                }

                //insert tool purpose
                //insert room purpose
                foreach (var itemToolPurpose in item.ListDataPurposes)
                {
                    var insertToolPurposeTrx = new TransactionToolPurpose()
                    {
                        ToolId = insertToolTrx.Id,
                        CreatedAt = DateHelper.Now(),
                        CreatedBy = insertToolTrx.CreatedBy
                    };

                    var newTransactionToolPurpose = await _InsertTransactionToolPurpose(insertToolPurposeTrx);

                    var purpId = 0;
                    foreach (var itemPurp in itemToolPurpose.Purpose)
                    {
                        purpId = itemPurp.Id;
                    }

                    var insertRoomPurposeToMasterPurpose = new TransactionToolPurposeToMasterPurpose()
                    {
                        PurposeId = purpId,
                        ToolPurposeId = newTransactionToolPurpose.Id,
                        CreatedAt = DateHelper.Now(),
                        CreatedBy = newTransactionToolPurpose.CreatedBy
                    };

                    insertTransactionToolPurposeToMasterPurpose.Add(insertRoomPurposeToMasterPurpose);



                    //insert room sampling point

                    foreach (var itemSamplingPoint in itemToolPurpose.SamplingPoints)
                    {
                        #region sampling point
                        var samplingPointLite = samplingPointExisting.Find(x => x.SamplingPointCode == itemSamplingPoint.Code);
                        var samplingPointIdExisting = 0;
                        if (samplingPointLite == null)
                        {
                            TransactionSamplingPoint insertSamplintPoint = new TransactionSamplingPoint()
                            {
                                Code = itemSamplingPoint.Code,
                                ToolId = insertToolTrx.Id,
                                CreatedAt = DateHelper.Now(),
                                CreatedBy = insertToolTrx.CreatedBy
                            };

                            var newTransactionSamplingPointTrx = await _InsertTransactionSamplingPoint(insertSamplintPoint);
                            samplingPointIdExisting = newTransactionSamplingPointTrx.Id;

                            var samplingCode = new SamplingPointLiteViewModel()
                            {
                                SamplingPointCode = newTransactionSamplingPointTrx.Code,
                                SamplingPointId = newTransactionSamplingPointTrx.Id
                            };
                            samplingPointExisting.Add(samplingCode);
                        }
                        else
                        {
                            samplingPointIdExisting = samplingPointLite.SamplingPointId;
                        }


                        #region rel_room_sampling
                        TransactionRelSamplingTool insertRelSampling = new TransactionRelSamplingTool()
                        {
                            //RoomId = insertToolTrx.Id,
                            SamplingPoinId = samplingPointIdExisting,
                            ScenarioLabel = itemSamplingPoint.ScenarioLabel,
                            ToolPurposeId = newTransactionToolPurpose.Id,
                            CreatedAt = DateHelper.Now(),
                            CreatedBy = insertToolTrx.CreatedBy
                        };

                        insertTransactionRelSamplingTool.Add(insertRelSampling);
                        #endregion

                        #endregion

                        foreach (var itemTestParams in itemSamplingPoint.TestParameter)
                        {
                            foreach (var itemTestScenario in testScenario)
                            {
                                var relTestScenarioParamTrx = lsRelTestScenarioParamTrx
                                    .FirstOrDefault(x => x.TestScenarioId == itemTestScenario.Id
                                        && x.TestParameterId == itemTestParams.Id);

                                var insertRelSamplingTestParamsTrx = new TransactionRelSamplingTestParam()
                                {
                                    SamplingPointId = samplingPointIdExisting,
                                    TestScenarioParamId = relTestScenarioParamTrx.Id,
                                    CreatedAt = DateHelper.Now(),
                                    CreatedBy = insertToolTrx.CreatedBy
                                };

                                insertTransactionRelSamplingTestParam.Add(insertRelSamplingTestParamsTrx);
                            }
                        }

                    }

                    //insert room sampling point layout
                    foreach (var itemSamplingPointLayout in itemToolPurpose.SamplingPointLayout)
                    {
                        var samplingPointLayout = new TransactionToolSamplingPointLayout()
                        {
                            AttachmentFile = itemSamplingPointLayout.AttachmentFile,
                            FileName = itemSamplingPointLayout.FileName,
                            FileType = itemSamplingPointLayout.FileType,
                            ToolPurposeId = insertToolPurposeTrx.Id,
                            CreatedAt = DateHelper.Now(),
                            CreatedBy = itemSamplingPointLayout.CreatedBy
                        };

                        insertTransactionToolSamplingPointLayout.Add(samplingPointLayout);

                    }
                    //insert tool sampling point 
                    //insert tool sampling point layout

                }
                newDataTrx.Add(insertToolTrx);
                var newTransactionToolActivityTrx = await _InsertTransactionToolActivity(insertTransactionToolActivity);
                var newTransactionToolPurposeToMasterPurpose = await _InsertTransactionToolPurposeToMasterPurpose(insertTransactionToolPurposeToMasterPurpose);
                var newTransactionRelSamplingToolTrx = await _InsertTransactionRelSamplingTool(insertTransactionRelSamplingTool);
                var newTransactionRelSamplingTestParam = await _InsertTransactionRelSamplingTestParam(insertTransactionRelSamplingTestParam);
                var newTransactionToolSamplingPointLayout = await _InsertTransactionToolSamplingPointLayout(insertTransactionToolSamplingPointLayout);

            }
            return samplingPointExisting;

        }

        public async Task<TransactionToolViewModel> _InsertToolTrxByToolId(int toolId, List<TransactionTestParameter> getTestParam, List<SamplingPointLiteViewModel> samplingPointExisting)
        {
            var toolExt = await _toolDataProvider.GetToolDetailById(toolId);
            var gradeRoomId = 0;

            if (toolExt.GradeRoomId != 0)
            {
                var getGRTrx = await _InsertGradeRoom(toolExt.GradeRoomId.Value, getTestParam);
                gradeRoomId = getGRTrx.Id;
            }

            var toolGroupExt = await _context.ToolGroups.FirstOrDefaultAsync(x => x.Id == toolExt.ToolGroupId);

            TransactionToolGroup insertToolGroupTrx = new TransactionToolGroup()
            {
                Code = toolGroupExt.Code,
                Name = toolGroupExt.Name,
                Label = toolGroupExt.Label,
                CreatedAt = DateHelper.Now(),
                CreatedBy = toolGroupExt.CreatedBy
            };

            var newToolGroupTrx = await _InsertTransactionToolGroup(insertToolGroupTrx);

            TransactionTool insertToolTrx = new TransactionTool()
            {
                ToolCode = toolExt.Code,
                Name = toolExt.Name,
                ToolGroupId = newToolGroupTrx.Id,
                //RoomId = roomTrx,
                GradeRoomId = gradeRoomId,
                SerialNumberId = toolExt.SerialNumberId,
                MachineId = toolExt.MachineId,
                CreatedBy = toolGroupExt.CreatedBy,
                CreatedAt = DateHelper.Now()
            };

            TransactionToolViewModel insertToolDetailTrx = new TransactionToolViewModel()
            {
                ToolCode = toolExt.Code,
                Name = toolExt.Name,
                ToolGroupId = newToolGroupTrx.Id,
                //RoomId = roomTrx,
                GradeRoomId = gradeRoomId,
                SerialNumberId = toolExt.SerialNumberId,
                MachineId = toolExt.MachineId,
                CreatedBy = toolGroupExt.CreatedBy,
                CreatedAt = DateHelper.Now()
            };

            await _context.TransactionTool.AddAsync(insertToolTrx);
            await _context.SaveChangesAsync();

            var testScenario = await (from ts in _context.TransactionTestScenario
                                      join rgts in _context.TransactionRelGradeRoomScenario on ts.Id equals rgts.TestScenarioId
                                      where rgts.GradeRoomId == insertToolTrx.GradeRoomId
                                      select ts).ToListAsync();

            var testScenarioIds = testScenario.Select(x => x.Id).ToList();
            var lsRelTestScenarioParamTrx = await (from rtsp in _context.TransactionRelTestScenarioParam
                                                   where testScenarioIds.Contains(rtsp.TestScenarioId)
                                                   select rtsp).ToListAsync();

            var insertTransactionToolActivity = new List<TransactionToolActivity>();
            var insertTransactionToolPurposeToMasterPurpose = new List<TransactionToolPurposeToMasterPurpose>();
            var insertTransactionRelSamplingTool = new List<TransactionRelSamplingTool>();
            var insertTransactionRelSamplingTestParam = new List<TransactionRelSamplingTestParam>();
            var insertTransactionToolSamplingPointLayout = new List<TransactionToolSamplingPointLayout>();

            foreach (var itemToolActivity in toolExt.Activities)
            {
                var newActivities = new TransactionActivity()
                {
                    Code = itemToolActivity.ActivityCode,
                    Name = itemToolActivity.ActivityName,
                    CreatedAt = DateHelper.Now(),
                    CreatedBy = itemToolActivity.CreatedBy
                };

                var newTransactionActivity = await _InsertTransactionActivity(newActivities);

                var newToolActivity = new TransactionToolActivity()
                {
                    ToolId = insertToolTrx.Id,
                    ActivityId = newActivities.Id,
                    ActivityCode = itemToolActivity.ActivityCode,
                    ActivityDate = itemToolActivity.ActivityDate,
                    ExpiredDate = itemToolActivity.ExpiredDate,
                    CreatedAt = DateHelper.Now(),
                    CreatedBy = itemToolActivity.CreatedBy
                };

                insertTransactionToolActivity.Add(newToolActivity);

            }

            //insert tool purpose
            //insert room purpose
            foreach (var itemToolPurpose in toolExt.ListDataPurposes)
            {
                var insertToolPurposeTrx = new TransactionToolPurpose()
                {
                    ToolId = insertToolTrx.Id,
                    CreatedAt = DateHelper.Now(),
                    CreatedBy = insertToolTrx.CreatedBy
                };

                var newTransactionToolPurpose = await _InsertTransactionToolPurpose(insertToolPurposeTrx);

                var purpId = 0;
                foreach (var itemPurp in itemToolPurpose.Purpose)
                {
                    purpId = itemPurp.Id;
                }

                var insertRoomPurposeToMasterPurpose = new TransactionToolPurposeToMasterPurpose()
                {
                    PurposeId = purpId,
                    ToolPurposeId = newTransactionToolPurpose.Id,
                    CreatedAt = DateHelper.Now(),
                    CreatedBy = newTransactionToolPurpose.CreatedBy
                };

                insertTransactionToolPurposeToMasterPurpose.Add(insertRoomPurposeToMasterPurpose);

                //insert room sampling point
                foreach (var itemSamplingPoint in itemToolPurpose.SamplingPoints)
                {


                    #region sampling point
                    var samplingPointLite = samplingPointExisting.Find(x => x.SamplingPointCode == itemSamplingPoint.Code);
                    var samplingPointIdExisting = 0;
                    if (samplingPointLite == null)
                    {
                        TransactionSamplingPoint insertSamplintPoint = new TransactionSamplingPoint()
                        {
                            Code = itemSamplingPoint.Code,
                            ToolId = insertToolTrx.Id,
                            CreatedAt = DateHelper.Now(),
                            CreatedBy = insertToolTrx.CreatedBy
                        };

                        var newTransactionSamplingPointTrx = await _InsertTransactionSamplingPoint(insertSamplintPoint);
                        samplingPointIdExisting = newTransactionSamplingPointTrx.Id;

                        var samplingCode = new SamplingPointLiteViewModel()
                        {
                            SamplingPointCode = newTransactionSamplingPointTrx.Code,
                            SamplingPointId = newTransactionSamplingPointTrx.Id
                        };
                        samplingPointExisting.Add(samplingCode);
                    }
                    else
                    {
                        samplingPointIdExisting = samplingPointLite.SamplingPointId;
                    }

                    #region rel_room_sampling
                    TransactionRelSamplingTool insertRelSampling = new TransactionRelSamplingTool()
                    {
                        //RoomId = insertToolTrx.Id,
                        SamplingPoinId = samplingPointIdExisting,
                        ScenarioLabel = itemSamplingPoint.ScenarioLabel,
                        ToolPurposeId = newTransactionToolPurpose.Id,
                        CreatedAt = DateHelper.Now(),
                        CreatedBy = insertToolTrx.CreatedBy
                    };

                    insertTransactionRelSamplingTool.Add(insertRelSampling);

                    #endregion

                    #endregion

                    foreach (var itemTestParams in itemSamplingPoint.TestParameter)
                    {
                        foreach (var itemTestScenario in testScenario)
                        {
                            var relTestScenarioParamTrx = lsRelTestScenarioParamTrx
                                .FirstOrDefault(x => x.TestScenarioId == itemTestScenario.Id
                                    && x.TestParameterId == itemTestParams.Id);

                            var insertRelSamplingTestParamsTrx = new TransactionRelSamplingTestParam()
                            {
                                SamplingPointId = samplingPointIdExisting,
                                TestScenarioParamId = relTestScenarioParamTrx.Id,
                                CreatedAt = DateHelper.Now(),
                                CreatedBy = insertToolTrx.CreatedBy
                            };

                            insertTransactionRelSamplingTestParam.Add(insertRelSamplingTestParamsTrx);
                        }
                    }

                }

                //insert room sampling point layout
                foreach (var itemSamplingPointLayout in itemToolPurpose.SamplingPointLayout)
                {
                    var samplingPointLayout = new TransactionToolSamplingPointLayout()
                    {
                        AttachmentFile = itemSamplingPointLayout.AttachmentFile,
                        FileName = itemSamplingPointLayout.FileName,
                        FileType = itemSamplingPointLayout.FileType,
                        ToolPurposeId = insertToolPurposeTrx.Id,
                        CreatedAt = DateHelper.Now(),
                        CreatedBy = itemSamplingPointLayout.CreatedBy
                    };

                    insertTransactionToolSamplingPointLayout.Add(samplingPointLayout);
                }

            }
            var newTransactionToolActivityTrx = await _InsertTransactionToolActivity(insertTransactionToolActivity);
            var newTransactionToolPurposeToMasterPurpose = await _InsertTransactionToolPurposeToMasterPurpose(insertTransactionToolPurposeToMasterPurpose);
            var newTransactionRelSamplingToolTrx = await _InsertTransactionRelSamplingTool(insertTransactionRelSamplingTool);
            var newTransactionRelSamplingTestParam = await _InsertTransactionRelSamplingTestParam(insertTransactionRelSamplingTestParam);
            var newTransactionToolSamplingPointLayout = await _InsertTransactionToolSamplingPointLayout(insertTransactionToolSamplingPointLayout);
            insertToolDetailTrx.SamplingPointInfo = samplingPointExisting;
            return insertToolDetailTrx;

        }

        private async Task<TransactionBuilding> _InsertBuilding(int buildingId)
        {
            var buildingExt = await _context.Buildings.FirstOrDefaultAsync(x => x.Id == buildingId);

            var insertBuldingTrx = new TransactionBuilding()
            {
                BuildingId = buildingExt.Code,
                BuildingName = buildingExt.Name,
                CreatedAt = DateHelper.Now(),
                CreatedBy = buildingExt.CreatedBy
            };

            await _context.TransactionBuilding.AddAsync(insertBuldingTrx);
            await _context.SaveChangesAsync();

            return insertBuldingTrx;
        }

        public async Task<TransactionPurposes> InsertPurpose(int purposeId)
        {
            //get data purpose exist
            var purpExt = await _context.Purposes.FirstOrDefaultAsync(x => x.Id == purposeId);

            var insertNewPurpose = new TransactionPurposes()
            {
                Code = purpExt.Code,
                Name = purpExt.Name,
                CreatedAt = DateHelper.Now(),
                CreatedBy = purpExt.CreatedBy
            };

            await _context.TransactionPurposes.AddAsync(insertNewPurpose);
            await _context.SaveChangesAsync();

            return insertNewPurpose;
        }

        public async Task<ToolsAHUViewModel> getAhuById(int id)
        {
            var result = await (from t in _context.TransactionTool
                                join tg in _context.TransactionToolGroup on t.ToolGroupId equals tg.Id
                                where tg.Code == ApplicationConstant.TOOL_GROUP_CODE_AHU
                                && t.Id == id
                                && t.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                select new ToolsAHUViewModel
                                {
                                    ToolId = t.Id,
                                    ToolCode = t.ToolCode,
                                    ToolName = t.Name
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<Organization> GetDetailById(int id)
        {
            var result = await (from p in _context.TransactionOrganization
                                where p.Id == id
                                select p).FirstOrDefaultAsync();

            var organization = new Organization()
            {
                Id = result.Id,
                BIOHROrganizationId = result.BiohrOrganizationId.Value,
                OrgCode = result.OrgCode,
                Name = result.Name
            };

            return organization;
        }

        private async Task<TransactionTestScenario> _InsertTransactionTestScenario(TransactionTestScenario data)
        {


            await _context.TransactionTestScenario.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        private async Task<List<TransactionRelGradeRoomScenario>> _InsertTransactionRelGradeRoomScenario(List<TransactionRelGradeRoomScenario> data)
        {

            await _context.TransactionRelGradeRoomScenario.AddRangeAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }

        private async Task<TransactionRelTestScenarioParam> _InsertTransactionRelTestScenarioParam(TransactionRelTestScenarioParam data)
        {

            await _context.TransactionRelTestScenarioParam.AddAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }

        private async Task<List<TransactionTestVariable>> _InsertTransactionTestVariable(List<TransactionTestVariable> data)
        {
            await _context.TransactionTestVariable.AddRangeAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        private async Task<TransactionRoomPurpose> _InsertTransactionRoomPurpose(TransactionRoomPurpose data)
        {

            await _context.TransactionRoomPurpose.AddAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }

        private async Task<List<TransactionRoomPurposeToMasterPurpose>> _InsertTransactionRoomPurposeToMasterPurpose(List<TransactionRoomPurposeToMasterPurpose> data)
        {
            await _context.TransactionRoomPurposeToMasterPurpose.AddRangeAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }

        private async Task<TransactionSamplingPoint> _InsertTransactionSamplingPoint(TransactionSamplingPoint data)
        {

            await _context.TransactionSamplingPoint.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;

        }

        private async Task<List<TransactionRelRoomSamplingPoint>> _InsertTransactionRelRoomSamplingPoint(List<TransactionRelRoomSamplingPoint> data)
        {

            await _context.TransactionRelRoomSamplingPoint.AddRangeAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }

        private async Task<List<TransactionRelSamplingTestParam>> _InsertTransactionRelSamplingTestParam(List<TransactionRelSamplingTestParam> data)
        {

            await _context.TransactionRelSamplingTestParam.AddRangeAsync(data);
            await _context.SaveChangesAsync();

            return data;

        }

        private async Task<List<TransactionRoomSamplingPointLayout>> _InsertTransactionRoomSamplingPointLayout(List<TransactionRoomSamplingPointLayout> data)
        {

            await _context.TransactionRoomSamplingPointLayout.AddRangeAsync(data);
            await _context.SaveChangesAsync();

            return data;

        }


        private async Task<TransactionToolGroup> _InsertTransactionToolGroup(TransactionToolGroup data)
        {
            await _context.TransactionToolGroup.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;

        }


        private async Task<TransactionActivity> _InsertTransactionActivity(TransactionActivity data)
        {

            await _context.TransactionActivity.AddAsync(data);
            await _context.SaveChangesAsync();

            return data;

        }

        private async Task<List<TransactionToolActivity>> _InsertTransactionToolActivity(List<TransactionToolActivity> data)
        {

            await _context.TransactionToolActivity.AddRangeAsync(data);
            await _context.SaveChangesAsync();

            return data;

        }

        private async Task<TransactionToolPurpose> _InsertTransactionToolPurpose(TransactionToolPurpose data)
        {

            await _context.TransactionToolPurpose.AddAsync(data);
            await _context.SaveChangesAsync();

            return data;

        }

        private async Task<List<TransactionToolPurposeToMasterPurpose>> _InsertTransactionToolPurposeToMasterPurpose(List<TransactionToolPurposeToMasterPurpose> data)
        {
            await _context.TransactionToolPurposeToMasterPurpose.AddRangeAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        private async Task<List<TransactionRelSamplingTool>> _InsertTransactionRelSamplingTool(List<TransactionRelSamplingTool> data)
        {
            await _context.TransactionRelSamplingTool.AddRangeAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        private async Task<List<TransactionToolSamplingPointLayout>> _InsertTransactionToolSamplingPointLayout(List<TransactionToolSamplingPointLayout> data)
        {
            await _context.TransactionToolSamplingPointLayout.AddRangeAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<List<TransactionTestParameter>> GetListTransactionTestParams()
        {
            return await _context.TransactionTestParameter.ToListAsync();
        }


    }
}