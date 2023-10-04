using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BusinessProviders
{
    public interface ISamplingShipmentBusinessProvider
    {
        public Task<ResponseViewModel<QcSamplingShipmentRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int fromOrgId, int toOrgId, int qcSamplingId);
        public Task<ResponseViewModel<QcShipmentLateViewModel>> ListLate(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int OrgId, string nik);
        public Task<ResponseViewModel<QcSamplingTransferViewModel>> ListTransfer(string search, int limit, int page, DateTime? startDate, DateTime? endDate, int status);

        public Task<ResponseOneDataViewModel<QcShipmentLateViewModel>> GetTransferLateBySamplingId(Int32 qcSamplingId, string nik);
        public Task<ResponseOneDataViewModel<QcSamplingTransferViewModel>> GetTransferDetail(Int32 sampleId);
        public Task<ResponseOneDataViewModel<QcSampling>> InsertApproval(InsertApprovalShipmentBindingModel data);
        public Task<ResponseOneDataViewModel<QcSamplingShipmentRelationViewModel>> GetById(Int32 id);
        public Task<ResponseOneDataViewModel<QcSamplingShipment>> InsertSending(InsertSamplingShipmentBindingModel data);
        public Task<ResponseOneDataViewModel<QcSamplingShipment>> InsertReceiving(InsertSamplingShipmentBindingModel data);
        public Task<ResponseViewModel<QcSamplingShipmentRelationViewModelV2>> ListByBatch(string search, int limit, int page, DateTime? startDate, DateTime? endDate, string status, int fromOrgId, int toOrgId);
        public Task<ResponseOneDataViewModel<QcSamplingShipmentDetailRelationViewModel>> GetByRequestQcsId(Int32 requestQcsId);
        public Task<ResponseOneDataViewModel<QcShipmentLateDetailViewModel>> GetTransferLateByRequestQcsId(Int32 requestQcsId);
    }
}
