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
    public class UserTestingDataProvider : IUserTestingDataProvider
    {
        private readonly QcsProductContext _context;

        [ExcludeFromCodeCoverage]
        public UserTestingDataProvider(QcsProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserTesting> Insert(UserTesting data)
        {
            

            await _context.UserTestings.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }
    }
}
