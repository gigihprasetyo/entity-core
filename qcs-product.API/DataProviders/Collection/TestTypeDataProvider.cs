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
    public class TestTypeDataProvider : ITestTypeDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<TestTypeDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public TestTypeDataProvider(QcsProductContext context, ILogger<TestTypeDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<TestTypeViewModel>> List()
        {
            var result = await (from tt in _context.TestTypes
                                where tt.RowStatus == null
                                select new TestTypeViewModel
                                {
                                    Id = tt.Id,
                                    OrgId = tt.OrgId,
                                    OrgName = tt.OrgName,
                                    TestTypeCode = tt.TestTypeCode,
                                    Name = tt.Name
                                }).ToListAsync();

            return result;
        }
    }
}
