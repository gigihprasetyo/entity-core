using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using qcs_product.API.WorkflowModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class MonitoringDataProvider : IMonitoringDataProvider
    {

        private readonly QcsProductContext _context;
        private readonly ILogger<MonitoringDataProvider> _logger;
        private readonly IWorkflowServiceDataProvider _workflowServiceDataProvider;
        private readonly IQcSamplingDataProvider _qcSamplingDataProvider;
        private readonly IQcTestDataProvider _qcTestDataProvider;
        private readonly IQcRequestDataProvider _qcQcRequestDataProvider;

        [ExcludeFromCodeCoverage]
        public MonitoringDataProvider(QcsProductContext context,
        IWorkflowServiceDataProvider workflowServiceDataProvider,
        IQcSamplingDataProvider qcSamplingDataProvider,
        IQcTestDataProvider qcTestDataProvider,
        ILogger<MonitoringDataProvider> logger,
        IQcRequestDataProvider qcQcRequestDataProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _workflowServiceDataProvider = workflowServiceDataProvider ?? throw new ArgumentNullException(nameof(workflowServiceDataProvider));
            _qcSamplingDataProvider = qcSamplingDataProvider ?? throw new ArgumentNullException(nameof(qcSamplingDataProvider));
            _qcTestDataProvider = qcTestDataProvider ?? throw new ArgumentNullException(nameof(qcTestDataProvider));
            _qcQcRequestDataProvider = qcQcRequestDataProvider ?? throw new ArgumentNullException(nameof(qcQcRequestDataProvider));
            _logger = logger;
        }


        public async Task<Personal> GetPersonalById(Int32 Id)
        {
            return await (from p in _context.Personals
                          where p.Id == Id
                          select p).FirstOrDefaultAsync();
        }

        public async Task<List<MonitoringListViewModel>> ListShort(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, List<int> typeRequestId, int orgId, string nik, int? facilityId)
        {
            var result = new List<MonitoringListViewModel>();
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var query = (from p in _context.RequestQcs
                join tb in _context.TransactionBatches on p.Id equals tb.RequestQcsId into tbGroup
                from tb in tbGroup.DefaultIfEmpty()
                join tbl in _context.TransactionBatchLines on tb.Id equals tbl.TrsBatchId into tblGroup
                from tbl in tblGroup.DefaultIfEmpty()
                where ((EF.Functions.Like(p.NoRequest.ToLower(), "%" + filter + "%")) ||
                    (EF.Functions.Like(p.NoBatch.ToLower(), "%" + filter + "%")) ||
                    (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")) ||
                    EF.Functions.Like(tbl.NoBatch.ToLower(), "%" + filter + "%")
                )
                && status.Contains(p.Status)
                && typeRequestId.Contains(p.TypeRequestId)
                && p.RowStatus == null
                && (
                    (
                        p.Date >= startDate || !startDate.HasValue
                    ) &&
                    (
                        p.Date <= endDate || !endDate.HasValue
                    ) &&
                    (
                        p.OrgId == orgId || orgId == 0
                    )
                )
                orderby p.CreatedAt descending
                select new MonitoringListViewModel
                {
                    Id = p.Id,
                    Date = p.Date,
                    ReceiptDate = p.ReceiptDate,
                    ReceiptDateQA = p.ReceiptDateQA,
                    ReceiptDateKabag = p.ReceiptDateKabag,
                    NoRequest = p.NoRequest,
                    TypeRequestId = p.TypeRequestId,
                    TypeRequest = p.TypeRequest,
                    NoBatch = p.NoBatch,
                    ItemId = p.ItemId,
                    ItemName = p.ItemName,
                    TypeFormId = p.TypeFormId,
                    TypeFormName = p.TypeFormName,
                    Status = p.Status,
                    NoDeviation = p.NoDeviation,
                    //Conclusion = p.Conclusion,
                    ConclusionTemp = p.Conclusion,
                    WorkflowStatus = p.WorkflowStatus,
                    FacilityId = p.FacilityId,
                    OrgId = p.OrgId,
                    OrgName = p.OrgName,
                    CreatedAt = p.CreatedAt,
                    CreatedBy = p.CreatedBy,
                    Sampling = (from qrs in _context.QcSamplings
                                where p.Id == qrs.RequestQcsId
                                select new MonitoringSamplingListViewModel
                                {
                                    SamplingId = qrs.Id,
                                    Code = qrs.Code,
                                    // check sample receive date
                                    //   ReceiptDate = (from qss in _context.QcSamplingShipments
                                    //                  join qst in _context.QcSamplingShipmentTrackers on qss.Id equals qst.QcSamplingShipmentId
                                    //                  where qss.QcSamplingId == qrs.Id
                                    //                  && qst.Type == ApplicationConstant.TRACKER_TYPE_RECEIVE
                                    //                  select qst.processAt
                                    //                   ).FirstOrDefault(),
                                    ReceiptDate = qrs.ReceiptDate,
                                    NoRequest = p.NoRequest,
                                    TypeRequestId = p.TypeRequestId,
                                    TypeRequest = p.TypeRequest,
                                    SamplingTypeId = qrs.SamplingTypeId,
                                    SamplingTypeName = qrs.SamplingTypeName,
                                    Process = ApplicationConstant.PROCESS_STATUS_PHASE_REQUEST,
                                    Status = p.Status,
                                    StatusRequest = p.Status,
                                    StatusSampling = qrs.Status,
                                    StatusTransfer = null,
                                    StatusTesting = null,
                                    // process phase, cek di workflow masing2 phase, mundur dari step paling akhir dulu
                                    // TODO: baru sampe transfer, nunggu implemen testing
                                }).ToList()

                }).AsQueryable();

            if ((facilityId != null) && (facilityId != 0))
            {
                query = query.Where(x => x.FacilityId == facilityId);
            }

            result = await query.Distinct().ToListAsync();

            var resultDataByNik = new List<MonitoringListViewModel>();
            var resultList = new List<MonitoringListViewModel>();
            var workflowStatus = "";


            if (nik != null)
            {
                var listPendingWorkflow = await _workflowServiceDataProvider.GetListPendingByNik(nik);
                var listRequestIdFromSampling = new List<int>();
                var listRequestIdFromTesting = new List<int>();

                var resultToDo = new List<MonitoringListViewModel>();

                if (listPendingWorkflow.ListPending.Any())
                {
                    workflowStatus = listPendingWorkflow.ListPending.Last().StatusName;

                    var getRequestIdByWorkflowCodeSampling = await _qcSamplingDataProvider.GetAllRequestIdByWorkflowCode(listPendingWorkflow.ListPending, workflowStatus);
                    listRequestIdFromSampling.AddRange(getRequestIdByWorkflowCodeSampling);

                    var getRequestIdByWorkflowCodeSamplingTesting = await _qcTestDataProvider.GetAllRequestIdByWorkflowCode(listPendingWorkflow.ListPending, workflowStatus);
                    listRequestIdFromTesting.AddRange(getRequestIdByWorkflowCodeSamplingTesting);
                }

                foreach (var item2 in listRequestIdFromSampling.Distinct().ToList().GroupBy(x => x))
                {
                    if (listRequestIdFromTesting.GroupBy(x => x).Where(y => y.Key == item2.Key).Any())
                    {
                        var resData = result.FirstOrDefault(x => x.Id == item2.Key);
                        if (resData != null)
                        {
                            resultToDo.Add(resData);
                        }
                    }
                }

                //cek jika review kabag ada kah data yang di QA 
                if (workflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG)
                {
                    var listRequestIdFromSamplingInQA = new List<int>();
                    var listRequestIdFromTestingInQA = new List<int>();
                    var listRequestInQA = new List<int>();

                    //get data testing in review QA
                    var getRequestIdSamplingTestingInReviewQa = await _qcTestDataProvider.GetRequestIdSamplingTestingInReviewQa();
                    if (getRequestIdSamplingTestingInReviewQa != null)
                    {
                        foreach (var item in getRequestIdSamplingTestingInReviewQa.GroupBy(x => x))
                        {
                            listRequestIdFromTestingInQA.Add(item.Key);
                        }
                    }
                    //get data sampling in review QA
                    var getRequestIdSamplingInReviewQa = await _qcSamplingDataProvider.GetRequestIdSamplingInReviewQa();
                    if (getRequestIdSamplingInReviewQa != null)
                    {
                        foreach (var item in getRequestIdSamplingInReviewQa.GroupBy(x => x))
                        {
                            listRequestIdFromSamplingInQA.Add(item.Key);
                        }
                    }

                    //handle after complete//
                    //get data sampling has complete in phase 2
                    var requestIdBySamplingHasCompleteInPhase2 = await _qcSamplingDataProvider.ListRequestIdSamplingComplete(listRequestIdFromSampling.Distinct().ToList());
                    //masukan data ke variable list request sebelumnya
                    listRequestIdFromTestingInQA.AddRange(requestIdBySamplingHasCompleteInPhase2);
                    //end handle after complete//
                    //get data request id dari data testing yang sudah complete phase 2
                    var requestIdByTestingHasCompleteInPhase2 = await _qcTestDataProvider.ListRequestIdTestingComplete(listRequestIdFromSampling.Distinct().ToList());

                    //cari data yang dari  dua  list tersebut dua-duanya sedang tidak berada list QA
                    //cari data sampling yang di testing tidak ada 
                    foreach (var itemA in listRequestIdFromSamplingInQA.Distinct().ToList().GroupBy(x => x))
                    {
                        if (listRequestIdFromTestingInQA.GroupBy(x => x).Where(y => y.Key != itemA.Key).Any())
                        {
                            listRequestInQA.Add(itemA.Key);
                        }
                    }

                    //cari data testing yang di sampling tidak ada 
                    foreach (var itemB in listRequestIdFromTestingInQA.Distinct().ToList().GroupBy(x => x))
                    {
                        if (listRequestIdFromSamplingInQA.GroupBy(x => x).Where(y => y.Key != itemB.Key).Any())
                        {
                            listRequestInQA.Add(itemB.Key);
                        }
                    }

                    //seleksi data jika sampling di kabag dan testing di QA
                    foreach (var item2 in listRequestIdFromSampling.Distinct().ToList().GroupBy(x => x))
                    {
                        if (listRequestInQA.GroupBy(x => x).Where(y => y.Key == item2.Key).Any())
                        {

                            var resData = result.FirstOrDefault(x => x.Id == item2.Key);
                            if (resData != null)
                            {
                                resultToDo.Add(resData);
                            }
                        }
                    }

                    //seleksi data jika testing di kabag dan sampling di QA
                    foreach (var item2 in listRequestIdFromTesting.Distinct().ToList().GroupBy(x => x))
                    {
                        if (listRequestInQA.GroupBy(x => x).Where(y => y.Key == item2.Key).Any())
                        {
                            var resData = result.FirstOrDefault(x => x.Id == item2.Key);
                            if (resData != null)
                            {
                                resultToDo.Add(resData);
                            }
                        }
                    }

                    //data testing yang sudah complete
                    //seleksi data jika sampling di kabag dan testing di QA
                    foreach (var item2 in listRequestIdFromSampling.Distinct().ToList().GroupBy(x => x))
                    {
                        if (requestIdByTestingHasCompleteInPhase2.GroupBy(x => x).Where(y => y.Key == item2.Key).Any())
                        {

                            var resData = result.FirstOrDefault(x => x.Id == item2.Key);
                            if (resData != null)
                            {
                                resultToDo.Add(resData);
                            }
                        }
                    }


                }
                else if (workflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA)
                {
                    //handle after complete//
                    //get data sampling has complete in phase 2
                    var requestIdBySamplingHasCompleteInPhase2 = await _qcSamplingDataProvider.ListRequestIdSamplingComplete(listRequestIdFromSampling.Distinct().ToList());
                    //get data request id dari data testing yang sudah complete phase 2
                    var requestIdByTestingHasCompleteInPhase2 = await _qcTestDataProvider.ListRequestIdTestingComplete(listRequestIdFromSampling.Distinct().ToList());

                    //get data request id dari data testing yang tertinggal di todo 
                    var requestIdByTestingNotComplete = await _qcTestDataProvider.ListRequestIdTestingNotComplete(listRequestIdFromSampling.Distinct().ToList(), workflowStatus);

                    //get data yang sama 
                    var listFinal = requestIdBySamplingHasCompleteInPhase2.Intersect(requestIdByTestingHasCompleteInPhase2);
                    var listFinalV2 = requestIdBySamplingHasCompleteInPhase2.Intersect(requestIdByTestingNotComplete);
                    var resData = result.Where(x => listFinal.Contains(x.Id)).ToList();
                    var resDataV2 = result.Where(x => listFinalV2.Contains(x.Id)).ToList();
                    if (resData != null)
                    {
                        resultToDo.AddRange(resData);
                        resultToDo.AddRange(resDataV2);
                        resultToDo.Distinct().ToList();
                    }


                }

                Console.WriteLine(resultToDo);
                if (resultToDo.Any())
                {

                    if (workflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA)
                    {
                        //jika yang akses merupakan QA 
                        resultDataByNik = resultToDo.OrderBy(x => x.CreatedAt).Where(x => x != null).ToList();
                    }
                    else
                    {
                        resultDataByNik = resultToDo.OrderByDescending(x => x.CreatedAt).Where(x => x != null).ToList();
                    }
                }
                else
                {
                    resultDataByNik = null;
                }
            }
            else
            {
                //tampilkan recepit date jika data sudah berada di QA 
                resultDataByNik = result.OrderByDescending(x => x.CreatedAt).ToList();
            }

            var resultData = new List<MonitoringListViewModel>();

            if (resultDataByNik != null)
            {
                resultData = resultDataByNik.GroupBy(x => x.Id).Select(y => y.First()).ToList();
            }

            //get statusTransfer dan statusTesting by list of samplingId
            List<int> samplingIds = new List<int>();
            foreach (var item in resultData)
            {
                var samplingId = item.Sampling.Select(x => x.SamplingId).ToList();
                samplingIds.AddRange(samplingId);
            }
            samplingIds.Distinct().ToList();

            var statusTransfers = await (from qss in _context.QcSamplingShipments
                                         where samplingIds.Contains(qss.QcSamplingId)
                                         orderby qss.Status
                                         select new
                                         {
                                             StatusTransfer = qss.Status,
                                             SamplingId = qss.QcSamplingId
                                         }).Distinct().ToListAsync();
            var statusTestings = await (from qs in _context.QcSamples
                                        join qts in _context.QcTransactionSamples on qs.Id equals qts.QcSampleId
                                        join qtg in _context.QcTransactionGroups on qts.QcTransactionGroupId equals qtg.Id
                                        where samplingIds.Contains(qs.QcSamplingId)
                                        orderby qtg.Status
                                        select new
                                        {
                                            StatusTesting = qtg.Status,
                                            SamplingId = qs.QcSamplingId
                                        }).Distinct().ToListAsync();

            //set statusTransfer dan statusTesting
            foreach (var item in resultData)
            {
                foreach (var itemSampling in item.Sampling)
                {
                    itemSampling.StatusTransfer = statusTransfers.Where(x => x.SamplingId == itemSampling.SamplingId).Select(x => x.StatusTransfer).FirstOrDefault();
                    itemSampling.StatusTesting = statusTestings.Where(x => x.SamplingId == itemSampling.SamplingId).Select(x => x.StatusTesting).FirstOrDefault();
                }
            }

            if (resultData.Any())
            {
                for (int i = 0; i < resultData.Count(); i++)
                {

                    var qcResEMPC = false;
                    var qcResEMM = false;
                    var qcRecDateEMPC = false;
                    var qcRecDateEMM = false;
                    for (int j = 0; j < resultData[i].Sampling.Count(); j++)
                    {
                        // by default prosesnya request, statusnya sama dengan status request, nanti diupdate disesuaiin yg terakhir

                        // Sampling Type == EM-M
                        if (resultData[i].Sampling[j].SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_EMM)
                        {

                            if (resultData[i].Sampling[j].StatusSampling >= ApplicationConstant.STATUS_SUBMIT && resultData[i].Sampling[j].StatusTesting >= ApplicationConstant.STATUS_TEST_INPROGRESS)
                            {
                                qcResEMPC = true;
                            }


                            //jika status sampling masih di kabag dan testing sudah di QA (setelah proses riject)
                            if ((resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_KABAG) && (resultData[i].Sampling[j].StatusTesting > 5))
                            {
                                resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_RESULT;
                                resultData[i].Sampling[j].Status = ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_PRODUKSI;
                                continue;
                            }
                            else if ((resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_QA) && (resultData[i].Sampling[j].StatusTesting == ApplicationConstant.STATUS_TEST_APPROVED))
                            {
                                resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_RESULT;
                                resultData[i].Sampling[j].Status = ApplicationConstant.STATUS_TEST_INREVIEW_QA;
                                continue;
                            }

                            // Test
                            if (!(
                                    resultData[i].Sampling[j].StatusTesting == ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_PRODUKSI
                                    || resultData[i].Sampling[j].StatusTesting == ApplicationConstant.STATUS_TEST_INREVIEW_QA
                                    || resultData[i].Sampling[j].StatusTesting == ApplicationConstant.STATUS_TEST_APPROVED
                                    || resultData[i].Sampling[j].StatusTesting == null
                                    || resultData[i].Sampling[j].StatusTesting == 0
                                ))
                            {
                                resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_TESTING;
                                resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusTesting;
                                continue;
                            }

                            // Transfer
                            if (resultData[i].Sampling[j].StatusTransfer != ApplicationConstant.STATUS_SHIPMENT_RECEIVED)
                            {
                                if (resultData[i].Sampling[j].StatusSampling != ApplicationConstant.STATUS_IN_REVIEW_KABAG)
                                {
                                    resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                    resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;
                                    continue;
                                }
                                else
                                {
                                    if (resultData[i].Sampling[j].StatusTransfer == null)
                                    {
                                        resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                        resultData[i].Sampling[j].Status = ApplicationConstant.STATUS_READY_TO_TRANSFER;
                                        continue;
                                    }
                                    else
                                    {
                                        resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_TRANSFER;
                                        resultData[i].Sampling[j].Status = resultData[i].Sampling[j].StatusTransfer != null ? resultData[i].Sampling[j].StatusTransfer : resultData[i].Sampling[j].Status;
                                        continue;
                                    }
                                }
                            }

                            if (resultData[i].Sampling[j].StatusTransfer == ApplicationConstant.STATUS_SHIPMENT_RECEIVED)
                            {
                                if (resultData[i].Sampling[j].StatusTesting == null)
                                {
                                    resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_TRANSFER;
                                    resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusTransfer;
                                    continue;
                                }
                            }

                            // Sampling, udah lolos request
                            if (!(
                                    resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_APPROVED
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_KABAG
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_QA
                                ))
                            {
                                resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;
                                continue;
                            }

                            // Result
                            resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_RESULT;
                            resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusTesting;
                        }

                        // Sampling Type == EM-PC
                        else if (resultData[i].Sampling[j].SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_PC)
                        {
                            // Sampling, udah lolos request
                            if (!(
                                    resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_APPROVED
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_KABAG
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_QA
                                ))
                            {
                                resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;
                                continue;
                            }

                            // Result
                            resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_RESULT;
                            resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;

                            if (resultData[i].Sampling[j].StatusSampling >= ApplicationConstant.STATUS_SUBMIT)
                            {
                                qcResEMM = true;
                            }
                        }
                    }

                    for (int j = 0; j < resultData[i].Sampling.Count(); j++)
                    {
                        // Sampling Type == EM-M
                        if (resultData[i].Sampling[j].SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_EMM)
                        {
                            if (resultData[i].Sampling[j].Process == 5 && resultData[i].Sampling[j].Status >= ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_PRODUKSI)
                            {
                                qcRecDateEMM = true;
                            }
                        }

                        // Sampling Type == EM-PC
                        else if (resultData[i].Sampling[j].SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_PC)
                        {
                            if (resultData[i].Sampling[j].Process == 5 && resultData[i].Sampling[j].Status >= ApplicationConstant.STATUS_IN_REVIEW_KABAG)
                            {
                                qcRecDateEMPC = true;
                            }
                        }
                    }

                    if (qcResEMPC == true && qcResEMM == true)
                    {
                        resultData[i].Conclusion = resultData[i].ConclusionTemp;
                        Console.WriteLine(resultData[i].Conclusion);
                    }

                    if ((!qcRecDateEMM) || (!qcRecDateEMPC))
                    {
                        resultData[i].ReceiptDate = null;
                        resultData[i].ReceiptDateKabag = null;
                        resultData[i].ReceiptDateQA = null;
                    }
                }
            }

            return resultData.Skip(page).Take(limit).ToList();
        }

        public async Task<List<MonitoringRelationViewModel>> GetRequestQcById(int requestQcId, string? nik)
        {
            var result = (from rq in _context.RequestQcs
                          where rq.Id == requestQcId
                          select new MonitoringRelationViewModel
                          {
                              Id = rq.Id,
                              NoRequest = rq.NoRequest,
                              TypeRequest = rq.TypeRequest,
                              TypeRequestId = rq.TypeRequestId,
                              NoBatch = rq.NoBatch,
                              IsAllowedProcessApprove = false,
                              IsAllowedDeviationButton = false,
                              IsAllowedDeviationColoumn = false,
                              Sampling = (from qs in _context.QcSamplings
                                          where qs.RequestQcsId == rq.Id
                                          orderby qs.SamplingTypeName
                                          select new MonitoringSamplingSidebarViewModel
                                          {
                                              Id = qs.Id,
                                              SamplingTypeName = qs.SamplingTypeName,
                                              IsAllowedProcessApprove = false,
                                              IsAllowedProcessReject = false
                                          }).ToList(),

                              // TODO: ganti jadi list sampling shipment filter by qc sampling id
                              Transfer = (from qss in _context.QcSamplingShipments
                                          join qs in _context.QcSamplings on qss.QcSamplingId equals qs.Id
                                          where qs.RequestQcsId == rq.Id
                                          select new MonitoringSamplingSidebarViewModel
                                          {
                                              Id = qss.QcSamplingId,
                                              SamplingTypeName = qs.SamplingTypeName
                                          }).Take(1).ToList(),

                              Testing = (List<MonitoringSamplingSidebarViewModel>)(from qs in _context.QcSamples
                                                                                   join qsa in _context.QcSamplings on qs.QcSamplingId equals qsa.Id
                                                                                   join qts in _context.QcTransactionSamples on qs.Id equals qts.QcSampleId
                                                                                   join qtg in _context.QcTransactionGroups on qts.QcTransactionGroupId equals qtg.Id
                                                                                   where qsa.RequestQcsId == rq.Id
                                                                                   select new MonitoringSamplingSidebarViewModel
                                                                                   {
                                                                                       Id = qtg.Id,
                                                                                       Name = qtg.Code
                                                                                   }
                                         ).ToList(),
                              RequestPurposes = (from tp in _context.TransactionPurposes
                                                 join rp in _context.RequestPurposes on tp.Id equals rp.PurposeId
                                                 where rp.QcRequestId == rq.Id
                                                 select new RequestPurposesViewModel
                                                 {
                                                     PurposeId = tp.Id,
                                                     PurposeCode = tp.Code,
                                                     PurposeName = tp.Name
                                                 }).ToList()

                          }
                                ).ToList();

            var isAllowedProcessWorkflow = false;

            if (result.Any() && nik != null)
            {
                var listPendingWorkflow = await _workflowServiceDataProvider.GetListPendingByNik(nik);
                List<ListRequestId> listRequestIdFromSampling = new List<ListRequestId>();
                List<ListRequestId> listRequestIdFromTesting = new List<ListRequestId>();

                var resultToDo = new List<MonitoringListViewModel>();
                var workflowStatus = "";

                if (listPendingWorkflow.ListPending.Any())
                {
                    workflowStatus = listPendingWorkflow.ListPending.Last().StatusName;

                    var getRequestIdByWorkflowCodeSampling = await _qcSamplingDataProvider.GetAllRequestIdByWorkflowCode(listPendingWorkflow.ListPending, workflowStatus);
                    foreach (var reqidfs in getRequestIdByWorkflowCodeSampling)
                    {
                        ListRequestId listRequestId = new ListRequestId()
                        {
                            QcRequestId = reqidfs,
                            StatusName = workflowStatus,
                            Pic = nik
                        };
                        listRequestIdFromSampling.Add(listRequestId);
                    }

                    var getRequestIdByWorkflowCodeSamplingTesting = await _qcTestDataProvider.GetAllRequestIdByWorkflowCode(listPendingWorkflow.ListPending, workflowStatus);
                    foreach (var reqidfst in getRequestIdByWorkflowCodeSamplingTesting)
                    {
                        ListRequestId listRequestIdTesting = new ListRequestId()
                        {
                            QcRequestId = reqidfst,
                            StatusName = workflowStatus,
                            Pic = nik
                        };
                        listRequestIdFromTesting.Add(listRequestIdTesting);
                    }
                }

                // foreach (var item in listPendingWorkflow.ListPending)
                // {
                //     var getRequestIdByWorkflowCodeSampling = await _qcSamplingDataProvider.GetRequestIdByWorkflowCode(item.RecordId, item.StatusName);
                //     if (getRequestIdByWorkflowCodeSampling != 0)
                //     {
                //         ListRequestId listRequestId = new ListRequestId()
                //         {
                //             QcRequestId = getRequestIdByWorkflowCodeSampling,
                //             StatusName = item.StatusName,
                //             Pic = item.OrgId
                //         };
                //         listRequestIdFromSampling.Add(listRequestId);
                //     }
                //     var getRequestIdByWorkflowCodeSamplingTesting = await _qcTestDataProvider.GetRequestIdByWorkflowCode(item.RecordId, item.StatusName);
                //     if (getRequestIdByWorkflowCodeSamplingTesting != 0)
                //     {
                //         ListRequestId listRequestIdTesting = new ListRequestId()
                //         {
                //             QcRequestId = getRequestIdByWorkflowCodeSamplingTesting,
                //             StatusName = item.StatusName,
                //             Pic = item.OrgId
                //         };
                //         listRequestIdFromTesting.Add(listRequestIdTesting);
                //     }
                //     workflowStatus = item.StatusName;
                // }

                //cek jika review kabag ada kah data yang di QA 
                var listRequestInQA = new List<int>();
                if (workflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG)
                {
                    var listRequestIdFromSamplingInQA = new List<int>();
                    var listRequestIdFromTestingInQA = new List<int>();

                    //get data testing in review QA
                    var getRequestIdSamplingTestingInReviewQa = await _qcTestDataProvider.GetRequestIdSamplingTestingInReviewQa();
                    if (getRequestIdSamplingTestingInReviewQa != null)
                    {
                        foreach (var item in getRequestIdSamplingTestingInReviewQa.GroupBy(x => x))
                        {
                            listRequestIdFromTestingInQA.Add(item.Key);
                        }
                    }
                    //get data sampling in review QA
                    var getRequestIdSamplingInReviewQa = await _qcSamplingDataProvider.GetRequestIdSamplingInReviewQa();
                    if (getRequestIdSamplingInReviewQa != null)
                    {
                        foreach (var item in getRequestIdSamplingInReviewQa.GroupBy(x => x))
                        {
                            listRequestIdFromSamplingInQA.Add(item.Key);
                        }
                    }

                    //cek data sampling by id request if data in phase 2 has complete
                    var haveConditionSamplingRejectAfterComplete = await _qcSamplingDataProvider.GetRequestIdSamplingComplete(requestQcId, workflowStatus);

                    if ((listRequestIdFromSampling.FirstOrDefault(x => x.QcRequestId == requestQcId) != null) && (listRequestIdFromTestingInQA.GroupBy(x => x).FirstOrDefault(x => x.Key == requestQcId) != null))
                    {
                        result[0].IsAllowedProcessApprove = true;
                    }
                    else if ((listRequestIdFromTesting.FirstOrDefault(x => x.QcRequestId == requestQcId) != null) && (listRequestIdFromSamplingInQA.GroupBy(x => x).FirstOrDefault(x => x.Key == requestQcId) != null))
                    {
                        result[0].IsAllowedProcessApprove = true;
                    }
                    else if (haveConditionSamplingRejectAfterComplete)
                    {
                        result[0].IsAllowedProcessApprove = true;
                    }
                }
                else if (workflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA)
                {
                    var cekStillInReviewNotQA = await _qcSamplingDataProvider.GetRequestIdSamplingStillInReview(requestQcId);
                    if (cekStillInReviewNotQA == null)
                    {
                        var haveConditionSamplingRejectAfterComplete = await _qcSamplingDataProvider.GetRequestIdSamplingComplete(requestQcId, workflowStatus);
                        if (haveConditionSamplingRejectAfterComplete)
                        {
                            result[0].IsAllowedProcessApprove = true;
                        }

                    }
                }

                //deviation
                if ((listRequestIdFromSampling.FirstOrDefault(x => x.QcRequestId == requestQcId) != null) && (listRequestIdFromTesting.FirstOrDefault(x => x.QcRequestId == requestQcId) != null))
                {
                    var currentStatus = listRequestIdFromSampling.FirstOrDefault(x => x.QcRequestId == requestQcId);
                    result[0].IsAllowedProcessApprove = true;
                    isAllowedProcessWorkflow = true;
                    var getRequest = await _qcQcRequestDataProvider.GetById(currentStatus.QcRequestId);
                    //cek apakah pass or aaoaemm
                    List<QcResult> findNotPass = (from qr in _context.QcResults
                                                  join qs in _context.QcSamples on qr.SampleId equals qs.Id
                                                  join qsing in _context.QcSamplings on qs.QcSamplingId equals qsing.Id
                                                  where qsing.RequestQcsId == currentStatus.QcRequestId &&
                                                  qr.TestVariableConclusion != ApplicationConstant.TEST_VARIABLE_CONCLUSION_PASS
                                                  select qr
                    ).ToList();
                    if ((currentStatus.StatusName == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KABAG) && (findNotPass.Count() != 0) && (currentStatus.Pic == nik))
                    {
                        result[0].IsAllowedDeviationButton = true;
                        result[0].IsAllowedDeviationColoumn = true;
                    }
                    else if ((currentStatus.StatusName == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA) && (getRequest.NoDeviation != null) && (findNotPass.Count() != 0) && (currentStatus.Pic == nik))
                    {
                        result[0].IsAllowedDeviationButton = true;
                        result[0].IsAllowedDeviationColoumn = true;
                    }
                }
            }

            if (result.Any())
            {
                if (result[0].Sampling.Any() && nik != null)
                {
                    List<MonitoringSamplingSidebarViewModel> sampling = result[0].Sampling;
                    for (int i = 0; i < sampling.Count(); i++)
                    {
                        string? workflowDocumentCode = (from wqs in _context.WorkflowQcSampling
                                                        where wqs.QcSamplingId == sampling[i].Id
                                                        && wqs.IsInWorkflow == true
                                                        select new { wqs.WorkflowDocumentCode }
                                                       ).FirstOrDefault()?.WorkflowDocumentCode;
                        if (workflowDocumentCode != null)
                        {
                            var pending = await _workflowServiceDataProvider.GetListPendingByNik(nik);
                            List<ListReviewPending> listPending = pending.ListPending;
                            if (listPending.FirstOrDefault(x => x.RecordId == workflowDocumentCode) != null)
                            {
                                sampling[i].IsAllowedProcessReject = true;
                                sampling[i].IsAllowedProcessApprove = true;
                            }
                        }


                        var workflowDocumentCodeComplete = (from wqs in _context.WorkflowQcSampling
                                                            where wqs.QcSamplingId == sampling[i].Id
                                                            && wqs.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2
                                                            orderby wqs.UpdatedAt descending
                                                            select wqs
                                                       ).FirstOrDefault();

                        if (workflowDocumentCodeComplete != null)
                        {

                            if ((workflowDocumentCodeComplete.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME) && (workflowDocumentCodeComplete.IsInWorkflow == false))
                            {
                                var WorkflowHistoryQcSampling = await _workflowServiceDataProvider.GetListHistoryWorkflow(workflowDocumentCodeComplete.WorkflowDocumentCode);
                                List<string> listReviewerQA = new List<string>();
                                var listReviewerQAFormHistory = WorkflowHistoryQcSampling.History.FirstOrDefault(x => x.StatusName == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_QA);
                                foreach (var itemPIC in listReviewerQAFormHistory.PICs)
                                {
                                    listReviewerQA.Add(itemPIC.OrgId);
                                }

                                if ((listReviewerQA.Any()) && ((listReviewerQA.GroupBy(x => x).FirstOrDefault(x => x.Key == nik)) != null))
                                {
                                    sampling[i].IsAllowedProcessReject = true;
                                }

                            }
                        }


                    }
                    result[0].Sampling = sampling;
                }

                if (result[0].Testing.Any())
                {

                    List<MonitoringSamplingSidebarViewModel> testing = result[0].Testing;
                    testing = testing.GroupBy(x => new { x.Id, x.Name }).Select(g => g.First()).OrderBy(x => x.Name).ToList();

                    for (int i = 0; i < testing.Count(); i++)
                    {
                        testing[i].QcTransactionGroupProcess = GetQcProcessById(testing[i].Id).FirstOrDefault();

                        // TODO: make this recursive, memory leak problem
                        if (testing[i].QcTransactionGroupProcess != null)
                        {
                            testing[i].QcTransactionGroupProcess.QcTransactionGroupProcess = GetQcProcessById(testing[i].Id, testing[i].QcTransactionGroupProcess.Id);

                            if (nik != null)
                            {
                                string? workflowDocumentCode = (from wqtg in _context.WorkflowQcTransactionGroup
                                                                where wqtg.QcTransactionGroupId == testing[i].Id
                                                                && wqtg.IsInWorkflow == true
                                                                select new { wqtg.WorkflowDocumentCode }
                                                           ).FirstOrDefault()?.WorkflowDocumentCode;
                                if (workflowDocumentCode != null)
                                {
                                    // var findTesting = await _qcTestDataProvider.GetByWorkflowDocumentCodeOnPending(workflowDocumentCode, nik);
                                    var pending = await _workflowServiceDataProvider.GetListPendingByNik(nik);
                                    List<ListReviewPending> listPending = pending.ListPending;
                                    if (listPending.FirstOrDefault(x => x.RecordId == workflowDocumentCode) != null)
                                    {
                                        // TODO : code ini akan dihilangkan , untuk sementara di komen. terjadi karena kabag dan QA tidak dapat untuk reject data testing
                                        // testing[i].IsAllowedProcessReject = true;
                                        testing[i].IsAllowedProcessApprove = true;
                                    }
                                }
                            }
                        }
                    }

                    result[0].Testing = testing;
                }


            }

            return result;
        }

        public List<QcTransactionGroupProcessRelViewModel> GetQcProcessById(int groupId, int? parentId = null)
        {
            List<QcTransactionGroupProcessRelViewModel> result = (from qtgp in _context.QcTransactionGroupProcesses
                                                                  join qtg in _context.QcTransactionGroups on qtgp.QcTransactionGroupId equals qtg.Id
                                                                  where qtg.Id == groupId && qtgp.RowStatus == null && qtgp.ParentId == parentId
                                                                  orderby qtgp.Sequence
                                                                  select new QcTransactionGroupProcessRelViewModel
                                                                  {
                                                                      Id = qtgp.Id,
                                                                      QcTransactionGroupId = qtgp.QcTransactionGroupId,
                                                                      Sequence = qtgp.Sequence,
                                                                      Name = qtgp.Name,
                                                                      ParentId = qtgp.ParentId,
                                                                      RoomId = qtgp.RoomId,
                                                                      IsInputForm = qtgp.IsInputForm,
                                                                      QcProcessId = qtgp.QcProcessId,
                                                                      CreatedAt = qtgp.CreatedAt,
                                                                      CreatedBy = qtgp.CreatedBy,

                                                                      QcTransactionGroupFormMaterial = (from qtm in _context.QcTransactionGroupFormMaterials
                                                                                                        join uom in _context.Uoms on qtm.Uom equals uom.UomId
                                                                                                        join uomPackage in _context.Uoms on qtm.UomPackage equals uomPackage.UomId
                                                                                                        where qtm.QcTransactionGroupProcessId == qtgp.Id
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
                                                                                                    where qtt.QcTransactionGroupProcessId == qtgp.Id
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
                                                                                                         where qtfp.QcTransactionGroupProcessId == qtgp.Id
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
                                                                      //QcTransactionGroupProcess = GetQcProcessById(groupId, qtgp.Id)
                                                                  }

                                                            ).ToList();
            return result;
        }

        public async Task<List<MonitoringResultViewModel>> GetResult(int requestQcId)
        {
            var result = await (from rq in _context.RequestQcs
                                where rq.Id == requestQcId
                                select new MonitoringResultViewModel
                                {
                                    Id = rq.Id,
                                    NoRequest = rq.NoRequest,
                                    Date = rq.Date,
                                    SamplingDateFrom = (from qsi1 in _context.QcSamplings
                                                        where qsi1.RequestQcsId == rq.Id
                                                        orderby qsi1.SamplingDateFrom
                                                        select new { qsi1.SamplingDateFrom }).FirstOrDefault().SamplingDateFrom,
                                    SamplingDateTo = (from qsi2 in _context.QcSamplings
                                                      where qsi2.RequestQcsId == rq.Id
                                                      orderby qsi2.SamplingDateTo descending
                                                      select new { qsi2.SamplingDateTo }).FirstOrDefault().SamplingDateTo,
                                    EmPhaseId = rq.EmPhaseId,
                                    EmPhaseName = rq.EmPhaseName,
                                    EmRoomId = rq.EmRoomId,
                                    EmRoomName = rq.EmRoomName,
                                    NoDeviation = rq.NoDeviation,
                                    Conclusion = rq.Conclusion,
                                    EmRoomGradeId = rq.EmRoomGradeId,
                                    EmRoomGradeName = rq.EmRoomGradeName
                                    //Sampling = 
                                }).ToListAsync();

            var samplings = await (from qs in _context.QcSamplings
                                   where qs.RequestQcsId == requestQcId
                                   orderby qs.SamplingTypeName
                                   select new MonitoringSamplingSidebarViewModel
                                   {
                                       Id = qs.Id,
                                       SamplingTypeName = qs.SamplingTypeName,
                                       Notes = qs.Note,
                                       //Samples = 
                                   }).ToListAsync();
            var samplingIds = samplings.Select(x => x.Id).Distinct().ToList();

            var samples = await (from qse in _context.QcSamples
                                 join tps in _context.TransactionTestParameter on qse.TestParamId equals tps.Id
                                 join ts in _context.TransactionTestScenario on qse.TestScenarioId equals ts.Id
                                 join qr in _context.QcResults on qse.Id equals qr.SampleId into qr1
                                 from qr in qr1.DefaultIfEmpty()
                                     //where qse.QcSamplingId == qs.Id 
                                 where samplingIds.Contains(qse.QcSamplingId)
                                 && qse.ParentId == null
                                 select new QcSampleViewModel
                                 {
                                     Id = qse.Id,
                                     SampleSequence = (qse.SampleSequence != 0 ? qse.SampleSequence : 0),
                                     QcSamplingId = qse.QcSamplingId,
                                     Code = qse.Code,
                                     SamplingPointId = qse.SamplingPointId ?? null,
                                     SamplingPointCode = qse.SamplingPointCode,
                                     TestParamId = qse.TestParamId,
                                     TestParamName = tps.Name,
                                     ToolId = qse.ToolId,
                                     ToolCode = qse.ToolCode,
                                     ToolName = qse.ToolName,
                                     ToolGroupId = qse.ToolGroupId != 0 ? qse.ToolGroupId : 0,
                                     ToolGroupName = qse.ToolGroupName,
                                     ToolGroupLabel = qse.ToolGroupLabel,
                                     GradeRoomId = qse.GradeRoomId != null ? qse.GradeRoomId : null,
                                     GradeRoomName = qse.GradeRoomId != null ? qse.GradeRoomName : null,
                                     SamplingDateTimeFrom = qse.SamplingDateTimeFrom ?? null,
                                     SamplingDateTimeTo = qse.SamplingDateTimeTo ?? null,
                                     PersonalId = (qse.PersonalId == null ? null : qse.PersonalId),
                                     PersonalInitial = qse.PersonalInitial,
                                     PersonalName = qse.PersonalName,
                                     ParticleVolume = qse.ParticleVolume,
                                     Note = qse.Note,
                                     ResultValue = qr.Value,
                                     ResultConclusion = qr.TestVariableConclusion,
                                     TestParamSequence = tps.Sequence,
                                     TestParamIndex = 1,
                                     TestScenarioName = ts.Name,
                                     IsDefault = qse.IsDefault,
                                     Purpose = (from rp in _context.TransactionRoomPurpose
                                                join rgrp in _context.RequestGroupRoomPurpose on rp.Id equals rgrp.RoomPurposeId
                                                join s in _context.QcSamplings on rgrp.RequestQcsId equals s.RequestQcsId
                                                join rsp in _context.TransactionRelRoomSamplingPoint on rp.Id equals rsp.RoomPurposeId
                                                join rpmtp in _context.TransactionRoomPurposeToMasterPurpose on rp.Id equals rpmtp.RoomPurposeId
                                                join purp in _context.TransactionPurposes on rpmtp.PurposeId equals purp.Id
                                                where s.Id == qse.QcSamplingId
                                                && rsp.SamplingPoinId == qse.SamplingPointId
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
                                                         where s.Id == qse.QcSamplingId
                                                         && rts.SamplingPoinId == qse.SamplingPointId
                                                         select new RequestPurposesViewModel
                                                         {
                                                             PurposeId = purp.Id,
                                                             PurposeCode = purp.Code,
                                                             PurposeName = purp.Name
                                                         }).ToList(),
                                 }).OrderBy(x => x.TestParamSequence)
                           .ThenBy(x => x.SamplingPointCode)
                           .ThenBy(x => x.Id)
                           .ToListAsync();

            var purposes = await (from rp in _context.RequestPurposes
                join r in _context.RequestQcs on rp.QcRequestId equals r.Id
                join s in _context.QcSamplings on r.Id equals s.RequestQcsId
                //where s.Id == id
                where samplingIds.Contains(s.Id)
                select new RequestPurposesViewModel
                {
                    PurposeId = rp.PurposeId,
                    PurposeName = rp.PurposeName,
                    PurposeCode = rp.PurposeCode
                }).Distinct().ToListAsync();

            var purposeIds = purposes.Select(x => x.PurposeId).Distinct().ToList();
            var samplingPointIds = samples.Select(x => x.SamplingPointId).Distinct().ToList();

            var purposeRooms = await (from rp in _context.TransactionRoomPurpose
                join rgrp in _context.RequestGroupRoomPurpose on rp.Id equals rgrp.RoomPurposeId
                join s in _context.QcSamplings on rgrp.RequestQcsId equals s.RequestQcsId
                join rsp in _context.TransactionRelRoomSamplingPoint on rp.Id equals rsp.RoomPurposeId
                join rpmtp in _context.TransactionRoomPurposeToMasterPurpose on rp.Id equals rpmtp.RoomPurposeId
                join purp in _context.TransactionPurposes on rpmtp.PurposeId equals purp.Id
                where purposeIds.Contains(purp.Id)
                && samplingIds.Contains(s.Id)
                && samplingPointIds.Contains(rsp.SamplingPoinId)
                select new RequestPurposesViewModel
                {
                    QcSamplingId = s.Id,
                    SamplingPointId = rsp.SamplingPoinId,
                    PurposeId = purp.Id,
                    PurposeCode = purp.Code,
                    PurposeName = purp.Name
                }).Distinct().ToListAsync();

            var purposeTools = await (from tp in _context.TransactionToolPurpose
                join tgrp in _context.RequestGroupToolPurpose on tp.Id equals tgrp.ToolPurposeId
                join s in _context.QcSamplings on tgrp.RequestQcsId equals s.RequestQcsId
                join rts in _context.TransactionRelSamplingTool on tp.Id equals rts.ToolPurposeId
                join tpmp in _context.TransactionToolPurposeToMasterPurpose on tp.Id equals tpmp.ToolPurposeId
                join purp in _context.TransactionPurposes on tpmp.PurposeId equals purp.Id
                where purposeIds.Contains(purp.Id)
                && samplingIds.Contains(s.Id)
                && samplingPointIds.Contains(rts.SamplingPoinId)
                select new RequestPurposesViewModel
                {
                    QcSamplingId = s.Id,
                    SamplingPointId = rts.SamplingPoinId,
                    PurposeId = purp.Id,
                    PurposeCode = purp.Code,
                    PurposeName = purp.Name
                }).Distinct().ToListAsync();

            foreach (var item in result)
            {
                item.Sampling = samplings;
                foreach (var itemSampling in item.Sampling)
                {
                    itemSampling.Samples = samples.Where(x => x.QcSamplingId == itemSampling.Id).ToList();

                    foreach (var itemSample in itemSampling.Samples)
                    {
                        if (itemSample.GradeRoomId == null) itemSample.GradeRoomId = item.EmRoomGradeId;
                        if (itemSample.GradeRoomName == null) itemSample.GradeRoomName = item.EmRoomGradeName;

                        var purposeRoom = purposeRooms.Where(x =>
                            (x.QcSamplingId == itemSampling.Id)
                            && (x.SamplingPointId == itemSample.SamplingPointId)
                        ).ToList();

                        var purposeTool = purposeTools.Where(x =>
                            (x.QcSamplingId == itemSampling.Id)
                            && (x.SamplingPointId == itemSample.SamplingPointId)
                        ).ToList();

                        purposeRoom.AddRange(purposeTool);

                        //purpose untuk tes parameter jenis finger DB
                        if ((itemSample.TestParamName == ApplicationConstant.TEST_PARAMETER_LABEL_FD) || (itemSample.TestParamId == ApplicationConstant.TEST_PARAMETER_FD))
                        {
                            purposeRoom.AddRange(purposes);
                        }
                        itemSample.Purpose = purposeRoom.Distinct().ToList();
                    }
                }
            }

            int newTestParamIndex = 1;
            var existingSamplingPointCode = new List<string>();
            var existingTestParamIndex = new List<int>();
            int samplingPointCodeIndex = 0;
            int testParamIndex = 0;
            foreach (var item in result)
            {
                foreach (var itemSampling in item.Sampling)
                {
                    foreach (var sample in itemSampling.Samples)
                    {
                        if (sample.TestParamId == 1)
                        {
                            if (existingSamplingPointCode.Contains(sample.SamplingPointCode))
                            {
                                samplingPointCodeIndex = existingSamplingPointCode.IndexOf(sample.SamplingPointCode);
                                testParamIndex = existingTestParamIndex[samplingPointCodeIndex] + 1;
                                sample.TestParamIndex = testParamIndex;
                                existingTestParamIndex[samplingPointCodeIndex] = testParamIndex;
                            }
                            else
                            {
                                existingSamplingPointCode.Add(sample.SamplingPointCode);
                                existingTestParamIndex.Add(newTestParamIndex);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<List<MonitoringListViewModel>> ListReportQa(string search, int limit, int page, string nik, int? facilityId)
        {
            var result = new List<MonitoringListViewModel>();
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            if ((facilityId != null) && (facilityId != 0))
            {
                var getRequestId = await _FindRequestIdByFacilityId(facilityId);
                result = (from p in _context.RequestQcs
                          where ((EF.Functions.Like(p.NoRequest.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(p.NoBatch.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")))
                          && p.RowStatus == null
                        && getRequestId.Contains(p.Id)
                          select new MonitoringListViewModel
                          {
                              Id = p.Id,
                              Date = p.Date,
                              NoRequest = p.NoRequest,
                              TypeRequestId = p.TypeRequestId,
                              TypeRequest = p.TypeRequest,
                              ReceiptDate = p.ReceiptDate,
                              ReceiptDateQA = p.ReceiptDateQA,
                              ReceiptDateKabag = p.ReceiptDateKabag,
                              NoBatch = p.NoBatch,
                              ItemId = p.ItemId,
                              ItemName = p.ItemName,
                              TypeFormId = p.TypeFormId,
                              TypeFormName = p.TypeFormName,
                              PhaseId = p.EmPhaseId,
                              PhaseName = p.EmPhaseName,
                              RoomName = p.EmRoomName,
                              RoomId = p.EmRoomId,
                              Status = p.Status,
                              NoDeviation = p.NoDeviation,
                              Conclusion = p.Conclusion,
                              WorkflowStatus = p.WorkflowStatus,
                              OrgId = p.OrgId,
                              OrgName = p.OrgName,
                              CreatedAt = p.CreatedAt,
                              CreatedBy = p.CreatedBy,
                              Sampling = (from qrs in _context.QcSamplings
                                          where p.Id == qrs.RequestQcsId
                                          select new MonitoringSamplingListViewModel
                                          {
                                              SamplingId = qrs.Id,
                                              Code = qrs.Code,
                                              // check sample receive date
                                              ReceiptDate = null,
                                              NoRequest = p.NoRequest,
                                              TypeRequestId = p.TypeRequestId,
                                              TypeRequest = p.TypeRequest,
                                              SamplingTypeId = qrs.SamplingTypeId,
                                              SamplingTypeName = qrs.SamplingTypeName,
                                              Process = ApplicationConstant.PROCESS_STATUS_PHASE_REQUEST,
                                              Status = p.Status,
                                              StatusRequest = p.Status,
                                              StatusSampling = qrs.Status,
                                              StatusTransfer = null,
                                              StatusTesting = null,
                                              // process phase, cek di workflow masing2 phase, mundur dari step paling akhir dulu
                                              // TODO: baru sampe transfer, nunggu implemen testing


                                          }).ToList(),
                              RequestPurposes = (from rp in _context.RequestPurposes
                                                 join purp in _context.TransactionPurposes on rp.PurposeId equals purp.Id
                                                 where rp.QcRequestId == p.Id
                                                 select new RequestPurposesViewModel
                                                 {
                                                     PurposeId = purp.Id,
                                                     PurposeCode = purp.Code,
                                                     PurposeName = purp.Name
                                                 }).ToList()

                          }).OrderByDescending(x => x.CreatedAt).ToList();
            }
            else
            {
                result = (from p in _context.RequestQcs
                          where ((EF.Functions.Like(p.NoRequest.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(p.NoBatch.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")))
                          && p.RowStatus == null
                          select new MonitoringListViewModel
                          {
                              Id = p.Id,
                              Date = p.Date,
                              NoRequest = p.NoRequest,
                              TypeRequestId = p.TypeRequestId,
                              TypeRequest = p.TypeRequest,
                              ReceiptDate = p.ReceiptDate,
                              ReceiptDateQA = p.ReceiptDateQA,
                              ReceiptDateKabag = p.ReceiptDateKabag,
                              NoBatch = p.NoBatch,
                              ItemId = p.ItemId,
                              ItemName = p.ItemName,
                              TypeFormId = p.TypeFormId,
                              TypeFormName = p.TypeFormName,
                              PhaseId = p.EmPhaseId,
                              PhaseName = p.EmPhaseName,
                              RoomName = p.EmRoomName,
                              RoomId = p.EmRoomId,
                              Status = p.Status,
                              NoDeviation = p.NoDeviation,
                              Conclusion = p.Conclusion,
                              WorkflowStatus = p.WorkflowStatus,
                              OrgId = p.OrgId,
                              OrgName = p.OrgName,
                              CreatedAt = p.CreatedAt,
                              CreatedBy = p.CreatedBy,
                              Sampling = (from qrs in _context.QcSamplings
                                          where p.Id == qrs.RequestQcsId
                                          select new MonitoringSamplingListViewModel
                                          {
                                              SamplingId = qrs.Id,
                                              Code = qrs.Code,
                                              // check sample receive date
                                              ReceiptDate = null,
                                              NoRequest = p.NoRequest,
                                              TypeRequestId = p.TypeRequestId,
                                              TypeRequest = p.TypeRequest,
                                              SamplingTypeId = qrs.SamplingTypeId,
                                              SamplingTypeName = qrs.SamplingTypeName,
                                              Process = ApplicationConstant.PROCESS_STATUS_PHASE_REQUEST,
                                              Status = p.Status,
                                              StatusRequest = p.Status,
                                              StatusSampling = qrs.Status,
                                              StatusTransfer = null,
                                              StatusTesting = null,
                                              // process phase, cek di workflow masing2 phase, mundur dari step paling akhir dulu
                                              // TODO: baru sampe transfer, nunggu implemen testing


                                          }).ToList(),
                              RequestPurposes = (from rp in _context.RequestPurposes
                                                 join purp in _context.TransactionPurposes on rp.PurposeId equals purp.Id
                                                 where rp.QcRequestId == p.Id
                                                 select new RequestPurposesViewModel
                                                 {
                                                     PurposeId = purp.Id,
                                                     PurposeCode = purp.Code,
                                                     PurposeName = purp.Name
                                                 }).ToList()

                          }).OrderByDescending(x => x.CreatedAt).ToList();
            }



            var resultDataByNik = new List<MonitoringListViewModel>();
            var resultList = new List<MonitoringListViewModel>();

            var listRequestIdFromSampling = new List<int>();
            var listRequestIdFromTesting = new List<int>();
            var requestQcIds = result.Select(x => x.Id).Distinct().ToList();

            var resultToDo = new List<MonitoringListViewModel>();

            var workflowQcSamplings = await (from wqs in _context.WorkflowQcSampling
                                             join qs in _context.QcSamplings on wqs.QcSamplingId equals qs.Id
                                             where wqs.IsInWorkflow == false
                                             && wqs.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2
                                             && wqs.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                                             && requestQcIds.Contains(qs.RequestQcsId)
                                             select new
                                             {
                                                 Wqs = wqs,
                                                 RequestQcsId = qs.RequestQcsId
                                             }).ToListAsync();

            var workflowTrans = await (from qt in _context.QcTransactionGroups
                                       join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                                       join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                                       join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                                       join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                                       where wt.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_2
                                       && wt.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                                       && requestQcIds.Contains(sq.RequestQcsId)
                                       select new
                                       {
                                           WorkflowCode = wt.WorkflowDocumentCode,
                                           IsInWorkflow = wt.IsInWorkflow,
                                           RequestQcsId = sq.RequestQcsId
                                       }).Distinct().ToListAsync();

            foreach (var item in result)
            {
                var getRequestIdByWorkflowCodeSampling = workflowQcSamplings.Where(x => x.RequestQcsId == item.Id).ToList();
                if (getRequestIdByWorkflowCodeSampling.Count() == 2) listRequestIdFromSampling.Add(item.Id);

                var getRequestIdByWorkflowCodeSamplingTesting = workflowTrans.Where(x => x.RequestQcsId == item.Id).ToList();
                if (getRequestIdByWorkflowCodeSamplingTesting.FirstOrDefault(x => x.IsInWorkflow == true) == null) listRequestIdFromTesting.Add(item.Id);
            }

            foreach (var item2 in listRequestIdFromSampling.Distinct().ToList().GroupBy(x => x))
            {
                if (listRequestIdFromTesting.GroupBy(x => x).Where(y => y.Key == item2.Key).Any())
                {
                    resultToDo.Add(result.FirstOrDefault(x => x.Id == item2.Key));
                }
            }

            if (resultToDo != null)
            {
                resultDataByNik = resultToDo;
            }
            else
            {
                resultDataByNik = null;
            }

            var resultData = new List<MonitoringListViewModel>();

            if (limit > 0)
            {
                resultData = resultDataByNik.Skip(page).Take(limit).ToList();
            }
            else
            {
                resultData = resultDataByNik;
            }

            //get ReceiptDate, StatusTransfer, dan StatusSampling
            var samplings = resultData.Select(x => x.Sampling).ToList();
            List<int> samplingIds = new List<int>();
            foreach (var item in samplings)
            {
                foreach (var itemSampling in item)
                {
                    samplingIds.Add(itemSampling.SamplingId);
                }
            }
            samplingIds = samplingIds.Distinct().ToList();

            var receiptDates = await (from qss in _context.QcSamplingShipments
                                      join qst in _context.QcSamplingShipmentTrackers on qss.Id equals qst.QcSamplingShipmentId
                                      where qst.Type == ApplicationConstant.TRACKER_TYPE_RECEIVE
                                      && samplingIds.Contains(qss.QcSamplingId)
                                      select new
                                      {
                                          ProcessAt = qst.processAt,
                                          QcSamplingId = qss.QcSamplingId
                                      }).ToListAsync();

            var statusTransfers = await (from qss in _context.QcSamplingShipments
                                         where samplingIds.Contains(qss.QcSamplingId)
                                         select new
                                         {
                                             StatusTransfer = qss.Status,
                                             QcSamplingId = qss.QcSamplingId
                                         }).ToListAsync();

            var statusTestings = await (from qs in _context.QcSamples
                                        join qts in _context.QcTransactionSamples on qs.Id equals qts.QcSampleId
                                        join qtg in _context.QcTransactionGroups on qts.QcTransactionGroupId equals qtg.Id
                                        where samplingIds.Contains(qs.QcSamplingId)
                                        select new
                                        {
                                            StatusTesting = qtg.Status,
                                            QcSamplingId = qs.QcSamplingId
                                        }).ToListAsync();

            //set properties ReceiptDate, StatusTransfer, dan StatusSampling 
            foreach (var item in resultData)
            {
                foreach (var itemSampling in item.Sampling)
                {
                    itemSampling.ReceiptDate = receiptDates.Where(x => x.QcSamplingId == itemSampling.SamplingId).Select(x => x.ProcessAt).FirstOrDefault();
                    itemSampling.StatusTransfer = statusTransfers.Where(x => x.QcSamplingId == itemSampling.SamplingId).Select(x => x.StatusTransfer).FirstOrDefault();
                    itemSampling.StatusTesting = statusTestings.Where(x => x.QcSamplingId == itemSampling.SamplingId).Select(x => x.StatusTesting).FirstOrDefault();
                }
            }

            if (resultData.Any())
            {
                for (int i = 0; i < resultData.Count(); i++)
                {
                    for (int j = 0; j < resultData[i].Sampling.Count(); j++)
                    {
                        // by default prosesnya request, statusnya sama dengan status request, nanti diupdate disesuaiin yg terakhir

                        // Sampling Type == EM-M
                        if (resultData[i].Sampling[j].SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_EMM)
                        {

                            // Test
                            if (!(
                                    resultData[i].Sampling[j].StatusTesting == ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_PRODUKSI
                                    || resultData[i].Sampling[j].StatusTesting == ApplicationConstant.STATUS_TEST_INREVIEW_QA
                                    || resultData[i].Sampling[j].StatusTesting == ApplicationConstant.STATUS_TEST_APPROVED
                                    || resultData[i].Sampling[j].StatusTesting == null
                                ))
                            {
                                resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_TESTING;
                                resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusTesting;
                                continue;
                            }

                            // Transfer
                            if (resultData[i].Sampling[j].StatusTransfer != ApplicationConstant.STATUS_SHIPMENT_RECEIVED)
                            {
                                if (resultData[i].Sampling[j].StatusSampling != ApplicationConstant.STATUS_IN_REVIEW_KABAG)
                                {
                                    resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                    resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;
                                    continue;
                                }
                                else
                                {
                                    if (resultData[i].Sampling[j].StatusTransfer == null)
                                    {
                                        resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                        resultData[i].Sampling[j].Status = ApplicationConstant.STATUS_READY_TO_TRANSFER;
                                        continue;
                                    }
                                    else
                                    {
                                        resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_TRANSFER;
                                        resultData[i].Sampling[j].Status = resultData[i].Sampling[j].StatusTransfer != null ? resultData[i].Sampling[j].StatusTransfer : resultData[i].Sampling[j].Status;
                                        continue;
                                    }
                                }
                            }

                            if (resultData[i].Sampling[j].StatusTransfer == ApplicationConstant.STATUS_SHIPMENT_RECEIVED)
                            {
                                if (resultData[i].Sampling[j].StatusTesting == null)
                                {
                                    resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_TRANSFER;
                                    resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusTransfer;
                                    continue;
                                }
                            }

                            // Sampling, udah lolos request
                            if (!(
                                    resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_APPROVED
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_KABAG
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_QA
                                ))
                            {
                                resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;
                                continue;
                            }

                            // Result
                            resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_RESULT;
                            resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusTesting;
                        }

                        // Sampling Type == EM-PC
                        else if (resultData[i].Sampling[j].SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_PC)
                        {
                            // Sampling, udah lolos request
                            if (!(
                                    resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_APPROVED
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_KABAG
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_QA
                                ))
                            {
                                resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;
                                continue;
                            }

                            // Result
                            resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_RESULT;
                            resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;
                        }
                    }
                }
            }

            return resultData;
        }

        public async Task<List<MonitoringListViewModel>> ListReportQa2(string search, int limit, int page, string nik, int? facilityId)
        {
            var result = new List<MonitoringListViewModel>();
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var tempListRequestIdFromSampling = new List<int>();
            var tempListRequestIdFromTesting = new List<int>();
            var listRequestIdFromTesting = new List<int>();

            if ((facilityId != null) && (facilityId != 0))
            {
                var getRequestId = await _FindRequestIdByFacilityId(facilityId);

                tempListRequestIdFromSampling = await (
                    from wqs in _context.WorkflowQcSampling
                    join qs in _context.QcSamplings on wqs.QcSamplingId equals qs.Id
                    join rq in _context.RequestQcs on qs.RequestQcsId equals rq.Id
                    where wqs.IsInWorkflow == false
                    && wqs.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2
                    && wqs.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                    && (
                        (EF.Functions.Like(rq.NoRequest.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.ItemName.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.NoBatch.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.ItemName.ToLower(), "%" + filter + "%"))
                       )
                    && rq.RowStatus == null
                    && getRequestId.Contains(rq.Id)
                    select rq.Id
                ).ToListAsync();

                listRequestIdFromTesting = await (
                    from qt in _context.QcTransactionGroups
                    join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                    join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                    join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                    join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                    join rq in _context.RequestQcs on sq.RequestQcsId equals rq.Id
                    where wt.IsInWorkflow != true
                    && wt.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_2
                    && wt.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                    && (
                        (EF.Functions.Like(rq.NoRequest.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.ItemName.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.NoBatch.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.ItemName.ToLower(), "%" + filter + "%"))
                       )
                    && rq.RowStatus == null
                    && getRequestId.Contains(rq.Id)
                    select rq.Id
                ).Distinct().ToListAsync();
            }
            else
            {
                tempListRequestIdFromSampling = await (
                    from wqs in _context.WorkflowQcSampling
                    join qs in _context.QcSamplings on wqs.QcSamplingId equals qs.Id
                    join rq in _context.RequestQcs on qs.RequestQcsId equals rq.Id
                    where wqs.IsInWorkflow == false
                    && wqs.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_SAMPLING_PHASE_2
                    && wqs.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                    && (
                        (EF.Functions.Like(rq.NoRequest.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.ItemName.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.NoBatch.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.ItemName.ToLower(), "%" + filter + "%"))
                       )
                    && rq.RowStatus == null
                    select rq.Id
                ).ToListAsync();

                listRequestIdFromTesting = await (
                    from qt in _context.QcTransactionGroups
                    join wt in _context.WorkflowQcTransactionGroup on qt.Id equals wt.QcTransactionGroupId
                    join ts in _context.QcTransactionSamples on qt.Id equals ts.QcTransactionGroupId
                    join qs in _context.QcSamples on ts.QcSampleId equals qs.Id
                    join sq in _context.QcSamplings on qs.QcSamplingId equals sq.Id
                    join rq in _context.RequestQcs on sq.RequestQcsId equals rq.Id
                    where wt.IsInWorkflow != true
                    && wt.WorkflowCode == ApplicationConstant.WORKFLOW_CODE_TESTING_PHASE_2
                    && wt.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_COMPLETE_NAME
                    && (
                        (EF.Functions.Like(rq.NoRequest.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.ItemName.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.NoBatch.ToLower(), "%" + filter + "%"))
                        || (EF.Functions.Like(rq.ItemName.ToLower(), "%" + filter + "%"))
                       )
                    && rq.RowStatus == null
                    select rq.Id
                ).Distinct().ToListAsync();
            }

            var listRequestIdFromSampling = (
                from s in tempListRequestIdFromSampling
                group s by s into g
                where g.Count() == 2
                select g.First()
            ).ToList();

            var resultDataByNik = new List<MonitoringListViewModel>();
            var matchedRequestId = new List<int>();
            var resultList = new List<MonitoringListViewModel>();

            var resultToDo = new List<MonitoringListViewModel>();

            foreach (var item2 in listRequestIdFromSampling.Distinct().ToList().GroupBy(x => x))
            {
                if (listRequestIdFromTesting.GroupBy(x => x).Where(y => y.Key == item2.Key).Any())
                {
                    matchedRequestId.Add(item2.Key);
                }
            }

            var resultData = new List<MonitoringListViewModel>();

            if (limit > 0)
            {
                resultData = (from p in _context.RequestQcs
                              where ((EF.Functions.Like(p.NoRequest.ToLower(), "%" + filter + "%")) ||
                              (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")) ||
                              (EF.Functions.Like(p.NoBatch.ToLower(), "%" + filter + "%")) ||
                              (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")))
                              && p.RowStatus == null
                              && matchedRequestId.Contains(p.Id)
                              select new MonitoringListViewModel
                              {
                                  Id = p.Id,
                                  Date = p.Date,
                                  NoRequest = p.NoRequest,
                                  TypeRequestId = p.TypeRequestId,
                                  TypeRequest = p.TypeRequest,
                                  ReceiptDate = p.ReceiptDate,
                                  ReceiptDateQA = p.ReceiptDateQA,
                                  ReceiptDateKabag = p.ReceiptDateKabag,
                                  NoBatch = p.NoBatch,
                                  ItemId = p.ItemId,
                                  ItemName = p.ItemName,
                                  TypeFormId = p.TypeFormId,
                                  TypeFormName = p.TypeFormName,
                                  PhaseId = p.EmPhaseId,
                                  PhaseName = p.EmPhaseName,
                                  RoomName = p.EmRoomName,
                                  RoomId = p.EmRoomId,
                                  Status = p.Status,
                                  NoDeviation = p.NoDeviation,
                                  Conclusion = p.Conclusion,
                                  WorkflowStatus = p.WorkflowStatus,
                                  OrgId = p.OrgId,
                                  OrgName = p.OrgName,
                                  CreatedAt = p.CreatedAt,
                                  CreatedBy = p.CreatedBy,
                                  Sampling = (from qrs in _context.QcSamplings
                                              where p.Id == qrs.RequestQcsId
                                              select new MonitoringSamplingListViewModel
                                              {
                                                  SamplingId = qrs.Id,
                                                  Code = qrs.Code,
                                                  // check sample receive date
                                                  ReceiptDate = (from qss in _context.QcSamplingShipments
                                                                 join qst in _context.QcSamplingShipmentTrackers on qss.Id equals qst.QcSamplingShipmentId
                                                                 where qss.QcSamplingId == qrs.Id
                                                                 && qst.Type == ApplicationConstant.TRACKER_TYPE_RECEIVE
                                                                 select qst.processAt
                                                                  ).FirstOrDefault(),
                                                  NoRequest = p.NoRequest,
                                                  TypeRequestId = p.TypeRequestId,
                                                  TypeRequest = p.TypeRequest,
                                                  SamplingTypeId = qrs.SamplingTypeId,
                                                  SamplingTypeName = qrs.SamplingTypeName,
                                                  Process = ApplicationConstant.PROCESS_STATUS_PHASE_REQUEST,
                                                  Status = p.Status,
                                                  StatusRequest = p.Status,
                                                  StatusSampling = qrs.Status,
                                                  StatusTransfer = (from qss in _context.QcSamplingShipments
                                                                    where qss.QcSamplingId == qrs.Id
                                                                    select qss.Status
                                                                   ).FirstOrDefault(),
                                                  StatusTesting = (from qs in _context.QcSamples
                                                                   join qts in _context.QcTransactionSamples on qs.Id equals qts.QcSampleId
                                                                   join qtg in _context.QcTransactionGroups on qts.QcTransactionGroupId equals qtg.Id
                                                                   where qs.QcSamplingId == qrs.Id
                                                                   select qtg.Status
                                                                  ).FirstOrDefault(),
                                                  // process phase, cek di workflow masing2 phase, mundur dari step paling akhir dulu
                                                  // TODO: baru sampe transfer, nunggu implemen testing


                                              }).ToList(),
                                  RequestPurposes = (from rp in _context.RequestPurposes
                                                     join purp in _context.TransactionPurposes on rp.PurposeId equals purp.Id
                                                     where rp.QcRequestId == p.Id
                                                     select new RequestPurposesViewModel
                                                     {
                                                         PurposeId = purp.Id,
                                                         PurposeCode = purp.Code,
                                                         PurposeName = purp.Name
                                                     }).ToList()
                              }).OrderByDescending(x => x.CreatedAt).ToList().Skip(page).Take(limit).ToList();
            }
            else
            {
                resultData = resultData = (from p in _context.RequestQcs
                                           where ((EF.Functions.Like(p.NoRequest.ToLower(), "%" + filter + "%")) ||
                                           (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")) ||
                                           (EF.Functions.Like(p.NoBatch.ToLower(), "%" + filter + "%")) ||
                                           (EF.Functions.Like(p.ItemName.ToLower(), "%" + filter + "%")))
                                           && p.RowStatus == null
                                           && matchedRequestId.Contains(p.Id)
                                           select new MonitoringListViewModel
                                           {
                                               Id = p.Id,
                                               Date = p.Date,
                                               NoRequest = p.NoRequest,
                                               TypeRequestId = p.TypeRequestId,
                                               TypeRequest = p.TypeRequest,
                                               ReceiptDate = p.ReceiptDate,
                                               ReceiptDateQA = p.ReceiptDateQA,
                                               ReceiptDateKabag = p.ReceiptDateKabag,
                                               NoBatch = p.NoBatch,
                                               ItemId = p.ItemId,
                                               ItemName = p.ItemName,
                                               TypeFormId = p.TypeFormId,
                                               TypeFormName = p.TypeFormName,
                                               PhaseId = p.EmPhaseId,
                                               PhaseName = p.EmPhaseName,
                                               RoomName = p.EmRoomName,
                                               RoomId = p.EmRoomId,
                                               Status = p.Status,
                                               NoDeviation = p.NoDeviation,
                                               Conclusion = p.Conclusion,
                                               WorkflowStatus = p.WorkflowStatus,
                                               OrgId = p.OrgId,
                                               OrgName = p.OrgName,
                                               CreatedAt = p.CreatedAt,
                                               CreatedBy = p.CreatedBy,
                                               Sampling = (from qrs in _context.QcSamplings
                                                           where p.Id == qrs.RequestQcsId
                                                           select new MonitoringSamplingListViewModel
                                                           {
                                                               SamplingId = qrs.Id,
                                                               Code = qrs.Code,
                                                               // check sample receive date
                                                               ReceiptDate = (from qss in _context.QcSamplingShipments
                                                                              join qst in _context.QcSamplingShipmentTrackers on qss.Id equals qst.QcSamplingShipmentId
                                                                              where qss.QcSamplingId == qrs.Id
                                                                              && qst.Type == ApplicationConstant.TRACKER_TYPE_RECEIVE
                                                                              select qst.processAt
                                                                               ).FirstOrDefault(),
                                                               NoRequest = p.NoRequest,
                                                               TypeRequestId = p.TypeRequestId,
                                                               TypeRequest = p.TypeRequest,
                                                               SamplingTypeId = qrs.SamplingTypeId,
                                                               SamplingTypeName = qrs.SamplingTypeName,
                                                               Process = ApplicationConstant.PROCESS_STATUS_PHASE_REQUEST,
                                                               Status = p.Status,
                                                               StatusRequest = p.Status,
                                                               StatusSampling = qrs.Status,
                                                               StatusTransfer = (from qss in _context.QcSamplingShipments
                                                                                 where qss.QcSamplingId == qrs.Id
                                                                                 select qss.Status
                                                                                ).FirstOrDefault(),
                                                               StatusTesting = (from qs in _context.QcSamples
                                                                                join qts in _context.QcTransactionSamples on qs.Id equals qts.QcSampleId
                                                                                join qtg in _context.QcTransactionGroups on qts.QcTransactionGroupId equals qtg.Id
                                                                                where qs.QcSamplingId == qrs.Id
                                                                                select qtg.Status
                                                                               ).FirstOrDefault(),
                                                               // process phase, cek di workflow masing2 phase, mundur dari step paling akhir dulu
                                                               // TODO: baru sampe transfer, nunggu implemen testing


                                                           }).ToList(),
                                               RequestPurposes = (from rp in _context.RequestPurposes
                                                                  join purp in _context.TransactionPurposes on rp.PurposeId equals purp.Id
                                                                  where rp.QcRequestId == p.Id
                                                                  select new RequestPurposesViewModel
                                                                  {
                                                                      PurposeId = purp.Id,
                                                                      PurposeCode = purp.Code,
                                                                      PurposeName = purp.Name
                                                                  }).ToList()
                                           }).OrderByDescending(x => x.CreatedAt).ToList().ToList();
            }

            if (resultData.Any())
            {
                for (int i = 0; i < resultData.Count(); i++)
                {
                    for (int j = 0; j < resultData[i].Sampling.Count(); j++)
                    {
                        // by default prosesnya request, statusnya sama dengan status request, nanti diupdate disesuaiin yg terakhir

                        // Sampling Type == EM-M
                        if (resultData[i].Sampling[j].SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_EMM)
                        {

                            // Test
                            if (!(
                                    resultData[i].Sampling[j].StatusTesting == ApplicationConstant.STATUS_TEST_INREVIEW_KABAG_PRODUKSI
                                    || resultData[i].Sampling[j].StatusTesting == ApplicationConstant.STATUS_TEST_INREVIEW_QA
                                    || resultData[i].Sampling[j].StatusTesting == ApplicationConstant.STATUS_TEST_APPROVED
                                    || resultData[i].Sampling[j].StatusTesting == null
                                ))
                            {
                                resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_TESTING;
                                resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusTesting;
                                continue;
                            }

                            // Transfer
                            if (resultData[i].Sampling[j].StatusTransfer != ApplicationConstant.STATUS_SHIPMENT_RECEIVED)
                            {
                                if (resultData[i].Sampling[j].StatusSampling != ApplicationConstant.STATUS_IN_REVIEW_KABAG)
                                {
                                    resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                    resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;
                                    continue;
                                }
                                else
                                {
                                    if (resultData[i].Sampling[j].StatusTransfer == null)
                                    {
                                        resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                        resultData[i].Sampling[j].Status = ApplicationConstant.STATUS_READY_TO_TRANSFER;
                                        continue;
                                    }
                                    else
                                    {
                                        resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_TRANSFER;
                                        resultData[i].Sampling[j].Status = resultData[i].Sampling[j].StatusTransfer != null ? resultData[i].Sampling[j].StatusTransfer : resultData[i].Sampling[j].Status;
                                        continue;
                                    }
                                }
                            }

                            if (resultData[i].Sampling[j].StatusTransfer == ApplicationConstant.STATUS_SHIPMENT_RECEIVED)
                            {
                                if (resultData[i].Sampling[j].StatusTesting == null)
                                {
                                    resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_TRANSFER;
                                    resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusTransfer;
                                    continue;
                                }
                            }

                            // Sampling, udah lolos request
                            if (!(
                                    resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_APPROVED
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_KABAG
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_QA
                                ))
                            {
                                resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;
                                continue;
                            }

                            // Result
                            resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_RESULT;
                            resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusTesting;
                        }

                        // Sampling Type == EM-PC
                        else if (resultData[i].Sampling[j].SamplingTypeId == ApplicationConstant.REQUEST_SAPMLING_PC)
                        {
                            // Sampling, udah lolos request
                            if (!(
                                    resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_APPROVED
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_KABAG
                                    || resultData[i].Sampling[j].StatusSampling == ApplicationConstant.STATUS_IN_REVIEW_QA
                                ))
                            {
                                resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_SAMPLING;
                                resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;
                                continue;
                            }

                            // Result
                            resultData[i].Sampling[j].Process = ApplicationConstant.PROCESS_STATUS_PHASE_RESULT;
                            resultData[i].Sampling[j].Status = (int)resultData[i].Sampling[j].StatusSampling;
                        }
                    }
                }
            }

            return resultData;
        }

        private async Task<List<int>> _FindRequestIdByFacilityId(int? facilityId)
        {
            return await (
                            from fr in _context.RoomFacilities
                            join rr in _context.RequestRooms on fr.RoomId equals rr.RoomId
                            where fr.FacilityId == facilityId
                            select rr.QcRequestId
            ).ToListAsync();
        }
    }
}
