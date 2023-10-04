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


namespace qcs_product.API.DataProviders.Collection
{
    public class ReviewKasieDataProvider : IReviewKasieDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly q100_authorizationContext _authContext;
        private readonly ILogger<ReviewKasieDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public ReviewKasieDataProvider(
            QcsProductContext context, 
            q100_authorizationContext authContext, 
            ILogger<ReviewKasieDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _authContext = authContext;
            _logger = logger;
        }

        public async Task<TransactionTestingPersonnel> CheckInCheckOut(UpdateCheckInCheckOutPersonnel data)
        {
            TransactionTestingPersonnel dataPersonnel = _context.TransactionTestingPersonnel.FirstOrDefault(x => x.Id == data.TransactionTestingPersonnelId);

            if (dataPersonnel != null)
            {
                if (data.Type == "checkin")
                {
                    //dataPersonnel.CheckIn = DateTime.UtcNow.AddHours(7);
                }
                else if (data.Type == "checkout")
                {
                    //dataPersonnel.CheckOut = DateTime.UtcNow.AddHours(7);
                }

                dataPersonnel.UpdatedBy = data.UpdatedBy;
                dataPersonnel.UpdatedAt = DateTime.UtcNow.AddHours(7);

                await _context.SaveChangesAsync();
            }

            return dataPersonnel;
        }

        public List<TransactionTestingAttachment> DeleteAttachment(List<int> listId)
        {
            try
            {
                List<TransactionTestingAttachment> getData = (from x in _context.TransactionTestingAttachment
                                                              where listId.Contains(x.Id)
                                                              select x).ToList();

                _context.TransactionTestingAttachment.RemoveRange(getData);
                _context.SaveChanges();
                return getData;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
            return null;
        }

        public async Task<List<TransactionTestingAttachment>> GetAttachmentByTemplateTestingId(int templateId)
        {
            return await (from x in _context.TransactionTestingAttachment
                          where x.TestingId == templateId
                          select x).ToListAsync();
        }

        public async Task<List<TransactionTestingNote>> GetNoteByTemplateTestingId(int templateId)
        {
            return await (from x in _context.TransactionTestingNote
                          where x.TestingId == templateId
                          select x).ToListAsync();
        }

        public async Task<List<TransactionTestingPersonnel>> GetPersonnelByTemplateTestingId(int templateId, string filter)
        {
            return await (from x in _context.TransactionTestingPersonnel
                          where x.TestingId == templateId || filter != null ? x.Nama.ToLower().Contains(filter) : true
                          select x).ToListAsync();

        }

        public async Task<ResponseViewModel<ReviewKasieTemplateQCListViewModel>> ListReviewTemplate(int positionId, string search, List<int> status, DateTime? startDate, DateTime? endDate, int page = 1, int limit = 10)
        {
            ResponseViewModel<ReviewKasieTemplateQCListViewModel> result = new ResponseViewModel<ReviewKasieTemplateQCListViewModel>();
            string filter = "";
            int totalData = 0;
            int totalPage = 0;
            PaginationHelper pagination = new PaginationHelper(page, limit);

            if (!endDate.HasValue)
            {
                DateTime dt = DateTime.Now;
                endDate = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
            }

            if (!startDate.HasValue)
            {
                DateTime dt = DateTime.Now;
                startDate = new DateTime(dt.Year, dt.Month, dt.Day);
            }

            filter = string.IsNullOrEmpty(search) ? string.Empty : search.ToLower();

            var query = (from trxTest in _context.TransactionTesting
                         join ttt in _context.TemplateOperatorTesting on trxTest.TestTemplateId equals ttt.Id
                         join stat in _context.Status on trxTest.ObjectStatus equals stat.Id
                         where
                         (trxTest.TestTypeNameEn.ToLower().Contains(filter)
                             || (trxTest.TestTypeNameIdn ?? "").ToLower().Contains(filter)
                             || (ttt.TestTypeMethodName ?? "").ToLower().Contains(filter)
                             || (ttt.IdTemplate ?? "").ToLower().Contains(filter))
                             &&
                         status.Contains(trxTest.ObjectStatus)
                         &&
                         trxTest.RowStatus != "deleted"
                         select new ReviewKasieTemplateQCListViewModel
                         {
                             Id = trxTest.Id,
                             CreatedAt = trxTest.CreatedAt,
                             UpdatedAt = trxTest.UpdatedAt,
                             MethodId = trxTest.TestTypeMethodId,
                             MethodName = trxTest.TestTypeMethodName,
                             EndValidityPeriod = ttt.ValidityPeriodEnd,
                             StartValidityPeriod = ttt.ValidityPeriodStart,
                             ValidityPeriod = ttt.ValidityPeriodStart.ToString("dd MMM yyyy") + "-" + ttt.ValidityPeriodEnd.ToString("dd MMM yyyy"),
                             IdTemplate = ttt.IdTemplate,
                             Status = trxTest.ObjectStatus,
                             StatusName = stat.Name,
                             TestTypeName = trxTest.TestTypeNameIdn
                         })
                          .Where(x => ((x.StartValidityPeriod <= startDate || !startDate.HasValue) &&
                                     (x.EndValidityPeriod <= endDate || !endDate.HasValue))
                        ).AsQueryable();

            //filter by status
            if (status.Count > 0)
            {
                query = query.Where(x => status.Contains(x.Status)).AsQueryable();
            }

            //filter by positionId
            //if (positionId != 0)
            //{
            //    var role = (from a in _authContext.PositionToRole
            //                join b in _authContext.Role on a.RoleCode equals b.RoleCode
            //                where a.RowStatus != "deleted"
            //                && a.PosId == positionId.ToString()
            //                select b.RoleName.ToLower()).FirstOrDefault();

            //    if (role.Contains("kabag") || role.Contains("kasie"))
            //    {
            //        query = query.Where(x => x.StatusName.ToLower().Contains(role)).AsQueryable();
            //    }
            //}

            var resultData = new List<ReviewKasieTemplateQCListViewModel>();
            totalData = query.Count();

            if (limit > 0)
            {
                resultData = await query.Skip(pagination.CalculateOffset()).Take(limit).ToListAsync();
                totalPage = (int)Math.Ceiling((double)totalData / limit);
            }
            else
            {
                resultData = await query.ToListAsync();
                totalPage = 1;
            }

            var metaData = new MetaViewModel()
            {
                TotalItem = totalData,
                TotalPages = totalPage
            };

            if (!resultData.Any())
            {
                result.StatusCode = 404;
                result.Message = ApplicationConstant.NO_CONTENT_MESSAGE;
            }
            else
            {
                result.StatusCode = 200;
                result.Message = ApplicationConstant.OK_MESSAGE;
                result.Data = resultData;
                result.Meta = metaData;
            }

            return result;

        }

        public async Task<List<TransactionTestingAttachment>> UpdateAttachment(List<TransactionTestingAttachment> data)
        {
            try
            {
                await _context.TransactionTestingAttachment.AddRangeAsync(data);
                await _context.SaveChangesAsync();
                return data;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
            return null;
        }

        public async Task<TransactionTestingNote> UpdateNote(TransactionTestingNote data)
        {
            try
            {
                await _context.TransactionTestingNote.AddAsync(data);
                await _context.SaveChangesAsync();
                return data;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
            return null;
        }

        public Task<List<TransactionTestingPersonnel>> UpdatePersonnel(List<TransactionTestingPersonnel> data)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TransactionTemplateTestTypeProcess>> GetProcessByTemplateTestingId(int templateId)
        {
            return await (from x in _context.TransactionTemplatetestTypeProcess
                          join tt in _context.TransactionTesting on x.Methodid equals tt.TestTypeMethodId
                          where tt.Id == templateId
                          select x).ToListAsync();
        }
    }
}
