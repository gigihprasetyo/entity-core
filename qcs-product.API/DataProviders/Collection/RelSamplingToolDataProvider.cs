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
    public class RelSamplingToolDataProvider : IRelSamplingToolDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<RelSamplingToolDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public RelSamplingToolDataProvider(QcsProductContext context, ILogger<RelSamplingToolDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<RelSamplingTool>> GetByToolCode(string toolCode)
        {
            return await (from rst in _context.RelSamplingTools
                          join tp in _context.ToolPurposes on rst.ToolPurposeId equals tp.Id
                          join t in _context.Tools on tp.ToolId equals t.Id
                          where t.ToolCode == toolCode
                          select rst).ToListAsync();
        }

        public async Task<RelSamplingTool> GetByCodeAndSamplingPointCode(string samplingPointCode, string toolCode)
        {
            var result = await (from rel_sp in _context.RelSamplingTools
                                join sp in _context.SamplingPoints on rel_sp.SamplingPointId equals sp.Id
                                join tp in _context.ToolPurposes on rel_sp.ToolPurposeId equals tp.Id
                                join t in _context.Tools on tp.ToolId equals t.Id
                                where sp.Code == samplingPointCode
                                && t.ToolCode == toolCode
                                select rel_sp
                                ).FirstOrDefaultAsync();
            _logger.LogInformation($"Data result: {result}");
            return result;
        }

        public async Task<List<RelSamplingTool>> InsertList(List<RelSamplingTool> lsRelSamplingTool)
        {
            try
            {
                await _context.RelSamplingTools.AddRangeAsync(lsRelSamplingTool);
                await _context.SaveChangesAsync();
                return lsRelSamplingTool;

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return null;
        }

        public async Task<RelSamplingTool> Update(RelSamplingTool relSamplingTool)
        {
            try
            {
                _context.RelSamplingTools.Update(relSamplingTool);
                await _context.SaveChangesAsync();
                return relSamplingTool;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

            return null;
        }

        public async Task DeleteByToolCode(string toolCode)
        {
            RelSamplingTool[] arrObj = (await GetByToolCode(toolCode)).ToArray();

            _context.RelSamplingTools.RemoveRange(arrObj);
            await _context.SaveChangesAsync();
        }

    }
}
