using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class ProductProductionPhasesDataProvider : IProductProductionPhasesDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<ProductProductionPhasesDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public ProductProductionPhasesDataProvider(QcsProductContext context, ILogger<ProductProductionPhasesDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<ProductProductionPhaseViewModel>> List(string search, int limit)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = (from pp in _context.ProductProductionPhases
                               join pr in _context.Items on pp.ItemId equals pr.Id
                               where (EF.Functions.Like(pp.Name.ToLower(), "%" + filter + "%"))
                               select new ProductProductionPhaseViewModel
                               {
                                   Id = pp.Id,
                                   ProductProdPhaseCode = pp.ProductProdPhaseCode,
                                   ItemId = pp.ItemId,
                                   ItemName = pr.Name,
                                   Name = pp.Name,
                                   Rooms = (from r in _context.Rooms
                                            join rpppr in _context.RelProductProdPhaseToRooms on r.Id equals rpppr.RoomId
                                            where rpppr.ProductProductionPhasesId == pp.Id
                                            select new RoomViewModel
                                            {
                                                Id = r.Id,
                                                Code = r.Code,
                                                Name = r.Name
                                            }).ToList()
                               }).AsQueryable();

            var resultData = new List<ProductProductionPhaseViewModel>();

            if (limit > 0)
            {
                resultData = await result.Take(limit).ToListAsync();
            }
            else
            {
                resultData = await result.Take(100).ToListAsync();
            }

            return resultData;
        }

        public async Task<List<ProductProductionPhasesPersonelViewModel>> GetPersonelByPhaseId(int phaseId)
        {
            var result = await (from pppp in _context.ProductProductionPhasesPersonels
                                join ppp in _context.ProductProductionPhases on pppp.ProductProductionPhasesId equals ppp.Id
                                where pppp.ProductProductionPhasesId == phaseId
                                select new ProductProductionPhasesPersonelViewModel
                                {
                                    ProductProdPhasesPersonelId = pppp.Id,
                                    PersonelNik = pppp.PersonelNik,
                                    PersonelName = pppp.PersonelName,
                                    ProductProductionPhasesId = pppp.ProductProductionPhasesId,
                                    ProductProductionPhasesName = ppp.Name
                                }).ToListAsync();
            return result;
        }
    }
}
