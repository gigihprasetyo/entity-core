using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcTransactionGroupDetailViewModel
    {
        public Int32 Id { get; set; }
        public string Code { get; set; }
        public Int32 QcProcessId { get; set; }
        public string QcProcessName { get; set; }
        public int? AddSampleLayoutType { get; set; }
        public DateTime TestDate { get; set; }
        public string PersonelNik { get; set; }
        public string PersonelName { get; set; }
        public string PersonelPairingNik { get; set; }
        public string PersonelPairingName { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string WorkflowCode { get; set; }
        public List<QcTransactionSampleRelationViewModel> SamplesData { get; set; }
        public List<QcTransactionSamplingRelationViewModel> SamplingBatchData { get; set; }
        public QcTransactionGroupProcessRelViewModel QcTransactionGroupProcess { get; set; }
        public List<QcResultTestDetailViewModel> QcResult { get; set; }


    }
}
