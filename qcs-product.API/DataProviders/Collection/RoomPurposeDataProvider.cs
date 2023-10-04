using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.Infrastructure;

namespace qcs_product.API.DataProviders.Collection
{

    public class RoomPurposeDataProvider : IRoomPurposeDataProvider
    {
        private readonly QcsProductContext _context;

        [ExcludeFromCodeCoverage]
        public RoomPurposeDataProvider(QcsProductContext context)
        {
            _context = context;
        }

        public async Task<RoomPurpose> InsertRoomPurpose(RoomPurpose insert)
        {
            await _context.RoomPurpose.AddAsync(insert);
            await _context.SaveChangesAsync();
            return insert;
        }

        public async Task<List<RoomPurpose>> ListRoomPurposeByRoomId(int roomId)
        {
            return await _context.RoomPurpose.Where(x => x.RoomId == roomId).ToListAsync();
        }

        public List<RoomPurpose> RemoveRange(string roomCode)
        {
            var getRoomCode = _context.Rooms.FirstOrDefault(x => x.Code == roomCode);
            var listRoomPurpose = _context.RoomPurpose.Where(x => x.RoomId == getRoomCode.Id).ToList();
            _context.RoomPurpose.RemoveRange(listRoomPurpose);
            return listRoomPurpose;
        }
    }
}
