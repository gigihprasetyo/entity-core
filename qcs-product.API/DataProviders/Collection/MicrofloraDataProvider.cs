using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class MicrofloraDataProvider : IMicrofloraDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<MicrofloraDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public MicrofloraDataProvider(QcsProductContext context, ILogger<MicrofloraDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<ShortDataListViewModel>> List(string search)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from mc in _context.Microfloras
                                where (EF.Functions.Like(mc.Name.ToLower(), "%" + filter + "%"))
                                && mc.RowStatus == null
                                && mc.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                select new ShortDataListViewModel
                                {
                                    Id = mc.Id,
                                    Label = mc.Name,
                                }).OrderBy(x => x.Label).ToListAsync();

            return result;
        }

        public async Task<Microflora> Insert(Microflora insert)
        {
            try
            {
                await _context.Microfloras.AddAsync(insert);
                await _context.SaveChangesAsync();
                return insert;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

            return null;
        }

        public async Task<Microflora> GetByCode(string code)
        {
            return await _context.Microfloras.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Microflora> Update(Microflora microflora)
        {
            try
            {
                _context.Microfloras.Update(microflora);
                await _context.SaveChangesAsync();
                return microflora;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

            return null;
        }
    }
}
