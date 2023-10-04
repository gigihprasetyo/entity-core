using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.BusinessProviders;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace qcs_product.API.DataProviders.Collection
{
    public class TemplateTestingInfoDataProvider : ITemplateTestingInfoDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<TemplateTestingInfoDataProvider> _logger;
        public TemplateTestingInfoDataProvider(QcsProductContext context, ILogger<TemplateTestingInfoDataProvider> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<TemplateTestingPersonnel> CheckInCheckOut(InsertCheckInCheckOutPersonnel data)
        {
            TemplateTestingPersonnel dataPersonnel = _context.TemplateTestingPersonnel.FirstOrDefault(x => x.Id == data.TemplateTestingPersonnelId);

            if (dataPersonnel != null)
            {
                if (data.Type == "checkin")
                {
                    dataPersonnel.CheckIn = DateTime.UtcNow.AddHours(7);
                }
                else if (data.Type == "checkout")
                {
                    dataPersonnel.CheckOut = DateTime.UtcNow.AddHours(7);
                }

                dataPersonnel.UpdatedBy = data.UpdatedBy;
                dataPersonnel.UpdatedAt = DateTime.UtcNow.AddHours(7);


                await _context.SaveChangesAsync();


            }

            return dataPersonnel;
        }

        public List<TemplateTestingAttachment> DeleteAttachment(List<int> listId)
        {
            try
            {
                List<TemplateTestingAttachment> getData = (from x in _context.TemplateTestingAttachment
                                                                      where listId.Contains(x.Id)
                                                                      select x).ToList();

                _context.TemplateTestingAttachment.RemoveRange(getData);
                _context.SaveChanges();
                return getData;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
            return null;
        }

        public async Task<TemplateTestingAttachment> GetAttachmentById(int templateTestingAttachmentId)
        {
            return await(from x in _context.TemplateTestingAttachment
                         where x.Id == templateTestingAttachmentId
                         select x).FirstOrDefaultAsync();
        }

        public async Task<List<TemplateTestingAttachment>> GetAttachmentByTemplateTestingId(int templateTestingId)
        {
            return await (from x in _context.TemplateTestingAttachment
                          where x.TemplateTestingId == templateTestingId
                          select x).ToListAsync();
        }

        public async Task<List<TemplateTestingNote>> GetNoteByTemplateTestingId(int templateTestingId)
        {
            return await (from x in _context.TemplateTestingNote
                          where x.TemplateTestingId == templateTestingId
                          select x).ToListAsync();
        }

        public async Task<List<TemplateTestingPersonnel>> GetPersonnelByTemplateTestingId(int templateTestingId)
        {
            return await (from x in _context.TemplateTestingPersonnel
                          where x.TemplateTestingId == templateTestingId
                          select x).ToListAsync();
        }

        public async Task<List<TemplateTestingAttachment>> InsertAttachment(List<TemplateTestingAttachment> listAttachment)
        {
            try
            {
                await _context.TemplateTestingAttachment.AddRangeAsync(listAttachment);
                await _context.SaveChangesAsync();
                return listAttachment;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
            return null;
        }

        public async Task<TemplateTestingNote> InsertNote(TemplateTestingNote listNote)
        {
            try
            {
                await _context.TemplateTestingNote.AddAsync(listNote);
                await _context.SaveChangesAsync();
                return listNote;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
            return null;
        }

        public async Task<List<TemplateTestingPersonnel>> InsertPersonnel(List<TemplateTestingPersonnel> listPersonnel)
        {
            try
            {
                await _context.TemplateTestingPersonnel.AddRangeAsync(listPersonnel);
                await _context.SaveChangesAsync();
                return listPersonnel;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
            return null;
        }
    }
}
