using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    [ExcludeFromCodeCoverage]
    public class QcSamplingShipmentTracker : BaseEntity
    {
        public Int32? QcSamplingShipmentId { get; set; }
        public string QrCode { get; set; }
        public string Type { get; set; }
        public DateTime? processAt { get; set; }
        public string IdLogger { get; set; }
        public string Temperature { get; set; }
        public string UserNik { get; set; }
        public string UserName { get; set; }
        public Int32? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string RowStatus { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
    }
}
