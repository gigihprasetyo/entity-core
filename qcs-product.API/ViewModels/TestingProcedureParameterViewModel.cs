using qcs_product.API.Models;
using System.Collections.Generic;

namespace qcs_product.API.ViewModels
{
    public partial class TestingProcedureParameterViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int DeviationLevel { get; set; }
        public string DeviationNumber { get; set; }
        public string DeviationNote { get; set; }
        public string InvestigationResult { get; set; }
        public List<TransactionTestingProcedureParameterAttachment> exceptionParameter { get; set; }
    }
}
