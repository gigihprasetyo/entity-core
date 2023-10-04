using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.ViewModels;

namespace qcs_product.API.BusinessProviders
{
    public interface IPurposeBusinessProvider
    {
        //public Task<ResponseViewModel<PurposeViewModel>> List(string search, int RequestTypeId);
        public Task<ResponseViewModel<PurposeViewModel>> List(string search);
    }
}
