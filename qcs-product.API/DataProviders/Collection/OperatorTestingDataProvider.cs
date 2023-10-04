using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.Helpers;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Auth.Authorization.Infrastructure;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;


namespace qcs_product.API.DataProviders.Collection
{
    public class OperatorTestingDataProvider : IOperatorTestingDataProvider
    {
        private readonly QcsProductContext _context;
        //private readonly q100_authorizationContext _authContext;
        private readonly ILogger<ReviewKasieDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public OperatorTestingDataProvider(
            QcsProductContext context,
            //q100_authorizationContext authContext,
            ILogger<ReviewKasieDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_authContext = authContext;
            _logger = logger;
        }

        public async Task<List<TransactionTestingPersonnel>> DeleteNotInRangePersonel(int testingId, List<TransactionTestingPersonnel> latestPersonnelData)
        {
            var listTestingId = new List<int?>();
            var listTestingPersonelNewNik = new List<string>();
            foreach (var testingPersonel in latestPersonnelData)
            {
                listTestingId.Add(testingPersonel.TestingId);
                listTestingPersonelNewNik.Add(testingPersonel.NewNIK);
            }

            var listTestingPersonel = await (
                from thtp in _context.TransactionTestingPersonnel
                where thtp.TestingId == testingId
                && !listTestingPersonelNewNik.Contains(thtp.NewNIK)
                select thtp
            ).ToListAsync();

            _context.TransactionTestingPersonnel.RemoveRange(listTestingPersonel);
            await _context.SaveChangesAsync();
            return listTestingPersonel;
        }

        public async Task<List<TransactionTestingAttachment>> GetAttachmentByTestingId(int testingId)
        {
            return await (from x in _context.TransactionTestingAttachment
                          where x.TestingId == testingId
                          select x).ToListAsync();
        }

        public async Task<List<TransactionHtrTestingPersonnel>> GetHtrPersonelByTestingId(int testingId)
        {
            return await (
                from thtp in _context.TransactionHtrTestingPersonnel
                where (thtp.TestingId == testingId)
                orderby thtp.CreatedAt descending
                select thtp
            ).ToListAsync();
        }

        public async Task<List<TransactionTestingNote>> GetNoteByTestingId(int testingId)
        {
            return await (from x in _context.TransactionTestingNote
                          where x.TestingId == testingId
                          orderby x.CreatedAt descending
                          select x).ToListAsync();
        }

        public async Task<TransactionTestingPersonnel> GetPersonelByNewNIK(int testingId, string newNik)
        {
            return await (
                from data in _context.TransactionTestingPersonnel
                where data.NewNIK == newNik
                && data.TestingId == testingId
                select data
            ).FirstOrDefaultAsync();
        }

        public async Task<List<TransactionTestingPersonnel>> GetPersonnelByTestingId(int testingId, string filter)
        {
            return await (from x in _context.TransactionTestingPersonnel
                          where x.TestingId == testingId || filter != null ? x.Nama.ToLower().Contains(filter) : true
                          select x).ToListAsync();
        }

        public async Task<TransactionTesting> GetTrxTestingById(int testingId)
        {
            return await
            (
                from data in _context.TransactionTesting
                where data.Id == testingId
                select data
            ).FirstOrDefaultAsync();
        }

        public async Task<GeneralOperatorTestingInfoViewModel> InfoGeneralByTestingId(int testingId)
        {
            return await
            (
                from data in _context.TransactionTesting.AsNoTracking()
                where (data.Id == testingId)
                select new GeneralOperatorTestingInfoViewModel
                {
                    listAttachment = (
                        from attc in _context.TransactionTestingAttachment
                        where attc.TestingId == data.Id
                        orderby attc.CreatedAt descending
                        select attc
                    ).ToList(),
                    listHtrAttachment = (
                        from attc_htr in _context.TransactionHtrTestingAttachment
                        where attc_htr.TestingId == data.Id
                        orderby attc_htr.CreatedAt descending
                        select attc_htr
                    ).ToList(),
                    listNote = (
                        from note in _context.TransactionTestingNote
                        where note.TestingId == data.Id
                        orderby note.CreatedAt descending
                        select note
                    ).ToList(),
                    listHtrNote = (
                        from note_htr in _context.TransactionHtrTestingNote
                        where note_htr.TestingId == data.Id
                        orderby note_htr.CreatedAt descending
                        select note_htr
                    ).ToList(),
                    listPersonnel = (
                        from personel in _context.TransactionTestingPersonnel
                        where personel.TestingId == data.Id
                        orderby personel.CreatedAt descending
                        select personel
                    ).ToList(),
                    listHtrPersonnel = (
                        from personel_htr in _context.TransactionHtrTestingPersonnel
                        where personel_htr.TestingId == data.Id
                        orderby personel_htr.CreatedAt descending
                        select personel_htr
                    ).ToList()
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<TransactionHtrTestingPersonnel> InsertHtrPersonel(TransactionHtrTestingPersonnel personnelHistoryData)
        {
            await _context.TransactionHtrTestingPersonnel.AddAsync(personnelHistoryData);
            await _context.SaveChangesAsync();
            return personnelHistoryData;
        }

        public async Task<TransactionTestingNote> InsertNote(TransactionTestingNote data)
        {
            var nowTimestamp = _context.NowTimestamp.FromSqlRaw(ApplicationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            data.CreatedAt = nowTimestamp.CurrentTimestamp;
            data.UpdatedAt = nowTimestamp.CurrentTimestamp;
            await _context.TransactionTestingNote.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<TransactionHtrTestingNote> InsertHtrNote(TransactionHtrTestingNote data)
        {
            var nowTimestamp = _context.NowTimestamp.FromSqlRaw(ApplicationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            data.CreatedAt = nowTimestamp.CurrentTimestamp;
            data.UpdatedAt = nowTimestamp.CurrentTimestamp;
            await _context.TransactionHtrTestingNote.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<TransactionTestingPersonnel> InsertPersonel(TransactionTestingPersonnel productionProcessData)
        {
            var nowTimestamp = _context.NowTimestamp.FromSqlRaw(ApplicationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            productionProcessData.CreatedAt = nowTimestamp.CurrentTimestamp;
            productionProcessData.UpdatedAt = nowTimestamp.CurrentTimestamp;
            await _context.TransactionTestingPersonnel.AddAsync(productionProcessData);
            await _context.SaveChangesAsync();
            return productionProcessData;
        }

        public async Task<TransactionTesting> SetEndDate(int testingId)
        {
            TransactionTesting result = new TransactionTesting();
            var testing = _context.TransactionTesting.FirstOrDefault(x => x.Id == testingId);

            if (testing != null)
            {
                testing.TestingEndtDate = DateTime.UtcNow.AddHours(7);
                if (testing.RowStatus != null)
                {
                    testing.ObjectStatus = ApplicationConstant.STATUS_TEST_INREVIEW_PAIRING;
                }
                result = testing;
            }

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<TransactionTesting> SetStartDate(int testingId)
        {
            TransactionTesting result = new TransactionTesting();
            var testing = _context.TransactionTesting.FirstOrDefault(x => x.Id == testingId);

            if (testing != null)
            {
                testing.TestingStartDate = DateTime.UtcNow.AddHours(7);
                if (testing.RowStatus != null)
                {
                    testing.ObjectStatus = ApplicationConstant.STATUS_TEST_INPROGRESS;
                }
                result = testing;
            }

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<TransactionTestingPersonnel> UpdatePersonel(TransactionTestingPersonnel currentProcessPersonel)
        {
            var nowTimestamp = _context.NowTimestamp.FromSqlRaw(ApplicationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            currentProcessPersonel.UpdatedAt = nowTimestamp.CurrentTimestamp;
            _context.TransactionTestingPersonnel.Update(currentProcessPersonel);
            await _context.SaveChangesAsync();
            return currentProcessPersonel;
        }

        public async Task<List<TransactionHtrTestingAttachment>> GetHtrAttachmentByTestingId(int testingId)
        {
            return await(from x in _context.TransactionHtrTestingAttachment
                         where x.TestingId == testingId
                         orderby x.CreatedAt descending
                         select x).ToListAsync();
        }

        public async Task<TransactionTestingAttachment> InsertAttachment(TransactionTestingAttachment data)
        {
            var nowTimestamp = _context.NowTimestamp.FromSqlRaw(ApplicationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            data.CreatedAt = nowTimestamp.CurrentTimestamp;
            data.UpdatedAt = nowTimestamp.CurrentTimestamp;
            await _context.TransactionTestingAttachment.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<TransactionTestingAttachment> GetAttachmentByMediaLink(string attachmentStorageName)
        {
            return await(
                from data in _context.TransactionTestingAttachment
                where data.MediaLink == attachmentStorageName
                select data
            ).FirstOrDefaultAsync();
        }

        public async Task<List<TransactionTestingAttachment>> DeleteNotInRangeAttachment(int testingId, List<TransactionTestingAttachment> data)
        {
            var listTestingId = new List<int?>();
            var listTestingAttachmentMediaLink = new List<string>();
            foreach (var testingAttachment in data)
            {
                listTestingId.Add(testingAttachment.TestingId);
                listTestingAttachmentMediaLink.Add(testingAttachment.MediaLink);
            }

            var listtestingAttachment = await(
                from ppa in _context.TransactionTestingAttachment
                where ppa.TestingId == testingId
                && !listTestingAttachmentMediaLink.Contains(ppa.MediaLink)
                select ppa
            ).ToListAsync();

            _context.TransactionTestingAttachment.RemoveRange(listtestingAttachment);
            await _context.SaveChangesAsync();
            return listtestingAttachment;
        }

        public async Task<TransactionHtrTestingAttachment> InsertHtrAttachment(TransactionHtrTestingAttachment data)
        {
            await _context.TransactionHtrTestingAttachment.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<bool> Authenticate(string serialNumber, string nik)
        {
            DateTime now = DateTime.UtcNow;
            DateTime nowTimestamp = now.AddHours(ApplicationConstant.TIMEZONE);
            var user = await
            (
                from data in _context.digitalSigantures
                where data.Nik == nik
                select data
            ).FirstOrDefaultAsync();
            return true;
            if (user == null || !BC.Verify(serialNumber, user.SerialNumber))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<TransactionTestingPersonnel> GetCheckedInUserPresenceByUsername(string nik)
        {
            return await
            (
                from data in _context.TransactionTestingPersonnel
                where
                    data.Nik == nik &&
                    data.RowStatus == null &&
                    data.CheckOut == null
                select data
            ).FirstOrDefaultAsync();
        }

        public async Task<TransactionHtrTestingPersonnel> InsertHtrPresence(TransactionHtrTestingPersonnel user)
        {
            await _context.TransactionHtrTestingPersonnel.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<TransactionTestingPersonnel> CheckedInUserPresence(CheckInOutBindingModel data)
        {
            TransactionTestingPersonnel result = new TransactionTestingPersonnel();
            var testing = _context.TransactionTestingPersonnel.FirstOrDefault(x => x.Id == data.Id);

            if (testing != null)
            {
                if (testing.CheckIn == null)
                {
                    testing.CheckIn = DateTime.UtcNow.AddHours(ApplicationConstant.TIMEZONE);
                    testing.UpdatedAt = DateTime.UtcNow.AddHours(ApplicationConstant.TIMEZONE);
                    testing.UpdatedBy = data.Nama;
                }
                result = testing;
            }

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<TransactionTestingPersonnel> CheckedOutUserPresence(TransactionTestingPersonnel existingData)
        {
            _context.TransactionTestingPersonnel.Update(existingData);
            await _context.SaveChangesAsync();
            return existingData;
        }
    }

}