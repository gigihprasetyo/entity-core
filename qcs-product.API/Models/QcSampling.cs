using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcSampling : BaseEntity
    {
        public Int32 RequestQcsId { get; set; }
        public string Code { get; set; }
        public DateTime? SamplingDateFrom { get; set; }
        public DateTime? SamplingDateTo { get; set; }
        public Int32 SamplingTypeId { get; set; }
        public string SamplingTypeName { get; set; }
        public Int32 Status { get; set; }
        public string RowStatus { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public string UpdatedBy { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        public string WorkflowStatus { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string AttchmentFile { get; set; }
        public string Note { get; set; }
        public string ShipmentNote { get; set; }
        public DateTime? ShipmentApprovalDate { get; set; }
        public string ShipmentApprovalBy { get; set; }
        public DateTime? ProductDate { get; set; }
        public int? ProductMethodId { get; set; }
        public string ProductShipmentTemperature { get; set; }
        public DateTime? ProductShipmentDate { get; set; }
        public string ProductDataLogger { get; set; }
        public virtual ICollection<QcSamplingTools> Tools { get; set; }
        public virtual ICollection<QcSamplingMaterial> Materials { get; set; }
        public virtual ICollection<QcSamplingAttachment> Attachments { get; set; }
        public virtual ICollection<QcSample> Samples { get; set; }
    }
}
