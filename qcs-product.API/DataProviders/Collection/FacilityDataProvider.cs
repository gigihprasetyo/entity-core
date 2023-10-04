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
    public class FacilityDataProvider : IFacilityDataProvider
    {
        private readonly QcsProductContext _context;

        public FacilityDataProvider(QcsProductContext context)
        {
            _context = context;
        }

        public async Task<Facility> GetById(int id)
        {
            return await _context.Facilities.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Facility> GetByCode(string code)
        {
            return await _context.Facilities.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Facility> Insert(Facility facility)
        {
            await _context.AddAsync(facility);
            await _context.SaveChangesAsync();
            return facility;
        }

        public async Task<Facility> Update(Facility facility)
        {
            _context.Facilities.Update(facility);
            await _context.SaveChangesAsync();
            return facility;
        }

        public async Task<List<FacilityAHUViewModel>> getFacilityAHUList(string search)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from fc in _context.Facilities
                                join o in _context.Organizations on fc.OrganizationId equals o.Id
                                where ((EF.Functions.Like(fc.Code.ToLower(), "%" + filter + "%")) ||
                                  (EF.Functions.Like(fc.Name.ToLower(), "%" + filter + "%")) ||
                                  (EF.Functions.Like(o.Name.ToLower(), "%" + filter + "%")) ||
                                  (EF.Functions.Like(o.BIOHROrganizationId.ToString().ToLower(), "%" + filter + "%")) ||
                                  (EF.Functions.Like(o.OrgCode.ToLower(), "%" + filter + "%")))
                                && fc.RowStatus == null && fc.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                select new FacilityAHUViewModel
                                {
                                    Id = fc.Id,
                                    FacilityCode = fc.Code,
                                    FacilityName = fc.Name,
                                    OrganizationId = fc.OrganizationId,
                                    OrganizationCode = o.OrgCode,
                                    OrganizationName = o.Name,
                                    BIOHROrganizationId = o.BIOHROrganizationId,
                                    ToolsAHU = (from t in _context.Tools
                                                join tg in _context.ToolGroups on t.ToolGroupId equals tg.Id
                                                where tg.Code == ApplicationConstant.TOOL_GROUP_CODE_AHU
                                                && t.FacilityId == fc.Id
                                                && t.RowStatus == null
                                                select new ToolsAHUViewModel
                                                {
                                                    ToolId = t.Id,
                                                    ToolCode = t.ToolCode,
                                                    ToolName = t.Name
                                                }).ToList()
                                }).ToListAsync();


            return result;
        }

    }
}