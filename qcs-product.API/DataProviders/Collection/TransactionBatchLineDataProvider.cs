using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class TransactionBatchLineDataProvider : ITransactionBatchLineDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<RoomDataProvider> _logger;
        private readonly IMapper _mapper;

        [ExcludeFromCodeCoverage]
        public TransactionBatchLineDataProvider(QcsProductContext context, ILogger<RoomDataProvider> logger, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<TransactionBatchLineViewModel>> List()
        {
            var result = await (from b in _context.TransactionBatchLines
                select b)
                .ProjectTo<TransactionBatchLineViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return result;
        }
    }
}
