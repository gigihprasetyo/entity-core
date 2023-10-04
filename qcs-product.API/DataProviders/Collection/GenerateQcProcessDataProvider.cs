using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using qcs_product.API.Helpers;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.Constants;

namespace qcs_product.API.DataProviders.Collection
{
    public class GenerateQcProcessDataProvider : IGenerateQcProcessDataProvider
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<GenerateQcProcessDataProvider> _logger;
        private readonly QcsProductContext _context;


        public GenerateQcProcessDataProvider(IServiceScopeFactory serviceScopeFactory, ILogger<GenerateQcProcessDataProvider> logger, QcsProductContext context)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _context = context;
        }
        
        public async Task Generate(QcTransactionGroup data)
        {
            var stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            _logger.LogInformation("GenerateQcTestProcessAltV2 {Id} - start", data.Id);
            #region add parent process

            var sampleTestList = await (from st in _context.QcTransactionSamples
                where st.QcTransactionGroupId == data.Id
                select st).ToListAsync();
            
            var parentProcess = await GetQcProcessById(data.QcProcessId);
            var newParentProcess = new QcTransactionGroupProcess()
            {
                QcTransactionGroupId = data.Id,
                Sequence = parentProcess.Sequence,
                Name = parentProcess.Name,
                ParentId = parentProcess.ParentId,
                RoomId = parentProcess.RoomId,
                IsInputForm = parentProcess.IsInputForm,
                QcProcessId = data.QcProcessId,
                CreatedBy = data.CreatedBy,
                UpdatedBy = data.CreatedBy,
                CreatedAt = DateHelper.Now(),
                UpdatedAt = DateHelper.Now()
            };
            await _context.QcTransactionGroupProcesses.AddAsync(newParentProcess);
            await _context.SaveChangesAsync(); // save data
            
            #endregion
            
            #region add child process
            
            var processList1 = await GetQcProcessByParentId(data.QcProcessId);
            var newProcessList1 = new List<QcTransactionGroupProcess>();
            
            if (processList1.Any())
            {
                newProcessList1.AddRange(processList1.Select(p => new QcTransactionGroupProcess()
                {
                    QcTransactionGroupId = data.Id,
                    Sequence = p.Sequence,
                    Name = p.Name,
                    ParentId = newParentProcess.Id,
                    RoomId = p.RoomId,
                    IsInputForm = p.IsInputForm,
                    QcProcessId = p.Id,
                    CreatedBy = data.CreatedBy,
                    UpdatedBy = data.CreatedBy,
                    CreatedAt = DateHelper.Now(),
                    UpdatedAt = DateHelper.Now()
                }));

                await _context.QcTransactionGroupProcesses.AddRangeAsync(newProcessList1);
                await _context.SaveChangesAsync(); // save data
            }

            #endregion

            #region add form section
            
            var formSectionList = await GetFormSection();
            if (!formSectionList.Any())
            {
                return;
            }

            var newFormSectionList = new List<QcTransactionGroupFormSection>();
            newFormSectionList.AddRange(formSectionList.Select(fs => new QcTransactionGroupFormSection()
            {
                QcProcessId = data.QcProcessId,
                SectionId = fs.Id,
                SectionTypeId = fs.TypeId,
                Sequence = fs.Sequence,
                Label = fs.Label,
                Icon = fs.Icon,
                CreatedBy = data.CreatedBy,
                UpdatedBy = data.CreatedBy,
                CreatedAt = DateHelper.Now(),
                UpdatedAt = DateHelper.Now()
            }));

            await _context.QcTransactionGroupFormSections.AddRangeAsync(newFormSectionList);
            await _context.SaveChangesAsync(); // save data

            if (!newProcessList1.Any())
            {
                return;
            }

            foreach (var pc in newProcessList1)
            {
                //loop section
                foreach (var newFormSection in newFormSectionList)
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    switch (newFormSection.SectionTypeId)
                    {
                        case ApplicationConstant.SECTION_TYPE_ID_MATERIAL:
                            await AddTransactionGroupMaterial(data, pc, newFormSection);
                            stopwatch.Stop();
                            _logger.LogInformation("AddTransactionGroupMaterial. Elapsed Time is {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
                            break;
                        case ApplicationConstant.SECTION_TYPE_ID_TOOL:
                            await AddTransactionGroupTool(data, pc, newFormSection);
                            stopwatch.Stop();
                            _logger.LogInformation("AddTransactionGroupTool. Elapsed Time is {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
                            break;
                        case ApplicationConstant.SECTION_TYPE_ID_PROCEDURE:
                            await AddTransactionGroupProcedure(data, sampleTestList, pc, newFormSection);
                            stopwatch.Stop();
                            _logger.LogInformation("AddTransactionGroupProcedure. Elapsed Time is {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
                            break;
                        case ApplicationConstant.SECTION_TYPE_ID_NOTE:
                            await AddTransactionGroupGeneralProcedure(data, sampleTestList, pc, newFormSection);
                            stopwatch.Stop();
                            _logger.LogInformation("AddTransactionGroupGeneralProcedure. Elapsed Time is {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
                            break;
                    }
                }
            }

            #endregion
            
            stopwatch2.Stop();
            _logger.LogInformation("GenerateQcTestProcessAltV2. Elapsed Time is {ElapsedMilliseconds} ms", stopwatch2.ElapsedMilliseconds);
        }
        
        private async Task<QcProcess> GetQcProcessById(int id)
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
        
        private async Task<List<FormSection>> GetFormSection()
        {
            return await
            (
                from fs in _context.FormSections
                where fs.RowStatus == null
                select fs
            ).ToListAsync();
        }
        
        private async Task AddTransactionGroupProcedure(QcTransactionGroup data, List<QcTransactionGroupSample> sampleTestList, QcTransactionGroupProcess pc, QcTransactionGroupFormSection trxFormSection)
        {
            /* get form procedure process */
            var formProcedureList =
                await GetFormProcedureSection(pc.QcProcessId,
                    ApplicationConstant.SECTION_TYPE_ID_PROCEDURE);

            if (!formProcedureList.Any())
            {
                return;
            }
            
            var newFormProcedureTestList = formProcedureList.Select(frmProcedure => new QcTransactionGroupFormProcedure()
                {
                    QcTransactionGroupProcessId = pc.Id,
                    Sequence = frmProcedure.Sequence,
                    Description = frmProcedure.Description,
                    FormProcedureId = frmProcedure.Id,
                    QcTransactionGroupSectionId = trxFormSection.Id,
                    CreatedBy = data.CreatedBy,
                    UpdatedBy = data.CreatedBy,
                    CreatedAt = DateHelper.Now(),
                    UpdatedAt = DateHelper.Now()
                })
                .ToList();

            await _context.QcTransactionGroupFormProcedures.AddRangeAsync(newFormProcedureTestList);
            await _context.SaveChangesAsync(); // save data

            var newFormParameterList = new List<QcTransactionGroupFormParameter>();
            foreach (var prc in newFormProcedureTestList)
            {
                var formParameterProcedures = await GetFormParameter(prc.FormProcedureId);
                if (!formParameterProcedures.Any())
                {
                    continue;
                }

                newFormParameterList.AddRange(formParameterProcedures.Select(fpp =>
                    new QcTransactionGroupFormParameter()
                    {
                        QcTransactionGroupFormProcedureId = prc.Id,
                        Sequence = fpp.Sequence,
                        Label = fpp.Label,
                        Code = fpp.Code,
                        InputType = fpp.InputType,
                        Uom = fpp.Uom,
                        ThresholdOperator = fpp.ThresholdOperator,
                        ThresholdValue = fpp.ThresholdValue,
                        ThresholdValueTo = fpp.ThresholdValueTo,
                        ThresholdValueFrom = fpp.ThresholdValueFrom,
                        NeedAttachment = fpp.NeedAttachment,
                        Note = fpp.Note,
                        FormProcedureId = prc.FormProcedureId,
                        IsForAllSample = fpp.IsForAllSample,
                        IsResult = fpp.IsResult,
                        DefaultValue = fpp.DefaultValue,
                        CreatedBy = data.CreatedBy,
                        UpdatedBy = data.CreatedBy,
                        CreatedAt = DateHelper.Now(),
                        UpdatedAt = DateHelper.Now()
                    }));
            }

            if (newFormParameterList.Any())
            {
                await _context.QcTransactionGroupFormParameters.AddRangeAsync(newFormParameterList);
                await _context.SaveChangesAsync(); // save data

                //insert data sample param value
                if (pc.QcProcessId != ApplicationConstant.PROCESS_UJI_IDENTIFIKASI)
                {
                    foreach (var fpr in newFormParameterList)
                    {
                        if (sampleTestList.Any() && fpr.IsForAllSample == false)
                        {
                            var newSampleTestValues = new List<QcTransactionGroupSampleValue>();
                            var seqValSample = 1;
                            foreach (var newSampleTestValue in sampleTestList.Select(std =>
                                         new QcTransactionGroupSampleValue()
                                         {
                                             QcTransactionGroupFormParameterId = fpr.Id,
                                             QcTransactionSampleId = std.Id,
                                             Sequence = seqValSample,
                                             CreatedBy = data.CreatedBy,
                                             UpdatedBy = data.CreatedBy,
                                             CreatedAt = DateHelper.Now(),
                                             UpdatedAt = DateHelper.Now()
                                         }))
                            {
                                seqValSample++;
                                newSampleTestValues.Add(newSampleTestValue);
                            }

                            await _context.QcTransactionSampleValues.AddRangeAsync(newSampleTestValues);
                            await _context.SaveChangesAsync(); // save data
                        }
                        else if (fpr.IsForAllSample)
                        {
                            var newValue = new QcTransactionGroupValue()
                            {
                                QcTransactionGroupFormParameterId = fpr.Id,
                                Sequence = 1,
                                Value = fpr.DefaultValue,
                                CreatedBy = data.CreatedBy,
                                UpdatedBy = data.CreatedBy,
                                CreatedAt = DateHelper.Now(),
                                UpdatedAt = DateHelper.Now()
                            };

                            await _context.QcTransactionGroupValues.AddAsync(newValue);
                            await _context.SaveChangesAsync(); // save data
                        }
                    }
                }
            }
        }

        private async Task AddTransactionGroupGeneralProcedure(QcTransactionGroup data, List<QcTransactionGroupSample> sampleTest,
            QcTransactionGroupProcess pc, QcTransactionGroupFormSection trxFormSection)
        {
            /* get General Form procedure */
            var formProcedureList = await GetFormProcedureSection(pc.QcProcessId, ApplicationConstant.SECTION_TYPE_ID_NOTE);
            var newFormProcedureList = new List<QcTransactionGroupFormProcedure>();
            if (formProcedureList.Any())
            {
                newFormProcedureList.AddRange(formProcedureList.Select(frmProcedure => new QcTransactionGroupFormProcedure()
                {
                    QcTransactionGroupProcessId = pc.Id,
                    Sequence = frmProcedure.Sequence,
                    Description = frmProcedure.Description,
                    FormProcedureId = frmProcedure.Id,
                    QcTransactionGroupSectionId = trxFormSection.Id,
                    CreatedBy = data.CreatedBy,
                    UpdatedBy = data.CreatedBy,
                    CreatedAt = DateHelper.Now(),
                    UpdatedAt = DateHelper.Now()
                }));
            }

            if (newFormProcedureList.Any())
            {
                await _context.QcTransactionGroupFormProcedures.AddRangeAsync(newFormProcedureList);
                await _context.SaveChangesAsync(); // save data

                var newFormParameterTestList = new List<QcTransactionGroupFormParameter>();
                foreach (var prc in newFormProcedureList)
                {
                    var getFromParameterByProcedure = await GetFormParameter(prc.FormProcedureId);
                    if (!getFromParameterByProcedure.Any())
                    {
                        continue;
                    }

                    newFormParameterTestList.AddRange(getFromParameterByProcedure.Select(fpp => new QcTransactionGroupFormParameter()
                    {
                        QcTransactionGroupFormProcedureId = prc.Id,
                        Sequence = fpp.Sequence,
                        Label = fpp.Label,
                        Code = fpp.Code,
                        InputType = fpp.InputType,
                        Uom = fpp.Uom,
                        ThresholdOperator = fpp.ThresholdOperator,
                        ThresholdValue = fpp.ThresholdValue,
                        ThresholdValueTo = fpp.ThresholdValueTo,
                        ThresholdValueFrom = fpp.ThresholdValueFrom,
                        NeedAttachment = fpp.NeedAttachment,
                        Note = fpp.Note,
                        FormProcedureId = prc.FormProcedureId,
                        IsForAllSample = fpp.IsForAllSample,
                        IsResult = fpp.IsResult,
                        DefaultValue = fpp.DefaultValue,
                        CreatedBy = data.CreatedBy,
                        UpdatedBy = data.CreatedBy,
                        CreatedAt = DateHelper.Now(),
                        UpdatedAt = DateHelper.Now()
                    }));
                }
                
                if (newFormParameterTestList.Any())
                {
                    await _context.QcTransactionGroupFormParameters.AddRangeAsync(newFormParameterTestList);
                    await _context.SaveChangesAsync();
                    
                    var newSampleValueList = new List<QcTransactionGroupSampleValue>();
                    var newGroupValueList = new List<QcTransactionGroupValue>();
                    
                    //insert data sample param value
                    if (pc.QcProcessId != ApplicationConstant.PROCESS_UJI_IDENTIFIKASI)
                    {
                        foreach (var fpr in newFormParameterTestList)
                        {
                            if (sampleTest.Any() && fpr.IsForAllSample == false)
                            {
                                var seqValSample = 1;
                                foreach (var newSampleValue in sampleTest.Select(std => new QcTransactionGroupSampleValue()
                                         {
                                             QcTransactionGroupFormParameterId = fpr.Id,
                                             QcTransactionSampleId = std.Id,
                                             Sequence = seqValSample,
                                             CreatedBy = data.CreatedBy,
                                             UpdatedBy = data.CreatedBy,
                                             CreatedAt = DateHelper.Now(),
                                             UpdatedAt = DateHelper.Now()
                                         }))
                                {
                                    seqValSample++;
                                    newSampleValueList.Add(newSampleValue);
                                }
                            }
                            else if (fpr.IsForAllSample)
                            {
                                var newValue = new QcTransactionGroupValue()
                                {
                                    QcTransactionGroupFormParameterId = fpr.Id,
                                    Sequence = 1,
                                    Value = fpr.DefaultValue,
                                    CreatedBy = data.CreatedBy,
                                    UpdatedBy = data.CreatedBy,
                                    CreatedAt = DateHelper.Now(),
                                    UpdatedAt = DateHelper.Now()
                                };
                                
                                newGroupValueList.Add(newValue);
                            }
                        }
                    }

                    if (newSampleValueList.Any() || newGroupValueList.Any())
                    {
                        await _context.QcTransactionSampleValues.AddRangeAsync(newSampleValueList);
                        await _context.QcTransactionGroupValues.AddRangeAsync(newGroupValueList);
                        await _context.SaveChangesAsync(); 
                    }
                    
                }
            }
        }

        private async Task AddTransactionGroupTool(QcTransactionGroup data, QcTransactionGroupProcess pc, QcTransactionGroupFormSection trxFormSection)
        {
            var formToolList = await GetFormTool(pc.QcProcessId);

            if (formToolList.Any())
            {
                var newFormToolTestList = formToolList.Select(ft =>
                        new QcTransactionGroupFormTool()
                        {
                            QcTransactionGroupProcessId = pc.Id,
                            Sequence = ft.Sequence,
                            ToolId = ft.ToolId,
                            ItemId = ft.ItemId,
                            Code = ft.Code,
                            Name = ft.Name,
                            Quantity = ft.Qty,
                            QcProcessId = data.QcProcessId,
                            QcTransactionGroupSectionId = trxFormSection.Id,
                            CreatedBy = data.CreatedBy,
                            UpdatedBy = data.CreatedBy,
                            CreatedAt = DateHelper.Now(),
                            UpdatedAt = DateHelper.Now()
                        })
                    .ToList();

                await _context.QcTransactionGroupFormTools.AddRangeAsync(newFormToolTestList);
                await _context.SaveChangesAsync(); // save data
            }
        }

        private async Task AddTransactionGroupMaterial(QcTransactionGroup data, QcTransactionGroupProcess pc, QcTransactionGroupFormSection trxFormSection)
        {
            /* get form material process */
            var formMaterialList = await GetFormMaterial(pc.QcProcessId);

            if (formMaterialList.Any())
            {
                var newFormMaterialTestList = new List<QcTransactionGroupFormMaterial>();

                newFormMaterialTestList.AddRange(formMaterialList.Select(fm => new QcTransactionGroupFormMaterial()
                {
                    QcTransactionGroupProcessId = pc.Id,
                    Sequence = fm.Sequence,
                    ItemId = fm.ItemId,
                    Code = fm.BatchNumber,
                    Name = fm.Name,
                    DefaultPackageQty = fm.DefaultPackageQty,
                    UomPackage = fm.UomPackage,
                    DefaultQty = fm.DefaultQty,
                    Uom = fm.Uom,
                    QcProcessId = data.QcProcessId,
                    QcTransactionGroupSectionId = trxFormSection.Id,
                    GroupName = fm.GroupName,
                    CreatedBy = data.CreatedBy,
                    UpdatedBy = data.CreatedBy,
                    CreatedAt = DateHelper.Now(),
                    UpdatedAt = DateHelper.Now()
                }));

                await _context.QcTransactionGroupFormMaterials.AddRangeAsync(newFormMaterialTestList);
                await _context.SaveChangesAsync(); // save data
            }
        }
        
        private async Task<List<FormMaterial>> GetFormMaterial(int qcProcessId)
        {
            return await
            (
                from fm in _context.FormMaterial
                where fm.ProcessId == qcProcessId
                      && fm.RowStatus == null
                select fm
            ).ToListAsync();
        }
        
        private async Task<List<FormTool>> GetFormTool(int qcProcessId)
        {
            return await
            (
                from ft in _context.FormTool
                where ft.ProcessId == qcProcessId
                && ft.RowStatus == null
                select ft
            ).ToListAsync();
        }

        private async Task<List<FormProcedure>> GetFormProcedure(int qcProcessId)
        {
            return await
            (
                from fp in _context.FormProcedure
                where fp.ProcessId == qcProcessId
                && fp.RowStatus == null
                select fp
            ).ToListAsync();
        }

        private async Task<List<FormParameter>> GetFormParameter(int procedureId)
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

        private async Task<List<QcProcess>> GetQcProcessByParentId(int ParentId)
        {
            return await
            (
                from p in _context.QcProcess
                where p.ParentId == ParentId
                      && p.RowStatus == null
                select p
            ).ToListAsync();
        }
        
        private async Task<List<FormProcedure>> GetFormProcedureSection(int qcProcessId, int SectionTypeId)
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