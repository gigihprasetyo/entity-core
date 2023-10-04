using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public class InsertQcTestBindingModel
    {
        [Required]
        public Int32 QcProcessId { get; set; }
        [Required]
        public string QcProcessName { get; set; }
        // [Required]
        public string PersonelNik { get; set; }
        // [Required]
        public string PersonelName { get; set; }
        // [Required]
        public string PersonelPairingNik { get; set; }
        // [Required]
        public string PersonelPairingName { get; set; }
        [Required]
        public DateTime TestDate { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public bool IsSubmit { get; set; }
        public List<QcProcessSample> QcProcessSample { get; set; }
        public List<QcProcessSamplingBatch> QcProcessSamplingBatch { get; set; }
    }

    public partial class QcProcessSample
    {
        public Int32 QcSampleId { get; set; }
    }

    public partial class QcProcessSamplingBatch
    {
        public Int32 QcSamplingId { get; set; }
    }
}
