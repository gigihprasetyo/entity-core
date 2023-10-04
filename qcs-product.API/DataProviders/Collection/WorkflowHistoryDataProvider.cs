using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class WorkflowHistoryDataProvider : IWorkflowHistoryDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<WorkflowHistoryDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public WorkflowHistoryDataProvider(QcsProductContext context, ILogger<WorkflowHistoryDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<WorkflowHistory> Insert(WorkflowHistory data)
        {
            await _context.WorkflowHistories.AddAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }
    }
}
