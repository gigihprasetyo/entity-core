using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface IPurposeDataProvider
    {
        public Task<List<PurposeViewModel>> List(string search);
        public Task<Purpose> GetById(int id);
        public Task<Purpose> GetByCode(string code);
        public Task<Purpose> Insert(Purpose purpose);
        public Task<Purpose> Update(Purpose purpose);
        //public Task<List<PurposeViewModel>> List(string search, int RequestTypeId);
    }
}
