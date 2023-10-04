using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Q100Library.EventBus.Base.Abstractions;
using Q100Library.IntegrationEvents;
using qcs_product.API.Helpers;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.Constants;

namespace qcs_product.API.BusinessProviders.Collection
{
    public class AuditTrailBusinessProvider : IAuditTrailBusinessProvider
    {
        // private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEventBus _eventBus;
        private readonly ILogger<AuditTrailBusinessProvider> _logger;
        private readonly QcsProductContext _context;
        private readonly IAUAMServiceBusinessProviders _auamServiceBusinessProviders;

        public AuditTrailBusinessProvider(IEventBus eventBus, 
            // IHttpContextAccessor httpContextAccessor,
            ILogger<AuditTrailBusinessProvider> logger, QcsProductContext context,
            IAUAMServiceBusinessProviders auamServiceBusinessProviders)
        {
            // _httpContextAccessor = httpContextAccessor;
            _eventBus = eventBus;
            _logger = logger;
            _context = context;
            _auamServiceBusinessProviders = auamServiceBusinessProviders;
        }

        /**
         * TODO
         * Dibuat generic, untuk setiap entity yang perlu audit mungkin dibuat base class nya
         * misalnya: class Building : AuditTrail
         */
        private async Task Add(AuditTrailIntegrationEvent integrationEvent)
        {
            _logger.LogInformation("sync audit trail - {ObjectTable}", integrationEvent.ObjectTable);
            try
            {
                integrationEvent.AppId = ApplicationConstant.APP_CODE;
                // integrationEvent.IpClient = GetIpClient();
                // integrationEvent.ObjectTable = GetTableName(integrationEvent.ObjectTable);
                // integrationEvent.ParentTable = GetTableName(integrationEvent.ParentTable);
                //integrationEvent.CreatedBy = await GetUsernameByNik(integrationEvent.CreatedBy); /* Heavy Load data */
                integrationEvent.CreatedBy = integrationEvent.CreatedBy;

                _eventBus.PublishAsync(integrationEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
            }
        }

        // private string GetIpClient()
        // {
        //     try
        //     {
        //         return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        //     }
        //     catch (Exception)
        //     {
        //         // _logger.LogError(ex, "{Message}", ex.Message);
        //     }

        //     return "";
        // }
        private string GetTableName(BaseEntity baseEntity)
        {

            if (baseEntity == null)
            {
                return "";
            }

            var entity = _context.Model.FindEntityType(baseEntity.GetType());
            if (entity != null)
            {
                return entity.GetTableName();
            }

            return baseEntity.GetType().Name;
        }

        private Dictionary<string, dynamic> PopulateValues(BaseEntity baseEntity)
        {
            var entityType = _context.Model.FindEntityType(baseEntity.GetType());
            var columns = new Dictionary<string, dynamic>();
            foreach (var property in entityType.GetProperties())
            {
                var columnName = property.GetColumnName();
                var value = property.FieldInfo.GetValue(baseEntity);

                if (value != null)
                {
                    if (property.FieldInfo.FieldType == typeof(DateTime))
                    {
                        columns[columnName] = DateHelper.ToStr((DateTime)value);
                    }
                    else
                    {
                        columns[columnName] = value;
                    }
                    continue;
                }

                columns[columnName] = "";
            };
            return columns;
        }

        public async Task Add(string operation, string remarks, RequestQcs entity, string usernameModifier)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = $"{ApplicationConstant.MODULE_NAME_QC_REQUEST}-{entity.NoRequest}";
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = usernameModifier;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);

            if (entity.TestTypeQcs != null && entity.TestTypeQcs.Any())
            {
                foreach (var testType in entity.TestTypeQcs)
                {
                    await Add(operation, testType.TestParameterName, testType, entity, audit.ObjectCode, usernameModifier);
                }
            }
        }

        private async Task Add(string operation, string remarks, TestTypeQcs entity, RequestQcs parent, string objectCode, string usernameModifier)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = usernameModifier;
            audit.ParentTable = GetTableName(parent);
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);
        }

        public async Task Add(string operation, string remarks, QcSampling entity, string usernameModifier)
        {

            var requestQcs = await _context.RequestQcs.FindAsync(entity.RequestQcsId);
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = $"{ApplicationConstant.MODULE_NAME_QC_SAMPLING}-{requestQcs.NoRequest}-{entity.SamplingTypeName}";
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = usernameModifier;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);

            if (entity.Tools != null)
            {
                foreach (var tool in entity.Tools)
                {
                    await Add(operation, tool.ToolCode, tool, entity, audit.ObjectCode, usernameModifier);
                }
            }

            if (entity.Materials != null)
            {
                foreach (var material in entity.Materials)
                {
                    await Add(operation, $"{material.ItemName} ({material.NoBatch})", material, entity, audit.ObjectCode, usernameModifier);
                }
            }

            if (entity.Samples != null)
            {
                foreach (var sample in entity.Samples)
                {
                    if (string.IsNullOrEmpty(sample.Code))
                    {
                        continue;
                    }

                    var sampleRemarks = sample.Code;
                    if (!string.IsNullOrEmpty(sample.SamplingPointCode))
                    {
                        sampleRemarks = $"{sample.SamplingPointCode} ({sample.Code})";
                    }

                    await Add(operation, sampleRemarks, sample, entity, audit.ObjectCode, usernameModifier);
                }
            }
        }

        private async Task Add(string operation, string remarks, QcSamplingMaterial entity, QcSampling parent, string objectCode, string usernameModifier)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = usernameModifier;
            audit.ParentTable = GetTableName(parent);
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);
        }

        private async Task Add(string operation, string remarks, QcSamplingTools entity, QcSampling parent, string objectCode, string usernameModifier)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = usernameModifier;
            audit.ParentTable = GetTableName(parent);
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);
        }

        private async Task Add(string operation, string remarks, QcSample entity, QcSampling parent, string objectCode, string usernameModifier)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;//TODO temporary solution
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = usernameModifier;
            audit.ParentTable = GetTableName(parent);
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);
        }

        public async Task Add(string operation, string remarks, QcSamplingShipment entity, string usernameModifier)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = $"{ApplicationConstant.MODULE_NAME_QC_TRANSFER}-{entity.NoRequest}";
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = usernameModifier;

            // var endDateStr = "";
            // if (entity.EndDate.HasValue)
            // {
            //     endDateStr = DateHelper.ToStr(entity.EndDate.Value);
            // }
            //
            // var startDateStr = "";
            // if (entity.StartDate.HasValue)
            // {
            //     startDateStr = DateHelper.ToStr(entity.StartDate.Value);
            // }

            //TODO formating field start_date and end_date
            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            // if (entity.QcSamplingShipmentTrackers != null && entity.QcSamplingShipmentTrackers.Any())
            // {
            //     foreach (var qcSamplingShipmentTracker in entity.QcSamplingShipmentTrackers)
            //     {
            //         await Add(operation, entity.NoRequest, qcSamplingShipmentTracker, entity);
            //     }
            // }

            await Add(audit);
        }

        public async Task Add(string operation, string remarks, QcSamplingShipmentTracker entity, string usernameModifier)
        {
            await Add(operation, remarks, entity, null, usernameModifier);
        }

        public async Task Add(string operation, string remarks, QcSamplingShipmentTracker entity, QcSamplingShipment parent, string usernameModifier)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = $"{ApplicationConstant.MODULE_NAME_QC_TRANSFER}-{parent.NoRequest}";
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = usernameModifier;
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);
        }

        public async Task Add(string operation, string remarks, QcTransactionGroup entity)
        {

            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = $"{ApplicationConstant.MODULE_NAME_QC_TEST}-{entity.Code}";
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = await GetUsernameByNik(entity.UpdatedBy);

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            if (entity.TransactionGroupProcesses != null)
            {
                foreach (var process in entity.TransactionGroupProcesses)
                {
                    await Add(operation, process.Name, process, entity, audit.ObjectCode);
                }
            }

            if (entity.TransactionGroupSamples != null)
            {
                foreach (var sample in entity.TransactionGroupSamples)
                {
                    var getSample = await (from s in _context.QcSamples
                                           where s.Id == sample.QcSampleId
                                           select s).FirstOrDefaultAsync();
                    if (getSample != null)
                    {
                        await Add(operation, getSample.Code, sample, entity, audit.ObjectCode);
                    }
                }
            }

            if (entity.TransactionGroupSamplings != null)
            {
                var batchNumbers = new List<string>();
                foreach (var sample in entity.TransactionGroupSamplings)
                {
                    var qcSampling = await (from s in _context.QcSamplings
                                            where s.Id == sample.QcSamplingId
                                            select s).FirstOrDefaultAsync();
                    if (qcSampling != null)
                    {
                        //pastikan tidak ada duplikasi data yang dikirim ke audit trail
                        var qcRequest = await _context.RequestQcs.FirstOrDefaultAsync(x => x.Id == qcSampling.RequestQcsId);
                        if (batchNumbers.Exists(batchNumber => batchNumber == qcRequest.NoBatch))
                        {
                            continue;
                        }

                        batchNumbers.Add(qcRequest.NoBatch);

                        await Add(operation, qcRequest.NoBatch, sample, entity, audit.ObjectCode);
                    }
                }
            }

            await Add(audit);
        }

        private async Task Add(string operation, string remarks, QcTransactionGroupProcess entity, QcTransactionGroup parent, string objectCode)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = await GetUsernameByNik(entity.UpdatedBy);
            audit.ParentTable = GetTableName(parent);
            audit.ParentIdRowData = entity.QcTransactionGroupId;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);


            if (entity.TransactionGroupFormProcedures != null)
            {
                foreach (var formProcedure in entity.TransactionGroupFormProcedures)
                {
                    await Add(operation, formProcedure.Id.ToString(), formProcedure, entity, objectCode);
                }
            }

            if (entity.TransactionGroupFormTools != null)
            {
                foreach (var formTool in entity.TransactionGroupFormTools)
                {
                    var tool = await _context.Tools.FirstOrDefaultAsync(x => x.Id == formTool.ToolId);
                    if (tool == null)
                    {
                        continue;
                    }
                    await Add(operation, tool.ToolCode, formTool, entity, objectCode);
                }
            }

        }

        private async Task Add(string operation, string remarks, QcTransactionGroupSample entity, QcTransactionGroup parent, string objectCode)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = await GetUsernameByNik(entity.UpdatedBy);
            audit.ParentTable = parent.GetType().Name;
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);
        }

        private async Task Add(string operation, string remarks, QcTransactionGroupSampling entity, QcTransactionGroup parent, string objectCode)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = await GetUsernameByNik(entity.UpdatedBy);
            audit.ParentTable = parent.GetType().Name;
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);
        }

        public async Task Add(string operation, string remarks, QcTransactionGroupFormMaterial entity, QcTransactionGroupProcess parent, string objectCode)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = await GetUsernameByNik(entity.UpdatedBy);
            audit.ParentTable = parent.GetType().Name;
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);
        }

        private async Task Add(string operation, string remarks, QcTransactionGroupFormParameter entity, QcTransactionGroupFormProcedure parent, string objectCode)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = await GetUsernameByNik(entity.UpdatedBy);
            audit.ParentTable = parent.GetType().Name;
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);

            if (entity.TransactionGroupValues != null)
            {
                foreach (var transactionGroupValue in entity.TransactionGroupValues)
                {
                    var param = await _context.QcTransactionGroupFormParameters.FirstOrDefaultAsync(x =>
                        x.Id == transactionGroupValue.QcTransactionGroupFormParameterId);

                    if (param == null)
                    {
                        continue;
                    }

                    await Add(operation, param.Code, transactionGroupValue, entity, objectCode);
                }
            }

            if (entity.TransactionGroupSampleValues != null)
            {
                foreach (var transactionGroupValue in entity.TransactionGroupSampleValues)
                {

                    var transactionSample = await _context.QcTransactionSamples.FirstOrDefaultAsync(x =>
                        x.QcSampleId == transactionGroupValue.QcTransactionSampleId);
                    if (transactionSample == null)
                    {
                        continue;
                    }


                    var sample =
                        await _context.QcSamples.FirstOrDefaultAsync(x => x.Id == transactionSample.QcSampleId);

                    if (sample == null)
                    {
                        continue;
                    }


                    await Add(operation, sample.Code, transactionGroupValue, entity, objectCode);
                }
            }
        }

        //TODO perbaikan valud remakrs
        private async Task Add(string operation, string remarks, QcTransactionGroupFormProcedure entity, QcTransactionGroupProcess parent, string objectCode)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = $"{entity.Id}";
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = await GetUsernameByNik(entity.UpdatedBy);
            audit.ParentTable = parent.GetType().Name;
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);

            if (entity.TransactionGroupFormParameters != null)
            {
                foreach (var formParameter in entity.TransactionGroupFormParameters)
                {
                    await Add(operation, formParameter.Code, formParameter, entity, objectCode);
                }
            }
        }

        private async Task Add(string operation, string remarks, QcTransactionGroupFormTool entity, QcTransactionGroupProcess parent, string objectCode)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = await GetUsernameByNik(entity.UpdatedBy);
            audit.ParentTable = parent.GetType().Name;
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);
        }

        private async Task Add(string operation, string remarks, QcTransactionGroupValue entity, QcTransactionGroupFormParameter parent, string objectCode)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = await GetUsernameByNik(entity.UpdatedBy);
            audit.ParentTable = parent.GetType().Name;
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);
        }

        private async Task Add(string operation, string remarks, QcTransactionGroupSampleValue entity, QcTransactionGroupFormParameter parent, string objectCode)
        {
            var audit = new AuditTrailIntegrationEvent();
            audit.Operation = operation;
            audit.Remaks = remarks;
            audit.ObjectCode = objectCode;
            audit.ObjectTable = GetTableName(entity);
            audit.IdRowData = entity.Id;
            audit.ProcessedAt = entity.UpdatedAt;
            audit.CreatedBy = await GetUsernameByNik(entity.UpdatedBy);
            audit.ParentTable = parent.GetType().Name;
            audit.ParentIdRowData = parent.Id;

            audit.Data = JsonSerializer.Serialize(PopulateValues(entity));

            await Add(audit);
        }

        public async Task<string> GetUsernameByNik(string nik)
        {
            var personal = await _auamServiceBusinessProviders.GetPersonalDetailByNik(nik);
            if (personal != null) return personal.Email;

            var personalExt = await _auamServiceBusinessProviders.GetPersonalExtDetailByNik(nik);
            if (personalExt != null) return personalExt.Email;

            return nik;
        }
    }
}