using System;

namespace qcs_product.API.Models
{
    public class QcTransactionTemplateTestingTypeMethod : BaseEntity
    {
      public string Code { get; set; }
      public DateTime CreatedAt { get; set; }
      public string RowStatus { get; set; }
      public string UpdatedBy { get; set; }
      public Int32 TestTypeId { get; set; }
      public string Name { get; set; }
      public string CreatedBy { get; set; }
      public string StandardProcedureNumber { get; set; }
      public DateTime UpdatedAt { get; set; }
      public string TestTypeCode { get; set; }
    }
}
