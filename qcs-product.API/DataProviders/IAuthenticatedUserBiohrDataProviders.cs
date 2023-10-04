using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders
{
    public interface IAuthenticatedUserBiohrDataProviders
    {
        public Task<AuthenticatedUserBiohr> GetAuthenticatedTokenActived();
        public Task Delete(Int32 id);
        public Task Insert(string token);
    }
}
