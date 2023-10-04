using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class InsertReviewQaNoteQcSample
    {
        [Required]
        public string UpdatedBy { get; set; }
        public List<QcSampleReviewNoteInsert> Samples { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public partial class QcSampleReviewNoteInsert
    {
        [Required]
        public int SampleId { get; set; }
        public string Notes { get; set; }
    }
}
