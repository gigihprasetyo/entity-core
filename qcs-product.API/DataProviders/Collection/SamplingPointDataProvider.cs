using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders.Collection
{
    public class SamplingPointDataProvider : ISamplingPointDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<SamplingPointDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public SamplingPointDataProvider(QcsProductContext context, ILogger<SamplingPointDataProvider> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SamplingPoint> Insert(SamplingPoint samplingPoint)
        {
            try
            {
                await _context.SamplingPoints.AddAsync(samplingPoint);
                await _context.SaveChangesAsync();
                return samplingPoint;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

            return null;
        }

        public async Task<SamplingPoint> Update(SamplingPoint samplingPoint)
        {
            try
            {
                _context.SamplingPoints.Update(samplingPoint);
                await _context.SaveChangesAsync();
                return samplingPoint;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }

            return null;
        }

        public async Task<SamplingPoint> GetByCode(string samplingPointCode)
        {
            var result = await (from sp in _context.SamplingPoints
                                where sp.Code == samplingPointCode
                                select sp).FirstOrDefaultAsync();
            _logger.LogInformation($"Data result : {result}");
            return result;
        }

        public async Task<SamplingPoint> GetByCodeAndRoomCode(string code, string roomCode)
        {
            return await (from sp in _context.SamplingPoints
                          join rrs in _context.RelRoomSamplings on sp.Id equals rrs.SamplingPointId
                          join room_purp in _context.RoomPurpose on rrs.RoomPurposeId equals room_purp.Id
                          join r in _context.Rooms on room_purp.RoomId equals r.Id
                          where sp.Code == code && r.Code == roomCode
                          select sp).FirstOrDefaultAsync();
        }

        public async Task<SamplingPoint> GetByCodeToolCode(string samplingPointCode, string toolCode)
        {
            var result = await (from sp in _context.SamplingPoints
                                join t in _context.Tools on sp.ToolId equals t.Id
                                where sp.Code == samplingPointCode
                                && t.ToolCode == toolCode
                                select sp
            ).FirstOrDefaultAsync();
            _logger.LogInformation($"Data result : {result}");
            return result;
        }

        public async Task<RelSamplingTestParam> InsertRelSamplingParam(RelSamplingTestParam relSamplingTestParam)
        {
            await _context.RelSamplingTestParams.AddAsync(relSamplingTestParam);
            await _context.SaveChangesAsync();
            return relSamplingTestParam;
        }

    }
}
