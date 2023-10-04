using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcTransactionGroupViewModel
    {
        public Int32 Id { get; set; }
        public string Code { get; set; }
        public Int32? QcProcessId { get; set; }
        public string QcProcessName { get; set; }
        public DateTime TestDate { get; set; }
        public string BatchNumber { get; set; }
        public int? ProductPhaseId { get; set; }
        public string ProductPhaseName { get; set; }
        public string PersonelNik { get; set; }
        public string PersonelName { get; set; }
        public string PersonelPairingNik { get; set; }
        public string PersonelPairingName { get; set; }
        public int SampleCount { get; set; }
        public int Status { get; set; }
        public string StatusProses { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string WorkflowDocumentCode { get; set; }
    }
}
