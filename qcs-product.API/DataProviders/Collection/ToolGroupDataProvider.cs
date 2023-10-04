using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders.Collection
{
    public class ToolGroupDataProvider : IToolGroupDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<ToolGroupDataProvider> _logger;

        public ToolGroupDataProvider(QcsProductContext context, ILogger<ToolGroupDataProvider> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ToolGroup> GetByCode(string code)
        {
            var toolGroup = await _context.ToolGroups.FirstOrDefaultAsync(x => x.Code == code);
            return toolGroup;
        }

        public async Task<List<ToolGroup>> GetList()
        {
            //TODO apakah perlu filter row_status ??
            var toolGroups = await _context.ToolGroups.ToListAsync();
            return toolGroups;
        }

        public async Task<ToolGroup> Insert(ToolGroup toolGroup)
        {
            await _context.ToolGroups.AddAsync(toolGroup);
            await _context.SaveChangesAsync();
            return toolGroup;
        }

        public async Task<ToolGroup> Update(ToolGroup toolGroup)
        {
            _context.ToolGroups.Update(toolGroup);
            await _context.SaveChangesAsync();
            return toolGroup;
        }
    }
}