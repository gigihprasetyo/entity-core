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
    public class ToolPurposeToMasterPurposeDataProvider : IToolPurposeToMasterPurposeDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<ToolPurposeToMasterPurposeDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public ToolPurposeToMasterPurposeDataProvider(QcsProductContext context, ILogger<ToolPurposeToMasterPurposeDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<ToolPurposeToMasterPurpose>> InsertList(List<ToolPurposeToMasterPurpose> lsToolPurposeToMasterPurpose)
        {
            try
            {
                await _context.ToolPurposeToMasterPurposes.AddRangeAsync(lsToolPurposeToMasterPurpose);
                await _context.SaveChangesAsync();
                return lsToolPurposeToMasterPurpose;

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return null;
        }

        public async Task<List<ToolPurposeToMasterPurpose>> GetByToolCode(string toolCode)
        {
            return await (from tpmp in _context.ToolPurposeToMasterPurposes
                          join tp in _context.ToolPurposes on tpmp.ToolPurposeId equals tp.Id
                          join t in _context.Tools on tp.ToolId equals t.Id
                          where t.ToolCode == toolCode
                          select tpmp).ToListAsync();
        }

        public async Task DeleteByToolCode(string toolCode)
        {
            ToolPurposeToMasterPurpose[] arrObj = (await GetByToolCode(toolCode)).ToArray();

            _context.ToolPurposeToMasterPurposes.RemoveRange(arrObj);
            await _context.SaveChangesAsync();
        }
    }
}
