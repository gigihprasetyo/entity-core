using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public partial class InsertRejectToDoBindingModel
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public string DigitalSignature { get; set; }
        public string NIK { get; set; }
        public int SamplingType { get; set; }
    }
}