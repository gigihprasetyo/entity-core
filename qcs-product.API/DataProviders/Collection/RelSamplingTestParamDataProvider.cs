using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class RelSamplingTestParamDataProvider : IRelSamplingTestParamDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<RelSamplingTestParamDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public RelSamplingTestParamDataProvider(QcsProductContext context, ILogger<RelSamplingTestParamDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<RelSamplingTestParam> Get(int samplingPointId, int testScnParamId)
        {
            return await (from rstp in _context.RelSamplingTestParams
                          where rstp.SamplingPointId == samplingPointId && rstp.TestScenarioParamId == testScnParamId
                          select rstp).FirstOrDefaultAsync();
        }

        public async Task<List<RelSamplingTestParam>> GetByToolCode(string toolCode)
        {
            return await (from rstp in _context.RelSamplingTestParams
                          join rst in _context.RelSamplingTools on rstp.SamplingPointId equals rst.Id
                          join tp in _context.ToolPurposes on rst.ToolPurposeId equals tp.Id
                          join t in _context.Tools on tp.ToolId equals t.Id
                          where t.ToolCode == toolCode
                          select rstp).ToListAsync();
        }

        public async Task<List<RelSamplingTestParam>> InsertList(List<RelSamplingTestParam> lsRelSamplingTestParam)
        {
            try
            {
                await _context.RelSamplingTestParams.AddRangeAsync(lsRelSamplingTestParam);
                await _context.SaveChangesAsync();
                return lsRelSamplingTestParam;

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return null;
        }

        public async Task<RelSamplingTestParam> Update(RelSamplingTestParam relSamplingTestParam)
        {
            try
            {
                _context.RelSamplingTestParams.Update(relSamplingTestParam);
                await _context.SaveChangesAsync();
                return relSamplingTestParam;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

            return null;
        }

        public async Task DeleteByToolCode(string toolCode)
        {
            RelSamplingTestParam[] arrObj = (await GetByToolCode(toolCode)).ToArray();

            _context.RelSamplingTestParams.RemoveRange(arrObj);
            await _context.SaveChangesAsync();
        }

    }
}
