using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders.Collection
{
    public class ToolActivityDataProvider : IToolActivityDataProvider
    {
        private readonly QcsProductContext _context;

        public ToolActivityDataProvider(QcsProductContext context)
        {
            _context = context;
        }
        
        public async Task<ToolActivity> GetByCode(string code)
        {
            var activity = await _context.ToolActivities.FirstOrDefaultAsync(x => x.ActivityCode == code);
            return activity;
        }

        public async Task<List<ToolActivity>> GetListByToolId(int toolId)
        {
            var activites = await _context.ToolActivities.Where(x => x.ToolId == toolId).ToListAsync();
            return activites;
        }

        public async Task<ToolActivity> Insert(ToolActivity toolActivity)
        {
            await _context.ToolActivities.AddAsync(toolActivity);
            await _context.SaveChangesAsync();
            return toolActivity;
        }

        public async Task<ToolActivity> Update(ToolActivity toolActivity)
        {
            _context.ToolActivities.Update(toolActivity);
            await _context.SaveChangesAsync();
            return toolActivity;
        }
    }
}