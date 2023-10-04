using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class ProductionPhaseDataProvider : IProductionPhaseDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<ProductionPhaseDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public ProductionPhaseDataProvider(QcsProductContext context, ILogger<ProductionPhaseDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<ProductionPhaseViewModel>> GetDetailProductionPhaseById(int id)
        {
            var result = await (from p in _context.ProductionPhases
                                where p.Id == id
                                select new ProductionPhaseViewModel
                                {
                                    Id = p.Id,
                                    ProdPhaseCode = p.ProdPhaseCode,
                                    Name = p.Name,
                                    RowStatus = p.RowStatus

                                }).ToListAsync();
            _logger.LogInformation($"Data result : {result}");

            return result;
        }

        public async Task<List<ProductionPhaseViewModel>> List(string search, int limit, int page)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            page = 0;

            var result = (from p in _context.ProductionPhases
                               where (EF.Functions.Like(p.Name.ToLower(), "%" + filter + "%"))
                               select new ProductionPhaseViewModel
                               {
                                   Id = p.Id,
                                   ProdPhaseCode = p.ProdPhaseCode,
                                   Name = p.Name,
                                   RowStatus = p.RowStatus

                               }).AsQueryable();

            var resultData = new List<ProductionPhaseViewModel>();

            if (limit > 0)
            {
                resultData = await result.Take(limit).ToListAsync();
            }
            else
            {
                resultData = await result.ToListAsync();
            }

            return resultData;
        }
    }
}
