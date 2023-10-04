using Microsoft.Extensions.Logging;
using qcs_product.API.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System;
using qcs_product.API.Models;
using Microsoft.EntityFrameworkCore;
using qcs_product.Constants;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using qcs_product.API.ViewModels;
using System.Collections.Generic;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionTestTypeDataProvider : ITransactionTestTypeDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<ItemDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public TransactionTestTypeDataProvider(QcsProductContext context, ILogger<ItemDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<TransactionTestingProcedureParameterAttachment>> DeleteNotInRange(int procedureParameterId, List<TransactionTestingProcedureParameterAttachment> data)
        {
            var listProcedureParameterId = new List<int?>();
            var listProcedureParameterAttachmentMediaLink = new List<string>();
            foreach (var procedureParameterAttachment in data)
            {
                listProcedureParameterId.Add(procedureParameterAttachment.TransactionTestingProcedureParameterId);
                listProcedureParameterAttachmentMediaLink.Add(procedureParameterAttachment.MediaLink);
            }

            var listProcedureParameterAttachment = await (
                from ppppa in _context.TransactionTestingProcedureParameterAttachments
                where ppppa.TransactionTestingProcedureParameterId == procedureParameterId
                && !listProcedureParameterAttachmentMediaLink.Contains(ppppa.MediaLink)
                select ppppa
            ).ToListAsync();

            _context.TransactionTestingProcedureParameterAttachments.RemoveRange(listProcedureParameterAttachment);
            await _context.SaveChangesAsync();
            return listProcedureParameterAttachment;
        }

        public async Task<TransactionTestingViewModel> GetTestingById(int testingId)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            return await (from testing in _context.TransactionTesting.AsNoTracking()
                          where testing.Id == testingId
                          select new TransactionTestingViewModel
                          {
                              Id = testing.Id,
                              Code = testing.Code,
                              TestingDate = testing.TestingDate,
                              ObjectStatus = testing.ObjectStatus,
                              CreatedBy = testing.CreatedBy,
                              CreatedAt = testing.CreatedAt,
                              UpdatedBy = testing.UpdatedBy,
                              UpdatedAt = testing.UpdatedAt,
                              TestTypeNameIdn = testing.TestTypeNameIdn,
                              TestTypeNameEn = testing.TestTypeNameEn,   
                              TestTypeCode = testing.TestTypeCode,      
                              TestTypeId = testing.TestTypeId,         
                              TestTypeMethodName = testing.TestTypeMethodName, 
                              TestTypeMethodCode = testing.TestTypeMethodCode,
                              TestTypeMethodId = testing.TestTypeMethodId,    
                              TestTemplateId = testing.TestTemplateId,
                              procedures = (from procedure in _context.TransactionTestingProcedures.AsNoTracking()
                                            where procedure.TransactionTestTypeMethodId == testing.Id
                                            orderby procedure.Sequence ascending
                                            select new TransactionTestingProcedureViewModel
                                            {
                                                Id = procedure.Id,
                                                TransactionTestTypeMethodId = procedure.TransactionTestTypeMethodId,
                                                TestTypeMethodCode = procedure.TestTypeMethodCode,
                                                Title = procedure.Title,
                                                Instruction = procedure.Instruction,
                                                RowStatus = procedure.RowStatus,
                                                Sequence = procedure.Sequence,
                                                AttachmentStorageName = procedure.AttachmentStorageName,
                                                AttachmentFile = procedure.AttachmentFile,
                                                IsEachSample = procedure.IsEachSample,
                                                CreatedBy = procedure.CreatedBy,
                                                CreatedAt = procedure.CreatedAt,
                                                UpdatedBy = procedure.UpdatedBy,
                                                UpdatedAt = procedure.UpdatedAt,
                                                Status = procedure.Status,
                                                parameters = (from parameter in _context.TransactionTestingProcedureParameters.AsNoTracking()
                                                              where parameter.TransactionTestingProcedureId == procedure.Id
                                                              orderby parameter.Sequence ascending
                                                              select new TransactionTestingProcedureParameterViewModel
                                                              {
                                                                  Id = parameter.Id,
                                                                  Code = parameter.Code,
                                                                  DeviationLevel = parameter.DeviationLevel,
                                                                  DeviationNote = parameter.DeviationNote,
                                                                  HasAttachment = parameter.HasAttachment,
                                                                  InputTypeId = parameter.InputTypeId,
                                                                  IsNullable = parameter.IsNullable,
                                                                  Name = parameter.Name,
                                                                  TransactionTestingProcedureId = parameter.TransactionTestingProcedureId,
                                                                  TestTypeProcedureCode = parameter.TestTypeProcedureCode,
                                                                  Properties = parameter.Properties != null ? JsonSerializer.Deserialize<dynamic>(Regex.Unescape(parameter.Properties.ToString().Trim('"')), options) : null,
                                                                  PropertiesValue = parameter.PropertiesValue != null ? JsonSerializer.Deserialize<dynamic>(Regex.Unescape(parameter.PropertiesValue.ToString().Trim('"')), options) : null,
                                                                  RowStatus = parameter.RowStatus,
                                                                  Sequence = parameter.Sequence,
                                                                  IsDeviation = parameter.IsDeviation,
                                                                  CreatedAt = parameter.CreatedAt,
                                                                  CreatedBy = parameter.CreatedBy,
                                                                  UpdatedAt = parameter.UpdatedAt,
                                                                  UpdatedBy = parameter.UpdatedBy,
                                                                  Attachments = (from attachment in _context.TransactionTestingProcedureParameterAttachments.AsNoTracking()
                                                                                 where attachment.TransactionTestingProcedureParameterId == parameter.Id
                                                                                 select attachment).ToList(),
                                                                  AttachmentHistories = (from ahistory in _context.TransactionHtrProductionProcessProcedureParameterAttachments.AsNoTracking()
                                                                                 where ahistory.TestingProcedureParameterId == parameter.Id
                                                                                 select ahistory).ToList(),
                                                                  Notes = (from note in _context.TransactionTestingProcedureParameterNotes.AsNoTracking()
                                                                           where note.TransactionTestingProcedureParameterId == parameter.Id
                                                                                 select note).ToList(),
                                                                  Histories = (from history in _context.TransactionHtrTestingProcedureParameter.AsNoTracking()
                                                                               where history.ParameterId == parameter.Id
                                                                                 select history).ToList(),
                                                              }).ToList(),
                                                parameterSamples = !    procedure.IsEachSample ? null : (from sample in _context.TransactionTestingSampling.AsNoTracking()
                                                                                                    where sample.TestingId == testingId
                                                                                                    select new ParameterTestingProcedureSample
                                                                                                    {
                                                                                                        Id = sample.Id,
                                                                                                        SampleId = sample.SampleId,
                                                                                                        SampleName = sample.SampleName,
                                                                                                        Attachments = (from attachment in _context.TransactionTestingProcedureParameterAttachments.AsNoTracking()
                                                                                                                       where attachment.TransactionTestingSamplingId == sample.Id
                                                                                                                       select attachment).ToList(),
                                                                                                        Notes = (from note in _context.TransactionTestingProcedureParameterNotes.AsNoTracking()
                                                                                                                 where note.TransactionTestingProcedureParameterId == sample.Id
                                                                                                                 select note).ToList()
                                                                                                    }).ToList(),
                                            }).ToList()
                          }).FirstOrDefaultAsync();
        }

        public async Task<TransactionTestingProcedureParameter> GetParameterById(int parameterId)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            return await (from parameter in _context.TransactionTestingProcedureParameters
                   where parameter.Id == parameterId
                          select new TransactionTestingProcedureParameter
                          {
                              Id = parameter.Id,
                              InputTypeId = parameter.InputTypeId,
                              Properties = parameter.Properties != null ? JsonSerializer.Deserialize<dynamic>(Regex.Unescape(parameter.Properties.ToString().Trim('"')), options) : null,
                              PropertiesValue = parameter.PropertiesValue != null ? JsonSerializer.Deserialize<dynamic>(Regex.Unescape(parameter.PropertiesValue.ToString().Trim('"')), options) : null,
                              Code = parameter.Code,
                              CreatedBy = parameter.CreatedBy,
                              RowStatus = parameter.RowStatus,
                              TransactionTestingProcedureId = parameter.TransactionTestingProcedureId,
                              TestTypeProcedureCode = parameter.TestTypeProcedureCode,
                              DeviationLevel = parameter.DeviationLevel,
                              DeviationNote = parameter.DeviationNote,
                              HasAttachment = parameter.HasAttachment,
                              IsNullable = parameter.IsNullable,
                              Name = parameter.Name,
                              Sequence = parameter.Sequence,
                              IsDeviation = parameter.IsDeviation,
                              CreatedAt = parameter.CreatedAt,
                              UpdatedAt = parameter.UpdatedAt,
                              UpdatedBy = parameter.UpdatedBy,
                          }).FirstOrDefaultAsync();
        }

        public async Task<TestingProcedureParameterViewModel> PatchExceptionParameter(int id, string exception)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            var patchData = await (
                from data in _context.TransactionTestingProcedureParameters
                where data.Id == id
                select data
            ).FirstOrDefaultAsync();

            patchData.DeviationNote = exception;
            patchData.DeviationLevel = 1;
            patchData.IsDeviation = true;
            await _context.SaveChangesAsync();

            return new TestingProcedureParameterViewModel
            {
                Id = patchData.Id,
                Code = patchData.Code,
                DeviationNote = exception,
                DeviationLevel = 1,
                InvestigationResult = "TEST_INVESTIGATION_RESULT",
                exceptionParameter = new List<TransactionTestingProcedureParameterAttachment>()
            };
        }

        public async Task<TransactionTestingProcedureParameterAttachment> InsertAttachmentException(string createdBy, string mediaLink, string filename, int procedureParameterId, string ext)
        {
            var attachment = new TransactionTestingProcedureParameterAttachment
            {
                CreatedBy = createdBy,
                CreatedAt = DateTime.Now.ToLocalTime(),
                UpdatedAt = DateTime.Now.ToLocalTime(),
                UpdatedBy = createdBy,
                MediaLink = mediaLink,
                Filename = filename,
                Ext = ext,
                TransactionTestingProcedureParameterId = procedureParameterId,
                Type = "DEVIASI",
                ProcedureParameter = null
            };

            _context.TransactionTestingProcedureParameterAttachments.Add(attachment);
            await _context.SaveChangesAsync();

            return attachment;
        }

        public async Task<TransactionTestingProcedure> GetProcedureById(int procedureid)
        {
            return await (from procedure in _context.TransactionTestingProcedures
                          where procedure.Id == procedureid
                          select procedure).FirstOrDefaultAsync();
        }

        public async Task<List<TransactionTestingProcedureParameterAttachment>> GetAttachmentByParameterId(int parameterId)
        {
            return await (from attachment in _context.TransactionTestingProcedureParameterAttachments
                          where attachment.TransactionTestingProcedureParameterId == parameterId
                          select attachment).ToListAsync();
        }

        public async Task<List<TransactionHtrProcessProcedureParameterAttachment>> GetAttachmentHistoryByParameterId(int parameterId)
        {
            return await (from history in _context.TransactionHtrProductionProcessProcedureParameterAttachments
                          where history.TestingProcedureParameterId == parameterId
                          select history).ToListAsync();
        }

        public async Task<TransactionTestingProcedureParameterAttachment> GetByMediaLink(string mediaLink)
        {
            return await (from attachment in _context.TransactionTestingProcedureParameterAttachments
                          where attachment.MediaLink == mediaLink
                          select attachment).FirstOrDefaultAsync();
        }

        public async Task<TransactionTestingProcedureParameter> UpdateParameter(TransactionTestingProcedureParameter data)
        {
            var nowTimestamp = _context.NowTimestamp.FromSqlRaw(ApplicationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            data.UpdatedAt = nowTimestamp.CurrentTimestamp;
            _context.TransactionTestingProcedureParameters.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<TransactionTestingProcedure> UpdateProcedure(TransactionTestingProcedure data)
        {
            var nowTimestamp = _context.NowTimestamp.FromSqlRaw(ApplicationConstant.GET_DB_CURRENT_TIMESTAMP_QUERY).FirstOrDefault();
            data.UpdatedAt = nowTimestamp.CurrentTimestamp;
            _context.TransactionTestingProcedures.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<TransactionTestingProcedureParameterAttachment> InsertProcedureParameterAttachment(TransactionTestingProcedureParameterAttachment data)
        {
            _context.TransactionTestingProcedureParameterAttachments.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<TransactionTestingProcedureParameterNote> InsertProcedureParameterNote(TransactionTestingProcedureParameterNote data)
        {
            _context.TransactionTestingProcedureParameterNotes.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<TransactionHtrTestingProcedureParameter> InsertHistoryParameter(TransactionHtrTestingProcedureParameter data)
        {
            _context.TransactionHtrTestingProcedureParameter.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }
        public async Task<TransactionHtrProcessProcedureParameterAttachment> InsertHistoryParameterAttachment(TransactionHtrProcessProcedureParameterAttachment data)
        {
            _context.TransactionHtrProductionProcessProcedureParameterAttachments.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }
    }
}
