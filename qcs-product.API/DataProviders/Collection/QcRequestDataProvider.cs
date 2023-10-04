using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Helpers;
// using AutoMapper;

namespace qcs_product.API.DataProviders.Collection
{
    public class QcRequestDataProvider : IQcRequestDataProvider
    {

        private readonly QcsProductContext _context;
        // private readonly IMapper _mapper;
        private readonly IReviewDataProvider _reviewDataProvider;
        private readonly IRoomDataProvider _dataProviderRoom;
        private readonly IToolDataProvider _dataProviderTool;
        private readonly ITestScenarioDataProvider _dataProviderTestScenario;
        private readonly IPurposeDataProvider _dataProviderPurpose;
        private readonly ILogger<QcRequestDataProvider> _logger;
        private readonly ITransactionRoomDataProvider _transactionRoomDataProvider;
        private readonly ITransactionTestScenarioDataProvider _transactionTestScenariDataProvider;
        private readonly ITransactionDataProvider _transactionDataProvider;
        private readonly ITransactionBatchDataProvider _transactionBatchDataProvider;

        [ExcludeFromCodeCoverage]
        public QcRequestDataProvider(
            QcsProductContext context,
            // IMapper mapper,
            IRoomDataProvider dataProviderRoom,
            IToolDataProvider dataProviderTool,
            ITestScenarioDataProvider dataProviderTestScenario,
            IPurposeDataProvider dataProviderPurpose,
            ILogger<QcRequestDataProvider> logger,
            IReviewDataProvider reviewDataProvider,
            ITransactionRoomDataProvider transactionRoomDataProvider,
            ITransactionTestScenarioDataProvider transactionTestScenariDataProvider,
            ITransactionDataProvider transactionDataProvider,
            ITransactionBatchDataProvider transactionBatchDataProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            // _mapper = mapper;
            _reviewDataProvider = reviewDataProvider ?? throw new ArgumentNullException(nameof(reviewDataProvider));
            _dataProviderRoom = dataProviderRoom ?? throw new ArgumentNullException(nameof(dataProviderRoom));
            _dataProviderTool = dataProviderTool ?? throw new ArgumentNullException(nameof(dataProviderTool));
            _dataProviderTestScenario = dataProviderTestScenario ?? throw new ArgumentNullException(nameof(dataProviderTestScenario));
            _dataProviderPurpose = dataProviderPurpose ?? throw new ArgumentNullException(nameof(dataProviderPurpose));
            _logger = logger;
            _transactionRoomDataProvider = transactionRoomDataProvider;
            _transactionTestScenariDataProvider = transactionTestScenariDataProvider;
            _transactionDataProvider = transactionDataProvider;
            _transactionBatchDataProvider = transactionBatchDataProvider;
        }

        public async Task<List<RequestQcsRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int orgId, int TypeRequestId)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = (from p in _context.RequestQcs
                          where ((EF.Functions.Like(p.NoRequest.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(p.TypeRequest.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(p.NoBatch.ToLower(), "%" + filter + "%")))
                          && status.Contains(p.Status)
                          && p.RowStatus == null
                          select new RequestQcsRelationViewModel
                          {

                              RequestQcs = new RequestQcsViewModel
                              {
                                  Id = p.Id,
                                  OrgId = p.OrgId,
                                  Date = p.Date,
                                  NoRequest = p.NoRequest,
                                  ItemId = p.ItemId,
                                  ItemName = p.ItemName,
                                  TestScenarioLabel = p.TestScenarioLabel,
                                  TypeFormId = p.TypeFormId,
                                  TypeFormName = p.TypeFormName,
                                  NoBatch = p.NoBatch,
                                  TypeRequestId = p.TypeRequestId,
                                  TypeRequest = p.TypeRequest,
                                  ProductFormId = p.ProductFormId,
                                  ProductFormName = p.ProductFormName,
                                  ProductPhaseId = p.ProductPhaseId,
                                  ProductPhaseName = p.ProductPhaseName,
                                  ProductTemperature = p.ProductTemperature,
                                  Location = p.Location,
                                  ProcessId = p.ProcessId,
                                  ProcessName = p.ProcessName,
                                  ProcessDate = p.ProcessDate,
                                  ItemTemperature = p.ItemTemperature,
                                  IsNoBatchEditable = p.IsNoBatchEditable,
                                  Status = p.Status,
                                  //   WorkFlowHistory = (from wh in _context.WorkflowHistories
                                  //                      join per in _context.Personals on wh.PersonalId equals per.Id
                                  //                      join ps in _context.Positions on per.PosId equals ps.Id
                                  //                      where wh.GroupMenuName == ApplicationConstant.GROUP_NAME_REQUEST_QCS
                                  //                      && wh.GroupMenuId == p.Id && wh.RowStatus == null
                                  //                      orderby wh.CreatedAt descending
                                  //                      select new WorkflowHistoryViewModel
                                  //                      {
                                  //                          Id = wh.Id,
                                  //                          Action = wh.Action,
                                  //                          Note = wh.Note,
                                  //                          GroupMenuId = wh.GroupMenuId,
                                  //                          GroupMenuName = wh.GroupMenuName,
                                  //                          PersonalName = per.Name,
                                  //                          PersonalNik = per.Nik,
                                  //                          Position = ps.Name,
                                  //                          CreatedAt = wh.CreatedAt
                                  //                      }).ToList(),
                                  TestTypeQcs = (
                                                  from t in _context.ProductTestTypeQcs
                                                  where t.RequestQcsId == p.Id
                                                  && t.RowStatus == null
                                                  select new TestTypeQcsViewModel
                                                  {
                                                      Id = t.Id,
                                                      OrgId = t.OrgId,
                                                      OrgName = t.OrgName,
                                                      RequestQcsId = t.RequestQcsId,
                                                      TestTypeId = (t.TestTypeId == 0 ? 0 : t.TestTypeId),
                                                      TestTypeName = t.TestTypeName,
                                                      TestParameterId = (t.TestParameterId == 0 ? 0 : t.TestParameterId),
                                                      TestParameterName = t.TestParameterName,
                                                      SampleAmountCount = t.SampleAmountCount,
                                                      SampleAmountVolume = t.SampleAmountVolume,
                                                      SampleAmountUnit = t.SampleAmountUnit
                                                  }

                                  ).ToList(),
                                  CreatedBy = p.CreatedBy,
                                  CreatedAt = p.CreatedAt,
                                  IsFromBulkRequest = p.IsFromBulkRequest

                              }


                          }).Where(x => ((x.RequestQcs.Date >= startDate || !startDate.HasValue) &&
                                         (x.RequestQcs.Date <= endDate || !endDate.HasValue)) &&
                                         (x.RequestQcs.OrgId == orgId || orgId == 0) &&
                                         (x.RequestQcs.TypeRequestId == TypeRequestId || TypeRequestId == 0)
                                        ).OrderByDescending(x => x.RequestQcs.CreatedAt).AsQueryable();

            var resultData = new List<RequestQcsRelationViewModel>();

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

        public async Task<RequestQcs> Insert(RequestQcs data, List<TestTypeQcs> dataTestType)
        {
            RequestQcs result = new RequestQcs();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.RequestQcs.AddAsync(data);
                    await _context.SaveChangesAsync();
                    
                    data.NoRequest = data.Id + data.NoRequest;
                    await _context.SaveChangesAsync();

                    // insert daftar uji

                    if (dataTestType.Any())
                    {
                        dataTestType.ForEach(x => x.RequestQcsId = data.Id);
                        await _context.ProductTestTypeQcs.AddRangeAsync(dataTestType);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();


                    if (dataTestType.Any())
                    {
                        await generateSamplingProductQcAlt(data, dataTestType);
                    }


                    data.TestTypeQcs = dataTestType;
                    result = data;
                    // transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }


            return result;
        }

        public async Task<RequestQcs> Edit(RequestQcs data, List<TestTypeQcs> dataTestType, bool isSubmit)
        {
            RequestQcs result = new RequestQcs();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.RequestQcs.Update(data);
                    await _context.SaveChangesAsync();

                    // insert daftar uji

                    if (dataTestType.Any())
                    {
                         var getProductTestTypeQcs = _context.ProductTestTypeQcs.Where(x => x.RequestQcsId == data.Id).ToList();
                        _context.ProductTestTypeQcs.RemoveRange(getProductTestTypeQcs);

                        dataTestType.ForEach(x => x.RequestQcsId = data.Id);
                        await _context.ProductTestTypeQcs.AddRangeAsync(dataTestType);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();


                    if (dataTestType.Any())
                    {
                        await generateSamplingProductQcAlt(data, dataTestType);
                    }


                    data.TestTypeQcs = dataTestType;
                    result = data;
                    // transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        public async Task<RequestQcs> EditNoBatch(RequestQcs data)
        {
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<RequestQcs> RejectRequestQc(RequestQcs data)
        {
            RequestQcs result = new RequestQcs();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();

                    var getSampling = await (from sm in _context.QcSamplings
                                             where sm.RequestQcsId == data.Id
                                             && sm.RowStatus == null
                                             select sm).ToListAsync();

                    // change status in sampling data
                    if (getSampling.Any())
                    {
                        foreach (var samp in getSampling)
                        {
                            samp.Status = ApplicationConstant.STATUS_CANCEL;
                            samp.UpdatedBy = data.UpdatedBy;
                            samp.UpdatedAt = DateHelper.Now();
                        }

                        await _context.SaveChangesAsync();
                    }

                    await _context.SaveChangesAsync();

                    result = data;
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }


            return result;

        }

        public async Task<List<TestTypeQcs>> InsertProductTestTypeQcs(List<TestTypeQcs> data)
        {
            await _context.ProductTestTypeQcs.AddRangeAsync(data);
            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<RequestQcs> GetById(int id)
        {
            return await (from rq in _context.RequestQcs
                          where rq.Id == id
                          select rq).FirstOrDefaultAsync();
        }
        public async Task<RequestQcs> EditStatus(RequestQcs data)
        {

            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<Personal> GetPersonalById(Int32 Id)
        {
            return await (from p in _context.Personals
                          where p.Id == Id
                          select p).FirstOrDefaultAsync();
        }

        public async Task<List<RequestQcsListViewModel>> ListShort(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int orgId, int TypeRequestId)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = (from p in _context.RequestQcs
                          join tb in _context.TransactionBatches on p.Id equals tb.RequestQcsId into tbGroup
                          from tb in tbGroup.DefaultIfEmpty()
                          join tbl in _context.TransactionBatchLines on tb.Id equals tbl.TrsBatchId into tblGroup
                          from tbl in tblGroup.DefaultIfEmpty()
                          where (
                            (EF.Functions.Like(p.NoRequest.ToLower(), "%" + filter + "%")) ||
                            (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")) ||
                            (EF.Functions.Like(p.NoBatch.ToLower(), "%" + filter + "%")) ||
                            (EF.Functions.Like(p.TypeRequest.ToLower(), "%" + filter + "%")) ||
                            (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")) ||
                            EF.Functions.Like(tbl.NoBatch.ToLower(), "%" + filter + "%")
                          )
                          && status.Contains(p.Status)
                          && p.RowStatus == null
                          select new RequestQcsListViewModel
                          {
                              Id = p.Id,
                              Date = p.Date,
                              NoRequest = p.NoRequest,
                              TypeRequestId = p.TypeRequestId,
                              TypeRequest = p.TypeRequest,
                              NoBatch = p.NoBatch,
                              ItemId = p.ItemId,
                              ItemName = p.ItemName,
                              TypeFormId = p.TypeFormId,
                              TypeFormName = p.TypeFormName,
                              Status = p.Status,
                              WorkflowStatus = p.WorkflowStatus,
                              OrgId = p.OrgId,
                              OrgName = p.OrgName,
                              CreatedAt = p.CreatedAt,
                              CreatedBy = p.CreatedBy,
                              UpdatedAt = p.UpdatedAt,
                              UpdatedBy = p.UpdatedBy
                          }).Where(x => ((x.Date >= startDate || !startDate.HasValue) &&
                                         (x.Date <= endDate || !endDate.HasValue)) &&
                                         (x.OrgId == orgId || orgId == 0) &&
                                         (x.TypeRequestId == TypeRequestId || TypeRequestId == 0)
                                        )
                          .Distinct()
                          .OrderByDescending(x => x.Id)
                          .ThenByDescending(x => (x.UpdatedAt > x.CreatedAt ? x.UpdatedAt : x.CreatedAt))
                          .AsQueryable();

            var resultData = new List<RequestQcsListViewModel>();

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

        public async Task<List<RequestQcsRelationViewModel>> GetRequestQcById(int requestQcId)
        {
            var result = await (from p in _context.RequestQcs
                                where p.Id == requestQcId
                                && p.RowStatus == null
                                select new RequestQcsRelationViewModel
                                {

                                    RequestQcs = new RequestQcsViewModel
                                    {
                                        Id = p.Id,
                                        OrgId = p.OrgId,
                                        OrgName = p.OrgName,
                                        Date = p.Date,
                                        NoRequest = p.NoRequest,
                                        ItemId = p.ItemId,
                                        PurposeId = p.PurposeId,
                                        PurposeName = p.PurposeName,
                                        // ItemCode = (p.ItemId != null ? (from mi in _context.Items
                                        //                                 where mi.Id == p.ItemId
                                        //                                 select mi.ItemCode).FirstOrDefault() : null),
                                        ItemName = p.ItemName,
                                        TestScenarioLabel = p.TestScenarioLabel,
                                        TypeFormId = p.TypeFormId,
                                        TypeFormName = p.TypeFormName,
                                        NoBatch = p.NoBatch,
                                        TypeRequestId = p.TypeRequestId,
                                        TypeRequest = p.TypeRequest,
                                        ProductFormId = p.ProductFormId,
                                        ProductFormName = p.ProductFormName,
                                        ProductGroupId = p.ProductGroupId,
                                        ProductGroupName = p.ProductGroupName,
                                        ProductPresentationId = p.ProductPresentationId,
                                        ProductPresentationName = p.ProductPresentationName,
                                        ProductPhaseId = p.ProductPhaseId,
                                        ProductPhaseName = p.ProductPhaseName,
                                        ProductTemperature = p.ProductTemperature,
                                        Status = p.Status,
                                        StorageTemperatureId = p.StorageTemperatureId,
                                        StorageTemperatureName = p.StorageTemperatureName,
                                        EmPhaseId = p.EmPhaseId,
                                        EmPhaseName = p.EmPhaseName,
                                        NoDeviation = p.NoDeviation,
                                        Conclusion = p.Conclusion,
                                        FacilityId = p.FacilityId,
                                        FacilityCode = p.FacilityCode,
                                        FacilityName = p.FacilityName,
                                        Location = p.Location,
                                        ProcessId = p.ProcessId,
                                        ProcessName = p.ProcessName,
                                        ProcessDate = p.ProcessDate,
                                        ItemTemperature = p.ItemTemperature,
                                        IsNoBatchEditable = p.IsNoBatchEditable,
                                        // RequestAhu = (from ra in _context.RequestAhus
                                        //               where ra.QcRequestId == p.Id
                                        //               && ra.RowStatus == null
                                        //               select new RequestAhuViewModel
                                        //               {
                                        //                   Id = ra.Id,
                                        //                   AhuId = ra.AhuId,
                                        //                   AhuCode = ra.AhuCode,
                                        //                   AhuName = ra.AhuName
                                        //               }).ToList(),
                                        // RequestRooms = (from rr in _context.RequestRooms
                                        //                 where rr.QcRequestId == p.Id
                                        //                 && rr.RowStatus == null
                                        //                 select new RequestRoomViewModel
                                        //                 {
                                        //                     Id = rr.Id,
                                        //                     RoomId = rr.RoomId,
                                        //                     RoomCode = rr.RoomCode,
                                        //                     RoomName = rr.RoomName,
                                        //                     GradeRoomId = rr.GradeRoomId,
                                        //                     GradeRoomCode = rr.GradeRoomCode,
                                        //                     GradeRoomName = rr.GradeRoomName,
                                        //                     TestScenarioId = rr.TestScenarioId,
                                        //                     TestScenarioName = rr.TestScenarioName,
                                        //                     TestScenarioLabel = rr.TestScenarioLabel,
                                        //                     AhuId = rr.AhuId,
                                        //                     AhuCode = rr.AhuCode,
                                        //                     AhuName = rr.AhuName,
                                        //                 }).ToList(),
                                        // RequestPurposes = (from rp in _context.RequestPurposes
                                        //                    where rp.QcRequestId == p.Id
                                        //                    && rp.RowStatus == null
                                        //                    orderby rp.Id
                                        //                    select new RequestPurposesViewModel
                                        //                    {
                                        //                        Id = rp.Id,
                                        //                        PurposeId = rp.PurposeId,
                                        //                        PurposeCode = rp.PurposeCode,
                                        //                        PurposeName = rp.PurposeName
                                        //                    }).ToList(),
                                        // WorkFlowHistory = (from wh in _context.WorkflowHistories
                                        //                    join per in _context.Personals on wh.PersonalId equals per.Id
                                        //                    join ps in _context.Positions on per.PosId equals ps.Id
                                        //                    where wh.GroupMenuName == ApplicationConstant.GROUP_NAME_REQUEST_QCS
                                        //                    && wh.GroupMenuId == p.Id && wh.RowStatus == null
                                        //                    orderby wh.CreatedAt descending
                                        //                    select new WorkflowHistoryViewModel
                                        //                    {
                                        //                        Id = wh.Id,
                                        //                        Action = wh.Action,
                                        //                        Note = wh.Note,
                                        //                        GroupMenuId = wh.GroupMenuId,
                                        //                        GroupMenuName = wh.GroupMenuName,
                                        //                        PersonalName = per.Name,
                                        //                        PersonalNik = per.Nik,
                                        //                        Position = ps.Name,
                                        //                        CreatedAt = wh.CreatedAt
                                        //                    }).ToList(),
                                        TestTypeQcs = (from t in _context.ProductTestTypeQcs
                                                    //    join tps in _context.TestParameters on t.TestParameterId equals tps.Id into tempParam
                                                    //    from jm in tempParam.DefaultIfEmpty()
                                                       where t.RequestQcsId == p.Id
                                                       && t.RowStatus == null
                                                       select new TestTypeQcsViewModel
                                                       {
                                                           Id = t.Id,
                                                           OrgId = t.OrgId,
                                                           OrgName = t.OrgName,
                                                           RequestQcsId = t.RequestQcsId,
                                                           PurposeId = t.PurposeId,
                                                           TestTypeId = t.TestTypeId,
                                                           TestTypeName = t.TestTypeName,
                                                           TestTypeMethodId = t.TestTypeMethodId,
                                                           TestTypeMethodName = t.TestTypeMethodName,
                                                           TestParameterId = null,
                                                           TestParameterSequence = null,
                                                           TestParameterName = null,
                                                           SampleAmountCount = t.SampleAmountCount,
                                                           SampleAmountVolume = t.SampleAmountVolume,
                                                           SampleAmountUnit = t.SampleAmountUnit,
                                                           SampleAmountPresentation = t.SampleAmountPresentation,
                                                       }).OrderBy(x => x.TestParameterSequence).ToList(),
                                        ItemSamplings = (from t in _context.ProductTestTypeQcs
                                                    //    join tps in _context.TestParameters on t.TestParameterId equals tps.Id into tempParam
                                                    //    from jm in tempParam.DefaultIfEmpty()
                                                       where t.RequestQcsId == p.Id
                                                       && t.RowStatus == null
                                                       select new TestTypeQcsViewModel
                                                       {
                                                           Id = t.Id,
                                                           OrgId = t.OrgId,
                                                           OrgName = t.OrgName,
                                                           RequestQcsId = t.RequestQcsId,
                                                           PurposeId = t.PurposeId,
                                                           TestTypeId = t.TestTypeId,
                                                           TestTypeName = t.TestTypeName,
                                                           TestTypeMethodId = t.TestTypeMethodId,
                                                           TestTypeMethodName = t.TestTypeMethodName,
                                                           TestParameterId = null,
                                                           TestParameterSequence = null,
                                                           TestParameterName = null,
                                                           SampleAmountCount = t.SampleAmountCount,
                                                           SampleAmountVolume = t.SampleAmountVolume,
                                                           SampleAmountUnit = t.SampleAmountUnit,
                                                           SampleAmountPresentation = t.SampleAmountPresentation,
                                                       }).OrderBy(x => x.TestParameterSequence).ToList(),
                                        CreatedBy = p.CreatedBy,
                                        CreatedAt = p.CreatedAt,
                                        IsFromBulkRequest = p.IsFromBulkRequest
                                    }
                                }).ToListAsync();

            var batch = await _transactionBatchDataProvider.GetByRequestId(requestQcId);

            if (batch == null) return result;

            foreach (var item in result)
            {
                item.RequestQcs.Batch = batch;
            }

            return result;
        }

        public async Task<RequestQcsViewModel> GetRequestQcByBatchNumber(string BatchNumber)
        {
            var result = await (from p in _context.RequestQcs
                                where p.NoBatch == BatchNumber
                                && p.RowStatus == null
                                select new RequestQcsViewModel
                                {
                                    Id = p.Id,
                                    OrgId = p.OrgId,
                                    OrgName = p.OrgName,
                                    Date = p.Date,
                                    NoRequest = p.NoRequest,
                                    ItemId = p.ItemId,
                                    ItemCode = (p.ItemId != null ? (from mi in _context.Items
                                                                    where mi.Id == p.ItemId
                                                                    select mi.ItemCode).FirstOrDefault() : null),
                                    ItemName = p.ItemName,
                                    TestScenarioLabel = p.TestScenarioLabel,
                                    TypeFormId = p.TypeFormId,
                                    TypeFormName = p.TypeFormName,
                                    NoBatch = p.NoBatch,
                                    TypeRequestId = p.TypeRequestId,
                                    TypeRequest = p.TypeRequest,
                                    ProductFormId = p.ProductFormId,
                                    ProductFormName = p.ProductFormName,
                                    ProductPhaseId = p.ProductPhaseId,
                                    ProductPhaseName = p.ProductPhaseName,
                                    ProductTemperature = p.ProductTemperature,
                                    Status = p.Status,
                                    StorageTemperatureId = p.StorageTemperatureId,
                                    StorageTemperatureName = p.StorageTemperatureName,
                                    EmPhaseId = p.EmPhaseId,
                                    EmPhaseName = p.EmPhaseName,
                                    NoDeviation = p.NoDeviation,
                                    Conclusion = p.Conclusion,
                                    FacilityId = p.FacilityId,
                                    FacilityCode = p.FacilityCode,
                                    FacilityName = p.FacilityName,
                                    Location = p.Location,
                                    ProcessId = p.ProcessId,
                                    ProcessName = p.ProcessName,
                                    ProcessDate = p.ProcessDate,
                                    ItemTemperature = p.ItemTemperature,
                                    IsNoBatchEditable = p.IsNoBatchEditable,
                                    RequestAhu = (from ra in _context.RequestAhus
                                                  where ra.QcRequestId == p.Id
                                                  && ra.RowStatus == null
                                                  select new RequestAhuViewModel
                                                  {
                                                      Id = ra.Id,
                                                      AhuId = ra.AhuId,
                                                      AhuCode = ra.AhuCode,
                                                      AhuName = ra.AhuName
                                                  }).ToList(),
                                    RequestRooms = (from rr in _context.RequestRooms
                                                    where rr.QcRequestId == p.Id
                                                    && rr.RowStatus == null
                                                    select new RequestRoomViewModel
                                                    {
                                                        Id = rr.Id,
                                                        RoomId = rr.RoomId,
                                                        RoomCode = rr.RoomCode,
                                                        RoomName = rr.RoomName,
                                                        GradeRoomId = rr.GradeRoomId,
                                                        GradeRoomCode = rr.GradeRoomCode,
                                                        GradeRoomName = rr.GradeRoomName,
                                                        TestScenarioId = rr.TestScenarioId,
                                                        TestScenarioName = rr.TestScenarioName,
                                                        TestScenarioLabel = rr.TestScenarioLabel,
                                                        AhuId = rr.AhuId,
                                                        AhuCode = rr.AhuCode,
                                                        AhuName = rr.AhuName,
                                                    }).ToList(),
                                    RequestPurposes = (from rp in _context.RequestPurposes
                                                       where rp.QcRequestId == p.Id
                                                       && rp.RowStatus == null
                                                       orderby rp.Id
                                                       select new RequestPurposesViewModel
                                                       {
                                                           Id = rp.Id,
                                                           PurposeId = rp.PurposeId,
                                                           PurposeCode = rp.PurposeCode,
                                                           PurposeName = rp.PurposeName
                                                       }).ToList(),
                                    // WorkFlowHistory = (from wh in _context.WorkflowHistories
                                    //                    join per in _context.Personals on wh.PersonalId equals per.Id
                                    //                    join ps in _context.Positions on per.PosId equals ps.Id
                                    //                    where wh.GroupMenuName == ApplicationConstant.GROUP_NAME_REQUEST_QCS
                                    //                    && wh.GroupMenuId == p.Id && wh.RowStatus == null
                                    //                    orderby wh.CreatedAt descending
                                    //                    select new WorkflowHistoryViewModel
                                    //                    {
                                    //                        Id = wh.Id,
                                    //                        Action = wh.Action,
                                    //                        Note = wh.Note,
                                    //                        GroupMenuId = wh.GroupMenuId,
                                    //                        GroupMenuName = wh.GroupMenuName,
                                    //                        PersonalName = per.Name,
                                    //                        PersonalNik = per.Nik,
                                    //                        Position = ps.Name,
                                    //                        CreatedAt = wh.CreatedAt
                                    //                    }).ToList(),
                                    TestTypeQcs = (from t in _context.ProductTestTypeQcs
                                                   join tps in _context.TestParameters on t.TestParameterId equals tps.Id into tempParam
                                                   from jm in tempParam.DefaultIfEmpty()
                                                   where t.RequestQcsId == p.Id
                                                   && t.RowStatus == null
                                                   select new TestTypeQcsViewModel
                                                   {
                                                       Id = t.Id,
                                                       OrgId = t.OrgId,
                                                       OrgName = t.OrgName,
                                                       RequestQcsId = t.RequestQcsId,
                                                       TestTypeId = (t.TestTypeId == 0 ? 0 : t.TestTypeId),
                                                       TestTypeName = t.TestTypeName,
                                                       TestParameterId = (t.TestParameterId == 0 ? 0 : t.TestParameterId),
                                                       TestParameterSequence = jm.Sequence,
                                                       TestParameterName = t.TestParameterName,
                                                       SampleAmountCount = t.SampleAmountCount,
                                                       SampleAmountVolume = t.SampleAmountVolume,
                                                       SampleAmountUnit = t.SampleAmountUnit
                                                   }).OrderBy(x => x.TestParameterSequence).ToList(),
                                    CreatedBy = p.CreatedBy,
                                    CreatedAt = p.CreatedAt,
                                    IsFromBulkRequest = p.IsFromBulkRequest

                                }).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();

            var batch = await _transactionBatchDataProvider.GetByRequestId(result.Id);

            if (batch == null) return result;

            result.Batch = batch;

            return result;
        }
        public async Task<List<QcRequestSamplingGenerateViewModel>> getSamplingRequest(int RequestTypeId)
        {
            var checkData = await (from tpr in _context.RequestSamplings
                                   join st in _context.QcSamplingTypes on tpr.TypeSamplingId equals st.Id
                                   where tpr.TypeRequestId == RequestTypeId
                                   select new QcRequestSamplingGenerateViewModel
                                   {
                                       TypeRequestId = tpr.TypeRequestId,
                                       TypeSamplingId = tpr.TypeSamplingId,
                                       RequestSamplingName = st.Name,
                                       RequestSamplingLabel = st.Code
                                   }).ToListAsync();

            return checkData;
        }


        public async Task<QcSampling> generateSamplingQcAlt(int TypeSamplingId, string RequestSamplingName, RequestQcs data, List<TestTypeQcs> dataTestType)
        {
            // insert sampling data
            QcSampling insertDataSampling = new QcSampling()
            {
                RequestQcsId = data.Id,
                Code = generateCodeSample(6),//TODO 6 pindah ke consant
                SamplingTypeId = TypeSamplingId,
                SamplingTypeName = RequestSamplingName,
                Status = 0,
                CreatedBy = data.CreatedBy,
                CreatedAt = data.CreatedAt,
                UpdatedBy = data.UpdatedBy,
                UpdatedAt = data.UpdatedAt
            };

            await _context.QcSamplings.AddAsync(insertDataSampling);
            await _context.SaveChangesAsync();

            // init sample data
            List<QcSample> insertSampleData = new List<QcSample>();

            // get request room
            var getRequestRoom = await (from rr in _context.RequestRooms
                                        where rr.QcRequestId == data.Id && rr.RowStatus == null
                                        select rr).ToListAsync();

            // get request purposes
            var getRequestPurpose = await (from rp in _context.RequestPurposes
                                           where rp.QcRequestId == data.Id && rp.RowStatus == null
                                           select rp.PurposeId).ToListAsync();

            //get dataSample berdasarkan sample point dari tool yg ada di room
            var roomIds = getRequestRoom.Select(x => x.RoomId).Distinct().ToList();
            var samplingPointsByTools = await GetSamplePointTestParamByToolsInRooms(roomIds, data.TestScenarioLabel, getRequestPurpose);

            //filter sesuai TypeSamplingId(EM atau PC)
            samplingPointsByTools = samplingPointsByTools.Where(x => x.TestGroupId == TypeSamplingId).ToList();

            if (getRequestRoom.Any())
            {
                // looping request room
                foreach (var requestRoom in getRequestRoom)
                {
                    //get total sampling request per test parameter
                    //looping test type for get parameter
                    foreach (var testType in dataTestType)
                    {
                        //mengurangi jumlah sample sesuai sample point dari tool supaya tidak menimbulkan left over sample. Hal ini dikarenakan SampleAmountCount dari FE nya sudah ditambahkan dengan sample point dari tool terlebih dahulu
                        int amountSamplingPointFromTools = samplingPointsByTools
                            .Where(x => x.RoomId == requestRoom.RoomId && x.TestParameterId == testType.TestParameterId)
                            .Select(x => x.SamplePointId)
                            .Distinct()
                            .Count();
                        testType.SampleAmountCount -= amountSamplingPointFromTools;

                        // check sample porint per room or grade room
                        /*List<VSamplePointTestParam> getSampleTest = (from stp in _context.VSamplePointTestParams
                                                                        where stp.RoomId == requestRoom.RoomId &&
                                                                            stp.TestParameterId == testType.TestParameterId &&
                                                                            stp.TestScenarioId == requestRoom.TestScenarioId
                                                                        select stp).ToList();*/

                        // kika sedang kerjakan
                        // get sample test 
                        //var getSampleTest = await getSamplePointTestParameter(requestRoom.RoomId, testType.TestParameterId, data.TestScenarioLabel, getRequestPurpose);
                        var getSampleTest = await getSamplePointTestParameterByRequestId(data.Id, testType.TestParameterId, data.TestScenarioLabel, requestRoom.RoomId);
                        //data.Id request id
                        // get count sample point
                        //var getCountSamplePoint = await getSamplePointTestParameterCount(requestRoom.RoomId, testType.TestParameterId, data.TestScenarioLabel, getRequestPurpose);
                        var getCountSamplePoint = await getSamplePointTestParameterCountByRequestId(data.Id, testType.TestParameterId, data.TestScenarioLabel);

                        var CountLimitSample = 0;
                        var SequencePC05 = 1;
                        var SequencePC50 = 1;
                        // looping sample for generate
                        foreach (var SampleTest in getSampleTest)
                        {
                            //Generate Sampling Test Parameter PC -- OK
                            #region generate sample PC
                            if (SampleTest.TestGroupId == ApplicationConstant.REQUEST_SAPMLING_PC && TypeSamplingId == ApplicationConstant.REQUEST_SAPMLING_PC)
                            {

                                if (CountLimitSample > getCountSamplePoint)
                                {
                                    break;
                                };

                                var sequenceParam = 0;
                                if (SampleTest.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC05)
                                {
                                    sequenceParam = SequencePC05;
                                    SequencePC05++;
                                }
                                else if (SampleTest.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC50)
                                {
                                    sequenceParam = SequencePC50;
                                    SequencePC50++;
                                }

                                QcSample dataSample = new QcSample()
                                {
                                    Code = generateCodeSample(6) + "-" + await checkScenarioCode(requestRoom.TestScenarioId),//TODO 6 pindahkan ke constant  
                                    QcSamplingId = insertDataSampling.Id,
                                    SamplingPointId = SampleTest.SamplePointId,
                                    SamplingPointCode = SampleTest.SamplePointName,
                                    TestParamId = SampleTest.TestParameterId,
                                    TestParamName = SampleTest.TestParameterName,
                                    GradeRoomId = SampleTest.GradeRoomId,
                                    GradeRoomName = SampleTest.GradeRoomName,
                                    SampleSequence = sequenceParam,
                                    CreatedBy = data.CreatedBy,
                                    CreatedAt = DateHelper.Now(),
                                    UpdatedBy = data.CreatedBy,
                                    UpdatedAt = DateHelper.Now(),
                                    IsDefault = true,
                                    TestScenarioId = SampleTest.TestScenarioId
                                };
                                insertSampleData.Add(dataSample);
                            }
                            #endregion

                            //Generate Sampling Test Parameter EM-M -- OK
                            #region generate sample EMM
                            if (SampleTest.TestGroupId == ApplicationConstant.REQUEST_SAPMLING_EMM && TypeSamplingId == ApplicationConstant.REQUEST_SAPMLING_EMM)
                            {
                                if (CountLimitSample > getCountSamplePoint)
                                {
                                    break;
                                };

                                QcSample dataSample = new QcSample()
                                {
                                    Code = generateCodeSample(6) + "-" + await checkScenarioCode(requestRoom.TestScenarioId),//TODO 6 pindahkan ke constant
                                    QcSamplingId = insertDataSampling.Id,
                                    SamplingPointId = SampleTest.SamplePointId,
                                    SamplingPointCode = SampleTest.SamplePointName,
                                    TestParamId = SampleTest.TestParameterId,
                                    TestParamName = SampleTest.TestParameterName,
                                    GradeRoomId = SampleTest.GradeRoomId,
                                    GradeRoomName = SampleTest.GradeRoomName,
                                    CreatedBy = data.CreatedBy,
                                    CreatedAt = DateHelper.Now(),
                                    UpdatedBy = data.CreatedBy,
                                    UpdatedAt = DateHelper.Now(),
                                    IsDefault = true,
                                    TestScenarioId = SampleTest.TestScenarioId
                                };
                                insertSampleData.Add(dataSample);
                            }
                            #endregion

                            CountLimitSample++;

                        }

                        #region left over sample generator
                        if (testType.TestParameterId != ApplicationConstant.TEST_PARAMETER_FD)
                        {
                            // sample count
                            if (getRequestRoom.Count() == 1)
                            {

                                if (TypeSamplingId == ApplicationConstant.REQUEST_SAPMLING_EMM &&
                                (testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_SP ||
                                    testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_AS ||
                                    testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_CA ||
                                    testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_GV
                                ))
                                {
                                    var SampleLeftOver = testType.SampleAmountCount - CountLimitSample;
                                    if (SampleLeftOver > 0)
                                    {
                                        for (int samp = 0; samp < SampleLeftOver; samp++)
                                        {
                                            QcSample dataSampleLeftOver = new QcSample()
                                            {
                                                //Code = generateCodeSample(6) + "-" + await checkScenarioCode(requestRoom.TestScenarioId),//TODO 6 pindahkan ke constant
                                                Code = generateCodeSample(6),//TODO 6 pindahkan ke constant
                                                QcSamplingId = insertDataSampling.Id,
                                                SamplingPointId = null,
                                                SamplingPointCode = null,
                                                TestParamId = testType.TestParameterId,
                                                TestParamName = testType.TestParameterName,
                                                GradeRoomId = null,
                                                GradeRoomName = null,
                                                CreatedBy = data.CreatedBy,
                                                CreatedAt = DateHelper.Now(),
                                                UpdatedBy = data.CreatedBy,
                                                UpdatedAt = DateHelper.Now(),
                                                IsDefault = true,
                                                TestScenarioId = null
                                            };
                                            insertSampleData.Add(dataSampleLeftOver);
                                        }
                                    }
                                }


                                if (TypeSamplingId == ApplicationConstant.REQUEST_SAPMLING_PC &&
                                (testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC05 ||
                                    testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC50
                                ))
                                {
                                    var SampleLeftOver = testType.SampleAmountCount - CountLimitSample;
                                    if (SampleLeftOver > 0)
                                    {
                                        for (int samp = 0; samp < SampleLeftOver; samp++)
                                        {

                                            var sequenceParam = 0;
                                            if (testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC05)
                                            {
                                                sequenceParam = SequencePC05;
                                                SequencePC05++;
                                            }
                                            else if (testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC50)
                                            {
                                                sequenceParam = SequencePC50;
                                                SequencePC50++;
                                            }

                                            QcSample dataSampleLeftOver = new QcSample()
                                            {
                                                //Code = generateCodeSample(6) + "-" + await checkScenarioCode(requestRoom.TestScenarioId),//TODO 6 pindahkan ke constant
                                                Code = generateCodeSample(6),//TODO 6 pindahkan ke constant
                                                QcSamplingId = insertDataSampling.Id,
                                                SamplingPointId = null,
                                                SamplingPointCode = null,
                                                TestParamId = testType.TestParameterId,
                                                TestParamName = testType.TestParameterName,
                                                GradeRoomId = null,
                                                GradeRoomName = null,
                                                SampleSequence = sequenceParam,
                                                CreatedBy = data.CreatedBy,
                                                CreatedAt = DateHelper.Now(),
                                                UpdatedBy = data.CreatedBy,
                                                UpdatedAt = DateHelper.Now(),
                                                IsDefault = true,
                                                TestScenarioId = null
                                            };
                                            insertSampleData.Add(dataSampleLeftOver);
                                        }
                                    }
                                }
                            }


                        }
                        #endregion

                    }

                }
            }

            foreach (var testType in dataTestType)
            {
                //Generate Test Parameter FD -- OK
                #region test finger DAB
                if (testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_FD && TypeSamplingId == ApplicationConstant.REQUEST_SAPMLING_EMM)
                {

                    if (testType.SampleAmountCount > 0)
                    {
                        var NoFD = 1;
                        for (int fd = 0; fd < testType.SampleAmountCount; fd++)
                        {
                            //Insert Data FD Left
                            QcSample dataSampleFDLeft = new QcSample()
                            {
                                Code = generateCodeSample(6),//TODO 6 pindahkan ke constant
                                QcSamplingId = insertDataSampling.Id,
                                SamplingPointId = null,
                                SamplingPointCode = "FDL-" + NoFD,
                                TestParamId = testType.TestParameterId,
                                TestParamName = testType.TestParameterName,
                                GradeRoomId = null,
                                GradeRoomName = null,
                                CreatedBy = data.CreatedBy,
                                CreatedAt = DateHelper.Now(),
                                UpdatedBy = data.CreatedBy,
                                UpdatedAt = DateHelper.Now(),
                                IsDefault = true,
                                TestScenarioId = null
                            };
                            insertSampleData.Add(dataSampleFDLeft);

                            //Insert Data FD Right
                            QcSample dataSampleFDRight = new QcSample()
                            {
                                Code = generateCodeSample(6),//TODO 6 pindahkan ke constant
                                QcSamplingId = insertDataSampling.Id,
                                SamplingPointId = null,
                                SamplingPointCode = "FDR-" + NoFD,
                                TestParamId = testType.TestParameterId,
                                TestParamName = testType.TestParameterName,
                                GradeRoomId = null,
                                GradeRoomName = null,
                                CreatedBy = data.CreatedBy,
                                CreatedAt = DateHelper.Now(),
                                UpdatedBy = data.CreatedBy,
                                UpdatedAt = DateHelper.Now(),
                                IsDefault = true,
                                TestScenarioId = null
                            };
                            insertSampleData.Add(dataSampleFDRight);

                            NoFD++;
                        }

                    }
                }
                #endregion
            }

            #region add dataSample berdasarkan sample point dari tool yg ada di room
            foreach (var item in samplingPointsByTools)
            {
                string code = generateCodeSample(6);
                if (item.TestGroupId == ApplicationConstant.REQUEST_SAPMLING_EMM || item.TestGroupId == ApplicationConstant.REQUEST_SAPMLING_PC)
                    code += "-" + await checkScenarioCode(item.TestScenarioId); //TODO 6 pindahkan ke constant

                QcSample dataSample = new QcSample()
                {
                    Code = code,
                    QcSamplingId = insertDataSampling.Id,
                    SamplingPointId = item.SamplePointId,
                    SamplingPointCode = item.SamplePointName,
                    TestParamId = item.TestParameterId,
                    TestParamName = item.TestParameterName,
                    GradeRoomId = item.GradeRoomId,
                    GradeRoomName = item.GradeRoomName,
                    CreatedBy = data.CreatedBy,
                    CreatedAt = DateHelper.Now(),
                    UpdatedBy = data.CreatedBy,
                    UpdatedAt = DateHelper.Now(),
                    IsDefault = true,
                    TestScenarioId = item.TestScenarioId
                };
                insertSampleData.Add(dataSample);
            }
            #endregion

            //save sample data generate
            if (insertSampleData.Any())
            {
                await _context.QcSamples.AddRangeAsync(insertSampleData);
            }

            await _context.SaveChangesAsync();



            return insertDataSampling;
        }
        public string generateCodeSample(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }
        public async Task<int> checkScenarioCode(int? TestScenarioId)
        {
            var returnCode = ApplicationConstant.TEST_SCENARIO_CODE_UNDEFINED;

            var getScenario = await (from sc in _context.TransactionTestScenario
                                     where sc.Id == TestScenarioId
                                     && sc.RowStatus == null
                                     select sc).FirstOrDefaultAsync();
            Console.WriteLine(getScenario);
            if (getScenario != null)
            {
                if (getScenario.Label == ApplicationConstant.TEST_SCENARIO_LABEL_AT_REST || getScenario.Label == ApplicationConstant.TEST_SCENARIO_LABELALT_AT_REST)
                {
                    returnCode = ApplicationConstant.TEST_SCENARIO_CODE_AT_REST;
                }
                else if (getScenario.Label == ApplicationConstant.TEST_SCENARIO_LABEL_IN_OPERATIONS || getScenario.Label == ApplicationConstant.TEST_SCENARIO_LABELALT_IN_OPERATIONS)
                {
                    returnCode = ApplicationConstant.TEST_SCENARIO_CODE_IN_OPERATIONS;
                }
            }

            return returnCode;
        }

        public async Task<ResponseViewModel<InsertEditDev>> UpdateDeviation(InsertEditDev data)
        {
            ResponseViewModel<InsertEditDev> result = new ResponseViewModel<InsertEditDev>();
            var findDuplicate = _context.RequestQcs.FirstOrDefault(x => x.NoDeviation == data.DevNumber);

            if (findDuplicate == null)
            {
                var course = _context.RequestQcs.FirstOrDefault(x => x.Id == data.QcRequestId);

                if (course != null)
                {
                    course.NoDeviation = data.DevNumber;
                    course.UpdatedBy = data.UpdatedBy;
                    course.UpdatedAt = DateHelper.Now();
                    await _context.SaveChangesAsync();

                    result.StatusCode = 200;
                    result.Message = ApplicationConstant.MESSAGE_EDIT_DEVIASI_SUCCESS;
                }
                else
                {
                    result.StatusCode = 500;
                    result.Message = ApplicationConstant.MESSAGE_EDIT_DEVIASI_NOT_SUCCESS;
                }
            }
            else
            {
                result.StatusCode = 403;
                result.Message = ApplicationConstant.MESSAGE_EDIT_DEVIASI_DUPLICATE;
            }

            return result;

        }
        public async Task<RequestQcs> GetByBatchAndPhaseId(string batchNumber, int phaseId)
        {
            var batchNumberTemp = batchNumber.ToUpper();
            var exceptStatusList = new List<int>()
            {
                ApplicationConstant.STATUS_DRAFT
            };
            return await _context.RequestQcs
                .FirstOrDefaultAsync(x => x.NoBatch.ToUpper().Equals(batchNumberTemp)
                                          && x.EmPhaseId == phaseId
                                          && !exceptStatusList.Contains(x.Status));
        }

        public async Task<bool> UpdateConclusion(InsertEditConclusion data)
        {
            var course = _context.RequestQcs.FirstOrDefault(x => x.Id == data.QcRequestId);

            if (course != null)
            {
                course.Conclusion = data.Conclusion;
                course.UpdatedBy = data.UpdatedBy;
                course.UpdatedAt = DateHelper.Now();
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateReceiptDate(int id, bool isQA)
        {
            var data = _context.RequestQcs.FirstOrDefault(x => x.Id == id);
            if (data != null)
            {
                data.ReceiptDate = DateHelper.Now();
                if (isQA)
                {
                    data.ReceiptDateQA = DateHelper.Now();
                }
                else
                {
                    data.ReceiptDateKabag = DateHelper.Now();
                }
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateIsNoBatchEditable(int id, bool isNoBatchEditable)
        {
            var data = _context.RequestQcs.FirstOrDefault(x => x.Id == id);
            if (data == null) return false;
            
            data.IsNoBatchEditable = isNoBatchEditable;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GenerateUpdateConclusion(int requestId)
        {
            var conclusionIsPass = true;

            try
            {
                //get Request Qc
                var requestQc = await (from r in _context.RequestQcs
                                       where r.RowStatus == null
                                             && r.Id == requestId
                                       select r).FirstOrDefaultAsync();

                if (requestQc == null)
                {
                    throw new Exception($"QC Request {requestId} tidak terdadftar");
                }

                var samplingIds = await (from s in _context.QcSamplings
                                         where s.RowStatus == null
                                               && s.RequestQcsId == requestId
                                         select s.Id).ToListAsync();

                if (!samplingIds.Any())
                {
                    throw new Exception($"Daftar sampling untuk request id {requestId} kosong");
                }

                var sampleIds = await (from sm in _context.QcSamples
                                       where sm.RowStatus == null
                                             && sm.ParentId == null
                                             && samplingIds.Contains(sm.QcSamplingId)
                                       select sm.Id).ToListAsync();

                if (!sampleIds.Any())
                {
                    throw new Exception($"Daftar sample untuk request id {requestId} kosong");
                }

                var qcResultFailed = await (from res in _context.QcResults
                                            where res.RowStatus == null
                                                  && sampleIds.Contains(res.SampleId)
                                                  && res.TestVariableConclusion == ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL
                                            select res)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();


                requestQc.Conclusion = ApplicationConstant.MSG_CONCLUSION_PASS;

                if (qcResultFailed != null)
                {
                    requestQc.Conclusion = ApplicationConstant.MSG_CONCLUSION_NOT_PASS;
                    conclusionIsPass = false;
                }

                requestQc.UpdatedAt = DateHelper.Now();

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                conclusionIsPass = false;
                _logger.LogError(e, "{Message}", e.Message);
            }

            return conclusionIsPass;
        }

        public async Task<List<string>> getRequestPurposeNames(int requestId)
        {
            var result = await (from rp in _context.RequestPurposes
                                where rp.QcRequestId == requestId
                                && rp.RowStatus == null
                                select rp.PurposeName).ToListAsync();

            return result;
        }

        public async Task<List<QcResult>> findRequestNotAllPass(int qcRequestId)
        {
            var result = await (from qr in _context.QcResults
                                join qs in _context.QcSamples on qr.SampleId equals qs.Id
                                join qsing in _context.QcSamplings on qs.QcSamplingId equals qsing.Id
                                where qsing.RequestQcsId == qcRequestId &&
                                qr.TestVariableConclusion != ApplicationConstant.TEST_VARIABLE_CONCLUSION_PASS
                                select qr
                        ).ToListAsync();
            return result;
        }

        public async Task<List<TestScenarioViewModel>> GetTestScenarioById(int id)
        {
            var result = await (from rr in _context.RequestRooms
                                join gr in _context.GradeRooms on rr.GradeRoomId equals gr.Id
                                join rgrs in _context.RelGradeRoomScenarios on rr.GradeRoomId equals rgrs.GradeRoomId
                                join ts in _context.TestScenarios on rgrs.TestScenarioId equals ts.Id
                                where rr.QcRequestId == id
                                && rr.RowStatus == null
                                && gr.RowStatus == null
                                group new { rr, gr, ts } by new { rr.GradeRoomId, ts.Id } into g
                                select new TestScenarioViewModel
                                {
                                    Id = g.Max(x => x.ts.Id),
                                    Label = g.Max(x => x.ts.Label),
                                    Name = g.Max(x => x.ts.Name),
                                    GradeRoomId = g.Max(x => x.rr.GradeRoomId),
                                    GradeRoomCode = g.Max(x => x.gr.Code),
                                    GradeRoomName = g.Max(x => x.gr.Name),
                                }).ToListAsync();

            return result;
        }

        public async Task<List<VSamplePointTestParam>> _getSamplePointTestParameter(int roomId, int TestParameterId, string testScenarioLabel)
        {
            var result = await (from rrsp in _context.RelRoomSamplings
                                join room_purp in _context.RoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                join r in _context.Rooms on room_purp.RoomId equals r.Id
                                join gr in _context.GradeRooms on r.GradeRoomId equals gr.Id
                                join sp in _context.SamplingPoints on rrsp.SamplingPointId equals sp.Id
                                join rstp in _context.RelSamplingTestParams on sp.Id equals rstp.SamplingPointId
                                join rtsp in _context.RelTestScenarioParams on rstp.TestScenarioParamId equals rtsp.Id
                                join ts in _context.TestScenarios on rtsp.TestScenarioId equals ts.Id
                                join tp in _context.TestParameters on rtsp.TestParameterId equals tp.Id
                                select new VSamplePointTestParam
                                {
                                    RoomId = room_purp.RoomId,
                                    ToolId = null,
                                    ToolName = null,
                                    GradeRoomId = r.GradeRoomId,
                                    GradeRoomName = gr.Name,
                                    SamplePointId = sp.Id,
                                    SamplePointName = sp.Code,
                                    TestScenarioId = ts.Id,
                                    TestScenarioName = ts.Name,
                                    TestScenarioLabel = ts.Label,
                                    TestParameterId = tp.Id,
                                    TestParameterName = tp.Name,
                                    TestGroupId = tp.TestGroupId,
                                    Seq = tp.Sequence
                                }).Union(
                                    from t in _context.Tools
                                    join r in _context.Rooms on t.RoomId equals r.Id
                                    join gr in _context.GradeRooms on t.GradeRoomId equals gr.Id
                                    join tps in _context.ToolPurposes on t.Id equals tps.ToolId
                                    join rst in _context.RelSamplingTools on tps.Id equals rst.ToolPurposeId
                                    join sp in _context.SamplingPoints on rst.SamplingPointId equals sp.Id
                                    join rstp in _context.RelSamplingTestParams on sp.Id equals rstp.SamplingPointId
                                    join rtsp in _context.RelTestScenarioParams on rstp.TestScenarioParamId equals rtsp.Id
                                    join ts in _context.TestScenarios on rtsp.TestScenarioId equals ts.Id
                                    join tp in _context.TestParameters on rtsp.TestParameterId equals tp.Id
                                    select new VSamplePointTestParam
                                    {
                                        RoomId = t.RoomId,
                                        ToolId = t.Id,
                                        ToolName = t.Name,
                                        GradeRoomId = gr.Id,
                                        GradeRoomName = gr.Name,
                                        SamplePointId = sp.Id,
                                        SamplePointName = sp.Code,
                                        TestScenarioId = ts.Id,
                                        TestScenarioName = ts.Name,
                                        TestScenarioLabel = ts.Label,
                                        TestParameterId = tp.Id,
                                        TestParameterName = tp.Name,
                                        TestGroupId = tp.TestGroupId,
                                        Seq = tp.Sequence
                                    }).Where(x => x.RoomId == roomId && x.TestParameterId == TestParameterId && x.TestScenarioLabel == testScenarioLabel)
                                    .OrderBy(x => x.Seq).
                                    ToListAsync();

            return result;

        }

        public async Task<List<VSamplePointTestParam>> getSamplePointTestParameter(int roomId, int TestParameterId, string testScenarioLabel, List<int> purposeId)
        {
            List<int> getGroupToolPurposes = await (from rptg in _context.RelSamplingPurposeToolGroups
                                                    where rptg.RowStatus == null
                                                    && purposeId.Contains(rptg.PurposeId)
                                                    select rptg.ToolGroupId).ToListAsync();

            //menyeleksi data sampling point berdasarkan purpose nya, 
            List<int> getPurposeIdByPurposeRoom = await (from room_purp in _context.RoomPurpose
                                                         join room_purp_master in _context.RoomPurposeToMasterPurposes on room_purp.Id equals room_purp_master.RoomPurposeId
                                                         where room_purp.RowStatus == null
                                                         && room_purp.RoomId == roomId
                                                         && purposeId.Contains(room_purp_master.PurposeId)
                                                         select room_purp.Id).ToListAsync();

            var getCountPurposePerRoomPurpose = await (from room_purp_2 in _context.RoomPurpose
                                                       join room_purp_to_master in _context.RoomPurposeToMasterPurposes on room_purp_2.Id equals room_purp_to_master.RoomPurposeId
                                                       where room_purp_2.RoomId == roomId
                                                       && purposeId.Contains(room_purp_to_master.PurposeId)
                                                       group room_purp_2 by room_purp_2.Id into newGroup
                                                       select new
                                                       {
                                                           roomPurposeId = newGroup.Key,
                                                           countPurpose = newGroup.Count()
                                                       }).FirstOrDefaultAsync(x => x.countPurpose == purposeId.Count());

            var result = await (from rrsp in _context.RelRoomSamplings
                                join room_purp in _context.RoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                join r in _context.Rooms on room_purp.RoomId equals r.Id
                                join gr in _context.GradeRooms on r.GradeRoomId equals gr.Id
                                join sp in _context.SamplingPoints on rrsp.SamplingPointId equals sp.Id
                                join rstp in _context.RelSamplingTestParams on sp.Id equals rstp.SamplingPointId
                                join rtsp in _context.RelTestScenarioParams on rstp.TestScenarioParamId equals rtsp.Id
                                join ts in _context.TestScenarios on rtsp.TestScenarioId equals ts.Id
                                join rgrs in _context.RelGradeRoomScenarios on new { rtsp.TestScenarioId, r.GradeRoomId } equals new { rgrs.TestScenarioId, rgrs.GradeRoomId }
                                join tp in _context.TestParameters on rtsp.TestParameterId equals tp.Id
                                where rrsp.RoomPurposeId == getCountPurposePerRoomPurpose.roomPurposeId
                                select new VSamplePointTestParam
                                {
                                    RoomId = room_purp.RoomId,
                                    ToolId = null,
                                    ToolName = null,
                                    GradeRoomId = r.GradeRoomId,
                                    GradeRoomName = gr.Name,
                                    SamplePointId = sp.Id,
                                    SamplePointName = sp.Code,
                                    TestScenarioId = ts.Id,
                                    TestScenarioName = ts.Name,
                                    TestScenarioLabel = ts.Label,
                                    TestParameterId = tp.Id,
                                    TestParameterName = tp.Name,
                                    TestGroupId = tp.TestGroupId,
                                    Seq = tp.Sequence
                                }).Union(
                                    from t in _context.Tools
                                    join r in _context.Rooms on t.RoomId equals r.Id
                                    join gr in _context.GradeRooms on t.GradeRoomId equals gr.Id
                                    join tps in _context.ToolPurposes on t.Id equals tps.ToolId
                                    join rst in _context.RelSamplingTools on tps.Id equals rst.ToolPurposeId
                                    join sp in _context.SamplingPoints on rst.SamplingPointId equals sp.Id
                                    join rstp in _context.RelSamplingTestParams on sp.Id equals rstp.SamplingPointId
                                    join rtsp in _context.RelTestScenarioParams on rstp.TestScenarioParamId equals rtsp.Id
                                    join ts in _context.TestScenarios on rtsp.TestScenarioId equals ts.Id
                                    join rgrs in _context.RelGradeRoomScenarios on new { rtsp.TestScenarioId, r.GradeRoomId } equals new { rgrs.TestScenarioId, rgrs.GradeRoomId }
                                    join tp in _context.TestParameters on rtsp.TestParameterId equals tp.Id
                                    where getGroupToolPurposes.Contains(t.ToolGroupId)
                                    select new VSamplePointTestParam
                                    {
                                        RoomId = t.RoomId,
                                        ToolId = t.Id,
                                        ToolName = t.Name,
                                        GradeRoomId = gr.Id,
                                        GradeRoomName = gr.Name,
                                        SamplePointId = sp.Id,
                                        SamplePointName = sp.Code,
                                        TestScenarioId = ts.Id,
                                        TestScenarioName = ts.Name,
                                        TestScenarioLabel = ts.Label,
                                        TestParameterId = tp.Id,
                                        TestParameterName = tp.Name,
                                        TestGroupId = tp.TestGroupId,
                                        Seq = tp.Sequence
                                    }).Where(x => x.RoomId == roomId && x.TestParameterId == TestParameterId && x.TestScenarioLabel == testScenarioLabel)
                                    .OrderBy(x => x.Seq).
                                    ToListAsync();

            return result;

        }

        public async Task<List<VSamplePointTestParam>> getSamplePointTestParameterByRequestId(int requestId, int TestParameterId, string testScenarioLabel, int roomId)
        {
            var result = new List<VSamplePointTestParam>();
            var getRoomPurpose = await (from rq in _context.RequestQcs
                                        join rr in _context.RequestGroupRoomPurpose on rq.Id equals rr.RequestQcsId
                                        join rp in _context.TransactionRoomPurpose on rr.RoomPurposeId equals rp.Id
                                        where rr.RequestQcsId == requestId && rp.RoomId == roomId
                                        select rr.RoomPurposeId).ToListAsync();

            // var getToolPurpose = await (from rq in _context.RequestQcs
            //                             join rt in _context.RequestGroupToolPurpose on rq.Id equals rt.RequestQcsId
            //                             where rt.RequestQcsId == requestId
            //                             select rt).ToListAsync();

            var roomSamplePointTestParam = await (from rrsp in _context.TransactionRelRoomSamplingPoint
                                                  join room_purp in _context.TransactionRoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                  join r in _context.TransactionRoom on room_purp.RoomId equals r.Id
                                                  join gr in _context.TransactionGradeRoom on r.GradeRoomId equals gr.Id
                                                  join sp in _context.TransactionSamplingPoint on rrsp.SamplingPoinId equals sp.Id
                                                  join rstp in _context.TransactionRelSamplingTestParam on sp.Id equals rstp.SamplingPointId
                                                  join rtsp in _context.TransactionRelTestScenarioParam on rstp.TestScenarioParamId equals rtsp.Id
                                                  join ts in _context.TransactionTestScenario on rtsp.TestScenarioId equals ts.Id
                                                  join rgrs in _context.TransactionRelGradeRoomScenario on new { rtsp.TestScenarioId, r.GradeRoomId } equals new { rgrs.TestScenarioId, rgrs.GradeRoomId }
                                                  join tp in _context.TransactionTestParameter on rtsp.TestParameterId equals tp.Id
                                                  where getRoomPurpose.Contains(rrsp.RoomPurposeId)
                                                  && rrsp.ScenarioLabel == testScenarioLabel
                                                  select new VSamplePointTestParam
                                                  {
                                                      RoomId = room_purp.RoomId,
                                                      ToolId = null,
                                                      ToolName = null,
                                                      GradeRoomId = r.GradeRoomId,
                                                      GradeRoomName = gr.Name,
                                                      SamplePointId = sp.Id,
                                                      SamplePointName = sp.Code,
                                                      TestScenarioId = ts.Id,
                                                      TestScenarioName = ts.Name,
                                                      TestScenarioLabel = ts.Label,
                                                      TestParameterId = tp.Id,
                                                      TestParameterName = tp.Name,
                                                      TestGroupId = tp.TestGroupId,
                                                      Seq = tp.Sequence
                                                  }).Distinct().Where(x => x.TestParameterId == TestParameterId && x.TestScenarioLabel == testScenarioLabel).ToListAsync();

            // var toolSamplePointTestParam = await (
            //                                     from t in _context.TransactionTool
            //                                     join r in _context.TransactionRoom on t.RoomId equals r.Id
            //                                     join gr in _context.TransactionGradeRoom on t.GradeRoomId equals gr.Id
            //                                     join tps in _context.TransactionToolPurpose on t.Id equals tps.ToolId
            //                                     join rst in _context.TransactionRelSamplingTool on tps.Id equals rst.ToolPurposeId
            //                                     join sp in _context.TransactionSamplingPoint on rst.SamplingPoinId equals sp.Id
            //                                     join rstp in _context.TransactionRelSamplingTestParam on sp.Id equals rstp.SamplingPointId
            //                                     join rtsp in _context.TransactionRelTestScenarioParam on rstp.TestScenarioParamId equals rtsp.Id
            //                                     join ts in _context.TransactionTestScenario on rtsp.TestScenarioId equals ts.Id
            //                                     join rgrs in _context.TransactionRelGradeRoomScenario on new { rtsp.TestScenarioId, r.GradeRoomId } equals new { rgrs.TestScenarioId, rgrs.GradeRoomId }
            //                                     join tp in _context.TransactionTestParameter on rtsp.TestParameterId equals tp.Id
            //                                     //where getGroupToolPurposes.Contains(t.ToolGroupId)
            //                                     where getToolPurpose.Select(x => x.ToolPurposeId).ToList().Contains(tps.Id)
            //                                     && rst.ScenarioLabel == testScenarioLabel
            //                                     select new VSamplePointTestParam
            //                                     {
            //                                         RoomId = t.RoomId.Value,
            //                                         ToolId = t.Id,
            //                                         ToolName = t.Name,
            //                                         GradeRoomId = gr.Id,
            //                                         GradeRoomName = gr.Name,
            //                                         SamplePointId = sp.Id,
            //                                         SamplePointName = sp.Code,
            //                                         TestScenarioId = ts.Id,
            //                                         TestScenarioName = ts.Name,
            //                                         TestScenarioLabel = ts.Label,
            //                                         TestParameterId = tp.Id,
            //                                         TestParameterName = tp.Name,
            //                                         TestGroupId = tp.TestGroupId,
            //                                         Seq = tp.Sequence

            //                                     }).Where(x => x.TestParameterId == TestParameterId && x.TestScenarioLabel == testScenarioLabel).ToListAsync();

            //roomSamplePointTestParam.AddRange(toolSamplePointTestParam);
            var dataSample = roomSamplePointTestParam.OrderBy(x => x.Seq).ToList();
            result.AddRange(dataSample);

            return result;

        }

        /// <summary>
        /// Get VSamplePointTestParam berdasarkan 
        ///     1. Sample point milik Tools yg ada pada list Room
        ///     2. Test Scenario : In Operation/At Rest
        ///     2. Tujuan uji/purposes
        /// </summary>
        /// <param name="roomIds"></param>
        /// <param name="testScenarioLabel"></param>
        /// <param name="purposes"></param>
        /// <returns></returns>
        public async Task<List<VSamplePointTestParam>> GetSamplePointTestParamByToolsInRooms(List<int> roomIds, string testScenarioLabel, List<int> purposeIds)
        {
            var toolPurposeIds = await (
                from tpmp in _context.TransactionToolPurposeToMasterPurpose
                where purposeIds.Contains(tpmp.PurposeId ?? 0)
                select tpmp.ToolPurposeId).ToListAsync();

            var tpmps = await (
                from tpmp in _context.TransactionToolPurposeToMasterPurpose
                where toolPurposeIds.Contains(tpmp.ToolPurposeId)
                select tpmp).ToListAsync();

            List<int> selectedToolPurposeIds = new List<int>();

            //pilih tool purpose id yg match dengan purpose
            var groups = tpmps.GroupBy(x => x.ToolPurposeId);
            foreach (var group in groups)
            {
                if (group.Count() == purposeIds.Count())
                {
                    var tpmpsByToolPurposeId = group.ToList();
                    var lsPurposeId = tpmpsByToolPurposeId.Select(x => x.PurposeId ?? 0).ToList();

                    //pilih yg memiliki semua purpose Id
                    if (ContainsAllItems<int>(lsPurposeId, purposeIds))
                    {
                        selectedToolPurposeIds.Add(tpmpsByToolPurposeId[0].ToolPurposeId);
                    }
                }
            }

            var vSamplePointTestParams = await (from t in _context.TransactionTool
                                                join tps in _context.TransactionToolPurpose on t.Id equals tps.ToolId
                                                join relst in _context.TransactionRelSamplingTool on tps.Id equals relst.ToolPurposeId
                                                join sp in _context.TransactionSamplingPoint on relst.SamplingPoinId equals sp.Id

                                                //join r in _context.TransactionRoom on sp.RoomId equals r.Id
                                                join r in _context.TransactionRoom on t.RoomId equals r.Id
                                                join gr in _context.TransactionGradeRoom on t.GradeRoomId equals gr.Id

                                                join rstp in _context.TransactionRelSamplingTestParam on sp.Id equals rstp.SamplingPointId
                                                join rtsp in _context.TransactionRelTestScenarioParam on rstp.TestScenarioParamId equals rtsp.Id
                                                join ts in _context.TransactionTestScenario on rtsp.TestScenarioId equals ts.Id
                                                join tp in _context.TransactionTestParameter on rtsp.TestParameterId equals tp.Id

                                                where
                                                // t.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                                // && r.RowStatus == null
                                                // && r.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                                // && gr.RowStatus == null
                                                // && gr.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                                roomIds.Contains(t.RoomId.Value)
                                                && ts.Label == testScenarioLabel
                                                && toolPurposeIds.Contains(tps.Id)
                                                && relst.ScenarioLabel == testScenarioLabel
                                                select new VSamplePointTestParam
                                                {
                                                    RoomId = r.Id,
                                                    ToolId = t.Id,
                                                    ToolName = t.Name,
                                                    GradeRoomId = gr.Id,
                                                    GradeRoomName = gr.Name,
                                                    SamplePointId = sp.Id,
                                                    SamplePointName = sp.Code,
                                                    TestScenarioId = ts.Id,
                                                    TestScenarioName = ts.Name,
                                                    TestScenarioLabel = ts.Label,
                                                    TestParameterId = tp.Id,
                                                    TestParameterName = tp.Name,
                                                    TestGroupId = tp.TestGroupId,
                                                    Seq = tp.Sequence,
                                                    toolPurposeId = tps.Id
                                                }).Distinct().ToListAsync();
            return vSamplePointTestParams;
        }

        public async Task<List<VSamplePointTestParam>> GetSamplePointTestParamByToolsInRoomsExisting(List<int> roomIds, string testScenarioLabel, List<int> purposeIds)
        {
            var toolPurposeIds = await (
                from tpmp in _context.ToolPurposeToMasterPurposes
                where purposeIds.Contains(tpmp.PurposeId ?? 0)
                select tpmp.ToolPurposeId).ToListAsync();

            var tpmps = await (
                from tpmp in _context.ToolPurposeToMasterPurposes
                where toolPurposeIds.Contains(tpmp.ToolPurposeId)
                select tpmp).ToListAsync();

            List<int> selectedToolPurposeIds = new List<int>();

            //pilih tool purpose id yg match dengan purpose
            var groups = tpmps.GroupBy(x => x.ToolPurposeId);
            foreach (var group in groups)
            {
                if (group.Count() == purposeIds.Count())
                {
                    var tpmpsByToolPurposeId = group.ToList();
                    var lsPurposeId = tpmpsByToolPurposeId.Select(x => x.PurposeId ?? 0).ToList();

                    //pilih yg memiliki semua purpose Id
                    if (ContainsAllItems<int>(lsPurposeId, purposeIds))
                    {
                        selectedToolPurposeIds.Add(tpmpsByToolPurposeId[0].ToolPurposeId);
                    }
                }
            }

            var vSamplePointTestParams = await (from t in _context.Tools
                                                join tps in _context.ToolPurposes on t.Id equals tps.ToolId
                                                join relst in _context.RelSamplingTools on tps.Id equals relst.ToolPurposeId
                                                join sp in _context.SamplingPoints on relst.SamplingPointId equals sp.Id

                                                join r in _context.Rooms on sp.RoomId equals r.Id
                                                join gr in _context.GradeRooms on r.GradeRoomId equals gr.Id

                                                join rstp in _context.RelSamplingTestParams on sp.Id equals rstp.SamplingPointId
                                                join rtsp in _context.RelTestScenarioParams on rstp.TestScenarioParamId equals rtsp.Id
                                                join ts in _context.TestScenarios on rtsp.TestScenarioId equals ts.Id
                                                join tp in _context.TestParameters on rtsp.TestParameterId equals tp.Id

                                                where t.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                                && r.RowStatus == null
                                                && r.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                                && gr.RowStatus == null
                                                && gr.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                                && roomIds.Contains(t.RoomId)
                                                && ts.Label == testScenarioLabel
                                                && toolPurposeIds.Contains(tps.Id)
                                                && relst.ScenarioLabel == testScenarioLabel
                                                select new VSamplePointTestParam
                                                {
                                                    RoomId = r.Id,
                                                    ToolId = t.Id,
                                                    ToolName = t.Name,
                                                    GradeRoomId = gr.Id,
                                                    GradeRoomName = gr.Name,
                                                    SamplePointId = sp.Id,
                                                    SamplePointName = sp.Code,
                                                    TestScenarioId = ts.Id,
                                                    TestScenarioName = ts.Name,
                                                    TestScenarioLabel = ts.Label,
                                                    TestParameterId = tp.Id,
                                                    TestParameterName = tp.Name,
                                                    TestGroupId = tp.TestGroupId,
                                                    Seq = tp.Sequence,
                                                    toolPurposeId = tps.Id
                                                }).Distinct().ToListAsync();
            return vSamplePointTestParams;
        }

        public async Task<int> _getSamplePointTestParameterCount(int roomId, int TestParameterId, string testScenarioLabel)
        {
            var getCountSamplePoint = (await (from rrs in _context.RelRoomSamplings //get count sample point room
                                              join room_purp in _context.RoomPurpose on rrs.RoomPurposeId equals room_purp.Id
                                              join sttp in _context.RelSamplingTestParams on rrs.SamplingPointId equals sttp.SamplingPointId
                                              join tsp in _context.RelTestScenarioParams on sttp.TestScenarioParamId equals tsp.Id
                                              join ts in _context.TestScenarios on tsp.TestScenarioId equals ts.Id
                                              where ts.Label == testScenarioLabel
                                              && room_purp.RoomId == roomId
                                              && tsp.TestParameterId == TestParameterId
                                              select rrs.SamplingPointId).CountAsync() +
                                        await (from rst in _context.RelSamplingTools //get count sample point tool
                                               join tp in _context.ToolPurposes on rst.ToolPurposeId equals tp.Id
                                               join tl in _context.Tools on tp.ToolId equals tl.Id
                                               join sttp in _context.RelSamplingTestParams on rst.SamplingPointId equals sttp.SamplingPointId
                                               join tsp in _context.RelTestScenarioParams on sttp.TestScenarioParamId equals tsp.Id
                                               join ts in _context.TestScenarios on tsp.TestScenarioId equals ts.Id
                                               where ts.Label == testScenarioLabel
                                               && tl.RoomId == roomId
                                               && tsp.TestParameterId == TestParameterId
                                               select rst.SamplingPointId).CountAsync());

            return getCountSamplePoint;
        }

        public async Task<int> getSamplePointTestParameterCount(int roomId, int TestParameterId, string testScenarioLabel, List<int> purposeId)
        {
            List<int> getGroupToolPurposes = await (from rptg in _context.RelSamplingPurposeToolGroups
                                                    where rptg.RowStatus == null
                                                    && purposeId.Contains(rptg.PurposeId)
                                                    select rptg.ToolGroupId).ToListAsync();

            //menyeleksi data sampling point berdasarkan purpose nya, 
            List<int> getPurposeIdByPurposeRoom = await (from room_purp in _context.RoomPurpose
                                                         join room_purp_master in _context.RoomPurposeToMasterPurposes on room_purp.Id equals room_purp_master.RoomPurposeId
                                                         where room_purp.RowStatus == null
                                                         && room_purp.RoomId == roomId
                                                         && purposeId.Contains(room_purp_master.PurposeId)
                                                         select room_purp.Id).ToListAsync(); // to be deleted

            var getCountPurposePerRoomPurpose = await (from room_purp_2 in _context.RoomPurpose
                                                       join room_purp_to_master in _context.RoomPurposeToMasterPurposes on room_purp_2.Id equals room_purp_to_master.RoomPurposeId
                                                       where room_purp_2.RoomId == roomId
                                                       && purposeId.Contains(room_purp_to_master.PurposeId)
                                                       group room_purp_2 by room_purp_2.Id into newGroup
                                                       select new
                                                       {
                                                           roomPurposeId = newGroup.Key,
                                                           countPurpose = newGroup.Count()
                                                       }).FirstOrDefaultAsync(x => x.countPurpose == purposeId.Count());

            var getCountSamplePoint = (await (from rrs in _context.RelRoomSamplings //get count sample point room
                                              join room_purp in _context.RoomPurpose on rrs.RoomPurposeId equals room_purp.Id
                                              join sttp in _context.RelSamplingTestParams on rrs.SamplingPointId equals sttp.SamplingPointId
                                              join tsp in _context.RelTestScenarioParams on sttp.TestScenarioParamId equals tsp.Id
                                              join ts in _context.TestScenarios on tsp.TestScenarioId equals ts.Id
                                              where ts.Label == testScenarioLabel
                                              && rrs.RoomPurposeId == getCountPurposePerRoomPurpose.roomPurposeId
                                              && room_purp.RoomId == roomId
                                              && tsp.TestParameterId == TestParameterId
                                              select rrs.SamplingPointId).CountAsync() +
                                        await (from rst in _context.RelSamplingTools //get count sample point tool
                                               join tp in _context.ToolPurposes on rst.ToolPurposeId equals tp.Id
                                               join tl in _context.Tools on tp.ToolId equals tl.Id
                                               join sttp in _context.RelSamplingTestParams on rst.SamplingPointId equals sttp.SamplingPointId
                                               join tsp in _context.RelTestScenarioParams on sttp.TestScenarioParamId equals tsp.Id
                                               join ts in _context.TestScenarios on tsp.TestScenarioId equals ts.Id
                                               where ts.Label == testScenarioLabel
                                               && tl.RoomId == roomId
                                               && tsp.TestParameterId == TestParameterId
                                               && getGroupToolPurposes.Contains(tl.ToolGroupId)
                                               select rst.SamplingPointId).CountAsync());

            return getCountSamplePoint;
        }

        public async Task<int> getSamplePointTestParameterCountByRequestId(int requestId, int TestParameterId, string testScenarioLabel)
        {
            var getRoomPurpose = await (from rq in _context.RequestQcs
                                        join rr in _context.RequestGroupRoomPurpose on rq.Id equals rr.RequestQcsId
                                        where rr.RequestQcsId == requestId
                                        select rr).ToListAsync();

            var getToolPurpose = await (from rq in _context.RequestQcs
                                        join rt in _context.RequestGroupToolPurpose on rq.Id equals rt.RequestQcsId
                                        where rt.RequestQcsId == requestId
                                        select rt).ToListAsync();

            var getCountSamplePoint = (await (from rrs in _context.TransactionRelRoomSamplingPoint //get count sample point room
                                              join room_purp in _context.TransactionRoomPurpose on rrs.RoomPurposeId equals room_purp.Id
                                              join sttp in _context.TransactionRelSamplingTestParam on rrs.SamplingPoinId equals sttp.SamplingPointId
                                              join tsp in _context.TransactionRelTestScenarioParam on sttp.TestScenarioParamId equals tsp.Id
                                              join ts in _context.TransactionTestScenario on tsp.TestScenarioId equals ts.Id
                                              where ts.Label == testScenarioLabel
                                              && getRoomPurpose.Select(x => x.RoomPurposeId).ToList().Contains(rrs.RoomPurposeId)
                                              && tsp.TestParameterId == TestParameterId
                                              && rrs.ScenarioLabel == testScenarioLabel
                                              select rrs.SamplingPoinId).CountAsync() +
                                        await (from rst in _context.TransactionRelSamplingTool //get count sample point tool
                                               join tp in _context.TransactionToolPurpose on rst.ToolPurposeId equals tp.Id
                                               join tl in _context.TransactionTool on tp.ToolId equals tl.Id
                                               join sttp in _context.TransactionRelSamplingTestParam on rst.SamplingPoinId equals sttp.SamplingPointId
                                               join tsp in _context.TransactionRelTestScenarioParam on sttp.TestScenarioParamId equals tsp.Id
                                               join ts in _context.TransactionTestScenario on tsp.TestScenarioId equals ts.Id
                                               where ts.Label == testScenarioLabel
                                               && tsp.TestParameterId == TestParameterId
                                               && getToolPurpose.Select(x => x.ToolPurposeId).ToList().Contains(tp.Id)
                                               && rst.ScenarioLabel == testScenarioLabel
                                               select rst.SamplingPoinId).CountAsync());

            return getCountSamplePoint;
        }

        public async Task<List<RequestQcs>> CheckNoBatchRequestEM(string noBatch, int? phaseId, string testScenarioLabel, int? requestId)
        {
            var result = await (from rq in _context.RequestQcs
                                where rq.RowStatus == null
                                && rq.NoBatch == noBatch
                                && rq.EmPhaseId == phaseId
                                && rq.TestScenarioLabel == testScenarioLabel
                                && rq.Status >= 0
                                select rq).Where(x => x.Id != requestId || requestId == null).ToListAsync();


            return result;
        }

        public async Task<QcSampling> generateSamplingProductQcAlt(RequestQcs data, List<TestTypeQcs> dataTestType)
        {
            var getQcSampling = _context.QcSamplings.Where(x => x.RequestQcsId == data.Id).ToList();
            foreach (var qcSampling in getQcSampling)
            {
                var getQcSample = _context.QcSamples.Where(x => x.QcSamplingId == qcSampling.Id).ToList();
                _context.QcSamples.RemoveRange(getQcSample);
            }
            _context.QcSamplings.RemoveRange(getQcSampling);
            // insert sampling data
            QcSampling insertDataSampling = new QcSampling()
            {
                RequestQcsId = data.Id,
                Code = generateCodeSample(6),
                Status = 0,
                CreatedBy = data.CreatedBy,
                CreatedAt = data.CreatedAt,
                UpdatedBy = data.UpdatedBy,
                UpdatedAt = data.UpdatedAt
            };

            await _context.QcSamplings.AddAsync(insertDataSampling);
            await _context.SaveChangesAsync();
            // init sample data
            List<QcSample> insertSampleData = new List<QcSample>();
            foreach (var testType in dataTestType)
            {
                
                QcSample dataSample = new QcSample()
                {
                    Code = generateCodeSample(6),//TODO 6 pindahkan ke constant
                    QcSamplingId = insertDataSampling.Id,
                    TestTypeId = testType.TestTypeId,
                    TestTypeName = testType.TestTypeName,
                    TestTypeMethodId = testType.TestTypeMethodId,
                    TestTypeMethodName = testType.TestTypeMethodName,
                    CreatedBy = data.CreatedBy,
                    CreatedAt = DateHelper.Now(),
                    UpdatedBy = data.CreatedBy,
                    UpdatedAt = DateHelper.Now(),
                    IsDefault = true,
                };
                insertSampleData.Add(dataSample);
                await _context.QcSamples.AddAsync(dataSample);
                await _context.SaveChangesAsync();
            }

            return insertDataSampling;
        }

        private bool ContainsAllItems<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            return !b.Except(a).Any();
        }

        private async Task<List<int>> _GetRoomGroupPurpose(List<int> purposeId, int roomId)
        {
            return await (from room_purp_2 in _context.TransactionRoomPurpose
                          join room_purp_to_master in _context.TransactionRoomPurposeToMasterPurpose on room_purp_2.Id equals room_purp_to_master.RoomPurposeId
                          where room_purp_2.RoomId == roomId
                          && purposeId.Contains(room_purp_to_master.PurposeId.Value)
                          select room_purp_2.Id).ToListAsync();
        }

        public async Task<List<VSamplePointTestParam>> GetSamplePointTestParamByToolsInRoomsById(int roomIds, string testScenarioLabel)
        {
            var vSamplePointTestParams = await (from t in _context.TransactionTool
                                                join tps in _context.TransactionToolPurpose on t.Id equals tps.ToolId
                                                join relst in _context.TransactionRelSamplingTool on tps.Id equals relst.ToolPurposeId
                                                join sp in _context.TransactionSamplingPoint on relst.SamplingPoinId equals sp.Id

                                                join r in _context.TransactionRoom on t.RoomId equals r.Id
                                                join gr in _context.TransactionGradeRoom on t.GradeRoomId equals gr.Id

                                                join rstp in _context.TransactionRelSamplingTestParam on sp.Id equals rstp.SamplingPointId
                                                join rtsp in _context.TransactionRelTestScenarioParam on rstp.TestScenarioParamId equals rtsp.Id
                                                join ts in _context.TransactionTestScenario on rtsp.TestScenarioId equals ts.Id
                                                join tp in _context.TransactionTestParameter on rtsp.TestParameterId equals tp.Id

                                                where
                                                // t.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                                // && r.RowStatus == null
                                                // && r.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                                // && gr.RowStatus == null
                                                // && gr.ObjectStatus == ApplicationConstant.OBJECT_STATUS_ACTIVE
                                                //&& 
                                                t.RoomId == roomIds
                                                && ts.Label == testScenarioLabel
                                                select new VSamplePointTestParam
                                                {
                                                    RoomId = r.Id,
                                                    ToolId = t.Id,
                                                    ToolName = t.Name,
                                                    GradeRoomId = gr.Id,
                                                    GradeRoomName = gr.Name,
                                                    SamplePointId = sp.Id,
                                                    SamplePointName = sp.Code,
                                                    TestScenarioId = ts.Id,
                                                    TestScenarioName = ts.Name,
                                                    TestScenarioLabel = ts.Label,
                                                    TestParameterId = tp.Id,
                                                    TestParameterName = tp.Name,
                                                    TestGroupId = tp.TestGroupId,
                                                    Seq = tp.Sequence,
                                                    toolPurposeId = tps.Id
                                                }).Distinct().ToListAsync();
            return vSamplePointTestParams;
        }


        public async Task<QcSampling> generateSamplingQcAltV2(int TypeSamplingId, string RequestSamplingName, RequestQcs data, List<TestTypeQcs> dataTestType)
        {
            // insert sampling data
            QcSampling insertDataSampling = new QcSampling()
            {
                RequestQcsId = data.Id,
                Code = generateCodeSample(6),//TODO 6 pindah ke consant
                SamplingTypeId = TypeSamplingId,
                SamplingTypeName = RequestSamplingName,
                Status = 0,
                CreatedBy = data.CreatedBy,
                CreatedAt = data.CreatedAt,
                UpdatedBy = data.UpdatedBy,
                UpdatedAt = data.UpdatedAt
            };

            await _context.QcSamplings.AddAsync(insertDataSampling);
            await _context.SaveChangesAsync();

            // init sample data
            List<QcSample> insertSampleData = new List<QcSample>();

            // get request room
            var getRequestRoom = await (from rr in _context.RequestRooms
                                        where rr.QcRequestId == data.Id && rr.RowStatus == null
                                        select rr).ToListAsync();

            // get request purposes
            var getRequestPurpose = await (from rp in _context.RequestPurposes
                                           where rp.QcRequestId == data.Id && rp.RowStatus == null
                                           select rp.PurposeId).ToListAsync();

            //get dataSample berdasarkan sample point dari tool yg ada di room
            var roomIds = getRequestRoom.Select(x => x.RoomId).Distinct().ToList();
            //var samplingPointsByTools = await GetSamplePointTestParamByToolsInRooms(roomIds, data.TestScenarioLabel, getRequestPurpose);

            //filter sesuai TypeSamplingId(EM atau PC)
            //samplingPointsByTools = samplingPointsByTools.Where(x => x.TestGroupId == TypeSamplingId).ToList();
            if (getRequestRoom.Any())
            {
                // looping request room
                // foreach (var requestRoom in getRequestRoom)
                // {
                //     //get total sampling request per test parameter
                //     //looping test type for get parameter
                //     foreach (var testType in dataTestType)
                //     {
                //         //mengurangi jumlah sample sesuai sample point dari tool supaya tidak menimbulkan left over sample. Hal ini dikarenakan SampleAmountCount dari FE nya sudah ditambahkan dengan sample point dari tool terlebih dahulu
                //         int amountSamplingPointFromTools = samplingPointsByTools
                //             .Where(x => x.RoomId == requestRoom.RoomId && x.TestParameterId == testType.TestParameterId)
                //             .Select(x => x.SamplePointId)
                //             .Distinct()
                //             .Count();
                //         testType.SampleAmountCount -= amountSamplingPointFromTools;

                //         // check sample porint per room or grade room
                //         /*List<VSamplePointTestParam> getSampleTest = (from stp in _context.VSamplePointTestParams
                //                                                         where stp.RoomId == requestRoom.RoomId &&
                //                                                             stp.TestParameterId == testType.TestParameterId &&
                //                                                             stp.TestScenarioId == requestRoom.TestScenarioId
                //                                                         select stp).ToList();*/

                //         // kika sedang kerjakan
                //         // get sample test 
                //         //var getSampleTest = await getSamplePointTestParameter(requestRoom.RoomId, testType.TestParameterId, data.TestScenarioLabel, getRequestPurpose);

                //         //data.Id request id
                //         // get count sample point
                //         //var getCountSamplePoint = await getSamplePointTestParameterCount(requestRoom.RoomId, testType.TestParameterId, data.TestScenarioLabel, getRequestPurpose);
                //         var getCountSamplePoint = await getSamplePointTestParameterCountByRequestId(data.Id, testType.TestParameterId, data.TestScenarioLabel);

                //         var CountLimitSample = 0;
                //         var SequencePC05 = 1;
                //         var SequencePC50 = 1;


                //         #region left over sample generator
                //         if (testType.TestParameterId != ApplicationConstant.TEST_PARAMETER_FD)
                //         {
                //             // sample count
                //             if (getRequestRoom.Count() == 1)
                //             {

                //                 if (TypeSamplingId == ApplicationConstant.REQUEST_SAPMLING_EMM &&
                //                 (testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_SP ||
                //                     testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_AS ||
                //                     testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_CA ||
                //                     testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_GV
                //                 ))
                //                 {
                //                     var SampleLeftOver = testType.SampleAmountCount - CountLimitSample;
                //                     if (SampleLeftOver > 0)
                //                     {
                //                         for (int samp = 0; samp < SampleLeftOver; samp++)
                //                         {
                //                             QcSample dataSampleLeftOver = new QcSample()
                //                             {
                //                                 //Code = generateCodeSample(6) + "-" + await checkScenarioCode(requestRoom.TestScenarioId),//TODO 6 pindahkan ke constant
                //                                 Code = generateCodeSample(6),//TODO 6 pindahkan ke constant
                //                                 QcSamplingId = insertDataSampling.Id,
                //                                 SamplingPointId = null,
                //                                 SamplingPointCode = null,
                //                                 TestParamId = testType.TestParameterId,
                //                                 TestParamName = testType.TestParameterName,
                //                                 GradeRoomId = null,
                //                                 GradeRoomName = null,
                //                                 CreatedBy = data.CreatedBy,
                //                                 CreatedAt = DateHelper.Now(),
                //                                 UpdatedBy = data.CreatedBy,
                //                                 UpdatedAt = DateHelper.Now(),
                //                                 IsDefault = true,
                //                                 TestScenarioId = null
                //                             };
                //                             insertSampleData.Add(dataSampleLeftOver);
                //                         }
                //                     }
                //                 }


                //                 if (TypeSamplingId == ApplicationConstant.REQUEST_SAPMLING_PC &&
                //                 (testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC05 ||
                //                     testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC50
                //                 ))
                //                 {
                //                     var SampleLeftOver = testType.SampleAmountCount - CountLimitSample;
                //                     if (SampleLeftOver > 0)
                //                     {
                //                         for (int samp = 0; samp < SampleLeftOver; samp++)
                //                         {

                //                             var sequenceParam = 0;
                //                             if (testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC05)
                //                             {
                //                                 sequenceParam = SequencePC05;
                //                                 SequencePC05++;
                //                             }
                //                             else if (testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC50)
                //                             {
                //                                 sequenceParam = SequencePC50;
                //                                 SequencePC50++;
                //                             }

                //                             QcSample dataSampleLeftOver = new QcSample()
                //                             {
                //                                 //Code = generateCodeSample(6) + "-" + await checkScenarioCode(requestRoom.TestScenarioId),//TODO 6 pindahkan ke constant
                //                                 Code = generateCodeSample(6),//TODO 6 pindahkan ke constant
                //                                 QcSamplingId = insertDataSampling.Id,
                //                                 SamplingPointId = null,
                //                                 SamplingPointCode = null,
                //                                 TestParamId = testType.TestParameterId,
                //                                 TestParamName = testType.TestParameterName,
                //                                 GradeRoomId = null,
                //                                 GradeRoomName = null,
                //                                 SampleSequence = sequenceParam,
                //                                 CreatedBy = data.CreatedBy,
                //                                 CreatedAt = DateHelper.Now(),
                //                                 UpdatedBy = data.CreatedBy,
                //                                 UpdatedAt = DateHelper.Now(),
                //                                 IsDefault = true,
                //                                 TestScenarioId = null
                //                             };
                //                             insertSampleData.Add(dataSampleLeftOver);
                //                         }
                //                     }
                //                 }
                //             }


                //         }
                //         #endregion

                //     }

                // }

                // looping sample for generate
                var getSampleTest = await getSamplePointTestParameterByRequestIdV2(data.TestScenarioLabel, roomIds, getRequestPurpose);
                var SequencePC05 = 1;
                var SequencePC50 = 1;

                #region cek jika ada penambahan sample amount (Sample Left Over)
                //get list master test param
                var testParams = await (from tp in _context.TestParameters
                                        select tp).ToListAsync();

                //group sample test dari DB
                var groupedSampleTest = getSampleTest
                    .GroupBy(x => new { x.TestParameterId, x.TestParameterName })
                    .Select(x => new TestTypeQcs()
                    {
                        TestParameterId = x.Key.TestParameterId,
                        TestParameterName = x.Key.TestParameterName,
                        SampleAmountCount = x.Count()
                    });

                //cek jika ada penambahan sample amount
                foreach (var item in groupedSampleTest)
                {
                    var addedTest = dataTestType.FirstOrDefault(x => x.TestParameterId == item.TestParameterId);
                    if (addedTest.SampleAmountCount > item.SampleAmountCount)
                    {
                        int addedCount = addedTest.SampleAmountCount - item.SampleAmountCount;
                        var lastObj = getSampleTest.LastOrDefault(t => t.TestParameterId == item.TestParameterId);
                        int newIndex = getSampleTest.FindLastIndex(t => t.TestParameterId == item.TestParameterId) + 1;

                        //jika tidak ada pada list getSampleTest -> tambahkan newObj di urutan terakhir
                        if (newIndex == 0) newIndex = getSampleTest.Count();

                        VSamplePointTestParam newObj = new VSamplePointTestParam();
                        if (lastObj != null)
                        {
                            //copy obj karena tidak bisa lgsg direct menggunakan lastObj
                            newObj.TestScenarioLabel = lastObj.TestScenarioLabel;
                            newObj.TestParameterId = lastObj.TestParameterId;
                            newObj.TestParameterName = lastObj.TestParameterName;
                            newObj.TestGroupId = lastObj.TestGroupId;
                            newObj.Seq = lastObj.Seq;
                            newObj.toolPurposeId = lastObj.toolPurposeId;
                        }
                        else
                        {
                            var testParam = testParams.FirstOrDefault(x => x.Id == item.TestParameterId);

                            newObj.TestScenarioLabel = data.TestScenarioLabel;
                            newObj.TestParameterId = item.TestParameterId;
                            newObj.TestParameterName = item.TestParameterName;
                            newObj.TestGroupId = testParam.TestGroupId;
                            newObj.Seq = testParam.Sequence;
                            newObj.toolPurposeId = 0;
                        }

                        newObj.RoomId = 0;
                        newObj.GradeRoomId = 0;
                        newObj.GradeRoomName = null;
                        newObj.SamplePointId = 0;
                        newObj.SamplePointName = null;
                        newObj.TestScenarioId = 0;
                        newObj.TestScenarioName = null;

                        //tambahkan ke sample test dari DB
                        for (int i = 0; i < addedCount; i++)
                        {
                            getSampleTest.Insert(newIndex, newObj);
                        }

                    }
                }
                #endregion

                foreach (var SampleTest in getSampleTest)
                {
                    //Generate Sampling Test Parameter PC -- OK
                    #region generate sample PC
                    if (SampleTest.TestGroupId == ApplicationConstant.REQUEST_SAPMLING_PC && TypeSamplingId == ApplicationConstant.REQUEST_SAPMLING_PC)
                    {
                        var sequenceParam = 0;
                        if (SampleTest.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC05)
                        {
                            sequenceParam = SequencePC05;
                            SequencePC05++;
                        }
                        else if (SampleTest.TestParameterId == ApplicationConstant.TEST_PARAMETER_PC50)
                        {
                            sequenceParam = SequencePC50;
                            SequencePC50++;
                        }

                        QcSample dataSample = new QcSample()
                        {
                            Code = generateCodeSample(6) + "-" + await checkScenarioCode(SampleTest.TestScenarioId),//TODO 6 pindahkan ke constant  
                            QcSamplingId = insertDataSampling.Id,
                            SamplingPointId = SampleTest.SamplePointId,
                            SamplingPointCode = SampleTest.SamplePointName,
                            TestParamId = SampleTest.TestParameterId,
                            TestParamName = SampleTest.TestParameterName,
                            GradeRoomId = SampleTest.GradeRoomId,
                            GradeRoomName = SampleTest.GradeRoomName,
                            SampleSequence = sequenceParam,
                            CreatedBy = data.CreatedBy,
                            CreatedAt = DateHelper.Now(),
                            UpdatedBy = data.CreatedBy,
                            UpdatedAt = DateHelper.Now(),
                            IsDefault = true,
                            TestScenarioId = SampleTest.TestScenarioId
                        };
                        insertSampleData.Add(dataSample);
                    }
                    #endregion

                    //Generate Sampling Test Parameter EM-M -- OK
                    #region generate sample EMM
                    if (SampleTest.TestGroupId == ApplicationConstant.REQUEST_SAPMLING_EMM && TypeSamplingId == ApplicationConstant.REQUEST_SAPMLING_EMM)
                    {

                        QcSample dataSample = new QcSample()
                        {
                            Code = generateCodeSample(6) + "-" + await checkScenarioCode(SampleTest.TestScenarioId),//TODO 6 pindahkan ke constant
                            QcSamplingId = insertDataSampling.Id,
                            SamplingPointId = SampleTest.SamplePointId,
                            SamplingPointCode = SampleTest.SamplePointName,
                            TestParamId = SampleTest.TestParameterId,
                            TestParamName = SampleTest.TestParameterName,
                            GradeRoomId = SampleTest.GradeRoomId,
                            GradeRoomName = SampleTest.GradeRoomName,
                            CreatedBy = data.CreatedBy,
                            CreatedAt = DateHelper.Now(),
                            UpdatedBy = data.CreatedBy,
                            UpdatedAt = DateHelper.Now(),
                            IsDefault = true,
                            TestScenarioId = SampleTest.TestScenarioId
                        };
                        insertSampleData.Add(dataSample);
                    }
                    #endregion

                }
            }


            foreach (var testType in dataTestType)
            {
                //Generate Test Parameter FD -- OK
                #region test finger DAB
                if (testType.TestParameterId == ApplicationConstant.TEST_PARAMETER_FD && TypeSamplingId == ApplicationConstant.REQUEST_SAPMLING_EMM)
                {

                    if (testType.SampleAmountCount > 0)
                    {
                        var NoFD = 1;
                        for (int fd = 0; fd < testType.SampleAmountCount; fd++)
                        {
                            //Insert Data FD Left
                            QcSample dataSampleFDLeft = new QcSample()
                            {
                                Code = generateCodeSample(6),//TODO 6 pindahkan ke constant
                                QcSamplingId = insertDataSampling.Id,
                                SamplingPointId = null,
                                SamplingPointCode = "FDL-" + NoFD,
                                TestParamId = testType.TestParameterId,
                                TestParamName = testType.TestParameterName,
                                GradeRoomId = null,
                                GradeRoomName = null,
                                CreatedBy = data.CreatedBy,
                                CreatedAt = DateHelper.Now(),
                                UpdatedBy = data.CreatedBy,
                                UpdatedAt = DateHelper.Now(),
                                IsDefault = true,
                                TestScenarioId = null
                            };
                            insertSampleData.Add(dataSampleFDLeft);

                            //Insert Data FD Right
                            QcSample dataSampleFDRight = new QcSample()
                            {
                                Code = generateCodeSample(6),//TODO 6 pindahkan ke constant
                                QcSamplingId = insertDataSampling.Id,
                                SamplingPointId = null,
                                SamplingPointCode = "FDR-" + NoFD,
                                TestParamId = testType.TestParameterId,
                                TestParamName = testType.TestParameterName,
                                GradeRoomId = null,
                                GradeRoomName = null,
                                CreatedBy = data.CreatedBy,
                                CreatedAt = DateHelper.Now(),
                                UpdatedBy = data.CreatedBy,
                                UpdatedAt = DateHelper.Now(),
                                IsDefault = true,
                                TestScenarioId = null
                            };
                            insertSampleData.Add(dataSampleFDRight);

                            NoFD++;
                        }

                    }
                }
                #endregion
            }

            //save sample data generate
            if (insertSampleData.Any())
            {
                await _context.QcSamples.AddRangeAsync(insertSampleData);
            }
            await _context.SaveChangesAsync();

            return insertDataSampling;
        }


        public async Task<List<VSamplePointTestParam>> getSamplePointTestParameterByRequestIdV2(string testScenarioLabel, List<int> roomsId, List<int> purposes)
        {
            var roomSamplePointTestParam = await (from rrsp in _context.TransactionRelRoomSamplingPoint
                                                  join sp in _context.TransactionSamplingPoint on rrsp.SamplingPoinId equals sp.Id
                                                  join room_purp in _context.TransactionRoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                  join rptmp in _context.TransactionRoomPurposeToMasterPurpose on room_purp.Id equals rptmp.RoomPurposeId
                                                  join rstp in _context.TransactionRelSamplingTestParam on rrsp.SamplingPoinId equals rstp.SamplingPointId
                                                  join rtps in _context.TransactionRelTestScenarioParam on rstp.TestScenarioParamId equals rtps.Id
                                                  join ts in _context.TransactionTestScenario on rtps.TestScenarioId equals ts.Id
                                                  join rgrs in _context.TransactionRelGradeRoomScenario on ts.Id equals rgrs.TestScenarioId
                                                  join gr in _context.TransactionGradeRoom on rgrs.GradeRoomId equals gr.Id
                                                  join tp in _context.TransactionTestParameter on rtps.TestParameterId equals tp.Id
                                                  where roomsId.Contains(room_purp.RoomId)
                                                  && purposes.Contains(rptmp.PurposeId.Value)
                                                  && rrsp.ScenarioLabel == testScenarioLabel
                                                  && ts.Label == testScenarioLabel
                                                  orderby tp.Sequence
                                                  select new VSamplePointTestParam
                                                  {
                                                      RoomId = room_purp.RoomId,
                                                      ToolId = null,
                                                      ToolName = null,
                                                      GradeRoomId = gr.Id,
                                                      GradeRoomName = gr.Name,
                                                      SamplePointId = sp.Id,
                                                      SamplePointName = sp.Code,
                                                      TestScenarioId = ts.Id,
                                                      TestScenarioName = ts.Name,
                                                      TestScenarioLabel = ts.Label,
                                                      TestParameterId = tp.Id,
                                                      TestParameterName = tp.Name,
                                                      TestGroupId = tp.TestGroupId,
                                                      Seq = tp.Sequence,
                                                  }).Distinct().ToListAsync();

            var toolSamplePointTestParams = await (from t in _context.TransactionTool
                                                   join tps in _context.TransactionToolPurpose on t.Id equals tps.ToolId
                                                   join relst in _context.TransactionRelSamplingTool on tps.Id equals relst.ToolPurposeId
                                                   join sp in _context.TransactionSamplingPoint on relst.SamplingPoinId equals sp.Id

                                                   join tptmp in _context.TransactionToolPurposeToMasterPurpose on tps.Id equals tptmp.ToolPurposeId
                                                   join purpose in _context.TransactionPurposes on tptmp.PurposeId equals purpose.Id

                                                   join r in _context.TransactionRoom on t.RoomId equals r.Id
                                                   join gr in _context.TransactionGradeRoom on t.GradeRoomId equals gr.Id

                                                   join rstp in _context.TransactionRelSamplingTestParam on sp.Id equals rstp.SamplingPointId
                                                   join rtsp in _context.TransactionRelTestScenarioParam on rstp.TestScenarioParamId equals rtsp.Id
                                                   join ts in _context.TransactionTestScenario on rtsp.TestScenarioId equals ts.Id
                                                   join tp in _context.TransactionTestParameter on rtsp.TestParameterId equals tp.Id

                                                   where
                                                   roomsId.Contains(t.RoomId.Value)
                                                   && purposes.Contains(tptmp.PurposeId.Value)
                                                   && ts.Label == testScenarioLabel
                                                   select new VSamplePointTestParam
                                                   {
                                                       RoomId = r.Id,
                                                       ToolId = t.Id,
                                                       ToolName = t.Name,
                                                       GradeRoomId = gr.Id,
                                                       GradeRoomName = gr.Name,
                                                       SamplePointId = sp.Id,
                                                       SamplePointName = sp.Code,
                                                       TestScenarioId = ts.Id,
                                                       TestScenarioName = ts.Name,
                                                       TestScenarioLabel = ts.Label,
                                                       TestParameterId = tp.Id,
                                                       TestParameterName = tp.Name,
                                                       TestGroupId = tp.TestGroupId,
                                                       Seq = tp.Sequence
                                                       //toolPurposeId = tps.Id
                                                   }).Distinct().ToListAsync();

            roomSamplePointTestParam.AddRange(toolSamplePointTestParams);

            var dataSample = roomSamplePointTestParam.OrderBy(x => x.Seq).ToList();
            return dataSample;
        }
    }
}
