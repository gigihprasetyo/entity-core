using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Net.Mail;

namespace qcs_product.API.Models
{
    public class QcTransactionTemplateTestingTypeProcessProcedureParameter : BaseEntity
    {
        public string Attachment {  get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public int DeviationAttachment {  get; set; }
        public int DeviationLevel { get; set; }
        public string DeviationNote { get; set; }
        public bool HasAttachment { get; set; }
        public Int32 InputTypeId { get; set; }
        public bool IsNullable { get; set; }
        public string Name { get; set; }
        public Int32 TestTypeProcessProcedureId { get; set; }
        public string TestTypeProcessProcedureCode { get; set; }

        [Column(TypeName = "jsonb")]
        public string Properties { get; set; }
        [Column(TypeName = "jsonb")]
        public string PropertiesValue { get; set; }

        public string RowStatus { get; set; }
        public Int32 Sequence {  get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public Int32 TransactionTmpltTestTypeProcessProcedureId { get; set; }
        public string ComponentName { get; set; }

    }
}
