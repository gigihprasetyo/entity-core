using Microsoft.Extensions.Logging;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.IntegrationEvents;
using qcs_product.API.DataProviders;
using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace qcs_product.API.EventHandlers
{
    public class TestScenarioEventHandler : IIntegrationEventHandler<TestScenarioIntegrationEvent>
    {
        private readonly ILogger<TestScenarioEventHandler> _logger;
        private readonly ITestScenarioDataProvider _testScenarioDataProvider;

        public TestScenarioEventHandler(ILogger<TestScenarioEventHandler> logger, ITestScenarioDataProvider testScenarioDataProvider)
        {
            _logger = logger;
            _testScenarioDataProvider = testScenarioDataProvider;
        }

        public async Task Handle(TestScenarioIntegrationEvent @event)
        {
            _logger.LogInformation("sync building from google pub/sub");
            try
            {
                _logger.LogInformation(JsonSerializer.Serialize(@event));

                _logger.LogInformation("insert or update test scenario");
                var isNew = false;
                //var testScenario = await _testScenarioDataProvider.GetById(@event.DataId);

                //if (testScenario == null)
                //{
                    //isNew = true;
                    var testScenario = new TestScenario();
                    testScenario.Label = @event.TestScenarioLabel;
                    testScenario.Name = @event.TestScenarioName;
                    testScenario.CreatedAt = DateTime.Now;
                    testScenario.CreatedBy = @event.CreatedBy;
                    testScenario.UpdatedAt = DateTime.Now;
                    testScenario.UpdatedBy = @event.UpdatedBy;
                    testScenario.RowStatus = @event.RowStatus;
                //}
                await _testScenarioDataProvider.Insert(testScenario);

                if (isNew)
                {
                    await _testScenarioDataProvider.Insert(testScenario);
                }
                else
                {
                    await _testScenarioDataProvider.Update(testScenario);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

        }
    }
}
