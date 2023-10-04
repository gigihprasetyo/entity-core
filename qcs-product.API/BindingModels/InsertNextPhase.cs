using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class InsertNextPhase
    {
        public int SamplingId { get; set; }
        public string TypeSamplingId { get; set; }
        public string WorkflowCode { get; set; }
        public string PhaseWorkflow { get; set; }
        public string NIK { get; set; }
    }
}