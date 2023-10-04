using qcs_product.API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace production_execution_system.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class InsertParameterAttachmentViewModel
    {
        public List<TransactionTestingProcedureParameterAttachment> Attachments { get; set; }
        public List<TransactionHtrProcessProcedureParameterAttachment> AttachmentHistories { get; set; }
    }
}
