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
    public class PersonelDataProvider : IPersonelDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<PersonelDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public PersonelDataProvider(QcsProductContext context, ILogger<PersonelDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<QcPersonelViewModel>> List(string search)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from qcp in _context.QcPersonels
                                where ((EF.Functions.Like(qcp.Name.ToLower(), "%" + filter + "%")) ||
                                       (EF.Functions.Like(qcp.PersonelCode4.ToLower(), "%" + filter + "%")) ||
                                       (EF.Functions.Like(qcp.PersonelCode8.ToLower(), "%" + filter + "%")) ||
                                       (EF.Functions.Like(qcp.Initial.ToLower(), "%" + filter + "%")))
                                       && qcp.RowStatus == null
                                select new QcPersonelViewModel
                                {
                                    Id = qcp.Id,
                                    PersonelCode4 = qcp.PersonelCode4,
                                    PersonelCode8 = qcp.PersonelCode8,
                                    Name = qcp.Name,
                                    Initial = qcp.Initial
                                }).OrderBy(x => x.Name).ToListAsync();

            return result;

        }
    }
}
