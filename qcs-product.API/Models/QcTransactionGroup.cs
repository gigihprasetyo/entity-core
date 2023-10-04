using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcTransactionGroup : BaseEntity
    {
        public QcTransactionGroup()
        {
            WorkflowQcTransactionGroup = new HashSet<WorkflowQcTransactionGroup>();
        }
        public string Code { get; set; }
        public Int32 QcProcessId { get; set; }
        public string QcProcessName { get; set; }
        public DateTime TestDate { get; set; }
        public string PersonelNik { get; set; }
        public string PersonelName { get; set; }
        public string PersonelPairingNik { get; set; }
        public string PersonelPairingName { get; set; }
        public string WorkflowStatus { get; set; }
        public int Status { get; set; }
        public int StatusProses { get; set; }
        public string RowStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        [JsonIgnore]
        public virtual ICollection<WorkflowQcTransactionGroup> WorkflowQcTransactionGroup { get; set; }

        [JsonIgnore]
        public virtual ICollection<QcTransactionGroupProcess> TransactionGroupProcesses { get; set; }

        [JsonIgnore]
        public virtual ICollection<QcTransactionGroupSample> TransactionGroupSamples { get; set; }


        [JsonIgnore]
        public virtual ICollection<QcTransactionGroupSampling> TransactionGroupSamplings { get; set; }

    }
}
