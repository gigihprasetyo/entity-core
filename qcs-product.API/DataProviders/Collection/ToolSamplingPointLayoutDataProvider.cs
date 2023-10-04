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
    public class ToolSamplingPointLayoutDataProvider : IToolSamplingPointLayoutDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<ToolSamplingPointLayoutDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public ToolSamplingPointLayoutDataProvider(QcsProductContext context, ILogger<ToolSamplingPointLayoutDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<ToolSamplingPointLayout>> InsertList(List<ToolSamplingPointLayout> lsToolSamplingPointLayout)
        {
            try
            {
                await _context.ToolSamplingPointLayouts.AddRangeAsync(lsToolSamplingPointLayout);
                await _context.SaveChangesAsync();
                return lsToolSamplingPointLayout;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return null;
        }

        public async Task<List<ToolSamplingPointLayout>> GetByToolCode(string toolCode)
        {
            return await (from tspl in _context.ToolSamplingPointLayouts
                          join tp in _context.ToolPurposes on tspl.ToolPurposeId equals tp.Id
                          join t in _context.Tools on tp.ToolId equals t.Id
                          where t.ToolCode == toolCode
                          select tspl).ToListAsync();
        }

        public async Task DeleteByToolCode(string toolCode)
        {
            ToolSamplingPointLayout[] arrObj = (await GetByToolCode(toolCode)).ToArray();

            _context.ToolSamplingPointLayouts.RemoveRange(arrObj);
            await _context.SaveChangesAsync();
        }
    }
}
