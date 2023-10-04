using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.API.WorkflowModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IQcSamplingTypeBusinessProvider
    {
        public Task<ResponseViewModel<QcSamplingTypeViewModel>> List();
    }
}
