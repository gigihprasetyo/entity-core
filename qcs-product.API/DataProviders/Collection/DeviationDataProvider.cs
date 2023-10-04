using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.ViewModels;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Text.Json;

namespace qcs_product.API.DataProviders.Collection
{
    public class DeviationDataProvider : IDeviationDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<DeviationDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public DeviationDataProvider(QcsProductContext context, ILogger<DeviationDataProvider> logger) 
        {
            _context = context;
            _logger = logger;

        }

        public async Task<List<ListDeviationViewModel>> ListDeviation(string search, int page, int limit)
        {
            // search = !string.IsNullOrEmpty(search) ? search.ToLower() : string.Empty;

            // var result = new List<ListDeviationViewModel>();

            //     var query = (from a in _context.TransactionTestingProcedureParameter
            //                 where a.IsDeviation == true &&
            //                 a.RowStatus != "deleted"
            //                 select a).AsQueryable();

            //    // var result = await query.Skip(page).Take(limit).ToListAsync();
            

            // return result;
            throw new NotImplementedException();
        }

        public async Task<DetailDeviationViewModel> DetailDeviation(int sampleId)
        {
            throw new NotImplementedException();
        }
    }
}
