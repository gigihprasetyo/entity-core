using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Infrastructure;
using qcs_product.API.ViewModels;
using qcs_product.API.Models;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using qcs_product.Constants;

namespace qcs_product.API.DataProviders.Collection
{

    public class RoomPurposeToMasterPurposeDataProvider : IRoomPurposeToMasterPurposeDataProvider
    {
        private readonly QcsProductContext _context;

        [ExcludeFromCodeCoverage]
        public RoomPurposeToMasterPurposeDataProvider(QcsProductContext context)
        {
            _context = context;
        }

        public async Task<RoomPurposeToMasterPurpose> Insert(RoomPurposeToMasterPurpose insert)
        {
            await _context.RoomPurposeToMasterPurposes.AddAsync(insert);
            await _context.SaveChangesAsync();
            return insert;
        }

        public async Task<List<RoomPurposeToMasterPurpose>> ListRoomPurposeToMasterPurposeByRoomId(int roomPurposeId)
        {
            return await _context.RoomPurposeToMasterPurposes.Where(x => x.RoomPurposeId == roomPurposeId).ToListAsync();
        }


        public List<RoomPurposeToMasterPurpose> RemoveRange(List<RoomPurposeToMasterPurpose> data)
        {
            _context.RoomPurposeToMasterPurposes.RemoveRange(data);
            return data;
        }

    }
}
