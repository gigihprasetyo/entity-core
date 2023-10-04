using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class OrganizationDataProvider : IOrganizationDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<OrganizationDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public OrganizationDataProvider(QcsProductContext context, ILogger<OrganizationDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<OrganizationViewModel>> List()
        {
            return await (
                from data in _context.Organizations
                where data.RowStatus == null
                orderby data.UpdatedAt descending
                select new OrganizationViewModel
                {
                    Id = data.Id,
                    BIOHROrganizationId = data.BIOHROrganizationId,
                    OrganizationId = data.OrgCode,
                    OrganizationName = data.Name,
                    StatusObject = ApplicationConstant.OBJECT_STATUS_ACTIVE_NAME,
                    Modul = ApplicationConstant.MODUL_NAME_ORGANIZATION,
                    CreatedAt = data.CreatedAt,
                    CreatedBy = data.CreatedBy,
                    UpdatedAt = data.UpdatedAt,
                    UpdatedBy = data.UpdatedBy
                }
            ).ToListAsync();
        }

        public async Task<Organization> GetDetailByCode(string code, bool includeDeleted = false)
        {
            var query = _context.Organizations.Where(x => x.OrgCode == code);
            if (!includeDeleted)
            {
                query = query.Where(x => x.RowStatus == null);
            }
            
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Organization> GetDetailById(int id)
        {
            var result = await (from p in _context.Organizations
                                where p.Id == id
                                && p.RowStatus == null
                                select p).FirstOrDefaultAsync();

            return result;
        }
        
        public async Task<Organization> Insert(Organization organization)
        {
            try
            {
                await _context.Organizations.AddAsync(organization);
                await _context.SaveChangesAsync();
                return organization;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
            return null;
        }
        
        public async Task Delete(int id)
        {
            var authUser = await (from u in _context.Organizations
                where u.Id == id
                select u).FirstOrDefaultAsync();
            authUser.UpdatedAt = DateTime.Now;
            authUser.RowStatus = "deleted";

            await _context.SaveChangesAsync();
        }

        public async Task<Organization> Update(Organization organization)
        {
            try
            {
                _context.Organizations.Update(organization);
                await _context.SaveChangesAsync();
                return organization;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

            return null;
        }

        public async Task<Organization> GetDetailByBIOHROrganizationIdAndOrgName(int? orgId, string orgName, bool includeDeleted = false)
        {
            var query = _context.Organizations.Where(x => x.BIOHROrganizationId == orgId);
            query = _context.Organizations.Where(x => x.Name == orgName);
            if (!includeDeleted)
            {
                query = query.Where(x => x.RowStatus == null);
            }
            
            return await query.FirstOrDefaultAsync();
        }
    }
}
