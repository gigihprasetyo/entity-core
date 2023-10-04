using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders
{
    public interface IPersonelDataProvider
    {
        public Task<List<QcPersonelViewModel>> List(string search);
    }
}
