using System;
using System.ComponentModel.DataAnnotations;

namespace qcs_product.API.Models
{
    public class TransactionHtrProcessProcedureParameterAttachment
    {
        public int Id { get; set; }

        [Required]
        public int TestingProcedureParameterId { get; set; }

        [Required]
        public string Filename { get; set; }

        [Required]
        public string MediaLink { get; set; }

        [Required]
        public string Ext { get; set; }

        public string Note { get; set; }

        public string Action { get; set; }

        public string ExecutorName { get; set; }

        public string ExecutorPosition { get; set; }

        public string ExecutorNik { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        public string RowStatus { get; set; }
    }
}
