using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IOrganizationDataProvider
    {
        public Task<List<OrganizationViewModel>> List();
        public Task<Organization> GetDetailById(int id);
        public Task<Organization> GetDetailByCode(string code, bool includeDeleted = false);
        
        public Task<Organization> Insert(Organization organization);
        
        public Task<Organization> Update(Organization organization);
        public Task<Organization> GetDetailByBIOHROrganizationIdAndOrgName(int? orgId, string orgName, bool includeDeleted = false);

    }
}