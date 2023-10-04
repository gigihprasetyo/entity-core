using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class TestVariableDataProvider : ITestVariableDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<TestParameterDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public TestVariableDataProvider(QcsProductContext context, ILogger<TestParameterDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<TestVariable> GetByRelTestScenarioParamIdAndVariableName(int testScenarioParamId, string variableName)
        {
            return await (from tv in _context.TestVariables
                          where tv.TestParameterId == testScenarioParamId && tv.VariableName.ToLower() == variableName.ToLower()
                          select tv).FirstOrDefaultAsync();
        }

        public async Task<TestVariable> Insert(TestVariable testVariable)
        {
            await _context.TestVariables.AddAsync(testVariable);
            await _context.SaveChangesAsync();
            return testVariable;
        }

        public async Task<TestVariable> Update(TestVariable testVariable)
        {
            _context.TestVariables.Update(testVariable);
            await _context.SaveChangesAsync();
            return testVariable;
        }
    }
}
