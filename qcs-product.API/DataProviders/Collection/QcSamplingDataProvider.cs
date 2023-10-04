using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.API.WorkflowModels;
using qcs_product.Constants;
using qcs_product.API.BusinessProviders;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Helpers;
// using AutoMapper;

namespace qcs_product.API.DataProviders.Collection
{
    public class QcSamplingDataProvider : IQcSamplingDataProvider
    {
        private readonly QcsProductContext _context;
        // private readonly IMapper _mapper;
        private readonly ILogger<QcSamplingDataProvider> _logger;
        private readonly IReviewDataProvider _reviewDataProvider;
        private readonly IQcRequestDataProvider _requestDataProvider;
        private readonly IWorkflowQcSamplingDataProvider _workflowQcSamplingDataProvider;
        private readonly IWorkflowServiceDataProvider _workflowServiceDataProvider;
        private readonly ITestScenarioDataProvider _testScenarioDataProvider;
        private readonly ITransactionBatchDataProvider _transactionBatchDataProvider;
        private readonly IUploadFilesBusinessProvider _uploadBusinessProvider;
        private readonly IItemDataProvider _dataProviderItem;
        
        [ExcludeFromCodeCoverage]
        public QcSamplingDataProvider(QcsProductContext context, 
            // IMapper mapper, 
            ILogger<QcSamplingDataProvider> logger,
            IReviewDataProvider reviewDataProvider,
            IQcRequestDataProvider requestDataProvider,
            IWorkflowQcSamplingDataProvider workflowQcSamplingDataProvider,
            IWorkflowServiceDataProvider workflowServiceDataProvider,
            ITestScenarioDataProvider testScenarioDataProvider, ITransactionBatchDataProvider transactionBatchDataProvider,
            IUploadFilesBusinessProvider uploadBusinessProvider,
            IItemDataProvider dataProviderItem)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            // _mapper = mapper;
            _logger = logger;
            _reviewDataProvider = reviewDataProvider ?? throw new ArgumentNullException(nameof(reviewDataProvider));
            _requestDataProvider = requestDataProvider ?? throw new ArgumentNullException(nameof(requestDataProvider));
            _workflowQcSamplingDataProvider = workflowQcSamplingDataProvider ?? throw new ArgumentNullException(nameof(workflowQcSamplingDataProvider));
            _workflowServiceDataProvider = workflowServiceDataProvider ?? throw new ArgumentNullException(nameof(workflowServiceDataProvider));
            _testScenarioDataProvider = testScenarioDataProvider ?? throw new ArgumentNullException(nameof(testScenarioDataProvider));
            _dataProviderItem = dataProviderItem ?? throw new ArgumentNullException(nameof(dataProviderItem));
            _transactionBatchDataProvider = transactionBatchDataProvider;
            _uploadBusinessProvider = uploadBusinessProvider;
        }

        public async Task<List<QcRequestSamplingRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int? orgId, int TypeRequestId, int SamplingTypeId)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var resultData = new List<QcRequestSamplingRelationViewModel>();

            var query = (from s in _context.QcSamplings
                         join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                         join tb in _context.TransactionBatches on r.Id equals tb.RequestQcsId into tbGroup
                         from tb in tbGroup.DefaultIfEmpty()
                         join tbl in _context.TransactionBatchLines on tb.Id equals tbl.TrsBatchId into tblGroup
                         from tbl in tblGroup.DefaultIfEmpty()
                         where ((EF.Functions.Like(r.NoRequest.ToLower(), "%" + filter + "%")) ||
                             (EF.Functions.Like(s.Code.ToLower(), "%" + filter + "%")) ||
                             (EF.Functions.Like(r.NoBatch.ToLower(), "%" + filter + "%")) ||
                             (EF.Functions.Like(r.ItemName.ToLower(), "%" + filter + "%")) ||
                             EF.Functions.Like(tbl.NoBatch.ToLower(), "%" + filter + "%")
                         )
                         && status.Contains(s.Status) && s.RowStatus != "deleted"
                         select new QcRequestSamplingRelationViewModel
                         {
                             SamplingId = s.Id,
                             Code = s.Code,
                             //OrgId = o.BiohrOrganizationId,
                             OrgName = r.OrgName,
                             Date = r.Date,
                             RequestId = r.Id,
                             NoRequest = r.NoRequest,
                             NoBatch = r.NoBatch,
                             TypeRequestId = r.TypeRequestId,
                             TypeRequest = r.TypeRequest,
                             SamplingTypeId = s.SamplingTypeId,
                             SamplingTypeName = s.SamplingTypeName,
                             ItemId = r.ItemId,
                             ItemName = r.ItemName,
                             TypeFormId = r.TypeFormId,
                             TypeFormName = r.TypeFormName,
                             Status = s.Status,
                             SamplingDateFrom = s.SamplingDateFrom,
                             SamplingDateTo = s.SamplingDateTo,
                             CreatedBy = s.CreatedBy,
                             CreatedAt = s.CreatedAt,
                             Note = s.Note
                         }).Where(x => ((x.Date >= startDate || !startDate.HasValue) &&
                                     (x.Date <= endDate || !endDate.HasValue)) &&
                                     //(x.OrgId == orgId || orgId == 0) &&
                                     //x.ListOrgByRequest.Where() &&
                                     (x.TypeRequestId == TypeRequestId || TypeRequestId == 0) &&
                                     (x.SamplingTypeId == SamplingTypeId || SamplingTypeId == 0)
            )
            .AsQueryable();

            if (orgId != null)
            {
                query = from q in query
                        join rr in _context.RequestRooms on q.RequestId equals rr.QcRequestId
                        join room in _context.TransactionRoom on rr.RoomId equals room.Id
                        join o in _context.TransactionOrganization on room.OrganizationId equals o.Id
                        where o.BiohrOrganizationId == orgId
                        select q;
            }

            query = query.Distinct()
                .OrderByDescending(x => x.CreatedAt)
                .ThenBy(x => x.SamplingTypeId);

            if (limit > 0)
            {
                resultData = await query.Skip(page).Take(limit).ToListAsync();
            }
            else
            {
                resultData = await query.ToListAsync();
            }

            return resultData;

        }

        public async Task<List<QcSamplingRelationViewModel>> GetDetailRelationById(int id, string sort)
        {
            #region get workflow

            //get worfklow sampling
            List<WorkflowQcSampling> worfklowQcSampling = await _workflowQcSamplingDataProvider.GetByWorkflowByQcSamplingId(id);
            var purposes = await (from rp in _context.RequestPurposes
                                  join r in _context.RequestQcs on rp.QcRequestId equals r.Id
                                  join s in _context.QcSamplings on r.Id equals s.RequestQcsId
                                  where s.Id == id
                                  select new RequestPurposesViewModel
                                  {
                                      PurposeId = rp.PurposeId,
                                      PurposeName = rp.PurposeName,
                                      PurposeCode = rp.PurposeCode
                                  }
                                    ).ToListAsync();

            List<WorkflowHistoryQcSampling> workflowQcSamplingHistory = new List<WorkflowHistoryQcSampling>();
            string eDate = null;
            string eDate2 = null;

            var wfDocumentCodes = worfklowQcSampling.Select(x => x.WorkflowDocumentCode).Distinct();
            List<DocumentHistoryWithCodeViewModel> wfHistories = new List<DocumentHistoryWithCodeViewModel>();
            foreach (var item in wfDocumentCodes)
            {
                var wfHistory = await _reviewDataProvider.GetListHistoryWorkflow(item);
                var documentHistoryWithCode = new DocumentHistoryWithCodeViewModel();
                documentHistoryWithCode.HistoryResponse = wfHistory;
                documentHistoryWithCode.DocumentCode = item;
                wfHistories.Add(documentHistoryWithCode);
            }

            //foreach (var item in worfklowQcSampling)
            //{
            //    DocumentHistoryResponseViewModel workflowHistory = wfHistories
            //        .Where(x => x.DocumentCode == item.WorkflowDocumentCode)
            //        .Select(x => x.HistoryResponse)
            //        .FirstOrDefault();

            //    foreach (var itemHistory in workflowHistory.History)
            //    {
            //        if (((itemHistory.StatusName != "Complete") && (item.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_1)))
            //        {
            //            foreach (var itemPIC in itemHistory.PICs)
            //            {
            //                string actionUser = "";
            //                if (itemPIC.ActionName == null && itemPIC.IsPending == true)
            //                {
            //                    actionUser = ApplicationConstant.WORKFLOW_STATUS_PENDING;
            //                }
            //                else if (itemPIC.ActionName == null && itemPIC.IsPending == false)
            //                {
            //                    actionUser = "Close By System";
            //                }
            //                else if (itemPIC.ActionName == ApplicationConstant.WORKFLOW_ACTION_REJECT_COMPLETE_NAME)
            //                {
            //                    actionUser = "Reject";
            //                }
            //                else
            //                {
            //                    actionUser = itemPIC.ActionName;
            //                }
            //                WorkflowHistoryQcSampling addWorkflowHistory = new WorkflowHistoryQcSampling()
            //                {
            //                    Action = actionUser,
            //                    Note = itemPIC.Notes,
            //                    DateTime = itemPIC.ActionDate == null ? eDate : itemPIC.ActionDate,
            //                    PersonalName = itemPIC.OrgName,
            //                    PersonalNik = itemPIC.OrgId,
            //                    Position = itemPIC.OrgPositionName,
            //                    ChangeStatusTime = itemPIC.ActionDate == null ? (actionUser == ApplicationConstant.WORKFLOW_STATUS_PENDING ? ApplicationConstant.MAX_DATETIME : eDate) : itemPIC.ActionDate
            //                };

            //                eDate = addWorkflowHistory.DateTime;

            //                workflowQcSamplingHistory.Add(addWorkflowHistory);
            //            }
            //        }
            //        else if (item.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2)
            //        {
            //            foreach (var itemPIC in itemHistory.PICs)
            //            {
            //                if (itemPIC.ActionName != ApplicationConstant.WORKFLOW_ACTION_SUBMIT_REJECT_NAME)
            //                {
            //                    string actionUser = "";
            //                    if (itemPIC.ActionName == null && itemPIC.IsPending == true)
            //                    {
            //                        actionUser = ApplicationConstant.WORKFLOW_STATUS_PENDING;
            //                    }
            //                    else if (itemPIC.ActionName == null && itemPIC.IsPending == false)
            //                    {
            //                        actionUser = "Close By System";
            //                    }
            //                    else if (itemPIC.ActionName == ApplicationConstant.WORKFLOW_ACTION_REJECT_COMPLETE_NAME)
            //                    {
            //                        actionUser = "Reject";
            //                    }
            //                    else
            //                    {
            //                        actionUser = itemPIC.ActionName;
            //                    }
            //                    WorkflowHistoryQcSampling addWorkflowHistory = new WorkflowHistoryQcSampling()
            //                    {
            //                        Action = actionUser,
            //                        Note = itemPIC.Notes,
            //                        DateTime = itemPIC.ActionDate == null ? eDate2 : itemPIC.ActionDate,
            //                        PersonalName = itemPIC.OrgName,
            //                        PersonalNik = itemPIC.OrgId,
            //                        Position = itemPIC.OrgPositionName,
            //                        ChangeStatusTime = itemPIC.ActionDate == null ? (actionUser == ApplicationConstant.WORKFLOW_STATUS_PENDING ? ApplicationConstant.MAX_DATETIME : eDate2) : itemPIC.ActionDate
            //                    };
            //                    eDate2 = addWorkflowHistory.DateTime;

            //                    if (itemHistory.StatusName != ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT || addWorkflowHistory.Action != ApplicationConstant.WORKFLOW_ACTION_SUBMIT_NAME)
            //                    {
            //                        workflowQcSamplingHistory.Add(addWorkflowHistory);
            //                    }

            //                }
            //            }
            //        }
            //    }
            //}
            List<WorkflowHistoryQcSampling> workflowHistoryQcs =
            workflowQcSamplingHistory.OrderByDescending(x => x.ChangeStatusTime).ToList();

            #endregion

            WorkflowQcSampling currentWorkflow = await _workflowQcSamplingDataProvider.GetByWorkflowByQcSamplingIdIsInWorkflow(id);
            var list_samples = (from sml in _context.QcSamples
                                 join tps in _context.TransactionTestParameter on sml.TestParamId equals tps.Id into tempParam
                                 from jm in tempParam.DefaultIfEmpty()
                                 where sml.QcSamplingId == id && sml.RowStatus == null && sml.ParentId == null
                                 select new QcSampleViewModel
                                 {
                                     Id = sml.Id,
                                     ParentId = (sml.ParentId != 0 ? sml.ParentId : 0),
                                     SampleSequence = (sml.SampleSequence != 0 ? sml.SampleSequence : 0),
                                     QcSamplingId = sml.QcSamplingId,
                                     Code = sml.Code,
                                     SamplingPointId = (sml.SamplingPointId == null ? null : sml.SamplingPointId),
                                     SamplingPointCode = sml.SamplingPointCode,
                                     ToolId = (sml.ToolId != 0 ? sml.ToolId : 0),
                                     ToolCode = sml.ToolCode,
                                     ToolName = sml.ToolName,
                                     ToolGroupId = (sml.ToolGroupId != 0 ? sml.ToolGroupId : 0),
                                     ToolGroupName = sml.ToolGroupName,
                                     ToolGroupLabel = sml.ToolGroupLabel,
                                     SelectedTool = (from st in _context.QcSamplingTools
                                                  join sample in _context.QcSamples on st.Id equals sample.QcSamplingToolsId
                                                  where sample.QcSamplingId == id && sample.Id == sml.Id
                                                  select new QcSamplingToolsViewModel
                                                      {
                                                          Id = st.Id,
                                                          QcSamplingId = st.QcSamplingId,
                                                          ToolId = st.ToolId,
                                                          ToolCode = st.ToolCode,
                                                          ToolName = st.ToolName,
                                                          ToolGroupId = st.ToolGroupId,
                                                          ToolGroupName = st.ToolGroupName,
                                                          ToolGroupLabel = st.ToolGroupLabel,
                                                          EdCalibration = st.EdCalibration,
                                                          EdValidation = st.EdValidation,
                                                          CreatedBy = st.CreatedBy,
                                                          CreatedAt = st.CreatedAt
                                                      }).FirstOrDefault(),
                                     SelectedMaterial = (from sm in _context.QcSamplingMaterials
                                                     join sample in _context.QcSamples on sm.Id equals sample.QcSamplingMaterialsId
                                                     where sample.QcSamplingId == id && sample.Id == sml.Id
                                                          select new QcSamplingMaterialViewModel
                                                          {
                                                              Id = sm.Id,
                                                              QcSamplingId = sm.QcSamplingId,
                                                              ItemId = sm.ItemId,
                                                              ItemName = sm.ItemName,
                                                              ItemBatchId = sm.ItemBatchId,
                                                              NoBatch = sm.NoBatch,
                                                              ExpireDate = sm.ExpireDate,
                                                              CreatedBy = sm.CreatedBy,
                                                              CreatedAt = sm.CreatedAt
                                                          }).FirstOrDefault(),
                                     GradeRoomId = sml.GradeRoomId,
                                     GradeRoomCode = (sml.GradeRoomId != null ? (from grd in _context.TransactionGradeRoom
                                                                                 where grd.Id == sml.GradeRoomId
                                                                                 select grd.Code).FirstOrDefault() : null),
                                     GradeRoomName = sml.GradeRoomName,
                                     TestParamId = sml.TestParamId,
                                     TestParamName = sml.TestParamName,
                                     TestParamSequence = jm.Sequence,
                                     PersonalId = (sml.PersonalId == null ? null : sml.PersonalId),
                                     PersonalInitial = sml.PersonalInitial,
                                     PersonalName = sml.PersonalName,
                                     SamplingDateTimeFrom = (sml.SamplingDateTimeFrom != null ? sml.SamplingDateTimeFrom : null),
                                     SamplingDateTimeTo = (sml.SamplingDateTimeTo != null ? sml.SamplingDateTimeTo : null),
                                     ParticleVolume = (sml.ParticleVolume != 0 ? sml.ParticleVolume : 0),
                                     AttchmentFile = sml.AttchmentFile,
                                     Note = sml.Note,
                                     CreatedBy = sml.CreatedBy,
                                     CreatedAt = sml.CreatedAt,
                                     TestScenarioId = sml.TestScenarioId,
                                     ReviewQaNote = sml.ReviewQaNote,
                                     SampleChild = (from smlc in _context.QcSamples
                                                    where smlc.ParentId == sml.Id
                                                    select new QcSampleChildViewModel
                                                    {
                                                        Id = smlc.Id,
                                                        ParentId = smlc.ParentId,
                                                        SampleSequence = (smlc.SampleSequence != 0 ? smlc.SampleSequence : 0),
                                                        ParticleVolume = smlc.ParticleVolume,
                                                        AttchmentFile = smlc.AttchmentFile,
                                                        SamplingDateTimeFrom = smlc.SamplingDateTimeFrom,
                                                        SamplingDateTimeTo = smlc.SamplingDateTimeTo,
                                                        ReviewQaNote = sml.ReviewQaNote,
                                                        Note = smlc.Note
                                                    }).ToList(),
                                     Purpose = (from rp in _context.TransactionRoomPurpose
                                                join rgrp in _context.RequestGroupRoomPurpose on rp.Id equals rgrp.RoomPurposeId
                                                join s in _context.QcSamplings on rgrp.RequestQcsId equals s.RequestQcsId
                                                join rsp in _context.TransactionRelRoomSamplingPoint on rp.Id equals rsp.RoomPurposeId
                                                join rpmtp in _context.TransactionRoomPurposeToMasterPurpose on rp.Id equals rpmtp.RoomPurposeId
                                                join purp in _context.TransactionPurposes on rpmtp.PurposeId equals purp.Id
                                                where s.Id == sml.QcSamplingId
                                                && rsp.SamplingPoinId == sml.SamplingPointId
                                                select new RequestPurposesViewModel
                                                {
                                                    PurposeId = purp.Id,
                                                    PurposeCode = purp.Code,
                                                    PurposeName = purp.Name
                                                }).Union(from tp in _context.TransactionToolPurpose
                                                         join tgrp in _context.RequestGroupToolPurpose on tp.Id equals tgrp.ToolPurposeId
                                                         join s in _context.QcSamplings on tgrp.RequestQcsId equals s.RequestQcsId
                                                         join rts in _context.TransactionRelSamplingTool on tp.Id equals rts.ToolPurposeId
                                                         join tpmp in _context.TransactionToolPurposeToMasterPurpose on tp.Id equals tpmp.ToolPurposeId
                                                         join purp in _context.TransactionPurposes on tpmp.PurposeId equals purp.Id
                                                         where s.Id == sml.QcSamplingId
                                                         && rts.SamplingPoinId == sml.SamplingPointId
                                                         select new RequestPurposesViewModel
                                                         {
                                                             PurposeId = purp.Id,
                                                             PurposeCode = purp.Code,
                                                             PurposeName = purp.Name
                                                         }).ToList(),
                                     TestParamIndex = 1,
                                     IsDefault = sml.IsDefault,
                                     //QcSamplingToolsId = sml.QcSamplingToolsId,
                                     //QcSamplingMaterialsId = sml.QcSamplingMaterialsId
                                 })
                                 .AsQueryable();
            var sample_type = await _context.QcSamplings.FirstOrDefaultAsync(e => e.Id == id);
            if (sort == "DESC")
            {
                if (sample_type.SamplingTypeName == "EM-PC")
                {
                    list_samples = list_samples
                        .OrderByDescending(e => e.SamplingDateTimeFrom)
                        .ThenByDescending(e => e.SamplingDateTimeTo)
                        .ThenBy(e => e.Id);
                }
                else
                {
                    list_samples = list_samples
                        .OrderBy(e => e.TestParamName)
                        .ThenBy(e => e.SamplingPointCode)
                        .ThenByDescending(e => e.SamplingDateTimeFrom)
                        .ThenByDescending(e => e.SamplingDateTimeTo)
                        .ThenBy(e => e.Id);
                }
            }
            else
            {
                if (sample_type.SamplingTypeName == "EM-PC")
                {
                    list_samples = list_samples
                        .OrderBy(e => e.SamplingDateTimeFrom)
                        .ThenBy(e => e.SamplingDateTimeTo)
                        .ThenBy(e => e.Id);
                }
                else
                {
                    list_samples = list_samples
                        .OrderBy(e => e.TestParamName)
                        .ThenBy(e => e.SamplingPointCode)
                        .ThenBy(e => e.SamplingDateTimeFrom)
                        .ThenBy(e => e.SamplingDateTimeTo)
                        .ThenBy(e => e.Id);
                }
            }
            var samples = await list_samples.ToListAsync();
            foreach (var itemSample in samples)
            {
                var purposeRoom = await (from rp in _context.TransactionRoomPurpose
                                         join rgrp in _context.RequestGroupRoomPurpose on rp.Id equals rgrp.RoomPurposeId
                                         join s in _context.QcSamplings on rgrp.RequestQcsId equals s.RequestQcsId
                                         join rsp in _context.TransactionRelRoomSamplingPoint on rp.Id equals rsp.RoomPurposeId
                                         join rpmtp in _context.TransactionRoomPurposeToMasterPurpose on rp.Id equals rpmtp.RoomPurposeId
                                         join purp in _context.TransactionPurposes on rpmtp.PurposeId equals purp.Id
                                         where s.Id == itemSample.QcSamplingId
                                         && (purposes.Select(x => x.PurposeId).ToList()).Contains(purp.Id)
                                         && rsp.SamplingPoinId == itemSample.SamplingPointId
                                         select new RequestPurposesViewModel
                                         {
                                             PurposeId = purp.Id,
                                             PurposeCode = purp.Code,
                                             PurposeName = purp.Name
                                         }).Distinct().ToListAsync();

                var purposeTool = await (from tp in _context.TransactionToolPurpose
                                         join tgrp in _context.RequestGroupToolPurpose on tp.Id equals tgrp.ToolPurposeId
                                         join s in _context.QcSamplings on tgrp.RequestQcsId equals s.RequestQcsId
                                         join rts in _context.TransactionRelSamplingTool on tp.Id equals rts.ToolPurposeId
                                         join tpmp in _context.TransactionToolPurposeToMasterPurpose on tp.Id equals tpmp.ToolPurposeId
                                         join purp in _context.TransactionPurposes on tpmp.PurposeId equals purp.Id
                                         where s.Id == itemSample.QcSamplingId
                                         && (purposes.Select(x => x.PurposeId).ToList()).Contains(purp.Id)
                                         && rts.SamplingPoinId == itemSample.SamplingPointId
                                         select new RequestPurposesViewModel
                                         {
                                             PurposeId = purp.Id,
                                             PurposeCode = purp.Code,
                                             PurposeName = purp.Name
                                         }).Distinct().ToListAsync();
                purposeRoom.AddRange(purposeTool);

                //purpose untuk tes parameter jenis finger DB
                if ((itemSample.TestParamName == ApplicationConstant.TEST_PARAMETER_LABEL_FD) || (itemSample.TestParamId == ApplicationConstant.TEST_PARAMETER_FD))
                {
                    purposeRoom.AddRange(purposes);
                }
                itemSample.Purpose = purposeRoom.Distinct().ToList();

            }

            var requestRooms = await (from s in _context.QcSamplings
                                      join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                      where s.Id == id && r.RowStatus == null
                                      from rr in _context.RequestRooms
                                      where rr.QcRequestId == r.Id
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
                                      }).ToListAsync();

            var result = await (from s in _context.QcSamplings
                                join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                where s.Id == id && r.RowStatus == null
                                select new QcSamplingRelationViewModel
                                {
                                    SamplingId = s.Id,
                                    RequestId = r.Id,
                                    Code = s.Code,
                                    Date = r.Date,
                                    NoRequest = r.NoRequest,
                                    TypeRequestId = r.TypeRequestId,
                                    TypeRequest = r.TypeRequest,
                                    SamplingTypeId = s.SamplingTypeId,
                                    SamplingTypeName = s.SamplingTypeName,
                                    SamplingDateFrom = (s.SamplingDateFrom != null ? s.SamplingDateFrom : null),
                                    SamplingDateTo = (s.SamplingDateTo != null ? s.SamplingDateTo : null),
                                    ItemId = r.ItemId,
                                    ItemCode = (r.ItemId != null ? (from mi in _context.Items
                                                                    where mi.Id == r.ItemId
                                                                    select mi.ItemCode).FirstOrDefault() : null),
                                    ItemName = r.ItemName,
                                    StorageTemperatureId = r.StorageTemperatureId,
                                    StorageTemperatureName = r.StorageTemperatureName,
                                    TypeFormId = r.ProductFormId,
                                    TypeFormName = r.ProductFormName,
                                    TestScenarioLabel = r.TestScenarioLabel,
                                    NoBatch = r.NoBatch,
                                    EmPhaseId = r.EmPhaseId,
                                    EmPhaseName = r.EmPhaseName,
                                    Status = s.Status,
                                    AttchmentFile = s.AttchmentFile,
                                    Note = s.Note,
                                    OrgId = r.OrgId,
                                    OrgName = r.OrgName,
                                    FacilityId = r.FacilityId,
                                    FacilityCode = r.FacilityCode,
                                    FacilityName = r.FacilityName,
                                    Location = r.Location,
                                    ProcessId = r.ProcessId,
                                    ProcessName = r.ProcessName,
                                    ProcessDate = r.ProcessDate,
                                    ItemTemperature = r.ItemTemperature,
                                    IsNoBatchEditable = r.IsNoBatchEditable,

                                    CreatedBy = s.CreatedBy,
                                    CreatedAt = s.CreatedAt,
                                    WorkflowCode = currentWorkflow != null ? currentWorkflow.WorkflowCode : null,
                                    ProductFormId = r.ProductFormId,
                                    ProductFormName = r.ProductFormName,
                                    ProductGroupId = r.ProductGroupId,
                                    ProductGroupName = r.ProductGroupName,
                                    ProductDate = (s.ProductDate != null ? s.ProductDate : null),
                                    ProductMethodId = s.ProductMethodId,
                                    ProductMethodName = (s.ProductMethodId == ApplicationConstant.PRODUCT_METHOD_ID_ASEPTIC
                                                        ? ApplicationConstant.PRODUCT_METHOD_NAME_ASEPTIC
                                                        : (s.ProductMethodId == ApplicationConstant.PRODUCT_METHOD_ID_NON_ASEPTIC
                                                        ? ApplicationConstant.PRODUCT_METHOD_NAME_NON_ASEPTIC
                                                        : null)),
                                    ProductShipmentTemperature = s.ProductShipmentTemperature,
                                    ProductShipmentDate = (s.ProductShipmentDate != null ? s.ProductShipmentDate : null),
                                    ProductDataLogger = s.ProductDataLogger,
                                    ProductPhaseId = r.ProductPhaseId,
                                    ProductPhaseName = r.ProductPhaseName,
                                    ProductTestTypeQcs = (from t in _context.ProductTestTypeQcs
                                                          join tps in _context.TransactionTestParameter on t.TestParameterId equals tps.Id into tempParam
                                                          from jm in tempParam.DefaultIfEmpty()
                                                          where t.RequestQcsId == r.Id
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
                                                              SampleAmountUnit = t.SampleAmountUnit,
                                                              SampleAmountPresentation = t.SampleAmountPresentation,
                                                          }).OrderBy(x => x.TestParameterSequence).ToList(),
                                    RequestAhu = (from ra in _context.RequestAhus
                                                  where ra.QcRequestId == r.Id
                                                  && ra.RowStatus == null
                                                  select new RequestAhuViewModel
                                                  {
                                                      Id = ra.Id,
                                                      AhuId = ra.AhuId,
                                                      AhuCode = ra.AhuCode,
                                                      AhuName = ra.AhuName
                                                  }).ToList(),
                                    RequestRooms = requestRooms,
                                    RequestPurposes = (from rp in _context.RequestPurposes
                                                       where rp.QcRequestId == r.Id
                                                       && rp.RowStatus == null
                                                       orderby rp.Id
                                                       select new RequestPurposesViewModel
                                                       {
                                                           Id = rp.Id,
                                                           PurposeId = rp.PurposeId,
                                                           PurposeCode = rp.PurposeCode,
                                                           PurposeName = rp.PurposeName
                                                       }).ToList(),
                                    SamplingTools = (from st in _context.QcSamplingTools
                                                     where st.QcSamplingId == id && st.RowStatus == null
                                                     select new QcSamplingToolsViewModel
                                                     {
                                                         Id = st.Id,
                                                         QcSamplingId = st.QcSamplingId,
                                                         ToolId = st.ToolId,
                                                         ToolCode = st.ToolCode,
                                                         ToolName = st.ToolName,
                                                         ToolGroupId = st.ToolGroupId,
                                                         ToolGroupName = st.ToolGroupName,
                                                         ToolGroupLabel = st.ToolGroupLabel,
                                                         EdCalibration = st.EdCalibration,
                                                         EdValidation = st.EdValidation,
                                                         CreatedBy = st.CreatedBy,
                                                         CreatedAt = st.CreatedAt
                                                     }).ToList(),
                                    SamplingMaterials = (from sm in _context.QcSamplingMaterials
                                                         where sm.QcSamplingId == id && sm.RowStatus == null
                                                         select new QcSamplingMaterialViewModel
                                                         {
                                                             Id = sm.Id,
                                                             QcSamplingId = sm.QcSamplingId,
                                                             ItemId = sm.ItemId,
                                                             ItemName = sm.ItemName,
                                                             ItemBatchId = sm.ItemBatchId,
                                                             NoBatch = sm.NoBatch,
                                                             Quantity = (from itemBatchNumber in _context.ItemBatchNumbers
                                                                 where itemBatchNumber.RowStatus == null && itemBatchNumber.BatchNumber == sm.NoBatch
                                                                 select new {itemBatchNumber.Quantity}).FirstOrDefault().Quantity,
                                                             ExpireDate = sm.ExpireDate,
                                                             CreatedBy = sm.CreatedBy,
                                                             CreatedAt = sm.CreatedAt
                                                         }).ToList(),
                                    SamplingAttachments = (from stc in _context.QcSamplingAttachment
                                                           where stc.QcSamplingId == id && stc.RowStatus == null
                                                           select new QcSamplingAttachmentViewModel
                                                           {
                                                               Id = stc.Id,
                                                               AttachmentFileName = stc.AttachmentFileName,
                                                               AttachmentStorageName = stc.AttachmentStorageName
                                                           }).ToList(),
                                    SamplingPersonels = (from qsp in _context.QcSamplingPersonels
                                                         where qsp.QcSamplingId == id && qsp.RowStatus == null
                                                         select new QcSamplingPersonelViewModel
                                                         {
                                                             SamplingPersonelId = qsp.Id,
                                                             ProductProdPhasesPersonelId = qsp.ProductProductionPhasesPersonelId,
                                                             PersonelNik = qsp.PersonelNik,
                                                             PersonelName = qsp.PersonelName
                                                         }).ToList(),
                                    SampleData = samples,
                                    WorkflowHistory = workflowHistoryQcs
                                }).ToListAsync();

            var requestIds = result.Select(x => x.RequestId).Distinct().ToList();
            var batches = await _transactionBatchDataProvider.GetByRequestIds(requestIds);

            foreach (var res in result)
            {
                #region select graderoom will used to sampling
                // select graderoom will used to sampling ------------------------------------------
                var gradeRoomIds = new List<int?>();
                if (res.RequestRooms.Any())
                {
                    res.SamplesGradeRooms = await (from qs in _context.QcSamples
                                                   join gr in _context.TransactionGradeRoom on qs.GradeRoomId equals gr.Id
                                                   where qs.QcSamplingId == id && qs.RowStatus == null && qs.GradeRoomId != null
                                                   group new { qs, gr } by new { qs.GradeRoomId } into g
                                                   select new QcSamplingGradeRoomViewModel
                                                   {
                                                       GradeRoomId = g.Max(x => x.gr.Id),
                                                       GradeRoomCode = g.Max(x => x.gr.Code),
                                                       GradeRoomName = g.Max(x => x.gr.Name),
                                                   }).ToListAsync();

                    foreach (var grId in res.SamplesGradeRooms)
                    {
                        gradeRoomIds.Add(grId.GradeRoomId);
                    }
                }
                #endregion

                #region TestParameterSampling
                // TestParameterSampling ------------------------------------------
                res.TestParameterSampling = await (from rralt in _context.RequestRooms
                                                   where rralt.QcRequestId == res.RequestId
                                                   && rralt.RowStatus == null
                                                   select new RoomsTestTypeViewModel
                                                   {
                                                       RoomId = rralt.RoomId,
                                                       RoomCode = rralt.RoomCode,
                                                       RoomName = rralt.RoomName,
                                                       TestScenarioId = rralt.TestScenarioId,
                                                       TestScenarioName = rralt.TestScenarioName,
                                                       TestScenarioLabel = rralt.TestScenarioLabel,
                                                   }).ToListAsync();

                if (res.TestParameterSampling.Any())
                {
                    List<int> allTestParameterIds = new List<int>();
                    List<int> allRoomIds = new List<int>();
                    List<int> allTestScenarioIds = new List<int>();
                    foreach (var tps in res.TestParameterSampling)
                    {
                        tps.TestParameterSamplingDetail = await (
                            from tpts in _context.TransactionTestParameter
                            orderby tpts.Sequence
                            select new EmTestTypeViewModel
                            {
                                TestParameterId = tpts.Id,
                                TestParameterName = tpts.Name,
                                RoomId = tps.RoomId,
                                TestParameterSquence = tpts.Sequence,
                                TestScenarioId = tps.TestScenarioId,
                                ThresholdRoomSamplingPoint = (tpts.Id == ApplicationConstant.TEST_PARAMETER_GV ?
                                    0 : (from room_purp in _context.TransactionRoomPurpose
                                         join rrsp in _context.TransactionRelRoomSamplingPoint on room_purp.Id equals rrsp.RoomPurposeId
                                         where room_purp.RoomId == tps.RoomId
                                         select rrsp.SamplingPoinId).Count()),
                                ThresholdToolSamplingPoint = (
                                    from a in (from rst in _context.TransactionRelSamplingTool
                                               join tp in _context.TransactionToolPurpose on rst.ToolPurposeId equals tp.Id
                                               join t in _context.Tools on tp.ToolId equals t.Id
                                               group new { rst, t } by new
                                               {
                                                   rst.SamplingPoinId,
                                                   t.RoomId
                                               } into g
                                               where g.Key.RoomId == tps.RoomId
                                               select new
                                               { SpId = g.Key.SamplingPoinId })
                                    select a.SpId).Count()
                            }).ToListAsync();

                        var testParameterIds = tps.TestParameterSamplingDetail
                            .Select(x => x.TestParameterId)
                            .ToList();
                        allTestParameterIds.AddRange(testParameterIds);

                        var roomIds = tps.TestParameterSamplingDetail
                            .Select(x => x.RoomId ?? 0)
                            .ToList();
                        allRoomIds.AddRange(roomIds);

                        var testScenarioIds = tps.TestParameterSamplingDetail
                            .Select(x => x.TestScenarioId)
                            .ToList();
                        allTestScenarioIds.AddRange(testScenarioIds);
                    }

                    #region get CountParamater
                    allRoomIds.RemoveAll(x => x == 0);
                    allTestParameterIds = allTestParameterIds.Distinct().ToList();
                    allRoomIds = allRoomIds.Distinct().ToList();
                    allTestScenarioIds = allTestScenarioIds.Distinct().ToList();
                    var countParameters = await (
                        from tpr in _context.TestParamRoomModels
                        where allTestParameterIds.Contains(tpr.TestParameterId)
                        && allRoomIds.Contains(tpr.RoomId)
                        && allTestScenarioIds.Contains(tpr.TestScenarioId)
                        select new
                        {
                            TestParameterId = tpr.TestParameterId,
                            RoomId = tpr.RoomId,
                            TestScenarioId = tpr.TestScenarioId,
                            TotalTestParameter = tpr.TotalTestParameter
                        }
                    ).ToListAsync();

                    foreach (var tps in res.TestParameterSampling)
                    {
                        foreach (var tpsDetail in tps.TestParameterSamplingDetail)
                        {
                            tpsDetail.CountParamater = countParameters
                                .Where(x => x.TestParameterId == tpsDetail.TestParameterId
                                    && x.RoomId == tpsDetail.RoomId
                                    && x.TestScenarioId == tpsDetail.TestScenarioId)
                                .Select(x => x.TotalTestParameter)
                                .FirstOrDefault();
                        }
                    }
                    #endregion
                }

                int newTestParamIndex = 1;
                var existingSamplingPointCode = new List<string>();
                var existingTestParamIndex = new List<int>();
                int samplingPointCodeIndex = 0;
                int testParamIndex = 0;
                
                int spPersiapannewTestParamIndex = 1;
                var spPersiapanexistingSamplingPointCode = new List<string>();
                var spPersiapanexistingTestParamIndex = new List<int>();
                int spPersiapansamplingPointCodeIndex = 0;
                int spPersiapantestParamIndex = 0;

                //get list sampleTestScenario by list of gradeRoomId
                var sampleTestScenarios = await (from rts in _context.TransactionRelGradeRoomScenario
                                                 join ts in _context.TransactionTestScenario on rts.TestScenarioId equals ts.Id
                                                 where gradeRoomIds.Contains(rts.GradeRoomId)
                                                 && ts.RowStatus == null
                                                 select new
                                                 {
                                                     GradeRoomId = rts.GradeRoomId,
                                                     TestScenarioId = ts.Id,
                                                     TestScenarioName = ts.Name,
                                                     TestScenarioLabel = ts.Label
                                                 }).ToListAsync();


                foreach (var sampData in res.SampleData)
                {
                    //set sampData.TestParamIndex jika sampData.TestParamId == 1
                    if (sampData.TestParamId == 1)
                    {
                        if (existingSamplingPointCode.Contains(sampData.SamplingPointCode))
                        {
                            samplingPointCodeIndex = existingSamplingPointCode.IndexOf(sampData.SamplingPointCode);
                            testParamIndex = existingTestParamIndex[samplingPointCodeIndex] + 1;
                            sampData.TestParamIndex = testParamIndex;
                            existingTestParamIndex[samplingPointCodeIndex] = testParamIndex;
                        }
                        else
                        {
                            existingSamplingPointCode.Add(sampData.SamplingPointCode);
                            existingTestParamIndex.Add(newTestParamIndex);
                        }
                    }
                    
                    //set sampData.TestParamIndex jika sampData.TestParamId == 1
                    else if (sampData.TestParamName == "SP Persiapan")
                    {
                        if (spPersiapanexistingSamplingPointCode.Contains(sampData.SamplingPointCode))
                        {
                            spPersiapansamplingPointCodeIndex = spPersiapanexistingSamplingPointCode.IndexOf(sampData.SamplingPointCode);
                            spPersiapantestParamIndex = spPersiapanexistingTestParamIndex[spPersiapansamplingPointCodeIndex] + 1;
                            sampData.TestParamIndex = spPersiapantestParamIndex;
                            spPersiapanexistingTestParamIndex[spPersiapansamplingPointCodeIndex] = spPersiapantestParamIndex;
                        }
                        else
                        {
                            spPersiapanexistingSamplingPointCode.Add(sampData.SamplingPointCode);
                            spPersiapanexistingTestParamIndex.Add(newTestParamIndex);
                        }
                    }

                    //set sampData.SampleTestScenario
                    if (sampData.GradeRoomId == null)
                    {
                        sampData.SampleTestScenario = sampleTestScenarios
                            .Select(x => new QcSampleTestScenarioViewModel
                            {
                                TestScenarioId = x.TestScenarioId,
                                TestScenarioName = x.TestScenarioName,
                                TestScenarioLabel = x.TestScenarioLabel
                            }).ToList();
                    }
                    else
                    {
                        //select sampleTestScenario by gradeRoomId
                        sampData.SampleTestScenario = sampleTestScenarios
                            .Where(x => x.GradeRoomId == sampData.GradeRoomId)
                            .Select(x => new QcSampleTestScenarioViewModel
                            {
                                TestScenarioId = x.TestScenarioId,
                                TestScenarioName = x.TestScenarioName,
                                TestScenarioLabel = x.TestScenarioLabel
                            }).ToList();
                    }
                }
                #endregion

                foreach (var matSampling in res.SamplingMaterials)
                {
                    matSampling.ItemBatchQuotation = await _dataProviderItem.ItemMediaQuotation(matSampling.NoBatch);
                }

                #region Transaction Batch
                res.Batch = batches.FirstOrDefault(x => x.RequestId == res.RequestId);
                #endregion

                #region generate link
                //foreach (var item in res.SamplingAttachments)
                //{
                //    item.AttachmentFileLink = await _uploadBusinessProvider.GenerateV4SignedReadUrl(item.AttachmentStorageName);
                //}
                #endregion
            }

            return result;
        }

        public async Task<QcSampling> Edit(QcSampling data, List<EditSampleQcSampleBindingModel> editSampleQcSamples, List<EditSampleQcPersonelBindingModel> editSampleQcPersonel, EditBatchRequestQcBindingModel editSampleBatch)
        {
            // list test parameter can remove data
            var testParamCanRemove = new List<int>();
            testParamCanRemove.Add(ApplicationConstant.TEST_PARAMETER_FD);
            testParamCanRemove.Add(ApplicationConstant.TEST_PARAMETER_SP);

            QcSampling result = new QcSampling();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (data.SamplingTypeId == 15 || data.SamplingTypeId == 16)
                    {
                        List<Int32> SampleQcSamplesIds = editSampleQcSamples.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();

                        //get Workflow status cek status untuk validasi
                        var validatonWFGet = await getRiviewKabagSampling(data.Id);

                        await _context.SaveChangesAsync();

                        #region process sample changes data
                        //PRECESS SAMPLE CHANGES DATA ------------------

                        // can remove sample FD & SP only
                        var deleteSamplingSampleData = await (from sp in _context.QcSamples
                                                              where !SampleQcSamplesIds.Contains(sp.Id)
                                                              && sp.QcSamplingId == data.Id && sp.ParentId == null
                                                              && testParamCanRemove.Contains(sp.TestParamId)
                                                              select sp).ToListAsync();
                        _context.QcSamples.RemoveRange(deleteSamplingSampleData);

                        var SamplingSampleDatas = await (from sp in _context.QcSamples
                                                         where SampleQcSamplesIds.Contains(sp.Id) && sp.QcSamplingId == data.Id && sp.ParentId == null
                                                         select sp).ToListAsync();

                        var lsTestScenario = await _testScenarioDataProvider.GetList();

                        var samplingSampleChildDatas = await (from sp in _context.QcSamples
                                                              where sp.QcSamplingId == data.Id
                                                              && sp.ParentId != null
                                                              select sp).ToListAsync();

                        var listMaterial = _context.QcSamplingMaterials.Where(e => e.QcSamplingId == data.Id).ToList();
                        var listTools = _context.QcSamplingTools.Where(e => e.QcSamplingId == data.Id).ToList();

                        for (int i = 0; i < SamplingSampleDatas.Count; i++)
                        {
                            var dataSample = SamplingSampleDatas[i];
                            var editSampling = editSampleQcSamples.Where(x => x.Id.Value == dataSample.Id).FirstOrDefault();

                            var sampleMaterial = listMaterial.Any() ? listMaterial.Where(x => x.ItemId == editSampling.ItemId && x.NoBatch == editSampling.NoBatch).FirstOrDefault() : null;
                            var sampleTool = listTools.Any() ? listTools.Where(x => x.ToolId == editSampling.ToolId && x.ToolCode == editSampling.ToolCode).FirstOrDefault() : null;

                            if (editSampling != null)
                            {
                                dataSample.ParentId = editSampling.ParentId;
                                dataSample.Code = updateCodeSample(dataSample.Code, lsTestScenario, editSampling.TestScenarioId);
                                dataSample.SampleSequence = editSampling.SampleSequence;
                                dataSample.SamplingPointId = editSampling.SamplingPointId;
                                dataSample.SamplingPointCode = editSampling.SamplingPointCode;

                                if (validatonWFGet == true)
                                    dataSample.TestScenarioId = editSampling.TestScenarioId;

                                dataSample.GradeRoomId = editSampling.GradeRoomId;
                                dataSample.GradeRoomName = editSampling.GradeRoomName;
                                dataSample.TestParamId = editSampling.TestParamId;
                                dataSample.TestParamName = editSampling.TestParamName;

                                dataSample.ToolId = editSampling.ToolId;
                                dataSample.ToolCode = editSampling.ToolCode;
                                dataSample.ToolName = editSampling.ToolName;
                                dataSample.ToolGroupId = editSampling.ToolGroupId;
                                dataSample.ToolGroupName = editSampling.ToolGroupName;
                                dataSample.ToolGroupLabel = editSampling.ToolGroupLabel;

                                dataSample.PersonalId = editSampling.PersonalId;
                                dataSample.PersonalInitial = editSampling.PersonalInitial;
                                dataSample.PersonalName = editSampling.PersonalName;
                                dataSample.SamplingDateTimeFrom = editSampling.SamplingDateTimeFrom;
                                dataSample.SamplingDateTimeTo = editSampling.SamplingDateTimeTo;
                                dataSample.ParticleVolume = (editSampling.ParticleVolume == null ? 0 : editSampling.ParticleVolume);
                                dataSample.AttchmentFile = editSampling.AttchmentFile;
                                dataSample.Note = editSampling.Note;
                                dataSample.UpdatedBy = data.UpdatedBy;
                                dataSample.UpdatedAt = DateHelper.Now();

                                dataSample.QcSamplingToolsId = editSampling.QcSamplingToolsId != null ? editSampling.QcSamplingToolsId : sampleTool?.Id;
                                dataSample.QcSamplingMaterialsId = editSampling.QcSamplingMaterialsId != null ? editSampling.QcSamplingMaterialsId : sampleMaterial?.Id;

                                var SampleChildDataEdit = editSampling.SampleChild;

                                //state sampling child data edit

                                if (SampleChildDataEdit != null)
                                {
                                    List<Int32> SampleQcSamplesChildIds = editSampling.SampleChild.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();

                                    var deleteSamplingSampleChildData = samplingSampleChildDatas
                                        .Where(x => !SampleQcSamplesChildIds.Contains(x.Id) && x.ParentId == editSampling.Id)
                                        .ToList();

                                    var SamplingSampleChildDatas = samplingSampleChildDatas
                                        .Where(x => SampleQcSamplesChildIds.Contains(x.Id) && x.ParentId == editSampling.Id)
                                        .ToList();

                                    //loop edit sampling child
                                    #region sample child - edit

                                    foreach (var dataSampleChild in SamplingSampleChildDatas)
                                    {
                                        var editSamplingChild = SampleChildDataEdit.Where(x => x.Id.Value == dataSampleChild.Id).FirstOrDefault();
                                        if (editSamplingChild != null)
                                        {
                                            dataSampleChild.ParentId = editSampling.Id;
                                            dataSampleChild.SampleSequence = editSamplingChild.SampleSequence;
                                            dataSampleChild.SamplingPointId = editSampling.SamplingPointId;
                                            dataSampleChild.SamplingPointCode = editSampling.SamplingPointCode;
                                            dataSampleChild.GradeRoomId = editSampling.GradeRoomId;
                                            dataSampleChild.GradeRoomName = editSampling.GradeRoomName;
                                            dataSampleChild.TestParamId = editSampling.TestParamId;
                                            dataSampleChild.TestParamName = editSampling.TestParamName;

                                            dataSampleChild.ToolId = editSampling.ToolId;
                                            dataSampleChild.ToolCode = editSampling.ToolCode;
                                            dataSampleChild.ToolName = editSampling.ToolName;
                                            dataSampleChild.ToolGroupId = editSampling.ToolGroupId;
                                            dataSampleChild.ToolGroupName = editSampling.ToolGroupName;
                                            dataSampleChild.ToolGroupLabel = editSampling.ToolGroupLabel;

                                            dataSampleChild.PersonalId = editSampling.PersonalId;
                                            dataSampleChild.PersonalInitial = editSampling.PersonalInitial;
                                            dataSampleChild.PersonalName = editSampling.PersonalName;
                                            dataSampleChild.SamplingDateTimeFrom = editSamplingChild.SamplingDateTimeFrom;
                                            dataSampleChild.SamplingDateTimeTo = editSamplingChild.SamplingDateTimeTo;
                                            dataSampleChild.ParticleVolume = (editSamplingChild.ParticleVolume == null ? 0 : editSamplingChild.ParticleVolume);
                                            dataSampleChild.AttchmentFile = editSamplingChild.AttchmentFile;
                                            dataSampleChild.Note = editSamplingChild.Note;
                                            dataSampleChild.UpdatedBy = data.UpdatedBy;
                                            dataSampleChild.UpdatedAt = DateHelper.Now();

                                            dataSampleChild.QcSamplingToolsId = editSampling.QcSamplingToolsId != null ? editSampling.QcSamplingToolsId : sampleTool?.Id;
                                            dataSampleChild.QcSamplingMaterialsId = editSampling.QcSamplingMaterialsId != null ? editSampling.QcSamplingMaterialsId : sampleMaterial?.Id;
                                        }
                                    }
                                    #endregion

                                    #region sample child - add

                                    var addSamplingSampleChild = SampleChildDataEdit.Where(x => !x.Id.HasValue).ToList();

                                    List<QcSample> listAddSamplingSampleChild = new List<QcSample>();
                                    foreach (var addDataSamplingSampleChild in addSamplingSampleChild)
                                    {
                                        QcSample newSamplingSampleChildInsert = new QcSample();

                                        var samplingDateTimeFrom = addDataSamplingSampleChild.SamplingDateTimeFrom ?? editSampling.SamplingDateTimeFrom;
                                        var samplingDateTimeTo = addDataSamplingSampleChild.SamplingDateTimeTo ?? editSampling.SamplingDateTimeTo;

                                        newSamplingSampleChildInsert.QcSamplingId = data.Id;
                                        newSamplingSampleChildInsert.ParentId = editSampling.Id;
                                        newSamplingSampleChildInsert.SampleSequence = addDataSamplingSampleChild.SampleSequence;
                                        newSamplingSampleChildInsert.SamplingPointId = editSampling.SamplingPointId;
                                        newSamplingSampleChildInsert.SamplingPointCode = editSampling.SamplingPointCode;
                                        newSamplingSampleChildInsert.GradeRoomId = editSampling.GradeRoomId;
                                        newSamplingSampleChildInsert.GradeRoomName = editSampling.GradeRoomName;

                                        newSamplingSampleChildInsert.ToolId = editSampling.ToolId;
                                        newSamplingSampleChildInsert.ToolCode = editSampling.ToolCode;
                                        newSamplingSampleChildInsert.ToolName = editSampling.ToolName;
                                        newSamplingSampleChildInsert.ToolGroupId = editSampling.ToolGroupId;
                                        newSamplingSampleChildInsert.ToolGroupName = editSampling.ToolGroupName;
                                        newSamplingSampleChildInsert.ToolGroupLabel = editSampling.ToolGroupLabel;

                                        newSamplingSampleChildInsert.TestParamId = editSampling.TestParamId;
                                        newSamplingSampleChildInsert.TestParamName = editSampling.TestParamName;
                                        newSamplingSampleChildInsert.PersonalId = editSampling.PersonalId;
                                        newSamplingSampleChildInsert.PersonalInitial = editSampling.PersonalInitial;
                                        newSamplingSampleChildInsert.PersonalName = editSampling.PersonalName;
                                        newSamplingSampleChildInsert.SamplingDateTimeFrom = samplingDateTimeFrom;
                                        newSamplingSampleChildInsert.SamplingDateTimeTo = samplingDateTimeTo;
                                        newSamplingSampleChildInsert.ParticleVolume = (addDataSamplingSampleChild.ParticleVolume == null ? 0 : addDataSamplingSampleChild.ParticleVolume);
                                        newSamplingSampleChildInsert.AttchmentFile = addDataSamplingSampleChild.AttchmentFile;
                                        newSamplingSampleChildInsert.Note = addDataSamplingSampleChild.Note;
                                        newSamplingSampleChildInsert.CreatedBy = data.UpdatedBy;
                                        newSamplingSampleChildInsert.UpdatedBy = data.UpdatedBy;
                                        newSamplingSampleChildInsert.CreatedAt = DateHelper.Now();
                                        newSamplingSampleChildInsert.UpdatedAt = DateHelper.Now();

                                        newSamplingSampleChildInsert.QcSamplingToolsId = editSampling.QcSamplingToolsId != null ? editSampling.QcSamplingToolsId : sampleTool?.Id;
                                        newSamplingSampleChildInsert.QcSamplingMaterialsId = editSampling.QcSamplingMaterialsId != null ? editSampling.QcSamplingMaterialsId : sampleMaterial?.Id;

                                        // _context.QcSamples.Add(newSamplingSampleChildInsert);
                                        listAddSamplingSampleChild.Add(newSamplingSampleChildInsert);
                                    }
                                    _context.QcSamples.AddRange(listAddSamplingSampleChild);

                                    #endregion
                                }
                            }
                        }

                        //state add sampling new data
                        #region sample baru
                        if (validatonWFGet == true) //draft conndition can add sample
                        {
                            var addSamplingSample = editSampleQcSamples.Where(x => !x.Id.HasValue).ToList();

                            List<QcSample> listAddSamplingSample = new List<QcSample>();
                            foreach (var addDataSamplingSample in addSamplingSample)
                            {
                                QcSample newSamplingSampleInsert = new QcSample();
                                var sampleMaterial = listMaterial.Any() ? listMaterial.Where(x => x.ItemId == addDataSamplingSample.ItemId && x.NoBatch == addDataSamplingSample.NoBatch).FirstOrDefault() : null;
                                var sampleTool = listTools.Any() ? listTools.Where(x => x.ToolId == addDataSamplingSample.ToolId && x.ToolCode == addDataSamplingSample.ToolCode).FirstOrDefault() : null;

                                newSamplingSampleInsert.QcSamplingId = data.Id;
                                newSamplingSampleInsert.Code = updateCodeSample(generateCodeSample(6), lsTestScenario, addDataSamplingSample.TestScenarioId);//TODO 6 pindahkan ke constant
                                newSamplingSampleInsert.ParentId = addDataSamplingSample.ParentId;
                                newSamplingSampleInsert.SampleSequence = addDataSamplingSample.SampleSequence;
                                newSamplingSampleInsert.SamplingPointId = addDataSamplingSample.SamplingPointId;
                                newSamplingSampleInsert.SamplingPointCode = addDataSamplingSample.SamplingPointCode;
                                newSamplingSampleInsert.TestScenarioId = addDataSamplingSample.TestScenarioId;
                                newSamplingSampleInsert.GradeRoomId = addDataSamplingSample.GradeRoomId;
                                newSamplingSampleInsert.GradeRoomName = addDataSamplingSample.GradeRoomName;

                                newSamplingSampleInsert.ToolId = addDataSamplingSample.ToolId;
                                newSamplingSampleInsert.ToolCode = addDataSamplingSample.ToolCode;
                                newSamplingSampleInsert.ToolName = addDataSamplingSample.ToolName;
                                newSamplingSampleInsert.ToolGroupId = addDataSamplingSample.ToolGroupId;
                                newSamplingSampleInsert.ToolGroupName = addDataSamplingSample.ToolGroupName;
                                newSamplingSampleInsert.ToolGroupLabel = addDataSamplingSample.ToolGroupLabel;

                                newSamplingSampleInsert.TestParamId = addDataSamplingSample.TestParamId;
                                newSamplingSampleInsert.TestParamName = addDataSamplingSample.TestParamName;
                                newSamplingSampleInsert.PersonalId = addDataSamplingSample.PersonalId;
                                newSamplingSampleInsert.PersonalInitial = addDataSamplingSample.PersonalInitial;
                                newSamplingSampleInsert.PersonalName = addDataSamplingSample.PersonalName;
                                newSamplingSampleInsert.SamplingDateTimeTo = addDataSamplingSample.SamplingDateTimeTo;
                                newSamplingSampleInsert.SamplingDateTimeFrom = addDataSamplingSample.SamplingDateTimeFrom;
                                newSamplingSampleInsert.ParticleVolume = addDataSamplingSample.ParticleVolume;
                                newSamplingSampleInsert.AttchmentFile = addDataSamplingSample.AttchmentFile;
                                newSamplingSampleInsert.Note = addDataSamplingSample.Note;
                                newSamplingSampleInsert.CreatedBy = data.UpdatedBy;
                                newSamplingSampleInsert.UpdatedBy = data.UpdatedBy;
                                newSamplingSampleInsert.CreatedAt = DateHelper.Now();
                                newSamplingSampleInsert.UpdatedAt = DateHelper.Now();

                                newSamplingSampleInsert.QcSamplingToolsId = addDataSamplingSample.QcSamplingToolsId != null ? addDataSamplingSample.QcSamplingToolsId : sampleTool?.Id;
                                newSamplingSampleInsert.QcSamplingMaterialsId = addDataSamplingSample.QcSamplingMaterialsId != null ? addDataSamplingSample.QcSamplingMaterialsId : sampleMaterial?.Id;

                                _context.QcSamples.Add(newSamplingSampleInsert);
                                await _context.SaveChangesAsync();

                                var SampleChildDataEdit = addDataSamplingSample.SampleChild;

                                //state sampling child data edit

                                if (SampleChildDataEdit != null)
                                {
                                    //loop edit sampling child
                                    #region sample child - add

                                    var addSamplingSampleChild = SampleChildDataEdit.Where(x => !x.Id.HasValue).ToList();

                                    List<QcSample> listAddSamplingSampleChild = new List<QcSample>();
                                    foreach (var addDataSamplingSampleChild in addSamplingSampleChild)
                                    {
                                        QcSample newSamplingSampleChildInsert = new QcSample();

                                        var samplingDateTimeFrom = addDataSamplingSampleChild.SamplingDateTimeFrom ?? addDataSamplingSample.SamplingDateTimeFrom;
                                        var samplingDateTimeTo = addDataSamplingSampleChild.SamplingDateTimeTo ?? addDataSamplingSample.SamplingDateTimeTo;

                                        newSamplingSampleChildInsert.QcSamplingId = data.Id;
                                        newSamplingSampleChildInsert.ParentId = newSamplingSampleInsert.Id;
                                        newSamplingSampleChildInsert.SampleSequence = addDataSamplingSampleChild.SampleSequence;
                                        newSamplingSampleChildInsert.SamplingPointId = addDataSamplingSample.SamplingPointId;
                                        newSamplingSampleChildInsert.SamplingPointCode = addDataSamplingSample.SamplingPointCode;
                                        newSamplingSampleChildInsert.GradeRoomId = addDataSamplingSample.GradeRoomId;
                                        newSamplingSampleChildInsert.GradeRoomName = addDataSamplingSample.GradeRoomName;

                                        newSamplingSampleChildInsert.ToolId = addDataSamplingSample.ToolId;
                                        newSamplingSampleChildInsert.ToolCode = addDataSamplingSample.ToolCode;
                                        newSamplingSampleChildInsert.ToolName = addDataSamplingSample.ToolName;
                                        newSamplingSampleChildInsert.ToolGroupId = addDataSamplingSample.ToolGroupId;
                                        newSamplingSampleChildInsert.ToolGroupName = addDataSamplingSample.ToolGroupName;
                                        newSamplingSampleChildInsert.ToolGroupLabel = addDataSamplingSample.ToolGroupLabel;

                                        newSamplingSampleChildInsert.TestParamId = addDataSamplingSample.TestParamId;
                                        newSamplingSampleChildInsert.TestParamName = addDataSamplingSample.TestParamName;
                                        newSamplingSampleChildInsert.PersonalId = addDataSamplingSample.PersonalId;
                                        newSamplingSampleChildInsert.PersonalInitial = addDataSamplingSample.PersonalInitial;
                                        newSamplingSampleChildInsert.PersonalName = addDataSamplingSample.PersonalName;
                                        newSamplingSampleChildInsert.SamplingDateTimeFrom = samplingDateTimeFrom;
                                        newSamplingSampleChildInsert.SamplingDateTimeTo = samplingDateTimeTo;
                                        newSamplingSampleChildInsert.ParticleVolume = (addDataSamplingSampleChild.ParticleVolume == null ? 0 : addDataSamplingSampleChild.ParticleVolume);
                                        newSamplingSampleChildInsert.AttchmentFile = addDataSamplingSampleChild.AttchmentFile;
                                        newSamplingSampleChildInsert.Note = addDataSamplingSampleChild.Note;
                                        newSamplingSampleChildInsert.CreatedBy = data.UpdatedBy;
                                        newSamplingSampleChildInsert.UpdatedBy = data.UpdatedBy;
                                        newSamplingSampleChildInsert.CreatedAt = DateHelper.Now();
                                        newSamplingSampleChildInsert.UpdatedAt = DateHelper.Now();

                                        newSamplingSampleChildInsert.QcSamplingToolsId = addDataSamplingSample.QcSamplingToolsId != null ? addDataSamplingSample.QcSamplingToolsId : sampleTool?.Id;
                                        newSamplingSampleChildInsert.QcSamplingMaterialsId = addDataSamplingSample.QcSamplingMaterialsId != null ? addDataSamplingSample.QcSamplingMaterialsId : sampleMaterial?.Id;

                                        listAddSamplingSampleChild.Add(newSamplingSampleChildInsert);
                                    }
                                    _context.QcSamples.AddRange(listAddSamplingSampleChild);

                                    #endregion
                                }
                            }
                        }
                        #endregion

                        await _context.SaveChangesAsync();
                        //END PRECESS SAMPLE CHANGES DATA ------------------
                        #endregion

                        #region batch changes data
                        #region -delete batch
                        var deleteTrsBatch = await (from b in _context.TransactionBatches
                                                    where b.RequestQcsId == data.RequestQcsId
                                                    select b).FirstOrDefaultAsync();

                        if (deleteTrsBatch != null)
                        {
                            var deleteTrsBatchLines = await (from bl in _context.TransactionBatchLines
                                                             where bl.TrsBatchId == deleteTrsBatch.Id
                                                             select bl).ToListAsync();

                            var deleteTrsBatchAttachments = await (from ba in _context.TransactionBatchAttachments
                                                                   where ba.TrsBatchId == deleteTrsBatch.Id
                                                                   select ba).ToListAsync();

                            _context.TransactionBatchLines.RemoveRange(deleteTrsBatchLines);
                            _context.TransactionBatchAttachments.RemoveRange(deleteTrsBatchAttachments);

                            _context.TransactionBatches.Remove(deleteTrsBatch);
                            await _context.SaveChangesAsync();
                        }
                        #endregion

                        #region -insert batch hasil edit
                        // if (editSampleBatch != null)
                        // {
                        //     var trsBatch = _mapper.Map<TransactionBatch>(editSampleBatch);
                        //     trsBatch.RequestQcsId = data.RequestQcsId;
                        //     trsBatch.CreatedBy = data.CreatedBy;
                        //     trsBatch.CreatedAt = data.CreatedAt;

                        //     await _context.TransactionBatches.AddAsync(trsBatch);
                        //     await _context.SaveChangesAsync();

                        //     if (editSampleBatch.Lines.Any())
                        //     {
                        //         var trsBatchLines = _mapper.Map<List<TransactionBatchLine>>(editSampleBatch.Lines);
                        //         foreach (var batchLine in trsBatchLines)
                        //         {
                        //             batchLine.TrsBatchId = trsBatch.Id;
                        //             batchLine.CreatedBy = data.CreatedBy;
                        //             batchLine.CreatedAt = data.CreatedAt;
                        //             await _context.TransactionBatchLines.AddAsync(batchLine);
                        //             await _context.SaveChangesAsync();
                        //         }
                        //     }

                        //     if (editSampleBatch.Attachments.Any())
                        //     {
                        //         var trsBatchAttachments = _mapper.Map<List<TransactionBatchAttachment>>(editSampleBatch.Attachments);
                        //         foreach (var batchAttachment in trsBatchAttachments)
                        //         {
                        //             batchAttachment.TrsBatchId = trsBatch.Id;
                        //             batchAttachment.CreatedBy = data.CreatedBy;
                        //             batchAttachment.CreatedAt = data.CreatedAt;
                        //             await _context.TransactionBatchAttachments.AddAsync(batchAttachment);
                        //             await _context.SaveChangesAsync();
                        //         }
                        //     }

                        //     await _context.SaveChangesAsync();
                        // }
                        #endregion
                        #endregion

                        //generated data
                        if (data.Status == ApplicationConstant.STATUS_SUBMIT)
                        {
                            await generateQcResult(data);
                        }
                    }
                    else if (data.SamplingTypeId == 34)
                    {
                        #region sampling personel changes data
                        if (editSampleQcPersonel.Any())
                        {
                            List<int> samplingPersonelIds = editSampleQcPersonel.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();
                            var deleteSamplingPersonel = await (from qsp in _context.QcSamplingPersonels
                                                                where !samplingPersonelIds.Contains(qsp.Id) && qsp.QcSamplingId == data.Id
                                                                select qsp).ToListAsync();
                            _context.QcSamplingPersonels.RemoveRange(deleteSamplingPersonel);

                            var samplingPersonelData = await (from qsp in _context.QcSamplingPersonels
                                                              where samplingPersonelIds.Contains(qsp.Id) && qsp.QcSamplingId == data.Id
                                                              select qsp).ToListAsync();
                            foreach (var dataPersonel in samplingPersonelData)
                            {
                                var editPersonel = editSampleQcPersonel.Where(x => x.Id.Value == dataPersonel.Id).FirstOrDefault();
                                if (editPersonel != null)
                                {
                                    dataPersonel.ProductProductionPhasesPersonelId = editPersonel.ProductProdPhasePersonelId;
                                    dataPersonel.PersonelNik = editPersonel.PersonelNik;
                                    dataPersonel.PersonelName = editPersonel.PersonelName;
                                    dataPersonel.UpdatedBy = data.UpdatedBy;
                                    dataPersonel.UpdatedAt = DateHelper.Now();
                                }
                            }

                            var addPersonel = editSampleQcPersonel.Where(x => !x.Id.HasValue).ToList();
                            List<QcSamplingPersonel> listAddPersonel = new List<QcSamplingPersonel>();
                            foreach (var addDataPersonel in addPersonel)
                            {
                                QcSamplingPersonel newPersonelInsert = new QcSamplingPersonel();
                                newPersonelInsert.QcSamplingId = data.Id;
                                newPersonelInsert.ProductProductionPhasesPersonelId = addDataPersonel.ProductProdPhasePersonelId;
                                newPersonelInsert.PersonelNik = addDataPersonel.PersonelNik;
                                newPersonelInsert.PersonelName = addDataPersonel.PersonelName;
                                newPersonelInsert.CreatedAt = DateHelper.Now();
                                newPersonelInsert.CreatedBy = data.CreatedBy;
                                newPersonelInsert.UpdatedAt = DateHelper.Now();
                                newPersonelInsert.UpdatedBy = data.UpdatedBy;

                                listAddPersonel.Add(newPersonelInsert);
                            }
                            _context.QcSamplingPersonels.AddRange(listAddPersonel);
                        }
                        else
                        {
                            List<int> samplingPersonelIds = editSampleQcPersonel.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();
                            var deleteSamplingPersonel = await (from qsp in _context.QcSamplingPersonels
                                                                where !samplingPersonelIds.Contains(qsp.Id) && qsp.QcSamplingId == data.Id
                                                                select qsp).ToListAsync();
                            _context.QcSamplingPersonels.RemoveRange(deleteSamplingPersonel);
                        }
                        #endregion

                        //save changes to DB
                        await _context.SaveChangesAsync();
                    }

                    //return data;
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

        public async Task<QcSamplingRelationViewModel> GetShortById(int id)
        {
            var result = await (from s in _context.QcSamplings
                                join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                where s.Id == id && r.RowStatus == null
                                select new QcSamplingRelationViewModel
                                {
                                    SamplingId = s.Id,
                                    RequestId = r.Id,
                                    Code = s.Code,
                                    Date = r.Date,
                                    NoRequest = r.NoRequest,
                                    TypeRequestId = r.TypeRequestId,
                                    TypeRequest = r.TypeRequest,

                                    ItemId = r.ItemId,
                                    ItemName = r.ItemName,
                                    StorageTemperatureId = r.StorageTemperatureId,
                                    StorageTemperatureName = r.StorageTemperatureName,
                                    TypeFormId = r.ProductFormId,
                                    TypeFormName = r.ProductFormName,
                                    TestScenarioLabel = r.TestScenarioLabel,
                                    NoBatch = r.NoBatch,
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<QcSampling> GetById(int id)
        {
            return await (from s in _context.QcSamplings
                          where s.Id == id
                          select s).FirstOrDefaultAsync();
        }

        public async Task<List<QcSample>> GetSampleBySamplingId(Int32 SamplingId)
        {
            return await (from s in _context.QcSamples
                          where s.QcSamplingId == SamplingId
                          && s.RowStatus == null
                          select s).ToListAsync();
        }

        public async Task<List<QcSampling>> GetByRequestId(int RequestId)
        {
            return await (from s in _context.QcSamplings
                          where s.RequestQcsId == RequestId
                          select s).ToListAsync();
        }

        public async Task<List<SampleAvailableViewModel>> ListSampleAvailable(string search, List<int> roomId, int testParamId)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var listSample = (from rs in await (from rrsp in _context.TransactionRelRoomSamplingPoint
                                                join room_purp in _context.TransactionRoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                join r in _context.TransactionRoom on room_purp.RoomId equals r.Id
                                                join gr in _context.TransactionGradeRoom on r.GradeRoomId equals gr.Id
                                                join sp in _context.TransactionSamplingPoint on rrsp.SamplingPoinId equals sp.Id
                                                select new SampleAvailableViewModel
                                                {
                                                    RoomId = room_purp.RoomId,
                                                    ToolId = null,
                                                    ToolName = null,
                                                    GradeRoomId = r.GradeRoomId,
                                                    GradeRoomCode = gr.Code,
                                                    GradeRoomName = gr.Name,
                                                    SamplePointId = rrsp.SamplingPoinId,
                                                    SamplePointName = sp.Code
                                                }).Union(
                                            from t in _context.TransactionTool
                                            join gr in _context.TransactionGradeRoom on t.GradeRoomId equals gr.Id
                                            join tp in _context.TransactionToolPurpose on t.Id equals tp.ToolId
                                            join rst in _context.TransactionRelSamplingTool on tp.Id equals rst.ToolPurposeId
                                            join sp in _context.TransactionSamplingPoint on rst.SamplingPoinId equals sp.Id
                                            select new SampleAvailableViewModel
                                            {
                                                RoomId = t.RoomId.Value,
                                                ToolId = t.Id,
                                                ToolName = t.Name,
                                                GradeRoomId = t.GradeRoomId,
                                                GradeRoomCode = gr.Code,
                                                GradeRoomName = gr.Name,
                                                SamplePointId = rst.SamplingPoinId,
                                                SamplePointName = sp.Code
                                            }).ToListAsync()
                              where (EF.Functions.Like(rs.SamplePointName.ToLower(), "%" + filter + "%"))
                              && roomId.Contains(rs.RoomId)
                              select new SampleAvailableViewModel
                              {
                                  RoomId = rs.RoomId,
                                  ToolId = rs.ToolId,
                                  ToolName = rs.ToolName,
                                  GradeRoomCode = rs.GradeRoomCode,
                                  GradeRoomId = rs.GradeRoomId,
                                  GradeRoomName = rs.GradeRoomName,
                                  SamplePointId = rs.SamplePointId,
                                  SamplePointName = rs.SamplePointName,
                                  FirstSamplePointName = rs.SamplePointName.Substring(0, rs.SamplePointName.LastIndexOf('-')),
                                  LastSamplePointName = GetLastSamplePointName(rs.SamplePointName)
                              }).ToList();

            listSample = listSample.OrderBy(x => x.FirstSamplePointName)
                                    .ThenBy(x => x.LastSamplePointName)
                                    .ToList();

            #region hide query alt
            //var SampleParam = await (from sp in _context.VSamplePointTestParams
            //where sp.RoomId == roomId && sp.TestParameterId == testParamId
            //select sp.SamplePointId).ToListAsync();


            //if (testParamId != ApplicationConstant.TEST_PARAMETER_SP)
            //{
            //listSample = listSample.Where(x => !SampleParam.Contains(x.SamplePointId)).ToList();
            //}

            #endregion

            if (testParamId == ApplicationConstant.TEST_PARAMETER_GV)
            {
                listSample = listSample.Where(x => x.ToolId != null).ToList();
            }

            return listSample;

        }

        public async Task<List<SampleAvailableViewModel>> ListSampleAvailableBySamplingId(string search, int samplingId, int? testParamId, string testScenarioLabel)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var purposeIdsFromSampling = await (from qs in _context.QcSamplings
                                                join rp in _context.RequestPurposes on qs.RequestQcsId equals rp.QcRequestId
                                                where qs.Id == samplingId
                                                select rp.PurposeId).ToListAsync();

            var getRoomPurpose = await (from rq in _context.RequestQcs
                                        join rr in _context.RequestGroupRoomPurpose on rq.Id equals rr.RequestQcsId
                                        join qs in _context.QcSamplings on rq.Id equals qs.RequestQcsId
                                        where qs.Id == samplingId
                                        select rr).ToListAsync();

            var getToolPurpose = await (from rq in _context.RequestQcs
                                        join rr in _context.RequestGroupToolPurpose on rq.Id equals rr.RequestQcsId
                                        join qs in _context.QcSamplings on rq.Id equals qs.RequestQcsId
                                        where qs.Id == samplingId
                                        select rr).ToListAsync();

            var roomSamplingPOintAvailable = await (from rrsp in _context.TransactionRelRoomSamplingPoint
                                                    join room_purp in _context.TransactionRoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                    join r in _context.TransactionRoom on room_purp.RoomId equals r.Id
                                                    join gr in _context.TransactionGradeRoom on r.GradeRoomId equals gr.Id
                                                    join sp in _context.TransactionSamplingPoint on rrsp.SamplingPoinId equals sp.Id
                                                    where getRoomPurpose.Select(x => x.RoomPurposeId).ToList().Contains(room_purp.Id)
                                                    && rrsp.ScenarioLabel == testScenarioLabel
                                                    select new SampleAvailableViewModel
                                                    {
                                                        RoomId = room_purp.RoomId,
                                                        ToolId = null,
                                                        ToolName = null,
                                                        GradeRoomId = r.GradeRoomId,
                                                        GradeRoomCode = gr.Code,
                                                        GradeRoomName = gr.Name,
                                                        SamplePointId = rrsp.SamplingPoinId,
                                                        SamplePointName = sp.Code,
                                                        Purposes = (from tp in _context.TransactionPurposes
                                                                    join mrp in _context.TransactionRoomPurposeToMasterPurpose on tp.Id equals mrp.PurposeId
                                                                    join rp in _context.TransactionRoomPurpose on mrp.RoomPurposeId equals rp.Id
                                                                    join trrsp in _context.TransactionRelRoomSamplingPoint on rp.Id equals trrsp.RoomPurposeId
                                                                    where trrsp.SamplingPoinId == rrsp.SamplingPoinId
                                                                    && purposeIdsFromSampling.Contains(tp.Id)
                                                                    && trrsp.ScenarioLabel == testScenarioLabel
                                                                    select new RequestPurposesViewModel
                                                                    {
                                                                        PurposeId = tp.Id,
                                                                        PurposeCode = tp.Code,
                                                                        PurposeName = tp.Name
                                                                    }
                                                        ).ToList()
                                                    }).ToListAsync();
            var toolSamplingPointAvalaible = await (
                                                    from t in _context.TransactionTool
                                                    join gr in _context.TransactionGradeRoom on t.GradeRoomId equals gr.Id
                                                    join tp in _context.TransactionToolPurpose on t.Id equals tp.ToolId
                                                    join rst in _context.TransactionRelSamplingTool on tp.Id equals rst.ToolPurposeId
                                                    join sp in _context.TransactionSamplingPoint on rst.SamplingPoinId equals sp.Id
                                                    where getToolPurpose.Select(x => x.ToolPurposeId).ToList().Contains(tp.Id)
                                                    && rst.ScenarioLabel == testScenarioLabel
                                                    select new SampleAvailableViewModel
                                                    {
                                                        RoomId = t.RoomId.Value,
                                                        ToolId = t.Id,
                                                        ToolName = t.Name,
                                                        GradeRoomId = t.GradeRoomId,
                                                        GradeRoomCode = gr.Code,
                                                        GradeRoomName = gr.Name,
                                                        SamplePointId = rst.SamplingPoinId,
                                                        SamplePointName = sp.Code,
                                                        Purposes = (from tp in _context.TransactionPurposes
                                                                    join mrp in _context.TransactionToolPurposeToMasterPurpose on tp.Id equals mrp.PurposeId
                                                                    join rp in _context.TransactionToolPurpose on mrp.ToolPurposeId equals rp.Id
                                                                    join trrsp in _context.TransactionRelSamplingTool on rp.Id equals trrsp.ToolPurposeId
                                                                    where trrsp.SamplingPoinId == rst.SamplingPoinId
                                                                    && purposeIdsFromSampling.Contains(tp.Id)
                                                                    && trrsp.ScenarioLabel == testScenarioLabel
                                                                    select new RequestPurposesViewModel
                                                                    {
                                                                        PurposeId = tp.Id,
                                                                        PurposeCode = tp.Code,
                                                                        PurposeName = tp.Name
                                                                    }).ToList()
                                                    }).ToListAsync();

            roomSamplingPOintAvailable.AddRange(toolSamplingPointAvalaible);

            var listSample = (from rs in roomSamplingPOintAvailable
                              where EF.Functions.Like(rs.SamplePointName.ToLower(), "%" + filter + "%")
                              select new SampleAvailableViewModel
                              {
                                  RoomId = rs.RoomId,
                                  ToolId = rs.ToolId,
                                  ToolName = rs.ToolName,
                                  GradeRoomCode = rs.GradeRoomCode,
                                  GradeRoomId = rs.GradeRoomId,
                                  GradeRoomName = rs.GradeRoomName,
                                  SamplePointId = rs.SamplePointId,
                                  SamplePointName = rs.SamplePointName,
                                  Purposes = rs.Purposes
                              }).ToList();

            foreach (var sample in listSample)
            {
                if (sample.SamplePointName.Contains('-'))
                {
                    sample.FirstSamplePointName = sample.SamplePointName.Substring(0, sample.SamplePointName.LastIndexOf('-'));
                    sample.LastSamplePointName = GetLastSamplePointName(sample.SamplePointName);
                }
            }

            listSample = listSample.OrderBy(x => x.FirstSamplePointName)
                                    .ThenBy(x => x.LastSamplePointName)
                                    .ToList();

            #region hide query alt
            //var SampleParam = await (from sp in _context.VSamplePointTestParams
            //where sp.RoomId == roomId && sp.TestParameterId == testParamId
            //select sp.SamplePointId).ToListAsync();


            //if (testParamId != ApplicationConstant.TEST_PARAMETER_SP)
            //{
            //listSample = listSample.Where(x => !SampleParam.Contains(x.SamplePointId)).ToList();
            //}

            #endregion

            if ((testParamId == ApplicationConstant.TEST_PARAMETER_GV) && (testParamId != null))
            {
                listSample = listSample.Where(x => x.ToolId != null).ToList();
            }

            return listSample;

        }

        public async Task<List<TestParameterAvailableViewModel>> ListTestParamAvailable(string search, Int32 testGroupId, Int32 requestId)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            List<TestParameterAvailableViewModel> result = new List<TestParameterAvailableViewModel>();
            // get room data request
            var getRoomRequest = await (from rr in _context.RequestRooms
                                        where rr.QcRequestId == requestId
                                        && rr.RowStatus == null
                                        select rr.RoomId).ToListAsync();



            if (requestId == 0)
            {
                result = await (from tp in _context.TransactionTestParameter
                                where (EF.Functions.Like(tp.Name.ToLower(), "%" + filter + "%"))
                                && (tp.Id == ApplicationConstant.TEST_PARAMETER_SP || tp.Id == ApplicationConstant.TEST_PARAMETER_FD)
                                select new TestParameterAvailableViewModel
                                {
                                    Id = tp.Id,
                                    Name = tp.Name,
                                    TestGroupId = tp.TestGroupId,
                                    Sequence = tp.Sequence
                                }).Where(x => x.TestGroupId == testGroupId || testGroupId == 0).ToListAsync();
            }
            else
            {
                // var getTestParamsRoom = await (
                //                         from rgrp in _context.RequestGroupRoomPurpose
                //                         join rp in _context.RequestPurposes on rgrp.RequestQcsId equals rp.QcRequestId
                //                         join rrsp in _context.TransactionRelRoomSamplingPoint on rgrp.RoomPurposeId equals rrsp.RoomPurposeId
                //                         join trstp in _context.TransactionRelSamplingTestParam on rrsp.SamplingPoinId equals trstp.SamplingPointId
                //                         join trtsp in _context.TransactionRelTestScenarioParam on trstp.TestScenarioParamId equals trtsp.Id
                //                         join tp in _context.TransactionTestParameter on trtsp.TestParameterId equals tp.Id
                //                         where rgrp.RequestQcsId == requestId
                //                         select tp.Id).ToListAsync();

                // var getTestParamsTool = await (
                //                             from rgrp in _context.RequestGroupToolPurpose
                //                             join rp in _context.RequestPurposes on rgrp.RequestQcsId equals rp.QcRequestId
                //                             join rrsp in _context.TransactionRelSamplingTool on rgrp.ToolPurposeId equals rrsp.ToolPurposeId
                //                             join trstp in _context.TransactionRelSamplingTestParam on rrsp.SamplingPoinId equals trstp.SamplingPointId
                //                             join trtsp in _context.TransactionRelTestScenarioParam on trstp.TestScenarioParamId equals trtsp.Id
                //                             join tp in _context.TransactionTestParameter on trtsp.TestParameterId equals tp.Id
                //                             where rgrp.RequestQcsId == requestId
                //                             select tp.Id).ToListAsync();

                // getTestParamsRoom.AddRange(getTestParamsTool);
                // getTestParamsRoom.Distinct().ToList();
                // getTestParamsRoom.Add(ApplicationConstant.TEST_PARAMETER_FD);

                result = await (from rty in _context.ProductTestTypeQcs
                                join r in _context.RequestQcs on rty.RequestQcsId equals r.Id
                                join tp in _context.TransactionTestParameter on rty.TestParameterId equals tp.Id
                                where (EF.Functions.Like(rty.TestParameterName.ToLower(), "%" + filter + "%"))
                                && r.Id == requestId
                                && r.Status > 0
                                && r.RowStatus == null
                                //&& getTestParamsRoom.Contains(tp.Id)
                                select new TestParameterAvailableViewModel
                                {
                                    Id = tp.Id,
                                    Name = rty.TestParameterName,
                                    TestGroupId = tp.TestGroupId,
                                    Sequence = tp.Sequence,
                                    CountParamater = rty.SampleAmountCount,
                                    ThresholdRoomSamplingPoint = (tp.Id == ApplicationConstant.TEST_PARAMETER_GV ? 0 : (from rrsp in _context.TransactionRelRoomSamplingPoint
                                                                                                                        join room_purp in _context.TransactionRoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                                                                                        where getRoomRequest.Contains(room_purp.RoomId)
                                                                                                                        select rrsp.SamplingPoinId).Count()),
                                    ThresholdToolSamplingPoint = (from a in (from rst in _context.TransactionRelSamplingTool
                                                                             join tp in _context.TransactionToolPurpose on rst.ToolPurposeId equals tp.Id
                                                                             join t in _context.TransactionTool on tp.ToolId equals t.Id
                                                                             group new { rst, t } by new { rst.SamplingPoinId, t.RoomId } into g
                                                                             where getRoomRequest.Contains(g.Key.RoomId.Value)
                                                                             select new
                                                                             { SpId = g.Key.SamplingPoinId })
                                                                  select a.SpId).Count()
                                }).Where(x => (x.TestGroupId == testGroupId || testGroupId == 0)).ToListAsync();


                for (int i = result.Count - 1; i >= 0; i--)
                {
                    if (result[i].Id != ApplicationConstant.TEST_PARAMETER_SP && result[i].Id != ApplicationConstant.TEST_PARAMETER_CA && result[i].Id != ApplicationConstant.TEST_PARAMETER_FD)
                    {
                        //test param gloves dimunculkan jika ada tool yg terkait pada room yg digunakan di sampling
                        if (result[i].Id == ApplicationConstant.TEST_PARAMETER_GV
                            && result[i].ThresholdToolSamplingPoint.Value > 0)
                            continue;

                        if (result[i].CountParamater >= (result[i].ThresholdRoomSamplingPoint + result[i].ThresholdToolSamplingPoint))
                        {
                            result.RemoveAt(i);
                        }

                    }

                }


            }



            return result;
        }

        public async Task<List<ParameterThresholdRelationViewModel>> ListParameterThreshold(List<int> roomId, int? testScenarioId = null)
        {
            var result = (from vrs in await (from rrsp in _context.TransactionRelRoomSamplingPoint
                                             join room_purp in _context.TransactionRoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                             join r in _context.TransactionRoom on room_purp.RoomId equals r.Id
                                             join gr in _context.TransactionGradeRoom on r.GradeRoomId equals gr.Id
                                             join sp in _context.TransactionSamplingPoint on rrsp.SamplingPoinId equals sp.Id
                                             select new VRoomSamplePoint
                                             {
                                                 RoomId = room_purp.RoomId,
                                                 ToolId = null,
                                                 ToolName = null,
                                                 GradeRoomId = r.GradeRoomId,
                                                 GradeRoomName = gr.Name,
                                                 SamplePointId = rrsp.SamplingPoinId,
                                                 SamplePointName = sp.Code
                                             }).Union(
                                            from t in _context.TransactionTool
                                            join gr in _context.TransactionGradeRoom on t.GradeRoomId equals gr.Id
                                            join tp in _context.TransactionToolPurpose on t.Id equals tp.ToolId
                                            join rst in _context.TransactionRelSamplingTool on tp.Id equals rst.ToolPurposeId
                                            join sp in _context.TransactionSamplingPoint on rst.SamplingPoinId equals sp.Id
                                            select new VRoomSamplePoint
                                            {
                                                RoomId = t.RoomId.Value,
                                                ToolId = t.Id,
                                                ToolName = t.Name,
                                                GradeRoomId = t.GradeRoomId,
                                                GradeRoomName = gr.Name,
                                                SamplePointId = rst.SamplingPoinId,
                                                SamplePointName = sp.Code
                                            }).ToListAsync()
                          where roomId.Contains(vrs.RoomId)
                          group new { vrs } by new { vrs.GradeRoomId } into g
                          select new ParameterThresholdRelationViewModel
                          {
                              RoomId = g.Max(x => x.vrs.RoomId),
                              ToolId = g.Max(x => x.vrs.ToolId),
                              ToolName = g.Max(x => x.vrs.ToolName),
                              GradeRoomId = g.Max(x => x.vrs.GradeRoomId),
                              GradeRoomName = g.Max(x => x.vrs.GradeRoomName),
                              SamplePointId = g.Max(x => x.vrs.SamplePointId),
                              SamplePointName = g.Max(x => x.vrs.SamplePointName)
                          }).ToList();



            #region populate test parameters

            foreach (var res in result)
            {
                var testParamQuery = from rgrs in _context.TransactionRelGradeRoomScenario
                                     join ts in _context.TransactionTestScenario on rgrs.TestScenarioId equals ts.Id
                                     join rtsp in _context.TransactionRelTestScenarioParam on ts.Id equals rtsp.TestScenarioId
                                     join tp in _context.TransactionTestParameter on rtsp.TestParameterId equals tp.Id
                                     where rgrs.GradeRoomId == res.GradeRoomId
                                     select new TestParameterVariableRelationViewModel
                                     {
                                         Id = tp.Id,
                                         Name = tp.Name,
                                         TestGroupId = tp.TestGroupId,
                                         Sequence = tp.Sequence,
                                         TestScenarioId = ts.Id,
                                         TestScenarioName = ts.Name,
                                         TestScenarioLabel = ts.Label,
                                         TestVariableThreshold = (from tv in _context.TransactionTestVariable
                                                                  join en in _context.EnumConstant on tv.TresholdOperator equals en.TypeId
                                                                  where tv.TestParameterId == rtsp.Id && en.KeyGroup == "threshold_operator"
                                                                  select new TestVariableViewModel
                                                                  {
                                                                      Id = tv.Id,
                                                                      VariableName = tv.VariableName,
                                                                      Sequence = tv.Sequence.Value,
                                                                      TresholdOperator = tv.TresholdOperator,
                                                                      TresholdOperatorName = en.Name,
                                                                      TresholdValue = tv.TresholdValue.Value,
                                                                      TresholdMin = tv.ThresholdValueFrom.Value,
                                                                      TresholdMax = tv.ThresholdValueTo.Value
                                                                  }).ToList()
                                     };

                if (testScenarioId.HasValue)
                {
                    testParamQuery = testParamQuery.Where(x => x.TestScenarioId == testScenarioId.Value);
                }

                testParamQuery = testParamQuery.OrderBy(x => x.Sequence);

                res.TestParameter = testParamQuery.ToList();
            }

            #endregion

            return result;
        }

        public async Task<List<ParameterThresholdRelationAltViewModel>> ListParameterThresholdAlt(List<int> GradeRoomId, string testScenarioLabel, int? testGroupId)
        {
            var result = await (from gr in _context.TransactionGradeRoom
                                where GradeRoomId.Contains(gr.Id)
                                && gr.RowStatus == null
                                select new ParameterThresholdRelationAltViewModel
                                {
                                    GradeRoomId = gr.Id,
                                    GradeRoomCode = gr.Code,
                                    GradeRoomName = gr.Name,
                                    TestScenario = (from rgrs in _context.TransactionRelGradeRoomScenario
                                                    join ts in _context.TransactionTestScenario on rgrs.TestScenarioId equals ts.Id
                                                    where rgrs.GradeRoomId == gr.Id
                                                    select new TestScenarioThresholdViewModel
                                                    {
                                                        TestScenarioId = ts.Id,
                                                        TestScenarioName = ts.Name,
                                                        TestScenarioLabel = ts.Label,
                                                        TestParameterThreshold = (from tp in _context.TransactionTestParameter
                                                                                  orderby tp.Sequence
                                                                                  select new TestParameterThresholdViewModel
                                                                                  {
                                                                                      TestParameterId = tp.Id,
                                                                                      TestParameterName = tp.Name,
                                                                                      TestParameterShort = tp.ShortName,
                                                                                      TestGroupId = tp.TestGroupId,
                                                                                      Sequence = tp.Sequence,
                                                                                      TestVariableThreshold = (from tv in _context.TransactionTestVariable
                                                                                                               join rtsp in _context.TransactionRelTestScenarioParam on ts.Id equals rtsp.TestScenarioId
                                                                                                               join en in _context.EnumConstant on tv.TresholdOperator equals en.TypeId
                                                                                                               where tv.TestParameterId == rtsp.Id
                                                                                                               && en.KeyGroup == "threshold_operator"
                                                                                                               && rtsp.TestParameterId == tp.Id
                                                                                                               orderby tv.Sequence
                                                                                                               select new TestVariableThresholdViewModel
                                                                                                               {
                                                                                                                   TestVariableId = tv.Id,
                                                                                                                   VariableName = tv.VariableName,
                                                                                                                   Sequence = tv.Sequence.Value,
                                                                                                                   TresholdOperator = tv.TresholdOperator,
                                                                                                                   TresholdOperatorName = en.Name,
                                                                                                                   TresholdValue = tv.TresholdValue,
                                                                                                                   TresholdMin = tv.ThresholdValueFrom,
                                                                                                                   TresholdMax = tv.TresholdValue,
                                                                                                               }).ToList()
                                                                                  }).Where(x => x.TestGroupId == testGroupId || testGroupId == null).ToList()

                                                    }).Where(x => x.TestScenarioLabel == testScenarioLabel || testScenarioLabel == null).ToList()
                                }).ToListAsync();

            return result;
        }

        public async Task<List<ParameterThresholdRelationAltViewModel>> ListParameterThresholdAltV2(List<int> GradeRoomId, string testScenarioLabel, int? testGroupId)
        {
            List<ParameterThresholdRelationAltViewModel> result = new List<ParameterThresholdRelationAltViewModel>();
            foreach (var item in GradeRoomId.Select(x => x).GroupBy(x => x))
            {
                var data = await (from gr in _context.TransactionGradeRoom
                                  where gr.Id == item.Key
                                  && gr.RowStatus == null
                                  select new ParameterThresholdRelationAltViewModel
                                  {
                                      GradeRoomId = gr.Id,
                                      GradeRoomCode = gr.Code,
                                      GradeRoomName = gr.Name,
                                      TestScenario = (from rgrs in _context.TransactionRelGradeRoomScenario
                                                      join ts in _context.TransactionTestScenario on rgrs.TestScenarioId equals ts.Id
                                                      where rgrs.GradeRoomId == gr.Id
                                                      select new TestScenarioThresholdViewModel
                                                      {
                                                          TestScenarioId = ts.Id,
                                                          TestScenarioName = ts.Name,
                                                          TestScenarioLabel = ts.Label,
                                                          TestParameterThreshold = (from tp in _context.TransactionTestParameter
                                                                                    orderby tp.Sequence
                                                                                    select new TestParameterThresholdViewModel
                                                                                    {
                                                                                        TestParameterId = tp.Id,
                                                                                        TestParameterName = tp.Name,
                                                                                        TestParameterShort = tp.ShortName,
                                                                                        TestGroupId = tp.TestGroupId,
                                                                                        Sequence = tp.Sequence,
                                                                                        TestVariableThreshold = (from tv in _context.TransactionTestVariable
                                                                                                                 join rtsp in _context.TransactionRelTestScenarioParam on ts.Id equals rtsp.TestScenarioId
                                                                                                                 join en in _context.EnumConstant on tv.TresholdOperator equals en.TypeId
                                                                                                                 where tv.TestParameterId == rtsp.Id
                                                                                                                 && en.KeyGroup == "threshold_operator"
                                                                                                                 && rtsp.TestParameterId == tp.Id
                                                                                                                 orderby tv.Sequence
                                                                                                                 select new TestVariableThresholdViewModel
                                                                                                                 {
                                                                                                                     TestVariableId = tv.Id,
                                                                                                                     VariableName = tv.VariableName,
                                                                                                                     Sequence = tv.Sequence.Value,
                                                                                                                     TresholdOperator = tv.TresholdOperator,
                                                                                                                     TresholdOperatorName = en.Name,
                                                                                                                     TresholdValue = tv.TresholdValue,
                                                                                                                     TresholdMin = tv.ThresholdValueFrom,
                                                                                                                     TresholdMax = tv.TresholdValue,
                                                                                                                 }).ToList()
                                                                                    }).Where(x => x.TestGroupId == testGroupId || testGroupId == null).ToList()

                                                      }).Where(x => x.TestScenarioLabel == testScenarioLabel || testScenarioLabel == null).ToList()
                                  }).ToListAsync();
                result.AddRange(data);

            }
            return result;

        }

        public async Task<List<QcLabelRelationViewModel>> ListLabelSampleQc(Int32 SamplingId, Int32 SampleId, string SampleCode, Int32 samplePointId, Int32 testParameterId)
        {
            string filter = "";
            if (SampleCode != null)
                filter = SampleCode.ToLower();

            var result = await (from spl in _context.QcSamples
                                join sm in _context.QcSamplings on spl.QcSamplingId equals sm.Id
                                join rqs in _context.RequestQcs on sm.RequestQcsId equals rqs.Id
                                where (EF.Functions.Like(spl.Code.ToLower(), "%" + filter + "%"))
                                select new QcLabelRelationViewModel
                                {
                                    RequestQc = new QcLabelRequestViewModel
                                    {
                                        QcRequestId = rqs.Id,
                                        NoBatch = rqs.NoBatch,
                                        NoRequest = rqs.NoRequest,
                                        TypeRequest = rqs.TypeRequest,
                                        ItemName = rqs.ItemName,
                                        EmRoomName = rqs.EmRoomName,
                                        EmRoomGradeName = rqs.EmRoomGradeName,
                                        EmPhaseName = rqs.EmPhaseName,
                                        TestScenarioName = rqs.TestScenarioName,
                                        TestScenarioLabel = rqs.TestScenarioLabel,
                                        CreatedBy = rqs.CreatedBy
                                    },
                                    SamplingQc = new QcLabelSamplingViewModel
                                    {
                                        QcSamplingId = sm.Id,
                                        SamplingTypeName = sm.SamplingTypeName,
                                        SamplingDateFrom = sm.SamplingDateFrom,
                                        SamplingDateTo = sm.SamplingDateTo,
                                        SamplingStatus = sm.Status
                                    },
                                    SampleData = new QcLabelSampleViewModel
                                    {
                                        SampleId = spl.Id,
                                        Code = spl.Code,
                                        SamplingPointId = spl.SamplingPointId,
                                        SamplingPointCode = spl.SamplingPointCode,
                                        GradeRoomName = spl.GradeRoomName,
                                        ToolCode = spl.ToolCode,
                                        ToolName = spl.ToolName,
                                        ToolGroupName = spl.ToolGroupName,
                                        ToolGroupLabel = spl.ToolGroupLabel,
                                        TestParamId = spl.TestParamId,
                                        TestParamName = spl.TestParamName,
                                        PersonalName = spl.PersonalName,
                                        PersonalInitial = spl.PersonalInitial,
                                        SampleDateTimeFrom = spl.SamplingDateTimeFrom,
                                        SampleDateTimeTo = spl.SamplingDateTimeTo,
                                        ParticleVolume = spl.ParticleVolume,
                                        AttchmentFile = spl.AttchmentFile,
                                        TestParamIndex = 1,
                                        Note = spl.Note
                                    }
                                })
                                .Where(x => (x.SampleData.SampleId == SampleId || SampleId == 0) &&
                                            (x.SamplingQc.QcSamplingId == SamplingId || SamplingId == 0) &&
                                            (x.SampleData.SamplingPointId == samplePointId || samplePointId == 0) &&
                                            (x.SampleData.TestParamId == testParameterId || testParameterId == 0))
                                .ToListAsync();

            int newTestParamIndex = 1;
            var existingSamplingPointCode = new List<string>();
            var existingTestParamIndex = new List<int>();
            int samplingPointCodeIndex = 0;
            int testParamIndex = 0;


            foreach (var res in result)
            {
                //set sampData.TestParamIndex jika sampData.TestParamId == 1
                if (res.SampleData.TestParamId == 1)
                {
                    if (existingSamplingPointCode.Contains(res.SampleData.SamplingPointCode))
                    {
                        samplingPointCodeIndex = existingSamplingPointCode.IndexOf(res.SampleData.SamplingPointCode);
                        testParamIndex = existingTestParamIndex[samplingPointCodeIndex] + 1;
                        res.SampleData.TestParamIndex = testParamIndex;
                        existingTestParamIndex[samplingPointCodeIndex] = testParamIndex;
                    }
                    else
                    {
                        existingSamplingPointCode.Add(res.SampleData.SamplingPointCode);
                        existingTestParamIndex.Add(newTestParamIndex);
                    }
                }
            }

            return result;
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

        public string updateCodeSample(string codeSample, List<TransactionTestScenario> lsTestScenario, int? TestScenarioId)
        {
            var returnCode = codeSample;

            var codeScnario = ApplicationConstant.TEST_SCENARIO_CODE_UNDEFINED;

            if (TestScenarioId.HasValue)
            {
                var getScenario = lsTestScenario.FirstOrDefault(s => s.Id == TestScenarioId);

                if (getScenario != null)
                {
                    if (getScenario.Label == ApplicationConstant.TEST_SCENARIO_LABEL_AT_REST || getScenario.Label == ApplicationConstant.TEST_SCENARIO_LABELALT_AT_REST)
                    {
                        codeScnario = ApplicationConstant.TEST_SCENARIO_CODE_AT_REST;
                    }
                    else if (getScenario.Label == ApplicationConstant.TEST_SCENARIO_LABEL_IN_OPERATIONS || getScenario.Label == ApplicationConstant.TEST_SCENARIO_LABELALT_IN_OPERATIONS)
                    {
                        codeScnario = ApplicationConstant.TEST_SCENARIO_CODE_IN_OPERATIONS;
                    }
                }
            }

            if (codeSample.Length == 6)
            {
                returnCode = codeSample + "-" + codeScnario;
            }
            else
            {
                codeSample = codeSample.Remove(codeSample.Length - 2, 2); //remove "-" and code scenario
                returnCode = codeSample + "-" + codeScnario;
            }

            return returnCode;
        }

        public async Task<List<QcLabelBatchRelationViewModel>> ListLabelBatchQc(List<int> SamplingId, string SampleCode)
        {
            var result = await (from s in _context.QcSamplings
                                join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                select new QcLabelBatchRelationViewModel
                                {
                                    SamplingId = s.Id,
                                    Code = s.Code,
                                    OrgId = r.OrgId,
                                    OrgName = r.OrgName,
                                    Date = r.Date,
                                    NoRequest = r.NoRequest,
                                    NoBatch = r.NoBatch,
                                    TypeRequestId = r.TypeRequestId,
                                    TypeRequest = r.TypeRequest,
                                    SamplingTypeId = s.SamplingTypeId,
                                    SamplingTypeName = s.SamplingTypeName,
                                    ItemId = r.ItemId,
                                    ItemName = r.ItemName,
                                    TypeFormId = r.TypeFormId,
                                    TypeFormName = r.TypeFormName,

                                    PurposeId = r.PurposeId,
                                    PurposeName = r.PurposeName,
                                    EmRoomId = r.EmRoomId,
                                    EmRoomName = r.EmRoomName,
                                    EmRoomGradeId = r.EmRoomGradeId,
                                    EmRoomGradeName = r.EmRoomGradeName,
                                    EmPhaseId = r.EmPhaseId,
                                    EmPhaseName = r.EmPhaseName,
                                    SamplingDateFrom = s.SamplingDateFrom,
                                    SamplingDateTo = s.SamplingDateTo,

                                    Status = s.Status,
                                    CreatedBy = s.CreatedBy,
                                    CreatedAt = s.CreatedAt,
                                    RequestPurposes = (from rp in _context.RequestPurposes
                                                       where rp.QcRequestId == r.Id
                                                       && rp.RowStatus == null
                                                       orderby rp.Id
                                                       select new RequestPurposesViewModel
                                                       {
                                                           Id = rp.Id,
                                                           PurposeId = rp.PurposeId,
                                                           PurposeCode = rp.PurposeCode,
                                                           PurposeName = rp.PurposeName
                                                       }).ToList(),
                                    RequestRooms = (from rr in _context.RequestRooms
                                                    where rr.QcRequestId == r.Id
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

                                }).Where(x =>
                                      (SamplingId.Contains(x.SamplingId) || !SamplingId.Any()) &&
                                      (x.Code == SampleCode || SampleCode == null)
                            ).OrderByDescending(x => x.CreatedAt).ToListAsync();

            foreach (var res in result)
            {
                res.Testparameters = (from sm in _context.QcSamples
                                      join tp in _context.TransactionTestParameter on sm.TestParamId equals tp.Id
                                      join o in _context.TransactionOrganization on tp.OrganizationId equals o.Id
                                      where sm.QcSamplingId == res.SamplingId && sm.ParentId == null
                                      group new { sm, tp, o } by new { sm.TestParamId } into g
                                      select new QcLabelBatchTestParamViewModel
                                      {
                                          Id = g.Max(x => x.sm.TestParamId),
                                          Name = g.Max(x => x.sm.TestParamName),
                                          Sequence = g.Max(x => x.tp.Sequence),
                                          CountParamater = g.Count(),
                                          OrgId = g.Max(x => x.tp.OrganizationId),
                                          OrgCode = g.Max(x => x.o.OrgCode),
                                          OrgName = g.Max(x => x.o.Name)
                                      }).OrderBy(x => x.Sequence).ToList();

                var ShipmentSampling = (from sp in _context.QcSamplingShipments
                                        where sp.QrCode == res.Code
                                        select sp).FirstOrDefault();

                if (ShipmentSampling == null)
                {
                    res.ShipmentStatus = ApplicationConstant.STATUS_SHIPMENT_NOTHING;
                }
                else
                {
                    res.ShipmentStatus = ShipmentSampling.Status;
                }
            }

            return result;
        }

        public async Task<QcLabelBatchRelationViewModel> GetSamplingBatchByQRCode(string QRCode)
        {
            var result = await (from s in _context.QcSamplings
                                join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                where s.Code == QRCode
                                select new QcLabelBatchRelationViewModel
                                {
                                    SamplingId = s.Id,
                                    Code = s.Code,
                                    OrgId = r.OrgId,
                                    OrgName = r.OrgName,
                                    Date = r.Date,
                                    NoRequest = r.NoRequest,
                                    NoBatch = r.NoBatch,
                                    TypeRequestId = r.TypeRequestId,
                                    TypeRequest = r.TypeRequest,
                                    SamplingTypeId = s.SamplingTypeId,
                                    SamplingTypeName = s.SamplingTypeName,
                                    ItemId = r.ItemId,
                                    ItemName = r.ItemName,
                                    TypeFormId = r.TypeFormId,
                                    TypeFormName = r.TypeFormName,
                                    PurposeId = r.PurposeId,
                                    PurposeName = r.PurposeName,
                                    EmRoomId = r.EmRoomId,
                                    EmRoomName = r.EmRoomName,
                                    EmRoomGradeId = r.EmRoomGradeId,
                                    EmRoomGradeName = r.EmRoomGradeName,
                                    EmPhaseId = r.EmPhaseId,
                                    EmPhaseName = r.EmPhaseName,
                                    SamplingDateFrom = s.SamplingDateFrom,
                                    SamplingDateTo = s.SamplingDateTo,
                                    Status = s.Status,
                                    CreatedBy = s.CreatedBy,
                                    CreatedAt = s.CreatedAt,
                                    RequestPurposes = (from rp in _context.RequestPurposes
                                                       where rp.QcRequestId == r.Id
                                                       && rp.RowStatus == null
                                                       orderby rp.Id
                                                       select new RequestPurposesViewModel
                                                       {
                                                           Id = rp.Id,
                                                           PurposeId = rp.PurposeId,
                                                           PurposeCode = rp.PurposeCode,
                                                           PurposeName = rp.PurposeName
                                                       }).ToList(),

                                }).FirstOrDefaultAsync();

            if (result != null)
            {
                result.Testparameters = (from sm in _context.QcSamples
                                         join tp in _context.TransactionTestParameter on sm.TestParamId equals tp.Id
                                         join o in _context.TransactionOrganization on tp.OrganizationId equals o.Id
                                         where sm.QcSamplingId == result.SamplingId && sm.ParentId == null
                                         group new { sm, tp, o } by new { sm.TestParamId } into g
                                         select new QcLabelBatchTestParamViewModel
                                         {
                                             Id = g.Max(x => x.sm.TestParamId),
                                             Name = g.Max(x => x.sm.TestParamName),
                                             Sequence = g.Max(x => x.tp.Sequence),
                                             CountParamater = g.Count(),
                                             OrgId = g.Max(x => x.tp.OrganizationId),
                                             OrgCode = g.Max(x => x.o.OrgCode),
                                             OrgName = g.Max(x => x.o.Name)
                                         }).OrderBy(x => x.Sequence).ToList();
            }

            return result;
        }

        public async Task<QcSampling> UpdateQcSamplingDataFromApproval(UpdateQcSamplingFromApproval data)
        {
            var course = _context.QcSamplings.FirstOrDefault(x => x.Id == data.SamplingId);

            if (course != null)
            {
                course.Id = data.SamplingId;
                course.WorkflowStatus = data.WorkflowStatus;
                course.Status = data.Status;
                course.UpdatedBy = data.UpdatedBy;
                course.UpdatedAt = DateTime.UtcNow.AddHours(7);
                course.ReceiptDate = DateTime.UtcNow.AddHours(7);
                if (data.RowStatus != null)
                {
                    course.RowStatus = data.RowStatus;
                }
            }

            //jika status berubah menjadi review QA
            if (data.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA)
            {
                await _requestDataProvider.UpdateReceiptDate(course.RequestQcsId, true);
            }
            else if (data.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG)
            {
                await _requestDataProvider.UpdateReceiptDate(course.RequestQcsId, false);
            }

            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<int> getOrganizatiionBySamplingId(int samplingId)
        {
            var result = await (from sq in _context.QcSamplings
                                join rq in _context.RequestQcs on sq.RequestQcsId equals rq.Id
                                join or in _context.TransactionOrganization on rq.OrgId equals or.Id
                                where sq.Id == samplingId
                                select new { BIOHROrganizationId = or.BiohrOrganizationId.Value }
                                ).FirstOrDefaultAsync();
            return result.BIOHROrganizationId;
        }

        public async Task<string> generateQcResult(QcSampling data)
        {
            var result = "not_generated";
            var isConclusionPass = true;

            var getRquestQc = await (from r in _context.RequestQcs
                                     where r.RowStatus == null
                                     && r.TypeRequestId == ApplicationConstant.REQUEST_TYPE_PRODUCT
                                     && r.Id == data.RequestQcsId
                                     select r).FirstOrDefaultAsync();

            if (getRquestQc != null)
            {
                // get data qc sample PC
                var getSamplePC = await (from s in _context.QcSamples
                                         join sp in _context.QcSamplings on s.QcSamplingId equals sp.Id
                                         where s.RowStatus == null
                                         && sp.Status >= ApplicationConstant.STATUS_SUBMIT
                                         && s.QcSamplingId == data.Id
                                         && s.ParentId == null
                                         && (s.TestParamId == ApplicationConstant.TEST_PARAMETER_PC05 || s.TestParamId == ApplicationConstant.TEST_PARAMETER_PC50)
                                         select s).ToListAsync();

                if (getSamplePC.Any())
                {
                    List<QcResult> lsNewQcResult = new List<QcResult>();
                    List<int> lsSampleId = getSamplePC.Select(s => s.Id).ToList();

                    var DeleteResult = await _context.QcResults
                            .Where(p => lsSampleId.Contains(p.SampleId))
                            .ToListAsync();

                    _context.QcResults.RemoveRange(DeleteResult);

                    //get testVariables by list of testScenarioId dan list of testParamId
                    var testScenarioIds = getSamplePC.Select(x => x.TestScenarioId).ToList();
                    var testParamIds = getSamplePC.Select(x => x.TestParamId).ToList();
                    var testVariables = await (from tv in _context.TransactionTestVariable
                                               join tstp in _context.TransactionRelTestScenarioParam on tv.TestParameterId equals tstp.Id
                                               join tp in _context.TransactionTestParameter on tstp.TestParameterId equals tp.Id
                                               where testScenarioIds.Contains(tstp.TestScenarioId)
                                               && testParamIds.Contains(tp.Id)
                                               select new
                                               {
                                                   TestVariable = tv,
                                                   TestScenarioId = tstp.TestScenarioId,
                                                   TestParamId = tp.Id
                                               }).ToListAsync();

                    foreach (var sample in getSamplePC)
                    {
                        //get testVariable by testScenarioId dan testParamId
                        var getTestVariableAlt = testVariables
                            .Where(x => x.TestParamId == sample.TestParamId && x.TestScenarioId == sample.TestScenarioId)
                            .Select(x => x.TestVariable)
                            .OrderBy(x => x.Sequence);

                        QcResult newQcResult = new QcResult();

                        var TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                        var IdVariable = 0;
                        if (getTestVariableAlt.Any())
                        {
                            foreach (var th in getTestVariableAlt)
                            {
                                if (sample.ParticleVolume.HasValue)
                                {
                                    /*operator checking*/
                                    switch (th.TresholdOperator)
                                    {
                                        case ApplicationConstant.THRESHOLD_EQUAL:
                                            if (sample.ParticleVolume == th.TresholdValue)
                                            {
                                                switch (th.VariableName)
                                                {
                                                    case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                        IdVariable = th.Id;
                                                        break;
                                                }
                                            }
                                            break;

                                        case ApplicationConstant.THRESHOLD_GREATER_THAN:
                                            if (sample.ParticleVolume <= th.TresholdValue)
                                            {
                                                switch (th.VariableName)
                                                {
                                                    case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                        IdVariable = th.Id;
                                                        break;
                                                }
                                            }
                                            break;

                                        case ApplicationConstant.THRESHOLD_LESS_THAN:
                                            if (sample.ParticleVolume >= th.TresholdValue)
                                            {
                                                switch (th.VariableName)
                                                {
                                                    case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                        IdVariable = th.Id;
                                                        break;
                                                }
                                            }
                                            break;

                                        case ApplicationConstant.THRESHOLD_GREATER_THAN_OR_EQUAL:
                                            if (sample.ParticleVolume < th.TresholdValue)
                                            {
                                                switch (th.VariableName)
                                                {
                                                    case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                        IdVariable = th.Id;
                                                        break;
                                                }
                                            }
                                            break;

                                        case ApplicationConstant.THRESHOLD_LESS_THAN_OR_EQUAL:
                                            if (sample.ParticleVolume > th.TresholdValue)
                                            {
                                                switch (th.VariableName)
                                                {
                                                    case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                        IdVariable = th.Id;
                                                        break;
                                                }
                                            }
                                            break;

                                        case ApplicationConstant.THRESHOLD_IN_BETTWEEN:
                                            if (sample.ParticleVolume <= th.ThresholdValueFrom && sample.ParticleVolume > th.ThresholdValueTo)
                                            {
                                                switch (th.VariableName)
                                                {
                                                    case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                        IdVariable = th.Id;
                                                        break;
                                                }
                                            }
                                            break;

                                        case ApplicationConstant.THRESHOLD_IN_BETTWEEN_OR_EQUAL:
                                            if (sample.ParticleVolume < th.ThresholdValueFrom && sample.ParticleVolume >= th.ThresholdValueTo)
                                            {
                                                switch (th.VariableName)
                                                {
                                                    case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                        IdVariable = th.Id;
                                                        break;

                                                    case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                        TestVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                        IdVariable = th.Id;
                                                        break;
                                                }
                                            }
                                            break;

                                    }

                                }
                                else
                                {
                                    sample.ParticleVolume = 0;
                                }

                            }
                        }


                        newQcResult.SampleId = sample.Id;
                        newQcResult.Value = sample.ParticleVolume.ToString();
                        newQcResult.TestVariableConclusion = TestVariableConclusionLabel;
                        newQcResult.TestVariableId = IdVariable;
                        newQcResult.Note = sample.Note;
                        newQcResult.AttchmentFile = sample.AttchmentFile;
                        newQcResult.CreatedBy = data.UpdatedBy;
                        newQcResult.UpdatedBy = data.UpdatedBy;
                        newQcResult.CreatedAt = DateTime.UtcNow.AddHours(7);
                        newQcResult.UpdatedAt = DateTime.UtcNow.AddHours(7);

                        lsNewQcResult.Add(newQcResult);

                    }

                    _context.QcResults.AddRange(lsNewQcResult);

                }

                await _context.SaveChangesAsync();

                //update conclusion request qc
                await _requestDataProvider.GenerateUpdateConclusion(data.RequestQcsId);

                result = "generated";
            }


            return result;
        }

        public async Task<int> GetRequestIdByWorkflowCode(string workflowDocumentCode, string workflowStatus)
        {
            var result = 0;
            var resultRequestQcsId = await (from sq in _context.QcSamplings
                                            join r in _context.WorkflowQcSampling on sq.Id equals r.QcSamplingId
                                            where r.WorkflowDocumentCode == workflowDocumentCode
                                            select new { RequestQcsId = sq.RequestQcsId }
                                ).FirstOrDefaultAsync();


            if (resultRequestQcsId != null)
            {
                var resultWorkflowStatus = await (from sq in _context.QcSamplings
                                                  join r in _context.WorkflowQcSampling on sq.Id equals r.QcSamplingId
                                                  where sq.RequestQcsId == resultRequestQcsId.RequestQcsId && r.WorkflowStatus != ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                                                  select new { WorkflowStatus = r.WorkflowStatus }
                                    ).ToListAsync();

                var getSampling = await _context.QcSamplings.Where(x => x.RequestQcsId == resultRequestQcsId.RequestQcsId && x.Status == 0).ToListAsync();
                if (getSampling.Count == 0)
                {

                    if ((resultWorkflowStatus.FirstOrDefault(x => x.WorkflowStatus != workflowStatus && x.WorkflowStatus != ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA)) == null)
                    {
                        result = resultRequestQcsId.RequestQcsId;
                    }
                }
            }

            return result;
        }

        public async Task<List<int>> GetAllRequestIdByWorkflowCode(List<ListReviewPending> listReviewPending, string workflowStatus)
        {
            List<int> result = new List<int>();
            List<string> workflowDocumentCodes = listReviewPending.Select(x => x.RecordId).ToList();
            var resultRequestQcsId = await (from sq in _context.QcSamplings
                                            join r in _context.WorkflowQcSampling on sq.Id equals r.QcSamplingId
                                            where workflowDocumentCodes.Contains(r.WorkflowDocumentCode)
                                            && r.WorkflowStatus == workflowStatus
                                            select sq.RequestQcsId
                                ).ToListAsync();

            if (resultRequestQcsId.Any())
            {
                var lsResultWorkflowStatus = await (from sq in _context.QcSamplings
                                                    join r in _context.WorkflowQcSampling on sq.Id equals r.QcSamplingId
                                                    where r.WorkflowStatus != ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                                                    select new
                                                    {
                                                        WorkflowStatus = r.WorkflowStatus,
                                                        Key = sq.RequestQcsId
                                                    }).ToListAsync();

                var samplings = await _context.QcSamplings
                    .Where(x => x.Status == 0).ToListAsync();

                foreach (var item in resultRequestQcsId.GroupBy(x => x))
                {
                    var resultWorkflowStatus = lsResultWorkflowStatus.Where(x => x.Key == item.Key).ToList();

                    var getSampling = samplings.Where(x => x.RequestQcsId == item.Key).ToList();
                    if (getSampling.Count == 0)
                    {

                        if ((resultWorkflowStatus.FirstOrDefault(x => x.WorkflowStatus != workflowStatus && x.WorkflowStatus != ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA)) == null)
                        {
                            result.Add(item.Key);
                        }
                    }

                }
            }

            return result;
        }

        public async Task<List<int>> GetRequestIdSamplingInReviewQa()
        {
            return await (from sq in _context.QcSamplings
                          join r in _context.WorkflowQcSampling on sq.Id equals r.QcSamplingId
                          where r.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA
                          select sq.RequestQcsId).ToListAsync();
        }

        public async Task<bool> GetRequestIdSamplingComplete(int requestId, string workflowStatus)
        {
            var result = false;
            var haveComplete = false;
            var data = await (from sq in _context.QcSamplings
                              join r in _context.WorkflowQcSampling on sq.Id equals r.QcSamplingId
                              where r.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                              && sq.RequestQcsId == requestId
                              && r.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2
                              orderby r.UpdatedAt descending
                              select r).FirstOrDefaultAsync();

            var dataPending = await (from sq in _context.QcSamplings
                                     join r in _context.WorkflowQcSampling on sq.Id equals r.QcSamplingId
                                     where r.WorkflowStatus == workflowStatus
                                     && sq.RequestQcsId == requestId
                                     && r.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2
                                     orderby r.UpdatedAt descending
                                     select r.IsInWorkflow).FirstOrDefaultAsync();
            if (data != null)
            {
                haveComplete = !data.IsInWorkflow ? true : false;
            }

            if ((haveComplete) && (dataPending))
            {
                result = true;
            }


            return result;
        }

        public async Task<QcSampling> GetRequestIdSamplingStillInReview(int requestId)
        {
            return await (from sq in _context.QcSamplings
                          join r in _context.WorkflowQcSampling on sq.Id equals r.QcSamplingId
                          where r.WorkflowStatus != ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA
                          && sq.RequestQcsId == requestId
                          && r.IsInWorkflow == true
                          orderby r.UpdatedAt descending
                          select sq).FirstOrDefaultAsync();
        }

        public async Task<List<int>> ListRequestIdSamplingComplete(List<int> requestId)
        {
            return await ((from sq in _context.QcSamplings
                           join r in _context.WorkflowQcSampling on sq.Id equals r.QcSamplingId
                           where r.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                           && requestId.Contains(sq.RequestQcsId)
                           && r.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2
                           orderby r.UpdatedAt descending
                           select new
                           {
                               IsInWorkflow = r.IsInWorkflow,
                               RequestQcsId = sq.RequestQcsId
                           }).AsQueryable()).Where(x => x.IsInWorkflow == false).Select(x => x.RequestQcsId).ToListAsync();
        }

        public async Task<List<QcSampling>> FindSamplingByWorkflowDocCodeAndQcRequestId(string workflowDocumentCode, string workflowStatus, int qcRequestId)
        {
            List<QcSampling> result = new List<QcSampling>();
            var resultWorkflow = await (from sq in _context.QcSamplings
                                        join r in _context.WorkflowQcSampling on sq.Id equals r.QcSamplingId
                                        where sq.RequestQcsId == qcRequestId &&
                                        r.WorkflowDocumentCode == workflowDocumentCode &&
                                        r.IsInWorkflow == true
                                        select sq).ToListAsync();

            if ((resultWorkflow.FirstOrDefault(x => x.WorkflowStatus != workflowStatus)) != null)
            {
                result = resultWorkflow;
            }

            return result;
        }

        public async Task<bool> GetRequestIdStatusComplete(int requestQcsId)
        {
            var result = 0;
            var requestId = await (from wqs in _context.WorkflowQcSampling
                                   join qs in _context.QcSamplings on wqs.QcSamplingId equals qs.Id
                                   where wqs.IsInWorkflow == false
                                   && wqs.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2
                                   && wqs.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                                   && qs.RequestQcsId == requestQcsId
                                   select wqs).ToListAsync();

            if (requestId.Count() == 2)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<List<QcSampling>> GetSamplingByRequestIdOnPending(int id, string nik)
        {
            var pending = await _workflowServiceDataProvider.GetListPendingByNik(nik);
            List<QcSampling> result = new List<QcSampling>();

            List<string> workflowDocumentCodes = pending.ListPending.Select(x => x.RecordId).ToList();
            if (workflowDocumentCodes.Any())
            {
                var sampling = await (
                                    from s in _context.QcSamplings
                                    join ws in _context.WorkflowQcSampling on s.Id equals ws.QcSamplingId
                                    where s.RequestQcsId == id
                                    && workflowDocumentCodes.Contains(ws.WorkflowDocumentCode)
                                    select s
                                ).ToListAsync();
                result.AddRange(sampling);
            }
            return result;
        }

        public async Task<List<QcSampling>> GetSamplingByWorkflowCodeIdOnPending(string workflowDocumentCode, string nik)
        {
            var pending = await _workflowServiceDataProvider.GetListPendingByNik(nik);
            List<QcSampling> result = new List<QcSampling>();

            foreach (var item in pending.ListPending)
            {
                var sampling = (from s in _context.QcSamplings
                                join ws in _context.WorkflowQcSampling on s.Id equals ws.QcSamplingId
                                where ws.WorkflowDocumentCode == workflowDocumentCode
                                && ws.WorkflowDocumentCode == item.RecordId
                                select s).FirstOrDefault();
                if (sampling != null)
                {
                    result.Add(sampling);
                }
            }
            return result;
        }


        public async Task<bool> ValidationEditSampling(int samplingId)
        {
            bool statusValEdit = true;

            IList<string> actionList = new List<string>();

            //get worfklow sampling
            List<WorkflowQcSampling> worfklowQcSampling = await _workflowQcSamplingDataProvider.GetByWorkflowByQcSamplingId(samplingId);

            List<WorkflowHistoryQcSampling> workflowQcSamplingHistory = new List<WorkflowHistoryQcSampling>();
            string eDate = null;
            string eDate2 = null;

            foreach (var item in worfklowQcSampling)
            {
                DocumentHistoryResponseViewModel workflowHistory = await _reviewDataProvider.GetListHistoryWorkflow(item.WorkflowDocumentCode);

                foreach (var itemHistory in workflowHistory.History)
                {
                    if (((itemHistory.StatusName != "Complete") && (item.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_1)))
                    {
                        foreach (var itemPIC in itemHistory.PICs)
                        {
                            string actionUser = "";
                            if (itemPIC.ActionName == null && itemPIC.IsPending == true)
                            {
                                actionUser = ApplicationConstant.WORKFLOW_STATUS_PENDING;
                            }
                            else if (itemPIC.ActionName == null && itemPIC.IsPending == false)
                            {
                                actionUser = "Close By System";
                            }
                            else
                            {
                                actionUser = itemPIC.ActionName;
                            }
                            WorkflowHistoryQcSampling addWorkflowHistory = new WorkflowHistoryQcSampling()
                            {
                                Action = actionUser,
                                Note = itemPIC.Notes,
                                DateTime = itemPIC.ActionDate == null ? (actionUser == ApplicationConstant.WORKFLOW_STATUS_PENDING ? ApplicationConstant.MAX_DATETIME : eDate) : itemPIC.ActionDate,
                                PersonalName = itemPIC.OrgName,
                                PersonalNik = itemPIC.OrgId,
                                Position = itemPIC.OrgPositionName,
                                ChangeStatusTime = itemHistory.StatusChangeDate
                            };

                            eDate = addWorkflowHistory.DateTime;

                            workflowQcSamplingHistory.Add(addWorkflowHistory);

                            actionList.Add(actionUser);
                        }
                    }
                    else if (item.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2)
                    {
                        foreach (var itemPIC in itemHistory.PICs)
                        {
                            string actionUser = "";
                            if (itemPIC.ActionName == null && itemPIC.IsPending == true)
                            {
                                actionUser = ApplicationConstant.WORKFLOW_STATUS_PENDING;
                            }
                            else if (itemPIC.ActionName == null && itemPIC.IsPending == false)
                            {
                                actionUser = "Close By System";
                            }
                            else
                            {
                                actionUser = itemPIC.ActionName;
                            }
                            WorkflowHistoryQcSampling addWorkflowHistory = new WorkflowHistoryQcSampling()
                            {
                                Action = actionUser,
                                Note = itemPIC.Notes,
                                DateTime = itemPIC.ActionDate == null ? (actionUser == ApplicationConstant.WORKFLOW_STATUS_PENDING ? ApplicationConstant.MAX_DATETIME : eDate2) : itemPIC.ActionDate,
                                PersonalName = itemPIC.OrgName,
                                PersonalNik = itemPIC.OrgId,
                                Position = itemPIC.OrgPositionName,
                                ChangeStatusTime = itemHistory.StatusChangeDate
                            };
                            eDate2 = addWorkflowHistory.DateTime;

                            if (itemHistory.StatusName != ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT || addWorkflowHistory.Action != ApplicationConstant.WORKFLOW_ACTION_SUBMIT_NAME)
                            {
                                workflowQcSamplingHistory.Add(addWorkflowHistory);

                                actionList.Add(actionUser);
                            }
                        }
                    }
                }
            }
            List<WorkflowHistoryQcSampling> workflowHistoryQcs =
            workflowQcSamplingHistory.OrderByDescending(x => x.DateTime).ToList();

            //check status contain reject?
            if (actionList.Contains(ApplicationConstant.REJECTED_ACTION_NOTIF_ALT))
            {
                statusValEdit = false;
            }

            /*
             NOTE : 
                jika statusValEdit return nya "TRUE"
                data yg di pasang fungsi ini bisa di edit
                jika "FALSE" maka seharusnya tidak bisa di edit.
             */

            return statusValEdit;

        }

        public async Task<bool> getRiviewKabagSampling(int samplingId)
        {
            bool statusValEdit = true;

            //get worfklow sampling
            List<WorkflowQcSampling> worfklowQcSampling = await _workflowQcSamplingDataProvider.GetByWorkflowPhase2ByQcSamplingId(samplingId);

            if (worfklowQcSampling.Any())
            {
                statusValEdit = false;
            }

            return statusValEdit;
        }

        public async Task<RequestQcs> GetRequestBySamplingId(int samplingId)
        {
            return await (
                from rq in _context.RequestQcs
                join s in _context.QcSamplings on rq.Id equals s.RequestQcsId
                where s.Id == samplingId
                select rq
            ).FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetPurposeByRequestId(int requestQcsId)
        {
            List<int> result = new List<int>();
            var purposeIds = await
            (
                from rq in _context.RequestQcs
                join rp in _context.RequestPurposes on rq.Id equals rp.QcRequestId
                where rq.Id == requestQcsId
                orderby rp.PurposeId ascending
                select new
                {
                    PurposeId = rp.PurposeId
                }
            ).ToListAsync();
            foreach (var item in purposeIds)
            {
                result.Add(item.PurposeId);
            }
            return result;
        }

        public async Task<List<PurposesPersonel>> GetPurposePersonelByPurposeId(int purposeId)
        {
            var result = await
            (
                from pp in _context.PurposesPersonel
                where pp.PurposeId == purposeId
                select pp
            ).ToListAsync();
            return result;
        }

        public static int? GetLastSamplePointName(string str)
        {
            int? substr;
            try
            {
                substr = int.Parse(str.Substring(str.LastIndexOf('-') + 1));
            }
            catch
            {
                substr = null;
            }
            return substr;
        }

        public async Task<int> GetSampleAmountCountById(int id, int testParameter)
        {
            var result = await (from pttq in _context.ProductTestTypeQcs
                                join qs in _context.QcSamplings on pttq.RequestQcsId equals qs.RequestQcsId
                                where qs.Id == id && pttq.TestParameterId == testParameter
                                select pttq
                          ).FirstOrDefaultAsync();
            return result.SampleAmountCount;
        }

        public async Task<string> GetBuildingCodeBySamplingId(int samplingId)
        {
            var result = await (
                from data in _context.TransactionBuilding
                join r in _context.TransactionRoom on data.Id equals r.BuildingId
                join rq in _context.RequestQcs on r.Id equals rq.EmRoomId
                join s in _context.QcSamplings on rq.Id equals s.RequestQcsId
                where s.Id == samplingId
                select data.BuildingId
            ).FirstOrDefaultAsync();

            return result;
        }

        public async Task<string> GetPosIdBySamplingId(int samplingId)
        {
            var result = await (
                           from data in _context.QcSamplings
                           join r in _context.RequestQcs on data.RequestQcsId equals r.Id
                           join rq in _context.TransactionRoom on r.EmRoomId equals rq.Id
                           where data.Id == samplingId
                           select rq.PosId
                       ).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<string>> GetPosIdBySamplingIdV2(int samplingId)
        {
            var result = await (
                              from data in _context.QcSamplings
                              join rrm in _context.RequestRooms on data.RequestQcsId equals rrm.QcRequestId
                              join r in _context.TransactionRoom on rrm.RoomId equals r.Id
                              where data.Id == samplingId
                              select r.PosId
                          ).ToListAsync();

            return result;
        }

        public async Task<List<QcSamplingNotReceivedViewModel>> ListSamplingNotReceived(string search)
        {
            var shipmentStatus = new List<int>();
            shipmentStatus.Add(ApplicationConstant.STATUS_SHIPMENT_NOTHING);
            shipmentStatus.Add(ApplicationConstant.STATUS_SHIPMENT_RECEIVED);
            shipmentStatus.Add(ApplicationConstant.STATUS_SHIPMENT_LATE_SAMPLE);
            shipmentStatus.Add(ApplicationConstant.STATUS_SHIPMENT_LATE_RECIVED);

            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from s in _context.QcSamplings
                                join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                join rr in _context.RequestRooms on r.Id equals rr.QcRequestId into TempRoom
                                from tr in TempRoom.DefaultIfEmpty()
                                where ((EF.Functions.Like(r.NoBatch.ToLower(), "%" + filter + "%")) ||
                                (EF.Functions.Like(s.Code.ToLower(), "%" + filter + "%")) ||
                                (EF.Functions.Like(r.ItemName.ToLower(), "%" + filter + "%")) ||
                                (EF.Functions.Like(s.SamplingTypeName.ToLower(), "%" + filter + "%")) ||
                                (EF.Functions.Like(r.EmPhaseName.ToLower(), "%" + filter + "%")) ||
                                (EF.Functions.Like(r.EmRoomName.ToLower(), "%" + filter + "%")) ||
                                (EF.Functions.Like(tr.RoomCode.ToLower(), "%" + filter + "%"))) &&
                                s.SamplingTypeId == ApplicationConstant.SAMPLING_TYPE_ID_EMM &&
                                s.RowStatus == null
                                select new QcSamplingNotReceivedViewModel
                                {
                                    SamplingId = s.Id,
                                    Code = s.Code,
                                    Date = r.Date,
                                    NoRequest = r.NoRequest,
                                    NoBatch = r.NoBatch,
                                    TypeRequestId = r.TypeRequestId,
                                    TypeRequest = r.TypeRequest,
                                    SamplingTypeId = s.SamplingTypeId,
                                    SamplingTypeName = s.SamplingTypeName,
                                    ItemId = r.ItemId,
                                    ItemName = r.ItemName,
                                    EmRoomId = r.EmRoomId,
                                    EmRoomName = r.EmRoomName,
                                    EmPhaseId = r.EmPhaseId,
                                    EmPhaseName = r.EmPhaseName,
                                    Status = s.Status,
                                    CreatedBy = s.CreatedBy,
                                    CreatedAt = s.CreatedAt,
                                    ShipmentStatus = (from sp in _context.QcSamplingShipments
                                                      where sp.QrCode == s.Code
                                                      select sp.Status).FirstOrDefault(),
                                    RequestRooms = (from rr in _context.RequestRooms
                                                    where rr.QcRequestId == r.Id
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
                                                    }).ToList()

                                }).Where(x => !shipmentStatus.Contains(x.ShipmentStatus))
                                .OrderByDescending(x => x.CreatedAt).ToListAsync();

            return result;
        }

        public async Task<List<QcSample>> UpdateSampleReviewQaNote(InsertReviewQaNoteQcSample insert)
        {
            List<QcSample> result = new List<QcSample>();
            foreach (var item in insert.Samples)
            {
                var data = await _context.QcSamples.FirstOrDefaultAsync(x => x.Id == item.SampleId);
                if (data != null)
                {
                    data.ReviewQaNote = item.Notes;
                    data.UpdatedBy = insert.UpdatedBy;
                    data.UpdatedAt = DateHelper.Now();
                }

                result.Add(data);

            }
            await _context.SaveChangesAsync();

            return result;

        }

        public async Task<List<QcLabelBatchRelationViewModel>> GetPendingReview(string workflowStatus)
        {
            var inReviewStatusList = new List<string>()
            {
                ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_PAIRING,
                ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_AHLI_MUDA,
                ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KASIE,
                ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG,
                ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG_QC,
                ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA,
            };

            var result = new List<QcLabelBatchRelationViewModel>();

            var samplings = await (from s in _context.QcSamplings
                                   join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                   where inReviewStatusList.Contains(s.WorkflowStatus)
                                   && s.WorkflowStatus == workflowStatus
                                   select new QcLabelBatchRelationViewModel
                                   {
                                       RequestId = r.Id,
                                       SamplingId = s.Id,
                                       Code = s.Code,
                                       OrgId = r.OrgId,
                                       OrgName = r.OrgName,
                                       Date = r.Date,
                                       NoRequest = r.NoRequest,
                                       NoBatch = r.NoBatch,
                                       TypeRequestId = r.TypeRequestId,
                                       TypeRequest = r.TypeRequest,
                                       SamplingTypeId = s.SamplingTypeId,
                                       SamplingTypeName = s.SamplingTypeName,
                                       ItemId = r.ItemId,
                                       ItemName = r.ItemName,
                                       TypeFormId = r.TypeFormId,
                                       TypeFormName = r.TypeFormName,
                                       PurposeId = r.PurposeId,
                                       PurposeName = r.PurposeName,
                                       EmRoomId = r.EmRoomId,
                                       EmRoomName = r.EmRoomName,
                                       EmRoomGradeId = r.EmRoomGradeId,
                                       EmRoomGradeName = r.EmRoomGradeName,
                                       EmPhaseId = r.EmPhaseId,
                                       EmPhaseName = r.EmPhaseName,
                                       SamplingDateFrom = s.SamplingDateFrom,
                                       SamplingDateTo = s.SamplingDateTo,
                                       Status = s.Status,
                                       CreatedBy = s.CreatedBy,
                                       CreatedAt = s.CreatedAt
                                   }).ToListAsync();

            if (!samplings.Any())
            {
                return result;
            }

            var samplingIds = samplings.Select(x => x.SamplingId).Distinct().ToList();

            var workflowBySamplings = await _workflowQcSamplingDataProvider.GetInWorkflowBySamplingIds(samplingIds);

            if (!workflowBySamplings.Any())
            {
                return result;
            }

            var requestIds = samplings.Select(x => x.RequestId).Distinct().ToList();

            var requestPurposes = await (from rp in _context.RequestPurposes
                                         where requestIds.Contains(rp.QcRequestId)
                                               && rp.RowStatus == null
                                         select new RequestPurposesViewModel
                                         {
                                             Id = rp.Id,
                                             PurposeId = rp.PurposeId,
                                             PurposeCode = rp.PurposeCode,
                                             PurposeName = rp.PurposeName,
                                             RequestId = rp.QcRequestId
                                         }).ToListAsync();

            var concurrentList = new ConcurrentBag<QcLabelBatchRelationViewModel>();

            Parallel.ForEach(samplings, sampling =>
            {
                var workflowQcSampling = workflowBySamplings.FirstOrDefault(x => x.QcSamplingId == sampling.SamplingId);

                if (workflowQcSampling == null)
                {
                    return;
                }

                sampling.WorkflowDocumentCode = workflowQcSampling.WorkflowDocumentCode;
                sampling.RequestPurposes = requestPurposes.Where(x => x.RequestId == sampling.RequestId).ToList();
                concurrentList.Add(sampling);
            });

            result = concurrentList.ToList();

            return result;
        }

        public async Task<List<QcLabelBatchRelationViewModel>> GetCompleteReview()
        {
            var inReviewStatusList = new List<string>()
            {
                ApplicationConstant.WORKFLOW_STATUS_NAME_COMPLETE
            };

            var result = new List<QcLabelBatchRelationViewModel>();

            var samplings = await (from s in _context.QcSamplings
                                   join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                   where inReviewStatusList.Contains(s.WorkflowStatus)
                                   select new QcLabelBatchRelationViewModel
                                   {
                                       RequestId = r.Id,
                                       SamplingId = s.Id,
                                       Code = s.Code,
                                       OrgId = r.OrgId,
                                       OrgName = r.OrgName,
                                       Date = r.Date,
                                       NoRequest = r.NoRequest,
                                       NoBatch = r.NoBatch,
                                       TypeRequestId = r.TypeRequestId,
                                       TypeRequest = r.TypeRequest,
                                       SamplingTypeId = s.SamplingTypeId,
                                       SamplingTypeName = s.SamplingTypeName,
                                       ItemId = r.ItemId,
                                       ItemName = r.ItemName,
                                       TypeFormId = r.TypeFormId,
                                       TypeFormName = r.TypeFormName,
                                       PurposeId = r.PurposeId,
                                       PurposeName = r.PurposeName,
                                       EmRoomId = r.EmRoomId,
                                       EmRoomName = r.EmRoomName,
                                       EmRoomGradeId = r.EmRoomGradeId,
                                       EmRoomGradeName = r.EmRoomGradeName,
                                       EmPhaseId = r.EmPhaseId,
                                       EmPhaseName = r.EmPhaseName,
                                       SamplingDateFrom = s.SamplingDateFrom,
                                       SamplingDateTo = s.SamplingDateTo,
                                       Status = s.Status,
                                       WorkflowStatus = s.WorkflowStatus,
                                       CreatedBy = s.CreatedBy,
                                       CreatedAt = s.CreatedAt
                                   }).ToListAsync();

            return samplings;
        }


    }
}
