using System;

namespace qcs_product.API.Models
{
    public partial class TransactionTestTypeMethod
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StandardProcedureNumber { get; set; }

        public int TestTypeId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public string RowStatus { get; set; }

        public string Code { get; set; }

    }
}
