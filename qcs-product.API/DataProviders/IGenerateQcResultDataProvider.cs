using System.Collections.Generic;
using System.Threading.Tasks;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders
{
    public interface IGenerateQcResultDataProvider
    {
        
        
        Task Generate(int qcTestId, List<QcTransactionGroupSampleValue> dataSampleObservasi,
            List<QcTransactionGroupSampleValue> dataSampleUjiIdentifikasi);
        public Task<QcTestVariableConclusion> GetTestVariableConclusion(List<TestVariableViewModel> testVariables, int testParameterId, int testScenarioId,
            string sampleTestValue);
    }
}