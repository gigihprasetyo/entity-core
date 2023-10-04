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
    public class ToolPurposeDataProvider : IToolPurposeDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<ToolPurposeDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public ToolPurposeDataProvider(QcsProductContext context, ILogger<ToolPurposeDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<ToolPurpose> Insert(ToolPurpose toolPurpose)
        {
            try
            {
                await _context.ToolPurposes.AddAsync(toolPurpose);
                await _context.SaveChangesAsync();
                return toolPurpose;

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return null;
        }

        public async Task<List<ToolPurpose>> GetByToolCode(string toolCode)
        {
            return await (from rtp in _context.ToolPurposes
                          join t in _context.Tools on rtp.ToolId equals t.Id
                          where t.ToolCode == toolCode
                          select rtp).ToListAsync();
        }

        public async Task DeleteByToolCode(string toolCode)
        {
            ToolPurpose[] arrObj = (await GetByToolCode(toolCode)).ToArray();

            _context.ToolPurposes.RemoveRange(arrObj);
            await _context.SaveChangesAsync();
        }
    }
}
