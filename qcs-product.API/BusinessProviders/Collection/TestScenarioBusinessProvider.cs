using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.DataProviders;
using qcs_product.API.ViewModels;
using qcs_product.Constants;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class TestScenarioBusinessProvider : ITestScenarioBusinessProvider
    {
        private readonly ILogger<TestScenarioBusinessProvider> _logger;
        private readonly ITestScenarioDataProvider _dataProvider;

        public TestScenarioBusinessProvider(ILogger<TestScenarioBusinessProvider> logger, ITestScenarioDataProvider dataProvider)
        {
            _logger = logger;
            _dataProvider = dataProvider;
        }

        public async Task<ResponseViewModel<TestScenarioViewModel>> GetList(string search, string GradeRoomId)
        {
            var gradeRoomFilter = new List<int>();
            // filter status from param status is string
            gradeRoomFilter = GradeRoomId.Split(',').Select(x => Int32.Parse(x)).Reverse().ToList();
            var response = new ResponseViewModel<TestScenarioViewModel>();
            var testScenarioList = await _dataProvider.GetListTransaction(search, gradeRoomFilter);

            response.StatusCode = 200;
            response.Message = ApplicationConstant.OK_MESSAGE;
            response.Data = testScenarioList;

            return response;
        }
    }
}