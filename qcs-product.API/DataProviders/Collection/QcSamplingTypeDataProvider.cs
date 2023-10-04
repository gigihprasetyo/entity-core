using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.API.WorkflowModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Helpers;

namespace qcs_product.API.DataProviders.Collection
{
    public class QcSamplingTypeDataProvider : IQcSamplingTypeDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<QcSamplingTypeDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public QcSamplingTypeDataProvider(QcsProductContext context, ILogger<QcSamplingTypeDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<QcSamplingTypeViewModel>> List()
        {
            return await (
                from s in _context.QcSamplingTypes
                select new QcSamplingTypeViewModel
                {
                    Id = s.Id,
                    Code = s.Code,
                    Name = s.Name,
                    RowStatus = s.RowStatus,
                    CreatedBy = s.CreatedBy,
                    CreatedAt = s.CreatedAt,
                    UpdatedBy = s.UpdatedBy,
                    UpdatedAt = s.UpdatedAt
                }
            ).OrderBy(x => x.Name).ToListAsync();
        }
    }
}