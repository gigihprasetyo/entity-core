using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Exceptions;
using qcs_product.API.Helpers;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;

namespace qcs_product.API.DataProviders.Collection
{
    public class GenerateQcResultDataProvider : IGenerateQcResultDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<GenerateQcResultDataProvider> _logger;
        private readonly IQcRequestDataProvider _QcRequestDataProvider;

        public GenerateQcResultDataProvider(QcsProductContext context,
            IQcRequestDataProvider QcRequestDataProvider,
            ILogger<GenerateQcResultDataProvider> logger)
        {
            _context = context;
            _QcRequestDataProvider = QcRequestDataProvider;
            _logger = logger;
        }
        public async Task Generate(int qcTestId, List<QcTransactionGroupSampleValue> dataSampleObservasi,
            List<QcTransactionGroupSampleValue> dataSampleUjiIdentifikasi)
        {
            var data = await (from t in _context.QcTransactionGroups
                where t.Id == qcTestId
                      && t.RowStatus == null
                select t).FirstOrDefaultAsync();

            #region generate qc result - observasi

            var observasiSampleTestIds =
                dataSampleObservasi.Select(x => x.QcTransactionSampleId).Distinct().ToList();

            var observasiTransactionSamples = await (from txs in _context.QcTransactionSamples
                join s in _context.QcSamples on txs.QcSampleId equals s.Id
                where txs.RowStatus == null
                      && observasiSampleTestIds.Contains(txs.Id)
                select new QcTransactionSampleViewModel()
                {
                    Id = txs.Id,
                    QcSampleId = txs.QcSampleId,
                    QcSamplingId = txs.QcSamplingId ?? 0,
                    TestParameterId = s.TestParamId,
                    TestScenarioId = s.TestScenarioId ?? 0
                }).ToListAsync();

            var observasiSampleIds = observasiTransactionSamples.Select(x => x.QcSampleId).ToList();

            var observasiResultList = await (from res in _context.QcResults
                where observasiSampleIds.Contains(res.SampleId)
                select res).ToListAsync();
            
            if (observasiResultList.Any())
            {
                _context.QcResults.RemoveRange(observasiResultList);
            }

            var testVariables = await (from tv in _context.TransactionTestVariable
                join tstp in _context.TransactionRelTestScenarioParam on tv.TestParameterId equals tstp.Id
                join tp in _context.TransactionTestParameter on tstp.TestParameterId equals tp.Id
                where tv.RowStatus == null
                orderby tv.Sequence
                select new TestVariableViewModel()
                {
                    Id = tv.Id,
                    TestScenarioId = tstp.TestScenarioId,
                    TestParameterId = tstp.TestParameterId,
                    TresholdMin = tv.ThresholdValueFrom.Value,
                    TresholdMax = tv.ThresholdValueTo.Value,
                    TresholdValue = tv.TresholdValue,
                    TresholdOperator = tv.TresholdOperator,
                    Sequence = tv.Sequence.Value,
                    VariableName = tv.VariableName
                }).ToListAsync();

            var transactionSampleMap = new ConcurrentDictionary<QcTransactionGroupSampleValue, QcResult>();

            Parallel.ForEach(dataSampleObservasi, async tsO =>
            {
                var newQcResult = new QcResult
                {
                    SampleId = 0
                };

                var sampleTestValue = tsO.Value;

                try
                {
                    var sampleTest =
                        observasiTransactionSamples.FirstOrDefault(x => x.Id == tsO.QcTransactionSampleId);

                    if (sampleTest == null)
                    {
                        _logger.LogError("GenerateQcResult. Sample Test ID: {QcTransactionSampleId} not found",
                            tsO.QcTransactionSampleId);
                        return;
                    }

                    newQcResult.SampleId = sampleTest.QcSampleId;

                    if (!Regex.IsMatch(sampleTestValue, @"^\d+$"))
                    {
                        _logger.LogError(
                            "GenerateQcResult. Sample Test ID: {QcTransactionSampleId} is invalid value: {Value} ",
                            tsO.QcTransactionSampleId, tsO.Value);
                        throw new InvalidValueException("Invalid sample value");
                    }
                    
                    var testParameterId = sampleTest.TestParameterId;
                    var testScenarioId = sampleTest.TestScenarioId;

                    var testVariableConclusion = await GetTestVariableConclusion(testVariables, testParameterId, testScenarioId, sampleTestValue);

                    newQcResult.TestVariableConclusion = sampleTestValue == null ? ApplicationConstant.LABEL_CONCLUSION_STRIPE : testVariableConclusion.Conclusion;
                    newQcResult.TestVariableId = sampleTestValue == null ? 0 : testVariableConclusion.TestVariableId;
                }
                catch (Exception)
                {
                    //ignore
                }
                
                newQcResult.Value = sampleTestValue;
                newQcResult.CreatedBy = data.UpdatedBy;
                newQcResult.UpdatedBy = data.UpdatedBy;
                newQcResult.CreatedAt = DateHelper.Now();
                newQcResult.UpdatedAt = DateHelper.Now();

                transactionSampleMap.TryAdd(tsO, newQcResult);
            });

            if (transactionSampleMap.Any())
            {
                //save changes to DB
                await _context.QcResults.AddRangeAsync(transactionSampleMap.Values);
                await _context.SaveChangesAsync();

                #region update qc request conclusion

                var samplingIds = await (from txs in _context.QcTransactionSamples
                        join s in _context.QcSamples on txs.QcSampleId equals s.Id
                        where txs.RowStatus == null
                              && observasiSampleTestIds.Contains(txs.Id)
                        select s.QcSamplingId)
                    .Distinct()
                    .ToListAsync();

                var requestIds = await (from smp in _context.QcSamplings
                        where smp.RowStatus == null
                              && samplingIds.Contains(smp.Id)
                        select smp.RequestQcsId)
                    .ToListAsync();

                foreach (var requestId in requestIds)
                {
                    await _QcRequestDataProvider.GenerateUpdateConclusion(requestId);
                }

                #endregion
            }

            #endregion

            #region generate qc result - uji identifikasi

            var ujiIdentifikasiSampleTestIds = dataSampleUjiIdentifikasi.Select(x => x.QcTransactionSampleId).Distinct().ToList();
            
            var ujiIdentifikasiSampleTestList = await (from txs in _context.QcTransactionSamples
                join s in _context.QcSamples on txs.QcSampleId equals s.Id
                where txs.RowStatus == null
                      && ujiIdentifikasiSampleTestIds.Contains(txs.Id)
                select txs).ToListAsync();

            var ujiIdentifikasiSampleIds = ujiIdentifikasiSampleTestList.Select(x => x.QcSampleId).ToList();

            var ujiIdentifikasiResultList = await (from res in _context.QcResults
                where ujiIdentifikasiSampleIds.Contains(res.SampleId)
                select res).ToListAsync();

            Parallel.ForEach(ujiIdentifikasiResultList, qcResult =>
            {
                try
                {
                    var sampleTest = ujiIdentifikasiSampleTestList.FirstOrDefault(x => x.QcSampleId == qcResult.SampleId);
                    if (sampleTest == null)
                    {
                        throw new DataNotFoundException($"Sample test ID: {qcResult.SampleId} not found");
                    }
                
                    var ts1 = dataSampleUjiIdentifikasi.FirstOrDefault(x => x.QcTransactionSampleId == sampleTest.Id);
                
                    if (ts1 == null || string.IsNullOrEmpty(ts1.Value))
                    {
                        throw new InvalidValueException($"Sample value is empty. Sample Test ID: {sampleTest.Id}");
                    }
                
                    var dataMikroba = JsonSerializer.Deserialize<List<ListNoteViewModel>>(ts1.Value);
                
                    if (dataMikroba == null || !dataMikroba.Any())
                    {
                        throw new DataNotFoundException("Microba is empty");
                    }

                    var labelMikroba = dataMikroba.Select(x => x.label).Distinct().ToList();
                    var noteMikroba = string.Join(',', labelMikroba);

                    qcResult.Note = noteMikroba;
                }
                catch (Exception e)
                {
                   _logger.LogError(e, "{Message}", e.Message);
                }
            });
            
           
            await _context.SaveChangesAsync();

            #endregion
        }

        public async Task<QcTestVariableConclusion> GetTestVariableConclusion(List<TestVariableViewModel> testVariables, int testParameterId, int testScenarioId, string sampleTestValue)
        {
            var testVariableConclusion = new QcTestVariableConclusion
            {
                Conclusion = ApplicationConstant.LABEL_CONCLUSION_PASS
            };

            var testVariableList = testVariables
                .Where(x => x.TestParameterId == testParameterId)
                .Where(x => x.TestScenarioId == testScenarioId)
                .OrderBy(x => x.Sequence)
                .ToList();

            bool isHasThreshold = testVariableList.Any(x => x.TresholdValue != null
                || x.TresholdMin != null
                || x.TresholdMax != null);

            if (!isHasThreshold)
            {
                testVariableConclusion.Conclusion = ApplicationConstant.LABEL_CONCLUSION_STRIPE;
                return await Task.FromResult(testVariableConclusion);
            }

            foreach (var th in from th in testVariableList let valid = CheckValue(th, sampleTestValue) where valid select th)
            {
                switch (th.VariableName)
                {
                    case ApplicationConstant.TEST_VARIABLE_ALERT:
                        testVariableConclusion.Conclusion = ApplicationConstant.LABEL_CONCLUSION_PASS;
                        testVariableConclusion.TestVariableId = th.Id;
                        break;
                    case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                        testVariableConclusion.Conclusion = ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                        testVariableConclusion.TestVariableId = th.Id;
                        break;
                    case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                        testVariableConclusion.Conclusion = ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                        testVariableConclusion.TestVariableId = th.Id;
                        break;
                }
            }

            return await Task.FromResult(testVariableConclusion);
        }

        private static bool CheckValue(TestVariableViewModel threshold, string value)
        {
            var valid = threshold.TresholdOperator switch
            {
                ApplicationConstant.THRESHOLD_EQUAL => int.Parse(value) == threshold.TresholdValue,
                ApplicationConstant.THRESHOLD_GREATER_THAN => int.Parse(value) <= threshold.TresholdValue,
                ApplicationConstant.THRESHOLD_LESS_THAN => int.Parse(value) >= threshold.TresholdValue,
                ApplicationConstant.THRESHOLD_GREATER_THAN_OR_EQUAL => int.Parse(value) < threshold.TresholdValue,
                ApplicationConstant.THRESHOLD_LESS_THAN_OR_EQUAL => int.Parse(value) > threshold.TresholdValue,
                ApplicationConstant.THRESHOLD_IN_BETTWEEN => int.Parse(value) <= threshold.TresholdMin
                                                             && int.Parse(value) >= threshold.TresholdMax,
                ApplicationConstant.THRESHOLD_IN_BETTWEEN_OR_EQUAL => int.Parse(value) < threshold.TresholdMin
                                                                      && int.Parse(value) > threshold.TresholdMax,
                _ => false
            };
            return valid;
        }
    }
}