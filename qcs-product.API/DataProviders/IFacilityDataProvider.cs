using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IFacilityDataProvider
    {
        Task<Facility> GetById(int id);

        Task<Facility> GetByCode(string code);

        Task<Facility> Insert(Facility facility);
        
        Task<Facility> Update(Facility facility);

        Task<List<FacilityAHUViewModel>> getFacilityAHUList(string search);
    }
}