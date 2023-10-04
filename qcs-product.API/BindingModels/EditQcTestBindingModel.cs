using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public class EditQcTestBindingModel
    {
        [Required]
        public Int32 Id { get; set; }
        [Required]
        public Int32 QcProcessId { get; set; }
        [Required]
        public string QcProcessName { get; set; }
        
        // [Required]
        public string PersonelPairingNik { get; set; }
        
        // [Required]
        public string PersonelPairingName { get; set; }
        
        [Required]
        public DateTime TestDate { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
        [Required]
        public bool IsSubmit { get; set; }
        public List<EditQcProcessSample> QcProcessSample { get; set; }
        public List<EditQcProcessSamplingBatch> QcProcessSamplingBatch { get; set; }
    }

    public partial class EditQcProcessSample
    {
        public Int32? Id { get; set; }
        public Int32 QcSampleId { get; set; }
    }

    public partial class EditQcProcessSamplingBatch
    {
        public Int32? Id { get; set; }
        public Int32 QcSamplingId { get; set; }
    }
}
