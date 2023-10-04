using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders.Collection
{
    public class ActivityDataProvider : IActivityDataProvider
    {
        private readonly QcsProductContext _context;

        public ActivityDataProvider(QcsProductContext context)
        {
            _context = context;
        }
        
        public async Task<Activity> GetByCode(string code)
        {
            var activity = await _context.Activities.FirstOrDefaultAsync(x => x.Code == code);
            return activity;
        }

        public async Task<Activity> Insert(Activity activity)
        {
            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<Activity> Update(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<List<Activity>> GetListByCodes(List<string> codes)
        {
            var activities = await _context.Activities.Where(x => codes.Contains(x.Code)).ToListAsync();
            return activities;
        }
    }
}