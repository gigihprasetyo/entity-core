using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    [ExcludeFromCodeCoverage]
    public class QcSamplingShipment : BaseEntity
    {
        public Int32 QcSamplingId { get; set; }
        public string QrCode { get; set; }
        public string NoRequest { get; set; }
        public Int32 TestParamId { get; set; }
        public string TestParamName { get; set; }
        public Int32? FromOrganizationId { get; set; }
        public string FromOrganizationName { get; set; }
        public Int32? ToOrganizationId { get; set; }
        public string ToOrganizationName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsLateTransfer { get; set; }
        public int Status { get; set; }
        public string RowStatus { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public string UpdatedBy { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public virtual ICollection<QcSamplingShipmentTracker> QcSamplingShipmentTrackers { get; set; }

    }
}
