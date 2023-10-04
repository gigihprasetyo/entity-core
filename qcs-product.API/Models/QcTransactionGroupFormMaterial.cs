using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.Models
{
    public class QcTransactionGroupFormMaterial : BaseEntity
    {
        public Int32 QcTransactionGroupProcessId { get; set; }
        public int Sequence { get; set; }
        public Int32 ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal DefaultPackageQty { get; set; }
        public int UomPackage { get; set; }
        public decimal DefaultQty { get; set; }
        public int Uom { get; set; }
        public Int32 QcProcessId { get; set; }
        public Int32 QcTransactionGroupSectionId { get; set; }
        public string GroupName { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string RowStatus { get; set; }
    }
}
