using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface IQcProcessBusinessProvider
    {
        public Task<ResponseViewModel<QcProcessShortViewModel>> ListShort(string search, int limit, int page, int roomId, int? purposeId);
        public Task<ResponseViewModel<QcProcessViewModel>> List();
        public Task<ResponseOneDataViewModel<QcProcessViewModel>> GetById(Int32 id);
        public Task<ResponseViewModel<QcProcess>> Insert(InsertQcProcessBindingModel data);
        public Task<ResponseViewModel<QcProcess>> Delete(int qcProcessId);
    }
}
