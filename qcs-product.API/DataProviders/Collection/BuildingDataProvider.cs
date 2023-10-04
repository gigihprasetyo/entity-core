using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders.Collection
{
    public class BuildingDataProvider : IBuildingDataProvider
    {
        private readonly QcsProductContext _context;
        public BuildingDataProvider(QcsProductContext context)
        {
            _context = context;
        }
        public async Task<Building> GetByCode(string code)
        {
            return await _context.Buildings.FirstOrDefaultAsync(x => x.Code == code);
        }
        public async Task<Building> Insert(Building building)
        {
            await _context.Buildings.AddAsync(building);
            await _context.SaveChangesAsync();
            return building;
        }
        public async Task<Building> Update(Building building)
        {
            _context.Buildings.Update(building);
            await _context.SaveChangesAsync();
            return building;
        }
    }
}