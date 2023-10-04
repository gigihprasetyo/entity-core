using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using qcs_product.API.ViewModels;
using qcs_product.API.BusinessProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionBatchDataProvider : ITransactionBatchDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<RoomDataProvider> _logger;
        private readonly IUploadFilesBusinessProvider _uploadFilesBusinessProvider;

        [ExcludeFromCodeCoverage]
        public TransactionBatchDataProvider(QcsProductContext context, ILogger<RoomDataProvider> logger,
        IUploadFilesBusinessProvider uploadFilesBusinessProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
            _uploadFilesBusinessProvider = uploadFilesBusinessProvider;
        }

        public async Task<TransactionBatchViewModel> GetById(int id)
        {
            var result = await (from b in _context.TransactionBatches
                                where b.Id == id
                                select new TransactionBatchViewModel
                                {
                                    Id = b.Id,
                                    AttachmentNotes = b.AttachmentNotes,
                                    RequestId = b.RequestQcsId
                                }).FirstOrDefaultAsync();

            if (result == null) return null;

            var lines = await (from l in _context.TransactionBatchLines
                               where l.TrsBatchId == id
                               select new TransactionBatchLineViewModel
                               {
                                   Id = l.Id,
                                   ItemId = l.ItemId,
                                   ItemName = l.ItemName,
                                   NoBatch = l.NoBatch,
                                   Notes = l.Notes,
                                   TrsBatchId = id
                               }).ToListAsync();

            var attachments = await (from a in _context.TransactionBatchAttachments
                                     where a.TrsBatchId == id
                                     select new TransactionBatchAttachmentViewModel
                                     {
                                         Id = a.Id,
                                         Title = a.Title,
                                         AttachmentStorageName = a.AttachmentStorageName,
                                         AttachmentFile = a.AttachmentFile,
                                         FileName = a.FileName,
                                         TrsBatchId = id
                                     }).ToListAsync();

            result.Lines = lines;
            result.Attachments = attachments;

            return result;
        }

        public async Task<TransactionBatchViewModel> GetByRequestId(int requestId)
        {
            var result = await (from b in _context.TransactionBatches
                                where b.RequestQcsId == requestId
                                select new TransactionBatchViewModel
                                {
                                    Id = b.Id,
                                    AttachmentNotes = b.AttachmentNotes,
                                    RequestId = b.RequestQcsId
                                }).FirstOrDefaultAsync();

            if (result == null) return null;

            var lines = await (from l in _context.TransactionBatchLines
                               where l.TrsBatchId == result.Id
                               select new TransactionBatchLineViewModel
                               {
                                   Id = l.Id,
                                   ItemId = l.ItemId,
                                   ItemName = l.ItemName,
                                   NoBatch = l.NoBatch,
                                   Notes = l.Notes,
                                   TrsBatchId = l.TrsBatchId
                               }).ToListAsync();

            var attachments = await (from a in _context.TransactionBatchAttachments
                                     where a.TrsBatchId == result.Id
                                     select new TransactionBatchAttachmentViewModel
                                     {
                                         Id = a.Id,
                                         Title = a.Title,
                                         AttachmentFile = a.AttachmentFile,
                                         AttachmentStorageName = a.AttachmentStorageName,
                                         FileName = a.FileName,
                                         TrsBatchId = a.TrsBatchId
                                     }).ToListAsync();

            foreach (var item in attachments)
            {
                item.AttachmentFile = await _uploadFilesBusinessProvider.GenerateV4SignedReadUrl(item.AttachmentStorageName);
            }


            result.Lines = lines;
            result.Attachments = attachments;

            return result;
        }

        public async Task<List<TransactionBatchViewModel>> GetByRequestIds(List<int> requestIds)
        {
            var result = await (from b in _context.TransactionBatches
                                where requestIds.Contains(b.RequestQcsId)
                                select new TransactionBatchViewModel
                                {
                                    Id = b.Id,
                                    AttachmentNotes = b.AttachmentNotes,
                                    RequestId = b.RequestQcsId
                                }).ToListAsync();

            if (result == null) return null;

            var batchIds = result.Select(x => x.Id).Distinct().ToList();

            var lines = await (from l in _context.TransactionBatchLines
                               where batchIds.Contains(l.TrsBatchId)
                               select new TransactionBatchLineViewModel
                               {
                                   Id = l.Id,
                                   ItemId = l.ItemId,
                                   ItemName = l.ItemName,
                                   NoBatch = l.NoBatch,
                                   Notes = l.Notes,
                                   TrsBatchId = l.TrsBatchId
                               }).ToListAsync();

            var attachments = await (from a in _context.TransactionBatchAttachments
                                     where batchIds.Contains(a.TrsBatchId)
                                     select new TransactionBatchAttachmentViewModel
                                     {
                                         Id = a.Id,
                                         Title = a.Title,
                                         AttachmentFile = a.AttachmentFile,
                                         AttachmentStorageName = a.AttachmentStorageName,
                                         FileName = a.FileName,
                                         TrsBatchId = a.TrsBatchId
                                     }).ToListAsync();

            foreach (var item in attachments)
            {
                item.AttachmentFile = await _uploadFilesBusinessProvider.GenerateV4SignedReadUrl(item.AttachmentStorageName);
            }

            foreach (var item in result)
            {
                item.Lines = lines.Where(x => x.TrsBatchId == item.Id).ToList();
                item.Attachments = attachments.Where(x => x.TrsBatchId == item.Id).ToList();
            }

            return result;
        }
    }
}
