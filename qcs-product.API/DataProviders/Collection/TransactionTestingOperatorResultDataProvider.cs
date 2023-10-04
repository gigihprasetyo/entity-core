using qcs_product.API.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionTestingOperatorResultDataProvider : ITransactionTestingOperatorResultDataProvider
    {
        private readonly QcsProductContext _context;

        [ExcludeFromCodeCoverage]
        public TransactionTestingOperatorResultDataProvider(QcsProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<TransactionTestingOperatorResultView>> GetAll(string filter, string status, int testingId, int page, int limit)
        {
            filter = string.IsNullOrEmpty(filter) ? string.Empty : filter.ToLower();

            var query = (from proc in _context.TransactionTestingProcedureParameters
                          join res in _context.TransactionTestingTypeMethodResultparameter on proc.Id equals res.TransactionTestingProcedureParameterId                  
                         select new TransactionTestingOperatorResultView
                         {
                             Id = res.Id,
                             InputTypeId = res.InputTypeId,
                             ParameterIdExisting = res.ParameterIdExisting,
                             Property = proc.Properties,
                             TestingId = res.TestingId,
                             Existing = res.Existing,
                             IsExisting = res.IsExisting,
                             MethodCode = res.MethodCode,
                             Name = res.Name,
                             NeedAttachment = res.NeedAttachment,
                             Properties = res.Properties,
                             PropertiesValue = res.PropertiesValue,
                             Sequence = res.Sequence,
                             TransactionTestingProcedureParameterId = res.TransactionTestingProcedureParameterId
                         }).AsQueryable();
            if(testingId > 0)
                query = query.Where(x => x.TestingId == testingId);

            if (limit > 0)
            {
                query = query.Skip(page).Take(limit);
            }

            var result = await query.ToListAsync();

            return result;
        }
    }
}
