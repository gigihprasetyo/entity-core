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
using qcs_product.API.Helpers;

namespace qcs_product.API.DataProviders.Collection
{
    public class QcProcessDataProvider : IQcProcessDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<QcProcessDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public QcProcessDataProvider(QcsProductContext context, ILogger<QcProcessDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<QcProcess> Delete(QcProcess data)
        {
            _context.QcProcess.Remove(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<QcProcess> GetById(int qcProcessId)
        {
            return await
            (
                from qc_process in _context.QcProcess
                where qc_process.Id == qcProcessId
                select qc_process
            ).FirstOrDefaultAsync();
        }

        public async Task<QcProcessViewModel> GetByIdDetail(int qcProcessId)
        {
            
            var result = await (from s in _context.QcProcess
                          where s.Id == qcProcessId
                          && s.ParentId == null
                          && s.RowStatus == null
                          select new QcProcessViewModel
                          {
                              Id = s.Id,
                              Sequence = s.Sequence,
                              Name = s.Name,
                              ParentId = s.ParentId,
                              RoomId = s.RoomId,
                              IsInputForm = s.IsInputForm,
                              AddSampleLayoutType = s.AddSampleLayoutType,
                              CreatedAt = s.CreatedAt,
                              CreatedBy = s.CreatedBy,
                              UpdatedAt = s.UpdatedAt,
                              UpdatedBy = s.UpdatedBy,
                              QcProcess = (from s1 in _context.QcProcess
                                           where s1.ParentId == s.Id
                                            && s1.RowStatus == null
                                           select new QcProcessViewModel
                                           {
                                               Id = s1.Id,
                                               Sequence = s1.Sequence,
                                               Name = s1.Name,
                                               ParentId = s1.ParentId,
                                               RoomId = s1.RoomId,
                                               IsInputForm = s1.IsInputForm,
                                               AddSampleLayoutType = s1.AddSampleLayoutType,
                                               CreatedAt = s1.CreatedAt,
                                               CreatedBy = s1.CreatedBy,
                                               UpdatedAt = s1.UpdatedAt,
                                               UpdatedBy = s1.UpdatedBy,
                                               FormMaterial = (from fm1 in _context.FormMaterial
                                                               join fsc1 in _context.FormSections on fm1.SectionId equals fsc1.Id
                                                               where fm1.ProcessId == s1.Id 
                                                               && fsc1.TypeId == ApplicationConstant.SECTION_TYPE_ID_MATERIAL
                                                               && fm1.RowStatus == null
                                                               select new FormMaterialViewModel
                                                               {
                                                                   Id = fm1.Id,
                                                                   Sequence = fm1.Sequence,
                                                                   ItemId = fm1.ItemId,
                                                                   Code = fm1.Code,
                                                                   Name = fm1.Name,
                                                                   BatchNumber = fm1.BatchNumber,
                                                                   DefaultPackageQty = fm1.DefaultPackageQty,
                                                                   UomPackage = fm1.UomPackage,
                                                                   DefaultQty = fm1.DefaultQty,
                                                                   Uom = fm1.Uom,
                                                                   GroupName = fm1.GroupName,
                                                                   CreatedAt = fm1.CreatedAt,
                                                                   CreatedBy = fm1.CreatedBy,
                                                                   UpdatedAt = fm1.UpdatedAt,
                                                                   UpdatedBy = fm1.UpdatedBy
                                                               }).ToList(),
                                               FormTool = (from ft1 in _context.FormTool
                                                           join fsc2 in _context.FormSections on ft1.SectionId equals fsc2.Id
                                                           where ft1.ProcessId == s1.Id
                                                           && fsc2.TypeId == ApplicationConstant.SECTION_TYPE_ID_TOOL
                                                           && ft1.RowStatus == null
                                                           select new FormToolViewModel
                                                           {
                                                               Id = ft1.Id,
                                                               Sequence = ft1.Sequence,
                                                               Type = ft1.Type,
                                                               ToolId = ft1.ToolId,
                                                               Code = ft1.Code,
                                                               Name = ft1.Name,
                                                               ItemId = ft1.ItemId,
                                                               Qty = ft1.Qty,
                                                               CreatedAt = ft1.CreatedAt,
                                                               CreatedBy = ft1.CreatedBy,
                                                               UpdatedAt = ft1.UpdatedAt,
                                                               UpdatedBy = ft1.UpdatedBy
                                                           }).ToList(),
                                               FormProcedure = (from fp1 in _context.FormProcedure
                                                                join fsc3 in _context.FormSections on fp1.SectionId equals fsc3.Id
                                                                where fp1.ProcessId == s1.Id
                                                                && fsc3.TypeId == ApplicationConstant.SECTION_TYPE_ID_PROCEDURE
                                                                && fp1.RowStatus == null
                                                                select new FormProcedureViewModel
                                                                {
                                                                    Id = fp1.Id,
                                                                    Sequence = fp1.Sequence,
                                                                    Description = fp1.Description,
                                                                    CreatedAt = fp1.CreatedAt,
                                                                    CreatedBy = fp1.CreatedBy,
                                                                    UpdatedAt = fp1.UpdatedAt,
                                                                    UpdatedBy = fp1.UpdatedBy,
                                                                    FormParameter = (from fpram in _context.FormParameters
                                                                                     where fpram.ProcedureId == fp1.Id
                                                                                     && fpram.RowStatus == null
                                                                                     select new FormParameterViewModel
                                                                                     {
                                                                                         Id = fpram.Id,
                                                                                         Sequence = fpram.Sequence,
                                                                                         Label = fpram.Label,
                                                                                         Code = fpram.Code,
                                                                                         InputType = fpram.InputType,
                                                                                         Uom = fpram.Uom,
                                                                                         ThresholdOperator = fpram.ThresholdOperator,
                                                                                         ThresholdValue = fpram.ThresholdValue,
                                                                                         ThresholdValueFrom = fpram.ThresholdValueFrom,
                                                                                         ThresholdValueTo = fpram.ThresholdValueTo,
                                                                                         NeedAttachment = fpram.NeedAttachment,
                                                                                         Note = fpram.Note,
                                                                                         IsForAllSample = fpram.IsForAllSample,
                                                                                         IsResult = fpram.IsResult,
                                                                                         DefaultValue = fpram.DefaultValue,
                                                                                         CreatedAt = fpram.CreatedAt,
                                                                                         CreatedBy = fpram.CreatedBy,
                                                                                         UpdatedAt = fpram.UpdatedAt,
                                                                                         UpdatedBy = fpram.UpdatedBy,
                                                                                     }).ToList()
                                                                    
                                                                }).ToList(),
                                               FormGeneral = (from fp2 in _context.FormProcedure
                                                              join fsc4 in _context.FormSections on fp2.SectionId equals fsc4.Id
                                                              where fp2.ProcessId == s1.Id
                                                              && fsc4.TypeId == ApplicationConstant.SECTION_TYPE_ID_NOTE
                                                              && fp2.RowStatus == null
                                                              select new FormGeneralViewModel
                                                              {
                                                                  Id = fp2.Id,
                                                                  Sequence = fp2.Sequence,
                                                                  Description = fp2.Description,
                                                                  CreatedAt = fp2.CreatedAt,
                                                                  CreatedBy = fp2.CreatedBy,
                                                                  UpdatedAt = fp2.UpdatedAt,
                                                                  UpdatedBy = fp2.UpdatedBy,
                                                                  FormParameter = (from fpram in _context.FormParameters
                                                                                   where fpram.ProcedureId == fp2.Id
                                                                                   && fpram.RowStatus == null
                                                                                   select new FormParameterViewModel
                                                                                   {
                                                                                       Id = fpram.Id,
                                                                                       Sequence = fpram.Sequence,
                                                                                       Label = fpram.Label,
                                                                                       Code = fpram.Code,
                                                                                       InputType = fpram.InputType,
                                                                                       Uom = fpram.Uom,
                                                                                       ThresholdOperator = fpram.ThresholdOperator,
                                                                                       ThresholdValue = fpram.ThresholdValue,
                                                                                       ThresholdValueFrom = fpram.ThresholdValueFrom,
                                                                                       ThresholdValueTo = fpram.ThresholdValueTo,
                                                                                       NeedAttachment = fpram.NeedAttachment,
                                                                                       Note = fpram.Note,
                                                                                       IsForAllSample = fpram.IsForAllSample,
                                                                                       IsResult = fpram.IsResult,
                                                                                       DefaultValue = fpram.DefaultValue,
                                                                                       CreatedAt = fpram.CreatedAt,
                                                                                       CreatedBy = fpram.CreatedBy,
                                                                                       UpdatedAt = fpram.UpdatedAt,
                                                                                       UpdatedBy = fpram.UpdatedBy,
                                                                                   }).ToList()

                                                              }).ToList(),
                                           }).ToList()
                              
                          }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<QcProcess> Insert(QcProcess data)
        {
            await _context.QcProcess.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<List<QcProcessViewModel>> List()
        {
            return await 
            (
                from qc_process in _context.QcProcess
                where qc_process.ParentId == null
                select new QcProcessViewModel()
                {
                    Id = qc_process.Id,
                    Sequence = qc_process.Sequence,
                    Name = qc_process.Name,
                    ParentId = qc_process.ParentId,
                    RoomId = qc_process.RoomId,
                    IsInputForm = qc_process.IsInputForm,
                    AddSampleLayoutType = qc_process.AddSampleLayoutType,
                    CreatedAt = qc_process.CreatedAt,
                    CreatedBy = qc_process.CreatedBy,
                    UpdatedAt = qc_process.UpdatedAt,
                    UpdatedBy = qc_process.UpdatedBy
                }
            ).ToListAsync();
        }

        public async Task<List<QcProcessShortViewModel>> ListShort(string search, int limit, int page, int roomId, int? purposeId)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = (from s in _context.QcProcess
                          where (EF.Functions.Like(s.Name.ToLower(), "%" + filter + "%"))
                          && s.ParentId == null
                          && s.RowStatus == null
                          select new QcProcessShortViewModel
                          {
                              Id = s.Id,
                              Sequence = s.Sequence,
                              Name = s.Name,
                              RoomId = s.RoomId,
                              IsInputForm = s.IsInputForm,
                              AddSampleLayoutType = s.AddSampleLayoutType,
                              PurposeId = s.PurposeId,
                              CreatedAt = s.CreatedAt,
                              CreatedBy = s.CreatedBy,
                              QcProcessChild = (from sc in _context.QcProcess
                                                where sc.ParentId == s.ParentId
                                                && sc.RowStatus == null
                                                select new QcProcessChildViewModel
                                                {
                                                    Id = sc.Id,
                                                    Sequence = sc.Sequence,
                                                    Name = sc.Name,
                                                    RoomId = sc.RoomId,
                                                    IsInputForm = sc.IsInputForm,
                                                    AddSampleLayoutType = sc.AddSampleLayoutType,
                                                    CreatedAt = sc.CreatedAt,
                                                    CreatedBy = sc.CreatedBy
                                                }).OrderBy(x => x.Sequence).ToList()
                          }).Where(x => (x.RoomId == roomId || roomId == 0)).OrderBy(x => x.Sequence).AsQueryable();

            if (purposeId != null)
            {
                result = result.Where(x => x.PurposeId == purposeId);
            }

            var resultData = new List<QcProcessShortViewModel>();

            if (limit > 0)
            {
                resultData = await result.Skip(page).Take(limit).ToListAsync();
            }
            else
            {
                resultData = await result.ToListAsync();
            }

            return resultData;
        }

        public async Task<QcProcess> GetQcProcessById(int id)
        {
            return await
            (
                from p in _context.QcProcess
                where p.Id == id
                && p.ParentId == null
                && p.RowStatus == null
                select p
            ).FirstOrDefaultAsync();
        }

        public async Task<List<QcProcess>> GetQcProcessByParentId(int ParentId)
        {
            return await
            (
                from p in _context.QcProcess
                where p.ParentId == ParentId
                && p.RowStatus == null
                select p
            ).ToListAsync();
        }

        public async Task<List<FormMaterial>> GetFormMaterial(int qcProcessId)
        {
            return await
            (
                from fm in _context.FormMaterial
                where fm.ProcessId == qcProcessId
                && fm.RowStatus == null
                select fm
            ).ToListAsync();
        }

        public async Task<List<FormTool>> GetFormTool(int qcProcessId)
        {
            return await
            (
                from ft in _context.FormTool
                where ft.ProcessId == qcProcessId
                && ft.RowStatus == null
                select ft
            ).ToListAsync();
        }

        public async Task<List<FormProcedure>> GetFormProcedure(int qcProcessId)
        {
            return await
            (
                from fp in _context.FormProcedure
                where fp.ProcessId == qcProcessId
                && fp.RowStatus == null
                select fp
            ).ToListAsync();
        }

        public async Task<List<FormParameter>> GetFormParameter(int procedureId)
        {
            return await
            (
                from fpr in _context.FormParameters
                where fpr.ProcedureId == procedureId
                && fpr.RowStatus == null
                select new FormParameter
                {
                    Id = fpr.Id,
                    Sequence = fpr.Sequence,
                    Label = fpr.Label,
                    Code = fpr.Code,
                    InputType = fpr.InputType,
                    Uom = fpr.Uom,
                    ThresholdOperator = fpr.ThresholdOperator,
                    ThresholdValue = fpr.ThresholdValue,
                    ThresholdValueTo = fpr.ThresholdValueTo,
                    ThresholdValueFrom = fpr.ThresholdValueFrom,
                    NeedAttachment = fpr.NeedAttachment,
                    Note = fpr.Note,
                    ProcedureId = fpr.ProcedureId,
                    IsForAllSample = fpr.IsForAllSample,
                    IsResult = fpr.IsResult,
                    DefaultValue = fpr.DefaultValue,
                    CreatedAt = fpr.CreatedAt,
                    CreatedBy = fpr.CreatedBy,
                    UpdatedAt = fpr.UpdatedAt,
                    UpdatedBy = fpr.UpdatedBy,
                    RowStatus = fpr.RowStatus
                }
            ).ToListAsync();
        }

        public async Task<List<FormSection>> GetFormSection()
        {
            return await
            (
                from fs in _context.FormSections
                where fs.RowStatus == null
                select fs
            ).ToListAsync();
        }

        public async Task<List<FormProcedure>> GetFormProcedureSection(int qcProcessId, int SectionTypeId)
        {
            return await
            (
                from fp in _context.FormProcedure
                join fs in _context.FormSections on fp.SectionId equals fs.Id
                where fp.ProcessId == qcProcessId
                && fs.TypeId == SectionTypeId
                && fp.RowStatus == null
                select fp
            ).ToListAsync();
        }
    }
}
