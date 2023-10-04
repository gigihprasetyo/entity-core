using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.Infrastructure;
using qcs_product.API.BusinessProviders;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using qcs_product.API.Helpers;
using qcs_product.API.WorkflowModels;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace qcs_product.API.DataProviders.Collection
{
    public class QcTestDataProvider : IQcTestDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly IQcProcessDataProvider _dataProviderQcProcess;
        private readonly IQcRequestDataProvider _requestDataProvider;
        private readonly IWorkflowQcTransactionGroupDataProvider _workflowTransactionGroupdataProvider;
        private readonly IWorkflowServiceDataProvider _workflowServiceDataProvider;
        private readonly ILogger<QcTestDataProvider> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IGenerateQcProcessDataProvider _generateQcProcessDataProvider;
        private readonly IGenerateQcResultDataProvider _generateQcResultDataProvider;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBioHRIntegrationBussinesProviders _bioHrBusinessProvider;

        [ExcludeFromCodeCoverage]
        public QcTestDataProvider(
            QcsProductContext context,
            IQcProcessDataProvider dataProviderQcProcess,
            IQcRequestDataProvider requestDataProvider,
            IWorkflowQcTransactionGroupDataProvider workflowTransactionGroupdataProvider,
            IWorkflowServiceDataProvider workflowServiceDataProvider,
            ILogger<QcTestDataProvider> logger,
            IServiceScopeFactory serviceScopeFactory,
            IGenerateQcProcessDataProvider generateQcProcessDataProvider,
            IGenerateQcResultDataProvider generateQcResultDataProvider,
            IServiceScopeFactory scopeFactory,
            IBioHRIntegrationBussinesProviders bioHrBusinessProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dataProviderQcProcess = dataProviderQcProcess ?? throw new ArgumentNullException(nameof(dataProviderQcProcess));
            _requestDataProvider = requestDataProvider ?? throw new ArgumentNullException(nameof(requestDataProvider));
            _workflowTransactionGroupdataProvider = workflowTransactionGroupdataProvider ?? throw new ArgumentNullException(nameof(workflowTransactionGroupdataProvider));
            _workflowServiceDataProvider = workflowServiceDataProvider ?? throw new ArgumentNullException(nameof(workflowServiceDataProvider));
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _generateQcProcessDataProvider = generateQcProcessDataProvider;
            _generateQcResultDataProvider = generateQcResultDataProvider;
            _scopeFactory = scopeFactory;
            _bioHrBusinessProvider = bioHrBusinessProvider;
        }

        public async Task<List<QcTransactionGroupViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, string orderDir)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = (from t in _context.QcTransactionGroups
                          join qp in _context.QcProcess on t.StatusProses equals qp.Id into defaultqp
                          from qcp in defaultqp.DefaultIfEmpty()
                          join qts in _context.QcTransactionSamplings on t.Id equals qts.QcTransactionGroupId
                          join qs in _context.QcSamplings on qts.QcSamplingId equals qs.Id
                          join rq in _context.RequestQcs on qs.RequestQcsId equals rq.Id
                          join tb in _context.TransactionBatches on rq.Id equals tb.RequestQcsId into tbGroup
                          from tb in tbGroup.DefaultIfEmpty()
                          join tbl in _context.TransactionBatchLines on tb.Id equals tbl.TrsBatchId into tblGroup
                          from tbl in tblGroup.DefaultIfEmpty()
                          where ((EF.Functions.Like(t.PersonelName.ToLower(), "%" + filter + "%")) ||
                              (EF.Functions.Like(t.Code.ToLower(), "%" + filter + "%")) ||
                              (EF.Functions.Like(t.PersonelPairingName.ToLower(), "%" + filter + "%")) ||
                              (EF.Functions.Like(t.QcProcessName.ToLower(), "%" + filter + "%")) ||
                              EF.Functions.Like(rq.NoBatch.ToLower(), "%" + filter + "%")
                          )
                          && status.Contains(t.Status)
                          && t.RowStatus == null
                          select new QcTransactionGroupViewModel
                          {
                              Id = t.Id,
                              Code = t.Code,
                              QcProcessId = t.QcProcessId,
                              QcProcessName = t.QcProcessName,
                              TestDate = t.TestDate,
                              ProductPhaseId = rq.ProductPhaseId,
                              ProductPhaseName = rq.ProductPhaseName,
                              BatchNumber = rq.NoBatch,
                              PersonelNik = t.PersonelNik,
                              PersonelName = t.PersonelName,
                              PersonelPairingNik = t.PersonelPairingNik,
                              PersonelPairingName = t.PersonelPairingName,
                              SampleCount = (from ts in _context.QcTransactionSamples
                                             where ts.QcTransactionGroupId == t.Id
                                             select ts.Id).Count(),
                              Status = t.Status,
                              StatusProses = qcp.Name,
                              CreatedBy = t.CreatedBy,
                              CreatedAt = t.CreatedAt

                          }).Where(x =>
                          (x.TestDate >= startDate || !startDate.HasValue) &&
                          (x.TestDate <= endDate || !endDate.HasValue))
                .Distinct()
                .AsQueryable();

            /*if (orderDir == "asc")
            {
                result = result.OrderBy(y => y.CreatedAt).AsQueryable();
            }
            else
            {
                result = result.OrderByDescending(y => y.CreatedAt).AsQueryable();
            }*/

            switch (orderDir)
            {
                case ApplicationConstant.ASC_ORDER:
                    // sort asc
                    result = result.OrderBy(y => y.TestDate).AsQueryable();
                    break;
                case ApplicationConstant.ASC_IN_REVIEW_AHLI_MUDA_UJI:
                    // sort in review ahli muda uji & data lama
                    result = result.OrderBy(y =>
                        y.Status == ApplicationConstant.STATUS_TEST_INREVIEW_AHLI_MUDA_QC ? 1 :
                        y.Status == ApplicationConstant.STATUS_TEST_DRAFT ? 2 :
                        y.Status == ApplicationConstant.STATUS_TEST_READYTOTEST ? 2 :
                        y.Status == ApplicationConstant.STATUS_TEST_INPROGRESS ? 2 :
                        y.Status == ApplicationConstant.STATUS_TEST_INREVIEW_PAIRING ? 2 :
                        y.Status == ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_QC ? 2 :
                        y.Status == ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_PRODUKSI ? 2 :
                        y.Status == ApplicationConstant.STATUS_TEST_INREVIEW_QA ? 2 :
                        y.Status == ApplicationConstant.STATUS_TEST_APPROVED ? 2 :
                        y.Status == ApplicationConstant.STATUS_TEST_REJECTED ? 2 : 2
                    ).ThenBy(y => y.TestDate).AsQueryable();
                    break;
                case ApplicationConstant.ASC_IN_REVIEW_KABAG_UJI:
                    // sort in review kabag uji & data lama
                    result = result.OrderBy(y =>
                        y.Status == ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_QC ? 1 :
                        y.Status == ApplicationConstant.STATUS_TEST_DRAFT ? 2 :
                        y.Status == ApplicationConstant.STATUS_TEST_READYTOTEST ? 3 :
                        y.Status == ApplicationConstant.STATUS_TEST_INPROGRESS ? 4 :
                        y.Status == ApplicationConstant.STATUS_TEST_INREVIEW_PAIRING ? 5 :
                        y.Status == ApplicationConstant.STATUS_TEST_INREVIEW_AHLI_MUDA_QC ? 6 :
                        y.Status == ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_PRODUKSI ? 7 :
                        y.Status == ApplicationConstant.STATUS_TEST_INREVIEW_QA ? 8 :
                        y.Status == ApplicationConstant.STATUS_TEST_APPROVED ? 9 :
                        y.Status == ApplicationConstant.STATUS_TEST_REJECTED ? 10 : 5
                    ).ThenBy(y => y.TestDate).AsQueryable();
                    break;
                default:
                    result = result.OrderByDescending(y => y.TestDate).AsQueryable();
                    break;
            }


            var resultData = new List<QcTransactionGroupViewModel>();

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

        public async Task<QcTransactionGroupDetailViewModel> GetById(int id)
        {
            //get worfklow testing
            List<WorkflowQcTransactionGroup> worfklowQcTransactionGroup = await _workflowTransactionGroupdataProvider.GetByWorkflowByQcTransactionGroupId(id);

            List<WorkflowHistoryQcSampling> workflowQcTransactionGroupHistory = new List<WorkflowHistoryQcSampling>();
            string eDate = null;
            string eDate2 = null;

            foreach (var item in worfklowQcTransactionGroup)
            {
                DocumentHistoryResponseViewModel workflowHistory = await _workflowServiceDataProvider.GetListHistoryWorkflow(item.WorkflowDocumentCode);

                foreach (var itemHistory in workflowHistory.History)
                {
                    if (((itemHistory.StatusName != "Complete") && (item.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_1)))
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
                                DateTime = itemPIC.ActionDate == null ? eDate : itemPIC.ActionDate,
                                PersonalName = itemPIC.OrgName,
                                PersonalNik = itemPIC.OrgId,
                                Position = itemPIC.OrgPositionName,
                                ChangeStatusTime = itemPIC.ActionDate == null ? (actionUser == ApplicationConstant.WORKFLOW_STATUS_PENDING ? ApplicationConstant.MAX_DATETIME : eDate) : itemPIC.ActionDate
                            };

                            eDate = addWorkflowHistory.DateTime;

                            workflowQcTransactionGroupHistory.Add(addWorkflowHistory);
                        }
                    }
                    else if (item.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_2)
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
                                DateTime = itemPIC.ActionDate == null ? eDate2 : itemPIC.ActionDate,
                                PersonalName = itemPIC.OrgName,
                                PersonalNik = itemPIC.OrgId,
                                Position = itemPIC.OrgPositionName,
                                ChangeStatusTime = itemPIC.ActionDate == null ? (actionUser == ApplicationConstant.WORKFLOW_STATUS_PENDING ? ApplicationConstant.MAX_DATETIME : eDate2) : itemPIC.ActionDate
                            };
                            eDate2 = addWorkflowHistory.DateTime;

                            if (itemHistory.StatusName != ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT || addWorkflowHistory.Action != ApplicationConstant.WORKFLOW_ACTION_SUBMIT_NAME)
                            {
                                workflowQcTransactionGroupHistory.Add(addWorkflowHistory);
                            }
                        }
                    }
                }
            }
            List<WorkflowHistoryQcSampling> workflowHistoryQcs =
            workflowQcTransactionGroupHistory.OrderByDescending(x => x.ChangeStatusTime).ToList();

            WorkflowQcTransactionGroup currentWorkflow = await _workflowTransactionGroupdataProvider.GetByWorkflowByQcTransactionGroupIdIsInWorkflow(id);

            var result = await (from t in _context.QcTransactionGroups
                                join qp in _context.QcProcess on t.QcProcessId equals qp.Id
                                where t.Id == id
                                && t.RowStatus == null
                                select new QcTransactionGroupDetailViewModel
                                {
                                    Id = t.Id,
                                    Code = t.Code,
                                    QcProcessId = t.QcProcessId,
                                    QcProcessName = t.QcProcessName,
                                    AddSampleLayoutType = qp.AddSampleLayoutType,
                                    TestDate = t.TestDate,
                                    PersonelNik = t.PersonelNik,
                                    PersonelName = t.PersonelName,
                                    PersonelPairingNik = t.PersonelPairingNik,
                                    PersonelPairingName = t.PersonelPairingName,
                                    Status = t.Status,
                                    CreatedBy = t.CreatedBy,
                                    CreatedAt = t.CreatedAt,
                                    WorkflowCode = currentWorkflow != null ? currentWorkflow.WorkflowCode : null,
                                    SamplingBatchData = (from tsb in _context.QcTransactionSamplings
                                                         join s in _context.QcSamplings on tsb.QcSamplingId equals s.Id
                                                         join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                                         where tsb.QcTransactionGroupId == t.Id
                                                         select new QcTransactionSamplingRelationViewModel
                                                         {
                                                             Id = tsb.Id,
                                                             SamplingId = s.Id,
                                                             BacthQrCode = s.Code,
                                                             NoBatch = r.NoBatch,
                                                             PhaseId = r.EmPhaseId,
                                                             PhaseName = r.EmPhaseName,
                                                             RoomId = r.EmRoomId,
                                                             RoomName = r.EmRoomName,
                                                             ShipmentStatus = (from shp1 in _context.QcSamplingShipments
                                                                               where shp1.QrCode == s.Code
                                                                               select shp1.Status).FirstOrDefault(),
                                                             ShipmentStartDate = (from shp2 in _context.QcSamplingShipments
                                                                                  where shp2.QcSamplingId == s.Id
                                                                                  select shp2.StartDate).FirstOrDefault(),
                                                             ShipmentEndDate = (from shp3 in _context.QcSamplingShipments
                                                                                where shp3.QcSamplingId == s.Id
                                                                                select shp3.EndDate).FirstOrDefault(),
                                                             SamplingTestTransaction = (from qts in _context.QcTransactionSamplings
                                                                                        where qts.QcSamplingId == s.Id
                                                                                        select qts).Count()
                                                         }).ToList(),
                                    SamplesData = (from ts in _context.QcTransactionSamples
                                                   join sm in _context.QcSamples on ts.QcSampleId equals sm.Id
                                                   join s in _context.QcSamplings on sm.QcSamplingId equals s.Id
                                                   join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                                   join tp in _context.TransactionTestParameter on sm.TestParamId equals tp.Id
                                                   join tsc in _context.TransactionTestScenario on sm.TestScenarioId equals tsc.Id
                                                   where ts.QcTransactionGroupId == t.Id
                                                   select new QcTransactionSampleRelationViewModel
                                                   {
                                                       Id = ts.Id,
                                                       QcSampleId = ts.QcSampleId,
                                                       SampleCode = sm.Code,
                                                       NoBatch = r.NoBatch,
                                                       NoRequest = r.NoRequest,
                                                       EmRoomId = r.EmRoomId,
                                                       EmRoomName = r.EmRoomName,
                                                       GradeRoomId = sm.GradeRoomId,
                                                       GradeRoomName = sm.GradeRoomName,
                                                       GradeRoomCode = (sm.GradeRoomId != null ? (from mgr in _context.TransactionGradeRoom
                                                                                                  where mgr.Id == sm.GradeRoomId
                                                                                                  select mgr.Code).FirstOrDefault() : null),
                                                       EmPhaseId = r.EmPhaseId,
                                                       EmPhaseName = r.EmPhaseName,
                                                       TestParamId = sm.TestParamId,
                                                       TestParamName = sm.TestParamName,
                                                       TestParamShortName = tp.ShortName,
                                                       TestParamSequence = GetTestParamSequenceByCode(tp.ShortName),
                                                       PersonalId = sm.PersonalId,
                                                       PersonalInitial = sm.PersonalInitial,
                                                       PersonalName = sm.PersonalName,
                                                       ShipmentStartDate = (from shp2 in _context.QcSamplingShipments
                                                                            where shp2.QcSamplingId == s.Id
                                                                            select shp2.StartDate).FirstOrDefault(),
                                                       ShipmentEndDate = (from shp3 in _context.QcSamplingShipments
                                                                          where shp3.QcSamplingId == s.Id
                                                                          select shp3.EndDate).FirstOrDefault(),
                                                       CreatedBy = ts.CreatedBy,
                                                       CreatedAt = ts.CreatedAt,
                                                       SamplingPointId = sm.SamplingPointId,
                                                       SamplingPointCode = sm.SamplingPointCode,
                                                       TestScenarioLabel = tsc.Label,
                                                       TestScenarioCode = (tsc.Label == "at_rest"
                                                                                ? 0
                                                                                : 1),
                                                       TestVariableThreshold = (from tv in _context.TransactionTestVariable
                                                                                join en in _context.EnumConstant on tv.TresholdOperator equals en.TypeId
                                                                                join rtsp in _context.TransactionRelTestScenarioParam on tv.TestParameterId equals rtsp.Id
                                                                                join ts in _context.TransactionTestScenario on rtsp.TestScenarioId equals ts.Id
                                                                                join tp in _context.TransactionTestParameter on rtsp.TestParameterId equals tp.Id
                                                                                where tp.Id == sm.TestParamId
                                                                                && ts.Id == sm.TestScenarioId
                                                                                && en.KeyGroup == "threshold_operator"
                                                                                select new TestVariableViewModel
                                                                                {
                                                                                    Id = tv.Id,
                                                                                    VariableName = tv.VariableName,
                                                                                    Sequence = tv.Sequence.Value,
                                                                                    TresholdOperator = tv.TresholdOperator,
                                                                                    TresholdOperatorName = en.Name,
                                                                                    TresholdValue = tv.TresholdValue,
                                                                                    TresholdMin = tv.ThresholdValueFrom.Value,
                                                                                    TresholdMax = tv.ThresholdValueTo.Value,
                                                                                }).ToList(),
                                                       TestParamIndex = 1,
                                                       SamplingDateTimeFrom = sm.SamplingDateTimeFrom,
                                                       SamplingDateTimeTo = sm.SamplingDateTimeTo,
                                                       Purpose = (from rp in _context.TransactionRoomPurpose
                                                                  join rgrp in _context.RequestGroupRoomPurpose on rp.Id equals rgrp.RoomPurposeId
                                                                  join s in _context.QcSamplings on rgrp.RequestQcsId equals s.RequestQcsId
                                                                  join rsp in _context.TransactionRelRoomSamplingPoint on rp.Id equals rsp.RoomPurposeId
                                                                  join rpmtp in _context.TransactionRoomPurposeToMasterPurpose on rp.Id equals rpmtp.RoomPurposeId
                                                                  join purp in _context.TransactionPurposes on rpmtp.PurposeId equals purp.Id
                                                                  where s.Id == sm.QcSamplingId
                                                                  && rsp.SamplingPoinId == sm.SamplingPointId
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
                                                                           where s.Id == sm.QcSamplingId
                                                                           && rts.SamplingPoinId == sm.SamplingPointId
                                                                           select new RequestPurposesViewModel
                                                                           {
                                                                               PurposeId = purp.Id,
                                                                               PurposeCode = purp.Code,
                                                                               PurposeName = purp.Name
                                                                           }).ToList(),
                                                   }).ToList(),
                                }).FirstOrDefaultAsync();

            foreach (var sample in result.SamplesData)
            {
                if (sample.SamplingPointCode.Contains('-'))
                {
                    sample.FdLastSamplingPointCode = (sample.TestParamId == 4 ? GetLastSamplingPointCode(sample.SamplingPointCode) : null);
                    sample.FirstSamplingPointCode = sample.SamplingPointCode.Substring(0, sample.SamplingPointCode.LastIndexOf('-'));
                    sample.LastSamplingPointCode = GetLastSamplingPointCode(sample.SamplingPointCode);
                }
            }

            //menambahkan createdByName
            result.CreatedByName = await _GetNameByNik(result.CreatedBy);

            result.SamplesData = result.SamplesData.OrderBy(x => x.TestParamSequence)
                                                    .ThenBy(x => x.EmRoomName)
                                                    .ThenBy(x => x.GradeRoomCode)
                                                    .ThenBy(x => x.FdLastSamplingPointCode)
                                                    .ThenBy(x => x.FirstSamplingPointCode)
                                                    .ThenBy(x => x.LastSamplingPointCode)
                                                    .ThenBy(x => x.TestScenarioCode)
                                                    .ThenBy(x => x.SamplingDateTimeFrom)
                                                    // .ThenBy(x => x.SampleCode)
                                                    .ThenBy(x => x.Id)
                                                    .ToList();

            if (result != null)
            {
                var checkQCProcessDataParent = await (from c1 in _context.QcTransactionGroupProcesses
                                                      where c1.QcTransactionGroupId == result.Id
                                                      && c1.RowStatus == null
                                                      select c1.Id).CountAsync();

                //TODO next-nya perlu dipertimbangkan untuk menggunakan recursive
                if (checkQCProcessDataParent > 0)
                {
                    result.QcTransactionGroupProcess =
                        (from qtp in _context.QcTransactionGroupProcesses
                         where qtp.QcTransactionGroupId == result.Id
                         && qtp.RowStatus == null
                         select new QcTransactionGroupProcessRelViewModel
                         {
                             Id = qtp.Id,
                             QcTransactionGroupId = qtp.QcTransactionGroupId,
                             Sequence = qtp.Sequence,
                             Name = qtp.Name,
                             ParentId = qtp.ParentId,
                             RoomId = qtp.RoomId,
                             IsInputForm = qtp.IsInputForm,
                             QcProcessId = qtp.QcProcessId,
                             CreatedAt = qtp.CreatedAt,
                             CreatedBy = qtp.CreatedBy,
                             WorkflowHistory = workflowHistoryQcs,
                         }).FirstOrDefault();

                    var checkQCProcessDataChild = await (from c2 in _context.QcTransactionGroupProcesses
                                                         where c2.ParentId == result.QcTransactionGroupProcess.Id
                                                         && c2.RowStatus == null
                                                         select c2.Id).CountAsync();

                    if (checkQCProcessDataChild > 0)
                    {
                        result.QcTransactionGroupProcess.
                            QcTransactionGroupProcess =
                               (from qtpC in _context.QcTransactionGroupProcesses
                                where qtpC.ParentId == result.QcTransactionGroupProcess.Id
                                && qtpC.RowStatus == null
                                select new QcTransactionGroupProcessRelViewModel
                                {
                                    Id = qtpC.Id,
                                    QcTransactionGroupId = qtpC.QcTransactionGroupId,
                                    Sequence = qtpC.Sequence,
                                    Name = qtpC.Name,
                                    ParentId = qtpC.ParentId,
                                    RoomId = qtpC.RoomId,
                                    IsInputForm = qtpC.IsInputForm,
                                    QcProcessId = qtpC.QcProcessId,
                                    CreatedAt = qtpC.CreatedAt,
                                    CreatedBy = qtpC.CreatedBy,
                                    WorkflowHistory = workflowHistoryQcs,
                                    QcTransactionGroupFormMaterial = (from qtm in _context.QcTransactionGroupFormMaterials
                                                                      join qfs in _context.QcTransactionGroupFormSections on qtm.QcTransactionGroupSectionId equals qfs.Id
                                                                      join uom in _context.Uoms on qtm.Uom equals uom.UomId
                                                                      join uomPackage in _context.Uoms on qtm.UomPackage equals uomPackage.UomId
                                                                      where qtm.QcTransactionGroupProcessId == qtpC.Id
                                                                      && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_MATERIAL
                                                                      && qtm.RowStatus == null
                                                                      select new QcTransactionGroupFormMaterialViewModel
                                                                      {
                                                                          Id = qtm.Id,
                                                                          QcTransactionGroupProcessId = qtm.QcTransactionGroupProcessId,
                                                                          Sequence = qtm.Sequence,
                                                                          ItemId = qtm.ItemId,
                                                                          Code = qtm.Code,
                                                                          Name = qtm.Name,
                                                                          DefaultPackageQty = qtm.DefaultPackageQty,
                                                                          UomPackage = qtm.UomPackage,
                                                                          UomUomPackageLabel = uomPackage.Label,
                                                                          DefaultQty = qtm.DefaultQty,
                                                                          Uom = qtm.Uom,
                                                                          UomLabel = uom.Label,
                                                                          QcProcessId = qtm.QcProcessId,
                                                                          GroupName = qtm.GroupName,
                                                                          CreatedAt = qtm.CreatedAt,
                                                                          CreatedBy = qtm.CreatedBy
                                                                      }).OrderBy(x => x.Sequence).ToList(),

                                    QcTransactionGroupFormTool = (from qtt in _context.QcTransactionGroupFormTools
                                                                  join qfs in _context.QcTransactionGroupFormSections on qtt.QcTransactionGroupSectionId equals qfs.Id
                                                                  where qtt.QcTransactionGroupProcessId == qtpC.Id
                                                                  && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_TOOL
                                                                  && qtt.RowStatus == null
                                                                  select new QcTransactionGroupFormToolViewModel
                                                                  {
                                                                      Id = qtt.Id,
                                                                      QcTransactionGroupProcessId = qtt.QcTransactionGroupProcessId,
                                                                      Sequence = qtt.Sequence,
                                                                      ToolId = qtt.ToolId,
                                                                      ItemId = qtt.ItemId,
                                                                      Code = qtt.Code,
                                                                      Name = qtt.Name,
                                                                      Quantity = qtt.Quantity,
                                                                      QcProcessId = qtt.QcProcessId,
                                                                      CreatedAt = qtt.CreatedAt,
                                                                      CreatedBy = qtt.CreatedBy
                                                                  }).OrderBy(x => x.Sequence).ToList(),

                                    QcTransactionGroupFormProcedure = (from qtfp in _context.QcTransactionGroupFormProcedures
                                                                       join qfs in _context.QcTransactionGroupFormSections on qtfp.QcTransactionGroupSectionId equals qfs.Id
                                                                       where qtfp.QcTransactionGroupProcessId == qtpC.Id
                                                                       && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_PROCEDURE
                                                                       && qtfp.RowStatus == null
                                                                       select new QcTransactionGroupFormProcedureViewModel
                                                                       {
                                                                           Id = qtfp.Id,
                                                                           QcTransactionGroupProcessId = qtfp.QcTransactionGroupProcessId,
                                                                           Sequence = qtfp.Sequence,
                                                                           Description = qtfp.Description,
                                                                           FormProcedureId = qtfp.FormProcedureId,
                                                                           CreatedAt = qtfp.CreatedAt,
                                                                           CreatedBy = qtfp.CreatedBy,
                                                                           QcTransactionGroupFormParameter = (from qtfpr in _context.QcTransactionGroupFormParameters
                                                                                                              join it in _context.InputTypes on qtfpr.InputType equals it.TypeId
                                                                                                              join uom in _context.Uoms on qtfpr.Uom equals uom.UomId
                                                                                                              where qtfpr.QcTransactionGroupFormProcedureId == qtfp.Id
                                                                                                              && qtfpr.RowStatus == null
                                                                                                              select new QcTransactionGroupFormParameterViewModel
                                                                                                              {
                                                                                                                  Id = qtfpr.Id,
                                                                                                                  QcTransactionGroupFormProcedureId = qtfpr.QcTransactionGroupFormProcedureId,
                                                                                                                  Sequence = qtfpr.Sequence,
                                                                                                                  Label = qtfpr.Label,
                                                                                                                  Code = qtfpr.Code,
                                                                                                                  InputType = qtfpr.InputType,
                                                                                                                  InputTypeLabel = it.Label,
                                                                                                                  Reference = it.Reference,
                                                                                                                  ReferenceType = it.ReferenceType,
                                                                                                                  Uom = qtfpr.Uom,
                                                                                                                  UomLabel = uom.Label,
                                                                                                                  ThresholdOperator = qtfpr.ThresholdOperator,
                                                                                                                  ThresholdValue = qtfpr.ThresholdValue,
                                                                                                                  ThresholdValueTo = qtfpr.ThresholdValueTo,
                                                                                                                  ThresholdValueFrom = qtfpr.ThresholdValueFrom,
                                                                                                                  NeedAttachment = qtfpr.NeedAttachment,
                                                                                                                  Note = qtfpr.Note,
                                                                                                                  FormProcedureId = qtfpr.FormProcedureId,
                                                                                                                  IsForAllSample = qtfpr.IsForAllSample,
                                                                                                                  IsResult = qtfpr.IsResult,
                                                                                                                  DefaultValue = qtfpr.DefaultValue,
                                                                                                                  CreatedAt = qtfpr.CreatedAt,
                                                                                                                  CreatedBy = qtfpr.CreatedBy,
                                                                                                                  GroupValues = (from gv in _context.QcTransactionGroupValues
                                                                                                                                 where gv.QcTransactionGroupFormParameterId == qtfpr.Id
                                                                                                                                 && gv.RowStatus == null
                                                                                                                                 select new GroupValue
                                                                                                                                 {
                                                                                                                                     Id = gv.Id,
                                                                                                                                     Sequence = gv.Sequence,
                                                                                                                                     Value = gv.Value,
                                                                                                                                     AttchmentFile = gv.AttchmentFile,
                                                                                                                                     QcTransactionGroupFormMaterialId = gv.QcTransactionGroupFormMaterialId,
                                                                                                                                     QcTransactionGroupFormToolId = gv.QcTransactionGroupFormToolId
                                                                                                                                 }).OrderBy(x => x.Sequence).ToList(),
                                                                                                                  GroupSampleValues = (from gsv in _context.QcTransactionSampleValues
                                                                                                                                       join gs in _context.QcTransactionSamples on gsv.QcTransactionSampleId equals gs.Id
                                                                                                                                       join smpl in _context.QcSamples on gs.QcSampleId equals smpl.Id
                                                                                                                                       where gsv.QcTransactionGroupFormParameterId == qtfpr.Id
                                                                                                                                       && gsv.RowStatus == null
                                                                                                                                       select new GroupSampleValue
                                                                                                                                       {
                                                                                                                                           Id = gsv.Id,
                                                                                                                                           QcTransactionSampleId = gsv.QcTransactionSampleId,
                                                                                                                                           SampleCode = smpl.Code,
                                                                                                                                           Sequence = gsv.Sequence,
                                                                                                                                           Value = gsv.Value,
                                                                                                                                           AttchmentFile = gsv.AttchmentFile,
                                                                                                                                           QcTransactionGroupFormMaterialId = gsv.QcTransactionGroupFromMaterialId,
                                                                                                                                           QcTransactionGroupFormToolId = gsv.QcTransactionGroupFromToolId
                                                                                                                                       }).OrderBy(x => x.Sequence).ToList(),
                                                                                                              }).OrderBy(x => x.Sequence).ToList()
                                                                       }).OrderBy(x => x.Sequence).ToList(),

                                    QcTransactionGroupFormGeneral = (from qtfp in _context.QcTransactionGroupFormProcedures
                                                                     join qfs in _context.QcTransactionGroupFormSections on qtfp.QcTransactionGroupSectionId equals qfs.Id
                                                                     where qtfp.QcTransactionGroupProcessId == qtpC.Id
                                                                     && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_NOTE
                                                                     && qtfp.RowStatus == null
                                                                     select new QcTransactionGroupFormGeneralViewModel
                                                                     {
                                                                         Id = qtfp.Id,
                                                                         QcTransactionGroupProcessId = qtfp.QcTransactionGroupProcessId,
                                                                         Sequence = qtfp.Sequence,
                                                                         Description = qtfp.Description,
                                                                         FormProcedureId = qtfp.FormProcedureId,
                                                                         CreatedAt = qtfp.CreatedAt,
                                                                         CreatedBy = qtfp.CreatedBy,
                                                                         QcTransactionGroupFormParameter = (from qtfpr in _context.QcTransactionGroupFormParameters
                                                                                                            join it in _context.InputTypes on qtfpr.InputType equals it.TypeId
                                                                                                            join uom in _context.Uoms on qtfpr.Uom equals uom.UomId
                                                                                                            where qtfpr.QcTransactionGroupFormProcedureId == qtfp.Id
                                                                                                            && qtfpr.RowStatus == null
                                                                                                            select new QcTransactionGroupFormParameterViewModel
                                                                                                            {
                                                                                                                Id = qtfpr.Id,
                                                                                                                QcTransactionGroupFormProcedureId = qtfpr.QcTransactionGroupFormProcedureId,
                                                                                                                Sequence = qtfpr.Sequence,
                                                                                                                Label = qtfpr.Label,
                                                                                                                Code = qtfpr.Code,
                                                                                                                InputType = qtfpr.InputType,
                                                                                                                InputTypeLabel = it.Label,
                                                                                                                Reference = it.Reference,
                                                                                                                ReferenceType = it.ReferenceType,
                                                                                                                Uom = qtfpr.Uom,
                                                                                                                UomLabel = uom.Label,
                                                                                                                ThresholdOperator = qtfpr.ThresholdOperator,
                                                                                                                ThresholdValue = qtfpr.ThresholdValue,
                                                                                                                ThresholdValueTo = qtfpr.ThresholdValueTo,
                                                                                                                ThresholdValueFrom = qtfpr.ThresholdValueFrom,
                                                                                                                NeedAttachment = qtfpr.NeedAttachment,
                                                                                                                Note = qtfpr.Note,
                                                                                                                FormProcedureId = qtfpr.FormProcedureId,
                                                                                                                IsForAllSample = qtfpr.IsForAllSample,
                                                                                                                IsResult = qtfpr.IsResult,
                                                                                                                DefaultValue = qtfpr.DefaultValue,
                                                                                                                CreatedAt = qtfpr.CreatedAt,
                                                                                                                CreatedBy = qtfpr.CreatedBy,
                                                                                                                GroupValues = (from gv in _context.QcTransactionGroupValues
                                                                                                                               where gv.QcTransactionGroupFormParameterId == qtfpr.Id
                                                                                                                               && gv.RowStatus == null
                                                                                                                               select new GroupValue
                                                                                                                               {
                                                                                                                                   Id = gv.Id,
                                                                                                                                   Sequence = gv.Sequence,
                                                                                                                                   Value = gv.Value,
                                                                                                                                   AttchmentFile = gv.AttchmentFile,
                                                                                                                                   QcTransactionGroupFormMaterialId = gv.QcTransactionGroupFormMaterialId,
                                                                                                                                   QcTransactionGroupFormToolId = gv.QcTransactionGroupFormToolId
                                                                                                                               }).OrderBy(x => x.Sequence).ToList(),
                                                                                                                GroupSampleValues = (from gsv in _context.QcTransactionSampleValues
                                                                                                                                     join gs in _context.QcTransactionSamples on gsv.QcTransactionSampleId equals gs.Id
                                                                                                                                     join smpl in _context.QcSamples on gs.QcSampleId equals smpl.Id
                                                                                                                                     where gsv.QcTransactionGroupFormParameterId == qtfpr.Id
                                                                                                                                     && gsv.RowStatus == null
                                                                                                                                     select new GroupSampleValue
                                                                                                                                     {
                                                                                                                                         Id = gsv.Id,
                                                                                                                                         QcTransactionSampleId = gsv.QcTransactionSampleId,
                                                                                                                                         SampleCode = smpl.Code,
                                                                                                                                         Sequence = gsv.Sequence,
                                                                                                                                         Value = gsv.Value,
                                                                                                                                         AttchmentFile = gsv.AttchmentFile,
                                                                                                                                         QcTransactionGroupFormMaterialId = gsv.QcTransactionGroupFromMaterialId,
                                                                                                                                         QcTransactionGroupFormToolId = gsv.QcTransactionGroupFromToolId
                                                                                                                                     }).OrderBy(x => x.Sequence).ToList(),
                                                                                                            }).OrderBy(x => x.Sequence).ToList()
                                                                     }).OrderBy(x => x.Sequence).ToList(),

                                }).OrderBy(x => x.Sequence).ToList();


                    }


                }

                /* Get QC Result Data List */
                result.QcResult = await (from ts in _context.QcTransactionSamples
                                         join sm in _context.QcSamples on ts.QcSampleId equals sm.Id
                                         join s in _context.QcSamplings on sm.QcSamplingId equals s.Id
                                         join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                         join ss in _context.QcSamplingShipments on s.Id equals ss.QcSamplingId
                                         where ts.QcTransactionGroupId == result.Id
                                         group new { s, r, ss } by new { r.Id } into g
                                         select new QcResultTestDetailViewModel
                                         {
                                             RequestId = g.Max(x => x.r.Id),
                                             NoBatch = g.Max(x => x.r.NoBatch),
                                             EmPhaseId = g.Max(x => x.r.EmPhaseId),
                                             EmPhaseName = g.Max(x => x.r.EmPhaseName),
                                             EmRoomId = g.Max(x => x.r.EmRoomId),
                                             EmRoomName = g.Max(x => x.r.EmRoomName),
                                             SamplingDateFrom = g.Max(x => x.s.SamplingDateFrom),
                                             SamplingDateTo = g.Max(x => x.s.SamplingDateTo),
                                             ShipmentStartDate = g.Max(x => x.ss.StartDate),
                                             ShipmentEndDate = g.Max(x => x.ss.EndDate)
                                         }).ToListAsync();

                if (result.QcResult.Any())
                {
                    foreach (var qRes in result.QcResult)
                    {
                        qRes.SamplingResult = (from ts1 in _context.QcTransactionSamples
                                               join sm1 in _context.QcSamples on ts1.QcSampleId equals sm1.Id
                                               join s1 in _context.QcSamplings on sm1.QcSamplingId equals s1.Id
                                               join res in _context.QcResults on sm1.Id equals res.SampleId
                                               join tsc in _context.TransactionTestScenario on sm1.TestScenarioId equals tsc.Id
                                               where ts1.QcTransactionGroupId == result.Id
                                               && s1.RequestQcsId == qRes.RequestId
                                               select new SamplingResult
                                               {
                                                   SampleId = res.SampleId,
                                                   SamplingPointId = sm1.SamplingPointId,
                                                   SamplingPointCode = sm1.SamplingPointCode,
                                                   SampleCode = sm1.Code,
                                                   TestScenarioId = sm1.TestScenarioId,
                                                   TestScenarioName = tsc.Name,
                                                   TestParamId = sm1.TestParamId,
                                                   TestParamName = sm1.TestParamName,
                                                   GradeRoomId = sm1.GradeRoomId,
                                                   GradeRoomName = sm1.GradeRoomName,
                                                   QcResultValue = res.Value,
                                                   TestVariableConclusion = res.TestVariableConclusion,
                                                   TestVariableId = res.TestVariableId,
                                                   Note = res.Note,
                                                   AttchmentFile = res.AttchmentFile
                                               }).ToList();
                    }
                }

                int newTestParamIndex = 1;
                var existingSamplingPointCode = new List<string>();
                var existingTestParamIndex = new List<int>();
                int samplingPointCodeIndex = 0;
                int testParamIndex = 0;
                foreach (var sampData in result.SamplesData)
                {
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
                }
            }
            return result;
        }

        public async Task<List<SampleByQcProcessRelationViewModel>> ListSampleTest(Int32 QcProcessId, string search, Int32 TestParamId, Int32 RoomId, Int32 PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            /*if (ReceiptStartDate.HasValue)
            {
                DateTime temp;
                Console.WriteLine(DateTime.TryParse(ReceiptStartDate.Value, out temp));
            }*/

            var AvailTestParam = await (from ts in _context.TransactionTestParameter
                                        where ts.QcProcessId == QcProcessId
                                        select ts.Id).ToListAsync();

            var result = await (from s in _context.QcSamples
                                join sp in _context.QcSamplings on s.QcSamplingId equals sp.Id
                                join r in _context.RequestQcs on sp.RequestQcsId equals r.Id
                                where ((EF.Functions.Like(s.Code.ToLower(), "%" + filter + "%")) ||
                                (EF.Functions.Like(r.NoBatch.ToLower(), "%" + filter + "%")))
                                && AvailTestParam.Contains(s.TestParamId)
                                && s.RowStatus == null
                                && sp.SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_EMM
                                && (sp.Status == ApplicationConstant.STATUS_APPROVED || sp.Status == ApplicationConstant.STATUS_IN_REVIEW_KABAG) //fix show sampling, todo : double confirm
                                select new SampleByQcProcessRelationViewModel
                                {
                                    Id = s.Id,
                                    BacthQrCode = sp.Code,
                                    QrCode = s.Code,
                                    SamplingPointId = s.SamplingPointId,
                                    SamplingPointCode = s.SamplingPointCode,
                                    NoBatch = r.NoBatch,
                                    PhaseId = r.EmPhaseId,
                                    PhaseName = r.EmPhaseName,
                                    TestParamId = s.TestParamId,
                                    TestParamName = s.TestParamName,
                                    TestParamShortName = (from tp in _context.TransactionTestParameter
                                                          where tp.Id == s.TestParamId
                                                          select tp.ShortName).FirstOrDefault(),
                                    RoomId = r.EmRoomId,
                                    RoomName = r.EmRoomName,
                                    SamplingDateTimeFrom = s.SamplingDateTimeFrom,
                                    SamplingDateTimeTo = s.SamplingDateTimeTo,
                                    ShipmentStatus = (from shp1 in _context.QcSamplingShipments
                                                      where shp1.QrCode == sp.Code
                                                      select shp1.Status).FirstOrDefault(),
                                    ShipmentStartDate = (from shp2 in _context.QcSamplingShipments
                                                         where shp2.QcSamplingId == sp.Id
                                                         select shp2.StartDate).FirstOrDefault(),
                                    ShipmentEndDate = (from shp3 in _context.QcSamplingShipments
                                                       where shp3.QcSamplingId == sp.Id
                                                       select shp3.EndDate).FirstOrDefault(),
                                    SampleTestTransaction = (from qts in _context.QcTransactionSamples
                                                             where qts.QcSampleId == s.Id
                                                             select qts).Count(),
                                    CreatedAt = s.CreatedAt,
                                    CreatedBy = s.CreatedBy
                                }).Where(x =>
                                    ((x.ShipmentStatus == ApplicationConstant.STATUS_SHIPMENT_RECEIVED) || (x.ShipmentStatus == ApplicationConstant.STATUS_SHIPMENT_LATE_RECIVED)) &&
                                    ((x.ShipmentEndDate >= ReceiptStartDate || !ReceiptStartDate.HasValue) &&
                                     (x.ShipmentEndDate <= ReceiptEndDate || !ReceiptEndDate.HasValue)) &&
                                    (x.SampleTestTransaction == 0) &&
                                    (x.TestParamId == TestParamId || TestParamId == 0) &&
                                    (x.RoomId == RoomId || RoomId == 0) &&
                                    (x.PhaseId == PhaseId || PhaseId == 0)
                                ).OrderByDescending(x => x.CreatedAt).ToListAsync();


            return result;
        }

        public async Task<List<SampleBatchQcProcessViewModel>> ListSampleBatchTest(int QcProcessId, string search, int RoomId, int PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from sp in _context.QcSamplings
                                join r in _context.RequestQcs on sp.RequestQcsId equals r.Id
                                where ((EF.Functions.Like(sp.Code.ToLower(), "%" + filter + "%")) ||
                                (EF.Functions.Like(r.NoBatch.ToLower(), "%" + filter + "%")))
                                && sp.RowStatus == null
                                && sp.SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_EMM
                                && (sp.Status == ApplicationConstant.STATUS_APPROVED || sp.Status == ApplicationConstant.STATUS_IN_REVIEW_KABAG)
                                select new SampleBatchQcProcessViewModel
                                {
                                    Id = sp.Id,
                                    BacthQrCode = sp.Code,
                                    NoBatch = r.NoBatch,
                                    PhaseId = r.EmPhaseId,
                                    PhaseName = r.EmPhaseName,
                                    RoomId = r.EmRoomId,
                                    RoomName = r.EmRoomName,
                                    ShipmentStatus = (from shp1 in _context.QcSamplingShipments
                                                      where shp1.QrCode == sp.Code
                                                      select shp1.Status).FirstOrDefault(),
                                    ShipmentStartDate = (from shp2 in _context.QcSamplingShipments
                                                         where shp2.QcSamplingId == sp.Id
                                                         select shp2.StartDate).FirstOrDefault(),
                                    ShipmentEndDate = (from shp3 in _context.QcSamplingShipments
                                                       where shp3.QcSamplingId == sp.Id
                                                       select shp3.EndDate).FirstOrDefault(),
                                    SamplingTestTransaction = (from qts in _context.QcTransactionSamplings
                                                               where qts.QcSamplingId == sp.Id
                                                               select qts).Count(),
                                    CreatedAt = sp.CreatedAt,
                                    CreatedBy = sp.CreatedBy,
                                    SampleListCount = (from s in _context.QcSamples
                                                       join tp in _context.TransactionTestParameter on s.TestParamId equals tp.Id
                                                       where s.QcSamplingId == sp.Id
                                                       && s.RowStatus == null
                                                       && tp.QcProcessId == QcProcessId
                                                       select new SampleListTestViewModel
                                                       {
                                                           Id = s.Id,
                                                           QrCode = s.Code,
                                                           TestParamId = s.TestParamId,
                                                           TestParamName = s.TestParamName,
                                                           TestParamShortName = tp.ShortName,
                                                           SamplingPointId = s.SamplingPointId,
                                                           SamplingPointCode = s.SamplingPointCode,
                                                           SamplingDateTimeFrom = s.SamplingDateTimeFrom,
                                                           SamplingDateTimeTo = s.SamplingDateTimeTo,
                                                           SampleTestTransaction = (from qts in _context.QcTransactionSamples
                                                                                    where qts.QcSampleId == s.Id
                                                                                    select qts).Count()
                                                       }).Where(w => w.SampleTestTransaction == 0).Count()

                                }).Where(x =>
                                    ((x.ShipmentStatus == ApplicationConstant.STATUS_SHIPMENT_RECEIVED) || (x.ShipmentStatus == ApplicationConstant.STATUS_SHIPMENT_LATE_RECIVED)) &&
                                    ((x.ShipmentEndDate >= ReceiptStartDate || !ReceiptStartDate.HasValue) &&
                                     (x.ShipmentEndDate <= ReceiptEndDate || !ReceiptEndDate.HasValue)) &&
                                    (x.SamplingTestTransaction == 0) &&
                                    (x.SampleListCount > 0) &&
                                    (x.RoomId == RoomId || RoomId == 0) &&
                                    (x.PhaseId == PhaseId || PhaseId == 0)
                                ).OrderByDescending(x => x.CreatedAt).ToListAsync();

            return result;
        }

        public async Task<QcTransactionGroup> Insert(QcTransactionGroup data, List<QcTransactionGroupSample> sampleTest, List<QcTransactionGroupSampling> samplingTest)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                #region generate sequence number untuk QC Test

                //TODO perlu diperbaiki terkait penomoran QC Test
                // var longDataQcTest = await (from t in _context.QcTransactionGroups select t.Id).CountAsync() + 1;
                var longDataQcTest = 1;
                var lastQcTransactionGroup = await _context.QcTransactionGroups.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

                if (lastQcTransactionGroup != null)
                {
                    longDataQcTest = lastQcTransactionGroup.Id + 1;
                }
                data.Code = $"{longDataQcTest}{data.Code}";

                #endregion

                await _context.QcTransactionGroups.AddAsync(data);
                await _context.SaveChangesAsync();

                var getQcProcess = await _dataProviderQcProcess.GetQcProcessById(data.QcProcessId);

                //save per batch
                if (getQcProcess.AddSampleLayoutType == ApplicationConstant.ADD_SAMPLE_TEST)
                {
                    //insert transaction sample
                    if (sampleTest.Any())
                    {
                        sampleTest.ForEach(x => x.QcTransactionGroupId = data.Id);
                        await _context.QcTransactionSamples.AddRangeAsync(sampleTest);
                    }
                }
                else if (getQcProcess.AddSampleLayoutType == ApplicationConstant.ADD_SAMPLE_BY_BATCH_TEST)
                {
                    if (samplingTest.Any())
                    {
                        samplingTest.ForEach(x => x.QcTransactionGroupId = data.Id);
                        await _context.QcTransactionSamplings.AddRangeAsync(samplingTest);

                        await _context.SaveChangesAsync();

                        if (sampleTest.Any())
                        {
                            foreach (var st in samplingTest)
                            {
                                foreach (var s in sampleTest.Where(s => st.QcSamplingId == s.QcSamplingId))
                                {
                                    s.QcTransactionGroupId = data.Id;
                                    s.QcTransactionSamplingId = st.Id;
                                }
                            }

                            await _context.QcTransactionSamples.AddRangeAsync(sampleTest);
                        }
                    }
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                stopwatch.Stop();
                _logger.LogInformation("Insert sampling & sample. Elapsed Time is {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

                #region generate qc transaction process test

                if (data.Status == ApplicationConstant.STATUS_TEST_READYTOTEST)
                {
                    //test comment qcTest
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await _generateQcProcessDataProvider.Generate(data);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "{Message}", ex.Message);
                        }

                    });
                }
                #endregion

                return data;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "{Message}", ex.Message);
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<QcTransactionGroup> Edit(QcTransactionGroup data, List<EditQcProcessSample> sampleTest, List<EditQcProcessSamplingBatch> samplingTest)
        {
            QcTransactionGroup result = new QcTransactionGroup();
            using (var transaction = _context.Database.BeginTransaction())

            {
                try
                {
                    var getQcProcess = await _dataProviderQcProcess.GetQcProcessById(data.QcProcessId);

                    if (getQcProcess.AddSampleLayoutType == ApplicationConstant.ADD_SAMPLE_TEST)
                    {
                        List<int> sampleTestIds = sampleTest.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();

                        var deleteSampleTestData = await (from st in _context.QcTransactionSamples
                                                          where !sampleTestIds.Contains(st.Id)
                                                          && st.QcTransactionGroupId == data.Id
                                                          select st).ToListAsync();

                        _context.QcTransactionSamples.RemoveRange(deleteSampleTestData);

                        var sampleTestData = await (from st in _context.QcTransactionSamples
                                                    where sampleTestIds.Contains(st.Id)
                                                    && st.QcTransactionGroupId == data.Id
                                                    select st).ToListAsync();

                        foreach (var edSamp in sampleTestData)
                        {
                            var editData = sampleTest.Where(x => x.Id.Value == edSamp.Id).FirstOrDefault();
                            if (editData != null)
                            {
                                edSamp.QcSampleId = editData.QcSampleId;
                                edSamp.UpdatedBy = data.UpdatedBy;
                                edSamp.UpdatedAt = DateHelper.Now();
                            }
                        }

                        var addSampleTestData = sampleTest.Where(x => !x.Id.HasValue).ToList();
                        foreach (var addSamp in addSampleTestData)
                        {
                            QcTransactionGroupSample newSampleTest = new QcTransactionGroupSample();

                            newSampleTest.QcTransactionGroupId = data.Id;
                            newSampleTest.QcSampleId = addSamp.QcSampleId;
                            newSampleTest.CreatedBy = data.UpdatedBy;
                            newSampleTest.UpdatedBy = data.UpdatedBy;
                            newSampleTest.CreatedAt = DateHelper.Now();
                            newSampleTest.UpdatedAt = DateHelper.Now();

                            _context.QcTransactionSamples.Add(newSampleTest);
                        }
                    }
                    else if (getQcProcess.AddSampleLayoutType == ApplicationConstant.ADD_SAMPLE_BY_BATCH_TEST)
                    {
                        List<int> samplingTestIds = samplingTest.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();

                        var deleteSamplingTestData = await (from st in _context.QcTransactionSamplings
                                                            where !samplingTestIds.Contains(st.Id)
                                                            && st.QcTransactionGroupId == data.Id
                                                            select st).ToListAsync();

                        if (deleteSamplingTestData.Any())
                        {
                            foreach (var delSamp in deleteSamplingTestData)
                            {
                                var getSampleDeleteData = await (from sd in _context.QcTransactionSamples
                                                                 where sd.QcTransactionSamplingId == delSamp.Id
                                                                 select sd).ToListAsync();

                                _context.QcTransactionSamples.RemoveRange(getSampleDeleteData);

                            }
                        }

                        _context.QcTransactionSamplings.RemoveRange(deleteSamplingTestData);

                        var samplingTestData = await (from st in _context.QcTransactionSamplings
                                                      where samplingTestIds.Contains(st.Id)
                                                      && st.QcTransactionGroupId == data.Id
                                                      select st).ToListAsync();

                        // update sampling
                        if (samplingTestData.Any())
                        {
                            foreach (var sp in samplingTestData)
                            {

                                var editDataSampling = samplingTest.Where(x => x.Id.Value == sp.Id).FirstOrDefault();
                                if (editDataSampling != null)
                                {

                                    List<QcTransactionGroupSample> updateTransactionSample = new List<QcTransactionGroupSample>();
                                    var getSampleData = (from s in _context.QcSamples
                                                         join tp in _context.TransactionTestParameter on s.TestParamId equals tp.Id
                                                         where s.RowStatus == null
                                                         && tp.QcProcessId == data.QcProcessId
                                                         && s.QcSamplingId == editDataSampling.QcSamplingId
                                                         select s).ToList();

                                    if (getSampleData.Any())
                                    {
                                        var getSampleDeleteData = await (from sd in _context.QcTransactionSamples
                                                                         where sd.QcTransactionSamplingId == sp.Id
                                                                         select sd).ToListAsync();

                                        _context.QcTransactionSamples.RemoveRange(getSampleDeleteData);

                                        sp.QcSamplingId = editDataSampling.QcSamplingId;
                                        sp.UpdatedBy = data.UpdatedBy;
                                        sp.UpdatedAt = DateHelper.Now();

                                        foreach (var sd in getSampleData)
                                        {
                                            QcTransactionGroupSample dataSampleTest = new QcTransactionGroupSample()
                                            {
                                                QcTransactionSamplingId = sp.Id,
                                                QcSamplingId = editDataSampling.QcSamplingId,
                                                QcTransactionGroupId = data.Id,
                                                QcSampleId = sd.Id,
                                                CreatedBy = data.CreatedBy,
                                                UpdatedBy = data.CreatedBy,
                                                CreatedAt = DateHelper.Now(),
                                                UpdatedAt = DateHelper.Now()
                                            };
                                            updateTransactionSample.Add(dataSampleTest);
                                        }

                                        await _context.QcTransactionSamples.AddRangeAsync(updateTransactionSample);
                                    }

                                }
                            }
                        }

                        //add sampling new
                        var addSamplingTestData = samplingTest.Where(x => !x.Id.HasValue).ToList();
                        if (addSamplingTestData.Any())
                        {
                            foreach (var addSamp in addSamplingTestData)
                            {

                                List<QcTransactionGroupSample> insertTransactionSample = new List<QcTransactionGroupSample>();
                                var getSampleData = (from s in _context.QcSamples
                                                     join tp in _context.TransactionTestParameter on s.TestParamId equals tp.Id
                                                     where s.RowStatus == null
                                                     && tp.QcProcessId == data.QcProcessId
                                                     && s.QcSamplingId == addSamp.QcSamplingId
                                                     select s).ToList();

                                if (getSampleData.Any())
                                {

                                    QcTransactionGroupSampling newSamplingTest = new QcTransactionGroupSampling();

                                    newSamplingTest.QcTransactionGroupId = data.Id;
                                    newSamplingTest.QcSamplingId = addSamp.QcSamplingId;
                                    newSamplingTest.CreatedBy = data.UpdatedBy;
                                    newSamplingTest.UpdatedBy = data.UpdatedBy;
                                    newSamplingTest.CreatedAt = DateHelper.Now();
                                    newSamplingTest.UpdatedAt = DateHelper.Now();

                                    _context.QcTransactionSamplings.Add(newSamplingTest);

                                    await _context.SaveChangesAsync();


                                    foreach (var sd in getSampleData)
                                    {
                                        QcTransactionGroupSample dataSampleTest = new QcTransactionGroupSample()
                                        {
                                            QcTransactionSamplingId = newSamplingTest.Id,
                                            QcSamplingId = addSamp.QcSamplingId,
                                            QcSampleId = sd.Id,
                                            QcTransactionGroupId = data.Id,
                                            CreatedBy = data.CreatedBy,
                                            UpdatedBy = data.CreatedBy,
                                            CreatedAt = DateHelper.Now(),
                                            UpdatedAt = DateHelper.Now()
                                        };
                                        insertTransactionSample.Add(dataSampleTest);
                                    }

                                    await _context.QcTransactionSamples.AddRangeAsync(insertTransactionSample);
                                }
                            }
                        }

                    }

                    await _context.SaveChangesAsync();

                    var sampleDataTest = await (from st in _context.QcTransactionSamples
                                                where st.QcTransactionGroupId == data.Id
                                                select st).ToListAsync();

                    /* Generate Qc Transaction Process Test */
                    if (data.Status == ApplicationConstant.STATUS_TEST_READYTOTEST)
                    {
                        await GenerateQcTestProcessAlt(data, sampleDataTest);
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

        public async Task<QcTransactionGroup> UpdateTest(StartQcTestBindingModel data)
        {
            var result = await GetByIdRaw(data.QcTestId);
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                //Validation update data by status
                IList<int> actionStatusUpdate = new List<int>
                {
                    ApplicationConstant.STATUS_TEST_DRAFT,
                    ApplicationConstant.STATUS_TEST_REJECTED,
                    ApplicationConstant.STATUS_TEST_READYTOTEST,
                    ApplicationConstant.STATUS_TEST_INPROGRESS,
                    ApplicationConstant.STATUS_TEST_INREVIEW_PAIRING,
                };

                //Validation if pairing personel changed on correction
                if (actionStatusUpdate.Contains(result.Status))
                {
                    result.PersonelPairingNik = data.PersonelPairingNik;
                    result.PersonelPairingName = data.PersonelPairingName;
                }

                //jika bukan save as draft ketika setelah riject pada bagian testing
                if (data.UpdateStatus)
                {
                    // status manage case pairing update data
                    if (result.Status == ApplicationConstant.STATUS_TEST_INREVIEW_PAIRING)
                    {
                        // case update with pairing
                        result.Status = ApplicationConstant.STATUS_TEST_INREVIEW_PAIRING;
                    }
                    else
                    {
                        // case update with operator
                        result.Status = ApplicationConstant.STATUS_TEST_INPROGRESS;
                        result.PersonelNik = data.PersonelNik;
                        result.PersonelName = data.PersonelName;

                        // status dinamis
                        if (data.IsSubmit == true)
                        {
                            result.Status = ApplicationConstant.STATUS_TEST_INREVIEW_PAIRING;
                        }
                    }
                }


                result.UpdatedBy = data.UpdatedBy;
                result.UpdatedAt = DateHelper.Now();

                await _context.SaveChangesAsync();

                /*init data process observasi value*/
                List<QcTransactionGroupSampleValue> dataSampleObservasiGen = new List<QcTransactionGroupSampleValue>();

                /*init data process uji identifikasi value*/
                List<QcTransactionGroupSampleValue> dataSampleUjiIdentifikasiGen = new List<QcTransactionGroupSampleValue>();

                foreach (var prc in data.qcTransactionGroupProcessChild)
                {
                    /*check name process*/
                    var getProcessData = await (from txtp in _context.QcTransactionGroupProcesses
                                                where txtp.Id == prc.Id
                                                select txtp).FirstOrDefaultAsync();

                    result.StatusProses = getProcessData.QcProcessId;
                    await _context.SaveChangesAsync();

                    #region material test qc

                    //disable material update
                    /*form material update*/
                    /*
                        List<Int32> materialDataIds = prc.qcTransactionGroupFormMaterial.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();


                        var deleteMaterialdata = await (from m in _context.QcTransactionGroupFormMaterials
                                                        where !materialDataIds.Contains(m.Id)
                                                        && m.QcTransactionGroupProcessId == prc.Id
                                                        select m).ToListAsync();

                        /*_context.QcTransactionGroupFormMaterials.RemoveRange(deleteMaterialdata);*/
                    /*
                        var MaterialData = await (from m in _context.QcTransactionGroupFormMaterials
                                                  where materialDataIds.Contains(m.Id)
                                                  && m.QcTransactionGroupProcessId == prc.Id
                                                  select m).ToListAsync();

                        foreach (var md in MaterialData)
                        {
                            var editDataMaterial = prc.qcTransactionGroupFormMaterial.Where(x => x.Id.Value == md.Id).FirstOrDefault();
                            if (editDataMaterial != null)
                            {
                                md.ItemId = editDataMaterial.ItemId;
                                md.Code = editDataMaterial.ItemCode;
                                md.Name = editDataMaterial.ItemName;
                                md.DefaultPackageQty = editDataMaterial.DefaultPackageQty;
                                md.DefaultQty = editDataMaterial.DefaultQty;
                                md.UpdatedBy = data.UpdatedBy;
                                md.UpdatedAt = DateHelper.Now();
                            }
                        }
                        */

                    //await _context.SaveChangesAsync();

                    #endregion

                    #region update transaction group form tool

                    var toolDataIds = prc.qcTransactionGroupFormTool.Where(x => x.Id.HasValue).Select(x => x.Id.Value)
                        .ToList();

                    var toolDataList = await (from t in _context.QcTransactionGroupFormTools
                                              join ts in _context.QcTransactionGroupFormSections on t.QcTransactionGroupSectionId equals ts.Id
                                              where toolDataIds.Contains(t.Id)
                                                    && ts.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_TOOL
                                                    && t.QcTransactionGroupProcessId == prc.Id
                                              select t).ToListAsync();

                    foreach (var td in toolDataList)
                    {
                        var editDataTool =
                            prc.qcTransactionGroupFormTool.FirstOrDefault(x => x.Id.HasValue && x.Id.Value == td.Id);
                        if (editDataTool == null)
                        {
                            continue;
                        }

                        td.ToolId = editDataTool.ToolId;
                        td.Code = editDataTool.ToolCode;
                        td.Name = editDataTool.ToolName;
                        td.Quantity = editDataTool.Quantity;
                        td.ItemId = editDataTool.ItemId;
                        td.UpdatedBy = data.UpdatedBy;
                        td.UpdatedAt = DateHelper.Now();
                    }

                    await _context.SaveChangesAsync();

                    #endregion

                    #region add or update transaction group form procedure

                    if (prc.qcTransactionGroupFormProcedure.Any())
                    {
                        var formGroupProcedureIds = prc.qcTransactionGroupFormProcedure.Select(x => x.Id).ToList();

                        var formGroupProcedureList = await (from qfp in _context.QcTransactionGroupFormProcedures
                                                            join ts in _context.QcTransactionGroupFormSections on qfp
                                                                .QcTransactionGroupSectionId equals ts.Id
                                                            where formGroupProcedureIds.Contains(qfp.Id)
                                                                  && ts.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_PROCEDURE
                                                                  && qfp.RowStatus == null
                                                            select qfp).ToListAsync();

                        foreach (var formProcedure in formGroupProcedureList)
                        {
                            var fprc = prc.qcTransactionGroupFormProcedure.First(x => x.Id == formProcedure.Id);

                            foreach (var fparam in fprc.qcTransactionGroupFormParameter)
                            {
                                #region transaction group values

                                if (fparam.GroupValues.Any())
                                {
                                    /*group value update*/
                                    var valueDataIds = fparam.GroupValues.Where(x => x.Id.HasValue)
                                        .Select(x => x.Id.Value).ToList();

                                    var valueData = await (from gv in _context.QcTransactionGroupValues
                                                           where valueDataIds.Contains(gv.Id)
                                                                 && gv.QcTransactionGroupFormParameterId == fparam.Id
                                                           select gv)
                                        .OrderBy(x => x.Sequence)
                                        .ToListAsync();

                                    var seqValueData = valueData.LastOrDefault()?.Sequence ?? 0;

                                    foreach (var vd in valueData)
                                    {
                                        var editDataValue =
                                            fparam.GroupValues.FirstOrDefault(x =>
                                                x.Id.HasValue && x.Id.Value == vd.Id);
                                        if (editDataValue == null)
                                        {
                                            continue;
                                        }

                                        vd.Value = editDataValue.Value;
                                        vd.AttchmentFile = editDataValue.AttchmentFile;
                                        vd.QcTransactionGroupFormMaterialId =
                                            editDataValue.QcTransactionGroupFormMaterialId;
                                        vd.QcTransactionGroupFormToolId = editDataValue.QcTransactionGroupFormToolId;
                                        vd.UpdatedBy = data.UpdatedBy;
                                        vd.UpdatedAt = DateHelper.Now();
                                    }

                                    var newGroupValues = new List<QcTransactionGroupValue>();

                                    var addValueData = fparam.GroupValues.Where(x => !x.Id.HasValue).ToList();
                                    foreach (var addValue in addValueData)
                                    {
                                        seqValueData++;
                                        var newValue = new QcTransactionGroupValue();
                                        newValue.QcTransactionGroupFormParameterId = fparam.Id;
                                        newValue.Sequence = seqValueData;
                                        newValue.Value = addValue.Value;
                                        newValue.AttchmentFile = addValue.AttchmentFile;
                                        newValue.QcTransactionGroupFormMaterialId =
                                            addValue.QcTransactionGroupFormMaterialId;
                                        newValue.QcTransactionGroupFormToolId = addValue.QcTransactionGroupFormToolId;
                                        newValue.CreatedBy = data.UpdatedBy;
                                        newValue.UpdatedBy = data.UpdatedBy;
                                        newValue.CreatedAt = DateHelper.Now();
                                        newValue.UpdatedAt = DateHelper.Now();

                                        newGroupValues.Add(newValue);
                                    }

                                    await _context.QcTransactionGroupValues.AddRangeAsync(newGroupValues);
                                    await _context.SaveChangesAsync();
                                }

                                #endregion

                                #region transaction group sample value

                                var sampleValueDataIds = fparam.GroupSampleValues.Where(x => x.Id.HasValue)
                                    .Select(x => x.Id.Value).ToList();

                                var deleteValueSampledata = await (from sv in _context.QcTransactionSampleValues
                                                                   where !sampleValueDataIds.Contains(sv.Id)
                                                                         && sv.QcTransactionGroupFormParameterId == fparam.Id
                                                                   select sv).ToListAsync();

                                _context.QcTransactionSampleValues.RemoveRange(deleteValueSampledata);

                                var valueSampleData = await (from sv in _context.QcTransactionSampleValues
                                                             where sampleValueDataIds.Contains(sv.Id)
                                                                   && sv.QcTransactionGroupFormParameterId == fparam.Id
                                                             select sv)
                                    .OrderBy(x => x.Sequence)
                                    .ToListAsync();

                                var seqValueSampleData = valueSampleData.LastOrDefault()?.Sequence ?? 0;

                                var dataSampleObservasi = new List<QcTransactionGroupSampleValue>();

                                foreach (var vs in valueSampleData)
                                {
                                    var editDataValueSample =
                                        fparam.GroupSampleValues.FirstOrDefault(x =>
                                            x.Id.HasValue && x.Id.Value == vs.Id);
                                    if (editDataValueSample != null)
                                    {
                                        if (editDataValueSample.Value != null && editDataValueSample.Value.Trim() == "-")
                                        {
                                            editDataValueSample.Value = null;
                                        }

                                        vs.Value = editDataValueSample.Value;
                                        vs.AttchmentFile = editDataValueSample.AttchmentFile;
                                        vs.QcTransactionGroupFromMaterialId =
                                            editDataValueSample.QcTransactionGroupFormMaterialId;
                                        vs.QcTransactionGroupFromToolId =
                                            editDataValueSample.QcTransactionGroupFormToolId;
                                        vs.UpdatedBy = data.UpdatedBy;
                                        vs.UpdatedAt = DateHelper.Now();
                                    }

                                    if (getProcessData.QcProcessId == ApplicationConstant.PROCESS_OBSERVASI)
                                    {
                                        if (vs.Value != null && vs.Value.Trim() == "-")
                                        {
                                            vs.Value = null;
                                        }

                                        var samplingData = new QcTransactionGroupSampleValue()
                                        {
                                            Id = vs.Id,
                                            QcTransactionGroupFormParameterId = vs.QcTransactionGroupFormParameterId,
                                            QcTransactionSampleId = vs.QcTransactionSampleId,
                                            Value = vs.Value,
                                            QcTransactionGroupFromMaterialId = vs.QcTransactionGroupFromMaterialId,
                                            QcTransactionGroupFromToolId = vs.QcTransactionGroupFromToolId,
                                            AttchmentFile = vs.AttchmentFile,
                                            RowStatus = vs.RowStatus,
                                            Sequence = vs.Sequence,
                                            CreatedAt = vs.CreatedAt,
                                            CreatedBy = vs.CreatedBy,
                                            UpdatedAt = vs.UpdatedAt,
                                            UpdatedBy = vs.UpdatedBy
                                        };
                                        dataSampleObservasi.Add(samplingData);
                                    }
                                }

                                var newSampleTestValues = new List<QcTransactionGroupSampleValue>();

                                //add jika tidak mencantumkan ID group sample value
                                var addSampleValueData = fparam.GroupSampleValues.Where(x => !x.Id.HasValue).ToList();
                                foreach (var addSampleValue in addSampleValueData)
                                {
                                    if (addSampleValue.Value != null && addSampleValue.Value.Trim() == "-")
                                    {
                                        addSampleValue.Value = null;
                                    }

                                    seqValueSampleData++;
                                    var newSampleValue = new QcTransactionGroupSampleValue();
                                    newSampleValue.QcTransactionGroupFormParameterId = fparam.Id;
                                    newSampleValue.QcTransactionSampleId = addSampleValue.qcTransactionSampleId;
                                    newSampleValue.Sequence = seqValueSampleData;
                                    newSampleValue.Value = addSampleValue.Value;
                                    newSampleValue.AttchmentFile = addSampleValue.AttchmentFile;
                                    newSampleValue.QcTransactionGroupFromMaterialId =
                                        addSampleValue.QcTransactionGroupFormMaterialId;
                                    newSampleValue.QcTransactionGroupFromToolId =
                                        addSampleValue.QcTransactionGroupFormToolId;
                                    newSampleValue.CreatedBy = data.UpdatedBy;
                                    newSampleValue.UpdatedBy = data.UpdatedBy;
                                    newSampleValue.CreatedAt = DateHelper.Now();
                                    newSampleValue.UpdatedAt = DateHelper.Now();

                                    newSampleTestValues.Add(newSampleValue);
                                }

                                await _context.QcTransactionSampleValues.AddRangeAsync(newSampleTestValues);
                                await _context.SaveChangesAsync();

                                #endregion

                                //get lagi data value sample nya
                                var valueSampleDataSet = await (from sv1 in _context.QcTransactionSampleValues
                                                                where sv1.QcTransactionGroupFormParameterId == fparam.Id
                                                                select sv1).ToListAsync();

                                if (getProcessData != null)
                                {
                                    if (getProcessData.QcProcessId == ApplicationConstant.PROCESS_OBSERVASI)
                                    {
                                        dataSampleObservasiGen = valueSampleDataSet;
                                    }
                                    else if (getProcessData.QcProcessId ==
                                             ApplicationConstant.PROCESS_UJI_IDENTIFIKASI)
                                    {
                                        dataSampleUjiIdentifikasiGen = valueSampleDataSet;
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region add or update transaction group form general

                    if (prc.qcTransactionGroupFormGeneral.Any())
                    {
                        var groupFormGeneralIds = prc.qcTransactionGroupFormGeneral.Select(x => x.Id).ToList();

                        var groupFormGeneralList = await (from qfp in _context.QcTransactionGroupFormProcedures
                                                          join ts in _context.QcTransactionGroupFormSections on qfp.QcTransactionGroupSectionId
                                                              equals ts.Id
                                                          where groupFormGeneralIds.Contains(qfp.Id)
                                                                && ts.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_NOTE
                                                                && qfp.RowStatus == null
                                                          select qfp).ToListAsync();

                        foreach (var formGeneral in groupFormGeneralList)
                        {
                            var tgf = prc.qcTransactionGroupFormGeneral.First(x => x.Id == formGeneral.Id);
                            foreach (var fparam in tgf.qcTransactionGroupFormParameter)
                            {
                                /*check list group value*/
                                if (!fparam.GroupValues.Any())
                                {
                                    continue;
                                }

                                /*group value update*/
                                var valueDataIds = fparam.GroupValues.Where(x => x.Id.HasValue)
                                    .Select(x => x.Id.Value).ToList();

                                var valueDataList = await (from gv in _context.QcTransactionGroupValues
                                                           where valueDataIds.Contains(gv.Id)
                                                                 && gv.QcTransactionGroupFormParameterId == fparam.Id
                                                           select gv)
                                    .OrderBy(x => x.Sequence)
                                    .ToListAsync();

                                var seqValueData = valueDataList.LastOrDefault()?.Sequence ?? 0;

                                foreach (var vd in valueDataList)
                                {
                                    var editDataValue =
                                        fparam.GroupValues.FirstOrDefault(x =>
                                            x.Id.HasValue && x.Id.Value == vd.Id);
                                    if (editDataValue == null)
                                    {
                                        continue;
                                    }

                                    vd.Value = editDataValue.Value;
                                    vd.AttchmentFile = editDataValue.AttchmentFile;
                                    vd.QcTransactionGroupFormMaterialId =
                                        editDataValue.QcTransactionGroupFormMaterialId;
                                    vd.QcTransactionGroupFormToolId = editDataValue.QcTransactionGroupFormToolId;
                                    vd.UpdatedBy = data.UpdatedBy;
                                    vd.UpdatedAt = DateHelper.Now();
                                }

                                var newGroupValues = new List<QcTransactionGroupValue>();

                                var addValueData = fparam.GroupValues.Where(x => !x.Id.HasValue).ToList();
                                foreach (var addValue in addValueData)
                                {
                                    seqValueData++;
                                    var newValue = new QcTransactionGroupValue();

                                    newValue.QcTransactionGroupFormParameterId = fparam.Id;
                                    newValue.Sequence = seqValueData;
                                    newValue.Value = addValue.Value;
                                    newValue.AttchmentFile = addValue.AttchmentFile;
                                    newValue.QcTransactionGroupFormMaterialId =
                                        addValue.QcTransactionGroupFormMaterialId;
                                    newValue.QcTransactionGroupFormToolId = addValue.QcTransactionGroupFormToolId;
                                    newValue.CreatedBy = data.UpdatedBy;
                                    newValue.UpdatedBy = data.UpdatedBy;
                                    newValue.CreatedAt = DateHelper.Now();
                                    newValue.UpdatedAt = DateHelper.Now();

                                    newGroupValues.Add(newValue);
                                }

                                await _context.QcTransactionGroupValues.AddRangeAsync(newGroupValues);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                    #endregion
                }

                stopwatch.Stop();
                _logger.LogInformation("UpdateTest. Elapsed Time is {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

                await transaction.CommitAsync();

                /* generate resut on submit */
                if (data.IsSubmit)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await _generateQcResultDataProvider.Generate(data.QcTestId, dataSampleObservasiGen, dataSampleUjiIdentifikasiGen);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "{Message}", e.Message);
                        }
                    });

                }

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "{Message}", ex.Message);
                await transaction.RollbackAsync();
                throw;
            }

            return result;
        }

        public async Task<QcTransactionGroup> GetByIdRaw(Int32 id)
        {
            var result = await (from t in _context.QcTransactionGroups
                                where t.Id == id
                                && t.RowStatus == null
                                select t).FirstOrDefaultAsync();

            return result;
        }
        public async Task<QcTransactionGroupProcess> GetGroupProcessByIdRaw(Int32 id)
        {
            var result = await (from p in _context.QcTransactionGroupProcesses
                                where p.Id == id
                                && p.RowStatus == null
                                select p).FirstOrDefaultAsync();

            return result;
        }
        public async Task<QcTransactionGroupProcess> GetGroupProcessByParentIdRaw(Int32 parentId)
        {
            var result = await (from p in _context.QcTransactionGroupProcesses
                                where p.ParentId == parentId
                                && p.RowStatus == null
                                select p).FirstOrDefaultAsync();

            return result;
        }
        public async Task<QcTransactionGroupFormMaterial> GetGroupFormMaterialByIdRaw(Int32 id)
        {
            var result = await (from m in _context.QcTransactionGroupFormMaterials
                                where m.Id == id
                                && m.RowStatus == null
                                select m).FirstOrDefaultAsync();

            return result;
        }
        public async Task<QcTransactionGroupFormTool> GetGroupFormToolByIdRaw(Int32 id)
        {
            var result = await (from tl in _context.QcTransactionGroupFormTools
                                where tl.Id == id
                                && tl.RowStatus == null
                                select tl).FirstOrDefaultAsync();

            return result;
        }
        public async Task<QcTransactionGroupFormProcedure> GetGroupFormProcedureByIdRaw(Int32 id)
        {
            var result = await (from prc in _context.QcTransactionGroupFormProcedures
                                where prc.Id == id
                                && prc.RowStatus == null
                                select prc).FirstOrDefaultAsync();

            return result;
        }
        public async Task<QcTransactionGroupFormParameter> GetGroupFormParameterByIdRaw(Int32 id)
        {
            var result = await (from pra in _context.QcTransactionGroupFormParameters
                                where pra.Id == id
                                && pra.RowStatus == null
                                select pra).FirstOrDefaultAsync();

            return result;
        }
        public async Task<List<QcTransactionGroupSample>> GetSampleTestByIdRaw(Int32 TestId)
        {
            var result = await (from s in _context.QcTransactionSamples
                                where s.QcTransactionGroupId == TestId
                                && s.RowStatus == null
                                select s).ToListAsync();

            return result;
        }
        public async Task<List<QcTransactionGroupValue>> GetGroupValueByParameterIdRaw(Int32 ParameterId)
        {
            var result = await (from gv in _context.QcTransactionGroupValues
                                where gv.QcTransactionGroupFormParameterId == ParameterId
                                && gv.RowStatus == null
                                select gv).ToListAsync();

            return result;
        }
        public async Task<List<QcTransactionGroupSampleValue>> GetGroupSampleValueByParameterIdRaw(Int32 ParameterId)
        {
            var result = await (from gsv in _context.QcTransactionSampleValues
                                where gsv.QcTransactionGroupFormParameterId == ParameterId
                                && gsv.RowStatus == null
                                select gsv).ToListAsync();

            return result;
        }

        public async Task<QcTransactionGroupProcess> GenerateQcTestProcessAlt(QcTransactionGroup data, List<QcTransactionGroupSample> sampleTest)
        {
            /* get Parent QcProcess */
            var getParentQcProcess = await _dataProviderQcProcess.GetQcProcessById(data.QcProcessId);
            QcTransactionGroupProcess insertParentTrxProcess = new QcTransactionGroupProcess()
            {
                QcTransactionGroupId = data.Id,
                Sequence = getParentQcProcess.Sequence,
                Name = getParentQcProcess.Name,
                ParentId = getParentQcProcess.ParentId,
                RoomId = getParentQcProcess.RoomId,
                IsInputForm = getParentQcProcess.IsInputForm,
                QcProcessId = data.QcProcessId,
                CreatedBy = data.CreatedBy,
                UpdatedBy = data.CreatedBy,
                CreatedAt = DateHelper.Now(),
                UpdatedAt = DateHelper.Now()
            };
            await _context.QcTransactionGroupProcesses.AddAsync(insertParentTrxProcess);
            await _context.SaveChangesAsync(); // save data

            /* get Child QcProcess */
            var getChildQcProcess = await _dataProviderQcProcess.GetQcProcessByParentId(data.QcProcessId);
            List<QcTransactionGroupProcess> insertChildTrxProcess = new List<QcTransactionGroupProcess>();
            List<Int32> insertChildTrxProcessIds = new List<Int32>();
            if (getChildQcProcess.Any())
            {
                foreach (var p in getChildQcProcess)
                {
                    QcTransactionGroupProcess dataChildTrxProcess = new QcTransactionGroupProcess()
                    {
                        QcTransactionGroupId = data.Id,
                        Sequence = p.Sequence,
                        Name = p.Name,
                        ParentId = insertParentTrxProcess.Id,
                        RoomId = p.RoomId,
                        IsInputForm = p.IsInputForm,
                        QcProcessId = p.Id,
                        CreatedBy = data.CreatedBy,
                        UpdatedBy = data.CreatedBy,
                        CreatedAt = DateHelper.Now(),
                        UpdatedAt = DateHelper.Now()
                    };
                    insertChildTrxProcess.Add(dataChildTrxProcess);
                    insertChildTrxProcessIds.Add(p.Id);
                }
            }
            if (insertChildTrxProcess.Any())
            {
                await _context.QcTransactionGroupProcesses.AddRangeAsync(insertChildTrxProcess);
                await _context.SaveChangesAsync(); // save data
            }

            /* get form section */
            var getFormSection = await _dataProviderQcProcess.GetFormSection();
            List<QcTransactionGroupFormSection> insertTrxFormSection = new List<QcTransactionGroupFormSection>();
            if (getFormSection.Any())
            {
                foreach (var fs in getFormSection)
                {
                    QcTransactionGroupFormSection dataTrxFormSection = new QcTransactionGroupFormSection()
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
                    };
                    insertTrxFormSection.Add(dataTrxFormSection);
                }

                if (insertTrxFormSection.Any())
                {
                    await _context.QcTransactionGroupFormSections.AddRangeAsync(insertTrxFormSection);
                    await _context.SaveChangesAsync(); // save data

                    if (insertChildTrxProcess.Any())
                    {

                        foreach (var pc in insertChildTrxProcess)
                        {

                            //loop section
                            foreach (var trxFormSection in insertTrxFormSection)
                            {
                                /*Section Material*/
                                if (trxFormSection.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_MATERIAL)
                                {
                                    #region transaction group material
                                    /* get form material process */
                                    var getFormMaterial = await _dataProviderQcProcess.GetFormMaterial(pc.QcProcessId);
                                    List<QcTransactionGroupFormMaterial> insertTrxFormMaterial = new List<QcTransactionGroupFormMaterial>();
                                    if (getFormMaterial.Any())
                                    {
                                        foreach (var fm in getFormMaterial)
                                        {
                                            QcTransactionGroupFormMaterial dataTrxFormMaterial = new QcTransactionGroupFormMaterial()
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
                                            };
                                            insertTrxFormMaterial.Add(dataTrxFormMaterial);
                                        }
                                    }

                                    if (insertTrxFormMaterial.Any())
                                    {
                                        await _context.QcTransactionGroupFormMaterials.AddRangeAsync(insertTrxFormMaterial);
                                        await _context.SaveChangesAsync(); // save data
                                    }
                                    #endregion
                                }

                                /*Section Tool*/
                                if (trxFormSection.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_TOOL)
                                {
                                    #region transaction group tool
                                    /* get form tool process */
                                    var getFormTool = await _dataProviderQcProcess.GetFormTool(pc.QcProcessId);
                                    List<QcTransactionGroupFormTool> insertTrxFormTool = new List<QcTransactionGroupFormTool>();
                                    if (getFormTool.Any())
                                    {
                                        foreach (var ft in getFormTool)
                                        {
                                            QcTransactionGroupFormTool dataTrxFormTool = new QcTransactionGroupFormTool()
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
                                            };
                                            insertTrxFormTool.Add(dataTrxFormTool);
                                        }
                                    }

                                    if (insertTrxFormTool.Any())
                                    {
                                        await _context.QcTransactionGroupFormTools.AddRangeAsync(insertTrxFormTool);
                                        await _context.SaveChangesAsync(); // save data
                                    }
                                    #endregion
                                }

                                /*Section Procedure*/
                                if (trxFormSection.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_PROCEDURE)
                                {
                                    #region tansaction group procedure
                                    /* get form procedure process */
                                    var getFormProcedure = await _dataProviderQcProcess.GetFormProcedureSection(pc.QcProcessId, ApplicationConstant.SECTION_TYPE_ID_PROCEDURE);
                                    List<QcTransactionGroupFormProcedure> insertTrxProcedure = new List<QcTransactionGroupFormProcedure>();
                                    if (getFormProcedure.Any())
                                    {
                                        foreach (var frmProcedure in getFormProcedure)
                                        {
                                            QcTransactionGroupFormProcedure dataTrxProcedure = new QcTransactionGroupFormProcedure()
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
                                            };
                                            insertTrxProcedure.Add(dataTrxProcedure);
                                        }
                                    }

                                    if (insertTrxProcedure.Any())
                                    {
                                        await _context.QcTransactionGroupFormProcedures.AddRangeAsync(insertTrxProcedure);
                                        await _context.SaveChangesAsync(); // save data

                                        List<QcTransactionGroupFormParameter> insertTrxFormParameter = new List<QcTransactionGroupFormParameter>();
                                        foreach (var prc in insertTrxProcedure)
                                        {
                                            var getFromParameterByProcedure = await _dataProviderQcProcess.GetFormParameter(prc.FormProcedureId);
                                            if (getFromParameterByProcedure.Any())
                                            {
                                                foreach (var fpp in getFromParameterByProcedure)
                                                {
                                                    QcTransactionGroupFormParameter dataTrxFormParameter = new QcTransactionGroupFormParameter()
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
                                                    };
                                                    insertTrxFormParameter.Add(dataTrxFormParameter);
                                                }
                                            }
                                        }

                                        if (insertTrxFormParameter.Any())
                                        {
                                            await _context.QcTransactionGroupFormParameters.AddRangeAsync(insertTrxFormParameter);
                                            await _context.SaveChangesAsync(); // save data

                                            //insert data sample param value
                                            if (pc.QcProcessId != ApplicationConstant.PROCESS_UJI_IDENTIFIKASI)
                                            {
                                                List<QcTransactionGroupSampleValue> insertTrxValueSample = new List<QcTransactionGroupSampleValue>();
                                                //List<QcTransactionGroupValue> insertTrxValue = new List<QcTransactionGroupValue>();
                                                foreach (var fpr in insertTrxFormParameter)
                                                {

                                                    if (sampleTest.Any() && fpr.IsForAllSample == false)
                                                    {
                                                        var SeqValSample = 1;
                                                        foreach (var std in sampleTest)
                                                        {
                                                            QcTransactionGroupSampleValue dataTrxValueSample = new QcTransactionGroupSampleValue()
                                                            {
                                                                QcTransactionGroupFormParameterId = fpr.Id,
                                                                QcTransactionSampleId = std.Id,
                                                                Sequence = SeqValSample,
                                                                CreatedBy = data.CreatedBy,
                                                                UpdatedBy = data.CreatedBy,
                                                                CreatedAt = DateHelper.Now(),
                                                                UpdatedAt = DateHelper.Now()
                                                            };
                                                            SeqValSample++;
                                                            insertTrxValueSample.Add(dataTrxValueSample);
                                                        }

                                                        if (insertTrxValueSample.Any())
                                                        {
                                                            await _context.QcTransactionSampleValues.AddRangeAsync(insertTrxValueSample);
                                                            await _context.SaveChangesAsync(); // save data
                                                        }
                                                    }
                                                    else if (fpr.IsForAllSample == true)
                                                    {
                                                        QcTransactionGroupValue dataTrxValue = new QcTransactionGroupValue()
                                                        {
                                                            QcTransactionGroupFormParameterId = fpr.Id,
                                                            Sequence = 1,
                                                            Value = fpr.DefaultValue,
                                                            CreatedBy = data.CreatedBy,
                                                            UpdatedBy = data.CreatedBy,
                                                            CreatedAt = DateHelper.Now(),
                                                            UpdatedAt = DateHelper.Now()
                                                        };

                                                        await _context.QcTransactionGroupValues.AddAsync(dataTrxValue);
                                                        await _context.SaveChangesAsync(); // save data

                                                    }


                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }

                                /*Section General Form procedure*/
                                if (trxFormSection.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_NOTE)
                                {
                                    #region tansaction group procedure note
                                    /* get General Form procedure */
                                    var getFormProcedure = await _dataProviderQcProcess.GetFormProcedureSection(pc.QcProcessId, ApplicationConstant.SECTION_TYPE_ID_NOTE);
                                    List<QcTransactionGroupFormProcedure> insertTrxProcedure = new List<QcTransactionGroupFormProcedure>();
                                    if (getFormProcedure.Any())
                                    {
                                        foreach (var frmProcedure in getFormProcedure)
                                        {
                                            QcTransactionGroupFormProcedure dataTrxProcedure = new QcTransactionGroupFormProcedure()
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
                                            };
                                            insertTrxProcedure.Add(dataTrxProcedure);
                                        }
                                    }

                                    if (insertTrxProcedure.Any())
                                    {
                                        await _context.QcTransactionGroupFormProcedures.AddRangeAsync(insertTrxProcedure);
                                        await _context.SaveChangesAsync(); // save data

                                        List<QcTransactionGroupFormParameter> insertTrxFormParameter = new List<QcTransactionGroupFormParameter>();
                                        foreach (var prc in insertTrxProcedure)
                                        {
                                            var getFromParameterByProcedure = await _dataProviderQcProcess.GetFormParameter(prc.FormProcedureId);
                                            if (getFromParameterByProcedure.Any())
                                            {
                                                foreach (var fpp in getFromParameterByProcedure)
                                                {
                                                    QcTransactionGroupFormParameter dataTrxFormParameter = new QcTransactionGroupFormParameter()
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
                                                    };
                                                    insertTrxFormParameter.Add(dataTrxFormParameter);
                                                }
                                            }
                                        }

                                        if (insertTrxFormParameter.Any())
                                        {
                                            await _context.QcTransactionGroupFormParameters.AddRangeAsync(insertTrxFormParameter);
                                            await _context.SaveChangesAsync(); // save data

                                            //insert data sample param value
                                            if (pc.QcProcessId != ApplicationConstant.PROCESS_UJI_IDENTIFIKASI)
                                            {
                                                List<QcTransactionGroupSampleValue> insertTrxValueSample = new List<QcTransactionGroupSampleValue>();
                                                //List<QcTransactionGroupValue> insertTrxValue = new List<QcTransactionGroupValue>();
                                                foreach (var fpr in insertTrxFormParameter)
                                                {

                                                    if (sampleTest.Any() && fpr.IsForAllSample == false)
                                                    {
                                                        var SeqValSample = 1;
                                                        foreach (var std in sampleTest)
                                                        {
                                                            QcTransactionGroupSampleValue dataTrxValueSample = new QcTransactionGroupSampleValue()
                                                            {
                                                                QcTransactionGroupFormParameterId = fpr.Id,
                                                                QcTransactionSampleId = std.Id,
                                                                Sequence = SeqValSample,
                                                                CreatedBy = data.CreatedBy,
                                                                UpdatedBy = data.CreatedBy,
                                                                CreatedAt = DateHelper.Now(),
                                                                UpdatedAt = DateHelper.Now()
                                                            };
                                                            SeqValSample++;
                                                            insertTrxValueSample.Add(dataTrxValueSample);
                                                        }

                                                        if (insertTrxValueSample.Any())
                                                        {
                                                            await _context.QcTransactionSampleValues.AddRangeAsync(insertTrxValueSample);
                                                            await _context.SaveChangesAsync(); // save data
                                                        }
                                                    }
                                                    else if (fpr.IsForAllSample == true)
                                                    {
                                                        QcTransactionGroupValue dataTrxValue = new QcTransactionGroupValue()
                                                        {
                                                            QcTransactionGroupFormParameterId = fpr.Id,
                                                            Sequence = 1,
                                                            Value = fpr.DefaultValue,
                                                            CreatedBy = data.CreatedBy,
                                                            UpdatedBy = data.CreatedBy,
                                                            CreatedAt = DateHelper.Now(),
                                                            UpdatedAt = DateHelper.Now()
                                                        };

                                                        await _context.QcTransactionGroupValues.AddAsync(dataTrxValue);
                                                        await _context.SaveChangesAsync(); // save data

                                                    }


                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }


                            }


                        }

                    }


                }

            }



            return insertParentTrxProcess;
        }

        public async Task<QcTransactionGroup> UpdateQcTransactionGroupDataFromApproval(UpdateQcTransactionGroupFromApproval data)
        {
            var course = _context.QcTransactionGroups.FirstOrDefault(x => x.Id == data.TransactionGroupId);

            if (course != null)
            {
                course.Id = data.TransactionGroupId;
                course.WorkflowStatus = data.WorkflowStatus;
                course.Status = data.Status;
                course.UpdatedBy = data.UpdatedBy;
                course.UpdatedAt = DateHelper.Now();
            }

            //update receiptdate in master data sampling 
            var GetIdSampling = _context.QcTransactionGroups.Join(
                _context.QcTransactionSamples,
                qtg => qtg.Id,
                qts => qts.QcTransactionGroupId,
                (qtg, qts) => new { qtg, qts }
            ).Join(
                _context.QcSamples,
                combineQtg => combineQtg.qts.QcSampleId,
                sample => sample.Id,
                (combineQtg, sample) => new { samplingId = sample.QcSamplingId, qcTransactionGroupId = combineQtg.qtg.Id }
            ).FirstOrDefault(x => x.qcTransactionGroupId == data.TransactionGroupId);

            var sampling = _context.QcSamplings.FirstOrDefault(x => x.Id == GetIdSampling.samplingId);
            if (sampling != null)
            {
                sampling.ReceiptDate = DateHelper.Now();
            }

            //jika status berubah menjadi review QA
            if (data.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA)
            {
                await _requestDataProvider.UpdateReceiptDate(sampling.RequestQcsId, true);
            }
            else if (data.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG)
            {
                await _requestDataProvider.UpdateReceiptDate(sampling.RequestQcsId, false);
            }
            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<int> GetRequestIdByWorkflowCode(string workflowDocumentCode, string workflowStatus)
        {
            var result = 0;
            var resultGetQcRequestIdByWorkflowCode = await (from qt in _context.QcTransactionGroups
                                                            join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                                                            join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                                                            join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                                                            join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                                                            where wt.WorkflowDocumentCode == workflowDocumentCode
                                                            select new { RequestQcsId = sq.RequestQcsId }
                                                            ).FirstOrDefaultAsync();

            if (resultGetQcRequestIdByWorkflowCode != null)
            {
                var resultWorkflowCodeByRequestIde = await (from qt in _context.QcTransactionGroups
                                                            join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                                                            join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                                                            join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                                                            join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                                                            where sq.RequestQcsId == resultGetQcRequestIdByWorkflowCode.RequestQcsId && wt.WorkflowStatus != ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                                                            select new { WorkflowCode = wt.WorkflowDocumentCode, WorkflowStatus = wt.WorkflowStatus }
                                                                ).Distinct().ToListAsync();
                if (resultWorkflowCodeByRequestIde.FirstOrDefault(x => x.WorkflowStatus != workflowStatus) == null)
                {
                    result = resultGetQcRequestIdByWorkflowCode.RequestQcsId;
                }
            }

            return result;
        }

        public async Task<List<int>> GetAllRequestIdByWorkflowCode(List<ListReviewPending> listReviewPending, string workflowStatus)
        {
            List<int> result = new List<int>();
            List<string> workflowDocumentCodes = listReviewPending.Select(x => x.RecordId).ToList();
            var resultGetQcRequestIdByWorkflowCode = await (from qt in _context.QcTransactionGroups
                                                            join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                                                            join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                                                            join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                                                            join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                                                            where workflowDocumentCodes.Contains(wt.WorkflowDocumentCode)
                                                            && sq.WorkflowStatus == workflowStatus
                                                            select sq.RequestQcsId
                                                            ).ToListAsync();

            var resultWorkflowCodeByRequestIds = await (from qt in _context.QcTransactionGroups
                                                        join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                                                        join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                                                        join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                                                        join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                                                        where wt.WorkflowStatus != ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                                                        select new
                                                        {
                                                            WorkflowCode = wt.WorkflowDocumentCode,
                                                            WorkflowStatus = wt.WorkflowStatus,
                                                            Key = sq.RequestQcsId
                                                        }).Distinct().ToListAsync();

            if (resultGetQcRequestIdByWorkflowCode.Any())
            {
                foreach (var item in resultGetQcRequestIdByWorkflowCode.GroupBy(x => x))
                {
                    var resultWorkflowCodeByRequestIde = resultWorkflowCodeByRequestIds.Where(x => x.Key == item.Key).ToList();
                    if (resultWorkflowCodeByRequestIde.FirstOrDefault(x => x.WorkflowStatus != workflowStatus) == null)
                    {
                        result.Add(item.Key);
                    }

                }
            }
            return result;
        }

        public async Task<List<int>> GetRequestIdSamplingTestingInReviewQa()
        {
            return await (from qt in _context.QcTransactionGroups
                          join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                          join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                          join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                          join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                          where wt.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA
                          select sq.RequestQcsId).ToListAsync();
        }

        public async Task<int> FindTestByWorkflowDocCodeAndQcRequestId(string workflowDocumentCode, string workflowStatus, int qcRequestId)
        {
            var result = 0;
            var resultGetQcRequestIdByWorkflowCode = await (from qt in _context.QcTransactionGroups
                                                            join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                                                            join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                                                            join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                                                            join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                                                            where wt.WorkflowDocumentCode == workflowDocumentCode
                                                            select new { RequestQcsId = sq.RequestQcsId }
                                                            ).FirstOrDefaultAsync();

            if (resultGetQcRequestIdByWorkflowCode != null)
            {
                var resultWorkflowCodeByRequestIde = await (from qt in _context.QcTransactionGroups
                                                            join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                                                            join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                                                            join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                                                            join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                                                            where sq.RequestQcsId == qcRequestId
                                                            select new { WorkflowCode = wt.WorkflowDocumentCode, WorkflowStatus = wt.WorkflowStatus }
                                                                ).Distinct().ToListAsync();
                if (resultWorkflowCodeByRequestIde.FirstOrDefault(x => x.WorkflowStatus != workflowStatus) != null)
                {
                    result = resultGetQcRequestIdByWorkflowCode.RequestQcsId;
                }
            }

            return result;
        }

        public async Task<List<RequestQcs>> GetQcRequestByTestId(int transactionGroupId)
        {
            return await
            (
                from rq in _context.RequestQcs
                join qsing in _context.QcSamplings on rq.Id equals qsing.RequestQcsId
                join qs in _context.QcSamples on qsing.Id equals qs.QcSamplingId
                join trgs in _context.QcTransactionSamples on qs.Id equals trgs.QcSampleId
                where trgs.QcTransactionGroupId == transactionGroupId
                select rq
            ).ToListAsync();
        }

        public async Task<BindingRequestQcTransactionGroupModel> GetQcRequestByTransactionGroupId(int transactionGroupId)
        {
            return await
            (
                from rq in _context.RequestQcs
                join qsing in _context.QcSamplings on rq.Id equals qsing.RequestQcsId
                join qs in _context.QcSamples on qsing.Id equals qs.QcSamplingId
                join trgs in _context.QcTransactionSamples on qs.Id equals trgs.QcSampleId
                join trg in _context.QcTransactionGroups on trgs.QcTransactionGroupId equals trg.Id
                where trgs.QcTransactionGroupId == transactionGroupId
                select new BindingRequestQcTransactionGroupModel { RequestQcId = rq.Id, CodeTest = trg.Code, TestType = qsing.SamplingTypeName }
            ).FirstOrDefaultAsync();
        }

        public async Task<int> getOrganizationByRequestId(int requestQcsId)
        {
            var result = await
            (
                from rq in _context.RequestQcs
                join rr in _context.RequestRooms on rq.Id equals rr.QcRequestId
                join r in _context.TransactionRoom on rr.RoomId equals r.Id
                join or in _context.TransactionOrganization on r.OrganizationId equals or.Id
                where rq.Id == requestQcsId
                select new { BIOHROrganizationId = or.BiohrOrganizationId.Value }
            ).FirstOrDefaultAsync();
            return result.BIOHROrganizationId;
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

        public async Task<QcTransactionGroup> UpdateRaw(QcTransactionGroup data)
        {
            _context.QcTransactionGroups.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<List<int>> GetByRequestId(int qcsRequestId)
        {
            return await
            (
                from trgs in _context.QcTransactionSamples
                join qs in _context.QcSamples on trgs.QcSampleId equals qs.Id
                join qsing in _context.QcSamplings on qs.QcSamplingId equals qsing.Id
                join rq in _context.RequestQcs on qsing.RequestQcsId equals rq.Id
                where rq.Id == qcsRequestId
                select trgs.QcTransactionGroupId
            ).Distinct().ToListAsync();
        }

        public async Task<List<int>> GetSameSamplingById(int transactionGroupId)
        {
            var result = await (
                    from wqtg in _context.WorkflowQcTransactionGroup
                    join qtg in _context.QcTransactionGroups on wqtg.QcTransactionGroupId equals qtg.Id
                    join qts in _context.QcTransactionSamples on qtg.Id equals qts.QcTransactionGroupId
                    join qs in _context.QcSamples on qts.QcSampleId equals qs.Id
                    join qsing in _context.QcSamplings on qs.QcSamplingId equals qsing.Id
                    join wqs in _context.WorkflowQcSampling on qsing.Id equals wqs.QcSamplingId
                    where qtg.Id == transactionGroupId
                    && qtg.WorkflowStatus == qsing.WorkflowStatus
                    && wqs.WorkflowStatus != ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                    && wqtg.WorkflowStatus != ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                    select qsing.Id
                ).ToListAsync();
            return result;
        }
        public async Task<List<int>> GetByRequestIdOnPending(int qcsRequestId, string nik)
        {
            var pending = await _workflowServiceDataProvider.GetListPendingByNik(nik);
            List<int> result = new List<int>();

            List<string> workflowDocumentCodes = pending.ListPending.Select(x => x.RecordId).ToList();
            if (workflowDocumentCodes.Any())
            {
                var testing =
                            await (
                                from trgs in _context.QcTransactionSamples
                                join qs in _context.QcSamples on trgs.QcSampleId equals qs.Id
                                join qsing in _context.QcSamplings on qs.QcSamplingId equals qsing.Id
                                join rq in _context.RequestQcs on qsing.RequestQcsId equals rq.Id
                                join qtg in _context.QcTransactionGroups on trgs.QcTransactionGroupId equals qtg.Id
                                join wt in _context.WorkflowQcTransactionGroup on qtg.Id equals wt.QcTransactionGroupId
                                where rq.Id == qcsRequestId
                                && workflowDocumentCodes.Contains(wt.WorkflowDocumentCode)
                                select trgs.QcTransactionGroupId
                            ).ToListAsync();
                result.AddRange(testing);
            }
            return result;
        }

        public async Task<List<int>> GetByWorkflowDocumentCodeOnPending(string workflowDocumentCode, string nik)
        {
            var pending = await _workflowServiceDataProvider.GetListPendingByNik(nik);
            List<int> result = new List<int>();

            foreach (var item in pending.ListPending)
            {
                var testing =
                            (
                                from trgs in _context.QcTransactionSamples
                                join qs in _context.QcSamples on trgs.QcSampleId equals qs.Id
                                join qsing in _context.QcSamplings on qs.QcSamplingId equals qsing.Id
                                join rq in _context.RequestQcs on qsing.RequestQcsId equals rq.Id
                                join qtg in _context.QcTransactionGroups on trgs.QcTransactionGroupId equals qtg.Id
                                join wt in _context.WorkflowQcTransactionGroup on qtg.Id equals wt.Id
                                where wt.WorkflowDocumentCode == workflowDocumentCode
                                && wt.WorkflowDocumentCode == item.RecordId
                                select trgs.QcTransactionGroupId
                            ).FirstOrDefault();

                if (testing != 0)
                {
                    result.Add(testing);
                }
            }

            return result;
        }

        public async Task<int> GetRequestIdStatusComplete(int requestQcsId)
        {
            var result = 0;
            var resultWorkflowCodeByRequestIde = await (from qt in _context.QcTransactionGroups
                                                        join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                                                        join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                                                        join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                                                        join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                                                        where sq.RequestQcsId == requestQcsId
                                                        && wt.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_2
                                                        && wt.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                                                        select new { WorkflowCode = wt.WorkflowDocumentCode, IsInWorkflow = wt.IsInWorkflow }
                                                            ).Distinct().ToListAsync();

            if (resultWorkflowCodeByRequestIde.FirstOrDefault(x => x.IsInWorkflow == true) == null)
            {
                result = requestQcsId;
            }
            return result;
        }

        public void GenerateQcResult(QcTransactionGroup data, List<QcTransactionGroupSampleValue> dataSampleObservasi, List<QcTransactionGroupSampleValue> dataSampleUjiIdentifikasi)
        {
            _ = Task.Run(async () =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<QcsProductContext>();
                var requestDataProvider = scope.ServiceProvider.GetRequiredService<IQcRequestDataProvider>();

                #region generate qc result - observasi

                var observasiSampleTestIds = dataSampleObservasi.Select(x => x.QcTransactionSampleId).Distinct().ToList();

                var observasiTransactionSamples = await (from txs in context.QcTransactionSamples
                                                         join s in context.QcSamples on txs.QcSampleId equals s.Id
                                                         where txs.RowStatus == null
                                                               && observasiSampleTestIds.Contains(txs.Id)
                                                         select new QcTransactionSampleViewModel()
                                                         {
                                                             Id = txs.Id,
                                                             QcSampleId = txs.QcSampleId,
                                                             QcSamplingId = txs.QcSamplingId ?? 0,
                                                             TestParameterId = s.TestParamId,
                                                             TestScenarioId = s.TestScenarioId ?? 0
                                                         }).ToListAsync();

                var observasiSampleIds = observasiTransactionSamples.Select(x => x.QcSampleId).ToList();

                var observasiResultList = await (from res in context.QcResults
                                                 where observasiSampleIds.Contains(res.SampleId)
                                                 select res).ToListAsync();


                if (observasiResultList.Any())
                {
                    context.QcResults.RemoveRange(observasiResultList);
                }

                var testVariables = await (from tv in context.TestVariables
                                           join tstp in context.RelTestScenarioParams on tv.TestParameterId equals tstp.Id
                                           join tp in context.TestParameters on tstp.TestParameterId equals tp.Id
                                           where tv.RowStatus == null
                                           orderby tv.Sequence
                                           select new TestVariableViewModel()
                                           {
                                               TestScenarioId = tstp.TestScenarioId,
                                               TestParameterId = tstp.TestParameterId,
                                               TresholdMax = tv.TresholdMax,
                                               TresholdMin = tv.TresholdMin,
                                               TresholdValue = tv.TresholdValue,
                                               TresholdOperator = tv.TresholdOperator,
                                               Sequence = tv.Sequence
                                           }).ToListAsync();

                var transactionSampleMap = new ConcurrentDictionary<QcTransactionGroupSampleValue, QcResult>();

                Parallel.ForEach(dataSampleObservasi, tsO =>
                {
                    var testVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                    var testVariableId = 0;

                    var newQcResult = new QcResult()
                    {
                        SampleId = 0,
                        TestVariableId = testVariableId,
                        TestVariableConclusion = testVariableConclusionLabel
                    };

                    var sampleTestValue = tsO.Value;

                    try
                    {
                        var getDetailSample =
                            observasiTransactionSamples.FirstOrDefault(x => x.Id == tsO.QcTransactionSampleId);

                        if (getDetailSample == null)
                        {
                            _logger.LogError("GenerateQcResult. Sample Test ID: {QcTransactionSampleId} not found",
                                tsO.QcTransactionSampleId);
                            return;
                        }

                        newQcResult.SampleId = getDetailSample.Id;

                        if (!Regex.IsMatch(sampleTestValue, @"^\d+$"))
                        {
                            _logger.LogError("GenerateQcResult. Sample Test ID: {QcTransactionSampleId} is invalid value: {Value} ", tsO.QcTransactionSampleId, tsO.Value);
                            throw new Exception("Invalid sample value");
                        }

                        var testVariableList = testVariables
                            .Where(x => x.TestParameterId == getDetailSample.TestParameterId)
                            .Where(x => x.TestScenarioId == getDetailSample.TestScenarioId)
                            .OrderBy(x => x.Sequence)
                            .ToList();

                        foreach (var th in testVariableList)
                        {
                            switch (th.TresholdOperator)
                            {
                                case ApplicationConstant.THRESHOLD_EQUAL:
                                    if (Int32.Parse(sampleTestValue) == th.TresholdValue)
                                    {
                                        switch (th.VariableName)
                                        {
                                            case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                testVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                testVariableId = th.Id;
                                                break;
                                        }
                                    }

                                    break;

                                case ApplicationConstant.THRESHOLD_GREATER_THAN:
                                    if (Int32.Parse(sampleTestValue) <= th.TresholdValue)
                                    {
                                        switch (th.VariableName)
                                        {
                                            case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                testVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                testVariableId = th.Id;
                                                break;
                                        }
                                    }

                                    break;

                                case ApplicationConstant.THRESHOLD_LESS_THAN:
                                    if (Int32.Parse(sampleTestValue) >= th.TresholdValue)
                                    {
                                        switch (th.VariableName)
                                        {
                                            case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                testVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                testVariableId = th.Id;
                                                break;
                                        }
                                    }

                                    break;

                                case ApplicationConstant.THRESHOLD_GREATER_THAN_OR_EQUAL:
                                    if (Int32.Parse(sampleTestValue) < th.TresholdValue)
                                    {
                                        switch (th.VariableName)
                                        {
                                            case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                testVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                testVariableId = th.Id;
                                                break;
                                        }
                                    }

                                    break;

                                case ApplicationConstant.THRESHOLD_LESS_THAN_OR_EQUAL:
                                    if (Int32.Parse(sampleTestValue) > th.TresholdValue) // 71
                                    {
                                        switch (th.VariableName)
                                        {
                                            case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_PASS; // 68
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING; // 71
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL; //90
                                                testVariableId = th.Id;
                                                break;
                                        }
                                    }

                                    break;

                                case ApplicationConstant.THRESHOLD_IN_BETTWEEN:
                                    if (Int32.Parse(sampleTestValue) <= th.TresholdMin &&
                                        Int32.Parse(sampleTestValue) >= th.TresholdMax)
                                    {
                                        switch (th.VariableName)
                                        {
                                            case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                testVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                testVariableId = th.Id;
                                                break;
                                        }
                                    }

                                    break;

                                case ApplicationConstant.THRESHOLD_IN_BETTWEEN_OR_EQUAL:
                                    if (Int32.Parse(sampleTestValue) < th.TresholdMin &&
                                        Int32.Parse(sampleTestValue) > th.TresholdMax)
                                    {
                                        switch (th.VariableName)
                                        {
                                            case ApplicationConstant.TEST_VARIABLE_ALERT:
                                                testVariableConclusionLabel = ApplicationConstant.LABEL_CONCLUSION_PASS;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_ACTION_LIMIT:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OOAEM_WARNING;
                                                testVariableId = th.Id;
                                                break;

                                            case ApplicationConstant.TEST_VARIABLE_SPESIFICATION:
                                                testVariableConclusionLabel =
                                                    ApplicationConstant.LABEL_CONCLUSION_OEM_FAIL;
                                                testVariableId = th.Id;
                                                break;
                                        }
                                    }

                                    break;
                            }
                        }

                        newQcResult.TestVariableConclusion = testVariableConclusionLabel;
                        newQcResult.TestVariableId = testVariableId;
                    }
                    catch (Exception)
                    {
                        //ignore
                    }


                    newQcResult.Value = sampleTestValue;
                    newQcResult.CreatedBy = data.UpdatedBy;
                    newQcResult.UpdatedBy = data.UpdatedBy;
                    newQcResult.CreatedAt = DateHelper.Now();
                    newQcResult.UpdatedAt = DateHelper.Now();

                    transactionSampleMap.TryAdd(tsO, newQcResult);

                });


                if (transactionSampleMap.Any())
                {
                    //save changes to DB
                    await context.QcResults.AddRangeAsync(transactionSampleMap.Values);
                    await context.SaveChangesAsync();

                    #region update qc request conclusion

                    var samplingIds = await (from txs in context.QcTransactionSamples
                                             join s in context.QcSamples on txs.QcSampleId equals s.Id
                                             where txs.RowStatus == null
                                                   && observasiSampleTestIds.Contains(txs.Id)
                                             select s.QcSamplingId)
                        .Distinct()
                        .ToListAsync();

                    var requestIds = await (from smp in context.QcSamplings
                                            where smp.RowStatus == null
                                                  && samplingIds.Contains(smp.Id)
                                            select smp.RequestQcsId)
                        .ToListAsync();

                    foreach (var requestId in requestIds)
                    {
                        await requestDataProvider.GenerateUpdateConclusion(requestId);
                    }

                    #endregion
                }
                #endregion

                #region generate result - uji identifikasi

                foreach (var ts1 in dataSampleUjiIdentifikasi)
                {
                    if (ts1.Value == null)
                    {
                        continue;
                    }

                    var dataMikroba = JsonSerializer.Deserialize<List<ListNoteViewModel>>(ts1.Value);

                    if (dataMikroba == null || !dataMikroba.Any())
                    {
                        _logger.LogWarning("Data mikroba kosong");
                        ts1.Value = null;
                        continue;
                    }

                    var getDetailSample2 = await (from txs in context.QcTransactionSamples
                                                  join s in context.QcSamples on txs.QcSampleId equals s.Id
                                                  where txs.RowStatus == null
                                                        && txs.Id == ts1.QcTransactionSampleId
                                                  select s).FirstOrDefaultAsync();

                    if (getDetailSample2 == null)
                    {
                        _logger.LogWarning("Daftar sample kosong");
                        ts1.Value = null;
                        continue;
                    }

                    var GetResult = await (from res in context.QcResults
                                           where res.SampleId == getDetailSample2.Id
                                           select res).FirstOrDefaultAsync();

                    if (GetResult == null)
                    {
                        _logger.LogWarning("QC Result kosong");
                        ts1.Value = null;
                        continue;
                    }

                    var labelMikroba = dataMikroba.Select(x => x.label).Distinct().ToList();
                    var NoteMikroba = String.Join(',', labelMikroba);

                    GetResult.Note = NoteMikroba;
                }

                //save changes to DB
                await context.SaveChangesAsync();

                #endregion
            });
        }

        public async Task<QcTransactionGroup> GetByIdForAudit(int id)
        {
            var result = await _context.QcTransactionGroups
                .Include(x => x.TransactionGroupSamples)
                .Include(x => x.TransactionGroupSamplings)

                .Include(x => x.TransactionGroupProcesses)
                    .ThenInclude(x => x.TransactionGroupFormProcedures)
                        .ThenInclude(x => x.TransactionGroupFormParameters)
                            .ThenInclude(x => x.TransactionGroupValues)

                .Include(x => x.TransactionGroupProcesses)
                    .ThenInclude(x => x.TransactionGroupFormProcedures)
                        .ThenInclude(x => x.TransactionGroupFormParameters)
                            .ThenInclude(x => x.TransactionGroupSampleValues)

                .Include(x => x.TransactionGroupProcesses)
                    .ThenInclude(x => x.TransactionGroupFormTools)
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<QcTransactionGroup> EditV2(QcTransactionGroup data, List<EditQcProcessSample> sampleTest, List<EditQcProcessSamplingBatch> samplingTest)
        {
            var result = new QcTransactionGroup();
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var qcProcess = await _dataProviderQcProcess.GetQcProcessById(data.QcProcessId);

                if (qcProcess.AddSampleLayoutType == ApplicationConstant.ADD_SAMPLE_TEST)
                {
                    var sampleTestIds = sampleTest.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();

                    var deleteSampleTestData = await (from st in _context.QcTransactionSamples
                                                      where !sampleTestIds.Contains(st.Id)
                                                            && st.QcTransactionGroupId == data.Id
                                                      select st).ToListAsync();

                    _context.QcTransactionSamples.RemoveRange(deleteSampleTestData);

                    var sampleTestData = await (from st in _context.QcTransactionSamples
                                                where sampleTestIds.Contains(st.Id)
                                                      && st.QcTransactionGroupId == data.Id
                                                select st).ToListAsync();

                    foreach (var edSamp in sampleTestData)
                    {
                        var editData = sampleTest.FirstOrDefault(x => x.Id.HasValue && x.Id.Value == edSamp.Id);
                        if (editData == null)
                        {
                            continue;
                        }

                        edSamp.QcSampleId = editData.QcSampleId;
                        edSamp.UpdatedBy = data.UpdatedBy;
                        edSamp.UpdatedAt = DateHelper.Now();
                    }

                    var addSampleTestData = sampleTest.Where(x => !x.Id.HasValue).ToList();
                    foreach (var newSampleTest in addSampleTestData.Select(addSamp => new QcTransactionGroupSample
                    {
                        QcTransactionGroupId = data.Id,
                        QcSampleId = addSamp.QcSampleId,
                        CreatedBy = data.UpdatedBy,
                        UpdatedBy = data.UpdatedBy,
                        CreatedAt = DateHelper.Now(),
                        UpdatedAt = DateHelper.Now()
                    }))
                    {
                        _context.QcTransactionSamples.Add(newSampleTest);
                    }

                    await _context.SaveChangesAsync();
                }
                else if (qcProcess.AddSampleLayoutType == ApplicationConstant.ADD_SAMPLE_BY_BATCH_TEST)
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    #region remove all transaction sampling & transaction sample

                    var deleteSamplingList = await (from st in _context.QcTransactionSamplings
                                                    where st.QcTransactionGroupId == data.Id
                                                    select st).ToListAsync();

                    var deleteSampleList = await (from sd in _context.QcTransactionSamples
                                                  where sd.QcTransactionSamplingId.HasValue
                                                        && sd.QcTransactionGroupId == data.Id
                                                  select sd).ToListAsync();

                    _context.QcTransactionSamples.RemoveRange(deleteSampleList);

                    _context.QcTransactionSamplings.RemoveRange(deleteSamplingList);

                    #endregion

                    #region add new transction sampling

                    var newSamplingTestList = samplingTest.Select(addSampling => new QcTransactionGroupSampling
                    {
                        QcTransactionGroupId = data.Id,
                        QcSamplingId = addSampling.QcSamplingId,
                        CreatedBy = data.UpdatedBy,
                        UpdatedBy = data.UpdatedBy,
                        CreatedAt = DateHelper.Now(),
                        UpdatedAt = DateHelper.Now()
                    })
                        .ToList();

                    await _context.QcTransactionSamplings.AddRangeAsync(newSamplingTestList);

                    #endregion

                    #region add new transaction sample

                    var addSamplingIds = samplingTest.Select(x => x.QcSamplingId).ToList();

                    var addSampleTestList = await (from s in _context.QcSamples
                                                   join tp in _context.TransactionTestParameter on s.TestParamId equals tp.Id
                                                   where s.RowStatus == null
                                                         && tp.QcProcessId == data.QcProcessId
                                                         && addSamplingIds.Contains(s.QcSamplingId)
                                                   select s).ToListAsync();

                    var newSampleTestList = new List<QcTransactionGroupSample>();
                    foreach (var newSamplingTest in newSamplingTestList)
                    {
                        newSampleTestList.AddRange(addSampleTestList.Where(x => x.QcSamplingId == newSamplingTest.QcSamplingId)
                            .Select(addSampleTest => new QcTransactionGroupSample()
                            {
                                QcTransactionSamplingId = newSamplingTest.Id,
                                QcSamplingId = newSamplingTest.QcSamplingId,
                                QcSampleId = addSampleTest.Id,
                                QcTransactionGroupId = data.Id,
                                CreatedBy = data.CreatedBy,
                                UpdatedBy = data.CreatedBy,
                                CreatedAt = DateHelper.Now(),
                                UpdatedAt = DateHelper.Now()
                            }));
                    }

                    await _context.QcTransactionSamples.AddRangeAsync(newSampleTestList);

                    #endregion

                    await _context.SaveChangesAsync();
                    stopwatch.Stop();

                    Console.WriteLine("Add transaction sample. Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
                }

                await transaction.CommitAsync();

                #region generate qc transaction process test

                if (data.Status == ApplicationConstant.STATUS_TEST_READYTOTEST)
                {
                    _ = Task.Run(async () =>
                    {
                        await _generateQcProcessDataProvider.Generate(data);
                    });
                }

                #endregion

                result = data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                await transaction.RollbackAsync();
            }

            return result;
        }

        public static int? GetLastSamplingPointCode(string str)
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

        public static int? GetTestParamSequence(int paramId)
        {
            int? paramSequence;
            if (paramId == 4)
            {
                paramSequence = 1;
            }
            else if (paramId == 7)
            {
                paramSequence = 2;
            }
            else if (paramId == 2)
            {
                paramSequence = 3;
            }
            else if (paramId == 3)
            {
                paramSequence = 4;
            }
            else if (paramId == 1)
            {
                paramSequence = 5;
            }
            else
            {
                paramSequence = default(int);
            }

            return paramSequence;
        }

        public static int? GetTestParamSequenceByCode(string code)
        {
            int? paramSequence;
            switch (code)
            {
                case ApplicationConstant.CODE_PARAMTER_UJI_AS:
                    paramSequence = 1;
                    break;
                case ApplicationConstant.CODE_PARAMTER_UJI_CA:
                    paramSequence = 2;
                    break;
                case ApplicationConstant.CODE_PARAMTER_UJI_SP:
                    paramSequence = 3;
                    break;
                case ApplicationConstant.CODE_PARAMTER_UJI_FD:
                    paramSequence = 4;
                    break;
                case ApplicationConstant.CODE_PARAMTER_UJI_GLOVE:
                    paramSequence = 5;
                    break;
                default:
                    paramSequence = default(int);
                    break;
            }

            return paramSequence;
        }

        public async Task<QcTransactionGroupDetailViewModel> GetByIdV2(int id)
        {
            //get worfklow testing
            List<WorkflowQcTransactionGroup> worfklowQcTransactionGroup = await _workflowTransactionGroupdataProvider.GetByWorkflowByQcTransactionGroupId(id);

            List<WorkflowHistoryQcSampling> workflowQcTransactionGroupHistory = new List<WorkflowHistoryQcSampling>();
            string eDate = null;
            string eDate2 = null;

            foreach (var item in worfklowQcTransactionGroup)
            {
                DocumentHistoryResponseViewModel workflowHistory = await _workflowServiceDataProvider.GetListHistoryWorkflow(item.WorkflowDocumentCode);

                foreach (var itemHistory in workflowHistory.History)
                {
                    if (((itemHistory.StatusName != "Complete") && (item.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_1)))
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
                                DateTime = itemPIC.ActionDate == null ? eDate : itemPIC.ActionDate,
                                PersonalName = itemPIC.OrgName,
                                PersonalNik = itemPIC.OrgId,
                                Position = itemPIC.OrgPositionName,
                                ChangeStatusTime = itemPIC.ActionDate == null ? (actionUser == ApplicationConstant.WORKFLOW_STATUS_PENDING ? ApplicationConstant.MAX_DATETIME : eDate) : itemPIC.ActionDate
                            };

                            eDate = addWorkflowHistory.DateTime;

                            workflowQcTransactionGroupHistory.Add(addWorkflowHistory);
                        }
                    }
                    else if (item.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_2)
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
                                DateTime = itemPIC.ActionDate == null ? eDate2 : itemPIC.ActionDate,
                                PersonalName = itemPIC.OrgName,
                                PersonalNik = itemPIC.OrgId,
                                Position = itemPIC.OrgPositionName,
                                ChangeStatusTime = itemPIC.ActionDate == null ? (actionUser == ApplicationConstant.WORKFLOW_STATUS_PENDING ? ApplicationConstant.MAX_DATETIME : eDate2) : itemPIC.ActionDate
                            };
                            eDate2 = addWorkflowHistory.DateTime;

                            if (itemHistory.StatusName != ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT || addWorkflowHistory.Action != ApplicationConstant.WORKFLOW_ACTION_SUBMIT_NAME)
                            {
                                workflowQcTransactionGroupHistory.Add(addWorkflowHistory);
                            }
                        }
                    }
                }
            }
            List<WorkflowHistoryQcSampling> workflowHistoryQcs =
            workflowQcTransactionGroupHistory.OrderByDescending(x => x.ChangeStatusTime).ToList();

            WorkflowQcTransactionGroup currentWorkflow = await _workflowTransactionGroupdataProvider.GetByWorkflowByQcTransactionGroupIdIsInWorkflow(id);

            var result = await (from t in _context.QcTransactionGroups
                                join qp in _context.QcProcess on t.QcProcessId equals qp.Id
                                where t.Id == id
                                && t.RowStatus == null
                                select new QcTransactionGroupDetailViewModel
                                {
                                    Id = t.Id,
                                    Code = t.Code,
                                    QcProcessId = t.QcProcessId,
                                    QcProcessName = t.QcProcessName,
                                    AddSampleLayoutType = qp.AddSampleLayoutType,
                                    TestDate = t.TestDate,
                                    PersonelNik = t.PersonelNik,
                                    PersonelName = t.PersonelName,
                                    PersonelPairingNik = t.PersonelPairingNik,
                                    PersonelPairingName = t.PersonelPairingName,
                                    Status = t.Status,
                                    CreatedBy = t.CreatedBy,
                                    CreatedAt = t.CreatedAt,
                                    WorkflowCode = currentWorkflow != null ? currentWorkflow.WorkflowCode : null,
                                    SamplingBatchData = (from tsb in _context.QcTransactionSamplings
                                                         join s in _context.QcSamplings on tsb.QcSamplingId equals s.Id
                                                         join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                                         where tsb.QcTransactionGroupId == t.Id
                                                         select new QcTransactionSamplingRelationViewModel
                                                         {
                                                             Id = tsb.Id,
                                                             SamplingId = s.Id,
                                                             BacthQrCode = s.Code,
                                                             NoBatch = r.NoBatch,
                                                             PhaseId = r.EmPhaseId,
                                                             PhaseName = r.EmPhaseName,
                                                             RoomId = r.EmRoomId,
                                                             RoomName = r.EmRoomName,
                                                             ListRoom = (from rr in _context.RequestRooms
                                                                         where rr.QcRequestId == s.RequestQcsId
                                                                         select rr).ToList(),
                                                             ShipmentStatus = (from shp1 in _context.QcSamplingShipments
                                                                               where shp1.QrCode == s.Code
                                                                               select shp1.Status).FirstOrDefault(),
                                                             ShipmentStartDate = (from shp2 in _context.QcSamplingShipments
                                                                                  where shp2.QcSamplingId == s.Id
                                                                                  select shp2.StartDate).FirstOrDefault(),
                                                             ShipmentEndDate = (from shp3 in _context.QcSamplingShipments
                                                                                where shp3.QcSamplingId == s.Id
                                                                                select shp3.EndDate).FirstOrDefault(),
                                                             SamplingTestTransaction = (from qts in _context.QcTransactionSamplings
                                                                                        where qts.QcSamplingId == s.Id
                                                                                        select qts).Count()
                                                         }).ToList(),
                                    SamplesData = (from ts in _context.QcTransactionSamples
                                                   join sm in _context.QcSamples on ts.QcSampleId equals sm.Id
                                                   join s in _context.QcSamplings on sm.QcSamplingId equals s.Id
                                                   join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                                   join tp in _context.TransactionTestParameter on sm.TestParamId equals tp.Id
                                                   join tsc in _context.TransactionTestScenario on sm.TestScenarioId equals tsc.Id
                                                   where ts.QcTransactionGroupId == t.Id
                                                   select new QcTransactionSampleRelationViewModel
                                                   {
                                                       Id = ts.Id,
                                                       QcSampleId = ts.QcSampleId,
                                                       SampleCode = sm.Code,
                                                       NoBatch = r.NoBatch,
                                                       NoRequest = r.NoRequest,
                                                       EmRoomId = r.EmRoomId,
                                                       EmRoomName = r.EmRoomName,
                                                       GradeRoomId = sm.GradeRoomId,
                                                       GradeRoomName = sm.GradeRoomName,
                                                       GradeRoomCode = (sm.GradeRoomId != null ? (from mgr in _context.TransactionGradeRoom
                                                                                                  where mgr.Id == sm.GradeRoomId
                                                                                                  select mgr.Code).FirstOrDefault() : null),
                                                       EmPhaseId = r.EmPhaseId,
                                                       EmPhaseName = r.EmPhaseName,
                                                       TestParamId = sm.TestParamId,
                                                       TestParamName = sm.TestParamName,
                                                       TestParamShortName = tp.ShortName,
                                                       TestParamSequence = GetTestParamSequenceByCode(tp.ShortName),
                                                       PersonalId = sm.PersonalId,
                                                       PersonalInitial = sm.PersonalInitial,
                                                       PersonalName = sm.PersonalName,
                                                       ShipmentStartDate = (from shp2 in _context.QcSamplingShipments
                                                                            where shp2.QcSamplingId == s.Id
                                                                            select shp2.StartDate).FirstOrDefault(),
                                                       ShipmentEndDate = (from shp3 in _context.QcSamplingShipments
                                                                          where shp3.QcSamplingId == s.Id
                                                                          select shp3.EndDate).FirstOrDefault(),
                                                       CreatedBy = ts.CreatedBy,
                                                       CreatedAt = ts.CreatedAt,
                                                       SamplingPointId = sm.SamplingPointId,
                                                       SamplingPointCode = sm.SamplingPointCode,
                                                       TestScenarioLabel = tsc.Label,
                                                       TestScenarioCode = (tsc.Label == "at_rest"
                                                                                ? 0
                                                                                : 1),
                                                       TestVariableThreshold = (from tv in _context.TransactionTestVariable
                                                                                join en in _context.EnumConstant on tv.TresholdOperator equals en.TypeId
                                                                                join rtsp in _context.TransactionRelTestScenarioParam on tv.TestParameterId equals rtsp.Id
                                                                                join ts in _context.TransactionTestScenario on rtsp.TestScenarioId equals ts.Id
                                                                                join tp in _context.TransactionTestParameter on rtsp.TestParameterId equals tp.Id
                                                                                where tp.Id == sm.TestParamId
                                                                                && ts.Id == sm.TestScenarioId
                                                                                && en.KeyGroup == "threshold_operator"
                                                                                select new TestVariableViewModel
                                                                                {
                                                                                    Id = tv.Id,
                                                                                    VariableName = tv.VariableName,
                                                                                    Sequence = tv.Sequence.Value,
                                                                                    TresholdOperator = tv.TresholdOperator,
                                                                                    TresholdOperatorName = en.Name,
                                                                                    TresholdValue = tv.TresholdValue,
                                                                                    TresholdMin = tv.ThresholdValueFrom.Value,
                                                                                    TresholdMax = tv.ThresholdValueTo.Value,
                                                                                }).ToList(),
                                                       TestParamIndex = 1,
                                                       SamplingDateTimeFrom = sm.SamplingDateTimeFrom,
                                                       SamplingDateTimeTo = sm.SamplingDateTimeTo
                                                   }).ToList(),
                                }).FirstOrDefaultAsync();

            foreach (var sample in result.SamplesData)
            {
                if (sample.SamplingPointCode.Contains('-'))
                {
                    sample.FdLastSamplingPointCode = (sample.TestParamId == 4 ? GetLastSamplingPointCode(sample.SamplingPointCode) : null);
                    sample.FirstSamplingPointCode = sample.SamplingPointCode.Substring(0, sample.SamplingPointCode.LastIndexOf('-'));
                    sample.LastSamplingPointCode = GetLastSamplingPointCode(sample.SamplingPointCode);
                }
            }

            //menambahkan createdByName
            result.CreatedByName = await _GetNameByNik(result.CreatedBy);

            result.SamplesData = result.SamplesData.OrderBy(x => x.TestParamSequence)
                                                    .ThenBy(x => x.EmRoomName)
                                                    .ThenBy(x => x.GradeRoomCode)
                                                    .ThenBy(x => x.FdLastSamplingPointCode)
                                                    .ThenBy(x => x.FirstSamplingPointCode)
                                                    .ThenBy(x => x.LastSamplingPointCode)
                                                    .ThenBy(x => x.TestScenarioCode)
                                                    .ThenBy(x => x.SamplingDateTimeFrom)
                                                    // .ThenBy(x => x.SampleCode)
                                                    .ThenBy(x => x.Id)
                                                    .ToList();

            if (result != null)
            {
                var checkQCProcessDataParent = await (from c1 in _context.QcTransactionGroupProcesses
                                                      where c1.QcTransactionGroupId == result.Id
                                                      && c1.RowStatus == null
                                                      select c1.Id).CountAsync();

                //TODO next-nya perlu dipertimbangkan untuk menggunakan recursive
                if (checkQCProcessDataParent > 0)
                {
                    result.QcTransactionGroupProcess =
                        (from qtp in _context.QcTransactionGroupProcesses
                         where qtp.QcTransactionGroupId == result.Id
                         && qtp.RowStatus == null
                         select new QcTransactionGroupProcessRelViewModel
                         {
                             Id = qtp.Id,
                             QcTransactionGroupId = qtp.QcTransactionGroupId,
                             Sequence = qtp.Sequence,
                             Name = qtp.Name,
                             ParentId = qtp.ParentId,
                             RoomId = qtp.RoomId,
                             IsInputForm = qtp.IsInputForm,
                             QcProcessId = qtp.QcProcessId,
                             CreatedAt = qtp.CreatedAt,
                             CreatedBy = qtp.CreatedBy,
                             WorkflowHistory = workflowHistoryQcs,
                         }).FirstOrDefault();

                    var checkQCProcessDataChild = await (from c2 in _context.QcTransactionGroupProcesses
                                                         where c2.ParentId == result.QcTransactionGroupProcess.Id
                                                         && c2.RowStatus == null
                                                         select c2.Id).CountAsync();

                    if (checkQCProcessDataChild > 0)
                    {
                        result.QcTransactionGroupProcess.
                            QcTransactionGroupProcess =
                               (from qtpC in _context.QcTransactionGroupProcesses
                                join qtp in _context.QcTransactionGroups on qtpC.QcTransactionGroupId equals qtp.Id
                                where qtpC.ParentId == result.QcTransactionGroupProcess.Id
                                && qtpC.RowStatus == null
                                select new QcTransactionGroupProcessRelViewModel
                                {
                                    Id = qtpC.Id,
                                    QcTransactionGroupId = qtpC.QcTransactionGroupId,
                                    Sequence = qtpC.Sequence,
                                    Name = qtpC.Name,
                                    ParentId = qtpC.ParentId,
                                    RoomId = qtpC.RoomId,
                                    IsInputForm = qtpC.IsInputForm,
                                    QcProcessId = qtpC.QcProcessId,
                                    CreatedAt = qtpC.CreatedAt,
                                    CreatedBy = qtpC.CreatedBy,
                                    WorkflowHistory = workflowHistoryQcs,
                                    IsLastProcess = qtp.StatusProses == qtpC.QcProcessId,
                                    QcTransactionGroupFormMaterial = (from qtm in _context.QcTransactionGroupFormMaterials
                                                                      join qfs in _context.QcTransactionGroupFormSections on qtm.QcTransactionGroupSectionId equals qfs.Id
                                                                      join uom in _context.Uoms on qtm.Uom equals uom.UomId
                                                                      join uomPackage in _context.Uoms on qtm.UomPackage equals uomPackage.UomId
                                                                      where qtm.QcTransactionGroupProcessId == qtpC.Id
                                                                      && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_MATERIAL
                                                                      && qtm.RowStatus == null
                                                                      select new QcTransactionGroupFormMaterialViewModel
                                                                      {
                                                                          Id = qtm.Id,
                                                                          QcTransactionGroupProcessId = qtm.QcTransactionGroupProcessId,
                                                                          Sequence = qtm.Sequence,
                                                                          ItemId = qtm.ItemId,
                                                                          Code = qtm.Code,
                                                                          Name = qtm.Name,
                                                                          DefaultPackageQty = qtm.DefaultPackageQty,
                                                                          UomPackage = qtm.UomPackage,
                                                                          UomUomPackageLabel = uomPackage.Label,
                                                                          DefaultQty = qtm.DefaultQty,
                                                                          Uom = qtm.Uom,
                                                                          UomLabel = uom.Label,
                                                                          QcProcessId = qtm.QcProcessId,
                                                                          GroupName = qtm.GroupName,
                                                                          CreatedAt = qtm.CreatedAt,
                                                                          CreatedBy = qtm.CreatedBy
                                                                      }).OrderBy(x => x.Sequence).ToList(),

                                    QcTransactionGroupFormTool = (from qtt in _context.QcTransactionGroupFormTools
                                                                  join qfs in _context.QcTransactionGroupFormSections on qtt.QcTransactionGroupSectionId equals qfs.Id
                                                                  where qtt.QcTransactionGroupProcessId == qtpC.Id
                                                                  && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_TOOL
                                                                  && qtt.RowStatus == null
                                                                  select new QcTransactionGroupFormToolViewModel
                                                                  {
                                                                      Id = qtt.Id,
                                                                      QcTransactionGroupProcessId = qtt.QcTransactionGroupProcessId,
                                                                      Sequence = qtt.Sequence,
                                                                      ToolId = qtt.ToolId,
                                                                      ItemId = qtt.ItemId,
                                                                      Code = qtt.Code,
                                                                      Name = qtt.Name,
                                                                      Quantity = qtt.Quantity,
                                                                      QcProcessId = qtt.QcProcessId,
                                                                      CreatedAt = qtt.CreatedAt,
                                                                      CreatedBy = qtt.CreatedBy
                                                                  }).OrderBy(x => x.Sequence).ToList(),

                                    QcTransactionGroupFormProcedure = (from qtfp in _context.QcTransactionGroupFormProcedures
                                                                       join qfs in _context.QcTransactionGroupFormSections on qtfp.QcTransactionGroupSectionId equals qfs.Id
                                                                       where qtfp.QcTransactionGroupProcessId == qtpC.Id
                                                                       && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_PROCEDURE
                                                                       && qtfp.RowStatus == null
                                                                       select new QcTransactionGroupFormProcedureViewModel
                                                                       {
                                                                           Id = qtfp.Id,
                                                                           QcTransactionGroupProcessId = qtfp.QcTransactionGroupProcessId,
                                                                           Sequence = qtfp.Sequence,
                                                                           Description = qtfp.Description,
                                                                           FormProcedureId = qtfp.FormProcedureId,
                                                                           CreatedAt = qtfp.CreatedAt,
                                                                           CreatedBy = qtfp.CreatedBy,
                                                                           QcTransactionGroupFormParameter = (from qtfpr in _context.QcTransactionGroupFormParameters
                                                                                                              join it in _context.InputTypes on qtfpr.InputType equals it.TypeId
                                                                                                              join uom in _context.Uoms on qtfpr.Uom equals uom.UomId
                                                                                                              where qtfpr.QcTransactionGroupFormProcedureId == qtfp.Id
                                                                                                              && qtfpr.RowStatus == null
                                                                                                              select new QcTransactionGroupFormParameterViewModel
                                                                                                              {
                                                                                                                  Id = qtfpr.Id,
                                                                                                                  QcTransactionGroupFormProcedureId = qtfpr.QcTransactionGroupFormProcedureId,
                                                                                                                  Sequence = qtfpr.Sequence,
                                                                                                                  Label = qtfpr.Label,
                                                                                                                  Code = qtfpr.Code,
                                                                                                                  InputType = qtfpr.InputType,
                                                                                                                  InputTypeLabel = it.Label,
                                                                                                                  Reference = it.Reference,
                                                                                                                  ReferenceType = it.ReferenceType,
                                                                                                                  Uom = qtfpr.Uom,
                                                                                                                  UomLabel = uom.Label,
                                                                                                                  ThresholdOperator = qtfpr.ThresholdOperator,
                                                                                                                  ThresholdValue = qtfpr.ThresholdValue,
                                                                                                                  ThresholdValueTo = qtfpr.ThresholdValueTo,
                                                                                                                  ThresholdValueFrom = qtfpr.ThresholdValueFrom,
                                                                                                                  NeedAttachment = qtfpr.NeedAttachment,
                                                                                                                  Note = qtfpr.Note,
                                                                                                                  FormProcedureId = qtfpr.FormProcedureId,
                                                                                                                  IsForAllSample = qtfpr.IsForAllSample,
                                                                                                                  IsResult = qtfpr.IsResult,
                                                                                                                  DefaultValue = qtfpr.DefaultValue,
                                                                                                                  CreatedAt = qtfpr.CreatedAt,
                                                                                                                  CreatedBy = qtfpr.CreatedBy,
                                                                                                                  GroupValues = (from gv in _context.QcTransactionGroupValues
                                                                                                                                 where gv.QcTransactionGroupFormParameterId == qtfpr.Id
                                                                                                                                 && gv.RowStatus == null
                                                                                                                                 select new GroupValue
                                                                                                                                 {
                                                                                                                                     Id = gv.Id,
                                                                                                                                     Sequence = gv.Sequence,
                                                                                                                                     Value = gv.Value,
                                                                                                                                     AttchmentFile = gv.AttchmentFile,
                                                                                                                                     QcTransactionGroupFormMaterialId = gv.QcTransactionGroupFormMaterialId,
                                                                                                                                     QcTransactionGroupFormToolId = gv.QcTransactionGroupFormToolId
                                                                                                                                 }).OrderBy(x => x.Sequence).ToList(),
                                                                                                                  GroupSampleValues = (from gsv in _context.QcTransactionSampleValues
                                                                                                                                       join gs in _context.QcTransactionSamples on gsv.QcTransactionSampleId equals gs.Id
                                                                                                                                       join smpl in _context.QcSamples on gs.QcSampleId equals smpl.Id
                                                                                                                                       where gsv.QcTransactionGroupFormParameterId == qtfpr.Id
                                                                                                                                       && gsv.RowStatus == null
                                                                                                                                       select new GroupSampleValue
                                                                                                                                       {
                                                                                                                                           Id = gsv.Id,
                                                                                                                                           QcTransactionSampleId = gsv.QcTransactionSampleId,
                                                                                                                                           SampleCode = smpl.Code,
                                                                                                                                           Sequence = gsv.Sequence,
                                                                                                                                           Value = gsv.Value,
                                                                                                                                           AttchmentFile = gsv.AttchmentFile,
                                                                                                                                           QcTransactionGroupFormMaterialId = gsv.QcTransactionGroupFromMaterialId,
                                                                                                                                           QcTransactionGroupFormToolId = gsv.QcTransactionGroupFromToolId
                                                                                                                                       }).OrderBy(x => x.Sequence).ToList(),
                                                                                                              }).OrderBy(x => x.Sequence).ToList()
                                                                       }).OrderBy(x => x.Sequence).ToList(),

                                    QcTransactionGroupFormGeneral = (from qtfp in _context.QcTransactionGroupFormProcedures
                                                                     join qfs in _context.QcTransactionGroupFormSections on qtfp.QcTransactionGroupSectionId equals qfs.Id
                                                                     where qtfp.QcTransactionGroupProcessId == qtpC.Id
                                                                     && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_NOTE
                                                                     && qtfp.RowStatus == null
                                                                     select new QcTransactionGroupFormGeneralViewModel
                                                                     {
                                                                         Id = qtfp.Id,
                                                                         QcTransactionGroupProcessId = qtfp.QcTransactionGroupProcessId,
                                                                         Sequence = qtfp.Sequence,
                                                                         Description = qtfp.Description,
                                                                         FormProcedureId = qtfp.FormProcedureId,
                                                                         CreatedAt = qtfp.CreatedAt,
                                                                         CreatedBy = qtfp.CreatedBy,
                                                                         QcTransactionGroupFormParameter = (from qtfpr in _context.QcTransactionGroupFormParameters
                                                                                                            join it in _context.InputTypes on qtfpr.InputType equals it.TypeId
                                                                                                            join uom in _context.Uoms on qtfpr.Uom equals uom.UomId
                                                                                                            where qtfpr.QcTransactionGroupFormProcedureId == qtfp.Id
                                                                                                            && qtfpr.RowStatus == null
                                                                                                            select new QcTransactionGroupFormParameterViewModel
                                                                                                            {
                                                                                                                Id = qtfpr.Id,
                                                                                                                QcTransactionGroupFormProcedureId = qtfpr.QcTransactionGroupFormProcedureId,
                                                                                                                Sequence = qtfpr.Sequence,
                                                                                                                Label = qtfpr.Label,
                                                                                                                Code = qtfpr.Code,
                                                                                                                InputType = qtfpr.InputType,
                                                                                                                InputTypeLabel = it.Label,
                                                                                                                Reference = it.Reference,
                                                                                                                ReferenceType = it.ReferenceType,
                                                                                                                Uom = qtfpr.Uom,
                                                                                                                UomLabel = uom.Label,
                                                                                                                ThresholdOperator = qtfpr.ThresholdOperator,
                                                                                                                ThresholdValue = qtfpr.ThresholdValue,
                                                                                                                ThresholdValueTo = qtfpr.ThresholdValueTo,
                                                                                                                ThresholdValueFrom = qtfpr.ThresholdValueFrom,
                                                                                                                NeedAttachment = qtfpr.NeedAttachment,
                                                                                                                Note = qtfpr.Note,
                                                                                                                FormProcedureId = qtfpr.FormProcedureId,
                                                                                                                IsForAllSample = qtfpr.IsForAllSample,
                                                                                                                IsResult = qtfpr.IsResult,
                                                                                                                DefaultValue = qtfpr.DefaultValue,
                                                                                                                CreatedAt = qtfpr.CreatedAt,
                                                                                                                CreatedBy = qtfpr.CreatedBy,
                                                                                                                GroupValues = (from gv in _context.QcTransactionGroupValues
                                                                                                                               where gv.QcTransactionGroupFormParameterId == qtfpr.Id
                                                                                                                               && gv.RowStatus == null
                                                                                                                               select new GroupValue
                                                                                                                               {
                                                                                                                                   Id = gv.Id,
                                                                                                                                   Sequence = gv.Sequence,
                                                                                                                                   Value = gv.Value,
                                                                                                                                   AttchmentFile = gv.AttchmentFile,
                                                                                                                                   QcTransactionGroupFormMaterialId = gv.QcTransactionGroupFormMaterialId,
                                                                                                                                   QcTransactionGroupFormToolId = gv.QcTransactionGroupFormToolId
                                                                                                                               }).OrderBy(x => x.Sequence).ToList(),
                                                                                                                GroupSampleValues = (from gsv in _context.QcTransactionSampleValues
                                                                                                                                     join gs in _context.QcTransactionSamples on gsv.QcTransactionSampleId equals gs.Id
                                                                                                                                     join smpl in _context.QcSamples on gs.QcSampleId equals smpl.Id
                                                                                                                                     where gsv.QcTransactionGroupFormParameterId == qtfpr.Id
                                                                                                                                     && gsv.RowStatus == null
                                                                                                                                     select new GroupSampleValue
                                                                                                                                     {
                                                                                                                                         Id = gsv.Id,
                                                                                                                                         QcTransactionSampleId = gsv.QcTransactionSampleId,
                                                                                                                                         SampleCode = smpl.Code,
                                                                                                                                         Sequence = gsv.Sequence,
                                                                                                                                         Value = gsv.Value,
                                                                                                                                         AttchmentFile = gsv.AttchmentFile,
                                                                                                                                         QcTransactionGroupFormMaterialId = gsv.QcTransactionGroupFromMaterialId,
                                                                                                                                         QcTransactionGroupFormToolId = gsv.QcTransactionGroupFromToolId
                                                                                                                                     }).OrderBy(x => x.Sequence).ToList(),
                                                                                                            }).OrderBy(x => x.Sequence).ToList()
                                                                     }).OrderBy(x => x.Sequence).ToList(),

                                }).OrderBy(x => x.Sequence).ToList();


                    }


                }

                /* Get QC Result Data List */
                result.QcResult = await (from ts in _context.QcTransactionSamples
                                         join sm in _context.QcSamples on ts.QcSampleId equals sm.Id
                                         join s in _context.QcSamplings on sm.QcSamplingId equals s.Id
                                         join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                                         join ss in _context.QcSamplingShipments on s.Id equals ss.QcSamplingId
                                         where ts.QcTransactionGroupId == result.Id
                                         group new { s, r, ss } by new { r.Id } into g
                                         select new QcResultTestDetailViewModel
                                         {
                                             RequestId = g.Max(x => x.r.Id),
                                             NoBatch = g.Max(x => x.r.NoBatch),
                                             EmPhaseId = g.Max(x => x.r.EmPhaseId),
                                             EmPhaseName = g.Max(x => x.r.EmPhaseName),
                                             EmRoomId = g.Max(x => x.r.EmRoomId),
                                             EmRoomName = g.Max(x => x.r.EmRoomName),
                                             SamplingDateFrom = g.Max(x => x.s.SamplingDateFrom),
                                             SamplingDateTo = g.Max(x => x.s.SamplingDateTo),
                                             ShipmentStartDate = g.Max(x => x.ss.StartDate),
                                             ShipmentEndDate = g.Max(x => x.ss.EndDate)
                                         }).ToListAsync();

                if (result.QcResult.Any())
                {
                    foreach (var qRes in result.QcResult)
                    {
                        qRes.SamplingResult = (from ts1 in _context.QcTransactionSamples
                                               join sm1 in _context.QcSamples on ts1.QcSampleId equals sm1.Id
                                               join s1 in _context.QcSamplings on sm1.QcSamplingId equals s1.Id
                                               join res in _context.QcResults on sm1.Id equals res.SampleId
                                               join tsc in _context.TransactionTestScenario on sm1.TestScenarioId equals tsc.Id
                                               where ts1.QcTransactionGroupId == result.Id
                                               && s1.RequestQcsId == qRes.RequestId
                                               select new SamplingResult
                                               {
                                                   SampleId = res.SampleId,
                                                   SamplingPointId = sm1.SamplingPointId,
                                                   SamplingPointCode = sm1.SamplingPointCode,
                                                   SampleCode = sm1.Code,
                                                   TestScenarioId = sm1.TestScenarioId,
                                                   TestScenarioName = tsc.Name,
                                                   TestParamId = sm1.TestParamId,
                                                   TestParamName = sm1.TestParamName,
                                                   GradeRoomId = sm1.GradeRoomId,
                                                   GradeRoomName = sm1.GradeRoomName,
                                                   QcResultValue = res.Value,
                                                   TestVariableConclusion = res.TestVariableConclusion,
                                                   TestVariableId = res.TestVariableId,
                                                   Note = res.Note,
                                                   AttchmentFile = res.AttchmentFile
                                               }).ToList();
                    }
                }

                int newTestParamIndex = 1;
                var existingSamplingPointCode = new List<string>();
                var existingTestParamIndex = new List<int>();
                int samplingPointCodeIndex = 0;
                int testParamIndex = 0;
                foreach (var sampData in result.SamplesData)
                {
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
                }
            }
            return result;
        }

        private async Task<List<WorkflowHistoryQcSampling>> GetWorkflowHistoryByTransactionGroupId(int id)
        {
            string eDate = null;
            string eDate2 = null;

            var workflowTransactionGroupdataProvider = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IWorkflowQcTransactionGroupDataProvider>();
            var workflowServiceDataProvider = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IWorkflowServiceDataProvider>();

            List<WorkflowQcTransactionGroup> worfklowQcTransactionGroup =
                await workflowTransactionGroupdataProvider.GetByWorkflowByQcTransactionGroupId(id);

            List<WorkflowHistoryQcSampling> workflowQcTransactionGroupHistory = new List<WorkflowHistoryQcSampling>();


            foreach (var item in worfklowQcTransactionGroup)
            {
                DocumentHistoryResponseViewModel workflowHistory =
                    await workflowServiceDataProvider.GetListHistoryWorkflow(item.WorkflowDocumentCode);

                foreach (var itemHistory in workflowHistory.History)
                {
                    if (((itemHistory.StatusName != "Complete") &&
                         (item.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_1)))
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
                                DateTime = itemPIC.ActionDate == null ? eDate : itemPIC.ActionDate,
                                PersonalName = itemPIC.OrgName,
                                PersonalNik = itemPIC.OrgId,
                                Position = itemPIC.OrgPositionName,
                                ChangeStatusTime = itemPIC.ActionDate == null
                                    ? (actionUser == ApplicationConstant.WORKFLOW_STATUS_PENDING
                                        ? ApplicationConstant.MAX_DATETIME
                                        : eDate)
                                    : itemPIC.ActionDate
                            };

                            eDate = addWorkflowHistory.DateTime;

                            workflowQcTransactionGroupHistory.Add(addWorkflowHistory);
                        }
                    }
                    else if (item.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_2)
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
                                DateTime = itemPIC.ActionDate == null ? eDate2 : itemPIC.ActionDate,
                                PersonalName = itemPIC.OrgName,
                                PersonalNik = itemPIC.OrgId,
                                Position = itemPIC.OrgPositionName,
                                ChangeStatusTime = itemPIC.ActionDate == null
                                    ? (actionUser == ApplicationConstant.WORKFLOW_STATUS_PENDING
                                        ? ApplicationConstant.MAX_DATETIME
                                        : eDate2)
                                    : itemPIC.ActionDate
                            };
                            eDate2 = addWorkflowHistory.DateTime;

                            if (itemHistory.StatusName != ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_DRAFT ||
                                addWorkflowHistory.Action != ApplicationConstant.WORKFLOW_ACTION_SUBMIT_NAME)
                            {
                                workflowQcTransactionGroupHistory.Add(addWorkflowHistory);
                            }
                        }
                    }
                }
            }

            List<WorkflowHistoryQcSampling> workflowHistoryQcs =
                workflowQcTransactionGroupHistory.OrderByDescending(x => x.ChangeStatusTime).ToList();
            return workflowHistoryQcs;
        }

        private async Task<List<QcTransactionSampleRelationViewModel>> GetTestSampleListByTransactionGroupId(int id)
        {

            _logger.LogInformation("GetTestSampleListByTransactionGroupId");

            await using var context = _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<QcsProductContext>();

            var testSampleList = await (from ts in context.QcTransactionSamples
                                        join sm in context.QcSamples on ts.QcSampleId equals sm.Id
                                        join s in context.QcSamplings on sm.QcSamplingId equals s.Id
                                        join r in context.RequestQcs on s.RequestQcsId equals r.Id
                                        join tp in context.TransactionTestParameter on sm.TestParamId equals tp.Id
                                        join tsc in context.TransactionTestScenario on sm.TestScenarioId equals tsc.Id
                                        where ts.QcTransactionGroupId == id
                                        select new QcTransactionSampleRelationViewModel
                                        {
                                            Id = ts.Id,
                                            QcSampleId = ts.QcSampleId,
                                            SampleCode = sm.Code,
                                            NoBatch = r.NoBatch,
                                            NoRequest = r.NoRequest,
                                            EmRoomId = r.EmRoomId,
                                            EmRoomName = r.EmRoomName,
                                            GradeRoomId = sm.GradeRoomId,
                                            GradeRoomName = sm.GradeRoomName,
                                            GradeRoomCode = (sm.GradeRoomId != null ? (from mgr in context.TransactionGradeRoom
                                                                                       where mgr.Id == sm.GradeRoomId
                                                                                       select mgr.Code).FirstOrDefault() : null),
                                            EmPhaseId = r.EmPhaseId,
                                            EmPhaseName = r.EmPhaseName,
                                            TestParamId = sm.TestParamId,
                                            TestParamName = sm.TestParamName,
                                            TestParamShortName = tp.ShortName,
                                            TestParamSequence = GetTestParamSequenceByCode(tp.ShortName),
                                            PersonalId = sm.PersonalId,
                                            PersonalInitial = sm.PersonalInitial,
                                            PersonalName = sm.PersonalName,
                                            ShipmentStartDate = (from shp2 in context.QcSamplingShipments
                                                                 where shp2.QcSamplingId == s.Id
                                                                 select shp2.StartDate).FirstOrDefault(),
                                            ShipmentEndDate = (from shp3 in context.QcSamplingShipments
                                                               where shp3.QcSamplingId == s.Id
                                                               select shp3.EndDate).FirstOrDefault(),
                                            CreatedBy = ts.CreatedBy,
                                            CreatedAt = ts.CreatedAt,
                                            SamplingPointId = sm.SamplingPointId,
                                            SamplingPointCode = sm.SamplingPointCode,
                                            TestScenarioLabel = tsc.Label,
                                            TestScenarioCode = (tsc.Label == "at_rest" ? 0 : 1),
                                            TestVariableThreshold = (from tv in context.TransactionTestVariable
                                                                     join en in context.EnumConstant on tv.TresholdOperator equals en.TypeId
                                                                     join rtsp in context.TransactionRelTestScenarioParam on tv.TestParameterId equals rtsp.Id
                                                                     join ts in context.TransactionTestScenario on rtsp.TestScenarioId equals ts.Id
                                                                     join tp in context.TransactionTestParameter on rtsp.TestParameterId equals tp.Id
                                                                     where tp.Id == sm.TestParamId
                                                                     && ts.Id == sm.TestScenarioId
                                                                     && en.KeyGroup == "threshold_operator"
                                                                     select new TestVariableViewModel
                                                                     {
                                                                         Id = tv.Id,
                                                                         VariableName = tv.VariableName,
                                                                         Sequence = tv.Sequence.Value,
                                                                         TresholdOperator = tv.TresholdOperator,
                                                                         TresholdOperatorName = en.Name,
                                                                         TresholdValue = tv.TresholdValue,
                                                                         TresholdMin = tv.ThresholdValueFrom.Value,
                                                                         TresholdMax = tv.ThresholdValueTo.Value,
                                                                     }).ToList(),
                                            TestParamIndex = 1,
                                            SamplingDateTimeFrom = sm.SamplingDateTimeFrom,
                                            SamplingDateTimeTo = sm.SamplingDateTimeTo
                                        }).ToListAsync();

            foreach (var testSample in testSampleList)
            {
                if (testSample.SamplingPointCode.Contains('-'))
                {
                    testSample.FdLastSamplingPointCode = (testSample.TestParamId == 4 ? GetLastSamplingPointCode(testSample.SamplingPointCode) : null);
                    testSample.FirstSamplingPointCode = testSample.SamplingPointCode.Substring(0, testSample.SamplingPointCode.LastIndexOf('-'));
                    testSample.LastSamplingPointCode = GetLastSamplingPointCode(testSample.SamplingPointCode);
                }
            }

            testSampleList = testSampleList.OrderBy(x => x.TestParamSequence)
                .ThenBy(x => x.EmRoomName)
                .ThenBy(x => x.GradeRoomCode)
                .ThenBy(x => x.FdLastSamplingPointCode)
                .ThenBy(x => x.FirstSamplingPointCode)
                .ThenBy(x => x.LastSamplingPointCode)
                .ThenBy(x => x.TestScenarioCode)
                .ThenBy(x => x.SampleCode)
                .ThenBy(x => x.Id)
                .ToList();

            return testSampleList;
        }

        public async Task<List<SampleBatchQcProcessViewModel>> ListSampleBatchTestV2(int QcProcessId, string search, int RoomId, int PhaseId, DateTime? ReceiptStartDate, DateTime? ReceiptEndDate)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = await (from sp in _context.QcSamplings
                                join r in _context.RequestQcs on sp.RequestQcsId equals r.Id
                                where ((EF.Functions.Like(sp.Code.ToLower(), "%" + filter + "%")) ||
                                (EF.Functions.Like(r.NoBatch.ToLower(), "%" + filter + "%")))
                                && sp.RowStatus == null
                                && sp.SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_EMM
                                && (sp.Status == ApplicationConstant.STATUS_APPROVED || sp.Status == ApplicationConstant.STATUS_IN_REVIEW_KABAG)
                                select new SampleBatchQcProcessViewModel
                                {
                                    Id = sp.Id,
                                    BacthQrCode = sp.Code,
                                    NoBatch = r.NoBatch,
                                    PhaseId = r.EmPhaseId,
                                    PhaseName = r.EmPhaseName,
                                    RoomId = r.EmRoomId,
                                    RoomName = r.EmRoomName,
                                    ListRoom = (from rr in _context.RequestRooms
                                                where rr.QcRequestId == sp.RequestQcsId
                                                select rr).ToList(),
                                    ShipmentStatus = (from shp1 in _context.QcSamplingShipments
                                                      where shp1.QrCode == sp.Code
                                                      select shp1.Status).FirstOrDefault(),
                                    ShipmentStartDate = (from shp2 in _context.QcSamplingShipments
                                                         where shp2.QcSamplingId == sp.Id
                                                         select shp2.StartDate).FirstOrDefault(),
                                    ShipmentEndDate = (from shp3 in _context.QcSamplingShipments
                                                       where shp3.QcSamplingId == sp.Id
                                                       select shp3.EndDate).FirstOrDefault(),
                                    SamplingTestTransaction = (from qts in _context.QcTransactionSamplings
                                                               where qts.QcSamplingId == sp.Id
                                                               select qts).Count(),
                                    CreatedAt = sp.CreatedAt,
                                    CreatedBy = sp.CreatedBy,
                                    SampleListCount = (from s in _context.QcSamples
                                                       join tp in _context.TransactionTestParameter on s.TestParamId equals tp.Id
                                                       where s.QcSamplingId == sp.Id
                                                       && s.RowStatus == null
                                                       && tp.QcProcessId == QcProcessId
                                                       select new SampleListTestViewModel
                                                       {
                                                           Id = s.Id,
                                                           QrCode = s.Code,
                                                           TestParamId = s.TestParamId,
                                                           TestParamName = s.TestParamName,
                                                           TestParamShortName = tp.ShortName,
                                                           SamplingPointId = s.SamplingPointId,
                                                           SamplingPointCode = s.SamplingPointCode,
                                                           SamplingDateTimeFrom = s.SamplingDateTimeFrom,
                                                           SamplingDateTimeTo = s.SamplingDateTimeTo,
                                                           SampleTestTransaction = (from qts in _context.QcTransactionSamples
                                                                                    where qts.QcSampleId == s.Id
                                                                                    select qts).Count()
                                                       }).Where(w => w.SampleTestTransaction == 0).Count()

                                }).Where(x =>
                                    ((x.ShipmentStatus == ApplicationConstant.STATUS_SHIPMENT_RECEIVED) || (x.ShipmentStatus == ApplicationConstant.STATUS_SHIPMENT_LATE_RECIVED)) &&
                                    ((x.ShipmentEndDate >= ReceiptStartDate || !ReceiptStartDate.HasValue) &&
                                     (x.ShipmentEndDate <= ReceiptEndDate || !ReceiptEndDate.HasValue)) &&
                                    (x.SamplingTestTransaction == 0) &&
                                    (x.SampleListCount > 0) &&
                                    (x.RoomId == RoomId || RoomId == 0) &&
                                    (x.PhaseId == PhaseId || PhaseId == 0)
                                ).OrderByDescending(x => x.CreatedAt).ToListAsync();

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
                                }).ToListAsync();
            var gradeRoomIds = result.Select(x => x.GradeRoomId).Distinct().ToList();

            var testScenarios = await (from rgrs in _context.TransactionRelGradeRoomScenario
                                       join ts in _context.TransactionTestScenario on rgrs.TestScenarioId equals ts.Id
                                       where gradeRoomIds.Contains(rgrs.GradeRoomId)
                                       select new TestScenarioThresholdViewModel
                                       {
                                           GradeRoomId = rgrs.GradeRoomId,
                                           TestScenarioId = ts.Id,
                                           TestScenarioName = ts.Name,
                                           TestScenarioLabel = ts.Label,
                                           TestParameterThreshold = (from tp in _context.TransactionTestParameter
                                                                     where tp.TestGroupId != ApplicationConstant.SAMPLING_TYPE_ID_EMP
                                                                     orderby tp.Sequence
                                                                     select new TestParameterThresholdViewModel
                                                                     {
                                                                         TestParameterId = tp.Id,
                                                                         TestParameterName = tp.Name,
                                                                         TestParameterShort = tp.ShortName,
                                                                         TestGroupId = tp.TestGroupId,
                                                                         Sequence = tp.Sequence,
                                                                     }).Where(x => x.TestGroupId == testGroupId || testGroupId == null).ToList()

                                       }).Where(x => x.TestScenarioLabel == testScenarioLabel || testScenarioLabel == null).ToListAsync();

            var testScenarioIds = testScenarios.Select(x => x.TestScenarioId).Distinct().ToList();
            var testParameterIds = new List<int>();
            foreach (var itemTestScenario in testScenarios)
            {
                testParameterIds.AddRange(itemTestScenario.TestParameterThreshold
                    .Select(x => x.TestParameterId).Distinct().ToList());
            }
            testParameterIds = testParameterIds.Distinct().ToList();

            var relTestScenarioParams = await (from rtsp in _context.TransactionRelTestScenarioParam
                                               where testScenarioIds.Contains(rtsp.TestScenarioId)
                                               && testParameterIds.Contains(rtsp.TestParameterId)
                                               select rtsp).Distinct().ToListAsync();
            var relTestScenarioParamIds = relTestScenarioParams.Select(x => x.Id).Distinct().ToList();

            var testVariableThresholds = await (from tv in _context.TransactionTestVariable
                                                join en in _context.EnumConstant on tv.TresholdOperator equals en.TypeId
                                                where relTestScenarioParamIds.Contains(tv.TestParameterId)
                                                && en.KeyGroup == "threshold_operator"
                                                && tv.RowStatus == null
                                                orderby tv.Sequence
                                                select new TestVariableThresholdViewModel
                                                {
                                                    TestVariableId = tv.Id,
                                                    TestParameterId = tv.TestParameterId,
                                                    VariableName = tv.VariableName,
                                                    Sequence = tv.Sequence.Value,
                                                    TresholdOperator = tv.TresholdOperator,
                                                    TresholdOperatorName = en.Name,
                                                    TresholdValue = tv.TresholdValue,
                                                    TresholdMin = tv.ThresholdValueFrom.Value,
                                                    TresholdMax = tv.ThresholdValueTo.Value,
                                                }).ToListAsync();

            foreach (var item in result)
            {
                item.TestScenario = testScenarios.Where(x => x.GradeRoomId == item.GradeRoomId).ToList();

                foreach (var itemTestScenario in item.TestScenario)
                {
                    foreach (var itemParamTreshold in itemTestScenario.TestParameterThreshold)
                    {
                        //get rtsp sesuai testScenarioId dan testParameterId
                        var selectedRelTestScenarioParams = relTestScenarioParams
                            .Where(x => x.TestScenarioId == itemTestScenario.TestScenarioId
                                && x.TestParameterId == itemParamTreshold.TestParameterId)
                            .ToList();
                        var selectedRtspIds = selectedRelTestScenarioParams.Select(x => x.Id).Distinct().ToList();

                        itemParamTreshold.TestVariableThreshold = testVariableThresholds
                            .Where(x => selectedRtspIds.Contains(x.TestParameterId))
                            .ToList();
                    }
                }
            }

            return result;
        }

        public async Task<RequestQcs> GetByTransactionGroupSampling(int transactionGroupId)
        {
            var result = await (from qr in _context.RequestQcs
                                join qs in _context.QcSamplings on qr.Id equals qs.RequestQcsId
                                join qts in _context.QcTransactionSamplings on qs.Id equals qts.QcSamplingId
                                join qtg in _context.QcTransactionGroups on qts.QcTransactionGroupId equals qtg.Id
                                where qtg.Id == transactionGroupId
                                select qr).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<int>> ListRequestIdTestingComplete(List<int> requestId)
        {
            return await ((from qt in _context.QcTransactionGroups
                           join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                           join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                           join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                           join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                           where wt.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                           && requestId.Contains(sq.RequestQcsId)
                           && wt.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_2
                           orderby wt.UpdatedAt descending
                           select new
                           {
                               IsInWorkflow = wt.IsInWorkflow,
                               RequestQcsId = sq.RequestQcsId
                           }).AsQueryable()).Where(x => x.IsInWorkflow == false).Select(x => x.RequestQcsId).ToListAsync();
        }

        public async Task<List<int>> ListRequestIdTestingNotComplete(List<int> requestId, string workflowStatus)
        {
            return await ((from qt in _context.QcTransactionGroups
                           join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                           join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                           join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                           join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                           where wt.WorkflowStatus == workflowStatus
                           && requestId.Contains(sq.RequestQcsId)
                           && wt.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_2
                           orderby wt.UpdatedAt descending
                           select new
                           {
                               IsInWorkflow = wt.IsInWorkflow,
                               RequestQcsId = sq.RequestQcsId
                           }).AsQueryable()).Where(x => x.IsInWorkflow == true).Select(x => x.RequestQcsId).ToListAsync();
        }

        public async Task<QcTransactionGroup> GetTestDataByRequestId(int qcsRequestId)
        {
            return await
            (
                from trg in _context.QcTransactionGroups
                join trgs in _context.QcTransactionSamplings on trg.Id equals trgs.QcTransactionGroupId
                join qs in _context.QcSamplings on trgs.QcSamplingId equals qs.Id
                join qr in _context.RequestQcs on qs.RequestQcsId equals qr.Id
                where qr.Id == qcsRequestId
                select trg
            ).Distinct().FirstOrDefaultAsync();
        }

        public async Task<QcTransactionGroupDetailViewModel> GetTransactionGroupProcessById(int id)
        {
            _logger.LogInformation("GetTransactionGroupProcessById");

            QcTransactionGroupProcessRelViewModel qcProcess = null;
            QcTransactionGroupDetailViewModel result = null;

            try
            {
                qcProcess = await (from qtp in _context.QcTransactionGroupProcesses
                                   where qtp.Id == id
                                         && qtp.RowStatus == null
                                   select new QcTransactionGroupProcessRelViewModel
                                   {
                                       Id = qtp.Id,
                                       QcTransactionGroupId = qtp.QcTransactionGroupId,
                                       Sequence = qtp.Sequence,
                                       Name = qtp.Name,
                                       ParentId = qtp.ParentId,
                                       RoomId = qtp.RoomId,
                                       IsInputForm = qtp.IsInputForm,
                                       QcProcessId = qtp.QcProcessId,
                                       CreatedAt = qtp.CreatedAt,
                                       CreatedBy = qtp.CreatedBy,
                                   }).FirstOrDefaultAsync();

                if (qcProcess == null)
                {
                    throw new Exception($"Transaction group process with ID {id} not found");
                }

                result = new QcTransactionGroupDetailViewModel();

                var taskGetFormMaterial = GetTransactionGroupFormMaterial(id);
                var taskGetFormTool = GetTransactionGroupFormTool(id);
                var taskGetFormProcedure = GetTransactionGroupFormProcedure(id);
                var taskGetFormGeneral = GetQcTransactionGroupFormGeneral(id);
                var taskGetWorkflowHistory = GetWorkflowHistoryByTransactionGroupId(qcProcess.QcTransactionGroupId);
                var taskGetTestSample = GetTestSampleListByTransactionGroupId(qcProcess.QcTransactionGroupId);

                await Task.WhenAll(taskGetFormMaterial, taskGetFormTool, taskGetFormProcedure, taskGetFormGeneral, taskGetWorkflowHistory, taskGetTestSample);

                qcProcess.QcTransactionGroupFormMaterial = taskGetFormMaterial.Result;
                qcProcess.QcTransactionGroupFormTool = taskGetFormTool.Result;
                qcProcess.QcTransactionGroupFormProcedure = taskGetFormProcedure.Result;
                qcProcess.QcTransactionGroupFormGeneral = taskGetFormGeneral.Result;
                qcProcess.WorkflowHistory = taskGetWorkflowHistory.Result;
                result.QcTransactionGroupProcess = qcProcess;
                result.SamplesData = taskGetTestSample.Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
            }

            return result;
        }

        private async Task<List<QcTransactionGroupFormMaterialViewModel>> GetTransactionGroupFormMaterial(int transactionGroupProcessId)
        {
            await using var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<QcsProductContext>();

            var formMaterials = await (from qtm in context.QcTransactionGroupFormMaterials
                                       join qfs in context.QcTransactionGroupFormSections on qtm
                                           .QcTransactionGroupSectionId equals qfs.Id
                                       join uom in context.Uoms on qtm.Uom equals uom.UomId
                                       join uomPackage in context.Uoms on qtm.UomPackage equals uomPackage.UomId
                                       where qtm.QcTransactionGroupProcessId == transactionGroupProcessId
                                             && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_MATERIAL
                                             && qtm.RowStatus == null
                                       select new QcTransactionGroupFormMaterialViewModel
                                       {
                                           Id = qtm.Id,
                                           QcTransactionGroupProcessId = qtm.QcTransactionGroupProcessId,
                                           Sequence = qtm.Sequence,
                                           ItemId = qtm.ItemId,
                                           Code = qtm.Code,
                                           Name = qtm.Name,
                                           DefaultPackageQty = qtm.DefaultPackageQty,
                                           UomPackage = qtm.UomPackage,
                                           UomUomPackageLabel = uomPackage.Label,
                                           DefaultQty = qtm.DefaultQty,
                                           Uom = qtm.Uom,
                                           UomLabel = uom.Label,
                                           QcProcessId = qtm.QcProcessId,
                                           GroupName = qtm.GroupName,
                                           CreatedAt = qtm.CreatedAt,
                                           CreatedBy = qtm.CreatedBy
                                       })
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            return formMaterials;
        }

        private async Task<List<QcTransactionGroupFormToolViewModel>> GetTransactionGroupFormTool(int transactionGroupProcessId)
        {
            await using var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<QcsProductContext>();

            var formTools = await (from qtt in context.QcTransactionGroupFormTools
                                   join qfs in context.QcTransactionGroupFormSections on qtt
                                       .QcTransactionGroupSectionId equals qfs.Id
                                   where qtt.QcTransactionGroupProcessId == transactionGroupProcessId
                                         && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_TOOL
                                         && qtt.RowStatus == null
                                   select new QcTransactionGroupFormToolViewModel
                                   {
                                       Id = qtt.Id,
                                       QcTransactionGroupProcessId = qtt.QcTransactionGroupProcessId,
                                       Sequence = qtt.Sequence,
                                       ToolId = qtt.ToolId,
                                       ItemId = qtt.ItemId,
                                       Code = qtt.Code,
                                       Name = qtt.Name,
                                       Quantity = qtt.Quantity,
                                       QcProcessId = qtt.QcProcessId,
                                       CreatedAt = qtt.CreatedAt,
                                       CreatedBy = qtt.CreatedBy
                                   })
                .OrderBy(x => x.Sequence)
                .ToListAsync();
            return formTools;
        }

        private async Task<List<QcTransactionGroupFormProcedureViewModel>> GetTransactionGroupFormProcedure(
            int transactionGroupProcessId)
        {
            await using var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<QcsProductContext>();

            var formProcedures = await (from qtfp in context.QcTransactionGroupFormProcedures
                                        join qfs in context.QcTransactionGroupFormSections on qtfp
                                            .QcTransactionGroupSectionId equals qfs.Id
                                        where qtfp.QcTransactionGroupProcessId == transactionGroupProcessId
                                              && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_PROCEDURE
                                              && qtfp.RowStatus == null
                                        select new QcTransactionGroupFormProcedureViewModel
                                        {
                                            Id = qtfp.Id,
                                            QcTransactionGroupProcessId = qtfp.QcTransactionGroupProcessId,
                                            Sequence = qtfp.Sequence,
                                            Description = qtfp.Description,
                                            FormProcedureId = qtfp.FormProcedureId,
                                            CreatedAt = qtfp.CreatedAt,
                                            CreatedBy = qtfp.CreatedBy,

                                        })
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            if (!formProcedures.Any())
            {
                return formProcedures;
            }

            var formProcedureIds = formProcedures.Select(x => x.Id).Distinct().ToList();

            #region get transaction group value by form procedure ids

            var groupValues = await (from gv in context.QcTransactionGroupValues
                                     join fp in context.QcTransactionGroupFormParameters on gv.QcTransactionGroupFormParameterId equals fp.Id
                                     where formProcedureIds.Contains(fp.QcTransactionGroupFormProcedureId)
                                           && gv.RowStatus == null
                                     select new GroupValue
                                     {
                                         Id = gv.Id,
                                         Sequence = gv.Sequence,
                                         Value = gv.Value,
                                         AttchmentFile = gv.AttchmentFile,
                                         QcTransactionGroupFormMaterialId = gv.QcTransactionGroupFormMaterialId,
                                         QcTransactionGroupFormToolId = gv.QcTransactionGroupFormToolId,
                                         QcTransactionGroupFormParameterId = gv.QcTransactionGroupFormParameterId
                                     })
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            #endregion

            #region get transaction sample value by form procedure ids

            var sampleValues = await (from gsv in context.QcTransactionSampleValues
                                      join fp in context.QcTransactionGroupFormParameters on gsv.QcTransactionGroupFormParameterId
                                          equals fp.Id
                                      join gs in context.QcTransactionSamples on gsv.QcTransactionSampleId equals gs.Id
                                      join smpl in context.QcSamples on gs.QcSampleId equals smpl.Id
                                      where formProcedureIds.Contains(fp.QcTransactionGroupFormProcedureId)
                                            && gsv.RowStatus == null
                                      select new GroupSampleValue
                                      {
                                          Id = gsv.Id,
                                          QcTransactionSampleId = gsv.QcTransactionSampleId,
                                          SampleCode = smpl.Code,
                                          Sequence = gsv.Sequence,
                                          Value = gsv.Value,
                                          AttchmentFile = gsv.AttchmentFile,
                                          QcTransactionGroupFormMaterialId = gsv.QcTransactionGroupFromMaterialId,
                                          QcTransactionGroupFormToolId = gsv.QcTransactionGroupFromToolId,
                                          QcTransactionGroupFormParameterId = gsv.QcTransactionGroupFormParameterId
                                      })
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            #endregion

            #region get transaction group parameter by form procedure ids

            var formParameters = await
                (from qtfpr in context.QcTransactionGroupFormParameters
                 join it in context.InputTypes on qtfpr.InputType equals it.TypeId
                 join uom in context.Uoms on qtfpr.Uom equals uom.UomId
                 where formProcedureIds.Contains(qtfpr.QcTransactionGroupFormProcedureId)
                       && qtfpr.RowStatus == null
                 select new QcTransactionGroupFormParameterViewModel
                 {
                     Id = qtfpr.Id,
                     QcTransactionGroupFormProcedureId = qtfpr.QcTransactionGroupFormProcedureId,
                     Sequence = qtfpr.Sequence,
                     Label = qtfpr.Label,
                     Code = qtfpr.Code,
                     InputType = qtfpr.InputType,
                     InputTypeLabel = it.Label,
                     Reference = it.Reference,
                     ReferenceType = it.ReferenceType,
                     Uom = qtfpr.Uom,
                     UomLabel = uom.Label,
                     ThresholdOperator = qtfpr.ThresholdOperator,
                     ThresholdValue = qtfpr.ThresholdValue,
                     ThresholdValueTo = qtfpr.ThresholdValueTo,
                     ThresholdValueFrom = qtfpr.ThresholdValueFrom,
                     NeedAttachment = qtfpr.NeedAttachment,
                     Note = qtfpr.Note,
                     FormProcedureId = qtfpr.FormProcedureId,
                     IsForAllSample = qtfpr.IsForAllSample,
                     IsResult = qtfpr.IsResult,
                     DefaultValue = qtfpr.DefaultValue,
                     CreatedAt = qtfpr.CreatedAt,
                     CreatedBy = qtfpr.CreatedBy
                 })
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            foreach (var formParameter in formParameters)
            {
                formParameter.GroupValues = groupValues
                    .Where(x => x.QcTransactionGroupFormParameterId == formParameter.Id)
                    .OrderBy(x => x.Sequence).ToList();

                formParameter.GroupSampleValues = sampleValues
                    .Where(x => x.QcTransactionGroupFormParameterId == formParameter.Id)
                    .OrderBy(x => x.Sequence).ToList();
            }

            #endregion

            #region set transaction group parameter to form procedure

            foreach (var formProcedure in formProcedures)
            {
                formProcedure.QcTransactionGroupFormParameter = formParameters
                    .Where(x => x.QcTransactionGroupFormProcedureId == formProcedure.Id).ToList();
            }

            #endregion

            return formProcedures;
        }

        private async Task<List<QcTransactionGroupFormGeneralViewModel>> GetQcTransactionGroupFormGeneral(
            int transactionGroupProcessId)
        {

            await using var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<QcsProductContext>();

            var formGenerals = await (from qtfp in context.QcTransactionGroupFormProcedures
                                      join qfs in context.QcTransactionGroupFormSections on qtfp
                                          .QcTransactionGroupSectionId equals qfs.Id
                                      where qtfp.QcTransactionGroupProcessId == transactionGroupProcessId
                                            && qfs.SectionTypeId == ApplicationConstant.SECTION_TYPE_ID_NOTE
                                            && qtfp.RowStatus == null
                                      select new QcTransactionGroupFormGeneralViewModel
                                      {
                                          Id = qtfp.Id,
                                          QcTransactionGroupProcessId = qtfp.QcTransactionGroupProcessId,
                                          Sequence = qtfp.Sequence,
                                          Description = qtfp.Description,
                                          FormProcedureId = qtfp.FormProcedureId,
                                          CreatedAt = qtfp.CreatedAt,
                                          CreatedBy = qtfp.CreatedBy,
                                      })
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            if (!formGenerals.Any())
            {
                return formGenerals;
            }

            var formGeneralIds = formGenerals.Select(x => x.Id).Distinct().ToList();

            #region get transaction group value by form procedure ids

            var groupValues = await (from gv in context.QcTransactionGroupValues
                                     join fp in context.QcTransactionGroupFormParameters on gv.QcTransactionGroupFormParameterId equals fp.Id
                                     where formGeneralIds.Contains(fp.QcTransactionGroupFormProcedureId)
                                           && gv.RowStatus == null
                                     select new GroupValue
                                     {
                                         Id = gv.Id,
                                         Sequence = gv.Sequence,
                                         Value = gv.Value,
                                         AttchmentFile = gv.AttchmentFile,
                                         QcTransactionGroupFormMaterialId = gv.QcTransactionGroupFormMaterialId,
                                         QcTransactionGroupFormToolId = gv.QcTransactionGroupFormToolId,
                                         QcTransactionGroupFormParameterId = gv.QcTransactionGroupFormParameterId
                                     })
                .OrderBy(x => x.Sequence)
                .ToListAsync();
            #endregion

            #region get transaction sample value by form procedure ids

            var sampleValues = await (from gsv in context.QcTransactionSampleValues
                                      join fp in context.QcTransactionGroupFormParameters on gsv.QcTransactionGroupFormParameterId
                                          equals fp.Id
                                      join gs in context.QcTransactionSamples on gsv.QcTransactionSampleId equals gs.Id
                                      join smpl in context.QcSamples on gs.QcSampleId equals smpl.Id
                                      where formGeneralIds.Contains(fp.QcTransactionGroupFormProcedureId)
                                            && gsv.RowStatus == null
                                      select new GroupSampleValue
                                      {
                                          Id = gsv.Id,
                                          QcTransactionSampleId = gsv.QcTransactionSampleId,
                                          SampleCode = smpl.Code,
                                          Sequence = gsv.Sequence,
                                          Value = gsv.Value,
                                          AttchmentFile = gsv.AttchmentFile,
                                          QcTransactionGroupFormMaterialId = gsv.QcTransactionGroupFromMaterialId,
                                          QcTransactionGroupFormToolId = gsv.QcTransactionGroupFromToolId,
                                          QcTransactionGroupFormParameterId = gsv.QcTransactionGroupFormParameterId
                                      })
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            #endregion

            #region get transaction group parameter by form procedure ids

            var formParameters = await (from qtfpr in context.QcTransactionGroupFormParameters
                                        join it in context.InputTypes on qtfpr.InputType equals it.TypeId
                                        join uom in context.Uoms on qtfpr.Uom equals uom.UomId
                                        where formGeneralIds.Contains(qtfpr.QcTransactionGroupFormProcedureId)
                                              && qtfpr.RowStatus == null
                                        select new QcTransactionGroupFormParameterViewModel
                                        {
                                            Id = qtfpr.Id,
                                            QcTransactionGroupFormProcedureId = qtfpr.QcTransactionGroupFormProcedureId,
                                            Sequence = qtfpr.Sequence,
                                            Label = qtfpr.Label,
                                            Code = qtfpr.Code,
                                            InputType = qtfpr.InputType,
                                            InputTypeLabel = it.Label,
                                            Reference = it.Reference,
                                            ReferenceType = it.ReferenceType,
                                            Uom = qtfpr.Uom,
                                            UomLabel = uom.Label,
                                            ThresholdOperator = qtfpr.ThresholdOperator,
                                            ThresholdValue = qtfpr.ThresholdValue,
                                            ThresholdValueTo = qtfpr.ThresholdValueTo,
                                            ThresholdValueFrom = qtfpr.ThresholdValueFrom,
                                            NeedAttachment = qtfpr.NeedAttachment,
                                            Note = qtfpr.Note,
                                            FormProcedureId = qtfpr.FormProcedureId,
                                            IsForAllSample = qtfpr.IsForAllSample,
                                            IsResult = qtfpr.IsResult,
                                            DefaultValue = qtfpr.DefaultValue,
                                            CreatedAt = qtfpr.CreatedAt,
                                            CreatedBy = qtfpr.CreatedBy
                                        })
                .OrderBy(x => x.Sequence)
                .ToListAsync();

            foreach (var formParameter in formParameters)
            {
                formParameter.GroupValues = groupValues
                    .Where(x => x.QcTransactionGroupFormParameterId == formParameter.Id)
                    .OrderBy(x => x.Sequence).ToList();

                formParameter.GroupSampleValues = sampleValues
                    .Where(x => x.QcTransactionGroupFormParameterId == formParameter.Id)
                    .OrderBy(x => x.Sequence).ToList();
            }

            #endregion

            #region set transaction group parameter to form procedure

            foreach (var formGeneral in formGenerals)
            {
                formGeneral.QcTransactionGroupFormParameter = formParameters
                    .Where(x => x.QcTransactionGroupFormProcedureId == formGeneral.Id)
                    .ToList();
            }
            #endregion

            return formGenerals;
        }

        private async Task<string> _GetNameByNik(string nik)
        {
            var name = "";
            var dataEmployee = await _bioHrBusinessProvider.GetEmployeeByNewNik(nik);
            name = dataEmployee.Name;
            return name;
        }

        public async Task<List<QcTransactionGroupViewModel>> GetPendingReview(string workflowStatus)
        {
            var result = new List<QcTransactionGroupViewModel>();

            var inReviewStatusList = new List<string>()
            {
                ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KASIE,
                ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG,
                ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG_QC,
                ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA,
            };

            var qcTestList = await (from s in _context.QcTransactionGroups
                                    where s.RowStatus == null
                                          && inReviewStatusList.Contains(s.WorkflowStatus)
                                          && s.WorkflowStatus == workflowStatus
                                    select new QcTransactionGroupViewModel()
                                    {
                                        Id = s.Id,
                                        Status = s.Status,
                                        CreatedAt = s.CreatedAt,
                                        TestDate = s.TestDate,
                                        Code = s.Code,
                                        QcProcessName = s.QcProcessName
                                    }).ToListAsync();

            var qcTestIds = qcTestList.Select(x => x.Id).Distinct().ToList();

            var workflowQcTestList = await _workflowTransactionGroupdataProvider.GetInWorkflowByTransactionGroupIds(qcTestIds);

            foreach (var qcTest in qcTestList)
            {
                var workflowQcTest = workflowQcTestList.FirstOrDefault(x => x.QcTransactionGroupId == qcTest.Id);

                if (workflowQcTest == null)
                {
                    continue;
                }

                qcTest.WorkflowDocumentCode = workflowQcTest.WorkflowDocumentCode;

                result.Add(qcTest);
            }

            return result;
        }

    }
}
