using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class AuthenticatedUserBiohrDataProviders : IAuthenticatedUserBiohrDataProviders
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<AuthenticatedUserBiohrDataProviders> _logger;

        [ExcludeFromCodeCoverage]
        public AuthenticatedUserBiohrDataProviders(QcsProductContext context, ILogger<AuthenticatedUserBiohrDataProviders> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<AuthenticatedUserBiohr> GetAuthenticatedTokenActived()
        {
            return await (from u in _context.AuthenticatedUserBiohrs
                          where u.RowStatus == null
                          select u).FirstOrDefaultAsync();
        }

        public async Task Delete(Int32 id)
        {
            var authUser = await (from u in _context.AuthenticatedUserBiohrs
                                  where u.Id == id
                                  select u).FirstOrDefaultAsync();
            authUser.UpdatedAt = DateTime.UtcNow.AddHours(7);
            authUser.RowStatus = "deleted";

            await _context.SaveChangesAsync();
        }

        public async Task Insert(string token)
        {
            AuthenticatedUserBiohr data = new AuthenticatedUserBiohr
            {
                Token = token,
                CreatedAt = DateTime.UtcNow.AddHours(7),
                UpdatedAt = DateTime.UtcNow.AddHours(7)
            };

            await _context.AuthenticatedUserBiohrs.AddAsync(data);
            await _context.SaveChangesAsync();
        }

    }
}
