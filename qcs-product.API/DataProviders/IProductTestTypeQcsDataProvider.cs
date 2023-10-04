using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface IProductTestTypeQcsDataProvider
    {
        public Task<TestTypeQcs> Insert(TestTypeQcs data);
    }
}
