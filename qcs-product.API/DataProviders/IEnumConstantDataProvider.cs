using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IEnumConstantDataProvider
    {
        public Task<List<EnumConstantViewModel>> List(string search, string keyGroup);
    }
}
