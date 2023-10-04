using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class RoomSamplingPointLayoutDataProvider : IRoomSamplingPointLayoutDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<RoomSamplingPointLayoutDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public RoomSamplingPointLayoutDataProvider(QcsProductContext context, ILogger<RoomSamplingPointLayoutDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<RoomSamplingPointLayout> Insert(RoomSamplingPointLayout roomSamplingPointLayout)
        {
            try
            {
                await _context.RoomSamplingPointLayout.AddAsync(roomSamplingPointLayout);
                await _context.SaveChangesAsync();
                return roomSamplingPointLayout;

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return null;
        }

        public async Task<List<RoomSamplingPointLayout>> GetByRoomCode(string roomCode)
        {
            return await (from rspl in _context.RoomSamplingPointLayout
                          join room_purp in _context.RoomPurpose on rspl.RoomPurposeId equals room_purp.Id
                          join r in _context.Rooms on room_purp.RoomId equals r.Id
                          where r.Code == roomCode && rspl.RowStatus == null
                          select rspl).ToListAsync();
        }
    }
}
