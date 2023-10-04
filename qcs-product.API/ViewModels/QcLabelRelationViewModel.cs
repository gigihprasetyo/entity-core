using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.ViewModels
{
    public class QcLabelRelationViewModel
    {
        public QcLabelRequestViewModel RequestQc { get; set; }
        public QcLabelSamplingViewModel SamplingQc { get; set; }
        public QcLabelSampleViewModel SampleData { get; set; }
    }
}
