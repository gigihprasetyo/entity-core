using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public class EditSampleQcSampleChildBindingModel
    {
        public Int32? Id { get; set; }
        public Int32? SampleSequence { get; set; }
        public decimal? ParticleVolume { get; set; }
        public string AttchmentFile { get; set; }
        public DateTime? SamplingDateTimeFrom { get; set; }
        
        public DateTime? SamplingDateTimeTo { get; set; }
        public string Note { get; set; }
    }
}
