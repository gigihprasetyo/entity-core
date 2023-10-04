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
    public class EnumConstantDataProvider : IEnumConstantDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<EnumConstantDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public EnumConstantDataProvider(QcsProductContext context, ILogger<EnumConstantDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<EnumConstantViewModel>> List(string search, string keyGroup)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = (from ec in _context.EnumConstant
                               where ec.RowStatus == null &&
                               (EF.Functions.Like(ec.Name.ToLower(), "%" + filter + "%"))
                          select new EnumConstantViewModel
                               {
                                   Id = ec.Id,
                                   TypeId = ec.TypeId,
                                   KeyGroup = ec.KeyGroup,
                                   KeyValueLabel = ec.keyValueLabel,
                                   Name = ec.Name
                               }).AsQueryable();

            var resultData = new List<EnumConstantViewModel>();

            if (keyGroup != null)
            {
                resultData = await result.Where(x => (x.KeyGroup == keyGroup)).ToListAsync();
            }
            else
            {
                resultData = await result.ToListAsync();
            }

            return resultData;
        }


    }
}
