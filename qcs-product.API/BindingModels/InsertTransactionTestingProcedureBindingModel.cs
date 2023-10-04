using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.BindingModels
{
    [ExcludeFromCodeCoverage]
    public class InsertTransactionTestingProcedureBindingModel
    {
        public int ProcedureId { get; set; }
        [Required]
        public bool IsSubmit { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Nik { get; set; }
        public List<InsertTransactionTestingParameterBindingModel> ProcedureParameterValues { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public partial class InsertTransactionTestingParameterBindingModel
    {
        [Required]
        public int TestingProcedureParameterId { get; set; }
        public object PropertiesValue { get; set; }
    }
}
