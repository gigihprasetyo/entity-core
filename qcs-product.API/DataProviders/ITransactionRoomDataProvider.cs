using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface ITransactionRoomDataProvider
    {
        public Task<RoomDetailRelationViewModel> GetTransactionRoomRelationDetailById(int id);
    }
}
