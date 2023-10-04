using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class PurposeDataProvider : IPurposeDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<PurposeDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public PurposeDataProvider(QcsProductContext context, ILogger<PurposeDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<Purpose> GetById(int id)
        {
            return await _context.Purposes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Purpose> GetByCode(string code)
        {
            return await _context.Purposes.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Purpose> Insert(Purpose purpose)
        {
            await _context.Purposes.AddAsync(purpose);
            await _context.SaveChangesAsync();
            return purpose;
        }

        public async Task<Purpose> Update(Purpose purpose)
        {
            _context.Purposes.Update(purpose);
            await _context.SaveChangesAsync();
            return purpose;
        }

        public async Task<List<PurposeViewModel>> List(string search)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from p in _context.Purposes
                                where ((EF.Functions.Like(p.Code.ToLower(), "%" + filter + "%")) ||
                                  (EF.Functions.Like(p.Name.ToLower(), "%" + filter + "%")))
                                && p.RowStatus == null
                                select new PurposeViewModel
                                {
                                    Id = p.Id,
                                    //RequestTypeId = p.RequestTypeId,
                                    Code = p.Code,
                                    Name = p.Name,
                                    // }).Where(x => x.RequestTypeId == RequestTypeId || RequestTypeId == 0).ToListAsync();
                                }).ToListAsync();

            _logger.LogInformation($"Data result : {result}");

            return result;
        }


    }
}
