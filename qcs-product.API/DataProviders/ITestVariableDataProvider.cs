using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface ITestVariableDataProvider
    {
        public Task<TestVariable> GetByRelTestScenarioParamIdAndVariableName(int testScenarioParamId, string variableName);
        public Task<TestVariable> Insert(TestVariable testVariable);
        public Task<TestVariable> Update(TestVariable testVariable);
    }
}
