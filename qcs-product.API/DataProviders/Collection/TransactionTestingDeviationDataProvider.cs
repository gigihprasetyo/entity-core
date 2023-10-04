using qcs_product.API.Infrastructure;
using qcs_product.API.ViewModels;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionTestingDeviationDataProvider : ITransactionTestingDeviationDataProvider
    {
        private readonly QcsProductContext _context;

        [ExcludeFromCodeCoverage]
        public TransactionTestingDeviationDataProvider(QcsProductContext context, QualityAssuranceSystemServiceContext qascontext)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<TransactionTestingDeviationViewModel>> GetAll(string filter, int sampleId, int productId, string batch, string testTypeName, int page, int limit)
        {
            filter = string.IsNullOrEmpty(filter) ? string.Empty : filter.ToLower();

            var query = (from ppa in _context.TransactionTestingProcedureParameterAttachments
                        join pp in _context.TransactionTestingProcedureParameters on ppa.TransactionTestingProcedureParameterId equals pp.Id
                        where ppa.Type == "DEVIASI"
                         select new TransactionTestingDeviationViewModel
                         {
                             Id = pp.Id,
                             Batch = batch,
                             ProductId = productId,
                             ReportDate = DateTime.Now,
                             SampleId = ppa.TransactionTestingSamplingId,
                             TestTypeId = 1,
                             TestTypeName = testTypeName,
                         }).AsQueryable().GroupBy(x => x.SampleId).Select(dt => dt.FirstOrDefault());

            

            if (limit > 0)
            {
                query = query.Skip(page).Take(limit);
            }

            var result = await query.ToListAsync();

            return result;
        }
    }
}
