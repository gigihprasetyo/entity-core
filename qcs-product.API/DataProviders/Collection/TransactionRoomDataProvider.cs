using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.Constants;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionRoomDataProvider : ITransactionRoomDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<RoomDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public TransactionRoomDataProvider(QcsProductContext context, ILogger<RoomDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<RoomDetailRelationViewModel> GetTransactionRoomRelationDetailById(int id)
        {
            var result = await (from r in _context.TransactionRoom
                                join gr in _context.TransactionGradeRoom on r.GradeRoomId equals gr.Id
                                join t in _context.TransactionTool on r.Ahu equals t.Id into ahu
                                from t in ahu.DefaultIfEmpty()
                                where r.Id == id
                                && r.RowStatus == null
                                select new RoomDetailRelationViewModel
                                {
                                    RoomId = r.Id,
                                    RoomCode = r.Code,
                                    RoomName = r.Name,
                                    GradeRoomId = r.GradeRoomId,
                                    GradeRoomCode = gr.Code,
                                    GradeRoomName = gr.Name,
                                    AhuId = r.Ahu,
                                    AhuCode = t.ToolCode,
                                    AhuName = t.Name
                                }).FirstOrDefaultAsync();

            return result;
        }


    }
}
