using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcRequestSamplingRelationViewModel
    {
        public Int32 SamplingId { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public int RequestId { get; set; }
        public string NoRequest { get; set; }
        public int? TypeRequestId { get; set; }
        public string TypeRequest { get; set; }
        public int? SamplingTypeId { get; set; }
        public string SamplingTypeName { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? TypeFormId { get; set; }
        public string TypeFormName { get; set; }
        public string NoBatch { get; set; }
        public DateTime? SamplingDateFrom { get; set; }
        public DateTime? SamplingDateTo { get; set; }

        public int? OrgId { get; set; }
        public int Status { get; set; }
        public string OrgName { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<int?> ListOrgByRequest { get; set; }
    }
}
