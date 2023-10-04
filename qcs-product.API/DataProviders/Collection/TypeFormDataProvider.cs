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
    public class TypeFormDataProvider : ITypeFormDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<TypeFormDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public TypeFormDataProvider(QcsProductContext context, ILogger<TypeFormDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<TypeFormViewModel>> List()
        {
            var result = await (from tf in _context.TypeForms
                                where tf.RowStatus == null
                                select new TypeFormViewModel
                                {
                                    Id = tf.Id,
                                    TypeFormCode = tf.TypeFormCode,
                                    Name = tf.Name
                                }).ToListAsync();

            return result;
        }
    }
}
