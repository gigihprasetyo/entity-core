using System;
using System.Collections.Generic;
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
    public class GradeRoomEventHandler : IIntegrationEventHandler<GradeRoomIntegrationEvent>
    {
        private readonly ILogger<GradeRoomEventHandler> _logger;
        private readonly QcsProductContext _context;
        private readonly IGradeRoomDataProvider _dataProvider;
        private readonly ITestScenarioDataProvider _testScenarioDataProvider;
        private readonly ITestParameterDataProvider _testParameterDataProvider;
        private readonly ITestVariableDataProvider _testVariableDataProvider;

        public GradeRoomEventHandler(ILogger<GradeRoomEventHandler> logger, QcsProductContext context, IGradeRoomDataProvider dataProvider, ITestScenarioDataProvider testScenarioDataProvider, ITestParameterDataProvider testParameterDataProvider, ITestVariableDataProvider testVariableDataProvider)
        {
            _logger = logger;
            _context = context;
            _dataProvider = dataProvider;
            _testScenarioDataProvider = testScenarioDataProvider;
            _testParameterDataProvider = testParameterDataProvider;
            _testVariableDataProvider = testVariableDataProvider;
        }

        public async Task Handle(GradeRoomIntegrationEvent @event)
        {
            _logger.LogInformation("sync grade room from google pub/sub");
            try
            {
                var gradeRoom = await _dataProvider.GetByCode(@event.Code);

                var isNew = false;
                if (gradeRoom == null)
                {
                    isNew = true;
                    gradeRoom = new GradeRoom()
                    {
                        CreatedAt = DateTime.Now,
                        Code = @event.Code
                    };
                }

                gradeRoom.Name = @event.Name;
                gradeRoom.GradeRoomDefault = @event.GradeRoomDefault;
                gradeRoom.UpdatedAt = DateTime.Now;
                gradeRoom.CreatedBy = @event.CreatedBy;
                gradeRoom.UpdatedBy = @event.UpdatedBy;
                gradeRoom.RowStatus = @event.RowStatus;
                gradeRoom.ObjectStatus = @event.ObjectStatus;
                gradeRoom.TestGroupId = 0;//TODO test group id di-get dari mana ?

                if (isNew)
                {
                    await _dataProvider.Insert(gradeRoom);
                }
                else
                {
                    await _dataProvider.Update(gradeRoom);
                }

                #region test scenario
                isNew = false;
                if (@event.TestScenarios != null)
                {
                    foreach (var testScenarioEvent in @event.TestScenarios)
                    {
                        var label = (testScenarioEvent.TestScenarioLabel == "In Operation"
                                        ? "in_operation"
                                        : (testScenarioEvent.TestScenarioLabel == "At Rest"
                                        ? "at_rest"
                                        : null));
                        var testScenario = await _testScenarioDataProvider.GetByGradeRoomCodeLabel(@event.Code, label);
                        if (testScenario == null)
                        {
                            isNew = true;
                            testScenario = new TestScenario()
                            {
                                CreatedAt = DateTime.Now,
                            };
                        }

                        testScenario.Label = (testScenarioEvent.TestScenarioLabel == "In Operation"
                                                ? "in_operation"
                                                : (testScenarioEvent.TestScenarioLabel == "At Rest"
                                                ? "at_rest"
                                                : null));
                        testScenario.Name = testScenarioEvent.TestScenarioName;
                        testScenario.CreatedBy = testScenarioEvent.CreatedBy;
                        testScenario.UpdatedAt = DateTime.Now;
                        testScenario.UpdatedBy = testScenarioEvent.UpdatedBy;
                        testScenario.RowStatus = testScenarioEvent.RowStatus;

                        if (isNew)
                        {
                            await _testScenarioDataProvider.Insert(testScenario);
                        }
                        else
                        {
                            await _testScenarioDataProvider.Update(testScenario);
                        }

                        #region insert update rel_grade_room_scenario

                        var relGradeRoomScenario = await _dataProvider.GetRelGradeRoomScenario(@event.Code, testScenario.Label);
                        if (relGradeRoomScenario == null)
                        {
                            var relGradeRoomScenarioNew = new RelGradeRoomScenario()
                            {
                                GradeRoomId = gradeRoom.Id,
                                TestScenarioId = testScenario.Id,
                                CreatedAt = DateTime.Now,
                                CreatedBy = gradeRoom.CreatedBy
                            };
                            await _dataProvider.InsertRelGradeRoomScenario(relGradeRoomScenarioNew);
                        }
                        else
                        {
                            relGradeRoomScenario.GradeRoomId = gradeRoom.Id;
                            relGradeRoomScenario.TestScenarioId = testScenario.Id;
                            relGradeRoomScenario.CreatedAt = DateTime.Now;
                            relGradeRoomScenario.CreatedBy = gradeRoom.CreatedBy;

                            await _dataProvider.UpdateRelGradeRoomScenario(relGradeRoomScenario);
                        }

                        #endregion

                        #region insert update rel_test_scenario_param and test variable

                        if (testScenarioEvent.TestParameters != null)
                        {
                            foreach (var testParameterEvent in testScenarioEvent.TestParameters)
                            {
                                var relTestScenarioParam = await _testScenarioDataProvider.GetRelTestScenarioParam(testScenario.Id, testParameterEvent.DataId);

                                if (relTestScenarioParam == null)
                                {
                                    relTestScenarioParam = new RelTestScenarioParam()
                                    {
                                        TestParameterId = testParameterEvent.DataId,
                                        TestScenarioId = testScenario.Id,
                                        CreatedAt = DateTime.Now,
                                        CreatedBy = gradeRoom.CreatedBy
                                    };
                                    await _testScenarioDataProvider.InsertRelTestScenarioParam(relTestScenarioParam);
                                }
                                else
                                {
                                    relTestScenarioParam.TestParameterId = testParameterEvent.DataId;
                                    relTestScenarioParam.TestScenarioId = testScenario.Id;
                                    relTestScenarioParam.CreatedAt = DateTime.Now;
                                    relTestScenarioParam.CreatedBy = gradeRoom.CreatedBy;

                                    await _testScenarioDataProvider.UpdateRelTestScenarioParam(relTestScenarioParam);
                                }

                                if (testParameterEvent.TestVariables != null)
                                {
                                    foreach (var testVariableEvent in testParameterEvent.TestVariables)
                                    {
                                        isNew = false;
                                        var testVariable = (relTestScenarioParam != null
                                                            ? await _testVariableDataProvider.GetByRelTestScenarioParamIdAndVariableName(relTestScenarioParam.Id, testVariableEvent.VariableName.ToLower())
                                                            : null);

                                        if (testVariable == null)
                                        {
                                            isNew = true;
                                            testVariable = new TestVariable()
                                            {
                                                TestParameterId = relTestScenarioParam.Id,
                                                CreatedAt = DateTime.Now
                                            };
                                        }

                                        testVariable.TresholdOperator = testVariableEvent.ThresholdOperator;
                                        testVariable.TresholdValue = ((long?)testVariableEvent.ThresholdValue);
                                        testVariable.TresholdMax = ((long?)testVariableEvent.ThresholdValueFrom);
                                        testVariable.TresholdMin = ((long?)testVariableEvent.ThresholdValueTo);
                                        testVariable.VariableName = testVariableEvent.VariableName;
                                        testVariable.Sequence = 0;
                                        if (testVariableEvent.Sequence.HasValue)
                                        {
                                            testVariable.Sequence = testVariableEvent.Sequence.Value;
                                        }
                                        testVariable.CreatedBy = testVariableEvent.CreatedBy;
                                        testVariable.UpdatedAt = DateTime.Now;
                                        testVariable.UpdatedBy = testVariableEvent.UpdatedBy;

                                        if (isNew)
                                        {
                                            await _testVariableDataProvider.Insert(testVariable);
                                        }
                                        else
                                        {
                                            await _testVariableDataProvider.Update(testVariable);
                                        }

                                    }
                                }

                            }
                        }

                        #endregion
                    }
                }

                #endregion

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
                _logger.LogError(JsonSerializer.Serialize(@event));
            }
        }
    }
}