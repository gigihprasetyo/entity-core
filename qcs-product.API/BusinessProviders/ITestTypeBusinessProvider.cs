using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ITestTypeBusinessProvider
    {
        public Task<ResponseViewModel<TestTypeViewModel>> List();
    }
}
