using qcs_product.API.Infrastructure;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using qcs_product.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace qcs_product.API.DataProviders.Collection
{
    public class TemplateOperatorTestingDataProvider : ITemplateOperatorTestingDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly QualityAssuranceSystemServiceContext _qascontext;

        [ExcludeFromCodeCoverage]
        public TemplateOperatorTestingDataProvider(QcsProductContext context, QualityAssuranceSystemServiceContext qascontext)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _qascontext = qascontext ?? throw new ArgumentNullException(nameof(qascontext));
        }

        public async Task<TemplateOperatorTesting> DetailTemplateTestingOperator(int templateTestingOperatorId)
        {
            return await(from x in _context.TemplateOperatorTesting
                         where x.Id == templateTestingOperatorId
                         select x).FirstOrDefaultAsync();
        }

        public async Task<TemplateOperatorTesting> Edit(TemplateOperatorTesting data)
        {
            var dataUpdate = _context.TemplateOperatorTesting.Where(x => x.Id == data.Id).FirstOrDefault();
           
            dataUpdate.CreatedAt = data.CreatedAt;
            dataUpdate.CreatedBy = data.CreatedBy;
            dataUpdate.IdTemplate = data.IdTemplate;
            dataUpdate.ObjectStatus = data.ObjectStatus;
            dataUpdate.RowStatus = data.RowStatus;
            dataUpdate.TestingDate = data.TestingDate;
            dataUpdate.TestTypeCode = data.TestTypeCode;
            dataUpdate.TestTypeId = data.TestTypeId;
            dataUpdate.TestTypeMethodCode = data.TestTypeMethodCode;
            dataUpdate.TestTypeMethodId = data.TestTypeMethodId;
            dataUpdate.TestTypeMethodName = data.TestTypeMethodName;
            dataUpdate.TestTypeNameEn = data.TestTypeNameEn;
            dataUpdate.TestTypeNameIdn = data.TestTypeNameIdn;
            dataUpdate.UpdatedAt = DateTime.Now;
            dataUpdate.UpdatedBy = data.UpdatedBy;
            dataUpdate.ValidityPeriodEnd = data.ValidityPeriodEnd;
            dataUpdate.ValidityPeriodStart = data.ValidityPeriodStart;

            await _context.SaveChangesAsync();

            return dataUpdate;
        }

        public async Task<List<QcSamplingTemplateViewModel>> GetAll(string filter, List<int> status, DateTime? startDate, DateTime? endDate,string methodCode, int page, int limit)
        {
            filter = string.IsNullOrEmpty(filter) ? string.Empty : filter.ToLower();

            var query = (from tot in _context.TemplateOperatorTesting
                         where tot.TestTypeMethodName.ToLower().Contains(filter)
                         && status.Contains(tot.ObjectStatus) 
                         //&& methodCode.Contains(tot.TestTypeMethodCode)
                         orderby tot.UpdatedAt descending
                         select new QcSamplingTemplateViewModel
                         {
                             Id = tot.Id,
                             CreatedAt = tot.CreatedAt,
                             UpdatedAt = tot.UpdatedAt,
                             CreatedBy = tot.CreatedBy,
                             MethodId = tot.TestTypeMethodId,
                             MethodName = tot.TestTypeMethodName,
                             name = "",
                             Status = tot.ObjectStatus,
                             TestTypeId = tot.TestTypeId,
                             TestTypeName = tot.TestTypeNameIdn,
                             UpdatedBy = tot.UpdatedBy,
                             ValidityPeriodEnd = tot.ValidityPeriodEnd,
                             ValidityPeriodStart = tot.ValidityPeriodStart,
                             IdTemplate = tot.IdTemplate,
                             TypeMethodCode = tot.TestTypeMethodCode
                         }).AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(x => ((x.ValidityPeriodStart >= startDate || !startDate.HasValue) &&
                                     (x.ValidityPeriodEnd <= endDate || !endDate.HasValue)));
            }

            if (!methodCode.IsNullOrEmpty())
            {
                query = query.Where(x => x.TypeMethodCode == methodCode);
            }

            if (limit > 0)
            {
                query = query.Skip(page).Take(limit);
            }

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<TemplateOperatorTesting> Insert(TemplateOperatorTesting templateOperatorTesting)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.TemplateOperatorTesting.AddAsync(templateOperatorTesting);
                    await _context.SaveChangesAsync();

                    //Init Data ke Table Terkait
                   // QcTransactionTemplateTestingType qcTransactionTemplateTestingType = new QcTransactionTemplateTestingType();
                   // QcTransactionTemplateTestingTypeMethod qcTransactionTemplateTestingTypeMethod = new QcTransactionTemplateTestingTypeMethod();
                    //QcTransactionTemplateTestingTypeMethodResultParameter qcTransactionTemplateTestingTMP = new QcTransactionTemplateTestingTypeMethodResultParameter();
                    //QcTransactionTemplateTestingTypeMethodValidationParameter qcTransactionTemplateTestingTMV = new QcTransactionTemplateTestingTypeMethodValidationParameter();
                    //TransactionTemplateTestTypeProcessProcedure transactionTemplateTestTypeProcessProcedure = new TransactionTemplateTestTypeProcessProcedure();
                    //TransactionTemplateTestTypeProcessProcedureParameter transactionTemplateTestTypeProcessProcedureParameter = new TransactionTemplateTestTypeProcessProcedureParameter();

                    //Set Data ke table-table
                    var qcTransactionTemplateTestingType = new QcTransactionTemplateTestingType
                    {
                        Code = templateOperatorTesting.TestTypeCode,
                        CreatedAt = DateTime.Now,
                        NameId = templateOperatorTesting.TestTypeNameIdn,
                        NameEn = templateOperatorTesting.TestTypeNameEn,
                        ObjectStatus = templateOperatorTesting.ObjectStatus,
                        OrganizationId = 0,
                        RowStatus = "0",
                        CreatedBy = "System",
                    };

                    await _context.QcTransactionTemplateTestingType.AddAsync(qcTransactionTemplateTestingType);
                    await _context.SaveChangesAsync();

                    var qcTransactionTemplateTestingTypeMethod = new QcTransactionTemplateTestingTypeMethod
                    {
                        Code = templateOperatorTesting.TestTypeCode,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "System",
                        RowStatus = "0",
                        Name = templateOperatorTesting.TestTypeMethodName,
                        TestTypeCode = templateOperatorTesting.TestTypeCode,
                        TestTypeId = templateOperatorTesting.TestTypeId,
                    };

                    var qcTransactionTemplateTestingTMP = new QcTransactionTemplateTestingTypeMethodResultParameter
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = "System",
                        RowStatus = "0",
                        MethodCode = templateOperatorTesting.TestTypeMethodCode,
                        
                    };

                    var transactionTemplateTestTypeProcessProcedure = new TransactionTemplateTestTypeProcessProcedure
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = "System",
                        RowStatus = "0",
                        TestTypeProcessCode = templateOperatorTesting.TestTypeCode,
                        TransactionTemplateTestingId= qcTransactionTemplateTestingType.Id,
                    };
                    await _context.TransactionTemplateTestTypeProcessProcedure.AddAsync(transactionTemplateTestTypeProcessProcedure);
                    await _context.SaveChangesAsync();

                    var transactionTemplateTestTypeProcessProcedureParameter = new TransactionTemplateTestTypeProcessProcedureParameter
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = "System",
                        RowStatus = "0",
                        Code = templateOperatorTesting.TestTypeCode,
                        TestTypeProcessPrecedureId = transactionTemplateTestTypeProcessProcedure.Id,
                        TestTypeProcessPrecedureCode = transactionTemplateTestTypeProcessProcedure.TestTypeProcessCode
                    };

                    await _context.TransactionTemplateTestTypeProcessProcedureParameter.AddAsync(transactionTemplateTestTypeProcessProcedureParameter);
                    await _context.SaveChangesAsync();

                    var qcTransactionTemplateTestingTMV = new QcTransactionTemplateTestingTypeMethodValidationParameter
                    {
                        CreatedAt = DateTime.Today, 
                        CreatedBy = "System",
                        RowStatus = "0",
                        TestTypeMethodCode = templateOperatorTesting.TestTypeMethodCode,
                        Code = templateOperatorTesting.TestTypeCode,
                        TestTypeMethodId = templateOperatorTesting.TestTypeMethodId,
                        TestTypeProcessProcedureParameterId = transactionTemplateTestTypeProcessProcedureParameter.Id,
                        TestTypeProcessProcedureParameterCode = transactionTemplateTestTypeProcessProcedureParameter.Code
                    };


                    //Save Context

                    await _context.QcTransactionTemplateTestingTypeMethod.AddAsync(qcTransactionTemplateTestingTypeMethod);
                    await _context.SaveChangesAsync();

                    await _context.QcTransactionTemplateTestingTypeMethodResultParameter.AddAsync(qcTransactionTemplateTestingTMP);
                    await _context.SaveChangesAsync();

                    await _context.QcTransactionTemplateTestingTypeMethodValidationParameter.AddAsync(qcTransactionTemplateTestingTMV);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
               

            return templateOperatorTesting;
        }
    }
}
