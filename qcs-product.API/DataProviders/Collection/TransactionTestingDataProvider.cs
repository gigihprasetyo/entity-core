using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using qcs_product.API.BusinessProviders.Collection;
using qcs_product.API.BindingModels;
using qcs_product.API.ViewModels;

namespace qcs_product.API.DataProviders.Collection
{
    public class TransactionTestingDataProvider : ITransactionTestingDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<TransactionTestingDataProvider> _logger;

        public TransactionTestingDataProvider(QcsProductContext context, ILogger<TransactionTestingDataProvider> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<TransactionTestingViewModel>> GetByFilter(string filter, int page, int limit, List<int> status, DateTime? startDate, DateTime? endDate)
        {
            filter = !string.IsNullOrEmpty(filter) ? filter.ToLower() : string.Empty;

            var query = (from a in _context.TransactionTesting
                         where (a.TestingDate >= startDate || !startDate.HasValue) &&
                         (a.TestingDate <= endDate || !endDate.HasValue) &&
                         status.Contains(a.ObjectStatus) &&
                         a.RowStatus != "deleted" &&
                         a.Code.ToLower().Contains(filter)
                         select new TransactionTestingViewModel
                         {
                             CreatedAt = a.CreatedAt,
                             UpdatedAt = a.UpdatedAt,
                             Code = a.Code,
                             CreatedBy = a.CreatedBy,
                             Id = a.Id,
                             ObjectStatus = a.ObjectStatus,
                             TestingDate = a.TestingDate,
                             TestTemplateId = a.TestTemplateId,
                             TestTypeCode = a.TestTypeCode,
                             TestTypeId = a.TestTypeId,
                             TestTypeMethodCode = a.TestTypeMethodCode,
                             TestTypeMethodId = a.TestTypeMethodId,
                             TestTypeMethodName = a.TestTypeMethodName,
                             TestTypeNameEn = a.TestTypeNameEn,
                             TestTypeNameIdn = a.TestTypeNameEn,
                             TotalSampling = 0,
                             UpdatedBy = a.UpdatedBy
                         });

            var transactionTesting = await query.Skip(page).Take(limit).ToListAsync();

            foreach (var item in transactionTesting)
            {
                item.TotalSampling = (from a in _context.TransactionTestingSampling
                                      where a.TestingId == item.Id
                                      select a).Count();
            }

            return transactionTesting;
        }
        public async Task<TransactionTesting> Insert(InsertTransactionTestingBindingModel transactionTesting)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //insert to transaction testing
                    var lastCode = await _context.TransactionTesting.OrderByDescending(x => x.Code).Select(x => x.Code).FirstOrDefaultAsync();
                    lastCode = lastCode.Substring(3, lastCode.Length - 3);
                    var code = Convert.ToInt32(lastCode) + 1;
                    var transactionCode = string.Format("ORG{0:00000000}", code);

                    var insertTransactionTesting = new TransactionTesting
                    {
                        CreatedAt = transactionTesting.CreatedAt,
                        UpdatedAt = transactionTesting.CreatedAt,
                        Code = transactionCode,
                        CreatedBy = transactionTesting.CreatedBy,
                        ObjectStatus = transactionTesting.ObjectStatus,
                        RowStatus = null,
                        TestingDate = transactionTesting.TestingDate,
                        TestTemplateId = transactionTesting.TestTemplateId,
                        TestTypeCode = transactionTesting.TestTypeCode,
                        TestTypeId = transactionTesting.TestTypeId,
                        TestTypeMethodCode = transactionTesting.TestTypeMethodCode,
                        TestTypeMethodId = transactionTesting.TestTypeMethodId,
                        TestTypeMethodName = transactionTesting.TestTypeMethodName,
                        TestTypeNameEn = transactionTesting.TestTypeNameEn,
                        TestTypeNameIdn = transactionTesting.TestTypeNameIdn,
                        UpdatedBy = transactionTesting.CreatedBy
                    };
                    await _context.TransactionTesting.AddAsync(insertTransactionTesting);
                    await _context.SaveChangesAsync();

                    //insert sampling
                    foreach (var item in transactionTesting.Samplings)
                    {
                        var insertTestingSampling = new TransactionTestingSampling
                        {
                            Attachment = item.Attachment,
                            Notes = item.Notes,
                            SampleId = item.SamplingId,
                            SampleName = item.SamplingName,
                            TestingCode = item.TestingCode,
                            TestingId = insertTransactionTesting.Id,
                            CreatedAt = transactionTesting.CreatedAt,
                            UpdatedAt = transactionTesting.CreatedAt,
                            RowStatus = null,
                            UpdatedBy = transactionTesting.CreatedBy,
                            CreatedBy = transactionTesting.CreatedBy,
                        };
                        await _context.TransactionTestingSampling.AddAsync(insertTestingSampling);
                    }
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return insertTransactionTesting;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{Message}", ex.Message);
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<TransactionTestingDetailViewModel> Detail(int id)
        {
            var trxTesting = await (from a in _context.TransactionTesting
                                    where a.Id == id
                                    select new TransactionTestingDetailViewModel
                                    {
                                        CreatedAt = a.CreatedAt,
                                        UpdatedAt = a.UpdatedAt,
                                        Code = a.Code,
                                        CreatedBy = a.CreatedBy,
                                        Id = a.Id,
                                        ObjectStatus = a.ObjectStatus,
                                        TestingDate = a.TestingDate,
                                        TestTemplateId = a.TestTemplateId,
                                        TestTypeCode = a.TestTypeCode,
                                        TestTypeId = a.TestTypeId,
                                        TestTypeMethodCode = a.TestTypeMethodCode,
                                        TestTypeMethodId = a.TestTypeMethodId,
                                        TestTypeMethodName = a.TestTypeMethodName,
                                        TestTypeNameEn = a.TestTypeNameEn,
                                        TestTypeNameIdn = a.TestTypeNameIdn,
                                        UpdatedBy = a.UpdatedBy,
                                        TestingStartDate = a.TestingStartDate,
                                        TestingEndDate = a.TestingEndtDate
                                    }).FirstOrDefaultAsync();

            trxTesting.samplings = await (from a in _context.TransactionTestingSampling
                                          where a.TestingId == id
                                          select new TransactionTestingSamplingViewModel
                                          {
                                              Attachment = a.Attachment,
                                              CreatedAt = a.CreatedAt,
                                              UpdatedAt = a.UpdatedAt,
                                              CreatedBy = a.CreatedBy,
                                              Id = a.Id,
                                              Notes = a.Notes,
                                              SamplingId = a.SampleId,
                                              SamplingName = a.SampleName,
                                              TestingCode = a.TestingCode,
                                              TestingId = a.TestingId,
                                              UpdatedBy = a.UpdatedBy
                                          }).ToListAsync();

            trxTesting.TotalSampling = trxTesting.samplings.Count();

            return trxTesting;
        }
        public async Task<TransactionTesting> Update(UpdateTransactionTestingBindingModel transactionTesting)
        {
            var getData = await (from a in _context.TransactionTesting
                                 where a.Id == transactionTesting.Id
                                 select a).FirstOrDefaultAsync();

            if(getData != null)
            {
                var update = new TransactionTesting
                {
                  CreatedAt = getData.CreatedAt,
                  UpdatedAt = DateTime.Now,
                  CreatedBy = getData.CreatedBy,
                  UpdatedBy = transactionTesting.UpdatedBy,
                  RowStatus = getData.RowStatus,
                  Code = getData.Code,
                  ObjectStatus = transactionTesting.ObjectStatus,
                  TestingDate = transactionTesting.TestingDate,
                  Id = getData.Id,
                  TestingEndtDate = getData.TestingEndtDate,
                  TestingStartDate = getData.TestingStartDate,
                  TestTemplateId = transactionTesting.TestTemplateId,
                  TestTypeCode = transactionTesting.TestTypeCode,
                  TestTypeId = transactionTesting.TestTypeId,
                  TestTypeMethodCode = transactionTesting.TestTypeMethodCode,
                  TestTypeMethodId = transactionTesting.TestTypeMethodId,
                  TestTypeMethodName = transactionTesting.TestTypeMethodName,
                  TestTypeNameEn = transactionTesting.TestTypeNameEn,
                  TestTypeNameIdn = transactionTesting.TestTypeNameIdn
                };

                _context.TransactionTesting.Update(update);
                await _context.SaveChangesAsync();

                return update;
            }
            return null;
        }
    }
}
