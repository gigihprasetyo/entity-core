using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using qcs_product.API.BindingModels;
using qcs_product.API.Helpers;
using qcs_product.API.Infrastructure;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using qcs_product.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders.Collection
{
    public class SamplingShipmentDataProvider : ISamplingShipmentDataProvider
    {
        private readonly QcsProductContext _context;
        private readonly ILogger<SamplingShipmentDataProvider> _logger;

        [ExcludeFromCodeCoverage]
        public SamplingShipmentDataProvider(QcsProductContext context, ILogger<SamplingShipmentDataProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<QcSamplingShipmentRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int fromOrgId, int toOrgId, int qcSamplingId)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = (from s in _context.QcSamplingShipments
                          join sm in _context.QcSamplings on s.QcSamplingId equals sm.Id
                          join r in _context.RequestQcs on sm.RequestQcsId equals r.Id
                          where ((EF.Functions.Like(s.QrCode.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(r.NoRequest.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(r.NoBatch.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(r.TypeRequest.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(sm.SamplingTypeName.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(r.ItemName.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(r.EmPhaseName.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(s.TestParamName.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(s.FromOrganizationName.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(s.ToOrganizationName.ToLower(), "%" + filter + "%")))
                          && status.Contains(s.Status)
                          && s.RowStatus == null
                          select new QcSamplingShipmentRelationViewModel
                          {
                              Id = s.Id,
                              QrCode = s.QrCode,
                              NoRequest = r.NoRequest,
                              NoBatch = r.NoBatch,
                              TypeRequestId = r.TypeRequestId,
                              TypeRequest = r.TypeRequest,
                              SamplingTypeId = sm.SamplingTypeId,
                              SamplingTypeName = sm.SamplingTypeName,
                              ItemId = r.ItemId,
                              ItemName = r.ItemName,
                              EmPhaseId = r.EmPhaseId,
                              EmPhaseName = r.EmPhaseName,
                              TestParamId = s.TestParamId,
                              TestParamName = s.TestParamName,
                              FromOrganizationId = s.FromOrganizationId,
                              FromOrganizationName = s.FromOrganizationName,
                              ToOrganizationId = s.ToOrganizationId,
                              ToOrganizationName = s.ToOrganizationName,
                              StartDate = s.StartDate,
                              EndDate = s.EndDate,
                              IsLateTransfer = s.IsLateTransfer,
                              Status = s.Status,
                              QcSamplingId = s.QcSamplingId,
                              LastSamplingDateTime = (from samp in _context.QcSamples
                                                      where samp.QcSamplingId == sm.Id
                                                      && samp.RowStatus == null
                                                      select samp.SamplingDateTimeTo).DefaultIfEmpty().Max(x => x == null ? null : x),
                              ShipmentNote = sm.ShipmentNote,
                              CreatedBy = s.CreatedBy,
                              CreatedAt = s.CreatedAt,
                              ShipmentTrackers = (from st in _context.QcSamplingShipmentTrackers
                                                  where st.QcSamplingShipmentId == s.Id &&
                                                  st.RowStatus == null
                                                  select new QcSamplingShipmentTranckerViewModel
                                                  {
                                                      Id = st.Id,
                                                      QrCode = st.QrCode,
                                                      Type = st.Type,
                                                      processAt = (DateTime)st.processAt,
                                                      UserNik = st.UserNik,
                                                      UserName = st.UserName,
                                                      OrganizationId = st.OrganizationId,
                                                      OrganizationName = st.OrganizationName,
                                                      CreatedAt = st.CreatedAt
                                                  }).OrderByDescending(x => x.CreatedAt).ToList()
                          }).Where(x =>
                            ((x.StartDate >= startDate || !startDate.HasValue) && (x.StartDate <= endDate || !endDate.HasValue)) &&
                            (x.FromOrganizationId == fromOrgId || fromOrgId == 0) &&
                            (x.ToOrganizationId == toOrgId || toOrgId == 0) &&
                            (x.QcSamplingId == qcSamplingId || qcSamplingId == 0)
                            ).OrderByDescending(x => x.CreatedAt).AsQueryable();

            var resultData = new List<QcSamplingShipmentRelationViewModel>();

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

        public async Task<List<QcShipmentLateViewModel>> ListLate(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int OrgId, string nik)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = (from sm in _context.QcSamplings
                          join r in _context.RequestQcs on sm.RequestQcsId equals r.Id
                          join rr in _context.RequestRooms on r.Id equals rr.QcRequestId
                          join room in _context.TransactionRoom on rr.RoomId equals room.Id
                          join o in _context.TransactionOrganization on room.OrganizationId equals o.Id
                          join sh in _context.QcSamplingShipments on sm.Id equals sh.QcSamplingId
                          join wqc in _context.WorkflowQcSampling on sm.Id equals wqc.QcSamplingId
                          join wh in _context.WorkflowHistories on wqc.WorkflowDocumentCode equals wh.WorkflowDocumentCode
                          where ((EF.Functions.Like(sm.Code.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(r.NoRequest.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(r.NoBatch.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(r.TypeRequest.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(sm.SamplingTypeName.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(r.ItemName.ToLower(), "%" + filter + "%")) ||
                          (EF.Functions.Like(r.EmPhaseName.ToLower(), "%" + filter + "%")))
                          && status.Contains(sh.Status)
                          && sm.RowStatus == null
                          && sh.IsLateTransfer == true
                          && wh.PicNik == nik
                          && wh.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KASIE
                          && o.BiohrOrganizationId == OrgId
                          group new { sm, r, sh } by new { sm.Id } into g
                          select new QcShipmentLateViewModel
                          {
                              QcRequestId = g.Max(x => x.r.Id),
                              QcSamplingId = g.Max(x => x.sm.Id),
                              QcShipmentId = g.Max(x => x.sh.Id),
                              NoRequest = g.Max(x => x.r.NoRequest),
                              NoBatch = g.Max(x => x.r.NoBatch),
                              QrSamplingCode = g.Max(x => x.sm.Code),
                              QrShipmentCode = g.Max(x => x.sh.QrCode),
                              ItemId = g.Max(x => x.r.ItemId),
                              ItemName = g.Max(x => x.r.ItemName),
                              TypeRequestId = g.Max(x => x.r.TypeRequestId),
                              TypeRequest = g.Max(x => x.r.TypeRequest),
                              SamplingTypeId = g.Max(x => x.sm.SamplingTypeId),
                              SamplingTypeName = g.Max(x => x.sm.SamplingTypeName),
                              EmPhaseId = g.Max(x => x.r.EmPhaseId),
                              EmPhaseName = g.Max(x => x.r.EmPhaseName),
                              ShipmentStartDate = g.Max(x => x.sh.StartDate),
                              ShipmentEndDate = g.Max(x => x.sh.EndDate),
                              FromOrganizationId = g.Max(x => x.sh.FromOrganizationId),
                              FromOrganizationName = g.Max(x => x.sh.FromOrganizationName),
                              ToOrganizationId = g.Max(x => x.sh.ToOrganizationId),
                              ToOrganizationName = g.Max(x => x.sh.ToOrganizationName),
                              OrgId = g.Max(x => x.r.OrgId),
                              Status = g.Max(x => x.sh.Status),
                              ShipmentNote = g.Max(x => x.sm.ShipmentNote),
                              ShipmentApprovalDate = g.Max(x => x.sm.ShipmentApprovalDate),
                              ShipmentApprovalBy = g.Max(x => x.sm.ShipmentApprovalBy),
                              //   ListOrgByRequest = (from room in _context.TransactionRoom
                              //                       join rr in _context.RequestRooms on room.Id equals rr.RoomId
                              //                       join o in _context.TransactionOrganization on room.OrganizationId equals o.Id
                              //                       where rr.QcRequestId == g.Max(x => x.r.Id)
                              //                       select o.BiohrOrganizationId).ToList(),
                          }).Where(x =>
                            ((x.ShipmentStartDate >= startDate || !startDate.HasValue) && (x.ShipmentStartDate <= endDate || !endDate.HasValue))
                            //&&
                            //(x.ListOrgByRequest.Contains(OrgId)
                            //(x.IsLateTransfer == isLateTransfer)
                            ).OrderBy(x => x.ShipmentStartDate).AsQueryable();


            var resultData = new List<QcShipmentLateViewModel>();

            if (limit > 0)
            {
                resultData = await result.Skip(page).Take(limit).ToListAsync();
            }
            else
            {
                resultData = await result.ToListAsync();
            }

            if (resultData.Any())
            {
                foreach (var itm in resultData)
                {
                    /* Get Last sampling time */
                    itm.LastSamplingDateTime = (from samp in _context.QcSamples
                                                where samp.QcSamplingId == itm.QcSamplingId
                                                && samp.RowStatus == null
                                                select samp.SamplingDateTimeTo).DefaultIfEmpty().Max(x => x == null ? null : x);

                    /* Get List Concat Test Param */
                    var ListTFTestParam = (from shtp in _context.QcSamplingShipments
                                           where shtp.QcSamplingId == itm.QcSamplingId
                                           && shtp.RowStatus == null
                                           select shtp.TestParamName).ToList();

                    if (ListTFTestParam.Any())
                    {
                        itm.TestParameterSamplings = String.Join(", ", ListTFTestParam);
                    }

                    /* Shipment Tracker */
                    itm.ShipmentTrackers = (from st in _context.QcSamplingShipmentTrackers
                                            where st.QcSamplingShipmentId == itm.QcShipmentId
                                            && st.RowStatus == null
                                            select new QcSamplingShipmentTranckerViewModel
                                            {
                                                Id = st.Id,
                                                QrCode = st.QrCode,
                                                Type = st.Type,
                                                processAt = (DateTime)st.processAt,
                                                UserNik = st.UserNik,
                                                UserName = st.UserName,
                                                OrganizationId = st.OrganizationId,
                                                OrganizationName = st.OrganizationName,
                                                CreatedAt = st.CreatedAt
                                            }).OrderByDescending(x => x.CreatedAt).ToList();


                }
            }

            return resultData;
        }

        public async Task<List<QcSamplingTransferViewModel>> ListTransfer(string search, int limit, int page, DateTime? startDate, DateTime? endDate, int status)
        {
            string filter = "";
            if (search != null)
            {
                filter = search.ToLower();
            }
                
            var statusFilter = new List<int>();
            if (status == 1)
            {
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_SENDING);
            }
            else if (status == 2)
            {
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_RECEIVED);
            } 
            else {
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_SENDING);
                statusFilter.Add(ApplicationConstant.STATUS_SHIPMENT_RECEIVED);
            }

            var result = (from ss in _context.QcSamplingShipments
                          join s in _context.QcSamplings on ss.QcSamplingId equals s.Id
                          join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                          where (
                              (EF.Functions.Like(r.NoRequest.ToLower(), "%" + filter + "%")) ||
                              (EF.Functions.Like(r.NoBatch.ToLower(), "%" + filter + "%")) ||
                              (EF.Functions.Like(r.ItemName.ToLower(), "%" + filter + "%")) ||
                              (EF.Functions.Like(ss.TestParamName.ToLower(), "%" + filter + "%"))
                          )
                          && statusFilter.Contains(ss.Status)
                          select new QcSamplingTransferViewModel
                          {
                              Id = ss.Id,
                              SamplingId = ss.QcSamplingId,
                              QrCode = ss.QrCode,
                              NoRequest = r.NoRequest,
                              NoBatch = r.NoBatch,
                              TypeRequestId = r.TypeRequestId,
                              TypeRequest = r.TypeRequest,
                              SamplingTypeId = s.SamplingTypeId,
                              SamplingTypeName = s.SamplingTypeName,
                              ItemId = r.ItemId,
                              ItemName = r.ItemName,
                              EmPhaseId = r.EmPhaseId,
                              EmPhaseName = r.EmPhaseName,
                              TestParamId = ss.TestParamId,
                              TestParamName = ss.TestParamName,
                              fromOrgId = ss.FromOrganizationId,
                              fromOrgName = ss.FromOrganizationName,
                              toOrgId = ss.ToOrganizationId,
                              toOrgName = ss.ToOrganizationName,
                              SamplingDateFrom = s.SamplingDateFrom,
                              SamplingDateTo = s.SamplingDateTo,
                              CreatedBy = s.CreatedBy,
                              CreatedAt = s.CreatedAt,
                              StartDate = (DateTime)ss.StartDate,
                              EndDate = (DateTime)ss.EndDate,
                              StatusId = ss.Status,
                              StatusName = (int)ss.Status == (int)ApplicationConstant.STATUS_SHIPMENT_RECEIVED ? ApplicationConstant.TRACKER_TYPE_RECEIVE : ApplicationConstant.TRACKER_TYPE_SEND,
                          })
                          .Where(x =>
                            ((x.StartDate >= startDate || !startDate.HasValue) && (x.StartDate <= endDate || !endDate.HasValue))
                           )
                          .OrderByDescending(x => x.StartDate).AsQueryable();

            var resultData = new List<QcSamplingTransferViewModel>();

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

        public async Task<QcShipmentLateViewModel> GetTransferLateBySamplingId(int qcSamplingId, string nik)
        {
            var result = await (from sm in _context.QcSamplings
                                join r in _context.RequestQcs on sm.RequestQcsId equals r.Id
                                join sh in _context.QcSamplingShipments on sm.Id equals sh.QcSamplingId
                                join wqc in _context.WorkflowQcSampling on sm.Id equals wqc.QcSamplingId
                                join wh in _context.WorkflowHistories on wqc.WorkflowDocumentCode equals wh.WorkflowDocumentCode
                                where sh.QcSamplingId == qcSamplingId
                                && sm.RowStatus == null
                                && sh.IsLateTransfer == true
                                && wh.WorkflowStatus == ApplicationConstant.WORKFLOW_STATUS_NAME_REVIEW_KASIE
                                group new { sm, r, sh, wh } by new { sm.Id } into g
                                select new QcShipmentLateViewModel
                                {
                                    QcRequestId = g.Max(x => x.r.Id),
                                    QcSamplingId = g.Max(x => x.sm.Id),
                                    QcShipmentId = g.Max(x => x.sh.Id),
                                    NoRequest = g.Max(x => x.r.NoRequest),
                                    NoBatch = g.Max(x => x.r.NoBatch),
                                    QrSamplingCode = g.Max(x => x.sm.Code),
                                    QrShipmentCode = g.Max(x => x.sh.QrCode),
                                    ItemId = g.Max(x => x.r.ItemId),
                                    ItemName = g.Max(x => x.r.ItemName),
                                    TypeRequestId = g.Max(x => x.r.TypeRequestId),
                                    TypeRequest = g.Max(x => x.r.TypeRequest),
                                    SamplingTypeId = g.Max(x => x.sm.SamplingTypeId),
                                    SamplingTypeName = g.Max(x => x.sm.SamplingTypeName),
                                    EmPhaseId = g.Max(x => x.r.EmPhaseId),
                                    EmPhaseName = g.Max(x => x.r.EmPhaseName),
                                    ShipmentStartDate = g.Max(x => x.sh.StartDate),
                                    ShipmentEndDate = g.Max(x => x.sh.EndDate),
                                    FromOrganizationId = g.Max(x => x.sh.FromOrganizationId),
                                    FromOrganizationName = g.Max(x => x.sh.FromOrganizationName),
                                    ToOrganizationId = g.Max(x => x.sh.ToOrganizationId),
                                    ToOrganizationName = g.Max(x => x.sh.ToOrganizationName),
                                    Status = g.Max(x => x.sh.Status),
                                    OrgId = g.Max(x => x.r.OrgId),
                                    ShipmentNote = g.Max(x => x.sm.ShipmentNote),
                                    ShipmentApprovalDate = g.Max(x => x.sm.ShipmentApprovalDate),
                                    ShipmentApprovalBy = g.Max(x => x.sm.ShipmentApprovalBy),
                                    isAllowedReview = g.Max(x => x.wh.PicNik) == nik ? true : false
                                }).FirstOrDefaultAsync();

            if (result != null)
            {
                /* Get Last sampling time */
                result.LastSamplingDateTime = (from samp in _context.QcSamples
                                               where samp.QcSamplingId == result.QcSamplingId
                                               && samp.RowStatus == null
                                               select samp.SamplingDateTimeTo).DefaultIfEmpty().Max(x => x == null ? null : x);

                /* Get List Concat Test Param */
                var ListTFTestParam = (from shtp in _context.QcSamplingShipments
                                       where shtp.QcSamplingId == result.QcSamplingId
                                       && shtp.RowStatus == null
                                       select shtp.TestParamName).ToList();

                if (ListTFTestParam.Any())
                {
                    result.TestParameterSamplings = String.Join(", ", ListTFTestParam);
                }

                /* Shipment Tracker */
                result.ShipmentTrackers = (from st in _context.QcSamplingShipmentTrackers
                                           where st.QcSamplingShipmentId == result.QcShipmentId
                                                       && st.RowStatus == null
                                           select new QcSamplingShipmentTranckerViewModel
                                           {
                                               Id = st.Id,
                                               QrCode = st.QrCode,
                                               Type = st.Type,
                                               processAt = (DateTime)st.processAt,
                                               UserNik = st.UserNik,
                                               UserName = st.UserName,
                                               OrganizationId = st.OrganizationId,
                                               OrganizationName = st.OrganizationName,
                                               CreatedAt = st.CreatedAt
                                           }).OrderByDescending(x => x.CreatedAt).ToList();

                #region add requestRooms
                result.RequestRooms = (from rr in _context.RequestRooms
                                       where rr.QcRequestId == result.QcRequestId
                                       select new RequestRoomRelationViewModel
                                       {
                                           Id = rr.Id,
                                           RoomId = rr.RoomId,
                                           RoomCode = rr.RoomCode,
                                           RoomName = rr.RoomName,
                                           GradeRoomId = rr.GradeRoomId,
                                           GradeRoomCode = rr.GradeRoomCode,
                                           GradeRoomName = rr.GradeRoomName,

                                           // Get total per Test Parameter    
                                           TestParametersCount = (from tp in _context.TransactionTestParameter
                                                                  where tp.Id != ApplicationConstant.TEST_PARAMETER_PC05
                                                                  && tp.Id != ApplicationConstant.TEST_PARAMETER_PC50
                                                                  select new TestParameterRelationViewModel
                                                                  {
                                                                      Id = tp.Id,
                                                                      Name = tp.Name,
                                                                      ShortName = tp.ShortName,
                                                                      Count = tp.Id == ApplicationConstant.TEST_PARAMETER_FD ?

                                                                      (from sml in _context.QcSamples
                                                                       where sml.QcSamplingId == result.QcSamplingId
                                                                       && sml.TestParamId == ApplicationConstant.TEST_PARAMETER_FD
                                                                       group sml by sml.PersonalInitial into g
                                                                       select g.Max(x => x.Id)).Count()

                                                                       :

                                                                      (from sml in _context.QcSamples
                                                                       join rrsp in _context.TransactionRelRoomSamplingPoint
                                                                       on sml.SamplingPointId equals rrsp.SamplingPoinId
                                                                       join room_purp in _context.TransactionRoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                                       where sml.TestParamId == tp.Id
                                                                       && sml.QcSamplingId == result.QcSamplingId
                                                                       && sml.RowStatus == null
                                                                       && sml.ParentId == null
                                                                       && room_purp.RoomId == rr.RoomId
                                                                       select sml).Union(from sml in _context.QcSamples
                                                                                         join rst in _context.TransactionRelSamplingTool on sml.SamplingPointId equals rst.SamplingPoinId
                                                                                         join tps in _context.TransactionToolPurpose on rst.ToolPurposeId equals tps.Id
                                                                                         join t in _context.Tools on tps.ToolId equals t.Id
                                                                                         where sml.TestParamId == tp.Id
                                                                                         && sml.QcSamplingId == result.QcSamplingId
                                                                                         && sml.RowStatus == null
                                                                                         && sml.ParentId == null
                                                                                         && t.RoomId == rr.RoomId
                                                                                         select sml).Count(),
                                                                  }).ToList(),
                                       }).ToList();
                #endregion
            }



            return result;
        }

        public async Task<QcSamplingShipmentRelationViewModel> GetById(int id)
        {

            var result = await (from s in _context.QcSamplingShipments
                                join sm in _context.QcSamplings on s.QcSamplingId equals sm.Id
                                join r in _context.RequestQcs on sm.RequestQcsId equals r.Id
                                where s.Id == id
                                && s.RowStatus == null
                                select new QcSamplingShipmentRelationViewModel
                                {
                                    Id = s.Id,
                                    QrCode = s.QrCode,
                                    NoRequest = s.NoRequest,
                                    NoBatch = r.NoBatch,
                                    TypeRequestId = r.TypeRequestId,
                                    TypeRequest = r.TypeRequest,
                                    SamplingTypeId = sm.SamplingTypeId,
                                    SamplingTypeName = sm.SamplingTypeName,
                                    ItemId = r.ItemId,
                                    ItemName = r.ItemName,
                                    EmPhaseId = r.EmPhaseId,
                                    EmPhaseName = r.EmPhaseName,
                                    TestParamId = s.TestParamId,
                                    TestParamName = s.TestParamName,
                                    FromOrganizationId = s.FromOrganizationId,
                                    FromOrganizationName = s.FromOrganizationName,
                                    ToOrganizationId = s.ToOrganizationId,
                                    ToOrganizationName = s.ToOrganizationName,
                                    StartDate = s.StartDate,
                                    EndDate = s.EndDate,
                                    IsLateTransfer = s.IsLateTransfer,
                                    Status = s.Status,
                                    LastSamplingDateTime = (from samp in _context.QcSamples
                                                            where samp.QcSamplingId == sm.Id
                                                            && samp.RowStatus == null
                                                            select samp.SamplingDateTimeTo).DefaultIfEmpty().Max(x => x == null ? null : x),
                                    ShipmentNote = sm.ShipmentNote,
                                    CreatedBy = s.CreatedBy,
                                    CreatedAt = s.CreatedAt,
                                    ShipmentTrackers = (from st in _context.QcSamplingShipmentTrackers
                                                        where st.QcSamplingShipmentId == id &&
                                                        st.RowStatus == null
                                                        select new QcSamplingShipmentTranckerViewModel
                                                        {
                                                            Id = st.Id,
                                                            QrCode = st.QrCode,
                                                            Type = st.Type,
                                                            processAt = (DateTime)st.processAt,
                                                            UserNik = st.UserNik,
                                                            UserName = st.UserName,
                                                            OrganizationId = st.OrganizationId,
                                                            OrganizationName = st.OrganizationName,
                                                            CreatedAt = st.CreatedAt
                                                        }).OrderByDescending(x => x.CreatedAt).ToList()
                                }).FirstOrDefaultAsync();

            return result;
        }

        public Task<QcSamplingTransferViewModel> GetTransferDetail(Int32 id)
        {
            var result = (from ss in _context.QcSamplingShipments
                          join s in _context.QcSamplings on ss.QcSamplingId equals s.Id
                          join r in _context.RequestQcs on s.RequestQcsId equals r.Id
                          where ss.Id == id
                          select new QcSamplingTransferViewModel
                          {
                              Id = ss.Id,
                              SamplingId = ss.QcSamplingId,
                              QrCode = ss.QrCode,
                              NoRequest = r.NoRequest,
                              NoBatch = r.NoBatch,
                              TypeRequestId = r.TypeRequestId,
                              TypeRequest = r.TypeRequest,
                              SamplingTypeId = s.SamplingTypeId,
                              SamplingTypeName = s.SamplingTypeName,
                              ItemId = r.ItemId,
                              ItemName = r.ItemName,
                              EmPhaseId = r.EmPhaseId,
                              EmPhaseName = r.EmPhaseName,
                              TestParamId = ss.TestParamId,
                              TestParamName = ss.TestParamName,
                              fromOrgId = ss.FromOrganizationId,
                              fromOrgName = ss.FromOrganizationName,
                              toOrgId = ss.ToOrganizationId,
                              toOrgName = ss.ToOrganizationName,
                              SamplingDateFrom = s.SamplingDateFrom,
                              SamplingDateTo = s.SamplingDateTo,
                              CreatedBy = s.CreatedBy,
                              CreatedAt = s.CreatedAt,
                              StartDate = (DateTime)ss.StartDate,
                              EndDate = (DateTime)ss.EndDate,
                              StatusId = ss.Status,
                              StatusName = (int)ss.Status == (int)ApplicationConstant.STATUS_SHIPMENT_RECEIVED ? ApplicationConstant.TRACKER_TYPE_RECEIVE : ApplicationConstant.TRACKER_TYPE_SEND,
                              ShipmentTrackers = (from st in _context.QcSamplingShipmentTrackers
                                                  where st.QcSamplingShipmentId == id && st.RowStatus == null
                                                  select new QcSamplingShipmentTranckerViewModel
                                                  {
                                                      Id = st.Id,
                                                      QrCode = st.QrCode,
                                                      Type = st.Type,
                                                      processAt = (DateTime)st.processAt,
                                                      UserNik = st.UserNik,
                                                      UserName = st.UserName,
                                                      OrganizationId = st.OrganizationId,
                                                      OrganizationName = st.OrganizationName,
                                                      CreatedAt = st.CreatedAt
                                                  }).OrderByDescending(x => x.CreatedAt).ToList()
                          }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<QcSamplingShipmentRelationViewModel>> GetByQRCode(string QRCode)
        {
            var result = await (from s in _context.QcSamplingShipments
                                join sm in _context.QcSamplings on s.QcSamplingId equals sm.Id
                                join r in _context.RequestQcs on sm.RequestQcsId equals r.Id
                                where s.QrCode == QRCode
                                && s.RowStatus == null
                                select new QcSamplingShipmentRelationViewModel
                                {
                                    Id = s.Id,
                                    QrCode = s.QrCode,
                                    NoRequest = s.NoRequest,
                                    NoBatch = r.NoBatch,
                                    TypeRequestId = r.TypeRequestId,
                                    TypeRequest = r.TypeRequest,
                                    SamplingTypeId = sm.SamplingTypeId,
                                    SamplingTypeName = sm.SamplingTypeName,
                                    ItemId = r.ItemId,
                                    ItemName = r.ItemName,
                                    EmPhaseId = r.EmPhaseId,
                                    EmPhaseName = r.EmPhaseName,
                                    TestParamId = s.TestParamId,
                                    TestParamName = s.TestParamName,
                                    FromOrganizationId = s.FromOrganizationId,
                                    FromOrganizationName = s.FromOrganizationName,
                                    ToOrganizationId = s.ToOrganizationId,
                                    ToOrganizationName = s.ToOrganizationName,
                                    StartDate = s.StartDate,
                                    EndDate = s.EndDate,
                                    IsLateTransfer = s.IsLateTransfer,
                                    Status = s.Status,
                                    CreatedBy = s.CreatedBy,
                                    CreatedAt = s.CreatedAt,
                                    // ShipmentTrackers = (from st in _context.QcSamplingShipmentTrackers
                                    //                     where st.QcSamplingShipmentId == s.Id &&
                                    //                     st.RowStatus == null
                                    //                     select new QcSamplingShipmentTranckerViewModel
                                    //                     {
                                    //                         Id = st.Id,
                                    //                         QrCode = st.QrCode,
                                    //                         Type = st.Type,
                                    //                         processAt = (DateTime)st.processAt,
                                    //                         UserNik = st.UserNik,
                                    //                         UserName = st.UserName,
                                    //                         OrganizationId = st.OrganizationId,
                                    //                         OrganizationName = st.OrganizationName,
                                    //                         CreatedAt = st.CreatedAt
                                    //                     }).OrderByDescending(x => x.CreatedAt).ToList()
                                }).ToListAsync();

            return result;

        }

        public async Task<QcSamplingShipment> InsertNewShipment(QcSamplingShipment data, QcSamplingShipmentTracker dataTracker, int status)
        {
            QcSamplingShipment result = new QcSamplingShipment();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (status == ApplicationConstant.STATUS_SHIPMENT_SENDING ||
                        status == ApplicationConstant.STATUS_SHIPMENT_LATE_SAMPLE)
                    {
                        //insert shipment header
                        await _context.QcSamplingShipments.AddAsync(data);
                        await _context.SaveChangesAsync();

                        //insert shipment tracker
                        if (dataTracker != null)
                        {
                            dataTracker.QcSamplingShipmentId = data.Id;
                            await _context.QcSamplingShipmentTrackers.AddAsync(dataTracker);
                        }

                        await _context.SaveChangesAsync();
                    }
                    else if (status == ApplicationConstant.STATUS_SHIPMENT_RECEIVED ||
                             status == ApplicationConstant.STATUS_SHIPMENT_LATE_RECIVED)
                    {

                        //update shipment 
                        await _context.SaveChangesAsync();
                        //insert shipment tracker
                        if (dataTracker != null)
                        {
                            dataTracker.QcSamplingShipmentId = data.Id;
                            await _context.QcSamplingShipmentTrackers.AddAsync(dataTracker);
                        }

                        await _context.SaveChangesAsync();
                    }

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

        public async Task<List<QcSamplingShipment>> GetShipmentHeaderByQRCode(string QRCode)
        {
            var result = await (from s in _context.QcSamplingShipments
                                where s.QrCode == QRCode
                                && s.RowStatus == null
                                select s).ToListAsync();

            return result;
        }

        public async Task<List<QcSamplingShipment>> GetBySamplingId(int qcSamplingId)
        {
            var result = await (from s in _context.QcSamplingShipments
                                where s.QcSamplingId == qcSamplingId
                                && s.RowStatus == null
                                select s).ToListAsync();

            return result;
        }

        public async Task<QcSampling> UpdateApprovalSampling(InsertApprovalShipmentBindingModel data)
        {

            var dataSampling = await (from samp in _context.QcSamplings
                                      where samp.Id == data.DataId
                                      && samp.RowStatus == null
                                      select samp).FirstOrDefaultAsync();


            if (dataSampling != null)
            {
                dataSampling.ShipmentApprovalBy = data.NIK;
                dataSampling.ShipmentApprovalDate = DateHelper.Now();
                dataSampling.ShipmentNote = data.Notes;
                dataSampling.UpdatedBy = data.NIK;
                dataSampling.UpdatedAt = DateHelper.Now();

                var dataShipment = await GetBySamplingId(data.DataId);
                if (dataShipment.Any())
                {
                    foreach (var sh in dataShipment)
                    {
                        sh.Status = ApplicationConstant.STATUS_SHIPMENT_LATE_REVIEWED;
                        sh.UpdatedBy = data.NIK;
                        sh.UpdatedAt = DateHelper.Now();
                    }
                }

                await _context.SaveChangesAsync();

            }

            var result = await (from samp in _context.QcSamplings
                                where samp.Id == data.DataId
                                && samp.RowStatus == null
                                select samp).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<QcSamplingShipmentRelationViewModelV2>> ListByBatch(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int fromOrgId, int toOrgId)
        {
            string filter = "";
            if (search != null)
                filter = search.ToLower();

            var result = (from r in _context.RequestQcs
                          join sm in _context.QcSamplings on r.Id equals sm.RequestQcsId
                          join tb in _context.TransactionBatches on r.Id equals tb.RequestQcsId into tbGroup
                          from tb in tbGroup.DefaultIfEmpty()
                          join tbl in _context.TransactionBatchLines on tb.Id equals tbl.TrsBatchId into tblGroup
                          from tbl in tblGroup.DefaultIfEmpty()
                          join ss in _context.QcSamplingShipments on sm.Id equals ss.QcSamplingId into ssGroup
                          from ss in ssGroup.DefaultIfEmpty()
                          where ((EF.Functions.Like(r.NoRequest.ToLower(), "%" + filter + "%")) ||
                                 (EF.Functions.Like(r.NoBatch.ToLower(), "%" + filter + "%")) ||
                                 (EF.Functions.Like(r.TypeRequest.ToLower(), "%" + filter + "%")) ||
                                 (EF.Functions.Like(sm.SamplingTypeName.ToLower(), "%" + filter + "%")) ||
                                 (EF.Functions.Like(r.ItemName.ToLower(), "%" + filter + "%")) ||
                                 (EF.Functions.Like(r.EmPhaseName.ToLower(), "%" + filter + "%"))
                                )
                                && sm.SamplingTypeId == ApplicationConstant.SAMPLING_TYPE_ID_EMM
                          select new QcSamplingShipmentRelationViewModelV2
                          {
                              RequestId = r.Id,
                              NoRequest = r.NoRequest,
                              NoBatch = r.NoBatch,
                              QrCode = sm.Code,
                              TypeRequestId = r.TypeRequestId,
                              TypeRequest = r.TypeRequest,
                              SamplingId = sm.Id,
                              SamplingTypeId = sm.SamplingTypeId,
                              SamplingTypeName = sm.SamplingTypeName,
                              ItemId = r.ItemId,
                              ItemName = r.ItemName,
                              EmPhaseId = r.EmPhaseId,
                              EmPhaseName = r.EmPhaseName,
                              FromOrganizationId = ss.FromOrganizationId,
                              ToOrganizationId = ss.ToOrganizationId,
                              LastSamplingShipmentDate = ss.CreatedAt,
                              IsLateTransfer = ss.IsLateTransfer,
                              StatusSamplingShipments = ss.Status
                          })
              .Where(x =>
                  ((x.LastSamplingShipmentDate >= startDate || !startDate.HasValue) && (x.LastSamplingShipmentDate <= endDate || !endDate.HasValue)) &&
                  (x.FromOrganizationId == fromOrgId || fromOrgId == 0) &&
                  (x.ToOrganizationId == toOrgId || toOrgId == 0) &&
                  (status.Contains(x.StatusSamplingShipments))
              )
              .Distinct()
              .AsQueryable();

            var resultData = await result.ToListAsync();

            //get max LastSamplingShipmentDate by samplingId
            foreach (var item in resultData)
            {
                var lastShipmentDate = resultData.Where(x => x.SamplingId == item.SamplingId)
                    .Select(x => x.LastSamplingShipmentDate)
                    .Max();
                item.LastSamplingShipmentDate = lastShipmentDate;
            }

            //get unique data by samplingId
            resultData = resultData.GroupBy(x => x.SamplingId)
                    .Select(x => x.First())
                    .ToList();

            resultData = resultData
              .OrderByDescending(x => x.LastSamplingShipmentDate.HasValue)
              .ThenByDescending(x => x.LastSamplingShipmentDate)
              .ToList();

            if (limit > 0)
            {
                resultData = resultData
                  .Skip(page).Take(limit)
                  .ToList();
            }

            var samplingIds = resultData.Select(x => x.SamplingId).ToList();

            //get last sampling datetime
            var samplingDates = await (from samp in _context.QcSamples
                                       where samplingIds.Contains(samp.QcSamplingId)
                                           && samp.RowStatus == null
                                       select new
                                       {
                                           SamplingId = samp.QcSamplingId,
                                           SamplingDateTo = samp.SamplingDateTimeTo
                                       }).ToListAsync();

            foreach (var item in resultData)
            {
                var maxSamplingDate = samplingDates
                    .Where(x => x.SamplingId == item.SamplingId)
                    .Select(x => x.SamplingDateTo)
                    .DefaultIfEmpty()
                    .Max();
                item.LastSamplingDateTime = maxSamplingDate;
            }

            return resultData;
        }

        public async Task<QcSamplingShipmentDetailRelationViewModel> GetByRequestQcsId(int requestQcsId)
        {

            var result = await (from s in _context.QcSamplingShipments
                                join sm in _context.QcSamplings on s.QcSamplingId equals sm.Id
                                join r in _context.RequestQcs on sm.RequestQcsId equals r.Id
                                where r.Id == requestQcsId
                                && s.RowStatus == null
                                select new QcSamplingShipmentDetailRelationViewModel
                                {
                                    RequestId = r.Id,
                                    NoRequest = r.NoRequest,
                                    NoBatch = r.NoBatch,
                                    QrCode = sm.Code,
                                    TypeRequestId = r.TypeRequestId,
                                    TypeRequest = r.TypeRequest,
                                    SamplingId = sm.Id,
                                    SamplingTypeId = sm.SamplingTypeId,
                                    SamplingTypeName = sm.SamplingTypeName,
                                    ItemId = r.ItemId,
                                    ItemName = r.ItemName,
                                    EmPhaseId = r.EmPhaseId,
                                    EmPhaseName = r.EmPhaseName,
                                    LastSamplingDateTime = (from samp in _context.QcSamples
                                                            where samp.QcSamplingId == sm.Id
                                                            && samp.RowStatus == null
                                                            select samp.SamplingDateTimeTo).DefaultIfEmpty().Max(x => x == null ? null : x),
                                    ShipmentNote = sm.ShipmentNote,
                                    ShipmentTrackers = (from st in _context.QcSamplingShipmentTrackers
                                                        where st.QcSamplingShipmentId == s.Id &&
                                                        st.RowStatus == null
                                                        select new QcSamplingShipmentTranckerViewModel
                                                        {
                                                            Id = st.Id,
                                                            QrCode = st.QrCode,
                                                            Type = st.Type,
                                                            processAt = (DateTime)st.processAt,
                                                            UserNik = st.UserNik,
                                                            UserName = st.UserName,
                                                            OrganizationId = st.OrganizationId,
                                                            OrganizationName = st.OrganizationName,
                                                            CreatedAt = st.CreatedAt
                                                        }).OrderByDescending(x => x.CreatedAt).ToList()
                                }).FirstOrDefaultAsync();

            // Get Room by Request Id
            if (result != null)
            {
                result.RequestRooms = (from rr in _context.RequestRooms
                                       where rr.QcRequestId == result.RequestId
                                       select new RequestRoomRelationViewModel
                                       {
                                           Id = rr.Id,
                                           RoomId = rr.RoomId,
                                           RoomCode = rr.RoomCode,
                                           RoomName = rr.RoomName,
                                           GradeRoomId = rr.GradeRoomId,
                                           GradeRoomCode = rr.GradeRoomCode,
                                           GradeRoomName = rr.GradeRoomName,

                                           // Get total per Test Parameter    
                                           TestParametersCount = (from tp in _context.TransactionTestParameter
                                                                  where tp.Id != ApplicationConstant.TEST_PARAMETER_PC05
                                                                  && tp.Id != ApplicationConstant.TEST_PARAMETER_PC50
                                                                  select new TestParameterRelationViewModel
                                                                  {
                                                                      Id = tp.Id,
                                                                      Name = tp.Name,
                                                                      ShortName = tp.ShortName,
                                                                      Count = tp.Id == ApplicationConstant.TEST_PARAMETER_FD ?

                                                                      (from sml in _context.QcSamples
                                                                       where sml.QcSamplingId == result.SamplingId
                                                                       && sml.TestParamId == ApplicationConstant.TEST_PARAMETER_FD
                                                                       group sml by sml.PersonalInitial into g
                                                                       select g.Max(x => x.Id)).Count()

                                                                       :

                                                                      (from sml in _context.QcSamples
                                                                       join rrsp in _context.TransactionRelRoomSamplingPoint
                                                                       on sml.SamplingPointId equals rrsp.SamplingPoinId
                                                                       join room_purp in _context.TransactionRoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                                       where sml.TestParamId == tp.Id
                                                                       && sml.QcSamplingId == result.SamplingId
                                                                       && sml.RowStatus == null
                                                                       && sml.ParentId == null
                                                                       && room_purp.RoomId == rr.RoomId
                                                                       select sml).Union(from sml in _context.QcSamples
                                                                                         join rst in _context.TransactionRelSamplingTool on sml.SamplingPointId equals rst.SamplingPoinId
                                                                                         join tps in _context.TransactionToolPurpose on rst.ToolPurposeId equals tps.Id
                                                                                         join t in _context.TransactionTool on tps.ToolId equals t.Id
                                                                                         where sml.TestParamId == tp.Id
                                                                                         && sml.QcSamplingId == result.SamplingId
                                                                                         && sml.RowStatus == null
                                                                                         && sml.ParentId == null
                                                                                         && t.RoomId == rr.RoomId
                                                                                         select sml).Count(),
                                                                  }).ToList(),
                                       }).ToList();
            }

            return result;
        }

        public async Task<QcShipmentLateDetailViewModel> GetTransferLateByRequestQcsId(int requestQcsId)
        {
            var result = await (from r in _context.RequestQcs
                                join sm in _context.QcSamplings on r.Id equals sm.RequestQcsId
                                join sh in _context.QcSamplingShipments on sm.Id equals sh.QcSamplingId
                                where r.Id == requestQcsId
                                && sm.RowStatus == null
                                && sh.IsLateTransfer == true
                                group new { sm, r, sh } by new { r.Id } into g
                                select new QcShipmentLateDetailViewModel
                                {
                                    QcRequestId = g.Max(x => x.r.Id),
                                    QcSamplingId = g.Max(x => x.sm.Id),
                                    QcShipmentId = g.Max(x => x.sh.Id),
                                    NoRequest = g.Max(x => x.r.NoRequest),
                                    NoBatch = g.Max(x => x.r.NoBatch),
                                    QrSamplingCode = g.Max(x => x.sm.Code),
                                    QrShipmentCode = g.Max(x => x.sh.QrCode),
                                    ItemId = g.Max(x => x.r.ItemId),
                                    ItemName = g.Max(x => x.r.ItemName),
                                    TypeRequestId = g.Max(x => x.r.TypeRequestId),
                                    TypeRequest = g.Max(x => x.r.TypeRequest),
                                    SamplingTypeId = g.Max(x => x.sm.SamplingTypeId),
                                    SamplingTypeName = g.Max(x => x.sm.SamplingTypeName),
                                    EmPhaseId = g.Max(x => x.r.EmPhaseId),
                                    EmPhaseName = g.Max(x => x.r.EmPhaseName),
                                    ShipmentStartDate = g.Max(x => x.sh.StartDate),
                                    ShipmentEndDate = g.Max(x => x.sh.EndDate),
                                    FromOrganizationId = g.Max(x => x.sh.FromOrganizationId),
                                    FromOrganizationName = g.Max(x => x.sh.FromOrganizationName),
                                    ToOrganizationId = g.Max(x => x.sh.ToOrganizationId),
                                    ToOrganizationName = g.Max(x => x.sh.ToOrganizationName),
                                    Status = g.Max(x => x.sh.Status),
                                    OrgId = g.Max(x => x.r.OrgId),
                                    ShipmentNote = g.Max(x => x.sm.ShipmentNote),
                                    ShipmentApprovalDate = g.Max(x => x.sm.ShipmentApprovalDate),
                                    ShipmentApprovalBy = g.Max(x => x.sm.ShipmentApprovalBy)
                                }).FirstOrDefaultAsync();

            if (result != null)
            {
                /* Get Last sampling time */
                result.LastSamplingDateTime = (from samp in _context.QcSamples
                                               where samp.QcSamplingId == result.QcSamplingId
                                               && samp.RowStatus == null
                                               select samp.SamplingDateTimeTo).DefaultIfEmpty().Max(x => x == null ? null : x);

                /* Get List Concat Test Param */
                var ListTFTestParam = (from shtp in _context.QcSamplingShipments
                                       where shtp.QcSamplingId == result.QcSamplingId
                                       && shtp.RowStatus == null
                                       select shtp.TestParamName).ToList();

                if (ListTFTestParam.Any())
                {
                    result.TestParameterSamplings = String.Join(", ", ListTFTestParam);
                }

                /* Request Room and Count Parameters */
                result.RequestRooms = (from rr in _context.RequestRooms
                                       where rr.QcRequestId == result.QcRequestId
                                       select new RequestRoomRelationViewModel
                                       {
                                           Id = rr.Id,
                                           RoomId = rr.RoomId,
                                           RoomCode = rr.RoomCode,
                                           RoomName = rr.RoomName,
                                           GradeRoomId = rr.GradeRoomId,
                                           GradeRoomCode = rr.GradeRoomCode,
                                           GradeRoomName = rr.GradeRoomName,

                                           // Get total per Test Parameter    
                                           TestParametersCount = (from tp in _context.TransactionTestParameter
                                                                  where tp.Id != ApplicationConstant.TEST_PARAMETER_PC05
                                                                  && tp.Id != ApplicationConstant.TEST_PARAMETER_PC50
                                                                  select new TestParameterRelationViewModel
                                                                  {
                                                                      Id = tp.Id,
                                                                      Name = tp.Name,
                                                                      ShortName = tp.ShortName,
                                                                      Count = tp.Id == ApplicationConstant.TEST_PARAMETER_FD ?

                                                                      (from sml in _context.QcSamples
                                                                       where sml.QcSamplingId == result.QcSamplingId
                                                                       && sml.TestParamId == ApplicationConstant.TEST_PARAMETER_FD
                                                                       group sml by sml.PersonalInitial into g
                                                                       select g.Max(x => x.Id)).Count()

                                                                       :

                                                                      (from sml in _context.QcSamples
                                                                       join rrsp in _context.TransactionRelRoomSamplingPoint
                                                                       on sml.SamplingPointId equals rrsp.SamplingPoinId
                                                                       join room_purp in _context.TransactionRoomPurpose on rrsp.RoomPurposeId equals room_purp.Id
                                                                       where sml.TestParamId == tp.Id
                                                                       && sml.RowStatus == null
                                                                       && sml.ParentId == null
                                                                       && room_purp.RoomId == rr.RoomId
                                                                       select sml).Union(from sml in _context.QcSamples
                                                                                         join rst in _context.TransactionRelSamplingTool on sml.SamplingPointId equals rst.SamplingPoinId
                                                                                         join tps in _context.TransactionToolPurpose on rst.ToolPurposeId equals tps.Id
                                                                                         join t in _context.TransactionTool on tps.ToolId equals t.Id
                                                                                         where sml.TestParamId == tp.Id
                                                                                         && sml.QcSamplingId == result.QcSamplingId
                                                                                         && sml.RowStatus == null
                                                                                         && sml.ParentId == null
                                                                                         && t.RoomId == rr.RoomId
                                                                                         select sml).Count(),

                                                                  }).ToList(),
                                       }).ToList();

                /* Shipment Tracker */
                result.ShipmentTrackers = (from st in _context.QcSamplingShipmentTrackers
                                           where st.QcSamplingShipmentId == result.QcShipmentId
                                           && st.RowStatus == null
                                           select new QcSamplingShipmentTranckerViewModel
                                           {
                                               Id = st.Id,
                                               QrCode = st.QrCode,
                                               Type = st.Type,
                                               processAt = (DateTime)st.processAt,
                                               UserNik = st.UserNik,
                                               UserName = st.UserName,
                                               OrganizationId = st.OrganizationId,
                                               OrganizationName = st.OrganizationName,
                                               CreatedAt = st.CreatedAt
                                           }).OrderByDescending(x => x.CreatedAt).ToList();
            }

            return result;
        }
    }
}
