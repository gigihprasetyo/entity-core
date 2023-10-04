using Google.Cloud.PubSub.V1;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace qcs_product.API.Models
{
    public class TransactionTestingTypeMethodResultparameter : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public int Existing { get; set; }
        public Int32 InputTypeId { get; set; }
        public bool IsExisting { get; set; }
        public string MethodCode { get; set; }
        public string Name { get; set; }
        public bool NeedAttachment { get; set; }
        public int ParameterIdExisting { get; set; }

        [Column(TypeName = "jsonb")]
        public string Properties { get; set; }
        [Column(TypeName = "jsonb")]
        public string PropertiesValue { get; set; }

        public string RowStatus { get; set; }
        public Int32 Sequence { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public Int32 TestingId { get; set; }
        public Int32 TransactionTestingProcedureParameterId {  get; set; }
    }
}
