using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class EditSampleQcBindingModel
    {
        [Required]
        public Int32 SamplingId { get; set; }
        [Required]
        public DateTime SamplingDateFrom { get; set; }
        [Required]
        public DateTime SamplingDateTo { get; set; }
        public string AttchmentFile { get; set; }
        public string Note { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
        public DateTime? ProductDate { get; set; }
        public int? ProductMethodId { get; set; }
        public string ProductShipmentTemperature { get; set; }
        public DateTime? ProductShipmentDate { get; set; }
        public string ProductDataLogger { get; set; }
        [Required]
        public bool IsSubmit { get; set; }
        public List<EditSampleQcSampleBindingModel> SampleData { get; set; }
        public List<EditSampleQcPersonelBindingModel> SamplingPersonels { get; set; }
        public EditBatchRequestQcBindingModel Batch { get; set; }

    }
}
