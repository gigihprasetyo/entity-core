using Microsoft.Extensions.Logging;
using qcs_product.API.DataProviders;
using qcs_product.API.ViewModels;
using qcs_product.API.Models;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TestParameterBusinessProvider : ITestParameterBusinessProvider
    {
        private readonly ITestParameterDataProvider _dataProvider;
        private readonly ILogger<TestParameterBusinessProvider> _logger;
        public TestParameterBusinessProvider(ITestParameterDataProvider dataProvider, ILogger<TestParameterBusinessProvider> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger;
        }

        public async Task<ResponseViewModel<TestParameterByScenarioRelationViewModel>> List(int roomId, int testScenarioId)
        {
            ResponseViewModel<TestParameterByScenarioRelationViewModel> result = new ResponseViewModel<TestParameterByScenarioRelationViewModel>();
            List<TestParameterByScenarioRelationViewModel> getData = await _dataProvider.List(roomId, testScenarioId);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;
        }

        public async Task<ResponseViewModel<TestParameterByScenarioRelationViewModel>> ListAlt(string roomIds, string testScenarioLabel)
        {
            ResponseViewModel<TestParameterByScenarioRelationViewModel> result = new ResponseViewModel<TestParameterByScenarioRelationViewModel>();

            var roomIdsFilter = new List<int>();
            if (roomIdsFilter == null)
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }
            else
            {
                // filter status from param status is string
                roomIdsFilter = roomIds.Split(',').Select(x => Int32.Parse(x)).Reverse().ToList();
            }

            // set label scenario test
            var testScenarioLabelRequired = new List<string>();

            testScenarioLabelRequired.Add(ApplicationConstant.TEST_SCENARIO_LABEL_IN_OPERATIONS);
            testScenarioLabelRequired.Add(ApplicationConstant.TEST_SCENARIO_LABEL_AT_REST);

            if (!testScenarioLabelRequired.Contains(testScenarioLabel))
            {
                result.StatusCode = 400;
                result.Message = ApplicationConstant.WRONG_LABEL_TEST_SCENARIO;
                return result;
            }

            List<TestParameterByScenarioRelationViewModel> getData = await _dataProvider.ListAlt(roomIdsFilter, testScenarioLabel);
            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;
        }

        public async Task<ResponseViewModel<TestParameterViewModel>> ListShort(string search, int limit, int page, int TestGroupId)
        {
            BasePagination pagination = new BasePagination(page, limit);
            ResponseViewModel<TestParameterViewModel> result = new ResponseViewModel<TestParameterViewModel>();
            List<TestParameterViewModel> getData = await _dataProvider.ListShort(search, limit, pagination.CalculateOffset(), TestGroupId);

            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;
        }

        public async Task<ResponseViewModel<TestParameterByScenarioRelationViewModel>> ListV2(string roomIds, string testScenarioLabel, string purposeId)
        {
            ResponseViewModel<TestParameterByScenarioRelationViewModel> result = new ResponseViewModel<TestParameterByScenarioRelationViewModel>();
            var roomIdsFilter = new List<int>();
            if (roomIds != null)
            {
                // filter status from param status is string
                roomIdsFilter = roomIds.Split(',').Select(x => Int32.Parse(x)).Reverse().ToList();
            }
            else
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            var purposeFilter = new List<int>();
            if (purposeId != null)
            {
                purposeFilter = purposeId.Split(',').Select(x => Int32.Parse(x)).Reverse().ToList();
            }
            else
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
                return result;
            }

            List<TestParameterByScenarioRelationViewModel> getData = await _dataProvider.ListV2(roomIdsFilter, testScenarioLabel, purposeFilter);
            if (!getData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = getData;
            }

            return result;

        }
    }
}
