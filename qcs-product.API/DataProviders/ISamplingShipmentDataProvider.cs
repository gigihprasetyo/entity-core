using qcs_product.API.BindingModels;
using qcs_product.API.Models;
using qcs_product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.DataProviders
{
    public interface ISamplingShipmentDataProvider
    {
        public Task<List<QcSamplingShipmentRelationViewModel>> List(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int fromOrgId, int toOrgId, int qcSamplingId);
        public Task<List<QcShipmentLateViewModel>> ListLate(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int OrgId, string nik);
        public Task<List<QcSamplingTransferViewModel>> ListTransfer(string search, int limit, int page, DateTime? startDate, DateTime? endDate, int status);
        public Task<QcShipmentLateViewModel> GetTransferLateBySamplingId(Int32 qcSamplingId, string nik);
        public Task<QcSamplingShipmentRelationViewModel> GetById(Int32 id);
        public Task<QcSamplingTransferViewModel> GetTransferDetail(Int32 id);
        public Task<List<QcSamplingShipment>> GetShipmentHeaderByQRCode(string QRCode);
        public Task<List<QcSamplingShipmentRelationViewModel>> GetByQRCode(string QRCode);
        public Task<List<QcSamplingShipment>> GetBySamplingId(Int32 qcSamplingId);

        public Task<QcSampling> UpdateApprovalSampling(InsertApprovalShipmentBindingModel data);
        public Task<QcSamplingShipment> InsertNewShipment(QcSamplingShipment data, QcSamplingShipmentTracker dataTracker, int status);
        public Task<List<QcSamplingShipmentRelationViewModelV2>> ListByBatch(string search, int limit, int page, DateTime? startDate, DateTime? endDate, List<int> status, int fromOrgId, int toOrgId);
        public Task<QcSamplingShipmentDetailRelationViewModel> GetByRequestQcsId(Int32 requestQcsId);
        public Task<QcShipmentLateDetailViewModel> GetTransferLateByRequestQcsId(Int32 requestQcsId);
    }
}
