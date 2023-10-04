using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IMicrofloraDataProvider
    {
        public Task<List<ShortDataListViewModel>> List(string search);
        public Task<Microflora> GetByCode(string code);
        public Task<Microflora> Insert(Microflora insert);
        public Task<Microflora> Update(Microflora microflora);
    }
}
