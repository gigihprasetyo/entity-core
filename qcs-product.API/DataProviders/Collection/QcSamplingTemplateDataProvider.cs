using Google.Api;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class QcSamplingTemplateDataProvider : IQcSamplingTemplateDataProvider
    {
        private readonly QcsProductContext _context;

        [ExcludeFromCodeCoverage]
        public QcSamplingTemplateDataProvider(QcsProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<QcSamplingTemplateViewModel>> GetAll(string filter, List<int> status, int page, int limit)
        {
            filter = string.IsNullOrEmpty(filter) ? string.Empty : filter.ToLower();

            var query = (from qst in _context.QcSamplingTemplate
                         join tt in _context.TestTypes on qst.TestTypeId equals tt.Id
                         where qst.name.ToLower().Contains(filter)
                         && status.Contains(qst.Status)
                         orderby qst.UpdatedAt descending
                         select new QcSamplingTemplateViewModel
                         {
                             Id = qst.Id,
                             CreatedAt = qst.CreatedAt,
                             UpdatedAt = qst.UpdatedAt,
                             CreatedBy = qst.CreatedBy,
                             MethodId = qst.MethodId,
                             MethodName = "",
                             name = qst.name,
                             Status = qst.Status,
                             TestTypeId = qst.TestTypeId,
                             TestTypeName = tt.Name,
                             UpdatedBy = qst.UpdatedBy,
                             ValidityPeriodEnd = qst.ValidityPeriodEnd,
                             ValidityPeriodStart = qst.ValidityPeriodStart
                         }).AsQueryable();

            if(limit > 0)
            {
                query = query.Skip(page).Take(limit);
            }

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<QcSamplingTemplate> Insert(QcSamplingTemplate qcSamplingTemplate)
        {
            await _context.QcSamplingTemplate.AddAsync(qcSamplingTemplate);
            await _context.SaveChangesAsync();
            return qcSamplingTemplate;
        }
    }
}
