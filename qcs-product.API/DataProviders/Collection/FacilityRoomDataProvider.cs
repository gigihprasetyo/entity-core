using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class FacilityRoomDataProvider : IFacilityRoomDataProvider
    {
        private readonly QcsProductContext _context;

        public FacilityRoomDataProvider(QcsProductContext context)
        {
            _context = context;
        }

        public async Task<RoomFacility> Insert(RoomFacility facilityRoom)
        {
            await _context.AddAsync(facilityRoom);
            await _context.SaveChangesAsync();
            return facilityRoom;
        }

        public async Task<RoomFacility> Update(RoomFacility facilityRoom)
        {
            _context.RoomFacilities.Update(facilityRoom);
            await _context.SaveChangesAsync();
            return facilityRoom;
        }

        public async Task<RoomFacility> GetByCode(int facilityId)
        {
            return await _context.RoomFacilities.FirstOrDefaultAsync(x => x.FacilityId == facilityId);
        }

        public async Task<List<RoomFacility>> GetByFacilityIdAndRoomId(int facilityId, int roomId)
        {
            return await (from rf in _context.RoomFacilities
                          where rf.FacilityId == facilityId && rf.RoomId == roomId
                          select rf).ToListAsync();
        }

        public async Task<List<RoomFacility>> GetByFacilityId(int facilityId)
        {
            return await (from rf in _context.RoomFacilities
                          where rf.FacilityId == facilityId
                          select rf).ToListAsync();
        }

    }
}